using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Administrator catalog management menu.
/// </summary>
public sealed class AdminMenu
{
    private readonly IProductService _productService;

    /// <summary>
    /// Initializes the administrator menu.
    /// </summary>
    public AdminMenu(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Runs the administrator menu loop.
    /// </summary>
    public void Run(ISessionContext sessionContext)
    {
        if (sessionContext.CurrentUser?.Role != UserRole.Administrator)
        {
            Console.WriteLine("Access denied. Administrator login required.");
            return;
        }

        bool done = false;
        while (!done)
        {
            ShowMenuOptions();
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ExecuteAction(AddProduct);
                    break;
                case "2":
                    ExecuteAction(UpdateProduct);
                    break;
                case "3":
                    ExecuteAction(DeleteProduct);
                    break;
                case "4":
                    ExecuteAction(RestockProduct);
                    break;
                case "5":
                    ViewProducts();
                    break;
                case "6":
                    ExecuteAction(ViewLowStockProducts);
                    break;
                case "7":
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter 1 through 7.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void ShowMenuOptions()
    {
        Console.WriteLine("=== Administrator Menu ===");
        Console.WriteLine("1. Add Product");
        Console.WriteLine("2. Update Product");
        Console.WriteLine("3. Delete Product");
        Console.WriteLine("4. Restock Product");
        Console.WriteLine("5. View Products");
        Console.WriteLine("6. View Low Stock Products");
        Console.WriteLine("7. Logout");
    }

    private void AddProduct()
    {
        string name = ConsoleInputHelper.ReadRequiredString("Name: ");
        string description = ConsoleInputHelper.ReadRequiredString("Description: ");
        string category = ConsoleInputHelper.ReadRequiredString("Category: ");
        decimal price = ConsoleInputHelper.ReadDecimal("Price: ");
        int stock = ConsoleInputHelper.ReadInt("Initial stock quantity: ");

        var product = _productService.AddProduct(name, description, category, price, stock);
        Console.WriteLine($"Product added successfully with ID: {product.Id}");
    }

    private void UpdateProduct()
    {
        Guid id = ConsoleInputHelper.ReadGuid("Product ID: ");
        string name = ConsoleInputHelper.ReadRequiredString("New name: ");
        string description = ConsoleInputHelper.ReadRequiredString("New description: ");
        string category = ConsoleInputHelper.ReadRequiredString("New category: ");
        decimal price = ConsoleInputHelper.ReadDecimal("New price: ");

        _productService.UpdateProduct(id, name, description, category, price);
        Console.WriteLine("Product updated successfully.");
    }

    private void DeleteProduct()
    {
        Guid id = ConsoleInputHelper.ReadGuid("Product ID to delete: ");
        _productService.DeleteProduct(id);
        Console.WriteLine("Product deleted successfully.");
    }

    private void RestockProduct()
    {
        Guid id = ConsoleInputHelper.ReadGuid("Product ID to restock: ");
        int quantity = ConsoleInputHelper.ReadInt("Restock quantity: ");

        _productService.RestockProduct(id, quantity);
        Console.WriteLine("Product restocked successfully.");
    }

    private void ViewProducts()
    {
        var products = _productService.GetAllProducts();
        ProductDisplayHelper.ShowProducts("=== Product Catalog ===", products);
    }

    private void ViewLowStockProducts()
    {
        int threshold = ConsoleInputHelper.ReadInt("Low-stock threshold: ");
        var products = _productService.GetLowStockProducts(threshold);
        ProductDisplayHelper.ShowProducts($"=== Low Stock Products (<= {threshold}) ===", products);
    }

    private static void ExecuteAction(Action action)
    {
        try
        {
            action();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Not found: {ex.Message}");
        }
    }
}
