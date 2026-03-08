using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Combines two specifications with logical OR.
/// </summary>
public sealed class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    /// Initializes a composite OR specification.
    /// </summary>
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left ?? throw new ValidationException("Left specification is required.");
        _right = right ?? throw new ValidationException("Right specification is required.");
    }

    /// <inheritdoc />
    public bool IsSatisfiedBy(T candidate)
    {
        return _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
    }
}
