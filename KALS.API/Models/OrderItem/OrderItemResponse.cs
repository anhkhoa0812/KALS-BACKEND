using KALS.API.Models.Product;

namespace KALS.API.Models.OrderItem;

public class OrderItemResponse
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public GetProductResponse Product { get; set; }
}