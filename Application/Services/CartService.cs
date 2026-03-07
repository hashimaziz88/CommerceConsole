using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements shopping cart workflows.
/// </summary>
public sealed class CartService : ICartService
{
    /// <inheritdoc />
    public void AddToCart(Customer customer, Product product, int quantity)
    {
        customer.Cart.AddItem(product.Id, product.Name, product.Price, quantity);
    }

    /// <inheritdoc />
    public void UpdateCartItem(Customer customer, Guid productId, int quantity)
    {
        customer.Cart.UpdateQuantity(productId, quantity);
    }

    /// <inheritdoc />
    public decimal GetCartTotal(Customer customer)
    {
        return customer.Cart.CalculateTotal();
    }
}
