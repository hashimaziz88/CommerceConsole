using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Administrator catalog, order, and reporting management menu.
/// </summary>
public sealed class AdminMenu
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IReportService _reportService;

    /// <summary>
    /// Initializes the administrator menu.
    /// </summary>
    public AdminMenu(IProductService productService, IOrderService orderService, IReportService reportService)
    {
        _productService = productService;
        _orderService = orderService;
        _reportService = reportService;
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
                    ViewAllOrders();
                    break;
                case "8":
                    ExecuteAction(UpdateOrderStatus);
                    break;
                case "9":
                    ExecuteAction(ViewSalesReport);
                    break;
                case "10":
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter 1 through 10.");
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
        Console.WriteLine("7. View All Orders");
        Console.WriteLine("8. Update Order Status");
        Console.WriteLine("9. View Sales Report");
        Console.WriteLine("10. Logout");
    }

    private void AddProduct()
    {
        string name = ConsoleInputHelper.ReadRequiredString("Name: ");
        string description = ConsoleInputHelper.ReadRequiredString("Description: ");
        string category = ConsoleInputHelper.ReadRequiredString("Category: ");
        decimal price = ConsoleInputHelper.ReadDecimal("Price: ");
        int stock = ConsoleInputHelper.ReadInt("Initial stock quantity: ");

        var product = _productService.AddProduct(name, description, category, price, stock);
        Console.WriteLine($"Product '{product.Name}' added successfully.");
    }

    private void UpdateProduct()
    {
        Product selectedProduct = SelectProductForAction("=== Select Product To Update ===");
        string name = ConsoleInputHelper.ReadRequiredString("New name: ");
        string description = ConsoleInputHelper.ReadRequiredString("New description: ");
        string category = ConsoleInputHelper.ReadRequiredString("New category: ");
        decimal price = ConsoleInputHelper.ReadDecimal("New price: ");

        _productService.UpdateProduct(selectedProduct.Id, name, description, category, price);
        Console.WriteLine("Product updated successfully.");
    }

    private void DeleteProduct()
    {
        Product selectedProduct = SelectProductForAction("=== Select Product To Delete ===");
        _productService.DeleteProduct(selectedProduct.Id);
        Console.WriteLine("Product deleted successfully.");
    }

    private void RestockProduct()
    {
        Product selectedProduct = SelectProductForAction("=== Select Product To Restock ===");
        int quantity = ConsoleInputHelper.ReadInt("Restock quantity: ");

        _productService.RestockProduct(selectedProduct.Id, quantity);
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

    private void ViewAllOrders()
    {
        List<Order> orders = _orderService.GetAllOrders();
        OrderDisplayHelper.ShowOrders("=== All Orders ===", orders);
    }

    private void UpdateOrderStatus()
    {
        List<Order> orders = _orderService.GetAllOrders();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders available.");
            return;
        }

        OrderDisplayHelper.ShowSelectableOrders("=== Select Order To Update ===", orders);
        int orderSelection = ConsoleInputHelper.ReadSelection("Choose order number: ", orders.Count);
        Order selectedOrder = orders[orderSelection - 1];

        IReadOnlyList<OrderStatus> allowedTransitions = _orderService.GetAllowedTransitions(selectedOrder.Status);
        if (allowedTransitions.Count == 0)
        {
            Console.WriteLine("This order is in a terminal state and cannot transition further.");
            return;
        }

        Console.WriteLine("=== Allowed Next Statuses ===");
        for (int index = 0; index < allowedTransitions.Count; index++)
        {
            Console.WriteLine($"{index + 1}. {allowedTransitions[index]}");
        }

        int statusSelection = ConsoleInputHelper.ReadSelection("Choose next status: ", allowedTransitions.Count);
        OrderStatus selectedStatus = allowedTransitions[statusSelection - 1];

        _orderService.UpdateOrderStatus(selectedOrder.Id, selectedStatus);
        Console.WriteLine($"Order status updated to {selectedStatus}.");
    }

    private void ViewSalesReport()
    {
        int topCount = ConsoleInputHelper.ReadInt("Top-selling products to show: ");
        int lowStockThreshold = ConsoleInputHelper.ReadInt("Low-stock threshold: ");

        decimal totalRevenue = _reportService.GetTotalRevenue();
        Dictionary<OrderStatus, int> ordersByStatus = _reportService.GetOrdersByStatus();
        var bestSellingProducts = _reportService.GetBestSellingProducts(topCount);
        var lowStockProducts = _reportService.GetLowStockProducts(lowStockThreshold);

        ReportDisplayHelper.ShowSalesReport(totalRevenue, ordersByStatus, bestSellingProducts, lowStockProducts);
    }

    private Product SelectProductForAction(string heading)
    {
        List<Product> products = _productService.GetAllProducts();
        if (products.Count == 0)
        {
            throw new NotFoundException("No products are available.");
        }

        ProductDisplayHelper.ShowSelectableProducts(heading, products);
        int selection = ConsoleInputHelper.ReadSelection("Choose product number: ", products.Count);
        return products[selection - 1];
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
