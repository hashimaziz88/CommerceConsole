using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Exceptions;

namespace CommerceConsole.Application.Services;

/// <summary>
/// Implements shopping cart workflows with stock validation.
/// </summary>
public sealed class CartService : ICartService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes cart service dependencies.
    /// </summary>
    public CartService(IProductRepository productRepository, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public void AddToCart(Customer customer, Guid productId, int quantity)
    {
        EnsureCustomer(customer);

        if (quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        Product product = GetProductOrThrow(productId);

        if (!product.IsActive)
        {
            throw new ValidationException("Cannot add inactive products to cart.");
        }

        int existingQuantity = customer.Cart.Items
            .FirstOrDefault(item => item.ProductId == productId)?.Quantity ?? 0;

        if (existingQuantity + quantity > product.StockQuantity)
        {
            throw new InsufficientStockException("Requested quantity exceeds available stock.");
        }

        customer.Cart.AddItem(product.Id, product.Name, product.Price, quantity);
        _userRepository.Update(customer);
    }

    /// <inheritdoc />
    public void UpdateCartItem(Customer customer, Guid productId, int quantity)
    {
        EnsureCustomer(customer);

        if (quantity < 0)
        {
            throw new ValidationException("Quantity cannot be negative.");
        }

        Product product = GetProductOrThrow(productId);

        if (quantity > product.StockQuantity)
        {
            throw new InsufficientStockException("Requested quantity exceeds available stock.");
        }

        customer.Cart.UpdateQuantity(productId, quantity);
        _userRepository.Update(customer);
    }

    /// <inheritdoc />
    public void RemoveFromCart(Customer customer, Guid productId)
    {
        EnsureCustomer(customer);
        customer.Cart.RemoveItem(productId);
        _userRepository.Update(customer);
    }

    /// <inheritdoc />
    public IReadOnlyList<CartItem> GetCartItems(Customer customer)
    {
        EnsureCustomer(customer);
        return customer.Cart.Items;
    }

    /// <inheritdoc />
    public decimal GetCartTotal(Customer customer)
    {
        EnsureCustomer(customer);
        return customer.Cart.CalculateTotal();
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

    private static void EnsureCustomer(Customer customer)
    {
        if (customer is null)
        {
            throw new ValidationException("Customer is required.");
        }
    }
}