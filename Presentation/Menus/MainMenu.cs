using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Exceptions;

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
            Console.Write("Select an option: ");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    RegisterCustomer();
                    break;
                case "2":
                    LoginAndRoute();
                    break;
                case "3":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter 1, 2, or 3.");
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
        Console.Write("Full name: ");
        string fullName = Console.ReadLine() ?? string.Empty;

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? string.Empty;

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? string.Empty;

        try
        {
            _authService.RegisterCustomer(fullName, email, password);
            Console.WriteLine("Registration successful. You can now log in.");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (DuplicateEmailException ex)
        {
            Console.WriteLine($"Registration error: {ex.Message}");
        }
    }

    private void LoginAndRoute()
    {
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? string.Empty;

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? string.Empty;

        try
        {
            var user = _authService.Login(email, password);
            _sessionContext.SignIn(user);
            Console.WriteLine($"Welcome, {user.FullName} ({user.Role}).");
            RouteByRole();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (AuthenticationException ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
        }
    }

    private void RouteByRole()
    {
        if (_sessionContext.CurrentUser is null)
        {
            return;
        }

        switch (_sessionContext.CurrentUser.Role)
        {
            case Domain.Enums.UserRole.Customer:
                _customerMenu.Run(_sessionContext);
                break;
            case Domain.Enums.UserRole.Administrator:
                _adminMenu.Run(_sessionContext);
                break;
            default:
                Console.WriteLine("Unsupported user role.");
                _sessionContext.SignOut();
                break;
        }
    }
}
