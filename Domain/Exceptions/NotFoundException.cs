namespace CommerceConsole.Domain.Exceptions;

/// <summary>
/// Raised when an entity cannot be located.
/// </summary>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Creates a not-found exception with a message.
    /// </summary>
    public NotFoundException(string message) : base(message)
    {
    }
}
