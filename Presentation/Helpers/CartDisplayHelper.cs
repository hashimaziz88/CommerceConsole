using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized cart rendering helper for menu screens.
/// </summary>
public static class CartDisplayHelper
{
    /// <summary>
    /// Displays current cart contents and totals.
    /// </summary>
    public static void ShowCart(IReadOnlyList<CartItem> items, decimal total)
    {
        Console.WriteLine("=== Cart ===");

        if (items.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        foreach (CartItem item in items)
        {
            Console.WriteLine($"Name: {item.ProductName}");
            Console.WriteLine($"Unit Price: {item.UnitPrice:C}");
            Console.WriteLine($"Quantity: {item.Quantity}");
            Console.WriteLine($"Line Total: {item.LineTotal:C}");
            Console.WriteLine(new string('-', 35));
        }

        Console.WriteLine($"Cart Total: {total:C}");
    }

    /// <summary>
    /// Displays numbered cart items to support index-based selection.
    /// </summary>
    public static void ShowSelectableCart(IReadOnlyList<CartItem> items, decimal total)
    {
        Console.WriteLine("=== Select Cart Item ===");

        if (items.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        for (int index = 0; index < items.Count; index++)
        {
            CartItem item = items[index];
            Console.WriteLine($"{index + 1}. {item.ProductName} | Qty: {item.Quantity} | Unit: {item.UnitPrice:C} | Line: {item.LineTotal:C}");
        }

        Console.WriteLine($"Cart Total: {total:C}");
    }
}
