using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Inverts a specification result with logical NOT.
/// </summary>
public sealed class NotSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _inner;

    /// <summary>
    /// Initializes a NOT specification.
    /// </summary>
    public NotSpecification(ISpecification<T> inner)
    {
        _inner = inner ?? throw new ValidationException("Inner specification is required.");
    }

    /// <inheritdoc />
    public bool IsSatisfiedBy(T candidate)
    {
        return !_inner.IsSatisfiedBy(candidate);
    }
}
