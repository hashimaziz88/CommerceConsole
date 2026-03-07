using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements registration and login workflows.
/// </summary>
public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes the auth service.
    /// </summary>
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public Customer RegisterCustomer(string fullName, string email, string password)
    {
        if (_userRepository.GetByEmail(email) is not null)
        {
            throw new DuplicateEmailException("A user with this email already exists.");
        }

        Customer customer = new(Guid.NewGuid(), fullName, email, password);
        _userRepository.Add(customer);
        return customer;
    }

    /// <inheritdoc />
    public User Login(string email, string password)
    {
        User? user = _userRepository.GetByEmail(email);
        if (user is null || !user.VerifyPassword(password))
        {
            throw new AuthenticationException("Invalid email or password.");
        }

        return user;
    }
}
