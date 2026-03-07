using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for user data operations.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Returns a user by email when found.
    /// </summary>
    User? GetByEmail(string email);
}
