using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for product catalog workflows.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Returns active products for customer browsing.
    /// </summary>
    List<Product> GetActiveProducts();

    /// <summary>
    /// Searches products by term.
    /// </summary>
    List<Product> SearchProducts(string term);
}
