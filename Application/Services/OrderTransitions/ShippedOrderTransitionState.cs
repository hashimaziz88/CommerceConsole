using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Transition policy for shipped orders.
/// </summary>
public sealed class ShippedOrderTransitionState : OrderTransitionStateBase
{
    /// <summary>
    /// Initializes shipped transition policy.
    /// </summary>
    public ShippedOrderTransitionState()
        : base(OrderStatus.Shipped, new[] { OrderStatus.Delivered })
    {
    }
}
