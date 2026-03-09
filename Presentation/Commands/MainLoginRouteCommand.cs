using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Main-menu command that performs login and role routing.
/// </summary>
public sealed class MainLoginRouteCommand : IMenuCommand
{
    private readonly Action _loginAndRoute;

    /// <summary>
    /// Initializes login-route command.
    /// </summary>
    public MainLoginRouteCommand(Action loginAndRoute)
    {
        _loginAndRoute = loginAndRoute ?? throw new ValidationException("Login route action is required.");
    }

    /// <inheritdoc />
    public MenuCommandResult Execute()
    {
        _loginAndRoute();
        return MenuCommandResult.Continue;
    }
}
