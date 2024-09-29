using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Lab;
using KALS.API.Models.Product;
using KALS.API.Services.Interface;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KALS.API.Services.Implement;

public class LabService: BaseService<LabService>, ILabService
{
    public LabService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<LabService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<GetProductResponse> AssignLabToProductAsync(Guid productId, AssignLabsToProductRequest request)
    {
        if(productId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.ProductIdNotNull);
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == productId,
            include: p => p.Include(p => p.LabProducts)
                .ThenInclude(lp => lp.Lab)
        );
        if(product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);
        
        var currentLabIds = product.LabProducts.Select(lp => lp.LabId).ToList();
        var newLabIds = request.LabIds.Except(currentLabIds).ToList();
        var removeLabIds = currentLabIds.Except(request.LabIds).ToList();

        if (removeLabIds.Any())
        {
            var removeLabProducts = product.LabProducts.Where(lp => removeLabIds.Contains(lp.LabId)).ToList();
            foreach (var removeLabProduct in removeLabProducts)
            {
                _unitOfWork.GetRepository<LabProduct>().DeleteAsync(removeLabProduct);
            }
        }
        if (newLabIds.Any())
        {
            foreach (var newLabId in newLabIds)
            {
                var newLab = _unitOfWork.GetRepository<Lab>().SingleOrDefaultAsync(
                    predicate: l => l.Id == newLabId
                );
                if (newLab != null)
                {
                    _unitOfWork.GetRepository<LabProduct>().InsertAsync(new LabProduct()
                    {
                        LabId = newLabId,
                        ProductId = productId
                    });
                }
            }
        }

        GetProductResponse response = null;
        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        if(isSuccess) response = _mapper.Map<GetProductResponse>(product);
        return response;
    }

    public async Task<IPaginate<LabResponse>> GetLabsAsync(int page, int size, string? searchName)
    {
        var labs = await _unitOfWork.GetRepository<Lab>().GetPagingListAsync(
            selector: l => new LabResponse()
            {
                Id = l.Id,
                Name = l.Name,
                Url = l.Url,
                CreatedAt = l.CreatedAt,
                ModifiedAt = l.ModifiedAt
            },
            predicate: l => (searchName.IsNullOrEmpty() || l.Name.Contains(searchName!)),
            page: page,
            size: size,
            orderBy: l => l.OrderByDescending(l => l.CreatedAt)
        );
        return labs;
    }

    public async Task<LabResponse> GetLabByIdAsync(Guid labId)
    {
        if(labId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Lab.LabIdNotNull);
        var lab =  await _unitOfWork.GetRepository<Lab>().SingleOrDefaultAsync(
            selector: l => new LabResponse()
            {
                Id = l.Id,
                Name = l.Name,
                Url = l.Url,
                CreatedAt = l.CreatedAt,
                ModifiedAt = l.ModifiedAt
            },
            predicate: l => l.Id == labId
        );
        return lab;
    }

    public async Task<ProductWithLabResponse> GetLabsByProductIdAsync(Guid productId)
    {
        if (productId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.ProductIdNotNull);
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: p => p.Id == productId,
            include: p => p.Include(p => p.LabProducts)
                .ThenInclude(lp => lp.Lab),
            selector: p => new ProductWithLabResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                IsHidden = p.IsHidden,
                
                Labs = p.LabProducts
                    .Where(lp => lp.ProductId == productId)
                    .Select(lp => lp.Lab).Select(l => new LabResponse()
                    {
                        Id = l.Id,
                        Name = l.Name,
                        Url = l.Url,
                        CreatedAt = l.CreatedAt,
                        ModifiedAt = l.ModifiedAt
                    }).ToList()
            }
        );
        return product;
    }
}