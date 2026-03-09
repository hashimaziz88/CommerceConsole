using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Wallet-backed payment strategy.
/// </summary>
public sealed class WalletPaymentStrategy : IPaymentStrategy
{
    private const string WalletMethod = "Wallet";

    /// <inheritdoc />
    public Payment ProcessPayment(Customer customer, Guid orderId, decimal amount)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required for payment.");
        }

        if (orderId == Guid.Empty)
        {
            throw new ValidationException("Order ID must be valid.");
        }

        if (amount <= 0)
        {
            throw new ValidationException("Payment amount must be greater than zero.");
        }

        if (customer.WalletBalance < amount)
        {
            throw new InsufficientFundsException("Insufficient wallet funds for checkout.");
        }

        Payment payment = new(Guid.NewGuid(), orderId, amount, WalletMethod);
        customer.DebitFunds(amount);
        payment.MarkCompleted();

        return payment;
    }
}
