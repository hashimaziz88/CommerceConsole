using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized reporting rendering helper for menu screens.
/// </summary>
public static class ReportDisplayHelper
{
    /// <summary>
    /// Displays the full sales report output.
    /// </summary>
    public static void ShowSalesReport(
        decimal totalRevenue,
        IReadOnlyDictionary<OrderStatus, int> ordersByStatus,
        IReadOnlyList<ProductSalesReportItem> bestSellers,
        IReadOnlyList<LowStockReportItem> lowStockItems)
    {
        Console.WriteLine("=== Sales Report ===");
        Console.WriteLine($"Total Revenue: {totalRevenue:C}");
        Console.WriteLine();

        Console.WriteLine("Orders By Status:");
        foreach (OrderStatus status in Enum.GetValues<OrderStatus>())
        {
            int count = ordersByStatus.TryGetValue(status, out int value) ? value : 0;
            Console.WriteLine($"- {status}: {count}");
        }

        Console.WriteLine();
        Console.WriteLine("Best-Selling Products:");
        if (bestSellers.Count == 0)
        {
            Console.WriteLine("No sales data available.");
        }
        else
        {
            for (int index = 0; index < bestSellers.Count; index++)
            {
                ProductSalesReportItem item = bestSellers[index];
                Console.WriteLine(
                    $"{index + 1}. {item.ProductName} | Qty Sold: {item.TotalQuantitySold} | Revenue: {item.TotalRevenue:C}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Low-Stock Products:");
        if (lowStockItems.Count == 0)
        {
            Console.WriteLine("No products at or below threshold.");
        }
        else
        {
            foreach (LowStockReportItem item in lowStockItems)
            {
                Console.WriteLine(
                    $"- {item.ProductName} | {item.Category} | Stock: {item.StockQuantity} | Price: {item.UnitPrice:C} | Status: {(item.IsActive ? "Active" : "Inactive")}");
            }
        }
    }
}
