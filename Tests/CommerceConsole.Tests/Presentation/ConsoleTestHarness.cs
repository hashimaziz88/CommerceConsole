namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Provides synchronized console input/output redirection for presentation tests.
/// </summary>
internal static class ConsoleTestHarness
{
    private static readonly object ConsoleSync = new();

    /// <summary>
    /// Executes a console read operation against deterministic input.
    /// </summary>
    public static T RunWithInput<T>(string input, Func<T> readOperation)
    {
        lock (ConsoleSync)
        {
            TextReader originalIn = Console.In;
            TextWriter originalOut = Console.Out;

            try
            {
                using StringReader reader = new(input);
                using StringWriter writer = new();

                Console.SetIn(reader);
                Console.SetOut(writer);

                return readOperation();
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }
    }

    /// <summary>
    /// Executes a console write operation and returns the captured output.
    /// </summary>
    public static string RunWithOutput(Action writeOperation)
    {
        lock (ConsoleSync)
        {
            TextWriter originalOut = Console.Out;

            try
            {
                using StringWriter writer = new();
                Console.SetOut(writer);

                writeOperation();
                return writer.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}
