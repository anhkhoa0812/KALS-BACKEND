using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Category;
using KALS.API.Models.Product;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KALS.API.Services.Implement;

public class ProductService: BaseService<ProductService>, IProductService
{
    public ProductService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<IPaginate<GetProductWithCatogoriesResponse>> GetAllProductPagingAsync(int page, int size, ProductFilter filter, string? sortBy, bool isAsc)
    {
        var products = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
            selector: p => new GetProductWithCatogoriesResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                IsHidden = p.IsHidden,
                IsKit = p.IsKit,
                Categories = p.ProductCategories.Any(pc => pc.ProductId == p.Id) ? p.ProductCategories.Select(pc => new CategoryResponse()
                {
                    Id = pc.Category.Id,
                    Name = pc.Category.Name,
                    Description = pc.Category.Description,
                    CreatedAt = pc.Category.CreatedAt,
                    ModifiedAt = pc.Category.ModifiedAt,
                }).ToList() : null
            },
            predicate: p => !p.IsHidden,
            // orderBy: p => p.OrderByDescending(p => p.CreatedAt),
            page: page,
            size: size,
            include: p => p.Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category),
            filter: filter,
            sortBy: sortBy,
            isAsc: isAsc
        );
        return products;
    }

    public async Task<GetProductDetailResponse> GetProductByIdAsync(Guid id)
    {
        if(id == Guid.Empty) throw new BadHttpRequestException("Id is invalid");
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
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
                IsKit = p.IsKit,
                ChildProducts = p.ChildProducts.Select(pr => pr.ChildProduct).Select(cp => new GetProductResponse()
                {
                    Id = cp.Id,
                    Name = cp.Name,
                    Description = cp.Description,
                    Quantity = cp.Quantity,
                    Price = cp.Price,
                    IsHidden = cp.IsHidden,
                    IsKit = cp.IsKit,
                    CreatedAt = cp.CreatedAt,
                    ModifiedAt = cp.ModifiedAt
                }).ToList()
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
        if (request.ChildProductIds.Any())
        {
            foreach (var childProductId in request.ChildProductIds)
            {
                var requestedChildProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                    predicate: p => p.Id == childProductId
                );
                if (requestedChildProduct == null) throw new BadHttpRequestException(MessageConstant.Product.ChildProductNotFound);
                // product.ChildProducts.Add(new ProductRelationship()
                // {
                //     ParentProductId = product.Id,
                //     ChildProductId = childProductId
                // });
                await _unitOfWork.GetRepository<ProductRelationship>().InsertAsync(new ProductRelationship()
                {
                    ParentProductId = product.Id,
                    ChildProductId = childProductId
                });
            }
        }

        if (request.CategoryIds.Any())
        {
            foreach (var categoryId in request.CategoryIds)
            {
                var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    predicate: c => c.Id == categoryId
                );
                if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFound);
                // product.ProductCategories.Add(new ProductCategory()
                // {
                //     ProductId = product.Id,
                //     CategoryId = categoryId
                // });
                await _unitOfWork.GetRepository<ProductCategory>().InsertAsync(
                    new ProductCategory()
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId
                    }
                );
            }
        }
        await _unitOfWork.GetRepository<Product>().InsertAsync(product);
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        GetProductResponse productResponse = null;
        if (isSuccess) productResponse = _mapper.Map<GetProductResponse>(product);
        return productResponse;
    }

    public async Task<GetProductResponse> UpdateProductByIdAsync(Guid id, UpdateProductRequest request)
    {
        if(id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.ProductIdNotNull);
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == id
        );
        if(product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Quantity = request.Quantity;
        product.IsHidden = request.IsHidden;
        product.IsHidden = request.IsHidden;
        product.ModifiedAt = TimeUtil.GetCurrentSEATime();
        _unitOfWork.GetRepository<Product>().UpdateAsync(product);
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        GetProductResponse productResponse = null;
        if (isSuccess) productResponse = _mapper.Map<GetProductResponse>(product);
        return productResponse;
    }

    public async Task<GetProductResponse> UpdateProductRelationshipByProductIdAsync(Guid parentId, UpdateChildProductForKitRequest request)
    {
        if(parentId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.ProductIdNotNull);
        if (!request.ChildProductIds.Any())
            throw new BadHttpRequestException(MessageConstant.Product.ChildProductIdNotNull);
        var parentProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == parentId,
            include: p => p.Include(p => p.ChildProducts)
                .ThenInclude(pr => pr.ChildProduct)
        );
        if(parentProduct == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);

        var currentChildProductIds = parentProduct.ChildProducts.Select(pr => pr.ChildProductId).ToList();
        var addChildProductIds = request.ChildProductIds.Except(currentChildProductIds);
        var removeChildProductIds = currentChildProductIds.Except(request.ChildProductIds);

        if (addChildProductIds.Any())
        {
            foreach (var addChildProductId in addChildProductIds)
            {
                var addChildProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                    predicate: acp => acp.Id == addChildProductId
                );
                if (addChildProduct != null)
                {
                    await _unitOfWork.GetRepository<ProductRelationship>().InsertAsync(
                        new ProductRelationship()
                        {
                            ParentProductId = parentId,
                            ChildProductId = addChildProductId
                        }
                    );
                }
            }
        }

        if (removeChildProductIds.Any())
        {
            foreach (var removeChildProductId in removeChildProductIds)
            {
                var removeChildProduct = parentProduct.ChildProducts.FirstOrDefault(pr => pr.ChildProductId == removeChildProductId);
                if (removeChildProduct != null)
                {
                    _unitOfWork.GetRepository<ProductRelationship>().DeleteAsync(removeChildProduct);
                }
            }
        }

        // _unitOfWork.GetRepository<Product>().UpdateAsync(parentProduct);
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        GetProductResponse productResponse = null;
        if (isSuccess) productResponse = _mapper.Map<GetProductResponse>(parentProduct);
        return productResponse;
    }

    public async Task<ICollection<GetProductResponse>> GetChildProductsByParentIdAsync(Guid parentId)
    {
        if(parentId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.ParentProductIdNotNull);
        var childProducts = await _unitOfWork.GetRepository<Product>().GetListAsync(
            predicate: p => p.ChildProducts.Any(cp => cp.ParentProductId == parentId)
        );
        var childProductResponses = _mapper.Map<ICollection<GetProductResponse>>(childProducts);
        return childProductResponses;
    }
}