using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Transition policy for cancelled orders.
/// </summary>
public sealed class CancelledOrderTransitionState : OrderTransitionStateBase
{
    /// <summary>
    /// Initializes cancelled transition policy.
    /// </summary>
    public CancelledOrderTransitionState()
        : base(OrderStatus.Cancelled, Array.Empty<OrderStatus>())
    {
    }
}
