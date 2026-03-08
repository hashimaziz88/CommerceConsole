using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Specifications;
using CommerceConsole.Infrastructure.Persistence;
using CommerceConsole.Infrastructure.Repositories.Models;

namespace CommerceConsole.Infrastructure.Repositories;

/// <summary>
/// In-memory user repository with JSON persistence.
/// </summary>
public sealed class InMemoryUserRepository : IUserRepository
{
    private const string FileName = "users.json";

    private readonly JsonFileStore _fileStore;
    private readonly List<User> _users;

    /// <summary>
    /// Initializes the user repository.
    /// </summary>
    public InMemoryUserRepository(string? dataDirectory = null)
    {
        _fileStore = new JsonFileStore(dataDirectory);
        _users = LoadUsers();
    }

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
        Persist();
    }

    /// <inheritdoc />
    public void Update(User entity)
    {
        int index = _users.FindIndex(user => user.Id == entity.Id);
        if (index >= 0)
        {
            _users[index] = entity;
            Persist();
        }
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        User? existing = GetById(id);
        if (existing is not null)
        {
            _users.Remove(existing);
            Persist();
        }
    }

    /// <inheritdoc />
    public List<User> Find(ISpecification<User> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);
        return _users.Where(specification.IsSatisfiedBy).ToList();
    }

    private List<User> LoadUsers()
    {
        List<UserRecord> records = _fileStore.LoadList<UserRecord>(FileName);
        return records.Select(ToDomain).ToList();
    }

    private void Persist()
    {
        List<UserRecord> records = _users.Select(FromDomain).ToList();
        _fileStore.SaveList(FileName, records);
    }

    private static User ToDomain(UserRecord record)
    {
        if (record.Role == UserRole.Administrator)
        {
            return new Administrator(record.Id, record.FullName, record.Email, record.Password);
        }

        Customer customer = new(record.Id, record.FullName, record.Email, record.Password);

        if (record.WalletBalance > 0)
        {
            customer.AddFunds(record.WalletBalance);
        }

        foreach (UserCartItemRecord item in record.CartItems)
        {
            customer.Cart.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
        }

        return customer;
    }

    private static UserRecord FromDomain(User user)
    {
        List<UserCartItemRecord> cartItems = new();
        decimal walletBalance = 0m;

        if (user is Customer customer)
        {
            walletBalance = customer.WalletBalance;
            cartItems = customer.Cart.Items.Select(item => new UserCartItemRecord
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            }).ToList();
        }

        return new UserRecord
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role,
            WalletBalance = walletBalance,
            CartItems = cartItems
        };
    }
}
