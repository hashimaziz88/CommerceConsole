# Cart and Wallet Workflows

## Scope

This document explains Prompt 4 implementation for:
- add/view/update cart
- stock-aware cart quantity validation
- remove cart item via zero quantity update
- wallet balance view
- wallet fund top-up

## Cart Workflow

### Add to cart

Input:
- pick a product from a numbered active-product list
- quantity

Rules:
- product must exist
- product must be active
- requested quantity + existing cart quantity must not exceed available stock

Service:
- `CartService.AddToCart(...)`

Exceptions:
- `NotFoundException` if product does not exist
- `ValidationException` if product is inactive or quantity invalid
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
- quantity <= 0 removes the item
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
- amount must be greater than zero
- balance update is persisted via user repository update

Exceptions:
- `ValidationException` on invalid amount

## Thin Presentation Design

Menus only:
- collect input using numbered selections for customer cart actions
- call service methods
- handle exceptions with friendly output

All cart/wallet business rules are in services/domain.

## Persistence Notes

User repository now persists:
- wallet balance
- cart item snapshots

File:
- `data/users.json`

## Key Files

- `Application/Services/CartService.cs`
- `Application/Services/WalletService.cs`
- `Presentation/Menus/CustomerMenu.cs`
- `Presentation/Helpers/CartDisplayHelper.cs`
- `Infrastructure/Repositories/InMemoryUserRepository.cs`

## Test Coverage

Covered in:
- `Tests/CommerceConsole.Tests/Application/CartWalletServiceTests.cs`

Scenarios:
- add-to-cart success
- add-to-cart stock violation
- update-to-zero removes item
- update quantity stock violation
- wallet top-up success + persistence
- wallet top-up validation failure

