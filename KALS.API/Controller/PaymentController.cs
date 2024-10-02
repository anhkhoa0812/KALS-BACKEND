using KALS.API.Constant;
using KALS.API.Models.Cart;
using KALS.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;
[ApiController]
[Route(ApiEndPointConstant.Payment.PaymentEndPoint)]
public class PaymentController: BaseController<PaymentController>
{
    private readonly IPaymentService _paymentService;
    public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService) : base(logger)
    {
        _paymentService = paymentService;
    }
    [HttpPost("/checkout")] 
    public async Task<IActionResult> CheckOut(ICollection<CartModelResponse> request)
    {
        
            var result = await _paymentService.CheckOut(request);
            return Ok(result);
        
        
    }
    
}