using KALS.API.Models.Product;
using KALS.Domain.Paginate;

namespace KALS.API.Services.Interface;

public interface IProductService
{
    Task<IPaginate<GetProductResponse>> GetAllProductPagingAsync(int page, int size, Guid? categoryId);
    Task<GetProductDetailResponse> GetProductByIdAsync(Guid id);
    Task<GetProductResponse> CreateProductAsync(CreateProductRequest request);
    
}