using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Defines a payment algorithm used during checkout.
/// </summary>
public interface IPaymentStrategy
{
    /// <summary>
    /// Executes payment for a checkout request and returns payment record details.
    /// </summary>
    Payment ProcessPayment(Customer customer, Guid orderId, decimal amount);
}
