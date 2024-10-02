using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models;
using KALS.API.Models.Sms;
using KALS.API.Models.User;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Enums;
using KALS.Repository.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;
using BadHttpRequestException = Microsoft.AspNetCore.Http.BadHttpRequestException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace KALS.API.Services.Implement;

public class UserService : BaseService<UserService>, IUserService
{
    public UserService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
            predicate: u => u.Username == request.UsernameOrPhoneNumber || u.PhoneNumber == request.UsernameOrPhoneNumber);
        if (user == null) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);

        if(user.Password != PasswordUtil.HashPassword(request.Password)) throw new BadHttpRequestException(MessageConstant.User.PasswordIncorrect);
        var guidClaim = new Tuple<string, Guid>("userId", user.Id);
        var token = JwtUtil.GenerateJwtToken(user, guidClaim, _configuration);
        var refreshToken = JwtUtil.GenerateRefreshToken();
        var loginResponse = new LoginResponse()
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName, 
            Token = token,
            RefreshToken = refreshToken
        };
        return loginResponse;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
    {
        var userList = await _unitOfWork.GetRepository<User>().GetListAsync();
        if (userList.Any(u => u.Username == request.Username)) throw new BadHttpRequestException(MessageConstant.User.UserNameExisted);
        if (userList.Any(u => u.PhoneNumber == request.PhoneNumber)) throw new BadHttpRequestException(MessageConstant.User.PhoneNumberExisted);
        var user = _mapper.Map<User>(request);
        var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        var db = redis.GetDatabase();
        var key = request.PhoneNumber;
        var existingOtp = db.StringGet(key);
        
        if (existingOtp.IsNullOrEmpty) throw new BadHttpRequestException(MessageConstant.Sms.OtpNotFound);
        if (existingOtp != request.Otp) throw new BadHttpRequestException(MessageConstant.Sms.OtpIncorrect);
        
        user.Password = PasswordUtil.HashPassword(request.Password);
        user.Role = RoleEnum.Member;
        await _unitOfWork.GetRepository<User>().InsertAsync(user);

        var member = new Member()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id
        };
        await _unitOfWork.GetRepository<Member>().InsertAsync(member);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        LoginResponse response = null;
        if (isSuccess) response = _mapper.Map<LoginResponse>(user);
        response.Token = JwtUtil.GenerateJwtToken(user, new Tuple<string, Guid>("userId", user.Id), _configuration);
        response.RefreshToken = JwtUtil.GenerateRefreshToken();
        return response;
    }

    public async Task<string> GenerateOtpAsync(GenerateOtpRequest request)
    {
        var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        var db = redis.GetDatabase();
        var key = request.PhoneNumber;
        
        var existingOtp = await db.StringGetAsync(key);
        if (!string.IsNullOrEmpty(existingOtp)) throw new BadHttpRequestException(MessageConstant.Sms.OtpAlreadySent);
        
        if(request.PhoneNumber == null) throw new BadHttpRequestException(MessageConstant.User.PhoneNumberNotFound);
        var phoneNumberArray = new string[] { request.PhoneNumber };
        var otp = OtpUtil.GenerateOtp();
        var content = "Mã OTP của bạn là: " + otp;
        var response = SmsUtil.sendSMS(phoneNumberArray, content, _configuration);
        _logger.LogInformation(response);
        var smsResponse = JsonSerializer.Deserialize<SmsModel.SmsResponse>(response);
        if (smsResponse.status != "success" && smsResponse.code != "00")
        {
            throw new BadHttpRequestException(MessageConstant.Sms.SendSmsFailed);
        }
        
        await db.StringSetAsync(key, otp, TimeSpan.FromMinutes(2));
        return request.PhoneNumber;
    }
}