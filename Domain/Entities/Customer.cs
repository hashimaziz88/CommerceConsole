using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents a customer account.
/// </summary>
public sealed class Customer : User
{
    /// <summary>
    /// Initializes a customer with empty order/review collections.
    /// </summary>
    public Customer(Guid id, string fullName, string email, string password)
        : base(id, fullName, email, password, UserRole.Customer)
    {
        Cart = new Cart(Id);
        Orders = new List<Order>();
        Reviews = new List<Review>();
    }

    /// <summary>
    /// Gets the current wallet balance.
    /// </summary>
    public decimal WalletBalance { get; private set; }

    /// <summary>
    /// Gets the active cart for this customer.
    /// </summary>
    public Cart Cart { get; }

    /// <summary>
    /// Gets the customer order history.
    /// </summary>
    public List<Order> Orders { get; }

    /// <summary>
    /// Gets the list of submitted reviews.
    /// </summary>
    public List<Review> Reviews { get; }

    /// <summary>
    /// Adds funds to the customer wallet.
    /// </summary>
    public void AddFunds(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ValidationException("Wallet top-up amount must be greater than zero.");
        }

        WalletBalance += amount;
    }

    /// <summary>
    /// Deducts funds from the customer wallet.
    /// </summary>
    public void DebitFunds(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ValidationException("Debit amount must be greater than zero.");
        }

        if (WalletBalance < amount)
        {
            throw new InsufficientFundsException("Insufficient wallet funds for this operation.");
        }

        WalletBalance -= amount;
    }
}
