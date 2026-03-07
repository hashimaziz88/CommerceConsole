# Checkout, Payment, and Order Processing

## Scope

This document explains Prompt 5 implementation for:
- wallet-only checkout
- stock and funds validation before payment
- stock deduction and wallet debit on success
- payment and order record creation
- snapshot-based order items
- cart clearing after successful checkout

## Checkout Workflow

Service:
- `OrderService.Checkout(Customer customer)`

Execution sequence:
1. Validate customer object and ensure cart is not empty.
2. Validate every cart line:
- product exists
- product is active
- cart quantity does not exceed current stock
3. Validate wallet balance is sufficient for cart total.
4. Create payment draft (`method = Wallet`).
5. Debit customer wallet.
6. Reduce stock for each product and persist product updates.
7. Mark payment completed.
8. Build snapshot `OrderItem` records from cart items.
9. Create order, set order status to `Paid`, and persist order.
10. Clear customer cart and persist user update.

## Validation and Exceptions

`OrderService.Checkout(...)` may throw:
- `ValidationException`
- `NotFoundException`
- `InsufficientStockException`
- `InsufficientFundsException`

Validation guarantees:
- no checkout from an empty cart
- no checkout for inactive/missing products
- no checkout when stock is insufficient
- no checkout when wallet funds are insufficient

## State Invariants

On successful checkout:
- payment status is `Completed`
- order status is `Paid`
- order total equals sum of snapshot order items
- product stock is reduced according to purchased quantities
- customer wallet balance is debited by order total
- customer cart is empty

On failed checkout validation:
- no order record is created
- cart remains unchanged
- stock remains unchanged
- wallet balance remains unchanged

## Snapshot Behavior

Order items are created from cart snapshots (`CartItem -> OrderItem`) so later product catalog edits do not rewrite historical order details.

## Presentation Integration

Customer menu now includes:
- `Checkout` action routed to `OrderService.Checkout(...)`

Presentation remains thin:
- menu handles input/output and friendly exception messages
- checkout orchestration remains in application service layer

## Persistence

Checkout touches three persistence stores:
- `data/orders.json` (new order + payment record)
- `data/products.json` (updated stock)
- `data/users.json` (wallet debit + cleared cart)

## Key Files

- `Application/Services/OrderService.cs`
- `Application/Interfaces/IOrderService.cs`
- `Infrastructure/Repositories/InMemoryOrderRepository.cs`
- `Presentation/Menus/CustomerMenu.cs`
- `Tests/CommerceConsole.Tests/Application/OrderServiceTests.cs`
