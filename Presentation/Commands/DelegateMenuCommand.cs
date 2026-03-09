using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Delegate-backed menu command for routine actions.
/// </summary>
public sealed class DelegateMenuCommand : IMenuCommand
{
    private readonly Action _action;
    private readonly MenuCommandResult _result;

    /// <summary>
    /// Initializes delegate command.
    /// </summary>
    public DelegateMenuCommand(Action action, MenuCommandResult result = MenuCommandResult.Continue)
    {
        _action = action ?? throw new ValidationException("Action command delegate is required.");
        _result = result;
    }

    /// <inheritdoc />
    public MenuCommandResult Execute()
    {
        _action();
        return _result;
    }
}
