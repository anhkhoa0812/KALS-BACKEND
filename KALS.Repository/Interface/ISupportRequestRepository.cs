using KALS.Domain.Entities;

namespace KALS.Repository.Interface;

public interface ISupportRequestRepository: IGenericRepository<SupportRequest>
{
    public Task<ICollection<SupportRequest>> GetSupportRequestIsOpen(Guid memberId);
    
    public Task<SupportRequest> GetSupportRequestById(Guid id);
}