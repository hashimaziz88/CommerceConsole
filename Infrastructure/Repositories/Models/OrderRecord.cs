using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Infrastructure.Repositories.Models;

/// <summary>
/// JSON persistence model for orders.
/// </summary>
internal sealed class OrderRecord
{
    /// <summary>
    /// Gets or sets order identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets customer identifier.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets order status.
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Gets or sets line item records.
    /// </summary>
    public List<OrderItemRecord> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets payment record.
    /// </summary>
    public PaymentRecord Payment { get; set; } = new();
}
