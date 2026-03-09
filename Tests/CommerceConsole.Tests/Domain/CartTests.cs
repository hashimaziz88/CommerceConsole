using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Tests shopping cart behavior and invariants.
/// </summary>
public sealed class CartTests
{
    /// <summary>
    /// Verifies cart constructor requires a valid customer ID.
    /// </summary>
    [Fact]
    public void Constructor_WithEmptyCustomerId_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            _ = new Cart(Guid.Empty));
    }

    /// <summary>
    /// Verifies adding the same product merges quantity instead of creating duplicates.
    /// </summary>
    [Fact]
    public void AddItem_WithExistingProduct_MergesQuantity()
    {
        Cart cart = CreateCart();
        Guid productId = Guid.NewGuid();

        cart.AddItem(productId, "Notebook", 25m, 1);
        cart.AddItem(productId, "Notebook", 25m, 2);

        Assert.Single(cart.Items);
        Assert.Equal(3, cart.Items[0].Quantity);
    }

    /// <summary>
    /// Verifies zero-quantity update removes the line item.
    /// </summary>
    [Fact]
    public void UpdateQuantity_WithZero_RemovesItem()
    {
        Cart cart = CreateCart();
        Guid productId = Guid.NewGuid();

        cart.AddItem(productId, "Notebook", 25m, 2);

        cart.UpdateQuantity(productId, 0);

        Assert.Empty(cart.Items);
    }

    /// <summary>
    /// Verifies missing line updates throw not-found exceptions.
    /// </summary>
    [Fact]
    public void UpdateQuantity_WithMissingItem_ThrowsNotFoundException()
    {
        Cart cart = CreateCart();

        Assert.Throws<NotFoundException>(() =>
            cart.UpdateQuantity(Guid.NewGuid(), 2));
    }

    /// <summary>
    /// Verifies explicit removal removes the matching item.
    /// </summary>
    [Fact]
    public void RemoveItem_WithExistingItem_RemovesItem()
    {
        Cart cart = CreateCart();
        Guid productId = Guid.NewGuid();

        cart.AddItem(productId, "Notebook", 25m, 2);

        cart.RemoveItem(productId);

        Assert.Empty(cart.Items);
    }

    /// <summary>
    /// Verifies clear removes all items in cart.
    /// </summary>
    [Fact]
    public void Clear_RemovesAllItems()
    {
        Cart cart = CreateCart();

        cart.AddItem(Guid.NewGuid(), "Notebook", 25m, 1);
        cart.AddItem(Guid.NewGuid(), "Pen", 5m, 2);

        cart.Clear();

        Assert.Empty(cart.Items);
    }

    /// <summary>
    /// Verifies cart total is derived from all line totals.
    /// </summary>
    [Fact]
    public void CalculateTotal_WithMultipleItems_ReturnsLineSum()
    {
        Cart cart = CreateCart();

        cart.AddItem(Guid.NewGuid(), "Notebook", 25m, 2);
        cart.AddItem(Guid.NewGuid(), "Pen", 5m, 3);

        decimal total = cart.CalculateTotal();

        Assert.Equal(65m, total);
    }

    private static Cart CreateCart()
    {
        return new Cart(Guid.NewGuid());
    }
}
