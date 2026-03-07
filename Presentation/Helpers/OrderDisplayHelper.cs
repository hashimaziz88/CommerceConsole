using CommerceConsole.Domain.Entities;

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
        Console.WriteLine(heading);

        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        foreach (Order order in orders)
        {
            Console.WriteLine($"Placed: {order.CreatedAt:yyyy-MM-dd HH:mm} UTC");
            Console.WriteLine($"Items: {order.Items.Count}");
            Console.WriteLine($"Total: {order.TotalAmount:C}");
            Console.WriteLine($"Order Status: {order.Status}");
            Console.WriteLine($"Payment Status: {order.Payment.Status}");
            Console.WriteLine(new string('-', 40));
        }
    }

    /// <summary>
    /// Displays numbered orders to support index-based selection.
    /// </summary>
    public static void ShowSelectableOrders(string heading, IReadOnlyList<Order> orders)
    {
        Console.WriteLine(heading);

        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        for (int index = 0; index < orders.Count; index++)
        {
            Order order = orders[index];
            Console.WriteLine(
                $"{index + 1}. {order.CreatedAt:yyyy-MM-dd HH:mm} UTC | Items: {order.Items.Count} | Total: {order.TotalAmount:C} | Status: {order.Status} | Payment: {order.Payment.Status}");
        }
    }

    /// <summary>
    /// Displays a tracking-focused view for a single order.
    /// </summary>
    public static void ShowTracking(Order order)
    {
        Console.WriteLine("=== Order Tracking ===");
        Console.WriteLine($"Placed: {order.CreatedAt:yyyy-MM-dd HH:mm} UTC");
        Console.WriteLine($"Current Status: {order.Status}");
        Console.WriteLine($"Payment Status: {order.Payment.Status}");
        Console.WriteLine($"Items: {order.Items.Count}");
        Console.WriteLine($"Total: {order.TotalAmount:C}");
    }
}
