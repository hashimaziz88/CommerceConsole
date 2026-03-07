namespace CommerceConsole.Domain.Enums;

/// <summary>
/// Represents the state of a payment attempt.
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3
}
