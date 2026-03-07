using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents a customer's active shopping cart.
/// </summary>
public sealed class Cart
{
    private readonly List<CartItem> _items;

    /// <summary>
    /// Initializes a cart.
    /// </summary>
    public Cart(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            throw new ValidationException("Customer ID must be valid.");
        }

        CustomerId = customerId;
        _items = new List<CartItem>();
    }

    /// <summary>
    /// Gets the owning customer ID.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets read-only cart items.
    /// </summary>
    public IReadOnlyList<CartItem> Items => _items;

    /// <summary>
    /// Adds an item or increases quantity if it exists.
    /// </summary>
    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        CartItem? existing = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is null)
        {
            _items.Add(new CartItem(productId, productName, unitPrice, quantity));
            return;
        }

        existing.UpdateQuantity(existing.Quantity + quantity);
    }

    /// <summary>
    /// Updates quantity for a specific product.
    /// </summary>
    public void UpdateQuantity(Guid productId, int quantity)
    {
        CartItem? item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            throw new NotFoundException("Cart item was not found.");
        }

        if (quantity <= 0)
        {
            _items.Remove(item);
            return;
        }

        item.UpdateQuantity(quantity);
    }

    /// <summary>
    /// Removes an item from the cart.
    /// </summary>
    public void RemoveItem(Guid productId)
    {
        CartItem? item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            _items.Remove(item);
        }
    }

    /// <summary>
    /// Clears all cart items.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }

    /// <summary>
    /// Calculates the full cart total.
    /// </summary>
    public decimal CalculateTotal()
    {
        return _items.Sum(i => i.LineTotal);
    }
}
