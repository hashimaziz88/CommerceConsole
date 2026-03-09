using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Workspaces;

/// <summary>
/// Maps user roles to concrete workspaces.
/// </summary>
public sealed class RoleWorkspaceFactory : IRoleWorkspaceFactory
{
    private readonly IReadOnlyDictionary<UserRole, IUserWorkspace> _workspaceByRole;

    /// <summary>
    /// Initializes workspace mappings.
    /// </summary>
    public RoleWorkspaceFactory(IEnumerable<IUserWorkspace> workspaces)
    {
        if (workspaces is null)
        {
            throw new ValidationException("Workspace registrations are required.");
        }

        Dictionary<UserRole, IUserWorkspace> map = new();

        foreach (IUserWorkspace workspace in workspaces)
        {
            if (map.ContainsKey(workspace.Role))
            {
                throw new ValidationException($"Duplicate workspace registration for role '{workspace.Role}'.");
            }

            map[workspace.Role] = workspace;
        }

        _workspaceByRole = map;
    }

    /// <inheritdoc />
    public bool TryResolve(UserRole role, out IUserWorkspace workspace)
    {
        return _workspaceByRole.TryGetValue(role, out workspace!);
    }
}
