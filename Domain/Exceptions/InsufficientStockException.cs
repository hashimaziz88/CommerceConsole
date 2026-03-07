namespace CommerceConsole.Domain.Exceptions;

/// <summary>
/// Raised when available stock is not enough for an operation.
/// </summary>
public sealed class InsufficientStockException : Exception
{
    /// <summary>
    /// Creates an insufficient-stock exception with a message.
    /// </summary>
    public InsufficientStockException(string message) : base(message)
    {
    }
}
