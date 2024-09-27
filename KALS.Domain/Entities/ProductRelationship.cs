using System.ComponentModel.DataAnnotations.Schema;
using KALS.Domain.Entity;

namespace KALS.Domain.Entities;

public class ProductRelationship
{
    public Guid ParentProductId { get; set; }
    [ForeignKey(nameof(ParentProductId))]
    public Product ParentProduct { get; set; }
    public Guid ChildProductId { get; set; }
    [ForeignKey(nameof(ChildProductId))]
    public Product ChildProduct { get; set; }
    
}