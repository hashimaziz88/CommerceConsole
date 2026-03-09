using CommerceConsole.Application.Interfaces;
using CommerceConsole.Presentation.Commands;
using CommerceConsole.Presentation.Helpers;
using CommerceConsole.Presentation.Workspaces;

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
    private readonly IRoleWorkspaceFactory _workspaceFactory;
    private readonly MenuCommandDispatcher _dispatcher;

    /// <summary>
    /// Initializes the main menu.
    /// </summary>
    public MainMenu(
        IAuthService authService,
        ISessionContext sessionContext,
        IRoleWorkspaceFactory workspaceFactory)
    {
        _authService = authService;
        _sessionContext = sessionContext;
        _workspaceFactory = workspaceFactory;
        _dispatcher = BuildDispatcher();
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
            MenuCommandResult result = _dispatcher.Dispatch(selection);

            if (result == MenuCommandResult.ExitMenu)
            {
                exitRequested = true;
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

    private MenuCommandDispatcher BuildDispatcher()
    {
        return new MenuCommandDispatcher(new Dictionary<int, IMenuCommand>
        {
            [1] = new DelegateMenuCommand(() => MenuActionHelper.Execute(RegisterCustomer)),
            [2] = new MainLoginRouteCommand(() => MenuActionHelper.Execute(LoginAndRoute)),
            [3] = new MainExitMenuCommand(RequestExit)
        });
    }

    private bool RequestExit()
    {
        if (ConfirmationPrompt.AskYesNo("Exit CommerceConsole now?", false))
        {
            ConsoleTheme.WriteInfo("Session ended. Goodbye.");
            return true;
        }

        ConsoleTheme.WriteInfo("Exit cancelled.");
        return false;
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

        if (!_workspaceFactory.TryResolve(_sessionContext.CurrentUser.Role, out IUserWorkspace workspace))
        {
            ConsoleTheme.WriteWarning("Unsupported user role. Signing out.");
            _sessionContext.SignOut();
            return;
        }

        ConsoleTheme.WriteInfo($"Opening {_sessionContext.CurrentUser.Role.ToString().ToLowerInvariant()} workspace...");
        workspace.Run(_sessionContext);
    }
}
