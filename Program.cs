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
        _ = new ProductService(productRepository);
        _ = new CartService();
        _ = new WalletService();
        _ = new OrderService(orderRepository);
        _ = new ReviewService();
        _ = new ReportService(orderRepository);

        SessionContext sessionContext = new();
        CustomerMenu customerMenu = new();
        AdminMenu adminMenu = new();

        MainMenu mainMenu = new(authService, sessionContext, customerMenu, adminMenu);
        mainMenu.Run();
    }
}
