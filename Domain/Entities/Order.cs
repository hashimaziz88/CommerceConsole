using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents an order placed by a customer.
/// </summary>
public sealed class Order
{
    /// <summary>
    /// Initializes an order.
    /// </summary>
    public Order(Guid id, Guid customerId, IEnumerable<OrderItem> items, Payment payment)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("Order ID must be valid.");
        }

        if (customerId == Guid.Empty)
        {
            throw new ValidationException("Customer ID must be valid.");
        }

        List<OrderItem> orderItems = items?.ToList() ?? new List<OrderItem>();
        if (orderItems.Count == 0)
        {
            throw new ValidationException("An order must contain at least one item.");
        }

        Id = id;
        CustomerId = customerId;
        Items = orderItems;
        TotalAmount = orderItems.Sum(i => i.LineTotal);
        Payment = payment;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets order ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets customer ID.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets immutable order item snapshots.
    /// </summary>
    public List<OrderItem> Items { get; }

    /// <summary>
    /// Gets total amount.
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Gets current order status.
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Gets creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets linked payment record.
    /// </summary>
    public Payment Payment { get; }

    /// <summary>
    /// Updates the order status.
    /// </summary>
    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }
}
