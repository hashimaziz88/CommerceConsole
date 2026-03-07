namespace CommerceConsole.Application.Models;

/// <summary>
/// Represents one low-stock product row in a stock report.
/// </summary>
public sealed record LowStockReportItem(
    string ProductName,
    string Category,
    int StockQuantity,
    decimal UnitPrice,
    bool IsActive);
