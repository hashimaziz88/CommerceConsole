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
        InMemoryUserRepository userRepository = new();
        AuthService authService = new(userRepository);

        var customer = authService.RegisterCustomer("Test User", "test@example.com", "pass123");

        Assert.NotNull(customer);
        Assert.Equal("test@example.com", customer.Email);
    }
}
