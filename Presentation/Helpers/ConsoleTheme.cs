namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Provides consistent terminal styling and messaging primitives.
/// </summary>
public static class ConsoleTheme
{
    private const int FrameWidth = 78;

    /// <summary>
    /// Displays the application welcome banner.
    /// </summary>
    public static void WriteBanner(string title, string subtitle)
    {
        string border = new('=', FrameWidth);

        Console.WriteLine(border);
        Console.WriteLine(CenterText(title));
        Console.WriteLine(CenterText(subtitle));
        Console.WriteLine(border);
        Console.WriteLine();
    }

    /// <summary>
    /// Displays a framed page header with breadcrumb context.
    /// </summary>
    public static void WriteHeader(string title, string breadcrumb)
    {
        Console.WriteLine(new string('=', FrameWidth));
        Console.WriteLine(title);
        Console.WriteLine($"Path: {breadcrumb}");
        Console.WriteLine(new string('=', FrameWidth));
    }

    /// <summary>
    /// Displays a section heading.
    /// </summary>
    public static void WriteSection(string heading)
    {
        Console.WriteLine();
        Console.WriteLine($"[{heading}]");
        Console.WriteLine(new string('-', FrameWidth));
    }

    /// <summary>
    /// Displays a neutral divider.
    /// </summary>
    public static void WriteDivider()
    {
        Console.WriteLine(new string('-', FrameWidth));
    }

    /// <summary>
    /// Writes an informational message.
    /// </summary>
    public static void WriteInfo(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }

    /// <summary>
    /// Writes a success message.
    /// </summary>
    public static void WriteSuccess(string message)
    {
        Console.WriteLine($"[OK] {message}");
    }

    /// <summary>
    /// Writes a warning message.
    /// </summary>
    public static void WriteWarning(string message)
    {
        Console.WriteLine($"[WARN] {message}");
    }

    /// <summary>
    /// Writes an error message.
    /// </summary>
    public static void WriteError(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }

    /// <summary>
    /// Writes a small usage hint.
    /// </summary>
    public static void WriteHint(string message)
    {
        Console.WriteLine($"[TIP] {message}");
    }

    /// <summary>
    /// Waits for enter key to continue.
    /// </summary>
    public static void Pause(string message = "Press Enter to continue...")
    {
        Console.WriteLine();
        Console.Write(message);
        Console.ReadLine();
        Console.WriteLine();
    }

    private static string CenterText(string value)
    {
        if (value.Length >= FrameWidth)
        {
            return value;
        }

        int leftPadding = (FrameWidth - value.Length) / 2;
        return value.PadLeft(leftPadding + value.Length);
    }
}