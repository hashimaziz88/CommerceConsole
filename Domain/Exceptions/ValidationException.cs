namespace CommerceConsole.Domain.Exceptions;

/// <summary>
/// Raised when user-provided data violates validation rules.
/// </summary>
public sealed class ValidationException : Exception
{
    /// <summary>
    /// Creates a validation exception with a message.
    /// </summary>
    public ValidationException(string message) : base(message)
    {
    }
}
