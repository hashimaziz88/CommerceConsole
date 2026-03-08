using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Infrastructure.Data;

/// <summary>
/// Seeds starter data for bootstrap flows.
/// </summary>
public static class SeedData
{
    private static readonly IReadOnlyList<(string Name, string Description, string Category, decimal Price, int Stock)> SeedProducts =
    [
        ("Laptop", "15-inch productivity laptop", "Electronics", 15000m, 10),
        ("Headphones", "Noise-cancelling headset", "Electronics", 2500m, 25),
        ("Notebook", "A5 ruled notebook", "Stationery", 45m, 100),
        ("Smartphone", "128GB dual-SIM smartphone", "Electronics", 11999m, 14),
        ("Mechanical Keyboard", "Tactile keyboard with backlight", "Electronics", 1899m, 18),
        ("27-inch Monitor", "QHD display for work and gaming", "Electronics", 4799m, 7),
        ("USB-C Hub", "Multiport adapter with HDMI and USB", "Accessories", 799m, 40),
        ("Wireless Mouse", "Ergonomic rechargeable mouse", "Accessories", 549m, 35),
        ("Electric Kettle", "1.7L fast-boil kettle", "Home and Kitchen", 699m, 20),
        ("Air Fryer", "4L digital air fryer", "Home and Kitchen", 2299m, 9),
        ("Blender", "High-speed countertop blender", "Home and Kitchen", 1399m, 11),
        ("Yoga Mat", "Non-slip 6mm exercise mat", "Fitness", 399m, 22),
        ("Resistance Bands", "Set of 5 workout bands", "Fitness", 299m, 30),
        ("Desk Lamp", "LED lamp with brightness control", "Home Office", 629m, 16),
        ("Clean Architecture Book", "Software design and architecture guide", "Books", 899m, 13)
    ];

    /// <summary>
    /// Seeds a default admin and sample products.
    /// </summary>
    public static void Seed(IUserRepository userRepository, IProductRepository productRepository)
    {
        if (!userRepository.GetAll().Any(user => user is Administrator))
        {
            Administrator admin = new(Guid.NewGuid(), "System Admin", "admin@commerce.local", "admin123");
            userRepository.Add(admin);
        }

        HashSet<string> existingNames = productRepository.GetAll()
            .Select(product => product.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach ((string name, string description, string category, decimal price, int stock) in SeedProducts)
        {
            if (existingNames.Contains(name))
            {
                continue;
            }

            productRepository.Add(new Product(Guid.NewGuid(), name, description, category, price, stock));
            existingNames.Add(name);
        }
    }
}
