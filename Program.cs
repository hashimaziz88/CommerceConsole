using CommerceConsole.Application.Services;
using CommerceConsole.Infrastructure.Data;
using CommerceConsole.Infrastructure.Export;
using CommerceConsole.Infrastructure.Repositories;
using CommerceConsole.Presentation.Menus;
using CommerceConsole.Presentation.Workspaces;

namespace CommerceConsole;

/// <summary>
/// Application bootstrap entry point.
/// </summary>
public static class Program
{
    /// <summary>
    /// Starts the console application.
    /// </summary>
    public static void Main()
    {
        InMemoryUserRepository userRepository = new();
        InMemoryProductRepository productRepository = new();
        InMemoryOrderRepository orderRepository = new();

        SeedData.Seed(userRepository, productRepository);

        AuthService authService = new(userRepository);
        ProductService productService = new(productRepository);
        CartService cartService = new(productRepository, userRepository);
        WalletService walletService = new(userRepository);

        WalletPaymentStrategy paymentStrategy = new();
        OrderService orderService = new(orderRepository, productRepository, userRepository, paymentStrategy);

        ReviewService reviewService = new(orderRepository, productRepository, userRepository);
        ReportService reportService = new(orderRepository, productRepository);

        InsightsService insightsService = new(orderRepository, productRepository);
        PdfReportExporter reportExporter = new();
        ReportExportService reportExportService = new(reportService, reportExporter);

        SessionContext sessionContext = new();
        CustomerMenu customerMenu = new(
            productService,
            cartService,
            walletService,
            orderService,
            reviewService,
            insightsService);

        AdminMenu adminMenu = new(
            productService,
            orderService,
            reportService,
            reportExportService,
            insightsService);

        CustomerWorkspace customerWorkspace = new(customerMenu);
        AdminWorkspace adminWorkspace = new(adminMenu);
        RoleWorkspaceFactory roleWorkspaceFactory = new([customerWorkspace, adminWorkspace]);

        MainMenu mainMenu = new(authService, sessionContext, roleWorkspaceFactory);
        mainMenu.Run();
    }
}
