using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Infrastructure.Data;

/// <summary>
/// Seeds starter data for bootstrap flows.
/// </summary>
public static class SeedData
{
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

        if (productRepository.GetAll().Count > 0)
        {
            return;
        }

        productRepository.Add(new Product(Guid.NewGuid(), "Laptop", "15-inch productivity laptop", "Electronics", 15000m, 10));
        productRepository.Add(new Product(Guid.NewGuid(), "Headphones", "Noise-cancelling headset", "Electronics", 2500m, 25));
        productRepository.Add(new Product(Guid.NewGuid(), "Notebook", "A5 ruled notebook", "Stationery", 45m, 100));
    }
}
