using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Category;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KALS.API.Services.Implement;

public class CategoryService: BaseService<CategoryService>, ICategoryService
{
    public CategoryService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<CategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<IPaginate<CategoryResponse>> GetCategoriesPagingAsync(int page, int size)
    {
        var categories = await _unitOfWork.GetRepository<Category>().GetPagingListAsync(
            selector: c => new CategoryResponse()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ModifiedAt = c.ModifiedAt
            },
            page: page,
            size: size,
            orderBy: c => c.OrderByDescending(c => c.CreatedAt)
        );
        return categories;
    }

    public async Task<CategoryResponse> GetCategoryByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryIdNotNull);
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            selector: c => new CategoryResponse()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ModifiedAt = c.ModifiedAt
            },
            predicate: c => c.Id == id
        );
        return category;
    }

    public async Task<CategoryResponse> UpdateProductCategoryByCategoryIdAsync(Guid categoryId, UpdateProductCategoryRequest request)
    {
        if(categoryId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryIdNotNull);
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: c => c.Id == categoryId,
            include: c => c.Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
        );
        if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFound);
        
        var currentProductIds = category.ProductCategories
            .Select(pc => pc.ProductId)
            .ToList();
        var newProductIds = request.ProductIds.Except(currentProductIds).ToList();
        var removeProductIds = currentProductIds.Except(request.ProductIds).ToList();

        if (removeProductIds.Any())
        {
            var removeProductCategories = category.ProductCategories.Where(pc => removeProductIds.Contains(pc.ProductId)).ToList();
            foreach (var removeProductCategory in removeProductCategories)
            {
                _unitOfWork.GetRepository<ProductCategory>().DeleteAsync(removeProductCategory);
            }
        }

        if (newProductIds.Any())
        {
            foreach (var newProductId in newProductIds)
            {
                var newProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                    predicate: p => p.Id == newProductId
                );
                if (newProduct != null)
                {
                    await _unitOfWork.GetRepository<ProductCategory>().InsertAsync(new ProductCategory()
                    {
                        ProductId = newProductId,
                        CategoryId = categoryId
                    });
                }
            }
        }

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        CategoryResponse response = null;
        if(isSuccess) response = _mapper.Map<CategoryResponse>(category);
        return response;
    }

    public async Task<CategoryResponse> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request)
    {
        if (categoryId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Category.CategoryIdNotNull);
        var category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: c => c.Id == categoryId
        );
        if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFound);
        category.Name = request.Name;
        category.Description = request.Description;
        category.ModifiedAt = TimeUtil.GetCurrentSEATime();
        
        _unitOfWork.GetRepository<Category>().UpdateAsync(category);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        CategoryResponse response = null;
        if (isSuccess) response = _mapper.Map<CategoryResponse>(category);
        return response;
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = _mapper.Map<Category>(request);
        category.Id = Guid.NewGuid();
        category.CreatedAt = TimeUtil.GetCurrentSEATime();
        category.ModifiedAt = TimeUtil.GetCurrentSEATime();

        await _unitOfWork.GetRepository<Category>().InsertAsync(category);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        CategoryResponse response = null;
        if (isSuccess) response = _mapper.Map<CategoryResponse>(category);
        return response;
    }
}