using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class LabProductRepository: GenericRepository<LabProduct>, ILabProductRepository
{
    public LabProductRepository(KitAndLabDbContext context) : base(context)
    {
    }
}