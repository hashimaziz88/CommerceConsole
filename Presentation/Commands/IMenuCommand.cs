namespace CommerceConsole.Presentation.Commands;

/// <summary>
/// Represents one executable menu command.
/// </summary>
public interface IMenuCommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    void Execute();
}
