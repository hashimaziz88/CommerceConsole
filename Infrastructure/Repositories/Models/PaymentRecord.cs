using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Infrastructure.Repositories.Models;

/// <summary>
/// JSON persistence model for payments.
/// </summary>
internal sealed class PaymentRecord
{
    /// <summary>
    /// Gets or sets payment identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets order identifier.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets payment method.
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets payment status.
    /// </summary>
    public PaymentStatus Status { get; set; }
}
