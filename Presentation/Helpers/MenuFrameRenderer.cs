namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Renders menu pages with consistent framing and help text.
/// </summary>
public static class MenuFrameRenderer
{
    /// <summary>
    /// Displays a structured menu section.
    /// </summary>
    public static void ShowMenu(string title, string breadcrumb, IReadOnlyList<string> options, string helpText)
    {
        ConsoleTheme.WriteHeader(title, breadcrumb);
        ConsoleTheme.WriteHint(helpText);
        ConsoleTheme.WriteDivider();

        foreach (string option in options)
        {
            Console.WriteLine(option);
        }

        ConsoleTheme.WriteDivider();
    }
}