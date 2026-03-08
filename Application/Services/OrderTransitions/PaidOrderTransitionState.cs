using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Transition policy for paid orders.
/// </summary>
public sealed class PaidOrderTransitionState : OrderTransitionStateBase
{
    /// <summary>
    /// Initializes paid transition policy.
    /// </summary>
    public PaidOrderTransitionState()
        : base(OrderStatus.Paid, new[] { OrderStatus.Processing, OrderStatus.Cancelled })
    {
    }
}
