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
    /// Reads a decimal value that must be greater than zero.
    /// </summary>
    public static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            decimal value = ReadDecimal(prompt);
            if (value > 0)
            {
                return value;
            }

            Console.WriteLine("Enter a value greater than zero.");
        }
    }

    /// <summary>
    /// Reads a decimal value that must be zero or positive.
    /// </summary>
    public static decimal ReadNonNegativeDecimal(string prompt)
    {
        while (true)
        {
            decimal value = ReadDecimal(prompt);
            if (value >= 0)
            {
                return value;
            }

            Console.WriteLine("Enter a value that is zero or greater.");
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
    /// Reads an integer value that must be greater than zero.
    /// </summary>
    public static int ReadPositiveInt(string prompt)
    {
        return ReadIntInRange(prompt, 1, int.MaxValue);
    }

    /// <summary>
    /// Reads an integer value that must be zero or positive.
    /// </summary>
    public static int ReadNonNegativeInt(string prompt)
    {
        return ReadIntInRange(prompt, 0, int.MaxValue);
    }

    /// <summary>
    /// Reads an integer value in a closed range.
    /// </summary>
    public static int ReadIntInRange(string prompt, int minInclusive, int maxInclusive)
    {
        if (minInclusive > maxInclusive)
        {
            throw new ArgumentOutOfRangeException(nameof(minInclusive), "Minimum must be less than or equal to maximum.");
        }

        while (true)
        {
            int value = ReadInt(prompt);
            if (value >= minInclusive && value <= maxInclusive)
            {
                return value;
            }

            Console.WriteLine($"Enter a number between {minInclusive} and {maxInclusive}.");
        }
    }

    /// <summary>
    /// Reads a one-based selection from a numbered list.
    /// </summary>
    public static int ReadSelection(string prompt, int maxOption)
    {
        if (maxOption < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxOption), "At least one option must be available.");
        }

        return ReadIntInRange(prompt, 1, maxOption);
    }
}