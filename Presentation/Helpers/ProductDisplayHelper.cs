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
            Console.WriteLine($"ID: {product.Id}");
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine($"Price: {product.Price:C}");
            Console.WriteLine($"Stock: {product.StockQuantity}");
            Console.WriteLine($"Active: {product.IsActive}");
            Console.WriteLine($"Average Rating: {CalculateAverageRating(product):0.00}");
            Console.WriteLine(new string('-', 40));
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
