using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Matches products with stock quantity at or below a threshold.
/// </summary>
public sealed class LowStockProductSpecification : ISpecification<Product>
{
    private readonly int _threshold;

    /// <summary>
    /// Initializes a low-stock specification.
    /// </summary>
    public LowStockProductSpecification(int threshold)
    {
        if (threshold < 0)
        {
            throw new ValidationException("Low-stock threshold cannot be negative.");
        }

        _threshold = threshold;
    }

    /// <inheritdoc />
    public bool IsSatisfiedBy(Product candidate)
    {
        return candidate is not null && candidate.StockQuantity <= _threshold;
    }
}
