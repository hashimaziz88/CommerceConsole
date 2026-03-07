using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for review workflows.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Returns products the customer is eligible to review.
    /// </summary>
    List<Product> GetReviewableProducts(Customer customer);

    /// <summary>
    /// Adds a customer review for a product.
    /// </summary>
    void AddReview(Customer customer, Guid productId, int rating, string comment);

    /// <summary>
    /// Returns reviews for a product.
    /// </summary>
    List<Review> GetProductReviews(Guid productId);

    /// <summary>
    /// Returns average rating for a product.
    /// </summary>
    double GetAverageRating(Guid productId);
}
