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
    /// Searches active products by term.
    /// </summary>
    List<Product> SearchProducts(string term);

    /// <summary>
    /// Returns all products for administrator management.
    /// </summary>
    List<Product> GetAllProducts();

    /// <summary>
    /// Returns products below a low-stock threshold.
    /// </summary>
    List<Product> GetLowStockProducts(int threshold);

    /// <summary>
    /// Adds a new product.
    /// </summary>
    Product AddProduct(string name, string description, string category, decimal price, int stockQuantity);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    void UpdateProduct(Guid productId, string name, string description, string category, decimal price);

    /// <summary>
    /// Deletes a product by ID.
    /// </summary>
    void DeleteProduct(Guid productId);

    /// <summary>
    /// Restocks an existing product.
    /// </summary>
    void RestockProduct(Guid productId, int quantity);
}
