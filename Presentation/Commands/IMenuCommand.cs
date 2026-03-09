namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Defines a single executable menu action.
/// </summary>
public interface IMenuCommand
{
    /// <summary>
    /// Executes command behavior and returns loop control result.
    /// </summary>
    MenuCommandResult Execute();
}
