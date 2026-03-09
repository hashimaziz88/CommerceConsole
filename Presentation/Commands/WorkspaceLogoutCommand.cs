using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Command that logs out current workspace and exits menu loop.
/// </summary>
public sealed class WorkspaceLogoutCommand : IMenuCommand
{
    private readonly Action _logoutAction;

    /// <summary>
    /// Initializes workspace logout command.
    /// </summary>
    public WorkspaceLogoutCommand(Action logoutAction)
    {
        _logoutAction = logoutAction ?? throw new ValidationException("Logout callback is required.");
    }

    /// <inheritdoc />
    public MenuCommandResult Execute()
    {
        _logoutAction();
        return MenuCommandResult.ExitMenu;
    }
}
