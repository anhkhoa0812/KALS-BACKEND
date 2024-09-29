using KALS.API.Models;
using KALS.API.Models.User;

namespace KALS.API.Services.Interface;

public interface IUserService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    
}