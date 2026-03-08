using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements customer wallet workflows.
/// </summary>
public sealed class WalletService : IWalletService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes wallet service dependencies.
    /// </summary>
    public WalletService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public void AddFunds(Customer customer, decimal amount)
    {
        EnsureCustomer(customer);
        customer.AddFunds(amount);
        _userRepository.Update(customer);
    }

    /// <inheritdoc />
    public decimal GetBalance(Customer customer)
    {
        EnsureCustomer(customer);
        return customer.WalletBalance;
    }

    private static void EnsureCustomer(Customer customer)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required.");
        }
    }
}