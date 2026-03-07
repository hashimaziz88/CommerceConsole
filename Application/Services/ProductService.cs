using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements catalog query workflows.
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
        return _productRepository.GetAll().Where(p => p.IsActive).ToList();
    }

    /// <inheritdoc />
    public List<Product> SearchProducts(string term)
    {
        return _productRepository.Search(term).Where(p => p.IsActive).ToList();
    }
}
