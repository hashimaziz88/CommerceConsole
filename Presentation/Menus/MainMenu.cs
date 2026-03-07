namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Main entry menu for unauthenticated users.
/// </summary>
public sealed class MainMenu
{
    /// <summary>
    /// Displays the main menu options.
    /// </summary>
    public void Show()
    {
        Console.WriteLine("=== Commerce Console ===");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Exit");
    }
}
