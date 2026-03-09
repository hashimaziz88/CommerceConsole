using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Presentation.Workspaces;

/// <summary>
/// Resolves role-specific workspace instances.
/// </summary>
public interface IRoleWorkspaceFactory
{
    /// <summary>
    /// Attempts to resolve a workspace for the provided role.
    /// </summary>
    bool TryResolve(UserRole role, out IUserWorkspace workspace);
}
