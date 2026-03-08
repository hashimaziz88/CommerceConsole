using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests bonus feature services for insights and report export workflows.
/// </summary>
public sealed class BonusFeaturesServiceTests
{
    /// <summary>
    /// Verifies recommendations exclude purchased/inactive/out-of-stock products and prioritize preferred categories.
    /// </summary>
    [Fact]
    public void GetCustomerRecommendations_ExcludesPurchasedAndRanksPreferredCategoryFirst()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            InMemoryProductRepository productRepository = new(dataDirectory);
            InMemoryOrderRepository orderRepository = new(dataDirectory);

            Customer customer = new(Guid.NewGuid(), "Insight User", "insight@example.com", "pass123");
            userRepository.Add(customer);

            Product purchased = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 1200m, 4);
            Product preferredCategoryCandidate = new(Guid.NewGuid(), "Headphones", "Noise-cancelling", "Electronics", 300m, 8);
            Product otherCategoryCandidate = new(Guid.NewGuid(), "Office Chair", "Ergonomic chair", "Furniture", 450m, 5);
            Product inactiveProduct = new(Guid.NewGuid(), "Legacy Phone", "Discontinued model", "Electronics", 90m, 2);
            Product outOfStockProduct = new(Guid.NewGuid(), "Smartwatch", "Latest model", "Electronics", 550m, 0);

            preferredCategoryCandidate.Reviews.Add(new Review(Guid.NewGuid(), preferredCategoryCandidate.Id, customer.Id, 4, "Solid"));
            otherCategoryCandidate.Reviews.Add(new Review(Guid.NewGuid(), otherCategoryCandidate.Id, customer.Id, 5, "Excellent"));
            inactiveProduct.Deactivate();

            productRepository.Add(purchased);
            productRepository.Add(preferredCategoryCandidate);
            productRepository.Add(otherCategoryCandidate);
            productRepository.Add(inactiveProduct);
            productRepository.Add(outOfStockProduct);

            Order purchasedOrder = CreateOrderForCustomer(
                customer.Id,
                new OrderItem(purchased.Id, purchased.Name, purchased.Price, 1));
            purchasedOrder.UpdateStatus(OrderStatus.Delivered);
            orderRepository.Add(purchasedOrder);

            InsightsService service = new(orderRepository, productRepository);

            List<ProductRecommendationItem> recommendations = service.GetCustomerRecommendations(customer, 3);

            Assert.Equal(2, recommendations.Count);
            Assert.Equal(preferredCategoryCandidate.Id, recommendations[0].ProductId);
            Assert.DoesNotContain(recommendations, row => row.ProductId == purchased.Id);
            Assert.DoesNotContain(recommendations, row => row.ProductId == inactiveProduct.Id);
            Assert.DoesNotContain(recommendations, row => row.ProductId == outOfStockProduct.Id);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies recommendation requests reject non-positive maximum counts.
    /// </summary>
    [Fact]
    public void GetCustomerRecommendations_WithInvalidMaxCount_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository productRepository = new(dataDirectory);
            InMemoryOrderRepository orderRepository = new(dataDirectory);

            Customer customer = new(Guid.NewGuid(), "Insight User", "insight@example.com", "pass123");
            InsightsService service = new(orderRepository, productRepository);

            Assert.Throws<ValidationException>(() => service.GetCustomerRecommendations(customer, 0));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies admin insights include core heuristic summary sections.
    /// </summary>
    [Fact]
    public void GetAdminInsights_ReturnsHeuristicSummaryLines()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository productRepository = new(dataDirectory);
            InMemoryOrderRepository orderRepository = new(dataDirectory);

            Customer customer = new(Guid.NewGuid(), "Insight User", "insight@example.com", "pass123");

            Product laptop = new(Guid.NewGuid(), "Laptop", "15-inch laptop", "Electronics", 1200m, 2);
            Product cable = new(Guid.NewGuid(), "Cable", "USB-C cable", "Accessories", 25m, 1);

            laptop.Reviews.Add(new Review(Guid.NewGuid(), laptop.Id, customer.Id, 5, "Great"));
            cable.Reviews.Add(new Review(Guid.NewGuid(), cable.Id, customer.Id, 4, "Useful"));

            productRepository.Add(laptop);
            productRepository.Add(cable);

            Order order = CreateOrderForCustomer(
                customer.Id,
                new OrderItem(laptop.Id, laptop.Name, laptop.Price, 1),
                new OrderItem(cable.Id, cable.Name, cable.Price, 2));
            order.UpdateStatus(OrderStatus.Paid);
            orderRepository.Add(order);

            InsightsService service = new(orderRepository, productRepository);

            IReadOnlyList<string> insights = service.GetAdminInsights(2);

            Assert.Contains(insights, line => line.StartsWith("Insight mode: heuristic", StringComparison.Ordinal));
            Assert.Contains(insights, line => line.StartsWith("Revenue snapshot:", StringComparison.Ordinal));
            Assert.Contains(insights, line => line.StartsWith("Top category by units sold:", StringComparison.Ordinal));
            Assert.Contains(insights, line => line.StartsWith("Restock watch:", StringComparison.Ordinal));
            Assert.Contains(insights, line => line.StartsWith("Review sentiment:", StringComparison.Ordinal));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies report export service composes a snapshot and delegates export to the configured exporter.
    /// </summary>
    [Fact]
    public void ExportSalesReportPdf_DelegatesToExporterAndReturnsPath()
    {
        StubReportService reportService = new();
        RecordingReportExporter exporter = new();
        ReportExportService service = new(reportService, exporter);

        string outputDirectory = Path.Combine(Path.GetTempPath(), "CommerceConsoleReports");
        string resultPath = service.ExportSalesReportPdf($"  {outputDirectory}  ", 3, 2);

        Assert.Equal(Path.Combine(outputDirectory, "exported.pdf"), resultPath);
        Assert.Equal(outputDirectory, exporter.CapturedOutputDirectory);
        Assert.NotNull(exporter.CapturedSnapshot);
        Assert.Equal(950m, exporter.CapturedSnapshot!.TotalRevenue);
        Assert.Single(exporter.CapturedSnapshot.BestSellingProducts);
        Assert.Single(exporter.CapturedSnapshot.LowStockProducts);
        Assert.Equal(2, exporter.CapturedSnapshot.OrdersByStatus[OrderStatus.Paid]);
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

    private sealed class StubReportService : IReportService
    {
        public decimal GetTotalRevenue()
        {
            return 950m;
        }

        public Dictionary<OrderStatus, int> GetOrdersByStatus()
        {
            return new Dictionary<OrderStatus, int>
            {
                [OrderStatus.Paid] = 2,
                [OrderStatus.Delivered] = 1,
                [OrderStatus.Cancelled] = 0
            };
        }

        public List<ProductSalesReportItem> GetBestSellingProducts(int topCount)
        {
            return
            [
                new ProductSalesReportItem("Laptop", 3, 900m)
            ];
        }

        public List<LowStockReportItem> GetLowStockProducts(int threshold)
        {
            return
            [
                new LowStockReportItem("Cable", "Accessories", 2, 25m, true)
            ];
        }
    }

    private sealed class RecordingReportExporter : IReportExporter
    {
        public SalesReportSnapshot? CapturedSnapshot { get; private set; }

        public string? CapturedOutputDirectory { get; private set; }

        public string Export(SalesReportSnapshot snapshot, string outputDirectory)
        {
            CapturedSnapshot = snapshot;
            CapturedOutputDirectory = outputDirectory;
            return Path.Combine(outputDirectory, "exported.pdf");
        }
    }
}
