using CommerceConsole.Application.Interfaces;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Customer menu with product browsing, cart, wallet, checkout, order tracking, and review actions.
/// </summary>
public sealed class CustomerMenu
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IWalletService _walletService;
    private readonly IOrderService _orderService;
    private readonly IReviewService _reviewService;

    /// <summary>
    /// Initializes the customer menu.
    /// </summary>
    public CustomerMenu(
        IProductService productService,
        ICartService cartService,
        IWalletService walletService,
        IOrderService orderService,
        IReviewService reviewService)
    {
        _productService = productService;
        _cartService = cartService;
        _walletService = walletService;
        _orderService = orderService;
        _reviewService = reviewService;
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
            int selection = ConsoleInputHelper.ReadSelection("Select an option: ", 12);

            switch (selection)
            {
                case 1:
                    MenuActionHelper.Execute(BrowseProducts);
                    break;
                case 2:
                    MenuActionHelper.Execute(SearchProducts);
                    break;
                case 3:
                    MenuActionHelper.Execute(() => AddToCart(customer));
                    break;
                case 4:
                    MenuActionHelper.Execute(() => ViewCart(customer));
                    break;
                case 5:
                    MenuActionHelper.Execute(() => UpdateCartItem(customer));
                    break;
                case 6:
                    MenuActionHelper.Execute(() => ViewWalletBalance(customer));
                    break;
                case 7:
                    MenuActionHelper.Execute(() => AddWalletFunds(customer));
                    break;
                case 8:
                    MenuActionHelper.Execute(() => Checkout(customer));
                    break;
                case 9:
                    MenuActionHelper.Execute(() => ViewOrderHistory(customer));
                    break;
                case 10:
                    MenuActionHelper.Execute(() => TrackOrderStatus(customer));
                    break;
                case 11:
                    MenuActionHelper.Execute(() => AddReview(customer));
                    break;
                case 12:
                    sessionContext.SignOut();
                    done = true;
                    Console.WriteLine("You have been logged out.");
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
        Console.WriteLine("11. Add Product Review");
        Console.WriteLine("12. Logout");
    }

    private void BrowseProducts()
    {
        List<Product> products = _productService.GetActiveProducts();
        ProductDisplayHelper.ShowProducts("=== Active Products ===", products);
    }

    private void SearchProducts()
    {
        string term = ConsoleInputHelper.ReadRequiredString("Search term (name/category): ");
        List<Product> products = _productService.SearchProducts(term);
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
        int quantity = ConsoleInputHelper.ReadPositiveInt("Quantity to add: ");

        Product selectedProduct = products[selection - 1];
        _cartService.AddToCart(customer, selectedProduct.Id, quantity);
        Console.WriteLine($"{selectedProduct.Name} added to cart successfully.");
    }

    private void ViewCart(Customer customer)
    {
        IReadOnlyList<CartItem> items = _cartService.GetCartItems(customer);
        decimal total = _cartService.GetCartTotal(customer);
        CartDisplayHelper.ShowCart(items, total);
    }

    private void UpdateCartItem(Customer customer)
    {
        IReadOnlyList<CartItem> items = _cartService.GetCartItems(customer);
        if (items.Count == 0)
        {
            Console.WriteLine("Your cart is empty.");
            return;
        }

        decimal total = _cartService.GetCartTotal(customer);
        CartDisplayHelper.ShowSelectableCart(items, total);
        int selection = ConsoleInputHelper.ReadSelection("Choose cart item number: ", items.Count);
        int quantity = ConsoleInputHelper.ReadNonNegativeInt("New quantity (0 removes item): ");

        CartItem selectedItem = items[selection - 1];
        _cartService.UpdateCartItem(customer, selectedItem.ProductId, quantity);

        if (quantity == 0)
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
        decimal amount = ConsoleInputHelper.ReadPositiveDecimal("Amount to add: ");
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

    private void AddReview(Customer customer)
    {
        List<Product> products = _reviewService.GetReviewableProducts(customer);
        if (products.Count == 0)
        {
            Console.WriteLine("You have no purchased products available for review.");
            return;
        }

        ProductDisplayHelper.ShowSelectableProducts("=== Select Product To Review ===", products);
        int selection = ConsoleInputHelper.ReadSelection("Choose product number: ", products.Count);

        int rating = ConsoleInputHelper.ReadIntInRange("Rating (1-5): ", 1, 5);
        string comment = ConsoleInputHelper.ReadRequiredString("Comment: ");

        Product selectedProduct = products[selection - 1];
        _reviewService.AddReview(customer, selectedProduct.Id, rating, comment);
        Console.WriteLine("Review submitted successfully.");
    }
}