using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for payment processing strategies.
/// </summary>
public interface IPaymentStrategy
{
    /// <summary>
    /// Gets the payment method label used by this strategy.
    /// </summary>
    string MethodName { get; }

    /// <summary>
    /// Processes payment for a customer and returns the payment record.
    /// </summary>
    Payment Process(Customer customer, Guid orderId, decimal amount);
}
