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
    private static readonly IReadOnlyList<string> MenuOptions = new List<string>
    {
        "Catalog Management",
        "1. Add Product",
        "2. Update Product",
        "3. Delete Product",
        "4. Restock Product",
        "5. View Products",
        "6. View Low Stock Products",
        string.Empty,
        "Order Operations",
        "7. View All Orders",
        "8. Update Order Status",
        string.Empty,
        "Reporting",
        "9. View Sales Report",
        "10. View Smart Insights (Bonus)",
        "11. Export Sales Report PDF (Bonus)",
        string.Empty,
        "Session",
        "12. Logout to Main Menu"
    };

    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IReportService _reportService;
    private readonly IReportExportService _reportExportService;
    private readonly IInsightsService _insightsService;

    /// <summary>
    /// Initializes the administrator menu.
    /// </summary>
    public AdminMenu(
        IProductService productService,
        IOrderService orderService,
        IReportService reportService,
        IReportExportService reportExportService,
        IInsightsService insightsService)
    {
        _productService = productService;
        _orderService = orderService;
        _reportService = reportService;
        _reportExportService = reportExportService;
        _insightsService = insightsService;
    }

    /// <summary>
    /// Runs the administrator menu loop.
    /// </summary>
    public void Run(ISessionContext sessionContext)
    {
        if (sessionContext.CurrentUser?.Role != UserRole.Administrator)
        {
            ConsoleTheme.WriteError("Access denied. Administrator login required.");
            return;
        }

        bool done = false;
        while (!done)
        {
            ShowMenuOptions(sessionContext.CurrentUser.FullName);
            int selection = ConsoleInputHelper.ReadSelection("Choose option (1-12): ", 12);

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
                    MenuActionHelper.Execute(ViewSmartInsights);
                    break;
                case 11:
                    MenuActionHelper.Execute(ExportSalesReportPdf);
                    break;
                case 12:
                    sessionContext.SignOut();
                    done = true;
                    ConsoleTheme.WriteInfo("Administrator session ended. Returning to main menu.");
                    break;
            }

            if (!done)
            {
                ConsoleTheme.Pause();
            }
        }
    }

    private static void ShowMenuOptions(string adminName)
    {
        MenuFrameRenderer.ShowMenu(
            "Administrator Workspace",
            "Home > Administrator",
            MenuOptions,
            $"Signed in as {adminName}. Use grouped actions for catalog, orders, and reporting.");
    }

    private void AddProduct()
    {
        ConsoleTheme.WriteSection("Admin > Catalog > Add Product");

        string name = ConsoleInputHelper.ReadRequiredString("Name: ");
        string description = ConsoleInputHelper.ReadRequiredString("Description: ");
        string category = ConsoleInputHelper.ReadRequiredString("Category: ");
        decimal price = ConsoleInputHelper.ReadNonNegativeDecimal("Price: ");
        int stock = ConsoleInputHelper.ReadNonNegativeInt("Initial stock quantity: ");

        Product product = _productService.AddProduct(name, description, category, price, stock);
        ConsoleTheme.WriteSuccess($"Product '{product.Name}' added successfully.");
    }

    private void UpdateProduct()
    {
        ConsoleTheme.WriteSection("Admin > Catalog > Update Product");

        Product selectedProduct = SelectProductForAction("Select Product To Update");
        string name = ConsoleInputHelper.ReadRequiredString("New name: ");
        string description = ConsoleInputHelper.ReadRequiredString("New description: ");
        string category = ConsoleInputHelper.ReadRequiredString("New category: ");
        decimal price = ConsoleInputHelper.ReadNonNegativeDecimal("New price: ");

        _productService.UpdateProduct(selectedProduct.Id, name, description, category, price);
        ConsoleTheme.WriteSuccess("Product updated successfully.");
    }

    private void DeleteProduct()
    {
        ConsoleTheme.WriteSection("Admin > Catalog > Delete Product");

        Product selectedProduct = SelectProductForAction("Select Product To Delete");

        if (!ConfirmationPrompt.AskYesNo($"Delete '{selectedProduct.Name}' from catalog?", false))
        {
            ConsoleTheme.WriteInfo("Delete cancelled.");
            return;
        }

        _productService.DeleteProduct(selectedProduct.Id);
        ConsoleTheme.WriteSuccess("Product deleted successfully.");
    }

    private void RestockProduct()
    {
        ConsoleTheme.WriteSection("Admin > Catalog > Restock Product");

        Product selectedProduct = SelectProductForAction("Select Product To Restock");
        int quantity = ConsoleInputHelper.ReadPositiveInt("Restock quantity: ");

        _productService.RestockProduct(selectedProduct.Id, quantity);
        ConsoleTheme.WriteSuccess("Product restocked successfully.");
    }

    private void ViewProducts()
    {
        ConsoleTheme.WriteSection("Admin > Catalog > View Products");
        List<Product> products = _productService.GetAllProducts();
        ProductDisplayHelper.ShowProducts("Product Catalog", products);
    }

    private void ViewLowStockProducts()
    {
        ConsoleTheme.WriteSection("Admin > Catalog > Low Stock");

        int threshold = ConsoleInputHelper.ReadNonNegativeInt("Low-stock threshold: ");
        List<Product> products = _productService.GetLowStockProducts(threshold);
        ProductDisplayHelper.ShowProducts($"Low Stock Products (<= {threshold})", products);
    }

    private void ViewAllOrders()
    {
        ConsoleTheme.WriteSection("Admin > Orders > View All");
        List<Order> orders = _orderService.GetAllOrders();
        OrderDisplayHelper.ShowOrders("All Orders", orders);
    }

    private void UpdateOrderStatus()
    {
        ConsoleTheme.WriteSection("Admin > Orders > Update Status");

        List<Order> orders = _orderService.GetAllOrders();
        if (orders.Count == 0)
        {
            ConsoleTheme.WriteInfo("No orders available.");
            return;
        }

        OrderDisplayHelper.ShowSelectableOrders("Select Order To Update", orders);
        int orderSelection = ConsoleInputHelper.ReadSelection("Choose order number: ", orders.Count);
        Order selectedOrder = orders[orderSelection - 1];

        IReadOnlyList<OrderStatus> allowedTransitions = _orderService.GetAllowedTransitions(selectedOrder.Status);
        if (allowedTransitions.Count == 0)
        {
            ConsoleTheme.WriteWarning("This order is in a terminal state and cannot transition further.");
            return;
        }

        ConsoleTheme.WriteSection("Allowed Next Statuses");
        for (int index = 0; index < allowedTransitions.Count; index++)
        {
            Console.WriteLine($"{index + 1}. {allowedTransitions[index]}");
        }

        int statusSelection = ConsoleInputHelper.ReadSelection("Choose next status: ", allowedTransitions.Count);
        OrderStatus selectedStatus = allowedTransitions[statusSelection - 1];

        if (!ConfirmationPrompt.AskYesNo($"Update status to '{selectedStatus}'?", false))
        {
            ConsoleTheme.WriteInfo("Status update cancelled.");
            return;
        }

        _orderService.UpdateOrderStatus(selectedOrder.Id, selectedStatus);
        ConsoleTheme.WriteSuccess($"Order status updated to {selectedStatus}.");
    }

    private void ViewSalesReport()
    {
        ConsoleTheme.WriteSection("Admin > Reporting > Sales Dashboard");

        int topCount = ConsoleInputHelper.ReadPositiveInt("Top-selling products to show: ");
        int lowStockThreshold = ConsoleInputHelper.ReadNonNegativeInt("Low-stock threshold: ");

        decimal totalRevenue = _reportService.GetTotalRevenue();
        Dictionary<OrderStatus, int> ordersByStatus = _reportService.GetOrdersByStatus();
        IReadOnlyList<ProductSalesReportItem> bestSellingProducts = _reportService.GetBestSellingProducts(topCount);
        IReadOnlyList<LowStockReportItem> lowStockProducts = _reportService.GetLowStockProducts(lowStockThreshold);

        ReportDisplayHelper.ShowSalesReport(totalRevenue, ordersByStatus, bestSellingProducts, lowStockProducts);
    }

    private void ViewSmartInsights()
    {
        ConsoleTheme.WriteSection("Admin > Reporting > Smart Insights");
        int lowStockThreshold = ConsoleInputHelper.ReadNonNegativeInt("Low-stock threshold for insights: ");

        IReadOnlyList<string> insights = _insightsService.GetAdminInsights(lowStockThreshold);
        ReportDisplayHelper.ShowInsights(insights);
    }

    private void ExportSalesReportPdf()
    {
        ConsoleTheme.WriteSection("Admin > Reporting > Export PDF");

        int topCount = ConsoleInputHelper.ReadPositiveInt("Top-selling products to include: ");
        int lowStockThreshold = ConsoleInputHelper.ReadNonNegativeInt("Low-stock threshold: ");

        string defaultOutputDirectory = Path.Combine(Environment.CurrentDirectory, "exports");
        ConsoleTheme.WriteInfo($"Default export folder: {defaultOutputDirectory}");

        string outputDirectory = defaultOutputDirectory;
        if (!ConfirmationPrompt.AskYesNo("Use default export folder?", true))
        {
            outputDirectory = ConsoleInputHelper.ReadRequiredString("Custom export folder path: ");
        }

        string filePath = _reportExportService.ExportSalesReportPdf(outputDirectory, topCount, lowStockThreshold);
        ConsoleTheme.WriteSuccess("Sales report exported successfully.");
        ConsoleTheme.WriteInfo($"PDF path: {filePath}");
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
