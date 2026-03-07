using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests review workflows and sales reporting calculations.
/// </summary>
public sealed class ReviewAndReportServiceTests
{
    /// <summary>
    /// Verifies adding a valid review persists and contributes to average rating.
    /// </summary>
    [Fact]
    public void AddReview_WithPurchasedProduct_PersistsReviewAndAverage()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            ReviewContext context = CreateReviewContext(dataDirectory);

            context.ReviewService.AddReview(context.Customer, context.Product.Id, 5, "Great product");

            InMemoryProductRepository reloadedProducts = new(dataDirectory);
            Product? persisted = reloadedProducts.GetById(context.Product.Id);

            Assert.NotNull(persisted);
            Assert.Single(persisted!.Reviews);
            Assert.Equal(5, persisted.Reviews[0].Rating);
            Assert.Equal(5d, context.ReviewService.GetAverageRating(context.Product.Id));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies users cannot review products they never purchased.
    /// </summary>
    [Fact]
    public void AddReview_WithoutPurchase_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            InMemoryProductRepository productRepository = new(dataDirectory);
            InMemoryOrderRepository orderRepository = new(dataDirectory);

            Customer customer = new(Guid.NewGuid(), "Review User", "review@example.com", "pass123");
            userRepository.Add(customer);

            Product product = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 1000m, 5);
            productRepository.Add(product);

            ReviewService service = new(orderRepository, productRepository, userRepository);

            Assert.Throws<ValidationException>(() =>
                service.AddReview(customer, product.Id, 5, "Should fail"));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies only purchased products are returned as review candidates.
    /// </summary>
    [Fact]
    public void GetReviewableProducts_ReturnsPurchasedProductsOnly()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            InMemoryProductRepository productRepository = new(dataDirectory);
            InMemoryOrderRepository orderRepository = new(dataDirectory);

            Customer customer = new(Guid.NewGuid(), "Review User", "review@example.com", "pass123");
            userRepository.Add(customer);

            Product purchasedProduct = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 1000m, 5);
            Product notPurchasedProduct = new(Guid.NewGuid(), "Mouse", "Wireless mouse", "Electronics", 50m, 10);

            productRepository.Add(purchasedProduct);
            productRepository.Add(notPurchasedProduct);

            Order purchasedOrder = CreateOrderForCustomer(customer.Id,
                new OrderItem(purchasedProduct.Id, purchasedProduct.Name, purchasedProduct.Price, 1));
            purchasedOrder.UpdateStatus(OrderStatus.Paid);
            orderRepository.Add(purchasedOrder);

            ReviewService service = new(orderRepository, productRepository, userRepository);
            List<Product> reviewableProducts = service.GetReviewableProducts(customer);

            Assert.Single(reviewableProducts);
            Assert.Equal(purchasedProduct.Id, reviewableProducts[0].Id);
            Assert.DoesNotContain(reviewableProducts, product => product.Id == notPurchasedProduct.Id);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies out-of-range review ratings are rejected.
    /// </summary>
    [Fact]
    public void AddReview_WithOutOfRangeRating_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            ReviewContext context = CreateReviewContext(dataDirectory);

            Assert.Throws<ValidationException>(() =>
                context.ReviewService.AddReview(context.Customer, context.Product.Id, 6, "Invalid rating"));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies total revenue and order-by-status report calculations.
    /// </summary>
    [Fact]
    public void ReportService_CalculatesRevenueAndOrdersByStatus()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            var context = CreateReportContext(dataDirectory);

            decimal totalRevenue = context.ReportService.GetTotalRevenue();
            Dictionary<OrderStatus, int> byStatus = context.ReportService.GetOrdersByStatus();

            Assert.Equal(400m, totalRevenue);
            Assert.Equal(1, byStatus[OrderStatus.Delivered]);
            Assert.Equal(1, byStatus[OrderStatus.Paid]);
            Assert.Equal(0, byStatus[OrderStatus.Cancelled]);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies best-selling report returns quantity/revenue-ranked products.
    /// </summary>
    [Fact]
    public void ReportService_GetBestSellingProducts_ReturnsRankedResults()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            var context = CreateReportContext(dataDirectory);

            var bestSellers = context.ReportService.GetBestSellingProducts(2);

            Assert.Equal(2, bestSellers.Count);
            Assert.Equal("Laptop", bestSellers[0].ProductName);
            Assert.Equal(3, bestSellers[0].TotalQuantitySold);
            Assert.Equal(300m, bestSellers[0].TotalRevenue);

            Assert.Equal("Mouse", bestSellers[1].ProductName);
            Assert.Equal(2, bestSellers[1].TotalQuantitySold);
            Assert.Equal(100m, bestSellers[1].TotalRevenue);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies low-stock report filters and sorts by threshold.
    /// </summary>
    [Fact]
    public void ReportService_GetLowStockProducts_ReturnsThresholdMatches()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            var context = CreateReportContext(dataDirectory);

            var lowStock = context.ReportService.GetLowStockProducts(2);

            Assert.Equal(2, lowStock.Count);
            Assert.Equal("Cable", lowStock[0].ProductName);
            Assert.Equal(1, lowStock[0].StockQuantity);
            Assert.Equal("Laptop", lowStock[1].ProductName);
            Assert.Equal(2, lowStock[1].StockQuantity);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    private static ReviewContext CreateReviewContext(string dataDirectory)
    {
        InMemoryUserRepository userRepository = new(dataDirectory);
        InMemoryProductRepository productRepository = new(dataDirectory);
        InMemoryOrderRepository orderRepository = new(dataDirectory);

        Customer customer = new(Guid.NewGuid(), "Review User", "review@example.com", "pass123");
        userRepository.Add(customer);

        Product product = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 1000m, 5);
        productRepository.Add(product);

        Order purchasedOrder = CreateOrderForCustomer(customer.Id,
            new OrderItem(product.Id, product.Name, product.Price, 1));
        purchasedOrder.UpdateStatus(OrderStatus.Paid);
        orderRepository.Add(purchasedOrder);

        ReviewService reviewService = new(orderRepository, productRepository, userRepository);
        return new ReviewContext(reviewService, customer, product);
    }

    private static ReportContext CreateReportContext(string dataDirectory)
    {
        InMemoryOrderRepository orderRepository = new(dataDirectory);
        InMemoryProductRepository productRepository = new(dataDirectory);

        Product laptop = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 100m, 2);
        Product mouse = new(Guid.NewGuid(), "Mouse", "Wireless mouse", "Electronics", 50m, 8);
        Product cable = new(Guid.NewGuid(), "Cable", "USB cable", "Accessories", 20m, 1);

        productRepository.Add(laptop);
        productRepository.Add(mouse);
        productRepository.Add(cable);

        Order deliveredOrder = CreateOrder(
            new OrderItem(laptop.Id, laptop.Name, laptop.Price, 2),
            new OrderItem(mouse.Id, mouse.Name, mouse.Price, 1));
        deliveredOrder.UpdateStatus(OrderStatus.Delivered);

        Order paidOrder = CreateOrder(
            new OrderItem(laptop.Id, laptop.Name, laptop.Price, 1),
            new OrderItem(mouse.Id, mouse.Name, mouse.Price, 1));
        paidOrder.UpdateStatus(OrderStatus.Paid);

        orderRepository.Add(deliveredOrder);
        orderRepository.Add(paidOrder);

        ReportService reportService = new(orderRepository, productRepository);
        return new ReportContext(reportService);
    }

    private static Order CreateOrder(params OrderItem[] items)
    {
        Guid orderId = Guid.NewGuid();
        decimal amount = items.Sum(item => item.LineTotal);

        Payment payment = new(Guid.NewGuid(), orderId, amount, "Wallet");
        payment.MarkCompleted();

        Order order = new(orderId, Guid.NewGuid(), items, payment);
        order.UpdateStatus(OrderStatus.Paid);
        return order;
    }

    private static Order CreateOrderForCustomer(Guid customerId, params OrderItem[] items)
    {
        Guid orderId = Guid.NewGuid();
        decimal amount = items.Sum(item => item.LineTotal);

        Payment payment = new(Guid.NewGuid(), orderId, amount, "Wallet");
        payment.MarkCompleted();

        Order order = new(orderId, customerId, items, payment);
        order.UpdateStatus(OrderStatus.Paid);
        return order;
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

    private sealed record ReviewContext(
        ReviewService ReviewService,
        Customer Customer,
        Product Product);

    private sealed record ReportContext(ReportService ReportService);
}
