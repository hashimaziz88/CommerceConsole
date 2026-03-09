using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Presentation.Workspaces;

/// <summary>
/// Represents a role-specific presentation workspace.
/// </summary>
public interface IUserWorkspace
{
    /// <summary>
    /// Gets the role served by this workspace.
    /// </summary>
    UserRole Role { get; }

    /// <summary>
    /// Runs the workspace for the active session.
    /// </summary>
    void Run(ISessionContext sessionContext);
}
