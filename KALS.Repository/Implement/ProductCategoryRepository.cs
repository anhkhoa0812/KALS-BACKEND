using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class ProductCategoryRepository: GenericRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(KitAndLabDbContext context) : base(context)
    {
    }
    
}