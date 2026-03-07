using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents a payment transaction for an order.
/// </summary>
public sealed class Payment
{
    /// <summary>
    /// Initializes a payment record.
    /// </summary>
    public Payment(Guid id, Guid orderId, decimal amount, string method)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("Payment ID must be valid.");
        }

        if (orderId == Guid.Empty)
        {
            throw new ValidationException("Order ID must be valid.");
        }

        if (amount <= 0)
        {
            throw new ValidationException("Payment amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(method))
        {
            throw new ValidationException("Payment method is required.");
        }

        Id = id;
        OrderId = orderId;
        Amount = amount;
        Method = method.Trim();
        Status = PaymentStatus.Pending;
    }

    /// <summary>
    /// Gets payment ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets associated order ID.
    /// </summary>
    public Guid OrderId { get; }

    /// <summary>
    /// Gets payment amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Gets payment method label.
    /// </summary>
    public string Method { get; }

    /// <summary>
    /// Gets payment status.
    /// </summary>
    public PaymentStatus Status { get; private set; }

    /// <summary>
    /// Gets payment timestamp when completed.
    /// </summary>
    public DateTime? PaidAt { get; private set; }

    /// <summary>
    /// Marks payment as completed.
    /// </summary>
    public void MarkCompleted()
    {
        Status = PaymentStatus.Completed;
        PaidAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks payment as failed.
    /// </summary>
    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;
    }
}
