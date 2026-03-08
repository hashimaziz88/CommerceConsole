using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for one order transition state policy.
/// </summary>
public interface IOrderTransitionState
{
    /// <summary>
    /// Gets the status represented by this state policy.
    /// </summary>
    OrderStatus Status { get; }

    /// <summary>
    /// Returns allowed next statuses from this state.
    /// </summary>
    IReadOnlyList<OrderStatus> GetAllowedTransitions();

    /// <summary>
    /// Returns true when a transition to the target status is valid.
    /// </summary>
    bool CanTransitionTo(OrderStatus targetStatus);
}
