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

        _ = new AuthService(userRepository);
        _ = new ProductService(productRepository);
        _ = new CartService();
        _ = new WalletService();
        _ = new OrderService(orderRepository);
        _ = new ReviewService();
        _ = new ReportService(orderRepository);

        MainMenu mainMenu = new();
        mainMenu.Show();

        Console.WriteLine("Scaffold ready. Continue with feature implementation prompts.");
    }
}
