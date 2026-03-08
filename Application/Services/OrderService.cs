using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements order and checkout workflows.
/// </summary>
public sealed class OrderService : IOrderService
{
    private const string WalletPaymentMethod = "Wallet";

    private static readonly Dictionary<OrderStatus, IReadOnlyList<OrderStatus>> AllowedTransitions = new()
    {
        [OrderStatus.Pending] = new[] { OrderStatus.Paid, OrderStatus.Cancelled },
        [OrderStatus.Paid] = new[] { OrderStatus.Processing, OrderStatus.Cancelled },
        [OrderStatus.Processing] = new[] { OrderStatus.Shipped, OrderStatus.Cancelled },
        [OrderStatus.Shipped] = new[] { OrderStatus.Delivered },
        [OrderStatus.Delivered] = Array.Empty<OrderStatus>(),
        [OrderStatus.Cancelled] = Array.Empty<OrderStatus>()
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes the order service.
    /// </summary>
    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public Order Checkout(Customer customer)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required for checkout.");
        }

        List<CartItem> cartItems = customer.Cart.Items.ToList();
        if (cartItems.Count == 0)
        {
            throw new ValidationException("Cannot checkout an empty cart.");
        }

        List<ProductCheckoutLine> checkoutLines = BuildCheckoutLines(cartItems);

        decimal totalAmount = customer.Cart.CalculateTotal();
        if (customer.WalletBalance < totalAmount)
        {
            throw new InsufficientFundsException("Insufficient wallet funds for checkout.");
        }

        Guid orderId = Guid.NewGuid();
        Payment payment = new(Guid.NewGuid(), orderId, totalAmount, WalletPaymentMethod);

        customer.DebitFunds(totalAmount);

        foreach (ProductCheckoutLine line in checkoutLines)
        {
            line.Product.ReduceStock(line.Quantity);
            _productRepository.Update(line.Product);
        }

        payment.MarkCompleted();

        List<OrderItem> orderItems = cartItems.Select(item =>
            new OrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity)).ToList();

        Order order = new(orderId, customer.Id, orderItems, payment);
        order.UpdateStatus(OrderStatus.Paid);

        _orderRepository.Add(order);

        customer.Orders.Add(order);
        customer.Cart.Clear();
        _userRepository.Update(customer);

        return order;
    }

    /// <inheritdoc />
    public List<Order> GetCustomerOrders(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            throw new ValidationException("Customer ID must be valid.");
        }

        return _orderRepository
            .GetByCustomerId(customerId)
            .OrderByDescending(order => order.CreatedAt)
            .ToList();
    }

    /// <inheritdoc />
    public List<Order> GetAllOrders()
    {
        return _orderRepository
            .GetAll()
            .OrderByDescending(order => order.CreatedAt)
            .ToList();
    }

    /// <inheritdoc />
    public IReadOnlyList<OrderStatus> GetAllowedTransitions(OrderStatus currentStatus)
    {
        return AllowedTransitions.TryGetValue(currentStatus, out IReadOnlyList<OrderStatus>? transitions)
            ? transitions
            : Array.Empty<OrderStatus>();
    }

    /// <inheritdoc />
    public void UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        if (orderId == Guid.Empty)
        {
            throw new ValidationException("Order ID must be valid.");
        }

        Order order = GetOrderOrThrow(orderId);

        if (order.Status == status)
        {
            return;
        }

        IReadOnlyList<OrderStatus> allowed = GetAllowedTransitions(order.Status);
        if (!allowed.Contains(status))
        {
            throw new ValidationException(
                $"Invalid status transition from '{order.Status}' to '{status}'.");
        }

        order.UpdateStatus(status);
        _orderRepository.Update(order);
    }

    private List<ProductCheckoutLine> BuildCheckoutLines(IEnumerable<CartItem> cartItems)
    {
        List<ProductCheckoutLine> lines = new();

        foreach (CartItem item in cartItems)
        {
            Product product = GetProductOrThrow(item.ProductId);

            if (!product.IsActive)
            {
                throw new ValidationException($"Cannot checkout inactive product '{product.Name}'.");
            }

            if (item.Quantity > product.StockQuantity)
            {
                throw new InsufficientStockException($"Insufficient stock for product '{product.Name}'.");
            }

            lines.Add(new ProductCheckoutLine(product, item.Quantity));
        }

        return lines;
    }

    private Product GetProductOrThrow(Guid productId)
    {
        Product? product = _productRepository.GetById(productId);
        if (product is null)
        {
            throw new NotFoundException("Product was not found during checkout.");
        }

        return product;
    }

    private Order GetOrderOrThrow(Guid orderId)
    {
        Order? order = _orderRepository.GetById(orderId);
        if (order is null)
        {
            throw new NotFoundException("Order was not found.");
        }

        return order;
    }

    private sealed record ProductCheckoutLine(Product Product, int Quantity);
}