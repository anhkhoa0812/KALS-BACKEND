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
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendOtp([FromBody] GenerateOtpRequest request)
    {
        var result = await _userService.GenerateOtpAsync(request);
        if (result == null)
        {
            return Problem(MessageConstant.Sms.SendSmsFailed);
        }

        return Ok(result);
    }
    [HttpPost(ApiEndPointConstant.Auth.Signup)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Signup([FromBody] RegisterRequest registerRequest)
    {
        var loginResponse = await _userService.RegisterAsync(registerRequest);
        if (loginResponse == null)
        {
            _logger.LogError($"Sign up failed with {registerRequest.Username}");
            return Problem(MessageConstant.User.RegisterFail);
        }
        _logger.LogInformation($"Sign up successful with {registerRequest.Username}");
        return CreatedAtAction(nameof(Signup), loginResponse);
    }
    [HttpPost(ApiEndPointConstant.Auth.Login)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginResponse = await _userService.LoginAsync(loginRequest);
        if (loginResponse == null)
        {
            _logger.LogError($"Login failed with {loginRequest.UsernameOrPhoneNumber}");
            return Problem(MessageConstant.User.LoginFail);
        }
        _logger.LogInformation($"Login successful with {loginRequest.UsernameOrPhoneNumber}");
        return Ok(loginResponse);
    }
    
}