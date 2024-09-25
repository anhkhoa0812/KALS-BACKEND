using KALS.API.Constant;
using KALS.API.Models.Product;
using KALS.API.Services.Interface;
using KALS.Domain.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;

[ApiController]
[Route(ApiEndPointConstant.Product.ProductEndpoint)]
public class ProductController : BaseController<ProductController>
{
    private readonly IProductService _productService;
    public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
    {
        _productService = productService;
    }
    [HttpGet(ApiEndPointConstant.Product.ProductEndpoint)]
    [ProducesResponseType(typeof(IPaginate<GetProductResponse>), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProduct(int page = 1, int size = 30, Guid? categoryId = null)
    {
        var response = await _productService.GetAllProductPagingAsync(page, size, categoryId);
        return Ok(response);
    }
    [HttpGet(ApiEndPointConstant.Product.ProductById)]
    [ProducesResponseType(typeof(GetProductResponse), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var response = await _productService.GetProductByIdAsync(id);
        return Ok(response);
    }
    [HttpPost(ApiEndPointConstant.Product.ProductEndpoint)]
    [ProducesResponseType(typeof(GetProductResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var response = await _productService.CreateProductAsync(request);
        if (response == null)
        {
            _logger.LogError($"Create new product failed with {request.Name}");
            return Problem($"{MessageConstant.Product.CreateProductFail}: {request.Name}");
        }
        _logger.LogInformation($"Create new product successful with {request.Name}");
        return CreatedAtAction(nameof(CreateProduct), response);
    }
    
    
}