using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Resolves transition state policies for order statuses.
/// </summary>
public interface IOrderTransitionStateFactory
{
    /// <summary>
    /// Resolves the transition policy for a current order status.
    /// </summary>
    IOrderTransitionState Resolve(OrderStatus status);
}
