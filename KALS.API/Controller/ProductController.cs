using KALS.API.Constant;
using KALS.API.Models.Lab;
using KALS.API.Models.Product;
using KALS.API.Models.ProductRelationship;
using KALS.API.Services.Interface;
using KALS.Domain.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace KALS.API.Controller;

[ApiController]
[Route(ApiEndPointConstant.Product.ProductEndpoint)]
public class ProductController : BaseController<ProductController>
{
    private readonly IProductService _productService;
    private readonly ILabService _labService;
    public ProductController(ILogger<ProductController> logger, IProductService productService, ILabService labService) : base(logger)
    {
        _productService = productService;
        _labService = labService;
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
    [HttpPatch(ApiEndPointConstant.Product.ProductById)]
    [ProducesResponseType(typeof(GetProductResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductById(Guid id, [FromBody] UpdateProductRequest request)
    {
        var response = await _productService.UpdateProductByIdAsync(id, request);
        if (response == null)
        {
            _logger.LogError($"Update product failed with {id}");
            return Problem($"{MessageConstant.Product.UpdateProductFail}: {id}");
        }
        _logger.LogInformation($"Update product successful with {id}");
        return Ok(response);
    }
    [HttpPatch(ApiEndPointConstant.Product.UpdateProductRelationship)]
    [ProducesResponseType(typeof(GetProductResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductRelationshipByProductId(Guid id, [FromBody] UpdateProductRelationshipRequest request)
    {
        var response = await _productService.UpdateProductRelationshipByProductIdAsync(id, request);
        if (response == null)
        {
            _logger.LogError($"Update product relationship failed with {id}");
            return Problem($"{MessageConstant.Product.UpdateProductRelationshipFail}: {id}");
        }
        _logger.LogInformation($"Update product relationship successful with {id}");
        return Ok(response);
    }
    [HttpPatch(ApiEndPointConstant.Product.LabToProduct)]
    [ProducesResponseType(typeof(GetProductResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignLabToProduct(Guid id, [FromBody] AssignLabsToProductRequest request)
    {
        var response = await _labService.AssignLabToProductAsync(id, request);
        if (response == null)
        {
            _logger.LogError($"Assign lab to product failed with {id}");
            return Problem($"{MessageConstant.Lab.AssignLabToProductFail}: {id}");
        }
        _logger.LogInformation($"Assign lab to product successful with {id}");
        return Ok(response);
    }

    [HttpGet(ApiEndPointConstant.Product.LabToProduct)]
    [ProducesResponseType(typeof(ProductWithLabResponse), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLabsByProductId(Guid id)
    {
        var response = await _labService.GetLabsByProductIdAsync(id);
        return Ok(response);
    }
}