using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Matches products that are currently in stock.
/// </summary>
public sealed class InStockProductSpecification : ISpecification<Product>
{
    /// <inheritdoc />
    public bool IsSatisfiedBy(Product candidate)
    {
        return candidate is not null && candidate.StockQuantity > 0;
    }
}
