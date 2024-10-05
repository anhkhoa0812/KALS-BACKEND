using KALS.API.Constant;
using KALS.API.Models.Filter;
using KALS.API.Models.Order;
using KALS.API.Services.Implement;
using KALS.API.Services.Interface;
using KALS.Domain.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;

[ApiController]
[Route(ApiEndPointConstant.Order.OrderEndpoint)]
public class OrderController: BaseController<OrderController>
{
    private readonly IOrderService _orderService;
    public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
    {
        _orderService = orderService;
    }
    
    [HttpGet(ApiEndPointConstant.Order.OrderEndpoint)]
    [ProducesResponseType(typeof(IPaginate<OrderResponse>), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderList([FromQuery] int page = 1, [FromQuery] int size = 30, 
        [FromQuery] OrderFilter? filter = null, [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = true)
    {
        var result = await _orderService.GetOrderList(page, size, filter, sortBy, isAsc);
        return Ok(result);
    }
}