namespace CommerceConsole.Infrastructure.Repositories.Models;

/// <summary>
/// JSON persistence model for products.
/// </summary>
internal sealed class ProductRecord
{
    /// <summary>
    /// Gets or sets product identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets stock quantity.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets active state.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets review records.
    /// </summary>
    public List<ProductReviewRecord> Reviews { get; set; } = new();
}
