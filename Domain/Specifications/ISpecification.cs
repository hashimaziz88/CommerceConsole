namespace CommerceConsole.Domain.Specifications;

/// <summary>
/// Defines a reusable predicate for domain-level filtering rules.
/// </summary>
public interface ISpecification<in T>
{
    /// <summary>
    /// Returns true when the candidate satisfies this specification.
    /// </summary>
    bool IsSatisfiedBy(T candidate);
}
