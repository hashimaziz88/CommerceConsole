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
        ConsoleTheme.WriteSection("Sales Report Dashboard");
        ConsoleTheme.WriteInfo($"Total Revenue: {totalRevenue:C}");

        ConsoleTheme.WriteSection("Orders By Status");
        foreach (OrderStatus status in Enum.GetValues<OrderStatus>())
        {
            int count = ordersByStatus.TryGetValue(status, out int value) ? value : 0;
            Console.WriteLine($"- {status}: {count}");
        }

        ConsoleTheme.WriteSection("Best-Selling Products");
        if (bestSellers.Count == 0)
        {
            ConsoleTheme.WriteInfo("No sales data available.");
        }
        else
        {
            for (int index = 0; index < bestSellers.Count; index++)
            {
                ProductSalesReportItem item = bestSellers[index];
                Console.WriteLine($"{index + 1}. {item.ProductName} | Qty Sold: {item.TotalQuantitySold} | Revenue: {item.TotalRevenue:C}");
            }
        }

        ConsoleTheme.WriteSection("Low-Stock Products");
        if (lowStockItems.Count == 0)
        {
            ConsoleTheme.WriteInfo("No products at or below threshold.");
        }
        else
        {
            foreach (LowStockReportItem item in lowStockItems)
            {
                string status = item.IsActive ? "ACTIVE" : "INACTIVE";
                string stockTag = item.StockQuantity <= 3 ? "[LOW STOCK]" : "[STOCK OK]";
                Console.WriteLine($"- {item.ProductName} | {item.Category} | Stock: {item.StockQuantity} {stockTag} | Price: {item.UnitPrice:C} | {status}");
            }
        }
    }

    /// <summary>
    /// Displays heuristic admin insights.
    /// </summary>
    public static void ShowInsights(IReadOnlyList<string> insights)
    {
        ConsoleTheme.WriteSection("Smart Insights");

        if (insights.Count == 0)
        {
            ConsoleTheme.WriteInfo("No insights available right now.");
            return;
        }

        for (int index = 0; index < insights.Count; index++)
        {
            Console.WriteLine($"{index + 1}. {insights[index]}");
        }
    }
}
