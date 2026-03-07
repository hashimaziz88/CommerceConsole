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
        _ = new OrderService(orderRepository);
        _ = new ReviewService();
        _ = new ReportService(orderRepository);

        SessionContext sessionContext = new();
        CustomerMenu customerMenu = new(productService, cartService, walletService);
        AdminMenu adminMenu = new(productService);

        MainMenu mainMenu = new(authService, sessionContext, customerMenu, adminMenu);
        mainMenu.Run();
    }
}
