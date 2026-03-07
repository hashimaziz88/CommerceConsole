using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for customer wallet workflows.
/// </summary>
public interface IWalletService
{
    /// <summary>
    /// Adds wallet funds.
    /// </summary>
    void AddFunds(Customer customer, decimal amount);

    /// <summary>
    /// Returns current wallet balance.
    /// </summary>
    decimal GetBalance(Customer customer);
}
