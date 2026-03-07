using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Domain.Entities;

/// <summary>
/// Represents a product review submitted by a customer.
/// </summary>
public sealed class Review
{
    /// <summary>
    /// Initializes a review.
    /// </summary>
    public Review(Guid id, Guid productId, Guid customerId, int rating, string comment)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("Review ID must be valid.");
        }

        if (productId == Guid.Empty)
        {
            throw new ValidationException("Product ID must be valid.");
        }

        if (customerId == Guid.Empty)
        {
            throw new ValidationException("Customer ID must be valid.");
        }

        if (rating < 1 || rating > 5)
        {
            throw new ValidationException("Rating must be between 1 and 5.");
        }

        Id = id;
        ProductId = productId;
        CustomerId = customerId;
        Rating = rating;
        Comment = comment?.Trim() ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets review ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets reviewed product ID.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets author customer ID.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets rating value.
    /// </summary>
    public int Rating { get; }

    /// <summary>
    /// Gets optional review comment.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    /// Gets review creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; }
}
