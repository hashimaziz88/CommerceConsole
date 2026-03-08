using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized order rendering helper for menu screens.
/// </summary>
public static class OrderDisplayHelper
{
    /// <summary>
    /// Displays order history in a readable list format.
    /// </summary>
    public static void ShowOrders(string heading, IReadOnlyList<Order> orders)
    {
        ConsoleTheme.WriteSection(heading);

        if (orders.Count == 0)
        {
            ConsoleTheme.WriteInfo("No orders found.");
            return;
        }

        for (int index = 0; index < orders.Count; index++)
        {
            Order order = orders[index];
            Console.WriteLine($"{index + 1}. Placed: {order.CreatedAt:yyyy-MM-dd HH:mm} UTC | Items: {order.Items.Count} | Total: {order.TotalAmount:C}");
            Console.WriteLine($"   Status: {FormatStatus(order.Status)} | Payment: {order.Payment.Status}");
            ConsoleTheme.WriteDivider();
        }
    }

    /// <summary>
    /// Displays numbered orders to support index-based selection.
    /// </summary>
    public static void ShowSelectableOrders(string heading, IReadOnlyList<Order> orders)
    {
        ConsoleTheme.WriteSection(heading);

        if (orders.Count == 0)
        {
            ConsoleTheme.WriteInfo("No orders found.");
            return;
        }

        for (int index = 0; index < orders.Count; index++)
        {
            Order order = orders[index];
            Console.WriteLine($"{index + 1}. {order.CreatedAt:yyyy-MM-dd HH:mm} UTC | Items: {order.Items.Count} | Total: {order.TotalAmount:C} | {FormatStatus(order.Status)}");
        }
    }

    /// <summary>
    /// Displays a tracking-focused view for a single order.
    /// </summary>
    public static void ShowTracking(Order order)
    {
        ConsoleTheme.WriteSection("Order Tracking");
        Console.WriteLine($"Placed: {order.CreatedAt:yyyy-MM-dd HH:mm} UTC");
        Console.WriteLine($"Current Status: {FormatStatus(order.Status)}");
        Console.WriteLine($"Payment Status: {order.Payment.Status}");
        Console.WriteLine($"Items: {order.Items.Count}");
        Console.WriteLine($"Total: {order.TotalAmount:C}");
        ConsoleTheme.WriteHint("Lifecycle: Pending -> Paid -> Processing -> Shipped -> Delivered");
    }

    private static string FormatStatus(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Cancelled => "[CANCELLED]",
            OrderStatus.Delivered => "[DELIVERED]",
            OrderStatus.Shipped => "[SHIPPED]",
            OrderStatus.Processing => "[PROCESSING]",
            OrderStatus.Paid => "[PAID]",
            _ => "[PENDING]"
        };
    }
}