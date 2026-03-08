using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Transition policy for pending orders.
/// </summary>
public sealed class PendingOrderTransitionState : OrderTransitionStateBase
{
    /// <summary>
    /// Initializes pending transition policy.
    /// </summary>
    public PendingOrderTransitionState()
        : base(OrderStatus.Pending, new[] { OrderStatus.Paid, OrderStatus.Cancelled })
    {
    }
}
