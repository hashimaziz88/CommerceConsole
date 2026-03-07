using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for review workflows.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Adds a new review.
    /// </summary>
    void AddReview(Product product, Review review);

    /// <summary>
    /// Returns reviews for a product.
    /// </summary>
    List<Review> GetProductReviews(Product product);
}
