using KALS.API.Constant;
using KALS.API.Models.User;
using KALS.API.Services.Interface;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = KALS.API.Models.User.LoginRequest;

namespace KALS.API.Controller;

[Route(ApiEndPointConstant.User.UserEndpoint)]
[ApiController]
public class UserController: BaseController<UserController>
{
    private readonly IUserService _userService;
    public UserController(ILogger<UserController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }
    
}