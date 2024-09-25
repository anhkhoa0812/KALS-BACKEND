using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Product;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entity;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KALS.API.Services.Implement;

public class ProductService: BaseService<ProductService>, IProductService
{
    public ProductService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<IPaginate<GetProductResponse>> GetAllProductPagingAsync(int page, int size, Guid? categoryId)
    {
        var products = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
            selector: p => new GetProductResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                IsHidden = p.IsHidden,
                Type = p.Type,
            },
            predicate: p => !p.IsHidden && (!categoryId.HasValue || p.CategoryId == categoryId),
            orderBy: p => p.OrderByDescending(p => p.CreatedAt),
            page: page,
            size: size
        );
        return products;
    }

    public Task<GetProductDetailResponse> GetProductByIdAsync(Guid id)
    {
        if(id == Guid.Empty) throw new BadHttpRequestException("Id is invalid");
        var product = _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            selector: p => new GetProductDetailResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                IsHidden = p.IsHidden,
                Type = p.Type,
                ChildProducts = p.ChildProducts.Select(cp => new GetProductResponse()
                {
                    Id = cp.ChildProduct.Id,
                    Name = cp.ChildProduct.Name,
                    Description = cp.ChildProduct.Description,
                    Price = cp.ChildProduct.Price,
                    Quantity = cp.ChildProduct.Quantity,
                    CreatedAt = cp.ChildProduct.CreatedAt,
                    ModifiedAt = cp.ChildProduct.ModifiedAt,
                    IsHidden = cp.ChildProduct.IsHidden,
                    Type = cp.ChildProduct.Type,
                }).ToList(),
            },
            predicate: p => p.Id == id,
            include: p => p.Include(p => p.ChildProducts)
                .ThenInclude(cp => cp.ChildProduct)
        );
        return product;
    }

    public async Task<GetProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = _mapper.Map<Product>(request);
        product.Id = Guid.NewGuid();
        product.CreatedAt = TimeUtil.GetCurrentSEATime();
        product.ModifiedAt = TimeUtil.GetCurrentSEATime();

        foreach (var childProductId in request.ChildProductIds)
        {
            var childProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: p => p.Id == childProductId
                );
            if (childProduct == null) throw new BadHttpRequestException(MessageConstant.Product.ChildProductNotFound);
            product.ChildProducts.Add(new ProductRelationship()
            {
                ParentProductId = product.Id,
                ChildProductId = childProductId
            });
        }
        await _unitOfWork.GetRepository<Product>().InsertAsync(product);
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        GetProductResponse productResponse = null;
        if (isSuccess) productResponse = _mapper.Map<GetProductResponse>(product);
        return productResponse;
    }
}