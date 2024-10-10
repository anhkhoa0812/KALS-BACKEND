using KALS.Domain.Entities;

namespace KALS.Repository.Interface;

public interface ILabMemberRepository: IGenericRepository<LabMember>
{
    Task<LabMember> GetLabMemberByLabIdAndMemberId(Guid labId, Guid memberId);
}