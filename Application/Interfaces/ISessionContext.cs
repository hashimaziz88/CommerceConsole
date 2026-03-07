using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Tracks the currently authenticated user session.
/// </summary>
public interface ISessionContext
{
    /// <summary>
    /// Gets the current authenticated user or null.
    /// </summary>
    User? CurrentUser { get; }

    /// <summary>
    /// Gets whether a user is currently authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Sets the active user session.
    /// </summary>
    void SignIn(User user);

    /// <summary>
    /// Clears the active user session.
    /// </summary>
    void SignOut();
}
