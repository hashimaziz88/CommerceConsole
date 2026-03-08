using CommerceConsole.Application.Interfaces;
using CommerceConsole.Application.Models;
using CommerceConsole.Domain.Entities;
using CommerceConsole.Domain.Enums;
using CommerceConsole.Presentation.Helpers;

namespace CommerceConsole.Presentation.Menus;

/// <summary>
/// Customer menu with product browsing, cart, wallet, checkout, order tracking, and review actions.
/// </summary>
public sealed class CustomerMenu
{
    private static readonly IReadOnlyList<string> MenuOptions = new List<string>
    {
        "Catalog",
        "1. Browse Active Products",
        "2. Search Products",
        string.Empty,
        "Cart and Wallet",
        "3. Add Product To Cart",
        "4. View Cart",
        "5. Update Cart Item Quantity",
        "6. View Wallet Balance",
        "7. Add Wallet Funds",
        string.Empty,
        "Orders and Reviews",
        "8. Checkout",
        "9. View Order History",
        "10. Track Order Status",
        "11. Add Product Review",
        string.Empty,
        "Bonus",
        "12. View Recommended Products (Bonus)",
        string.Empty,
        "Session",
        "13. Logout to Main Menu"
    };

    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IWalletService _walletService;
    private readonly IOrderService _orderService;
    private readonly IReviewService _reviewService;
    private readonly IInsightsService _insightsService;

    /// <summary>
    /// Initializes the customer menu.
    /// </summary>
    public CustomerMenu(
        IProductService productService,
        ICartService cartService,
        IWalletService walletService,
        IOrderService orderService,
        IReviewService reviewService,
        IInsightsService insightsService)
    {
        _productService = productService;
        _cartService = cartService;
        _walletService = walletService;
        _orderService = orderService;
        _reviewService = reviewService;
        _insightsService = insightsService;
    }

    /// <summary>
    /// Runs the customer menu loop.
    /// </summary>
    public void Run(ISessionContext sessionContext)
    {
        if (sessionContext.CurrentUser is not Customer customer || customer.Role != UserRole.Customer)
        {
            ConsoleTheme.WriteError("Access denied. Customer login required.");
            return;
        }

        bool done = false;
        while (!done)
        {
            ShowMenuOptions(customer.FullName);
            int selection = ConsoleInputHelper.ReadSelection("Choose option (1-13): ", 13);

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
                    MenuActionHelper.Execute(() => ViewRecommendations(customer));
                    break;
                case 13:
                    sessionContext.SignOut();
                    done = true;
                    ConsoleTheme.WriteInfo("You have been logged out and returned to the main menu.");
                    break;
            }

            if (!done)
            {
                ConsoleTheme.Pause();
            }
        }
    }

    private static void ShowMenuOptions(string customerName)
    {
        MenuFrameRenderer.ShowMenu(
            "Customer Workspace",
            "Home > Customer",
            MenuOptions,
            $"Signed in as {customerName}. Use numbered actions for a guided shopping flow.");
    }

    private void BrowseProducts()
    {
        ConsoleTheme.WriteSection("Customer > Catalog > Browse");
        List<Product> products = _productService.GetActiveProducts();
        ProductDisplayHelper.ShowProducts("Active Products", products);
    }

    private void SearchProducts()
    {
        ConsoleTheme.WriteSection("Customer > Catalog > Search");
        ConsoleTheme.WriteHint("Search by product name or category. Use search to narrow larger catalogs quickly.");

        string term = ConsoleInputHelper.ReadRequiredString("Search term: ");
        List<Product> products = _productService.SearchProducts(term);
        ProductDisplayHelper.ShowProducts($"Search Results for '{term}'", products);
    }

    private void AddToCart(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Cart > Add Item");

        List<Product> products = _productService.GetActiveProducts();
        if (products.Count == 0)
        {
            ConsoleTheme.WriteInfo("No active products are available right now.");
            return;
        }

        ProductDisplayHelper.ShowSelectableProducts("Select Product To Add", products);
        ConsoleTheme.WriteHint("Choose the product number shown on the left (global index across pages).");
        int selection = ConsoleInputHelper.ReadSelection("Choose product number: ", products.Count);
        int quantity = ConsoleInputHelper.ReadPositiveInt("Quantity to add: ");

        Product selectedProduct = products[selection - 1];
        _cartService.AddToCart(customer, selectedProduct.Id, quantity);
        ConsoleTheme.WriteSuccess($"{selectedProduct.Name} added to cart.");
    }

    private void ViewCart(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Cart > View");

        IReadOnlyList<CartItem> items = _cartService.GetCartItems(customer);
        decimal total = _cartService.GetCartTotal(customer);
        decimal walletBalance = _walletService.GetBalance(customer);

        CartDisplayHelper.ShowCart(items, total);
        ConsoleTheme.WriteInfo($"Wallet Balance: {walletBalance:C}");

        if (items.Count > 0 && walletBalance < total)
        {
            ConsoleTheme.WriteWarning("Wallet balance is below current cart total.");
        }
    }

    private void UpdateCartItem(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Cart > Update Item");

        IReadOnlyList<CartItem> items = _cartService.GetCartItems(customer);
        if (items.Count == 0)
        {
            ConsoleTheme.WriteInfo("Your cart is empty.");
            return;
        }

        decimal total = _cartService.GetCartTotal(customer);
        CartDisplayHelper.ShowSelectableCart(items, total);

        int selection = ConsoleInputHelper.ReadSelection("Choose cart item number: ", items.Count);
        int quantity = ConsoleInputHelper.ReadNonNegativeInt("New quantity (0 removes item): ");

        CartItem selectedItem = items[selection - 1];

        if (quantity == 0 && !ConfirmationPrompt.AskYesNo($"Remove '{selectedItem.ProductName}' from cart?", false))
        {
            ConsoleTheme.WriteInfo("Cart update cancelled.");
            return;
        }

        _cartService.UpdateCartItem(customer, selectedItem.ProductId, quantity);

        if (quantity == 0)
        {
            ConsoleTheme.WriteSuccess("Item removed from cart.");
        }
        else
        {
            ConsoleTheme.WriteSuccess("Cart item updated.");
        }
    }

    private void ViewWalletBalance(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Wallet > Balance");
        decimal balance = _walletService.GetBalance(customer);
        ConsoleTheme.WriteInfo($"Current wallet balance: {balance:C}");
    }

    private void AddWalletFunds(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Wallet > Top Up");
        decimal amount = ConsoleInputHelper.ReadPositiveDecimal("Amount to add: ");

        _walletService.AddFunds(customer, amount);
        ConsoleTheme.WriteSuccess("Funds added successfully.");
        ConsoleTheme.WriteInfo($"Updated wallet balance: {_walletService.GetBalance(customer):C}");
    }

    private void Checkout(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Checkout");

        IReadOnlyList<CartItem> items = _cartService.GetCartItems(customer);
        decimal total = _cartService.GetCartTotal(customer);
        decimal walletBalance = _walletService.GetBalance(customer);

        if (items.Count == 0)
        {
            ConsoleTheme.WriteInfo("Your cart is empty. Add items before checkout.");
            return;
        }

        ConsoleTheme.WriteInfo($"Items in cart: {items.Count}");
        ConsoleTheme.WriteInfo($"Cart total: {total:C}");
        ConsoleTheme.WriteInfo($"Wallet balance: {walletBalance:C}");

        if (!ConfirmationPrompt.AskYesNo("Proceed with checkout?", false))
        {
            ConsoleTheme.WriteInfo("Checkout cancelled.");
            return;
        }

        Order order = _orderService.Checkout(customer);

        ConsoleTheme.WriteSuccess("Checkout completed successfully.");
        Console.WriteLine($"Items purchased: {order.Items.Count}");
        Console.WriteLine($"Total paid: {order.TotalAmount:C}");
        Console.WriteLine($"Order status: {order.Status}");
        Console.WriteLine($"Payment status: {order.Payment.Status}");
    }

    private void ViewOrderHistory(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Orders > History");
        List<Order> orders = _orderService.GetCustomerOrders(customer.Id);
        OrderDisplayHelper.ShowOrders("Your Order History", orders);
    }

    private void TrackOrderStatus(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Orders > Track");

        List<Order> orders = _orderService.GetCustomerOrders(customer.Id);
        if (orders.Count == 0)
        {
            ConsoleTheme.WriteInfo("No orders found.");
            return;
        }

        OrderDisplayHelper.ShowSelectableOrders("Select Order To Track", orders);
        int selection = ConsoleInputHelper.ReadSelection("Choose order number: ", orders.Count);

        Order selectedOrder = orders[selection - 1];
        OrderDisplayHelper.ShowTracking(selectedOrder);
    }

    private void AddReview(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Reviews > Add");

        List<Product> products = _reviewService.GetReviewableProducts(customer);
        if (products.Count == 0)
        {
            ConsoleTheme.WriteInfo("You have no purchased products available for review.");
            return;
        }

        ProductDisplayHelper.ShowSelectableProducts("Select Product To Review", products);
        ConsoleTheme.WriteHint("Choose the product number shown on the left (global index across pages).");
        int selection = ConsoleInputHelper.ReadSelection("Choose product number: ", products.Count);

        int rating = ConsoleInputHelper.ReadIntInRange("Rating (1-5): ", 1, 5);
        string comment = ConsoleInputHelper.ReadRequiredString("Comment: ");

        Product selectedProduct = products[selection - 1];
        _reviewService.AddReview(customer, selectedProduct.Id, rating, comment);
        ConsoleTheme.WriteSuccess("Review submitted successfully.");
    }

    private void ViewRecommendations(Customer customer)
    {
        ConsoleTheme.WriteSection("Customer > Bonus > Recommendations");
        int maxCount = ConsoleInputHelper.ReadIntInRange("How many recommendations (1-10): ", 1, 10);

        IReadOnlyList<ProductRecommendationItem> recommendations =
            _insightsService.GetCustomerRecommendations(customer, maxCount);

        ProductDisplayHelper.ShowRecommendations("Recommended For You", recommendations);
    }
}

