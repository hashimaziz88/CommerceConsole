using CommerceConsole.Application.Services.Payments;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests wallet payment strategy behavior.
/// </summary>
public sealed class PaymentStrategyTests
{
    /// <summary>
    /// Verifies wallet strategy debits funds and marks payment completed.
    /// </summary>
    [Fact]
    public void WalletPaymentStrategy_WithValidFunds_CompletesPaymentAndDebitsWallet()
    {
        WalletPaymentStrategy strategy = new();
        Customer customer = new(Guid.NewGuid(), "Wallet User", "wallet@example.com", "pass123");
        customer.AddFunds(500m);

        Payment payment = strategy.Process(customer, Guid.NewGuid(), 200m);

        Assert.Equal(PaymentStatus.Completed, payment.Status);
        Assert.Equal("Wallet", payment.Method);
        Assert.Equal(300m, customer.WalletBalance);
        Assert.Equal(200m, payment.Amount);
        Assert.NotNull(payment.PaidAt);
    }

    /// <summary>
    /// Verifies wallet strategy rejects insufficient funds.
    /// </summary>
    [Fact]
    public void WalletPaymentStrategy_WithInsufficientFunds_ThrowsInsufficientFundsException()
    {
        WalletPaymentStrategy strategy = new();
        Customer customer = new(Guid.NewGuid(), "Wallet User", "wallet@example.com", "pass123");
        customer.AddFunds(50m);

        Assert.Throws<InsufficientFundsException>(() =>
            strategy.Process(customer, Guid.NewGuid(), 100m));

        Assert.Equal(50m, customer.WalletBalance);
    }
}
