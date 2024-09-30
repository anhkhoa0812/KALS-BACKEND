using AutoMapper;
using KALS.API.Models.User;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<User, LoginResponse>();
    }
}