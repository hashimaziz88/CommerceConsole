using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for order and checkout workflows.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Attempts checkout for a customer cart.
    /// </summary>
    Order Checkout(Customer customer);

    /// <summary>
    /// Returns all orders for the provided customer.
    /// </summary>
    List<Order> GetCustomerOrders(Guid customerId);

    /// <summary>
    /// Returns every order for admin use.
    /// </summary>
    List<Order> GetAllOrders();

    /// <summary>
    /// Updates one order status.
    /// </summary>
    void UpdateOrderStatus(Guid orderId, OrderStatus status);
}
