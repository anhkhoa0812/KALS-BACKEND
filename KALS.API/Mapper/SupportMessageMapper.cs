using AutoMapper;
using KALS.API.Models.SupportRequest;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class SupportMessageMapper: Profile
{
    public SupportMessageMapper()
    {
        CreateMap<SupportMessage, SupportMessageResponse>();
    }
}