using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Infrastructure.Repositories;

/// <summary>
/// In-memory product repository used for bootstrap and testing.
/// </summary>
public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    /// <inheritdoc />
    public List<Product> GetAll()
    {
        return _products.ToList();
    }

    /// <inheritdoc />
    public Product? GetById(Guid id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    /// <inheritdoc />
    public List<Product> Search(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return GetAll();
        }

        string normalized = term.Trim().ToLowerInvariant();
        return _products.Where(product =>
                product.Name.ToLowerInvariant().Contains(normalized) ||
                product.Category.ToLowerInvariant().Contains(normalized))
            .ToList();
    }

    /// <inheritdoc />
    public List<Product> GetLowStockProducts(int threshold)
    {
        return _products.Where(product => product.StockQuantity <= threshold).ToList();
    }

    /// <inheritdoc />
    public void Add(Product entity)
    {
        _products.Add(entity);
    }

    /// <inheritdoc />
    public void Update(Product entity)
    {
        int index = _products.FindIndex(product => product.Id == entity.Id);
        if (index >= 0)
        {
            _products[index] = entity;
        }
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        Product? existing = GetById(id);
        if (existing is not null)
        {
            _products.Remove(existing);
        }
    }
}
