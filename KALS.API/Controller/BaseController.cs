using KALS.API.Constant;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;

[Route(ApiEndPointConstant.RootEndPoint)]
[ApiController]
public class BaseController<T> : ControllerBase where T : BaseController<T>
{
    protected ILogger<T> _logger;
    
    public BaseController(ILogger<T> logger)
    {
        _logger = logger;
    }
}