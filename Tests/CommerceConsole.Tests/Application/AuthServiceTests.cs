using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Data;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests registration, login, and session scenarios.
/// </summary>
public sealed class AuthServiceTests
{
    /// <summary>
    /// Verifies customer registration persists a new customer.
    /// </summary>
    [Fact]
    public void RegisterCustomer_WithValidDetails_AddsCustomer()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            AuthService authService = new(userRepository);

            Customer customer = authService.RegisterCustomer("Test User", "test@example.com", "pass123");

            Assert.Equal("test@example.com", customer.Email);
            Assert.NotNull(userRepository.GetByEmail("test@example.com"));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies duplicate email registration is blocked.
    /// </summary>
    [Fact]
    public void RegisterCustomer_WithDuplicateEmail_ThrowsDuplicateEmailException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            AuthService authService = new(userRepository);

            _ = authService.RegisterCustomer("First User", "dup@example.com", "pass123");

            Assert.Throws<DuplicateEmailException>(() =>
                _ = authService.RegisterCustomer("Second User", "dup@example.com", "pass456"));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies invalid registration email format is rejected.
    /// </summary>
    [Fact]
    public void RegisterCustomer_WithInvalidEmail_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            AuthService authService = new(userRepository);

            Assert.Throws<ValidationException>(() =>
                _ = authService.RegisterCustomer("Test User", "invalid-email", "pass123"));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies customer login succeeds with valid credentials.
    /// </summary>
    [Fact]
    public void Login_WithCustomerCredentials_ReturnsCustomer()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            AuthService authService = new(userRepository);

            _ = authService.RegisterCustomer("Test User", "test@example.com", "pass123");

            User user = authService.Login("test@example.com", "pass123");

            Assert.IsType<Customer>(user);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies seeded admin login succeeds.
    /// </summary>
    [Fact]
    public void Login_WithSeededAdminCredentials_ReturnsAdministrator()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            InMemoryProductRepository productRepository = new(dataDirectory);
            SeedData.Seed(userRepository, productRepository);

            AuthService authService = new(userRepository);

            User user = authService.Login("admin@commerce.local", "admin123");

            Assert.IsType<Administrator>(user);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies invalid password fails authentication.
    /// </summary>
    [Fact]
    public void Login_WithWrongPassword_ThrowsAuthenticationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            InMemoryUserRepository userRepository = new(dataDirectory);
            AuthService authService = new(userRepository);

            _ = authService.RegisterCustomer("Test User", "test@example.com", "pass123");

            Assert.Throws<AuthenticationException>(() =>
                _ = authService.Login("test@example.com", "wrong-pass"));
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies session state transitions for sign in and sign out.
    /// </summary>
    [Fact]
    public void SessionContext_SignInAndSignOut_TracksAuthenticationState()
    {
        SessionContext sessionContext = new();
        Customer customer = new(Guid.NewGuid(), "Test User", "test@example.com", "pass123");

        sessionContext.SignIn(customer);
        Assert.True(sessionContext.IsAuthenticated);
        Assert.Equal(customer.Id, sessionContext.CurrentUser?.Id);

        sessionContext.SignOut();
        Assert.False(sessionContext.IsAuthenticated);
        Assert.Null(sessionContext.CurrentUser);
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
