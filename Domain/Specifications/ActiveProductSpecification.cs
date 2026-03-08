using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Matches only active products.
/// </summary>
public sealed class ActiveProductSpecification : ISpecification<Product>
{
    /// <inheritdoc />
    public bool IsSatisfiedBy(Product candidate)
    {
        return candidate is not null && candidate.IsActive;
    }
}
