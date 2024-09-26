using System.ComponentModel.DataAnnotations.Schema;

namespace KALS.Domain.Entity;

public class ProductCategory
{
    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
}