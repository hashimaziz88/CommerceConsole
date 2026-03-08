using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Executes a menu command by selection index.
/// </summary>
public static class MenuCommandDispatcher
{
    /// <summary>
    /// Executes the command mapped to the selected option.
    /// </summary>
    public static void Execute(IReadOnlyDictionary<int, IMenuCommand> commands, int selection)
    {
        if (!commands.TryGetValue(selection, out IMenuCommand? command))
        {
            throw new ValidationException($"No menu command is registered for selection '{selection}'.");
        }

        command.Execute();
    }
}
