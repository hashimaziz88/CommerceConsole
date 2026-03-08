using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Transition policy for processing orders.
/// </summary>
public sealed class ProcessingOrderTransitionState : OrderTransitionStateBase
{
    /// <summary>
    /// Initializes processing transition policy.
    /// </summary>
    public ProcessingOrderTransitionState()
        : base(OrderStatus.Processing, new[] { OrderStatus.Shipped, OrderStatus.Cancelled })
    {
    }
}
