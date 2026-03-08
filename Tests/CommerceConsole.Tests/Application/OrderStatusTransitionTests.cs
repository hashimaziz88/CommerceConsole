using CommerceConsole.Application.Services;
using CommerceConsole.Application.Services.OrderTransitions;
using CommerceConsole.Application.Services.Payments;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Infrastructure.Repositories;
using Xunit;

namespace CommerceConsole.Tests.Application;

/// <summary>
/// Tests valid and invalid order status transitions.
/// </summary>
public sealed class OrderStatusTransitionTests
{
    /// <summary>
    /// Verifies valid status transitions are allowed in sequence.
    /// </summary>
    [Fact]
    public void UpdateOrderStatus_WithValidSequence_UpdatesSuccessfully()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            TransitionContext context = CreateContextWithPaidOrder(dataDirectory);

            context.OrderService.UpdateOrderStatus(context.Order.Id, OrderStatus.Processing);
            context.OrderService.UpdateOrderStatus(context.Order.Id, OrderStatus.Shipped);
            context.OrderService.UpdateOrderStatus(context.Order.Id, OrderStatus.Delivered);

            Order? updated = context.OrderRepository.GetById(context.Order.Id);
            Assert.NotNull(updated);
            Assert.Equal(OrderStatus.Delivered, updated!.Status);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies invalid status jumps are rejected.
    /// </summary>
    [Fact]
    public void UpdateOrderStatus_WithInvalidTransition_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            TransitionContext context = CreateContextWithPaidOrder(dataDirectory);

            Assert.Throws<ValidationException>(() =>
                context.OrderService.UpdateOrderStatus(context.Order.Id, OrderStatus.Delivered));

            Order? unchanged = context.OrderRepository.GetById(context.Order.Id);
            Assert.NotNull(unchanged);
            Assert.Equal(OrderStatus.Paid, unchanged!.Status);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    /// <summary>
    /// Verifies terminal states cannot transition further.
    /// </summary>
    [Fact]
    public void UpdateOrderStatus_FromTerminalState_ThrowsValidationException()
    {
        string dataDirectory = CreateTempDataDirectory();

        try
        {
            TransitionContext context = CreateContextWithPaidOrder(dataDirectory);
            context.OrderService.UpdateOrderStatus(context.Order.Id, OrderStatus.Cancelled);

            Assert.Throws<ValidationException>(() =>
                context.OrderService.UpdateOrderStatus(context.Order.Id, OrderStatus.Processing));

            Order? unchanged = context.OrderRepository.GetById(context.Order.Id);
            Assert.NotNull(unchanged);
            Assert.Equal(OrderStatus.Cancelled, unchanged!.Status);
        }
        finally
        {
            DeleteDirectoryIfExists(dataDirectory);
        }
    }

    private static TransitionContext CreateContextWithPaidOrder(string dataDirectory)
    {
        InMemoryUserRepository userRepository = new(dataDirectory);
        InMemoryProductRepository productRepository = new(dataDirectory);
        InMemoryOrderRepository orderRepository = new(dataDirectory);

        Guid orderId = Guid.NewGuid();
        Payment payment = new(Guid.NewGuid(), orderId, 300m, "Wallet");
        payment.MarkCompleted();

        List<OrderItem> items = new()
        {
            new OrderItem(Guid.NewGuid(), "Keyboard", 150m, 2)
        };

        Order order = new(orderId, Guid.NewGuid(), items, payment);
        order.UpdateStatus(OrderStatus.Paid);
        orderRepository.Add(order);

        WalletPaymentStrategy paymentStrategy = new();
        OrderTransitionStateFactory transitionStateFactory = new();
        OrderService orderService = new(orderRepository, productRepository, userRepository, paymentStrategy, transitionStateFactory);

        return new TransitionContext(orderService, orderRepository, order);
    }

    private static string CreateTempDataDirectory()
    {
        string path = Path.Combine(Path.GetTempPath(), "CommerceConsoleTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    private sealed record TransitionContext(
        OrderService OrderService,
        InMemoryOrderRepository OrderRepository,
        Order Order);
}
