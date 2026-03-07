using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Infrastructure.Repositories;

/// <summary>
/// In-memory order repository used for bootstrap and testing.
/// </summary>
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();

    /// <inheritdoc />
    public List<Order> GetAll()
    {
        return _orders.ToList();
    }

    /// <inheritdoc />
    public Order? GetById(Guid id)
    {
        return _orders.FirstOrDefault(order => order.Id == id);
    }

    /// <inheritdoc />
    public List<Order> GetByCustomerId(Guid customerId)
    {
        return _orders.Where(order => order.CustomerId == customerId).ToList();
    }

    /// <inheritdoc />
    public void Add(Order entity)
    {
        _orders.Add(entity);
    }

    /// <inheritdoc />
    public void Update(Order entity)
    {
        int index = _orders.FindIndex(order => order.Id == entity.Id);
        if (index >= 0)
        {
            _orders[index] = entity;
        }
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        Order? existing = GetById(id);
        if (existing is not null)
        {
            _orders.Remove(existing);
        }
    }
}
