using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Menus;

namespace CommerceConsole.Presentation.Workspaces;

/// <summary>
/// Customer role workspace adapter.
/// </summary>
public sealed class CustomerWorkspace : IUserWorkspace
{
    private readonly CustomerMenu _customerMenu;

    /// <summary>
    /// Initializes customer workspace adapter.
    /// </summary>
    public CustomerWorkspace(CustomerMenu customerMenu)
    {
        _customerMenu = customerMenu;
    }

    /// <inheritdoc />
    public UserRole Role => UserRole.Customer;

    /// <inheritdoc />
    public void Run(ISessionContext sessionContext)
    {
        _customerMenu.Run(sessionContext);
    }
}
