using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;

namespace KALS.Repository.Implement;

public class LabMemberRepository: GenericRepository<LabMember>, ILabMemberRepository
{
    public LabMemberRepository(KitAndLabDbContext context) : base(context)
    {
    }

    public async Task<LabMember> GetLabMemberByLabIdAndMemberId(Guid labId, Guid memberId)
    {
        var labMember = await SingleOrDefaultAsync(
            predicate: lm => lm.LabId == labId && lm.MemberId == memberId
        );
        return labMember;
    }
}