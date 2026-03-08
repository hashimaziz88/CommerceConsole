using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Main entry menu for unauthenticated users.
/// </summary>
public sealed class MainMenu
{
    private static readonly IReadOnlyList<string> MenuOptions = new List<string>
    {
        "1. Register New Customer",
        "2. Login",
        "3. Exit Application"
    };

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
        bool bannerShown = false;

        while (!exitRequested)
        {
            if (!bannerShown)
            {
                ConsoleTheme.WriteBanner("CommerceConsole", "Online Shopping Backend Demo Experience");
                bannerShown = true;
            }

            ShowMenuOptions();
            int selection = ConsoleInputHelper.ReadSelection("Choose option (1-3): ", 3);

            switch (selection)
            {
                case 1:
                    MenuActionHelper.Execute(RegisterCustomer);
                    break;
                case 2:
                    MenuActionHelper.Execute(LoginAndRoute);
                    break;
                case 3:
                    if (ConfirmationPrompt.AskYesNo("Exit CommerceConsole now?", false))
                    {
                        exitRequested = true;
                        ConsoleTheme.WriteInfo("Session ended. Goodbye.");
                    }
                    else
                    {
                        ConsoleTheme.WriteInfo("Exit cancelled.");
                    }

                    break;
            }

            if (!exitRequested)
            {
                ConsoleTheme.Pause();
            }
        }
    }

    private static void ShowMenuOptions()
    {
        MenuFrameRenderer.ShowMenu(
            "Main Menu",
            "Home",
            MenuOptions,
            "Use number selection. Register or login to access role-based workspaces.");
    }

    private void RegisterCustomer()
    {
        ConsoleTheme.WriteSection("Register Customer");
        ConsoleTheme.WriteHint("Example email format: learner@example.com");

        string fullName = ConsoleInputHelper.ReadRequiredString("Full name: ");
        string email = ConsoleInputHelper.ReadRequiredString("Email: ");
        string password = ConsoleInputHelper.ReadRequiredString("Password: ");

        _authService.RegisterCustomer(fullName, email, password);
        ConsoleTheme.WriteSuccess("Registration successful. You can now log in.");
    }

    private void LoginAndRoute()
    {
        ConsoleTheme.WriteSection("Login");

        string email = ConsoleInputHelper.ReadRequiredString("Email: ");
        string password = ConsoleInputHelper.ReadRequiredString("Password: ");

        var user = _authService.Login(email, password);
        _sessionContext.SignIn(user);
        ConsoleTheme.WriteSuccess($"Welcome, {user.FullName} ({user.Role}).");
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
                ConsoleTheme.WriteInfo("Opening customer workspace...");
                _customerMenu.Run(_sessionContext);
                break;
            case UserRole.Administrator:
                ConsoleTheme.WriteInfo("Opening administrator workspace...");
                _adminMenu.Run(_sessionContext);
                break;
            default:
                ConsoleTheme.WriteWarning("Unsupported user role. Signing out.");
                _sessionContext.SignOut();
                break;
        }
    }
}