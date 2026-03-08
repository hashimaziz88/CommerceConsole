using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Interfaces;

namespace CommerceConsole.Presentation.Factories;

/// <summary>
/// Resolves role-based user workspaces.
/// </summary>
public interface IRoleMenuFactory
{
    /// <summary>
    /// Returns the workspace for a user role when registered.
    /// </summary>
    IUserWorkspace? Resolve(UserRole role);
}
