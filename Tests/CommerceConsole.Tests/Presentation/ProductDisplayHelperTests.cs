using CommerceConsole.Domain.Entities;
using CommerceConsole.Presentation.Helpers;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests product list rendering behavior for larger catalogs.
/// </summary>
public sealed class ProductDisplayHelperTests
{
    /// <summary>
    /// Verifies selectable product rendering shows paged headers and global index numbering.
    /// </summary>
    [Fact]
    public void ShowSelectableProducts_WithMultiplePages_ShowsPagedHeadersAndGlobalIndices()
    {
        List<Product> products = Enumerable.Range(1, 7)
            .Select(index => CreateProduct(index))
            .ToList();

        string output = ConsoleTestHarness.RunWithOutput(() =>
            ProductDisplayHelper.ShowSelectableProducts("Select Product", products));

        Assert.Contains("Showing 7 products across 2 page(s).", output);
        Assert.Contains("Page 1/2", output);
        Assert.Contains("Page 2/2", output);
        Assert.Contains("1. Product 1", output);
        Assert.Contains("7. Product 7", output);
    }

    /// <summary>
    /// Verifies an empty product list displays a friendly empty-state message.
    /// </summary>
    [Fact]
    public void ShowProducts_WithNoItems_ShowsEmptyStateMessage()
    {
        string output = ConsoleTestHarness.RunWithOutput(() =>
            ProductDisplayHelper.ShowProducts("Catalog", Array.Empty<Product>()));

        Assert.Contains("No products found.", output);
    }

    private static Product CreateProduct(int index)
    {
        return new Product(
            Guid.NewGuid(),
            $"Product {index}",
            "Demo product",
            "Category",
            10m + index,
            5 + index);
    }
}
