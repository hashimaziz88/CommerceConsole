using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Infrastructure.Persistence;
using CommerceConsole.Infrastructure.Repositories.Models;

namespace CommerceConsole.Infrastructure.Repositories;

/// <summary>
/// In-memory product repository with JSON persistence.
/// </summary>
public sealed class InMemoryProductRepository : IProductRepository
{
    private const string FileName = "products.json";

    private readonly JsonFileStore _fileStore;
    private readonly List<Product> _products;

    /// <summary>
    /// Initializes the product repository.
    /// </summary>
    public InMemoryProductRepository(string? dataDirectory = null)
    {
        _fileStore = new JsonFileStore(dataDirectory);
        _products = LoadProducts();
    }

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
        Persist();
    }

    /// <inheritdoc />
    public void Update(Product entity)
    {
        int index = _products.FindIndex(product => product.Id == entity.Id);
        if (index >= 0)
        {
            _products[index] = entity;
            Persist();
        }
    }

    /// <inheritdoc />
    public void Remove(Guid id)
    {
        Product? existing = GetById(id);
        if (existing is not null)
        {
            _products.Remove(existing);
            Persist();
        }
    }

    private List<Product> LoadProducts()
    {
        List<ProductRecord> records = _fileStore.LoadList<ProductRecord>(FileName);
        return records.Select(ToDomain).ToList();
    }

    private void Persist()
    {
        List<ProductRecord> records = _products.Select(FromDomain).ToList();
        _fileStore.SaveList(FileName, records);
    }

    private static Product ToDomain(ProductRecord record)
    {
        Product product = new(
            record.Id,
            record.Name,
            record.Description,
            record.Category,
            record.Price,
            record.StockQuantity);

        if (!record.IsActive)
        {
            product.Deactivate();
        }

        foreach (ProductReviewRecord reviewRecord in record.Reviews)
        {
            Review review = new(
                reviewRecord.Id,
                reviewRecord.ProductId,
                reviewRecord.CustomerId,
                reviewRecord.Rating,
                reviewRecord.Comment);
            product.Reviews.Add(review);
        }

        return product;
    }

    private static ProductRecord FromDomain(Product product)
    {
        return new ProductRecord
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            Reviews = product.Reviews.Select(review => new ProductReviewRecord
            {
                Id = review.Id,
                ProductId = review.ProductId,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment
            }).ToList()
        };
    }
}
