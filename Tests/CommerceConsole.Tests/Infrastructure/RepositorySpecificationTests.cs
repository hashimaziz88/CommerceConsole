using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Specifications;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Infrastructure;

/// <summary>
/// Tests repository specification-based querying.
/// </summary>
public sealed class RepositorySpecificationTests
{
    /// <summary>
    /// Verifies product repository Find filters active products.
    /// </summary>
    [Fact]
    public void ProductRepository_Find_WithActiveSpecification_ReturnsOnlyActiveProducts()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryProductRepository repository = new(dataDirectory);

            Product active = new(Guid.NewGuid(), "Active", "A", "Electronics", 10m, 2);
            Product inactive = new(Guid.NewGuid(), "Inactive", "B", "Electronics", 20m, 3);
            inactive.Deactivate();

            repository.Add(active);
            repository.Add(inactive);

            List<Product> results = repository.Find(new ActiveProductSpecification());

            Assert.Single(results);
            Assert.Equal(active.Id, results[0].Id);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies user repository Find can filter by composed specification.
    /// </summary>
    [Fact]
    public void UserRepository_Find_WithCustomSpecification_ReturnsExpectedUsers()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository repository = new(dataDirectory);

            Customer alpha = new(Guid.NewGuid(), "Alpha", "alpha@example.com", "pass");
            Customer beta = new(Guid.NewGuid(), "Beta", "beta@example.com", "pass");

            repository.Add(alpha);
            repository.Add(beta);

            List<User> results = repository.Find(new NameStartsWithSpecification("A"));

            Assert.Single(results);
            Assert.Equal(alpha.Id, results[0].Id);
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

    private sealed class NameStartsWithSpecification : ISpecification<User>
    {
        private readonly string _prefix;

        public NameStartsWithSpecification(string prefix)
        {
            _prefix = prefix;
        }

        public bool IsSatisfiedBy(User candidate)
        {
            return candidate.FullName.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase);
        }
    }
}
