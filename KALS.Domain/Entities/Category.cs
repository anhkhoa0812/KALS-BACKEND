using System.Collections;
using System.ComponentModel.DataAnnotations;
using KALS.Domain.Common;

namespace KALS.Domain.Entities;

public class Category : BaseEntity 
{
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
}