using System.Security.Claims;
using AutoMapper;
using KALS.Domain.DataAccess;
using KALS.Domain.Enums;
using KALS.Repository.Interface;

namespace KALS.API.Services;

public class BaseService<T> where T : class
{
    protected IUnitOfWork<KitAndLabDbContext> _unitOfWork;
    protected ILogger<T> _logger;
    protected IMapper _mapper;
    protected IHttpContextAccessor _httpContextAccessor;
    protected IConfiguration _configuration;
    
    public BaseService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<T> logger, IMapper mapper,
        IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }
    protected string GetRoleFromJwt()
    {
        string role = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
        return role;
    }

    protected Guid GetUserIdFromJwt()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
        if (userIdClaim != null)
        {
            return Guid.Parse(userIdClaim.Value);
        }
        return Guid.Empty;
    }
}