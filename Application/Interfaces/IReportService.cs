using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for reporting workflows.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Returns total revenue from all orders.
    /// </summary>
    decimal GetTotalRevenue();

    /// <summary>
    /// Returns counts of orders grouped by status.
    /// </summary>
    Dictionary<OrderStatus, int> GetOrdersByStatus();
}
