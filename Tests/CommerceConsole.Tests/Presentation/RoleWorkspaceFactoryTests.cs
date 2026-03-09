using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Presentation.Workspaces;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests role workspace factory resolution behavior.
/// </summary>
public sealed class RoleWorkspaceFactoryTests
{
    /// <summary>
    /// Verifies registered workspace is resolved for matching role.
    /// </summary>
    [Fact]
    public void TryResolve_WithRegisteredRole_ReturnsWorkspace()
    {
        StubWorkspace customerWorkspace = new(UserRole.Customer);
        StubWorkspace adminWorkspace = new(UserRole.Administrator);
        RoleWorkspaceFactory factory = new([customerWorkspace, adminWorkspace]);

        bool resolved = factory.TryResolve(UserRole.Customer, out IUserWorkspace workspace);

        Assert.True(resolved);
        Assert.Same(customerWorkspace, workspace);
    }

    /// <summary>
    /// Verifies unsupported role cannot be resolved.
    /// </summary>
    [Fact]
    public void TryResolve_WithUnsupportedRole_ReturnsFalse()
    {
        StubWorkspace customerWorkspace = new(UserRole.Customer);
        StubWorkspace adminWorkspace = new(UserRole.Administrator);
        RoleWorkspaceFactory factory = new([customerWorkspace, adminWorkspace]);

        bool resolved = factory.TryResolve((UserRole)999, out IUserWorkspace workspace);

        Assert.False(resolved);
        Assert.Null(workspace);
    }

    /// <summary>
    /// Verifies duplicate role registration is rejected.
    /// </summary>
    [Fact]
    public void Constructor_WithDuplicateRole_ThrowsValidationException()
    {
        StubWorkspace first = new(UserRole.Customer);
        StubWorkspace second = new(UserRole.Customer);

        Assert.Throws<ValidationException>(() => new RoleWorkspaceFactory([first, second]));
    }

    private sealed class StubWorkspace : IUserWorkspace
    {
        public StubWorkspace(UserRole role)
        {
            Role = role;
        }

        public UserRole Role { get; }

        public void Run(ISessionContext sessionContext)
        {
        }
    }
}
