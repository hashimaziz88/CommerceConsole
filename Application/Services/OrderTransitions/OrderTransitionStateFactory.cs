using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services.OrderTransitions;

/// <summary>
/// Resolves state-style transition policies by order status.
/// </summary>
public sealed class OrderTransitionStateFactory : IOrderTransitionStateFactory
{
    private readonly IReadOnlyDictionary<OrderStatus, IOrderTransitionState> _states;

    /// <summary>
    /// Initializes known transition state policies.
    /// </summary>
    public OrderTransitionStateFactory()
    {
        List<IOrderTransitionState> states =
        [
            new PendingOrderTransitionState(),
            new PaidOrderTransitionState(),
            new ProcessingOrderTransitionState(),
            new ShippedOrderTransitionState(),
            new DeliveredOrderTransitionState(),
            new CancelledOrderTransitionState()
        ];

        _states = states.ToDictionary(state => state.Status);
    }

    /// <inheritdoc />
    public IOrderTransitionState Resolve(OrderStatus status)
    {
        if (_states.TryGetValue(status, out IOrderTransitionState? state))
        {
            return state;
        }

        throw new ValidationException($"No transition policy exists for status '{status}'.");
    }
}
