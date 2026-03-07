using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Customer catalog menu with browse and search operations.
/// </summary>
public sealed class CustomerMenu
{
    private readonly IProductService _productService;

    /// <summary>
    /// Initializes the customer menu.
    /// </summary>
    public CustomerMenu(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Runs the customer menu loop.
    /// </summary>
    public void Run(ISessionContext sessionContext)
    {
        if (sessionContext.CurrentUser?.Role != UserRole.Customer)
        {
            Console.WriteLine("Access denied. Customer login required.");
            return;
        }

        bool done = false;
        while (!done)
        {
            Console.WriteLine("=== Customer Menu ===");
            Console.WriteLine("1. Browse Active Products");
            Console.WriteLine("2. Search Products");
            Console.WriteLine("3. Logout");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    BrowseProducts();
                    break;
                case "2":
                    SearchProducts();
                    break;
                case "3":
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter 1, 2, or 3.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private void BrowseProducts()
    {
        var products = _productService.GetActiveProducts();
        ProductDisplayHelper.ShowProducts("=== Active Products ===", products);
    }

    private void SearchProducts()
    {
        string term = ConsoleInputHelper.ReadRequiredString("Search term (name/category): ");
        var products = _productService.SearchProducts(term);
        ProductDisplayHelper.ShowProducts($"=== Search Results for '{term}' ===", products);
    }
}
