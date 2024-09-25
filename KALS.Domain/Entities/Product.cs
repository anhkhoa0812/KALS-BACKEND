using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;

namespace KALS.Domain.Entity;

public class Product: BaseEntity
{
    public Product()
    {
        ParentProducts = new HashSet<ProductRelationship>();
        ChildProducts = new HashSet<ProductRelationship>();
        LabProducts = new HashSet<LabProduct>();
    }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsHidden { get; set; }
    public string Type { get; set; }
    
    public Guid? CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
    
    public virtual ICollection<ProductRelationship> ParentProducts { get; set; } 
    public virtual ICollection<ProductRelationship> ChildProducts { get; set; } 
    public virtual ICollection<LabProduct> LabProducts { get; set; }
}