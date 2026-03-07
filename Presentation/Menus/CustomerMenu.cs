using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Domain.Exceptions;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Customer menu with product browsing, cart, wallet, checkout, and order tracking actions.
/// </summary>
public sealed class CustomerMenu
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IWalletService _walletService;
    private readonly IOrderService _orderService;

    /// <summary>
    /// Initializes the customer menu.
    /// </summary>
    public CustomerMenu(
        IProductService productService,
        ICartService cartService,
        IWalletService walletService,
        IOrderService orderService)
    {
        _productService = productService;
        _cartService = cartService;
        _walletService = walletService;
        _orderService = orderService;
    }

    /// <summary>
    /// Runs the customer menu loop.
    /// </summary>
    public void Run(ISessionContext sessionContext)
    {
        if (sessionContext.CurrentUser is not Customer customer || customer.Role != UserRole.Customer)
        {
            Console.WriteLine("Access denied. Customer login required.");
            return;
        }

        bool done = false;
        while (!done)
        {
            ShowMenuOptions();
            Console.Write("Select an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    BrowseProducts();
                    break;
                case "2":
                    SearchProducts();
                    break;
                case "3":
                    ExecuteAction(() => AddToCart(customer));
                    break;
                case "4":
                    ViewCart(customer);
                    break;
                case "5":
                    ExecuteAction(() => UpdateCartItem(customer));
                    break;
                case "6":
                    ViewWalletBalance(customer);
                    break;
                case "7":
                    ExecuteAction(() => AddWalletFunds(customer));
                    break;
                case "8":
                    ExecuteAction(() => Checkout(customer));
                    break;
                case "9":
                    ViewOrderHistory(customer);
                    break;
                case "10":
                    TrackOrderStatus(customer);
                    break;
                case "11":
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter 1 through 11.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void ShowMenuOptions()
    {
        Console.WriteLine("=== Customer Menu ===");
        Console.WriteLine("1. Browse Active Products");
        Console.WriteLine("2. Search Products");
        Console.WriteLine("3. Add Product To Cart");
        Console.WriteLine("4. View Cart");
        Console.WriteLine("5. Update Cart Item Quantity");
        Console.WriteLine("6. View Wallet Balance");
        Console.WriteLine("7. Add Wallet Funds");
        Console.WriteLine("8. Checkout");
        Console.WriteLine("9. View Order History");
        Console.WriteLine("10. Track Order Status");
        Console.WriteLine("11. Logout");
    }

    private void BrowseProducts()
    {
        var products = _productService.GetActiveProducts();
        ProductDisplayHelper.ShowProducts("=== Active Products ===", products);
    }

    private void SearchProducts()
    {
        string term = ConsoleInputHelper.ReadRequiredString("Search term (name/category): ");
        var products = _productService.SearchProducts(term);
        ProductDisplayHelper.ShowProducts($"=== Search Results for '{term}' ===", products);
    }

    private void AddToCart(Customer customer)
    {
        List<Product> products = _productService.GetActiveProducts();
        if (products.Count == 0)
        {
            Console.WriteLine("No active products are available right now.");
            return;
        }

        ProductDisplayHelper.ShowSelectableProducts("=== Select Product To Add ===", products);
        int selection = ConsoleInputHelper.ReadSelection("Choose product number: ", products.Count);
        int quantity = ConsoleInputHelper.ReadInt("Quantity to add: ");

        Product selectedProduct = products[selection - 1];
        _cartService.AddToCart(customer, selectedProduct.Id, quantity);
        Console.WriteLine($"{selectedProduct.Name} added to cart successfully.");
    }

    private void ViewCart(Customer customer)
    {
        var items = _cartService.GetCartItems(customer);
        decimal total = _cartService.GetCartTotal(customer);
        CartDisplayHelper.ShowCart(items, total);
    }

    private void UpdateCartItem(Customer customer)
    {
        var items = _cartService.GetCartItems(customer);
        if (items.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        decimal total = _cartService.GetCartTotal(customer);
        CartDisplayHelper.ShowSelectableCart(items, total);
        int selection = ConsoleInputHelper.ReadSelection("Choose cart item number: ", items.Count);
        int quantity = ConsoleInputHelper.ReadInt("New quantity (0 removes item): ");

        CartItem selectedItem = items[selection - 1];
        _cartService.UpdateCartItem(customer, selectedItem.ProductId, quantity);

        if (quantity <= 0)
        {
            Console.WriteLine("Item removed from cart.");
        }
        else
        {
            Console.WriteLine("Cart item updated successfully.");
        }
    }

    private void ViewWalletBalance(Customer customer)
    {
        decimal balance = _walletService.GetBalance(customer);
        Console.WriteLine($"Current wallet balance: {balance:C}");
    }

    private void AddWalletFunds(Customer customer)
    {
        decimal amount = ConsoleInputHelper.ReadDecimal("Amount to add: ");
        _walletService.AddFunds(customer, amount);
        Console.WriteLine("Funds added successfully.");
    }

    private void Checkout(Customer customer)
    {
        Order order = _orderService.Checkout(customer);

        Console.WriteLine("Checkout completed successfully.");
        Console.WriteLine($"Items: {order.Items.Count}");
        Console.WriteLine($"Total paid: {order.TotalAmount:C}");
        Console.WriteLine($"Order status: {order.Status}");
        Console.WriteLine($"Payment status: {order.Payment.Status}");
    }

    private void ViewOrderHistory(Customer customer)
    {
        List<Order> orders = _orderService.GetCustomerOrders(customer.Id);
        OrderDisplayHelper.ShowOrders("=== Your Order History ===", orders);
    }

    private void TrackOrderStatus(Customer customer)
    {
        List<Order> orders = _orderService.GetCustomerOrders(customer.Id);
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        OrderDisplayHelper.ShowSelectableOrders("=== Select Order To Track ===", orders);
        int selection = ConsoleInputHelper.ReadSelection("Choose order number: ", orders.Count);

        Order selectedOrder = orders[selection - 1];
        OrderDisplayHelper.ShowTracking(selectedOrder);
    }

    private static void ExecuteAction(Action action)
    {
        try
        {
            action();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Not found: {ex.Message}");
        }
        catch (InsufficientStockException ex)
        {
            Console.WriteLine($"Stock error: {ex.Message}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Funds error: {ex.Message}");
        }
    }
}
