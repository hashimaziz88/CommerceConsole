namespace CommerceConsole.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of an order.
/// </summary>
public enum OrderStatus
{
    Pending = 1,
    Paid = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6
}
