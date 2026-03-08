using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Main entry menu for unauthenticated users.
/// </summary>
public sealed class MainMenu
{
    private readonly IAuthService _authService;
    private readonly ISessionContext _sessionContext;
    private readonly CustomerMenu _customerMenu;
    private readonly AdminMenu _adminMenu;

    /// <summary>
    /// Initializes the main menu.
    /// </summary>
    public MainMenu(
        IAuthService authService,
        ISessionContext sessionContext,
        CustomerMenu customerMenu,
        AdminMenu adminMenu)
    {
        _authService = authService;
        _sessionContext = sessionContext;
        _customerMenu = customerMenu;
        _adminMenu = adminMenu;
    }

    /// <summary>
    /// Runs the main menu loop.
    /// </summary>
    public void Run()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            ShowMenuOptions();
            int selection = ConsoleInputHelper.ReadSelection("Select an option: ", 3);

            switch (selection)
            {
                case 1:
                    MenuActionHelper.Execute(RegisterCustomer);
                    break;
                case 2:
                    MenuActionHelper.Execute(LoginAndRoute);
                    break;
                case 3:
                    exitRequested = true;
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void ShowMenuOptions()
    {
        Console.WriteLine("=== Commerce Console ===");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Exit");
    }

    private void RegisterCustomer()
    {
        string fullName = ConsoleInputHelper.ReadRequiredString("Full name: ");
        string email = ConsoleInputHelper.ReadRequiredString("Email: ");
        string password = ConsoleInputHelper.ReadRequiredString("Password: ");

        _authService.RegisterCustomer(fullName, email, password);
        Console.WriteLine("Registration successful. You can now log in.");
    }

    private void LoginAndRoute()
    {
        string email = ConsoleInputHelper.ReadRequiredString("Email: ");
        string password = ConsoleInputHelper.ReadRequiredString("Password: ");

        var user = _authService.Login(email, password);
        _sessionContext.SignIn(user);
        Console.WriteLine($"Welcome, {user.FullName} ({user.Role}).");
        RouteByRole();
    }

    private void RouteByRole()
    {
        if (_sessionContext.CurrentUser is null)
        {
            return;
        }

        switch (_sessionContext.CurrentUser.Role)
        {
            case UserRole.Customer:
                _customerMenu.Run(_sessionContext);
                break;
            case UserRole.Administrator:
                _adminMenu.Run(_sessionContext);
                break;
            default:
                Console.WriteLine("Unsupported user role.");
                _sessionContext.SignOut();
                break;
        }
    }
}