# Cart and Wallet Workflows

## Scope

This document explains Prompt 4 implementation for:
- add/view/update cart
- stock-aware cart quantity validation
- remove cart item via zero quantity update
- wallet balance view
- wallet fund top-up
- bounded numeric input and friendly menu-boundary exception handling

## Cart Workflow

### Add to cart

Input:
- pick a product from a numbered active-product list
- quantity (must be a positive integer)

Rules:
- customer must be valid
- product must exist
- product must be active
- requested quantity + existing cart quantity must not exceed available stock

Service:
- `CartService.AddToCart(...)`

Exceptions:
- `ValidationException` for invalid customer, inactive product, or invalid quantity
- `NotFoundException` if product does not exist
- `InsufficientStockException` if stock threshold is exceeded

### View cart

Service calls:
- `CartService.GetCartItems(...)`
- `CartService.GetCartTotal(...)`

Rendering:
- handled by `Presentation/Helpers/CartDisplayHelper.cs`

### Update cart quantity

Behavior:
- customer selects cart item by number (no internal identifier input)
- quantity > 0 updates line quantity
- quantity == 0 removes the item
- quantity < 0 is rejected
- requested quantity must not exceed current product stock

Service:
- `CartService.UpdateCartItem(...)`

## Wallet Workflow

### View balance

Service:
- `WalletService.GetBalance(...)`

### Add funds

Service:
- `WalletService.AddFunds(...)`

Rules:
- customer must be valid
- amount must be greater than zero
- balance update is persisted via user repository update

Exceptions:
- `ValidationException` on invalid customer or amount

## Thin Presentation Design

Menus only:
- collect input using numbered selections and bounded helpers from `ConsoleInputHelper`
- call service methods
- handle exceptions with friendly output through `MenuActionHelper`

All cart/wallet business rules remain in services/domain.

## Persistence Notes

User repository persists:
- wallet balance
- cart item snapshots

File:
- `data/users.json`

## Key Files

- `Application/Services/CartService.cs`
- `Application/Services/WalletService.cs`
- `Presentation/Menus/CustomerMenu.cs`
- `Presentation/Helpers/ConsoleInputHelper.cs`
- `Presentation/Helpers/MenuActionHelper.cs`
- `Presentation/Helpers/CartDisplayHelper.cs`
- `Infrastructure/Repositories/InMemoryUserRepository.cs`

## Test Coverage

Covered in:
- `Tests/CommerceConsole.Tests/Application/CartWalletServiceTests.cs`

Scenarios:
- add-to-cart success
- add-to-cart stock violation
- add-to-cart null-customer guard
- update-to-zero removes item
- update negative quantity rejection
- update quantity stock violation
- wallet top-up success + persistence
- wallet top-up validation failure
- wallet balance null-customer guard