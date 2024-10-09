using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Filter;
using KALS.API.Models.Order;
using KALS.API.Models.OrderItem;
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
        if (userId == Guid.Empty) throw new UnauthorizedAccessException(MessageConstant.User.UserNotFound);
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
                        Address = o.Address,
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
                        Address = o.Address,
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

    public async Task<OrderResponse> UpdateOrderStatusCompleted(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Order.OrderIdNotNull);
        var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
            predicate: o => o.Id == orderId
        );
        if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFound);

        switch (order.Status)
        {
            case OrderStatus.Pending:
                throw new BadHttpRequestException(MessageConstant.Payment.YourOrderIsNotPaid);
            case OrderStatus.Cancelled:
                throw new BadHttpRequestException(MessageConstant.Payment.YourOrderIsCancelled);
            case OrderStatus.Completed:
                throw new BadHttpRequestException(MessageConstant.Payment.YourOrderIsCompleted);
            case OrderStatus.Processing:
                order.Status = order.Status = OrderStatus.Completed;
                break;
            default:
                throw new BadHttpRequestException(MessageConstant.Order.OrderStatusNotFound);
        }
        
        var orderItems = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
            predicate: oi => oi.OrderId == orderId
        );
        foreach (var orderItem in orderItems)
        {
            var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: p => p.Id == orderItem.ProductId,
                include: p => p.Include(p => p.LabProducts)
                    .ThenInclude(lp => lp.Lab)
            );
            if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);
            product.LabProducts!.ToList().ForEach(async lp =>
            {
                var existedLabMember = await _unitOfWork.GetRepository<LabMember>().SingleOrDefaultAsync(
                    predicate: lm => lm.LabId == lp.LabId && lm.MemberId == order.MemberId
                );
                if (existedLabMember != null) return;
                await _unitOfWork.GetRepository<LabMember>().InsertAsync(new LabMember()
                {
                    MemberId = order.MemberId,
                    LabId = lp.LabId
                });
                var isSuccess = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Product.UpdateProductFail);
            }); 
            
        }
        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        OrderResponse response = null;
        if (isSuccess) response = _mapper.Map<OrderResponse>(order);
        return response;
    }

    public async Task<ICollection<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Order.OrderIdNotNull);
        var orderItems = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
            predicate: oi => oi.OrderId == orderId,
            include: oi => oi.Include(oi => oi.Product)
        );
        var response = _mapper.Map<ICollection<OrderItemResponse>>(orderItems);
        return response;
    } 
}