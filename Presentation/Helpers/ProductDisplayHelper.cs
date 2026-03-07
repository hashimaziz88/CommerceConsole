using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized product rendering helper for menu screens.
/// </summary>
public static class ProductDisplayHelper
{
    /// <summary>
    /// Prints a list of products with a common table-like format.
    /// </summary>
    public static void ShowProducts(string heading, IEnumerable<Product> products)
    {
        Console.WriteLine(heading);
        List<Product> productList = products.ToList();
        if (productList.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (Product product in productList)
        {
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine($"Price: {product.Price:C}");
            Console.WriteLine($"Stock: {product.StockQuantity}");
            Console.WriteLine($"Status: {(product.IsActive ? "Active" : "Inactive")}");
            Console.WriteLine($"Average Rating: {CalculateAverageRating(product):0.00}");
            Console.WriteLine(new string('-', 40));
        }
    }

    /// <summary>
    /// Prints a numbered list for product selection without exposing internal identifiers.
    /// </summary>
    public static void ShowSelectableProducts(string heading, IReadOnlyList<Product> products)
    {
        Console.WriteLine(heading);
        if (products.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        for (int index = 0; index < products.Count; index++)
        {
            Product product = products[index];
            Console.WriteLine(
                $"{index + 1}. {product.Name} | {product.Category} | {product.Price:C} | Stock: {product.StockQuantity} | Status: {(product.IsActive ? "Active" : "Inactive")} | Rating: {CalculateAverageRating(product):0.00}");
        }
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
