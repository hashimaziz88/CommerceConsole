using CommerceConsole.Application.Models;
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

    /// <summary>
    /// Returns top-selling products by quantity sold.
    /// </summary>
    List<ProductSalesReportItem> GetBestSellingProducts(int topCount);

    /// <summary>
    /// Returns products at or below a low-stock threshold.
    /// </summary>
    List<LowStockReportItem> GetLowStockProducts(int threshold);
}
