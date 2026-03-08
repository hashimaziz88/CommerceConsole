using CommerceConsole.Application.Interfaces;
using CommerceConsole.Presentation.Commands;
using CommerceConsole.Presentation.Factories;
using CommerceConsole.Presentation.Helpers;
using CommerceConsole.Presentation.Interfaces;

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
    private readonly IRoleMenuFactory _roleMenuFactory;

    /// <summary>
    /// Initializes the main menu.
    /// </summary>
    public MainMenu(
        IAuthService authService,
        ISessionContext sessionContext,
        IRoleMenuFactory roleMenuFactory)
    {
        _authService = authService;
        _sessionContext = sessionContext;
        _roleMenuFactory = roleMenuFactory;
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

            IReadOnlyDictionary<int, IMenuCommand> commands = BuildCommands(() => exitRequested = true);
            MenuCommandDispatcher.Execute(commands, selection);

            if (!exitRequested)
            {
                ConsoleTheme.Pause();
            }
        }
    }

    private IReadOnlyDictionary<int, IMenuCommand> BuildCommands(Action requestExit)
    {
        return new Dictionary<int, IMenuCommand>
        {
            [1] = new DelegateMenuCommand(() => MenuActionHelper.Execute(RegisterCustomer)),
            [2] = new DelegateMenuCommand(() => MenuActionHelper.Execute(LoginAndRoute)),
            [3] = new DelegateMenuCommand(() => HandleExit(requestExit))
        };
    }

    private static void ShowMenuOptions()
    {
        MenuFrameRenderer.ShowMenu(
            "Main Menu",
            "Home",
            MenuOptions,
            "Use number selection. Register or login to access role-based workspaces.");
    }

    private static void HandleExit(Action requestExit)
    {
        if (ConfirmationPrompt.AskYesNo("Exit CommerceConsole now?", false))
        {
            requestExit();
            ConsoleTheme.WriteInfo("Session ended. Goodbye.");
        }
        else
        {
            ConsoleTheme.WriteInfo("Exit cancelled.");
        }
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

        IUserWorkspace? workspace = _roleMenuFactory.Resolve(_sessionContext.CurrentUser.Role);
        if (workspace is null)
        {
            ConsoleTheme.WriteWarning("Unsupported user role. Signing out.");
            _sessionContext.SignOut();
            return;
        }

        ConsoleTheme.WriteInfo($"Opening {workspace.SupportedRole} workspace...");
        workspace.Run(_sessionContext);
    }
}
