namespace CommerceConsole.Domain.Exceptions;

/// <summary>
/// Raised when authentication fails.
/// </summary>
public sealed class AuthenticationException : Exception
{
    /// <summary>
    /// Creates an authentication exception with a message.
    /// </summary>
    public AuthenticationException(string message) : base(message)
    {
    }
}
