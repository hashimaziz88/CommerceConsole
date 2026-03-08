using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Transition policy for delivered orders.
/// </summary>
public sealed class DeliveredOrderTransitionState : OrderTransitionStateBase
{
    /// <summary>
    /// Initializes delivered transition policy.
    /// </summary>
    public DeliveredOrderTransitionState()
        : base(OrderStatus.Delivered, Array.Empty<OrderStatus>())
    {
    }
}
