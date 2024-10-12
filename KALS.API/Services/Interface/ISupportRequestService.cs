using KALS.API.Models.SupportRequest;

namespace KALS.API.Services.Interface;

public interface ISupportRequestService
{
    Task<SupportRequestResponse> CreateSupportRequest(SupportRequest request);
    
    Task<SupportRequestResponse> ResponseSupportMessage( Guid supportRequestId, ResponseSupportRequest request);
}