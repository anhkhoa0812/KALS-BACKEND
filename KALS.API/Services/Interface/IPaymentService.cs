using KALS.API.Models.Cart;

namespace KALS.API.Services.Interface;

public interface IPaymentService
{
    Task<string> CheckOut(ICollection<CartModelResponse> request);
}