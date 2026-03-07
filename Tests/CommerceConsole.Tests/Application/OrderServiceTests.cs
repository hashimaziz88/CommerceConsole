using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests checkout, payment simulation, and order processing behaviors.
/// </summary>
public sealed class OrderServiceTests
{
    /// <summary>
    /// Verifies successful checkout charges wallet, reduces stock, creates records, and clears cart.
    /// </summary>
    [Fact]
    public void Checkout_WithValidWalletAndStock_CreatesPaidOrderAndClearsCart()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CheckoutContext context = CreateContext(dataDirectory);

            context.Customer.AddFunds(500m);
            context.UserRepository.Update(context.Customer);

            context.Customer.Cart.AddItem(context.ProductA.Id, context.ProductA.Name, context.ProductA.Price, 2);
            context.Customer.Cart.AddItem(context.ProductB.Id, context.ProductB.Name, context.ProductB.Price, 1);
            context.UserRepository.Update(context.Customer);

            Order order = context.OrderService.Checkout(context.Customer);

            Assert.Equal(OrderStatus.Paid, order.Status);
            Assert.Equal(PaymentStatus.Completed, order.Payment.Status);
            Assert.Equal(250m, order.TotalAmount);
            Assert.Empty(context.Customer.Cart.Items);
            Assert.Equal(250m, context.Customer.WalletBalance);

            Product? updatedA = context.ProductRepository.GetById(context.ProductA.Id);
            Product? updatedB = context.ProductRepository.GetById(context.ProductB.Id);

            Assert.NotNull(updatedA);
            Assert.NotNull(updatedB);
            Assert.Equal(3, updatedA!.StockQuantity);
            Assert.Equal(2, updatedB!.StockQuantity);

            Assert.Single(context.OrderRepository.GetAll());

            InMemoryUserRepository reloadedUsers = new(dataDirectory);
            Customer? persistedCustomer = reloadedUsers.GetByEmail(context.Customer.Email) as Customer;

            Assert.NotNull(persistedCustomer);
            Assert.Equal(250m, persistedCustomer!.WalletBalance);
            Assert.Empty(persistedCustomer.Cart.Items);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies checkout fails when wallet balance is insufficient.
    /// </summary>
    [Fact]
    public void Checkout_WhenWalletBalanceIsInsufficient_ThrowsInsufficientFundsException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CheckoutContext context = CreateContext(dataDirectory);

            context.Customer.AddFunds(80m);
            context.UserRepository.Update(context.Customer);

            context.Customer.Cart.AddItem(context.ProductA.Id, context.ProductA.Name, context.ProductA.Price, 1);
            context.UserRepository.Update(context.Customer);

            Assert.Throws<InsufficientFundsException>(() => context.OrderService.Checkout(context.Customer));

            Assert.Single(context.Customer.Cart.Items);
            Assert.Equal(80m, context.Customer.WalletBalance);
            Assert.Empty(context.OrderRepository.GetAll());
            Assert.Equal(5, context.ProductRepository.GetById(context.ProductA.Id)!.StockQuantity);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies checkout fails when stock cannot satisfy cart quantities.
    /// </summary>
    [Fact]
    public void Checkout_WhenStockIsInsufficient_ThrowsInsufficientStockException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CheckoutContext context = CreateContext(dataDirectory);

            context.Customer.AddFunds(1000m);
            context.UserRepository.Update(context.Customer);

            context.Customer.Cart.AddItem(context.ProductA.Id, context.ProductA.Name, context.ProductA.Price, context.ProductA.StockQuantity + 1);
            context.UserRepository.Update(context.Customer);

            Assert.Throws<InsufficientStockException>(() => context.OrderService.Checkout(context.Customer));

            Assert.Single(context.Customer.Cart.Items);
            Assert.Equal(1000m, context.Customer.WalletBalance);
            Assert.Empty(context.OrderRepository.GetAll());
            Assert.Equal(5, context.ProductRepository.GetById(context.ProductA.Id)!.StockQuantity);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies checkout fails when a cart line references a missing product.
    /// </summary>
    [Fact]
    public void Checkout_WhenCartContainsMissingProduct_ThrowsNotFoundException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CheckoutContext context = CreateContext(dataDirectory);

            context.Customer.AddFunds(1000m);
            context.UserRepository.Update(context.Customer);

            Guid missingProductId = Guid.NewGuid();
            context.Customer.Cart.AddItem(missingProductId, "Removed Product", 120m, 1);
            context.UserRepository.Update(context.Customer);

            Assert.Throws<NotFoundException>(() => context.OrderService.Checkout(context.Customer));
            Assert.Empty(context.OrderRepository.GetAll());
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies order items remain snapshot-based even if catalog data changes later.
    /// </summary>
    [Fact]
    public void Checkout_CreatesSnapshotOrderItems()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            CheckoutContext context = CreateContext(dataDirectory);

            context.Customer.AddFunds(500m);
            context.UserRepository.Update(context.Customer);

            context.Customer.Cart.AddItem(context.ProductA.Id, context.ProductA.Name, context.ProductA.Price, 1);
            context.UserRepository.Update(context.Customer);

            Order order = context.OrderService.Checkout(context.Customer);

            Product? product = context.ProductRepository.GetById(context.ProductA.Id);
            Assert.NotNull(product);
            product!.UpdateDetails("Renamed Laptop", "Updated", "Electronics", 999m);
            context.ProductRepository.Update(product);

            Assert.Single(order.Items);
            Assert.Equal("Laptop", order.Items[0].ProductName);
            Assert.Equal(100m, order.Items[0].UnitPrice);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    private static CheckoutContext CreateContext(string dataDirectory)
    {
        InMemoryUserRepository userRepository = new(dataDirectory);
        InMemoryProductRepository productRepository = new(dataDirectory);
        InMemoryOrderRepository orderRepository = new(dataDirectory);

        Customer customer = new(Guid.NewGuid(), "Checkout User", "checkout@example.com", "pass123");
        userRepository.Add(customer);

        Product productA = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 100m, 5);
        Product productB = new(Guid.NewGuid(), "Mouse", "Wireless mouse", "Electronics", 50m, 3);
        productRepository.Add(productA);
        productRepository.Add(productB);

        OrderService orderService = new(orderRepository, productRepository, userRepository);

        return new CheckoutContext(customer, productA, productB, orderService, userRepository, productRepository, orderRepository);
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

    private sealed record CheckoutContext(
        Customer Customer,
        Product ProductA,
        Product ProductB,
        OrderService OrderService,
        InMemoryUserRepository UserRepository,
        InMemoryProductRepository ProductRepository,
        InMemoryOrderRepository OrderRepository);
}
