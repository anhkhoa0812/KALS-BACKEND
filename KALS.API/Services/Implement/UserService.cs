using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models;
using KALS.API.Models.User;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entity;
using KALS.Repository.Interface;
using BadHttpRequestException = Microsoft.AspNetCore.Http.BadHttpRequestException;

namespace KALS.API.Services.Implement;

public class UserService : BaseService<UserService>, IUserService
{
    public UserService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
            predicate: u => u.UserName == request.UserNameOrPhoneNumber || u.PhoneNumber == request.UserNameOrPhoneNumber);
        if (user == null) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);

        if(user.Password != PasswordUtil.HashPassword(request.Password)) throw new BadHttpRequestException(MessageConstant.User.PasswordIncorrect);
        var guidClaim = new Tuple<string, Guid>("userId", user.Id);
        var token = JwtUtil.GenerateJwtToken(user, guidClaim, _configuration);
        var refreshToken = JwtUtil.GenerateRefreshToken();
        var loginResponse = new LoginResponse()
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName, 
            Token = token,
            RefreshToken = refreshToken
        };
        return loginResponse;
    }
}