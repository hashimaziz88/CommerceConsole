using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for cart workflows.
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Adds a product to a customer's cart.
    /// </summary>
    void AddToCart(Customer customer, Product product, int quantity);

    /// <summary>
    /// Updates a cart line quantity.
    /// </summary>
    void UpdateCartItem(Customer customer, Guid productId, int quantity);

    /// <summary>
    /// Returns the cart total.
    /// </summary>
    decimal GetCartTotal(Customer customer);
}
