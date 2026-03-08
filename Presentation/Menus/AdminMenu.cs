using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
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
            int selection = ConsoleInputHelper.ReadSelection("Select an option: ", 10);

            switch (selection)
            {
                case 1:
                    MenuActionHelper.Execute(AddProduct);
                    break;
                case 2:
                    MenuActionHelper.Execute(UpdateProduct);
                    break;
                case 3:
                    MenuActionHelper.Execute(DeleteProduct);
                    break;
                case 4:
                    MenuActionHelper.Execute(RestockProduct);
                    break;
                case 5:
                    MenuActionHelper.Execute(ViewProducts);
                    break;
                case 6:
                    MenuActionHelper.Execute(ViewLowStockProducts);
                    break;
                case 7:
                    MenuActionHelper.Execute(ViewAllOrders);
                    break;
                case 8:
                    MenuActionHelper.Execute(UpdateOrderStatus);
                    break;
                case 9:
                    MenuActionHelper.Execute(ViewSalesReport);
                    break;
                case 10:
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
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
        decimal price = ConsoleInputHelper.ReadNonNegativeDecimal("Price: ");
        int stock = ConsoleInputHelper.ReadNonNegativeInt("Initial stock quantity: ");

        Product product = _productService.AddProduct(name, description, category, price, stock);
        Console.WriteLine($"Product '{product.Name}' added successfully.");
    }

    private void UpdateProduct()
    {
        Product selectedProduct = SelectProductForAction("=== Select Product To Update ===");
        string name = ConsoleInputHelper.ReadRequiredString("New name: ");
        string description = ConsoleInputHelper.ReadRequiredString("New description: ");
        string category = ConsoleInputHelper.ReadRequiredString("New category: ");
        decimal price = ConsoleInputHelper.ReadNonNegativeDecimal("New price: ");

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
        int quantity = ConsoleInputHelper.ReadPositiveInt("Restock quantity: ");

        _productService.RestockProduct(selectedProduct.Id, quantity);
        Console.WriteLine("Product restocked successfully.");
    }

    private void ViewProducts()
    {
        List<Product> products = _productService.GetAllProducts();
        ProductDisplayHelper.ShowProducts("=== Product Catalog ===", products);
    }

    private void ViewLowStockProducts()
    {
        int threshold = ConsoleInputHelper.ReadNonNegativeInt("Low-stock threshold: ");
        List<Product> products = _productService.GetLowStockProducts(threshold);
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
        int topCount = ConsoleInputHelper.ReadPositiveInt("Top-selling products to show: ");
        int lowStockThreshold = ConsoleInputHelper.ReadNonNegativeInt("Low-stock threshold: ");

        decimal totalRevenue = _reportService.GetTotalRevenue();
        Dictionary<OrderStatus, int> ordersByStatus = _reportService.GetOrdersByStatus();
        IReadOnlyList<ProductSalesReportItem> bestSellingProducts = _reportService.GetBestSellingProducts(topCount);
        IReadOnlyList<LowStockReportItem> lowStockProducts = _reportService.GetLowStockProducts(lowStockThreshold);

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
}