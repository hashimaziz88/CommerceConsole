using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Enums;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Placeholder menu for customer actions.
/// </summary>
public sealed class CustomerMenu
{
    /// <summary>
    /// Runs a minimal customer navigation loop.
    /// </summary>
    public void Run(ISessionContext sessionContext)
    {
        if (sessionContext.CurrentUser?.Role != UserRole.Customer)
        {
            Console.WriteLine("Access denied. Customer login required.");
            return;
        }

        bool done = false;
        while (!done)
        {
            Console.WriteLine("=== Customer Menu ===");
            Console.WriteLine("1. View Profile Summary");
            Console.WriteLine("2. Logout");
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine($"Logged in as customer: {sessionContext.CurrentUser.FullName}");
                    break;
                case "2":
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter 1 or 2.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
