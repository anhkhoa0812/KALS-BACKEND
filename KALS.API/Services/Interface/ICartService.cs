using KALS.API.Models.Cart;
using KALS.API.Models.Product;

namespace KALS.API.Services.Interface;

public interface ICartService
{
    Task<ICollection<CartModelResponse>> AddToCartAsync(CartModel model);
    Task<ICollection<CartModelResponse>> GetCartAsync();
    
    Task<ICollection<CartModelResponse>> RemoveFromCartAsync(Guid productId);
    
    Task<ICollection<CartModelResponse>> UpdateQuantityAsync(CartModel request);
    
    Task<ICollection<CartModelResponse>> ClearCartAsync();
}