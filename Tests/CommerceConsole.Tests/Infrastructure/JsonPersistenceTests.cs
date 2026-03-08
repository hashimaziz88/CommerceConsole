using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Infrastructure.Data;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Infrastructure;

/// <summary>
/// Verifies JSON persistence behavior for repositories.
/// </summary>
public sealed class JsonPersistenceTests
{
    /// <summary>
    /// Verifies user and product seed data persists to JSON and avoids duplicate seeding.
    /// </summary>
    [Fact]
    public void SeedData_WithJsonBackedRepositories_PersistsAndAvoidsDuplicates()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepositoryFirstRun = new(dataDirectory);
            InMemoryProductRepository productRepositoryFirstRun = new(dataDirectory);
            SeedData.Seed(userRepositoryFirstRun, productRepositoryFirstRun);
            int firstSeededProductCount = productRepositoryFirstRun.GetAll().Count;

            InMemoryUserRepository userRepositorySecondRun = new(dataDirectory);
            InMemoryProductRepository productRepositorySecondRun = new(dataDirectory);
            SeedData.Seed(userRepositorySecondRun, productRepositorySecondRun);

            int adminCount = userRepositorySecondRun
                .GetAll()
                .Count(user => user.Role == UserRole.Administrator);

            int secondSeededProductCount = productRepositorySecondRun.GetAll().Count;

            Assert.Equal(1, adminCount);
            Assert.Equal(firstSeededProductCount, secondSeededProductCount);
            Assert.True(secondSeededProductCount >= 10);
            Assert.True(File.Exists(Path.Combine(dataDirectory, "users.json")));
            Assert.True(File.Exists(Path.Combine(dataDirectory, "products.json")));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies customer registration is persisted across repository instances.
    /// </summary>
    [Fact]
    public void UserRepository_PersistsRegisteredCustomerAcrossInstances()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository firstRepository = new(dataDirectory);
            Customer customer = new(
                Guid.NewGuid(),
                "Persisted User",
                "persisted@example.com",
                "pass123");

            firstRepository.Add(customer);

            InMemoryUserRepository secondRepository = new(dataDirectory);
            var loaded = secondRepository.GetByEmail("persisted@example.com");

            Assert.NotNull(loaded);
            Assert.Equal(UserRole.Customer, loaded!.Role);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    private static string CreateTempDataDirectory()
    {
        string path = Path.Combine(Path.GetTempPath(), "CommerceConsoleTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
}
