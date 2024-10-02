using AutoMapper;
using KALS.API.Models.Momo;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace KALS.API.Services.Implement;

public class MomoService: BaseService<MomoService>, IMomoService
{
    public MomoService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<MomoService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public string CreatePayment(MomoRequest momoRequest)
    {
        string endpoint = _configuration["MomoAPI:momo_Api"];
        string requestType = "captureWallet";

        string rawHash = "accessKey=" + _configuration["MomoAPI:accessKey"] +
                         "&amount=" + momoRequest.amount +
                         "&extraData=" + momoRequest.extraData +
                         "&ipnUrl=" + _configuration["MomoAPI:ipnUrl"] +
                         "&orderId=" + momoRequest.orderId +
                         "&orderInfo=" + momoRequest.orderInfo +
                         "&partnerCode=" + _configuration["MomoAPI:partnerCode"] +
                         //"&redirectUrl=" + redirectUrl +
                         "&redirectUrl=" + _configuration["MomoAPI:redirectUrl"] +
                         "&requestId=" + momoRequest.requestId +
                         "&requestType=" + requestType;
        string signature = MomoUtil.signSHA256(rawHash, _configuration["MomoAPI:secretKey"]);
        
        JObject message = new JObject
        {
            { "partnerCode", _configuration["MomoAPI:partnerCode"] },
            { "partnerName", "Test" },
            { "storeId", "MomoTestStore" },
            { "requestId", momoRequest.requestId },
            { "amount", momoRequest.amount },
            { "orderId", momoRequest.orderId },
            { "orderInfo", momoRequest.orderInfo },
            { "redirectUrl", momoRequest.ReturnUrl },
            //{ "redirectUrl", redirectUrl },
            { "ipnUrl", _configuration["MomoAPI:ipnUrl"] },
            { "lang", "en" },
            { "extraData", momoRequest.extraData },
            { "requestType", requestType },
            { "signature", signature }
        };

        string responseFromMomo = MomoUtil.sendPaymentRequest(endpoint, message.ToString());
        JObject jmessage = JObject.Parse(responseFromMomo);
        if (jmessage.GetValue("payUrl").IsNullOrEmpty())
        {
            return jmessage.GetValue("payUrl").ToString();
        }
        else
        {
            return jmessage.GetValue("message").ToString();
        }
    }
}