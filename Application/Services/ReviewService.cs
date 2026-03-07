using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements review workflows.
/// </summary>
public sealed class ReviewService : IReviewService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes review service dependencies.
    /// </summary>
    public ReviewService(IProductRepository productRepository, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public void AddReview(Customer customer, Guid productId, int rating, string comment)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required to add a review.");
        }

        Product product = GetProductOrThrow(productId);

        Review review = new(Guid.NewGuid(), product.Id, customer.Id, rating, comment);
        product.Reviews.Add(review);
        customer.Reviews.Add(review);

        _productRepository.Update(product);
        _userRepository.Update(customer);
    }

    /// <inheritdoc />
    public List<Review> GetProductReviews(Guid productId)
    {
        Product product = GetProductOrThrow(productId);
        return product.Reviews.ToList();
    }

    /// <inheritdoc />
    public double GetAverageRating(Guid productId)
    {
        Product product = GetProductOrThrow(productId);
        return product.Reviews.Count == 0 ? 0 : product.Reviews.Average(review => review.Rating);
    }

    private Product GetProductOrThrow(Guid productId)
    {
        Product? product = _productRepository.GetById(productId);
        if (product is null)
        {
            throw new NotFoundException("Product was not found.");
        }

        return product;
    }
}
