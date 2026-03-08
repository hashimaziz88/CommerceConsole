using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized product rendering helper for menu screens.
/// </summary>
public static class ProductDisplayHelper
{
    private const int LowStockWarningThreshold = 3;
    private const int ItemsPerPage = 6;

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

        int totalPages = GetTotalPages(productList.Count);
        ConsoleTheme.WriteInfo($"Showing {productList.Count} products across {totalPages} page(s).");

        for (int page = 0; page < totalPages; page++)
        {
            WritePageHeader(page + 1, totalPages);

            int startIndex = page * ItemsPerPage;
            int endExclusive = Math.Min(startIndex + ItemsPerPage, productList.Count);

            for (int index = startIndex; index < endExclusive; index++)
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

        int totalPages = GetTotalPages(products.Count);
        ConsoleTheme.WriteInfo($"Showing {products.Count} products across {totalPages} page(s).");

        for (int page = 0; page < totalPages; page++)
        {
            WritePageHeader(page + 1, totalPages);

            int startIndex = page * ItemsPerPage;
            int endExclusive = Math.Min(startIndex + ItemsPerPage, products.Count);

            for (int index = startIndex; index < endExclusive; index++)
            {
                Product product = products[index];
                Console.WriteLine(
                    $"{index + 1}. {product.Name} | {product.Category} | {product.Price:C} | Stock: {product.StockQuantity} {GetStockLabel(product.StockQuantity)} | {GetStatusLabel(product)} | Rating: {CalculateAverageRating(product):0.00}/5");
            }

            ConsoleTheme.WriteDivider();
        }

        ConsoleTheme.WriteHint("Use the product number shown on the left to choose an item.");
    }

    /// <summary>
    /// Prints customer recommendation rows with reason text.
    /// </summary>
    public static void ShowRecommendations(string heading, IReadOnlyList<ProductRecommendationItem> recommendations)
    {
        ConsoleTheme.WriteSection(heading);

        if (recommendations.Count == 0)
        {
            ConsoleTheme.WriteInfo("No recommendation candidates were found right now.");
            return;
        }

        int totalPages = GetTotalPages(recommendations.Count);
        ConsoleTheme.WriteInfo($"Showing {recommendations.Count} recommendation(s) across {totalPages} page(s).");

        for (int page = 0; page < totalPages; page++)
        {
            WritePageHeader(page + 1, totalPages);

            int startIndex = page * ItemsPerPage;
            int endExclusive = Math.Min(startIndex + ItemsPerPage, recommendations.Count);

            for (int index = startIndex; index < endExclusive; index++)
            {
                ProductRecommendationItem recommendation = recommendations[index];
                Console.WriteLine(
                    $"{index + 1}. {recommendation.ProductName} ({recommendation.Category}) | Price: {recommendation.Price:C} | Rating: {recommendation.AverageRating:0.00}/5");
                Console.WriteLine($"   Why: {recommendation.Reason}");
                ConsoleTheme.WriteDivider();
            }
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

    private static int GetTotalPages(int itemCount)
    {
        return (int)Math.Ceiling(itemCount / (double)ItemsPerPage);
    }

    private static void WritePageHeader(int pageNumber, int totalPages)
    {
        ConsoleTheme.WriteInfo($"Page {pageNumber}/{totalPages}");
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
