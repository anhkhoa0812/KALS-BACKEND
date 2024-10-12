using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Enums;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class SupportRequestRepository: GenericRepository<SupportRequest>, ISupportRequestRepository
{
    public SupportRequestRepository(KitAndLabDbContext context) : base(context)
    {
    }

    public async Task<ICollection<SupportRequest>> GetSupportRequestIsOpen(Guid memberId)
    {
        var supportRequests = await GetListAsync(
            predicate: sr => sr.MemberId == memberId && sr.Status == SupportRequestStatus.Open
        );
        return supportRequests;
    }

    public async Task<SupportRequest> GetSupportRequestById(Guid id)
    {
        var supportRequest = await SingleOrDefaultAsync(
            predicate: sr => sr.Id == id
        );
        return supportRequest;
    }
}