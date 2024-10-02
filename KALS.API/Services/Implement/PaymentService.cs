using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Cart;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Enums;
using KALS.Repository.Interface;
using Net.payOS;
using Net.payOS.Types;

namespace KALS.API.Services.Implement;

public class PaymentService: BaseService<PaymentService>, IPaymentService
{
    
    public PaymentService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<PaymentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
        
    }

    public async Task<string> CheckOut(ICollection<CartModelResponse> request)
    {
        PayOS _payOs = new PayOS(_configuration["PAYOS:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
            _configuration["PAYOS:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
            _configuration["PAYOS:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));
        
        var userId = JwtUtil.GetUserIdFromToken(_httpContextAccessor);
        var member = await _unitOfWork.GetRepository<Member>().SingleOrDefaultAsync(
            predicate: m => m.UserId == userId
        );
        if (member == null) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);
        
        var order = new Order()
        {
            Id = Guid.NewGuid(),
            CreatedAt = TimeUtil.GetCurrentSEATime(),
            ModifiedAt = TimeUtil.GetCurrentSEATime(),
            Status = OrderStatus.Pending,
            MemberId = member.Id
        };
        
        var orderItems = new List<OrderItem>();
        decimal orderTotal = 0;
        List<ItemData> items = new List<ItemData>();
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        
        foreach (var cartModel in request)
        {
            var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: p => p.Id == cartModel.ProductId
            );
            if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);
            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Quantity = cartModel.Quantity,
                CreatedAt = TimeUtil.GetCurrentSEATime(),
                ModifiedAt =TimeUtil.GetCurrentSEATime()
            };
            orderItems.Add(orderItem);
            
            decimal itemTotal = product.Price * cartModel.Quantity;
            orderTotal += itemTotal;
            var item = new ItemData(product.Name, cartModel.Quantity, (int) itemTotal);
            items.Add(item);
        }

        order.Total = orderTotal;
        
        var payment = new Payment()
        {
            Id = Guid.NewGuid(),
            TransactionId = orderCode.ToString(),
            CreatedAt = TimeUtil.GetCurrentSEATime(),
            ModifiedAt = TimeUtil.GetCurrentSEATime(),
            Status = PaymentStatus.Processing,
            PaymentDateTime = TimeUtil.GetCurrentSEATime(),
            Amount = order.Total,
        };
        using(var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);
                bool isPaymentSaved = await _unitOfWork.CommitAsync() > 0;
                if (!isPaymentSaved) throw new BadHttpRequestException("Failed to create the payment.");
                order.PaymentId = payment.Id;
                await _unitOfWork.GetRepository<Order>().InsertAsync(order);
                bool isOrderSaved = await _unitOfWork.CommitAsync() > 0;
                if (!isOrderSaved) throw new BadHttpRequestException("Failed to create the order.");
                
                foreach (var orderItem in orderItems)
                {
                    orderItem.OrderId = order.Id;
                }
                
                await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
                bool isSuccess = await _unitOfWork.CommitAsync() > 0;
                if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Order.CreateOrderFail);
                
                PaymentData paymentData = new PaymentData(
                    orderCode, 
                    (int)order.Total, 
                    "Thanh toán đơn hàng", 
                    items, 
                    "https://localhost:3002/cancel", 
                    "https://localhost:3002/success"
                );

                // Call the external payment service to create a payment link
                CreatePaymentResult createPayment = await _payOs.createPaymentLink(paymentData);
                

                return createPayment.checkoutUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await _unitOfWork.RollbackTransactionAsync(transaction);
                throw new BadHttpRequestException(MessageConstant.Order.CreateOrderFail);
            }
        }
        // await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);
        // order.Total = orderItems.Sum(oi => oi.Product.Price * oi.Quantity);
        // await _unitOfWork.GetRepository<Order>().InsertAsync(order);
        // await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
        // bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        // if (!isSuccess) throw new BadHttpRequestException(MessageConstant.Order.CreateOrderFail);
        //
        // PaymentData paymentData = new PaymentData(orderCode, (int) order.Total, "Thanh toan don hang", items, "https://localhost:3002/cancel", "https://localhost:3002/success");
        //
        // CreatePaymentResult createPayment = await _payOs.createPaymentLink(paymentData);
        //
        // return createPayment.checkoutUrl;

    }
}