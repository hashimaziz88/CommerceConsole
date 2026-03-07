using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Infrastructure.Repositories.Models;

/// <summary>
/// JSON persistence model for users.
/// </summary>
internal sealed class UserRecord
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the login email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password (demo scope).
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user role.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Gets or sets wallet balance for customer rows.
    /// </summary>
    public decimal WalletBalance { get; set; }

    /// <summary>
    /// Gets or sets persisted cart item snapshots.
    /// </summary>
    public List<UserCartItemRecord> CartItems { get; set; } = new();
}
