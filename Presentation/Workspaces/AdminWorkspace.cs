using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Menus;

namespace CommerceConsole.Presentation.Workspaces;

/// <summary>
/// Administrator role workspace adapter.
/// </summary>
public sealed class AdminWorkspace : IUserWorkspace
{
    private readonly AdminMenu _adminMenu;

    /// <summary>
    /// Initializes administrator workspace adapter.
    /// </summary>
    public AdminWorkspace(AdminMenu adminMenu)
    {
        _adminMenu = adminMenu;
    }

    /// <inheritdoc />
    public UserRole Role => UserRole.Administrator;

    /// <inheritdoc />
    public void Run(ISessionContext sessionContext)
    {
        _adminMenu.Run(sessionContext);
    }
}
