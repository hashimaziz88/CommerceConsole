using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Basic domain smoke tests for bootstrap verification.
/// </summary>
public sealed class DomainSmokeTests
{
    /// <summary>
    /// Verifies that invalid product price throws validation exception.
    /// </summary>
    [Fact]
    public void Product_WithNegativePrice_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            _ = new Product(Guid.NewGuid(), "Sample", "Desc", "Cat", -1m, 1));
    }
}
