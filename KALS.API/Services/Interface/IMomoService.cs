using KALS.API.Models.Momo;

namespace KALS.API.Services.Interface;

public interface IMomoService
{
    string CreatePayment(MomoRequest momoRequest);
}