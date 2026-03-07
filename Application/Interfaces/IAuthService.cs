using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for authentication workflows.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new customer.
    /// </summary>
    Customer RegisterCustomer(string fullName, string email, string password);

    /// <summary>
    /// Authenticates a user by email and password.
    /// </summary>
    User Login(string email, string password);
}
