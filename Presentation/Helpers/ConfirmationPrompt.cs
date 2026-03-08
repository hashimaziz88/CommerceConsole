namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Handles reusable yes/no confirmation prompts.
/// </summary>
public static class ConfirmationPrompt
{
    /// <summary>
    /// Reads a yes/no confirmation from the terminal.
    /// </summary>
    public static bool AskYesNo(string prompt, bool defaultValue = false)
    {
        string suffix = defaultValue ? "[Y/n]" : "[y/N]";

        while (true)
        {
            Console.Write($"{prompt} {suffix}: ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return defaultValue;
            }

            if (input.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (input.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            ConsoleTheme.WriteHint("Please enter yes or no.");
        }
    }
}