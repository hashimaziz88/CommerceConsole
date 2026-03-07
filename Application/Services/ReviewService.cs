using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements review workflows.
/// </summary>
public sealed class ReviewService : IReviewService
{
    private static readonly HashSet<OrderStatus> ReviewEligibleOrderStatuses = new()
    {
        OrderStatus.Paid,
        OrderStatus.Processing,
        OrderStatus.Shipped,
        OrderStatus.Delivered
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes review service dependencies.
    /// </summary>
    public ReviewService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
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

        if (!HasPurchasedProduct(customer.Id, productId))
        {
            throw new ValidationException("You can only review products you have purchased.");
        }

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

    private bool HasPurchasedProduct(Guid customerId, Guid productId)
    {
        return _orderRepository.GetByCustomerId(customerId)
            .Where(order => ReviewEligibleOrderStatuses.Contains(order.Status))
            .SelectMany(order => order.Items)
            .Any(item => item.ProductId == productId);
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
