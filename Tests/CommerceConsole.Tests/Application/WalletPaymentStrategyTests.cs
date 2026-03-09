using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests wallet payment strategy behavior.
/// </summary>
public sealed class WalletPaymentStrategyTests
{
    /// <summary>
    /// Verifies successful payment debits wallet and returns completed payment.
    /// </summary>
    [Fact]
    public void ProcessPayment_WithSufficientWallet_DebitsAndCompletesPayment()
    {
        WalletPaymentStrategy strategy = new();
        Customer customer = new(Guid.NewGuid(), "Strategy User", "strategy@example.com", "pass123");
        customer.AddFunds(300m);

        Payment payment = strategy.ProcessPayment(customer, Guid.NewGuid(), 120m);

        Assert.Equal(180m, customer.WalletBalance);
        Assert.Equal(PaymentStatus.Completed, payment.Status);
        Assert.Equal("Wallet", payment.Method);
        Assert.Equal(120m, payment.Amount);
        Assert.NotNull(payment.PaidAt);
    }

    /// <summary>
    /// Verifies insufficient funds are rejected and wallet balance remains unchanged.
    /// </summary>
    [Fact]
    public void ProcessPayment_WithInsufficientWallet_ThrowsAndPreservesBalance()
    {
        WalletPaymentStrategy strategy = new();
        Customer customer = new(Guid.NewGuid(), "Strategy User", "strategy@example.com", "pass123");
        customer.AddFunds(50m);

        Assert.Throws<InsufficientFundsException>(() =>
            strategy.ProcessPayment(customer, Guid.NewGuid(), 120m));

        Assert.Equal(50m, customer.WalletBalance);
    }

    /// <summary>
    /// Verifies non-positive amount is rejected.
    /// </summary>
    [Fact]
    public void ProcessPayment_WithNonPositiveAmount_ThrowsValidationException()
    {
        WalletPaymentStrategy strategy = new();
        Customer customer = new(Guid.NewGuid(), "Strategy User", "strategy@example.com", "pass123");
        customer.AddFunds(100m);

        Assert.Throws<ValidationException>(() =>
            strategy.ProcessPayment(customer, Guid.NewGuid(), 0m));
    }
}
