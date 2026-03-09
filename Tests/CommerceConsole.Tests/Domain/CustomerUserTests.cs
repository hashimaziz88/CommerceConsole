using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Tests customer/user behavior and account-level invariants.
/// </summary>
public sealed class CustomerUserTests
{
    /// <summary>
    /// Verifies customer constructor normalizes email and initializes owned collections.
    /// </summary>
    [Fact]
    public void Customer_Constructor_NormalizesEmailAndInitializesOwnedObjects()
    {
        Customer customer = new(Guid.NewGuid(), "Jane Doe", "  JANE@EXAMPLE.COM  ", "pass123");

        Assert.Equal("jane@example.com", customer.Email);
        Assert.Equal(UserRole.Customer, customer.Role);
        Assert.NotNull(customer.Cart);
        Assert.Equal(customer.Id, customer.Cart.CustomerId);
        Assert.Empty(customer.Orders);
        Assert.Empty(customer.Reviews);
    }

    /// <summary>
    /// Verifies adding funds increases wallet balance.
    /// </summary>
    [Fact]
    public void AddFunds_WithPositiveAmount_IncreasesWalletBalance()
    {
        Customer customer = CreateCustomer();

        customer.AddFunds(120m);

        Assert.Equal(120m, customer.WalletBalance);
    }

    /// <summary>
    /// Verifies non-positive top-ups are rejected.
    /// </summary>
    [Fact]
    public void AddFunds_WithNonPositiveAmount_ThrowsValidationException()
    {
        Customer customer = CreateCustomer();

        Assert.Throws<ValidationException>(() => customer.AddFunds(0));
    }

    /// <summary>
    /// Verifies debit rejects insufficient wallet balance.
    /// </summary>
    [Fact]
    public void DebitFunds_WhenInsufficient_ThrowsInsufficientFundsException()
    {
        Customer customer = CreateCustomer();
        customer.AddFunds(25m);

        Assert.Throws<InsufficientFundsException>(() => customer.DebitFunds(30m));
    }

    /// <summary>
    /// Verifies debit reduces wallet balance when funds are available.
    /// </summary>
    [Fact]
    public void DebitFunds_WithAvailableBalance_DecreasesWalletBalance()
    {
        Customer customer = CreateCustomer();
        customer.AddFunds(80m);

        customer.DebitFunds(30m);

        Assert.Equal(50m, customer.WalletBalance);
    }

    /// <summary>
    /// Verifies password checks are exact matches.
    /// </summary>
    [Fact]
    public void VerifyPassword_WithMatchingAndMismatchingValues_ReturnsExpectedResults()
    {
        Customer customer = CreateCustomer(password: "secret");

        Assert.True(customer.VerifyPassword("secret"));
        Assert.False(customer.VerifyPassword("wrong"));
    }

    /// <summary>
    /// Verifies administrators are created with administrator role.
    /// </summary>
    [Fact]
    public void Administrator_Constructor_AssignsAdministratorRole()
    {
        Administrator admin = new(Guid.NewGuid(), "Admin User", "admin@example.com", "adminpass");

        Assert.Equal(UserRole.Administrator, admin.Role);
    }

    private static Customer CreateCustomer(string password = "pass123")
    {
        return new Customer(Guid.NewGuid(), "Customer User", "customer@example.com", password);
    }
}
