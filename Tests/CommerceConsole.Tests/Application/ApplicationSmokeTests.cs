using CommerceConsole.Application.Services;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Basic service smoke tests for bootstrap verification.
/// </summary>
public sealed class ApplicationSmokeTests
{
    /// <summary>
    /// Verifies that auth service can register a new customer.
    /// </summary>
    [Fact]
    public void AuthService_RegisterCustomer_ReturnsCustomer()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            AuthService authService = new(userRepository);

            var customer = authService.RegisterCustomer("Test User", "test@example.com", "pass123");

            Assert.NotNull(customer);
            Assert.Equal("test@example.com", customer.Email);
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
