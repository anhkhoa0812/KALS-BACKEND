using KALS.API.Constant;
using KALS.API.Models.User;
using KALS.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;

[ApiController]
[Route(ApiEndPointConstant.ApiEndpoint)]
public class AuthController: BaseController<AuthController>
{
    private readonly IUserService _userService;
    public AuthController(ILogger<AuthController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }
    
    [HttpPost(ApiEndPointConstant.Auth.SendOtp)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendOtp([FromBody] string phoneNumber)
    {
        var result = await _userService.GenerateOtpAsync(phoneNumber);
        if (result == null)
        {
            return Problem(MessageConstant.Sms.SendSmsFailed);
        }

        return NoContent();
    }
    [HttpPost(ApiEndPointConstant.Auth.Signup)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Signup([FromBody] RegisterRequest registerRequest)
    {
        var loginResponse = await _userService.RegisterAsync(registerRequest);
        if (loginResponse == null)
        {
            _logger.LogError($"Sign up failed with {registerRequest.UserName}");
            return Problem(MessageConstant.User.RegisterFail);
        }
        _logger.LogInformation($"Sign up successful with {registerRequest.UserName}");
        return Ok(loginResponse);
    }
    [HttpPost(ApiEndPointConstant.Auth.Login)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginResponse = await _userService.LoginAsync(loginRequest);
        if (loginResponse == null)
        {
            _logger.LogError($"Login failed with {loginRequest.UserNameOrPhoneNumber}");
            return Problem(MessageConstant.User.LoginFail);
        }
        _logger.LogInformation($"Login successful with {loginRequest.UserNameOrPhoneNumber}");
        return Ok(loginResponse);
    }
    
}