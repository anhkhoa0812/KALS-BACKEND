using KALS.API.Models.Product;
using KALS.API.Models.ProductRelationship;
using KALS.Domain.Paginate;

namespace KALS.API.Services.Interface;

public interface IProductService
{
    Task<IPaginate<GetProductResponse>> GetAllProductPagingAsync(int page, int size, Guid? categoryId);
    Task<GetProductDetailResponse> GetProductByIdAsync(Guid id);
    Task<GetProductResponse> CreateProductAsync(CreateProductRequest request);
    
    Task<GetProductResponse> UpdateProductByIdAsync(Guid id, UpdateProductRequest request);
    Task<GetProductResponse> UpdateProductRelationshipByProductIdAsync(Guid parentId, UpdateProductRelationshipRequest request);
    
    
}