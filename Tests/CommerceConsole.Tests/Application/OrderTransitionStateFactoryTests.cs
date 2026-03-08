using CommerceConsole.Application.Services.OrderTransitions;
using CommerceConsole.Domain.Enums;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests state-style order transition policy resolution.
/// </summary>
public sealed class OrderTransitionStateFactoryTests
{
    /// <summary>
    /// Verifies each state resolves expected allowed transitions.
    /// </summary>
    [Theory]
    [InlineData(OrderStatus.Pending, 2)]
    [InlineData(OrderStatus.Paid, 2)]
    [InlineData(OrderStatus.Processing, 2)]
    [InlineData(OrderStatus.Shipped, 1)]
    [InlineData(OrderStatus.Delivered, 0)]
    [InlineData(OrderStatus.Cancelled, 0)]
    public void Resolve_WithKnownStatus_ReturnsExpectedTransitionCount(OrderStatus status, int expectedCount)
    {
        OrderTransitionStateFactory factory = new();

        var state = factory.Resolve(status);

        Assert.Equal(status, state.Status);
        Assert.Equal(expectedCount, state.GetAllowedTransitions().Count);
    }
}
