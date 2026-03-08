using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Provides heuristic insights and recommendation workflows.
/// </summary>
public sealed class InsightsService : IInsightsService
{
    private static readonly HashSet<OrderStatus> PurchasedStatuses = new()
    {
        OrderStatus.Paid,
        OrderStatus.Processing,
        OrderStatus.Shipped,
        OrderStatus.Delivered
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes insights service dependencies.
    /// </summary>
    public InsightsService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    /// <inheritdoc />
    public List<ProductRecommendationItem> GetCustomerRecommendations(Customer customer, int maxCount)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required.");
        }

        if (maxCount <= 0)
        {
            throw new ValidationException("Recommendation count must be greater than zero.");
        }

        List<Product> allProducts = _productRepository.GetAll();
        Dictionary<Guid, Product> productsById = allProducts.ToDictionary(product => product.Id, product => product);

        List<Order> customerOrders = _orderRepository.GetByCustomerId(customer.Id)
            .Where(order => PurchasedStatuses.Contains(order.Status))
            .ToList();

        HashSet<Guid> purchasedProductIds = customerOrders
            .SelectMany(order => order.Items)
            .Select(item => item.ProductId)
            .ToHashSet();

        HashSet<string> preferredCategories = customerOrders
            .SelectMany(order => order.Items)
            .Select(item => productsById.TryGetValue(item.ProductId, out Product? product) ? product.Category : string.Empty)
            .Where(category => !string.IsNullOrWhiteSpace(category))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        IEnumerable<Product> baseCandidates = allProducts
            .Where(product => product.IsActive)
            .Where(product => product.StockQuantity > 0)
            .Where(product => !purchasedProductIds.Contains(product.Id));

        IEnumerable<Product> rankedCandidates = baseCandidates
            .OrderByDescending(product => preferredCategories.Contains(product.Category))
            .ThenByDescending(GetAverageRating)
            .ThenByDescending(product => product.StockQuantity)
            .ThenBy(product => product.Name);

        return rankedCandidates
            .Take(maxCount)
            .Select(product => new ProductRecommendationItem(
                product.Id,
                product.Name,
                product.Category,
                product.Price,
                GetAverageRating(product),
                BuildRecommendationReason(product, preferredCategories)))
            .ToList();
    }

    /// <inheritdoc />
    public List<string> GetAdminInsights(int lowStockThreshold)
    {
        if (lowStockThreshold < 0)
        {
            throw new ValidationException("Low-stock threshold cannot be negative.");
        }

        List<string> insights = new();
        List<Order> orders = _orderRepository.GetAll();
        List<Product> products = _productRepository.GetAll();
        Dictionary<Guid, Product> productsById = products.ToDictionary(product => product.Id, product => product);

        insights.Add("Insight mode: heuristic (rule-based), no external AI dependency required.");

        decimal revenue = orders.Sum(order => order.TotalAmount);
        insights.Add($"Revenue snapshot: {revenue:C} across {orders.Count} total orders.");

        var topCategory = orders
            .Where(order => PurchasedStatuses.Contains(order.Status))
            .SelectMany(order => order.Items)
            .Select(item => new
            {
                Category = productsById.TryGetValue(item.ProductId, out Product? product)
                    ? product.Category
                    : "Uncategorized",
                item.Quantity
            })
            .GroupBy(item => item.Category)
            .Select(group => new { Category = group.Key, Quantity = group.Sum(row => row.Quantity) })
            .OrderByDescending(group => group.Quantity)
            .ThenBy(group => group.Category)
            .FirstOrDefault();

        if (topCategory is null)
        {
            insights.Add("Top category: unavailable until at least one paid/processing/shipped/delivered order exists.");
        }
        else
        {
            insights.Add($"Top category by units sold: {topCategory.Category} ({topCategory.Quantity} units).");
        }

        List<Product> lowStockActive = products
            .Where(product => product.IsActive)
            .Where(product => product.StockQuantity <= lowStockThreshold)
            .OrderBy(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .ToList();

        if (lowStockActive.Count == 0)
        {
            insights.Add("Restock watch: no active products currently at or below the selected threshold.");
        }
        else
        {
            string watchList = string.Join(", ", lowStockActive.Take(3).Select(product => $"{product.Name} ({product.StockQuantity})"));
            insights.Add($"Restock watch: {lowStockActive.Count} active products are low. Priority: {watchList}.");
        }

        List<Review> reviews = products.SelectMany(product => product.Reviews).ToList();
        if (reviews.Count == 0)
        {
            insights.Add("Review sentiment: no reviews captured yet.");
        }
        else
        {
            double averageRating = reviews.Average(review => review.Rating);
            int positiveCount = reviews.Count(review => review.Rating >= 4);
            double positiveRate = (double)positiveCount / reviews.Count;
            insights.Add($"Review sentiment: average rating {averageRating:0.00}/5, positive share {positiveRate:P0}.");
        }

        int awaitingFulfillment = orders.Count(order =>
            order.Status == OrderStatus.Paid || order.Status == OrderStatus.Processing);
        insights.Add($"Operational alert: {awaitingFulfillment} orders are waiting fulfillment action.");

        return insights;
    }

    private static string BuildRecommendationReason(Product product, IReadOnlySet<string> preferredCategories)
    {
        double averageRating = GetAverageRating(product);

        if (preferredCategories.Contains(product.Category))
        {
            return $"Based on your previous purchases in {product.Category}.";
        }

        if (averageRating >= 4)
        {
            return "Highly rated by customers in recent activity.";
        }

        if (product.StockQuantity <= 3)
        {
            return "Trending item with limited remaining stock.";
        }

        return "Suggested from active catalog performance signals.";
    }

    private static double GetAverageRating(Product product)
    {
        if (product.Reviews.Count == 0)
        {
            return 0;
        }

        return product.Reviews.Average(review => review.Rating);
    }
}