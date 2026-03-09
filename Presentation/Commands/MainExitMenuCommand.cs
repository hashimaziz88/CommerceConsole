using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Main-menu command that controls exit confirmation behavior.
/// </summary>
public sealed class MainExitMenuCommand : IMenuCommand
{
    private readonly Func<bool> _requestExit;

    /// <summary>
    /// Initializes exit command.
    /// </summary>
    public MainExitMenuCommand(Func<bool> requestExit)
    {
        _requestExit = requestExit ?? throw new ValidationException("Exit callback is required.");
    }

    /// <inheritdoc />
    public MenuCommandResult Execute()
    {
        return _requestExit() ? MenuCommandResult.ExitMenu : MenuCommandResult.Continue;
    }
}
