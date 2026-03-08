using CommerceConsole.Application.Services;
using CommerceConsole.Application.Services.OrderTransitions;
using CommerceConsole.Application.Services.Payments;
using CommerceConsole.Infrastructure.Data;
using CommerceConsole.Infrastructure.Export;
using CommerceConsole.Infrastructure.Repositories;
using CommerceConsole.Presentation.Factories;
using CommerceConsole.Presentation.Interfaces;
using CommerceConsole.Presentation.Menus;

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

        WalletPaymentStrategy walletPaymentStrategy = new();
        OrderTransitionStateFactory transitionStateFactory = new();
        OrderService orderService = new(
            orderRepository,
            productRepository,
            userRepository,
            walletPaymentStrategy,
            transitionStateFactory);

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

        RoleMenuFactory roleMenuFactory = new(new IUserWorkspace[] { customerMenu, adminMenu });

        MainMenu mainMenu = new(authService, sessionContext, roleMenuFactory);
        mainMenu.Run();
    }
}
