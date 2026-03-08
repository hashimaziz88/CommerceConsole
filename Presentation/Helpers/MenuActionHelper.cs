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
            ConsoleTheme.WriteError($"Validation: {ex.Message}");
        }
        catch (DuplicateEmailException ex)
        {
            ConsoleTheme.WriteError($"Registration: {ex.Message}");
        }
        catch (AuthenticationException ex)
        {
            ConsoleTheme.WriteError($"Login: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            ConsoleTheme.WriteError($"Not found: {ex.Message}");
        }
        catch (InsufficientStockException ex)
        {
            ConsoleTheme.WriteError($"Stock: {ex.Message}");
        }
        catch (InsufficientFundsException ex)
        {
            ConsoleTheme.WriteError($"Funds: {ex.Message}");
        }
        catch (Exception)
        {
            ConsoleTheme.WriteError("Unexpected error occurred. Please try again.");
        }
    }
}