using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KALS.Domain.Entity;

public class ProductRelationship
{
    public Guid ParentProductId { get; set; }
    [ForeignKey(nameof(ParentProductId))]
    public Product ParentProduct { get; set; }
    public Guid ChildProductId { get; set; }
    [ForeignKey(nameof(ChildProductId))]
    public Product ChildProduct { get; set; }
}