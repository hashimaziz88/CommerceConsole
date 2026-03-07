using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Infrastructure.Persistence;
using CommerceConsole.Infrastructure.Repositories.Models;

namespace CommerceConsole.Infrastructure.Repositories;

/// <summary>
/// In-memory order repository with JSON persistence.
/// </summary>
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private const string FileName = "orders.json";

    private readonly JsonFileStore _fileStore;
    private readonly List<Order> _orders;

    /// <summary>
    /// Initializes the order repository.
    /// </summary>
    public InMemoryOrderRepository(string? dataDirectory = null)
    {
        _fileStore = new JsonFileStore(dataDirectory);
        _orders = LoadOrders();
    }

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
        Persist();
    }

    /// <inheritdoc />
    public void Update(Order entity)
    {
        int index = _orders.FindIndex(order => order.Id == entity.Id);
        if (index >= 0)
        {
            _orders[index] = entity;
            Persist();
        }
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        Order? existing = GetById(id);
        if (existing is not null)
        {
            _orders.Remove(existing);
            Persist();
        }
    }

    private List<Order> LoadOrders()
    {
        List<OrderRecord> records = _fileStore.LoadList<OrderRecord>(FileName);
        return records.Select(ToDomain).ToList();
    }

    private void Persist()
    {
        List<OrderRecord> records = _orders.Select(FromDomain).ToList();
        _fileStore.SaveList(FileName, records);
    }

    private static Order ToDomain(OrderRecord record)
    {
        List<OrderItem> items = record.Items.Select(item =>
            new OrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity)).ToList();

        Payment payment = new(
            record.Payment.Id,
            record.Payment.OrderId,
            record.Payment.Amount,
            record.Payment.Method);

        if (record.Payment.Status == PaymentStatus.Completed)
        {
            payment.MarkCompleted();
        }
        else if (record.Payment.Status == PaymentStatus.Failed)
        {
            payment.MarkFailed();
        }

        Order order = new(record.Id, record.CustomerId, items, payment);
        order.UpdateStatus(record.Status);

        return order;
    }

    private static OrderRecord FromDomain(Order order)
    {
        return new OrderRecord
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            Items = order.Items.Select(item => new OrderItemRecord
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            }).ToList(),
            Payment = new PaymentRecord
            {
                Id = order.Payment.Id,
                OrderId = order.Payment.OrderId,
                Amount = order.Payment.Amount,
                Method = order.Payment.Method,
                Status = order.Payment.Status
            }
        };
    }
}
