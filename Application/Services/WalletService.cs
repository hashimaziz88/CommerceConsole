using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements customer wallet workflows.
/// </summary>
public sealed class WalletService : IWalletService
{
    /// <inheritdoc />
    public void AddFunds(Customer customer, decimal amount)
    {
        customer.AddFunds(amount);
    }

    /// <inheritdoc />
    public decimal GetBalance(Customer customer)
    {
        return customer.WalletBalance;
    }
}
