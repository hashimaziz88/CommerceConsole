using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Matches products by case-insensitive name/category term.
/// </summary>
public sealed class SearchProductSpecification : ISpecification<Product>
{
    private readonly string _normalizedTerm;

    /// <summary>
    /// Initializes a search specification from a text term.
    /// </summary>
    public SearchProductSpecification(string term)
    {
        _normalizedTerm = term?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    /// <inheritdoc />
    public bool IsSatisfiedBy(Product candidate)
    {
        if (candidate is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(_normalizedTerm))
        {
            return true;
        }

        return candidate.Name.ToLowerInvariant().Contains(_normalizedTerm) ||
               candidate.Category.ToLowerInvariant().Contains(_normalizedTerm);
    }
}
