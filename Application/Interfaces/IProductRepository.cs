using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for product data operations.
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Searches products by a text term.
    /// </summary>
    List<Product> Search(string term);

    /// <summary>
    /// Returns products below a low-stock threshold.
    /// </summary>
    List<Product> GetLowStockProducts(int threshold);
}
