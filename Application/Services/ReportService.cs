using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Domain.Specifications;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements aggregate reporting workflows.
/// </summary>
public sealed class ReportService : IReportService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes the report service.
    /// </summary>
    public ReportService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    /// <inheritdoc />
    public decimal GetTotalRevenue()
    {
        return _orderRepository.GetAll().Sum(order => order.TotalAmount);
    }

    /// <inheritdoc />
    public Dictionary<OrderStatus, int> GetOrdersByStatus()
    {
        Dictionary<OrderStatus, int> counts = _orderRepository.GetAll()
            .GroupBy(order => order.Status)
            .ToDictionary(group => group.Key, group => group.Count());

        foreach (OrderStatus status in Enum.GetValues<OrderStatus>())
        {
            if (!counts.ContainsKey(status))
            {
                counts[status] = 0;
            }
        }

        return counts;
    }

    /// <inheritdoc />
    public List<ProductSalesReportItem> GetBestSellingProducts(int topCount)
    {
        if (topCount <= 0)
        {
            throw new ValidationException("Top count must be greater than zero.");
        }

        return _orderRepository.GetAll()
            .SelectMany(order => order.Items)
            .GroupBy(item => item.ProductId)
            .Select(group => new ProductSalesReportItem(
                group.First().ProductName,
                group.Sum(item => item.Quantity),
                group.Sum(item => item.LineTotal)))
            .OrderByDescending(item => item.TotalQuantitySold)
            .ThenByDescending(item => item.TotalRevenue)
            .ThenBy(item => item.ProductName)
            .Take(topCount)
            .ToList();
    }

    /// <inheritdoc />
    public List<LowStockReportItem> GetLowStockProducts(int threshold)
    {
        if (threshold < 0)
        {
            throw new ValidationException("Low-stock threshold cannot be negative.");
        }

        LowStockProductSpecification specification = new(threshold);

        return _productRepository.Find(specification)
            .OrderBy(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .Select(product => new LowStockReportItem(
                product.Name,
                product.Category,
                product.StockQuantity,
                product.Price,
                product.IsActive))
            .ToList();
    }
}
