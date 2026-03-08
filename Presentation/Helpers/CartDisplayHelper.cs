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
        ConsoleTheme.WriteSection("Cart Summary");

        if (items.Count == 0)
        {
            ConsoleTheme.WriteInfo("Your cart is currently empty.");
            return;
        }

        for (int index = 0; index < items.Count; index++)
        {
            CartItem item = items[index];
            Console.WriteLine($"{index + 1}. {item.ProductName}");
            Console.WriteLine($"   Qty: {item.Quantity} | Unit: {item.UnitPrice:C} | Subtotal: {item.LineTotal:C}");
            ConsoleTheme.WriteDivider();
        }

        ConsoleTheme.WriteInfo($"Cart Total: {total:C}");
    }

    /// <summary>
    /// Displays numbered cart items to support index-based selection.
    /// </summary>
    public static void ShowSelectableCart(IReadOnlyList<CartItem> items, decimal total)
    {
        ConsoleTheme.WriteSection("Choose Cart Item");

        if (items.Count == 0)
        {
            ConsoleTheme.WriteInfo("Your cart is currently empty.");
            return;
        }

        for (int index = 0; index < items.Count; index++)
        {
            CartItem item = items[index];
            Console.WriteLine($"{index + 1}. {item.ProductName} | Qty: {item.Quantity} | Unit: {item.UnitPrice:C} | Subtotal: {item.LineTotal:C}");
        }

        ConsoleTheme.WriteInfo($"Cart Total: {total:C}");
        ConsoleTheme.WriteHint("Set quantity to 0 if you want to remove an item.");
    }
}