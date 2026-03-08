using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services.Payments;

/// <summary>
/// Processes wallet-based payments.
/// </summary>
public sealed class WalletPaymentStrategy : IPaymentStrategy
{
    /// <inheritdoc />
    public string MethodName => "Wallet";

    /// <inheritdoc />
    public Payment Process(Customer customer, Guid orderId, decimal amount)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required for payment processing.");
        }

        if (orderId == Guid.Empty)
        {
            throw new ValidationException("Order ID must be valid for payment processing.");
        }

        if (amount <= 0)
        {
            throw new ValidationException("Payment amount must be greater than zero.");
        }

        Payment payment = new(Guid.NewGuid(), orderId, amount, MethodName);
        customer.DebitFunds(amount);
        payment.MarkCompleted();

        return payment;
    }
}
