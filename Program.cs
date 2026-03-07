using CommerceConsole.Application.Services;
using CommerceConsole.Infrastructure.Data;
using CommerceConsole.Infrastructure.Repositories;
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
        OrderService orderService = new(orderRepository, productRepository, userRepository);
        ReviewService reviewService = new(orderRepository, productRepository, userRepository);
        ReportService reportService = new(orderRepository, productRepository);

        SessionContext sessionContext = new();
        CustomerMenu customerMenu = new(productService, cartService, walletService, orderService, reviewService);
        AdminMenu adminMenu = new(productService, orderService, reportService);

        MainMenu mainMenu = new(authService, sessionContext, customerMenu, adminMenu);
        mainMenu.Run();
    }
}
