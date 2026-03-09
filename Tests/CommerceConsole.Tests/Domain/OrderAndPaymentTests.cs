using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using Xunit;

namespace CommerceConsole.Tests.Domain;

/// <summary>
/// Tests order and payment entity behavior.
/// </summary>
public sealed class OrderAndPaymentTests
{
    /// <summary>
    /// Verifies payment constructor rejects non-positive amounts.
    /// </summary>
    [Fact]
    public void PaymentConstructor_WithNonPositiveAmount_ThrowsValidationException()
    {
        Assert.Throws<ValidationException>(() =>
            _ = new Payment(Guid.NewGuid(), Guid.NewGuid(), 0m, "Wallet"));
    }

    /// <summary>
    /// Verifies completed payments capture completion state and timestamp.
    /// </summary>
    [Fact]
    public void MarkCompleted_SetsCompletedStatusAndPaidAt()
    {
        Payment payment = CreatePayment();

        payment.MarkCompleted();

        Assert.Equal(PaymentStatus.Completed, payment.Status);
        Assert.NotNull(payment.PaidAt);
    }

    /// <summary>
    /// Verifies failed payments set failed status.
    /// </summary>
    [Fact]
    public void MarkFailed_SetsFailedStatus()
    {
        Payment payment = CreatePayment();

        payment.MarkFailed();

        Assert.Equal(PaymentStatus.Failed, payment.Status);
    }

    /// <summary>
    /// Verifies orders require at least one item.
    /// </summary>
    [Fact]
    public void OrderConstructor_WithNoItems_ThrowsValidationException()
    {
        Payment payment = CreatePayment();

        Assert.Throws<ValidationException>(() =>
            _ = new Order(Guid.NewGuid(), Guid.NewGuid(), Array.Empty<OrderItem>(), payment));
    }

    /// <summary>
    /// Verifies order constructor computes total and starts in pending state.
    /// </summary>
    [Fact]
    public void OrderConstructor_WithValidItems_ComputesTotalAndStartsPending()
    {
        Payment payment = CreatePayment();
        List<OrderItem> items = new()
        {
            new OrderItem(Guid.NewGuid(), "Laptop", 100m, 2),
            new OrderItem(Guid.NewGuid(), "Mouse", 25m, 1)
        };

        Order order = new(Guid.NewGuid(), Guid.NewGuid(), items, payment);

        Assert.Equal(225m, order.TotalAmount);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(2, order.Items.Count);
    }

    /// <summary>
    /// Verifies status updates mutate order status.
    /// </summary>
    [Fact]
    public void UpdateStatus_WithValidValue_UpdatesOrderStatus()
    {
        Order order = CreateOrder();

        order.UpdateStatus(OrderStatus.Shipped);

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    private static Payment CreatePayment()
    {
        return new Payment(Guid.NewGuid(), Guid.NewGuid(), 100m, "Wallet");
    }

    private static Order CreateOrder()
    {
        List<OrderItem> items = new() { new OrderItem(Guid.NewGuid(), "Laptop", 100m, 1) };
        return new Order(Guid.NewGuid(), Guid.NewGuid(), items, CreatePayment());
    }
}
