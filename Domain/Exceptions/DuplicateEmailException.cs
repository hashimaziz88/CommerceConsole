namespace CommerceConsole.Domain.Exceptions;

/// <summary>
/// Raised when an email already exists in the system.
/// </summary>
public sealed class DuplicateEmailException : Exception
{
    /// <summary>
    /// Creates a duplicate-email exception with a message.
    /// </summary>
    public DuplicateEmailException(string message) : base(message)
    {
    }
}
