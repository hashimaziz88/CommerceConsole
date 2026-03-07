using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents an administrator account.
/// </summary>
public sealed class Administrator : User
{
    /// <summary>
    /// Initializes an administrator.
    /// </summary>
    public Administrator(Guid id, string fullName, string email, string password)
        : base(id, fullName, email, password, UserRole.Administrator)
    {
    }
}
