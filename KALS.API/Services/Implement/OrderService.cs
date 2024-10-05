using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Filter;
using KALS.API.Models.Order;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Enums;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KALS.API.Services.Implement;

public class OrderService: BaseService<OrderService>, IOrderService
{
    public OrderService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<IPaginate<OrderResponse>> GetOrderList(int page, int size, OrderFilter? filter, string? sortBy, bool isAsc)
    {
        var userId = GetUserIdFromJwt();
        if (userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);
        var role = GetRoleFromJwt();

        RoleEnum roleEnum = EnumUtil.ParseEnum<RoleEnum>(role);
        IPaginate<OrderResponse> orders;
        switch (roleEnum)
        {
            case RoleEnum.Member:
                var member = await _unitOfWork.GetRepository<Member>().SingleOrDefaultAsync(
                    predicate: x => x.UserId == userId
                );
                if(member == null) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);
                 orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                    selector: o => new OrderResponse()
                    {
                        Id = o.Id,
                        CreatedAt = o.CreatedAt,
                        ModifiedAt = o.ModifiedAt,
                        Status = o.Status,
                        Total = o.Total,
                    },
                    predicate: o => o.MemberId == member.Id,
                    page: page,
                    size: size,
                    filter: filter,
                    sortBy: sortBy,
                    isAsc: isAsc
                );
                break;
            case RoleEnum.Manager:
            case RoleEnum.Staff:
                orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                    selector: o => new OrderResponse()
                    {
                        Id = o.Id,
                        CreatedAt = o.CreatedAt,
                        ModifiedAt = o.ModifiedAt,
                        Status = o.Status,
                        Total = o.Total,
                    },
                    page: page,
                    size: size,
                    filter: filter,
                    sortBy: sortBy,
                    isAsc: isAsc
                );
                break;
            default:
                throw new BadHttpRequestException(MessageConstant.User.RoleNotFound);
        }
        return orders;
    }
}