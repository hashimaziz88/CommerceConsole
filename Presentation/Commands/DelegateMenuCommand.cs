using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Wraps an action into a command object.
/// </summary>
public sealed class DelegateMenuCommand : IMenuCommand
{
    private readonly Action _action;

    /// <summary>
    /// Initializes a delegate-backed menu command.
    /// </summary>
    public DelegateMenuCommand(Action action)
    {
        _action = action ?? throw new ValidationException("Command action is required.");
    }

    /// <inheritdoc />
    public void Execute()
    {
        _action();
    }
}
