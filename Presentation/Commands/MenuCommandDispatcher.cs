using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Dispatcher for number-to-command mappings.
/// </summary>
public sealed class MenuCommandDispatcher
{
    private readonly IReadOnlyDictionary<int, IMenuCommand> _commands;

    /// <summary>
    /// Initializes command dispatcher.
    /// </summary>
    public MenuCommandDispatcher(IReadOnlyDictionary<int, IMenuCommand> commands)
    {
        _commands = commands ?? throw new ValidationException("Command mappings are required.");
    }

    /// <summary>
    /// Executes mapped command by numeric selection.
    /// </summary>
    public MenuCommandResult Dispatch(int selection)
    {
        if (!_commands.TryGetValue(selection, out IMenuCommand? command))
        {
            throw new ValidationException($"Unsupported menu selection '{selection}'.");
        }

        return command.Execute();
    }
}
