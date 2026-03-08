using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests cart mutations and wallet workflows.
/// </summary>
public sealed class CartWalletServiceTests
{
    /// <summary>
    /// Verifies adding to cart succeeds within stock limits.
    /// </summary>
    [Fact]
    public void AddToCart_WithValidQuantity_AddsCartItem()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);

            context.CartService.AddToCart(context.Customer, context.Product.Id, 2);

            Assert.Single(context.Customer.Cart.Items);
            Assert.Equal(2, context.Customer.Cart.Items[0].Quantity);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies null customer add-to-cart calls are rejected.
    /// </summary>
    [Fact]
    public void AddToCart_WithNullCustomer_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);

            Assert.Throws<ValidationException>(() =>
                context.CartService.AddToCart(null!, context.Product.Id, 1));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies stock violations are blocked when adding to cart.
    /// </summary>
    [Fact]
    public void AddToCart_WhenQuantityExceedsStock_ThrowsInsufficientStockException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);

            Assert.Throws<InsufficientStockException>(() =>
                context.CartService.AddToCart(context.Customer, context.Product.Id, context.Product.StockQuantity + 1));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies zero quantity update removes the item.
    /// </summary>
    [Fact]
    public void UpdateCartItem_WithZeroQuantity_RemovesItem()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);
            context.CartService.AddToCart(context.Customer, context.Product.Id, 2);

            context.CartService.UpdateCartItem(context.Customer, context.Product.Id, 0);

            Assert.Empty(context.Customer.Cart.Items);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies negative quantity update is rejected.
    /// </summary>
    [Fact]
    public void UpdateCartItem_WithNegativeQuantity_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);
            context.CartService.AddToCart(context.Customer, context.Product.Id, 1);

            Assert.Throws<ValidationException>(() =>
                context.CartService.UpdateCartItem(context.Customer, context.Product.Id, -1));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies stock violations are blocked when updating cart quantity.
    /// </summary>
    [Fact]
    public void UpdateCartItem_WhenQuantityExceedsStock_ThrowsInsufficientStockException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);
            context.CartService.AddToCart(context.Customer, context.Product.Id, 1);

            Assert.Throws<InsufficientStockException>(() =>
                context.CartService.UpdateCartItem(context.Customer, context.Product.Id, context.Product.StockQuantity + 5));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies wallet top-up updates balance and is persisted.
    /// </summary>
    [Fact]
    public void AddFunds_WithPositiveAmount_UpdatesAndPersistsBalance()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);

            context.WalletService.AddFunds(context.Customer, 250m);

            InMemoryUserRepository reloadedRepository = new(dataDirectory);
            Customer? reloadedCustomer = reloadedRepository.GetByEmail(context.Customer.Email) as Customer;

            Assert.NotNull(reloadedCustomer);
            Assert.Equal(250m, reloadedCustomer!.WalletBalance);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies invalid wallet top-up amount fails validation.
    /// </summary>
    [Fact]
    public void AddFunds_WithNonPositiveAmount_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);

            Assert.Throws<ValidationException>(() =>
                context.WalletService.AddFunds(context.Customer, 0));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies null customer balance checks are rejected.
    /// </summary>
    [Fact]
    public void GetBalance_WithNullCustomer_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CartWalletContext context = CreateContext(dataDirectory);

            Assert.Throws<ValidationException>(() =>
                _ = context.WalletService.GetBalance(null!));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    private static CartWalletContext CreateContext(string dataDirectory)
    {
        InMemoryUserRepository userRepository = new(dataDirectory);
        InMemoryProductRepository productRepository = new(dataDirectory);

        Customer customer = new(Guid.NewGuid(), "Cart User", "cartuser@example.com", "pass123");
        userRepository.Add(customer);

        Product product = new(
            Guid.NewGuid(),
            "Notebook",
            "A5 notebook",
            "Stationery",
            45m,
            10);
        productRepository.Add(product);

        CartService cartService = new(productRepository, userRepository);
        WalletService walletService = new(userRepository);

        return new CartWalletContext(customer, product, cartService, walletService);
    }

    private static string CreateTempDataDirectory()
    {
        string path = Path.Combine(Path.GetTempPath(), "CommerceConsoleTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    private sealed record CartWalletContext(
        Customer Customer,
        Product Product,
        CartService CartService,
        WalletService WalletService);
}