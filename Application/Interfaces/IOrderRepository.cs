using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for order data operations.
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Returns all orders for a customer.
    /// </summary>
    List<Order> GetByCustomerId(Guid customerId);
}
