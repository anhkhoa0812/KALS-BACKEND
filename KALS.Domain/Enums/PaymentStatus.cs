namespace KALS.Domain.Enums;

public enum PaymentStatus
{
    Fail = 0,
    Processing = 1,
    Refunded = 2,
    Paid = 3,
    PendingPayExtra = 4,
    PendingRefund = 5,
}