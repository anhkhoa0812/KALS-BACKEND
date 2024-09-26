using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Common;

namespace KALS.Domain.Entity;

public class Product: BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsHidden { get; set; }
    public string Type { get; set; }
    
    
    public virtual ICollection<ProductRelationship>? ParentProducts { get; set; } 
    public virtual ICollection<ProductRelationship>? ChildProducts { get; set; } 
    public virtual ICollection<LabProduct>? LabProducts { get; set; }
    
    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
}