using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Models;

/// <summary>
/// Represents a complete sales report snapshot ready for exporting.
/// </summary>
public sealed record SalesReportSnapshot(
    DateTime GeneratedAtUtc,
    decimal TotalRevenue,
    IReadOnlyDictionary<OrderStatus, int> OrdersByStatus,
    IReadOnlyList<ProductSalesReportItem> BestSellingProducts,
    IReadOnlyList<LowStockReportItem> LowStockProducts);