using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Factories;
using CommerceConsole.Presentation.Interfaces;
using Xunit;

namespace CommerceConsole.Tests.Presentation;

/// <summary>
/// Tests role-based workspace factory resolution.
/// </summary>
public sealed class RoleMenuFactoryTests
{
    /// <summary>
    /// Verifies known roles resolve to registered workspaces.
    /// </summary>
    [Fact]
    public void Resolve_WithKnownRole_ReturnsWorkspace()
    {
        IUserWorkspace workspace = new FakeWorkspace(UserRole.Customer);
        RoleMenuFactory factory = new(new[] { workspace });

        IUserWorkspace? resolved = factory.Resolve(UserRole.Customer);

        Assert.NotNull(resolved);
        Assert.Equal(UserRole.Customer, resolved!.SupportedRole);
    }

    /// <summary>
    /// Verifies unknown roles return null.
    /// </summary>
    [Fact]
    public void Resolve_WithUnknownRole_ReturnsNull()
    {
        IUserWorkspace workspace = new FakeWorkspace(UserRole.Customer);
        RoleMenuFactory factory = new(new[] { workspace });

        IUserWorkspace? resolved = factory.Resolve((UserRole)999);

        Assert.Null(resolved);
    }

    private sealed class FakeWorkspace : IUserWorkspace
    {
        public FakeWorkspace(UserRole role)
        {
            SupportedRole = role;
        }

        public UserRole SupportedRole { get; }

        public void Run(ISessionContext sessionContext)
        {
        }
    }
}
