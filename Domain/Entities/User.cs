using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Base type for all users of the system.
/// </summary>
public abstract class User
{
    /// <summary>
    /// Initializes a user base instance.
    /// </summary>
    protected User(Guid id, string fullName, string email, string password, UserRole role)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("User ID must be a valid GUID.");
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ValidationException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ValidationException("Password is required.");
        }

        Id = id;
        FullName = fullName.Trim();
        Email = email.Trim().ToLowerInvariant();
        Password = password;
        Role = role;
    }

    /// <summary>
    /// Gets the unique user identifier.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    public string FullName { get; private set; }

    /// <summary>
    /// Gets the login email address.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Gets the stored password value for the demo app.
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    /// Gets the role assigned to this user.
    /// </summary>
    public UserRole Role { get; }

    /// <summary>
    /// Verifies the supplied password.
    /// </summary>
    public bool VerifyPassword(string password)
    {
        return Password == password;
    }
}
