using AutoMapper;
using KALS.API.Models.SupportRequest;
using SupportRequest = KALS.Domain.Entities.SupportRequest;

namespace KALS.API.Mapper;

public class SupportRequestMapper: Profile
{
    public SupportRequestMapper()
    {
        CreateMap<SupportRequest, SupportRequestResponse>()
            .ForMember(dest => dest.SupportMessages, 
                opt => opt.MapFrom(src => src.SupportMessages));
    }
}