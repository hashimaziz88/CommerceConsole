using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Presentation.Helpers;

/// <summary>
/// Executes menu actions with consistent, user-friendly error handling.
/// </summary>
public static class MenuActionHelper
{
    /// <summary>
    /// Runs an action and maps known exceptions to friendly console messages.
    /// </summary>
    public static void Execute(Action action)
    {
        try
        {
            action();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (DuplicateEmailException ex)
        {
            Console.WriteLine($"Registration error: {ex.Message}");
        }
        catch (AuthenticationException ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Not found: {ex.Message}");
        }
        catch (InsufficientStockException ex)
        {
            Console.WriteLine($"Stock error: {ex.Message}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Funds error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}