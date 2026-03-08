namespace CommerceConsole.Application.Models;

/// <summary>
/// Represents a customer-facing recommendation row.
/// </summary>
public sealed record ProductRecommendationItem(
    Guid ProductId,
    string ProductName,
    string Category,
    decimal Price,
    double AverageRating,
    string Reason);