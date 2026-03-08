using CommerceConsole.Presentation.Helpers;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests bounded and typed console input parsing helpers.
/// </summary>
public sealed class ConsoleInputHelperTests
{
    private static readonly object ConsoleSync = new();

    /// <summary>
    /// Verifies selection parsing retries until an in-range value is provided.
    /// </summary>
    [Fact]
    public void ReadSelection_WithOutOfRangeThenValid_ReturnsValidSelection()
    {
        string input = string.Join(Environment.NewLine, "0", "4", "2") + Environment.NewLine;

        int selection = RunWithInput(input, () => ConsoleInputHelper.ReadSelection("Select: ", 3));

        Assert.Equal(2, selection);
    }

    /// <summary>
    /// Verifies positive integer parsing rejects invalid and non-positive values.
    /// </summary>
    [Fact]
    public void ReadPositiveInt_WithInvalidThenValid_ReturnsPositiveValue()
    {
        string input = string.Join(Environment.NewLine, "text", "0", "-2", "5") + Environment.NewLine;

        int quantity = RunWithInput(input, () => ConsoleInputHelper.ReadPositiveInt("Quantity: "));

        Assert.Equal(5, quantity);
    }

    /// <summary>
    /// Verifies non-negative integer parsing accepts zero and rejects negatives.
    /// </summary>
    [Fact]
    public void ReadNonNegativeInt_WithNegativeThenZero_ReturnsZero()
    {
        string input = string.Join(Environment.NewLine, "-1", "0") + Environment.NewLine;

        int quantity = RunWithInput(input, () => ConsoleInputHelper.ReadNonNegativeInt("Quantity: "));

        Assert.Equal(0, quantity);
    }

    /// <summary>
    /// Verifies positive decimal parsing retries until value is greater than zero.
    /// </summary>
    [Fact]
    public void ReadPositiveDecimal_WithNonPositiveThenValid_ReturnsPositiveValue()
    {
        string input = string.Join(Environment.NewLine, "0", "-15", "9") + Environment.NewLine;

        decimal amount = RunWithInput(input, () => ConsoleInputHelper.ReadPositiveDecimal("Amount: "));

        Assert.Equal(9m, amount);
    }

    /// <summary>
    /// Verifies invalid range configuration is rejected.
    /// </summary>
    [Fact]
    public void ReadIntInRange_WithInvalidRange_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ConsoleInputHelper.ReadIntInRange("Select: ", 5, 1));
    }

    private static T RunWithInput<T>(string input, Func<T> readOperation)
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
}