using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Infrastructure;

/// <summary>
/// Verifies repository contract behavior and JSON rehydration parity.
/// </summary>
public sealed class RepositoryContractTests
{
    /// <summary>
    /// Verifies product repository supports CRUD operations and persists updates across instances.
    /// </summary>
    [Fact]
    public void ProductRepository_CrudAndPersistenceParity()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            Product product = new(Guid.NewGuid(), "Keyboard", "Mechanical keyboard", "Accessories", 80m, 10);

            repository.Add(product);
            Assert.NotNull(repository.GetById(product.Id));

            List<Product> snapshot = repository.GetAll();
            snapshot.Clear();
            Assert.Single(repository.GetAll());

            product.UpdateDetails("Keyboard Pro", "Mechanical RGB keyboard", "Accessories", 95m);
            product.Restock(5);
            repository.Update(product);

            InMemoryProductRepository reloaded = new(dataDirectory);
            Product? persisted = reloaded.GetById(product.Id);

            Assert.NotNull(persisted);
            Assert.Equal("Keyboard Pro", persisted!.Name);
            Assert.Equal(15, persisted.StockQuantity);
            Assert.Equal(95m, persisted.Price);

            reloaded.Remove(product.Id);
            Assert.Null(reloaded.GetById(product.Id));

            InMemoryProductRepository afterRemoveReload = new(dataDirectory);
            Assert.Null(afterRemoveReload.GetById(product.Id));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies user repository supports CRUD operations and persists updates across instances.
    /// </summary>
    [Fact]
    public void UserRepository_CrudAndPersistenceParity()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository repository = new(dataDirectory);
            Customer customer = new(Guid.NewGuid(), "Repo User", "repo-user@example.com", "pass123");

            repository.Add(customer);
            Assert.NotNull(repository.GetById(customer.Id));
            Assert.NotNull(repository.GetByEmail("REPO-USER@example.com"));

            List<User> snapshot = repository.GetAll();
            snapshot.Clear();
            Assert.Single(repository.GetAll());

            customer.AddFunds(250m);
            customer.Cart.AddItem(Guid.NewGuid(), "Cable", 20m, 2);
            repository.Update(customer);

            InMemoryUserRepository reloaded = new(dataDirectory);
            Customer? persisted = reloaded.GetById(customer.Id) as Customer;

            Assert.NotNull(persisted);
            Assert.Equal(250m, persisted!.WalletBalance);
            Assert.Single(persisted.Cart.Items);

            reloaded.Remove(customer.Id);
            Assert.Null(reloaded.GetById(customer.Id));

            InMemoryUserRepository afterRemoveReload = new(dataDirectory);
            Assert.Null(afterRemoveReload.GetById(customer.Id));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies order repository supports CRUD operations and persists updates across instances.
    /// </summary>
    [Fact]
    public void OrderRepository_CrudAndPersistenceParity()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryOrderRepository repository = new(dataDirectory);
            Guid customerId = Guid.NewGuid();
            Guid orderId = Guid.NewGuid();

            List<OrderItem> items =
            [
                new OrderItem(Guid.NewGuid(), "Laptop", 100m, 2),
                new OrderItem(Guid.NewGuid(), "Mouse", 50m, 1)
            ];

            Payment payment = new(Guid.NewGuid(), orderId, 250m, "Wallet");
            payment.MarkCompleted();

            Order order = new(orderId, customerId, items, payment);
            order.UpdateStatus(OrderStatus.Paid);

            repository.Add(order);
            Assert.NotNull(repository.GetById(order.Id));
            Assert.Single(repository.GetByCustomerId(customerId));

            List<Order> snapshot = repository.GetAll();
            snapshot.Clear();
            Assert.Single(repository.GetAll());

            order.UpdateStatus(OrderStatus.Processing);
            repository.Update(order);

            InMemoryOrderRepository reloaded = new(dataDirectory);
            Order? persisted = reloaded.GetById(order.Id);

            Assert.NotNull(persisted);
            Assert.Equal(OrderStatus.Processing, persisted!.Status);
            Assert.Equal(PaymentStatus.Completed, persisted.Payment.Status);
            Assert.Equal(2, persisted.Items.Count);

            reloaded.Remove(order.Id);
            Assert.Null(reloaded.GetById(order.Id));

            InMemoryOrderRepository afterRemoveReload = new(dataDirectory);
            Assert.Null(afterRemoveReload.GetById(order.Id));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
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
}
