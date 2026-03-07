using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents a sellable catalog product.
/// </summary>
public sealed class Product
{
    /// <summary>
    /// Initializes a product.
    /// </summary>
    public Product(Guid id, string name, string description, string category, decimal price, int stockQuantity)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("Product ID must be valid.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Product name is required.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ValidationException("Product category is required.");
        }

        if (price < 0)
        {
            throw new ValidationException("Product price cannot be negative.");
        }

        if (stockQuantity < 0)
        {
            throw new ValidationException("Stock quantity cannot be negative.");
        }

        Id = id;
        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = true;
        Reviews = new List<Review>();
    }

    /// <summary>
    /// Gets product ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets product name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets product description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets product category.
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// Gets current unit price.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets available stock quantity.
    /// </summary>
    public int StockQuantity { get; private set; }

    /// <summary>
    /// Gets whether the product is visible to customers.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets customer reviews for the product.
    /// </summary>
    public List<Review> Reviews { get; }

    /// <summary>
    /// Updates product details.
    /// </summary>
    public void UpdateDetails(string name, string description, string category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Product name is required.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ValidationException("Product category is required.");
        }

        if (price < 0)
        {
            throw new ValidationException("Product price cannot be negative.");
        }

        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        Price = price;
    }

    /// <summary>
    /// Increases stock by the provided quantity.
    /// </summary>
    public void Restock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ValidationException("Restock quantity must be greater than zero.");
        }

        StockQuantity += quantity;
    }

    /// <summary>
    /// Reduces stock by the provided quantity.
    /// </summary>
    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        if (StockQuantity < quantity)
        {
            throw new InsufficientStockException("Insufficient stock available.");
        }

        StockQuantity -= quantity;
    }

    /// <summary>
    /// Marks the product as inactive.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}
