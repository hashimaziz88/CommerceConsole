namespace CommerceConsole.Application.Models;

/// <summary>
/// Represents one best-selling product row in a sales report.
/// </summary>
public sealed record ProductSalesReportItem(
    string ProductName,
    int TotalQuantitySold,
    decimal TotalRevenue);
