# Checkout, Payment, and Order Processing

## Purpose

This document explains checkout orchestration, payment simulation, order creation, and invariants after success/failure.

## Central Design Rule

Checkout orchestration lives in one place:
- `Application/Services/OrderService.Checkout(Customer customer)`

Why this is important:
- complex state changes (wallet, stock, payment, order, cart) stay coordinated in one use-case method
- menus remain thin

## Checkout Sequence (Detailed)

1. Validate customer object exists.
2. Read cart items; reject empty cart.
3. Build checkout lines by resolving each product and validating:
- product exists
- product is active
- requested quantity <= available stock
4. Calculate cart total.
5. Ensure wallet balance >= total.
6. Create payment draft (`method = Wallet`).
7. Debit customer wallet.
8. Deduct stock from each product and persist each product update.
9. Mark payment completed.
10. Build immutable order item snapshots from cart lines.
11. Create order (`Pending` by constructor), then set to `Paid`.
12. Persist order.
13. Append order to customer history and clear cart.
14. Persist updated customer state.

## Why Snapshot Order Items Matter

At checkout time, `OrderItem` captures:
- product ID
- product name
- unit price
- purchased quantity

Benefit:
- order history remains historically accurate even if catalog name/price later changes.

## Validation and Exception Model

Potential exceptions:
- `ValidationException` for invalid customer, empty cart, or inactive product
- `NotFoundException` for missing product during checkout
- `InsufficientStockException` if requested quantity exceeds stock
- `InsufficientFundsException` if wallet is too low

All are caught at presentation boundary for friendly messages.

## Post-Checkout Invariants (Success)

After successful checkout:
- payment status is `Completed`
- order status is `Paid`
- order total equals sum of snapshot line totals
- product stocks are reduced
- customer wallet is debited
- customer cart is empty
- order and user records are persisted

## Failure Invariants (No Partial Business Completion)

If validation fails before stock/funds mutation:
- no order is persisted
- wallet and stock remain unchanged
- cart remains unchanged

Note:
- this is not a database transaction engine; it is deterministic service sequencing suitable for single-process coursework scope.

## Persistence Touchpoints

Checkout writes to:
- `data/orders.json` (new order + payment state)
- `data/products.json` (updated stock)
- `data/users.json` (wallet debit + cart clear + order reference in memory)

## Architecture and Responsibility Split

Presentation:
- confirmation prompt
- summary display
- invoke checkout

Application (`OrderService`):
- full orchestration and cross-entity policy checks

Domain:
- payment/order/cart/product invariants and mutation rules

Infrastructure:
- write-through persistence after updates

## Testing Coverage

Primary file:
- `Tests/CommerceConsole.Tests/Application/OrderServiceTests.cs`

Representative scenarios:
- checkout happy path
- insufficient funds failure
- insufficient stock failure
- missing product failure
- snapshot integrity verification


