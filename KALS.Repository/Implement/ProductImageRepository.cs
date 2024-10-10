using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class ProductImageRepository: GenericRepository<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(KitAndLabDbContext context) : base(context)
    {
    }
}