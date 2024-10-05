using KALS.API.Models.Order;
using KALS.Domain.Enums;

namespace KALS.API.Models.Payment;

public class PaymentWithOrderResponse
{
    public Guid Id { get; set; }
    public DateTime PaymentDateTime { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public PaymentStatus Status { get; set; }
    public OrderResponse Order { get; set; }
}