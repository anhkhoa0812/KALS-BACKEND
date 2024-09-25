namespace KALS.API.Models.ProductRelationship;

public class UpdateProductRelationshipRequest
{
    public List<Guid> ChildProductIds { get; set; }
}