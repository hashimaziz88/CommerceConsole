using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Domain.Specifications;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements product browsing and catalog management workflows.
/// </summary>
public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes the product service.
    /// </summary>
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <inheritdoc />
    public List<Product> GetActiveProducts()
    {
        ActiveProductSpecification specification = new();

        return _productRepository
            .Find(specification)
            .OrderBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public List<Product> SearchProducts(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return GetActiveProducts();
        }

        ISpecification<Product> specification = new AndSpecification<Product>(
            new ActiveProductSpecification(),
            new SearchProductSpecification(term));

        return _productRepository
            .Find(specification)
            .OrderBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public List<Product> GetAllProducts()
    {
        return _productRepository
            .GetAll()
            .OrderBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public List<Product> GetLowStockProducts(int threshold)
    {
        if (threshold < 0)
        {
            throw new ValidationException("Low-stock threshold cannot be negative.");
        }

        LowStockProductSpecification specification = new(threshold);

        return _productRepository
            .Find(specification)
            .OrderBy(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .ToList();
    }

    /// <inheritdoc />
    public Product AddProduct(string name, string description, string category, decimal price, int stockQuantity)
    {
        Product product = new(
            Guid.NewGuid(),
            name,
            description,
            category,
            price,
            stockQuantity);

        _productRepository.Add(product);
        return product;
    }

    /// <inheritdoc />
    public void UpdateProduct(Guid productId, string name, string description, string category, decimal price)
    {
        Product product = FindProductOrThrow(productId);
        product.UpdateDetails(name, description, category, price);
        _productRepository.Update(product);
    }

    /// <inheritdoc />
    public void DeleteProduct(Guid productId)
    {
        _ = FindProductOrThrow(productId);
        _productRepository.Remove(productId);
    }

    /// <inheritdoc />
    public void RestockProduct(Guid productId, int quantity)
    {
        Product product = FindProductOrThrow(productId);
        product.Restock(quantity);
        _productRepository.Update(product);
    }

    private Product FindProductOrThrow(Guid productId)
    {
        Product? product = _productRepository.GetById(productId);
        if (product is null)
        {
            throw new NotFoundException("Product was not found.");
        }

        return product;
    }
}
