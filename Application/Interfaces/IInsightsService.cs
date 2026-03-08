using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Entities;

namespace CommerceConsole.Application.Interfaces;

/// <summary>
/// Contract for bonus recommendation and analytics insight workflows.
/// </summary>
public interface IInsightsService
{
    /// <summary>
    /// Returns customer-facing product recommendations.
    /// </summary>
    List<ProductRecommendationItem> GetCustomerRecommendations(Customer customer, int maxCount);

    /// <summary>
    /// Returns administrator-facing heuristic insights.
    /// </summary>
    List<string> GetAdminInsights(int lowStockThreshold);
}