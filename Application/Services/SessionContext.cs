using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Stores the current user session for the console process.
/// </summary>
public sealed class SessionContext : ISessionContext
{
    /// <inheritdoc />
    public User? CurrentUser { get; private set; }

    /// <inheritdoc />
    public bool IsAuthenticated => CurrentUser is not null;

    /// <inheritdoc />
    public void SignIn(User user)
    {
        CurrentUser = user ?? throw new ValidationException("A valid user is required to sign in.");
    }

    /// <inheritdoc />
    public void SignOut()
    {
        CurrentUser = null;
    }
}
