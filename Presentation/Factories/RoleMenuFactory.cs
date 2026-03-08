using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Interfaces;

namespace CommerceConsole.Presentation.Factories;

/// <summary>
/// Default role-to-workspace factory.
/// </summary>
public sealed class RoleMenuFactory : IRoleMenuFactory
{
    private readonly IReadOnlyDictionary<UserRole, IUserWorkspace> _workspaces;

    /// <summary>
    /// Initializes workspace registrations.
    /// </summary>
    public RoleMenuFactory(IEnumerable<IUserWorkspace> workspaces)
    {
        _workspaces = (workspaces ?? Array.Empty<IUserWorkspace>())
            .GroupBy(workspace => workspace.SupportedRole)
            .ToDictionary(group => group.Key, group => group.First());
    }

    /// <inheritdoc />
    public IUserWorkspace? Resolve(UserRole role)
    {
        return _workspaces.TryGetValue(role, out IUserWorkspace? workspace)
            ? workspace
            : null;
    }
}
