using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class SupportMessageRepository: GenericRepository<SupportMessage>, ISupportMessageRepository
{
    public SupportMessageRepository(KitAndLabDbContext context) : base(context)
    {
    }
}