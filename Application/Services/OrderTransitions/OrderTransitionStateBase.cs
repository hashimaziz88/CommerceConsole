using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Shared base behavior for state-style order transition policies.
/// </summary>
public abstract class OrderTransitionStateBase : IOrderTransitionState
{
    private readonly IReadOnlyList<OrderStatus> _allowedTransitions;

    /// <summary>
    /// Initializes a state policy with allowed next statuses.
    /// </summary>
    protected OrderTransitionStateBase(OrderStatus status, IReadOnlyList<OrderStatus> allowedTransitions)
    {
        Status = status;
        _allowedTransitions = allowedTransitions;
    }

    /// <inheritdoc />
    public OrderStatus Status { get; }

    /// <inheritdoc />
    public IReadOnlyList<OrderStatus> GetAllowedTransitions()
    {
        return _allowedTransitions;
    }

    /// <inheritdoc />
    public bool CanTransitionTo(OrderStatus targetStatus)
    {
        return _allowedTransitions.Contains(targetStatus);
    }
}
