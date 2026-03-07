namespace CommerceConsole.Domain.Exceptions;

/// <summary>
/// Raised when wallet balance is not enough for checkout.
/// </summary>
public sealed class InsufficientFundsException : Exception
{
    /// <summary>
    /// Creates an insufficient-funds exception with a message.
    /// </summary>
    public InsufficientFundsException(string message) : base(message)
    {
    }
}
