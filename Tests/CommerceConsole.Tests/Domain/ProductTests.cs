using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Tests product invariants and mutation behavior.
/// </summary>
public sealed class ProductTests
{
    /// <summary>
    /// Verifies constructor rejects negative stock values.
    /// </summary>
    [Fact]
    public void Constructor_WithNegativeStock_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            _ = CreateProduct(stockQuantity: -1));
    }

    /// <summary>
    /// Verifies update rejects empty name values.
    /// </summary>
    [Fact]
    public void UpdateDetails_WithEmptyName_ThrowsValidationException()
    {
        Product product = CreateProduct();

        Assert.Throws<ValidationException>(() =>
            product.UpdateDetails("  ", "Updated", "Electronics", 150m));
    }

    /// <summary>
    /// Verifies update applies and trims new values.
    /// </summary>
    [Fact]
    public void UpdateDetails_WithValidValues_UpdatesAndTrims()
    {
        Product product = CreateProduct();

        product.UpdateDetails("  New Name  ", "  New Description  ", "  New Category  ", 250m);

        Assert.Equal("New Name", product.Name);
        Assert.Equal("New Description", product.Description);
        Assert.Equal("New Category", product.Category);
        Assert.Equal(250m, product.Price);
    }

    /// <summary>
    /// Verifies restock increases available stock.
    /// </summary>
    [Fact]
    public void Restock_WithPositiveQuantity_IncreasesStock()
    {
        Product product = CreateProduct(stockQuantity: 4);

        product.Restock(3);

        Assert.Equal(7, product.StockQuantity);
    }

    /// <summary>
    /// Verifies restock rejects non-positive quantities.
    /// </summary>
    [Fact]
    public void Restock_WithNonPositiveQuantity_ThrowsValidationException()
    {
        Product product = CreateProduct();

        Assert.Throws<ValidationException>(() => product.Restock(0));
    }

    /// <summary>
    /// Verifies stock reduction rejects insufficient inventory.
    /// </summary>
    [Fact]
    public void ReduceStock_WhenInsufficient_ThrowsInsufficientStockException()
    {
        Product product = CreateProduct(stockQuantity: 2);

        Assert.Throws<InsufficientStockException>(() => product.ReduceStock(3));
    }

    /// <summary>
    /// Verifies stock reduction decreases quantity for valid requests.
    /// </summary>
    [Fact]
    public void ReduceStock_WithValidQuantity_DecreasesStock()
    {
        Product product = CreateProduct(stockQuantity: 5);

        product.ReduceStock(2);

        Assert.Equal(3, product.StockQuantity);
    }

    /// <summary>
    /// Verifies product deactivation flips active status.
    /// </summary>
    [Fact]
    public void Deactivate_SetsIsActiveToFalse()
    {
        Product product = CreateProduct();

        product.Deactivate();

        Assert.False(product.IsActive);
    }

    private static Product CreateProduct(
        decimal price = 100m,
        int stockQuantity = 10)
    {
        return new Product(
            Guid.NewGuid(),
            "Laptop",
            "Portable computer",
            "Electronics",
            price,
            stockQuantity);
    }
}
