using System.Text.RegularExpressions;
using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements registration and login workflows.
/// </summary>
public sealed class AuthService : IAuthService
{
    private static readonly Regex BasicEmailPattern = new(
        "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

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
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ValidationException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email is required.");
        }

        if (!BasicEmailPattern.IsMatch(email.Trim()))
        {
            throw new ValidationException("Email format is invalid.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ValidationException("Password is required.");
        }

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
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ValidationException("Password is required.");
        }

        User? user = _userRepository.GetByEmail(email);
        if (user is null || !user.VerifyPassword(password))
        {
            throw new AuthenticationException("Invalid email or password.");
        }

        return user;
    }
}
