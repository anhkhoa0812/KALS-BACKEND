namespace KALS.API.Models.Product;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsKit { get; set; }
    public List<Guid>? ChildProductIds { get; set; }
    public List<Guid>? CategoryIds { get; set; }
}