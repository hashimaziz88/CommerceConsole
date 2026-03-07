namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Centralized console input helper to reduce duplicated parsing logic.
/// </summary>
public static class ConsoleInputHelper
{
    /// <summary>
    /// Reads a required text value.
    /// </summary>
    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Value is required.");
        }
    }

    /// <summary>
    /// Reads a decimal value.
    /// </summary>
    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid decimal number.");
        }
    }

    /// <summary>
    /// Reads an integer value.
    /// </summary>
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid integer number.");
        }
    }

    /// <summary>
    /// Reads a GUID value.
    /// </summary>
    public static Guid ReadGuid(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (Guid.TryParse(input, out Guid value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid GUID.");
        }
    }
}
