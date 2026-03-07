using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements aggregate reporting workflows.
/// </summary>
public sealed class ReportService : IReportService
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes the report service.
    /// </summary>
    public ReportService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc />
    public decimal GetTotalRevenue()
    {
        return _orderRepository.GetAll().Sum(order => order.TotalAmount);
    }

    /// <inheritdoc />
    public Dictionary<OrderStatus, int> GetOrdersByStatus()
    {
        return _orderRepository.GetAll()
            .GroupBy(order => order.Status)
            .ToDictionary(group => group.Key, group => group.Count());
    }
}
