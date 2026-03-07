using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests product browsing, search, and catalog management workflows.
/// </summary>
public sealed class ProductServiceTests
{
    /// <summary>
    /// Verifies products can be added through the service.
    /// </summary>
    [Fact]
    public void AddProduct_WithValidValues_AddsProduct()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            var product = service.AddProduct("Keyboard", "Mechanical keyboard", "Electronics", 1200m, 15);

            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.NotNull(repository.GetById(product.Id));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies product validation is enforced on add.
    /// </summary>
    [Fact]
    public void AddProduct_WithNegativePrice_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            Assert.Throws<ValidationException>(() =>
                _ = service.AddProduct("Keyboard", "Mechanical keyboard", "Electronics", -1m, 10));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies search returns active matches by name or category.
    /// </summary>
    [Fact]
    public void SearchProducts_ByCategory_ReturnsOnlyActiveMatchingProducts()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            var active = service.AddProduct("Laptop", "15 inch laptop", "Electronics", 15000m, 4);
            _ = service.AddProduct("Desk", "Office desk", "Furniture", 3000m, 8);
            var inactive = service.AddProduct("Old Phone", "Legacy phone", "Electronics", 800m, 2);
            inactive.Deactivate();
            repository.Update(inactive);

            var results = service.SearchProducts("electronics");

            Assert.Single(results);
            Assert.Equal(active.Id, results[0].Id);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies update mutates the stored product details.
    /// </summary>
    [Fact]
    public void UpdateProduct_WithValidInput_UpdatesStoredProduct()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            var product = service.AddProduct("Mouse", "Optical mouse", "Electronics", 250m, 20);

            service.UpdateProduct(product.Id, "Gaming Mouse", "RGB gaming mouse", "Electronics", 399m);

            var updated = repository.GetById(product.Id);
            Assert.NotNull(updated);
            Assert.Equal("Gaming Mouse", updated!.Name);
            Assert.Equal(399m, updated.Price);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies update fails for unknown product IDs.
    /// </summary>
    [Fact]
    public void UpdateProduct_WithUnknownId_ThrowsNotFoundException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            Assert.Throws<NotFoundException>(() =>
                service.UpdateProduct(Guid.NewGuid(), "Name", "Desc", "Cat", 10m));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies delete removes a product from repository.
    /// </summary>
    [Fact]
    public void DeleteProduct_RemovesExistingProduct()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            var product = service.AddProduct("Chair", "Office chair", "Furniture", 900m, 10);

            service.DeleteProduct(product.Id);

            Assert.Null(repository.GetById(product.Id));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies restock increases stock quantity.
    /// </summary>
    [Fact]
    public void RestockProduct_IncreasesStockQuantity()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            var product = service.AddProduct("Notebook", "A5 notebook", "Stationery", 45m, 5);

            service.RestockProduct(product.Id, 7);

            var updated = repository.GetById(product.Id);
            Assert.NotNull(updated);
            Assert.Equal(12, updated!.StockQuantity);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies low-stock report returns products at or below threshold.
    /// </summary>
    [Fact]
    public void GetLowStockProducts_ReturnsMatchingProducts()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);
            ProductService service = new(repository);

            var lowA = service.AddProduct("Pen", "Blue pen", "Stationery", 10m, 2);
            var lowB = service.AddProduct("Pencil", "HB pencil", "Stationery", 5m, 3);
            _ = service.AddProduct("Monitor", "24 inch monitor", "Electronics", 2500m, 20);

            var results = service.GetLowStockProducts(3);
            var ids = results.Select(product => product.Id).ToHashSet();

            Assert.Equal(2, results.Count);
            Assert.Contains(lowA.Id, ids);
            Assert.Contains(lowB.Id, ids);
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
