using CommerceConsole.Presentation.Helpers;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests reusable yes/no confirmation prompt behavior.
/// </summary>
public sealed class ConfirmationPromptTests
{
    /// <summary>
    /// Verifies affirmative input returns true.
    /// </summary>
    [Fact]
    public void AskYesNo_WithYesInput_ReturnsTrue()
    {
        bool confirmed = ConsoleTestHarness.RunWithInput("y" + Environment.NewLine, () =>
            ConfirmationPrompt.AskYesNo("Proceed?", false));

        Assert.True(confirmed);
    }

    /// <summary>
    /// Verifies empty input falls back to provided default.
    /// </summary>
    [Fact]
    public void AskYesNo_WithEmptyInput_UsesDefaultValue()
    {
        bool confirmed = ConsoleTestHarness.RunWithInput(Environment.NewLine, () =>
            ConfirmationPrompt.AskYesNo("Proceed?", true));

        Assert.True(confirmed);
    }

    /// <summary>
    /// Verifies invalid input retries until a valid answer is provided.
    /// </summary>
    [Fact]
    public void AskYesNo_WithInvalidThenNo_ReturnsFalse()
    {
        string input = string.Join(Environment.NewLine, "maybe", "n") + Environment.NewLine;

        bool confirmed = ConsoleTestHarness.RunWithInput(input, () =>
            ConfirmationPrompt.AskYesNo("Proceed?", true));

        Assert.False(confirmed);
    }
}