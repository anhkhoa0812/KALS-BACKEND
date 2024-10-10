using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class ProductRelationshipRepository: GenericRepository<ProductRelationship>, IProductRelationshipRepository
{
    public ProductRelationshipRepository(KitAndLabDbContext context) : base(context)
    {
    }
}