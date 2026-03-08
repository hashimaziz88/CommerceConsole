using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Services;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Commands;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests command-based menu dispatch behavior.
/// </summary>
public sealed class MenuCommandDispatcherTests
{
    /// <summary>
    /// Verifies dispatcher executes mapped command actions.
    /// </summary>
    [Fact]
    public void Execute_WithMappedCommand_InvokesAction()
    {
        bool executed = false;
        IReadOnlyDictionary<int, IMenuCommand> commands = new Dictionary<int, IMenuCommand>
        {
            [1] = new DelegateMenuCommand(() => executed = true)
        };

        MenuCommandDispatcher.Execute(commands, 1);

        Assert.True(executed);
    }

    /// <summary>
    /// Verifies dispatcher can execute logout flow command.
    /// </summary>
    [Fact]
    public void Execute_WithLogoutCommand_SignsOutSession()
    {
        SessionContext session = new();
        session.SignIn(new Customer(Guid.NewGuid(), "User", "user@example.com", "pass"));

        IReadOnlyDictionary<int, IMenuCommand> commands = new Dictionary<int, IMenuCommand>
        {
            [9] = new DelegateMenuCommand(session.SignOut)
        };

        MenuCommandDispatcher.Execute(commands, 9);

        Assert.False(session.IsAuthenticated);
        Assert.Null(session.CurrentUser);
    }
}
