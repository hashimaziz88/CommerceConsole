using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents an immutable item snapshot within an order.
/// </summary>
public sealed class OrderItem
{
    /// <summary>
    /// Initializes an order item snapshot.
    /// </summary>
    public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
        {
            throw new ValidationException("Product ID must be valid.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ValidationException("Product name is required.");
        }

        if (unitPrice < 0)
        {
            throw new ValidationException("Unit price cannot be negative.");
        }

        if (quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    /// <summary>
    /// Gets product ID snapshot.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets product name snapshot.
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets unit price snapshot.
    /// </summary>
    public decimal UnitPrice { get; }

    /// <summary>
    /// Gets purchased quantity.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets line total.
    /// </summary>
    public decimal LineTotal => UnitPrice * Quantity;
}
