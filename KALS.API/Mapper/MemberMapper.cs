using AutoMapper;
using KALS.API.Models.User;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class MemberMapper: Profile
{
    public MemberMapper()
    {
        CreateMap<Member, MemberResponse>();
    }
}