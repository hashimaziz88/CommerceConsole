using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Infrastructure.Repositories;

/// <summary>
/// In-memory user repository used for bootstrap and testing.
/// </summary>
public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    /// <inheritdoc />
    public List<User> GetAll()
    {
        return _users.ToList();
    }

    /// <inheritdoc />
    public User? GetById(Guid id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }

    /// <inheritdoc />
    public User? GetByEmail(string email)
    {
        return _users.FirstOrDefault(user =>
            user.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public void Add(User entity)
    {
        _users.Add(entity);
    }

    /// <inheritdoc />
    public void Update(User entity)
    {
        int index = _users.FindIndex(user => user.Id == entity.Id);
        if (index >= 0)
        {
            _users[index] = entity;
        }
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        User? existing = GetById(id);
        if (existing is not null)
        {
            _users.Remove(existing);
        }
    }
}
