using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Presentation.Interfaces;

/// <summary>
/// Contract for role-specific user workspaces.
/// </summary>
public interface IUserWorkspace
{
    /// <summary>
    /// Gets the role this workspace handles.
    /// </summary>
    UserRole SupportedRole { get; }

    /// <summary>
    /// Runs the workspace loop.
    /// </summary>
    void Run(ISessionContext sessionContext);
}
