using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Tests cart-item and order-item line invariants.
/// </summary>
public sealed class LineItemTests
{
    /// <summary>
    /// Verifies cart item constructor rejects non-positive quantity.
    /// </summary>
    [Fact]
    public void CartItemConstructor_WithNonPositiveQuantity_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            _ = new CartItem(Guid.NewGuid(), "Notebook", 20m, 0));
    }

    /// <summary>
    /// Verifies cart item quantity updates reject non-positive values.
    /// </summary>
    [Fact]
    public void CartItemUpdateQuantity_WithNonPositiveQuantity_ThrowsValidationException()
    {
        CartItem item = new(Guid.NewGuid(), "Notebook", 20m, 1);

        Assert.Throws<ValidationException>(() => item.UpdateQuantity(0));
    }

    /// <summary>
    /// Verifies cart line total is quantity multiplied by unit price.
    /// </summary>
    [Fact]
    public void CartItemLineTotal_ReturnsQuantityTimesUnitPrice()
    {
        CartItem item = new(Guid.NewGuid(), "Notebook", 20m, 3);

        Assert.Equal(60m, item.LineTotal);
    }

    /// <summary>
    /// Verifies order item constructor rejects empty product IDs.
    /// </summary>
    [Fact]
    public void OrderItemConstructor_WithEmptyProductId_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            _ = new OrderItem(Guid.Empty, "Laptop", 100m, 1));
    }

    /// <summary>
    /// Verifies order line total is quantity multiplied by unit price.
    /// </summary>
    [Fact]
    public void OrderItemLineTotal_ReturnsQuantityTimesUnitPrice()
    {
        OrderItem item = new(Guid.NewGuid(), "Laptop", 100m, 2);

        Assert.Equal(200m, item.LineTotal);
    }
}
