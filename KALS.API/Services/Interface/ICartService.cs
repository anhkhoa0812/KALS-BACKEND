using KALS.API.Models.Cart;
using KALS.API.Models.Product;

namespace KALS.API.Services.Interface;

public interface ICartService
{
    Task<ICollection<CartModel>> AddToCartAsync(CartModel model);
    Task<ICollection<CartModel>> GetCartAsync();
    
    Task<ICollection<CartModel>> RemoveFromCartAsync(Guid productId);
}