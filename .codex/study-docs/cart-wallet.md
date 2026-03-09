# Cart and Wallet Workflows

## Purpose

This document details cart and wallet behavior, validation rules, persistence effects, and architecture ownership.

## Architecture Ownership

Presentation owns:
- cart/menu navigation
- index-based item selection
- numeric input loops and confirmations

Application owns:
- cart workflow orchestration (`CartService`)
- wallet workflow orchestration (`WalletService`)

Domain owns:
- cart mutation rules (`Cart`, `CartItem`)
- wallet balance mutation rules (`Customer`)

Infrastructure owns:
- user persistence (`users.json`) for wallet/cart state

## Cart Workflow

## 1. Add to Cart

Entry:
- customer selects product from numbered active-product list

Rules:
- customer must be valid
- quantity must be > 0
- product must exist and be active
- requested quantity + existing cart quantity <= current stock

Implementation:
- `CartService.AddToCart(customer, productId, quantity)`

Effects:
- line is inserted or increased
- user repository persists updated customer cart snapshot

## 2. View Cart

Calls:
- `GetCartItems`
- `GetCartTotal`
- wallet balance retrieval for context

Display:
- itemized rows with quantity, unit, subtotal
- cart total and wallet balance
- warning if wallet < cart total

## 3. Update Cart Item Quantity

Selection:
- user selects cart line by index

Rules:
- quantity < 0 -> invalid
- quantity == 0 -> remove item
- quantity > stock -> invalid
- quantity > 0 and <= stock -> update quantity

Implementation:
- `CartService.UpdateCartItem(...)`

UX behavior:
- remove operation prompts confirmation when quantity is zero

## 4. Remove From Cart

Behavior:
- removal can happen explicitly or via update-to-zero path
- operation persists in user repository

## 5. Cart Total Calculation

Domain method:
- `Cart.CalculateTotal()` sums line totals

Reason this is domain-owned:
- total is cart state behavior, not UI concern

## Wallet Workflow

## 1. View Wallet Balance

Implementation:
- `WalletService.GetBalance(customer)`

## 2. Add Wallet Funds

Implementation:
- `WalletService.AddFunds(customer, amount)`

Rules:
- customer must be valid
- amount must be > 0 (`Customer.AddFunds` guard)

Persistence effect:
- updated customer wallet balance is saved to `users.json`

## Error/Exception Model

Possible exceptions:
- `ValidationException` (invalid quantity/amount/customer, inactive product)
- `NotFoundException` (missing product)
- `InsufficientStockException` (stock violation)

Boundary handling:
- `MenuActionHelper` maps exceptions to friendly UI messages

## Consistency Guarantees

After successful cart/wallet mutation:
- customer state is persisted immediately
- next app run restores wallet and cart state from JSON

If validation fails:
- cart/wallet remain unchanged
- no partial write should be introduced by service flow

## Index-Based UX Rules

- users never type internal product IDs
- cart operations select by rendered list index
- selection range is validated before service calls

This improves usability and keeps internal IDs private.

## Test Coverage

Main file:
- `Tests/CommerceConsole.Tests/Application/CartWalletServiceTests.cs`

Covered scenarios include:
- add to cart success
- stock overflow rejection
- null-customer guard
- update-to-zero removes item
- negative quantity rejection
- quantity > stock rejection
- wallet top-up success and persistence
- wallet top-up validation failure


