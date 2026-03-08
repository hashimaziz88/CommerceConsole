using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized product rendering helper for menu screens.
/// </summary>
public static class ProductDisplayHelper
{
    private const int LowStockWarningThreshold = 3;

    /// <summary>
    /// Prints a list of products with a common table-like format.
    /// </summary>
    public static void ShowProducts(string heading, IEnumerable<Product> products)
    {
        ConsoleTheme.WriteSection(heading);

        List<Product> productList = products.ToList();
        if (productList.Count == 0)
        {
            ConsoleTheme.WriteInfo("No products found.");
            return;
        }

        for (int index = 0; index < productList.Count; index++)
        {
            Product product = productList[index];

            Console.WriteLine($"{index + 1}. {product.Name} ({product.Category}) {GetStatusLabel(product)} {GetStockLabel(product.StockQuantity)}");
            Console.WriteLine($"   Price: {product.Price:C} | Stock: {product.StockQuantity} | Rating: {CalculateAverageRating(product):0.00}/5");

            if (!string.IsNullOrWhiteSpace(product.Description))
            {
                Console.WriteLine($"   About: {product.Description}");
            }

            ConsoleTheme.WriteDivider();
        }
    }

    /// <summary>
    /// Prints a numbered list for product selection without exposing internal identifiers.
    /// </summary>
    public static void ShowSelectableProducts(string heading, IReadOnlyList<Product> products)
    {
        ConsoleTheme.WriteSection(heading);

        if (products.Count == 0)
        {
            ConsoleTheme.WriteInfo("No products found.");
            return;
        }

        for (int index = 0; index < products.Count; index++)
        {
            Product product = products[index];
            Console.WriteLine(
                $"{index + 1}. {product.Name} | {product.Category} | {product.Price:C} | Stock: {product.StockQuantity} {GetStockLabel(product.StockQuantity)} | {GetStatusLabel(product)} | Rating: {CalculateAverageRating(product):0.00}/5");
        }
    }

    private static string GetStatusLabel(Product product)
    {
        return product.IsActive ? "[ACTIVE]" : "[INACTIVE]";
    }

    private static string GetStockLabel(int stockQuantity)
    {
        return stockQuantity <= LowStockWarningThreshold ? "[LOW STOCK]" : "[STOCK OK]";
    }

    private static double CalculateAverageRating(Product product)
    {
        if (product.Reviews.Count == 0)
        {
            return 0;
        }

        return product.Reviews.Average(review => review.Rating);
    }
}