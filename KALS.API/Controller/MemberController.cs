using KALS.API.Constant;
using KALS.API.Models.Filter;
using KALS.API.Models.User;
using KALS.API.Services.Interface;
using KALS.Domain.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;

[ApiController]
[Route(ApiEndPointConstant.Member.MemberEndpoint)]
public class MemberController: BaseController<MemberController>
{
    private readonly IUserService _userService;
    public MemberController(ILogger<MemberController> logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }
    
    [HttpGet(ApiEndPointConstant.Member.MemberEndpoint)]
    [ProducesResponseType(typeof(IPaginate<MemberResponse>), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembersAsync([FromQuery] int page = 1, [FromQuery] int size = 30, 
        [FromQuery] MemberFilter filter = null, [FromQuery] string sortBy = null, [FromQuery] bool isAsc = true)
    {
        var members = await _userService.GetMembersAsync(page, size, filter, sortBy, isAsc);
        return Ok(members);
    }
    [HttpPatch(ApiEndPointConstant.Member.MemberById)]
    [ProducesResponseType(typeof(MemberResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMemberAsync(Guid id, [FromBody] UpdateMemberRequest request)
    {
        var member = await _userService.UpdateMemberAsync(id, request);
        if (member == null)
        {
            _logger.LogError($"Create new member failed with {request.Username}");
            return Problem($"{MessageConstant.User.UpdateMemberFail}: {request.Username}");
        }
        _logger.LogInformation($"Create new member successful with {request.Username}");
        return Ok(member);
    }
}