using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Presentation.Commands;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests menu command infrastructure and main-menu command behavior.
/// </summary>
public sealed class MenuCommandTests
{
    /// <summary>
    /// Verifies dispatcher executes mapped command and returns command result.
    /// </summary>
    [Fact]
    public void Dispatch_WithMappedCommand_ExecutesAndReturnsResult()
    {
        bool invoked = false;

        MenuCommandDispatcher dispatcher = new(new Dictionary<int, IMenuCommand>
        {
            [1] = new DelegateMenuCommand(() => invoked = true)
        });

        MenuCommandResult result = dispatcher.Dispatch(1);

        Assert.True(invoked);
        Assert.Equal(MenuCommandResult.Continue, result);
    }

    /// <summary>
    /// Verifies unknown selections are rejected.
    /// </summary>
    [Fact]
    public void Dispatch_WithUnknownSelection_ThrowsValidationException()
    {
        MenuCommandDispatcher dispatcher = new(new Dictionary<int, IMenuCommand>
        {
            [1] = new DelegateMenuCommand(() => { })
        });

        Assert.Throws<ValidationException>(() => dispatcher.Dispatch(9));
    }

    /// <summary>
    /// Verifies main login command invokes callback and continues menu loop.
    /// </summary>
    [Fact]
    public void MainLoginRouteCommand_ExecutesCallback_AndReturnsContinue()
    {
        bool invoked = false;
        MainLoginRouteCommand command = new(() => invoked = true);

        MenuCommandResult result = command.Execute();

        Assert.True(invoked);
        Assert.Equal(MenuCommandResult.Continue, result);
    }

    /// <summary>
    /// Verifies main exit command returns exit when callback confirms exit.
    /// </summary>
    [Fact]
    public void MainExitMenuCommand_WhenConfirmed_ReturnsExitMenu()
    {
        MainExitMenuCommand command = new(() => true);

        MenuCommandResult result = command.Execute();

        Assert.Equal(MenuCommandResult.ExitMenu, result);
    }

    /// <summary>
    /// Verifies main exit command continues when callback cancels exit.
    /// </summary>
    [Fact]
    public void MainExitMenuCommand_WhenCancelled_ReturnsContinue()
    {
        MainExitMenuCommand command = new(() => false);

        MenuCommandResult result = command.Execute();

        Assert.Equal(MenuCommandResult.Continue, result);
    }
}
