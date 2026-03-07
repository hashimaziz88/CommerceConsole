using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements review workflows.
/// </summary>
public sealed class ReviewService : IReviewService
{
    /// <inheritdoc />
    public void AddReview(Product product, Review review)
    {
        product.Reviews.Add(review);
    }

    /// <inheritdoc />
    public List<Review> GetProductReviews(Product product)
    {
        return product.Reviews.ToList();
    }
}
