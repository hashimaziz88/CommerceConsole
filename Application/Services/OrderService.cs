using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Starter implementation for order and checkout workflows.
/// </summary>
public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes the order service.
    /// </summary>
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc />
    public Order Checkout(Customer customer)
    {
        throw new NotImplementedException("Checkout flow will be implemented in Issue 5.");
    }

    /// <inheritdoc />
    public List<Order> GetCustomerOrders(Guid customerId)
    {
        return _orderRepository.GetByCustomerId(customerId);
    }

    /// <inheritdoc />
    public List<Order> GetAllOrders()
    {
        return _orderRepository.GetAll();
    }

    /// <inheritdoc />
    public void UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        Order? order = _orderRepository.GetById(orderId);
        if (order is null)
        {
            return;
        }

        order.UpdateStatus(status);
        _orderRepository.Update(order);
    }
}
