# 03. Domain Model

## Domain entities

### User (abstract)
Shared identity/auth base type.

Properties:
- `Id`
- `FullName`
- `Email`
- `Password`
- `Role`

Design notes:
- constructor is `protected`
- role-specific users derive from this base

### Customer : User
Customer-specific state owner.

Properties:
- `WalletBalance`
- `Cart`
- `Orders`
- `Reviews`

Key behaviors:
- `AddFunds(decimal amount)`
- `DebitFunds(decimal amount)`

### Administrator : User
Admin role marker with inherited identity/auth behavior.

### Product
Catalog and inventory aggregate.

Properties:
- `Id`
- `Name`
- `Description`
- `Category`
- `Price`
- `StockQuantity`
- `IsActive`
- `Reviews`

Key behaviors:
- `UpdateDetails(...)`
- `Restock(int quantity)`
- `ReduceStock(int quantity)`
- `Deactivate()`

### Cart
Customer cart aggregate.

Properties:
- `CustomerId`
- `Items`

Key behaviors:
- `AddItem(...)`
- `UpdateQuantity(...)`
- `RemoveItem(...)`
- `CalculateTotal()`
- `Clear()`

### CartItem
Snapshot line used by cart.

Properties:
- `ProductId`
- `ProductName`
- `UnitPrice`
- `Quantity`
- `LineTotal`

### Order
Order aggregate.

Properties:
- `Id`
- `CustomerId`
- `Items`
- `TotalAmount`
- `Status`
- `CreatedAt`
- `Payment`

Key behavior:
- `UpdateStatus(OrderStatus)`

### OrderItem
Snapshot line used by order history.

Properties:
- `ProductId`
- `ProductName`
- `UnitPrice`
- `Quantity`
- `LineTotal`

### Payment
Checkout payment record.

Properties:
- `Id`
- `OrderId`
- `Amount`
- `Method`
- `Status`

Key behaviors:
- `MarkCompleted()`
- `MarkFailed()`

### Review
Product review record.

Properties:
- `Id`
- `ProductId`
- `CustomerId`
- `Rating`
- `Comment`
- `CreatedAt`

## Enums

- `UserRole`: Customer, Administrator
- `OrderStatus`: Pending, Paid, Processing, Shipped, Delivered, Cancelled
- `PaymentStatus`: Pending, Completed, Failed

## Domain exceptions

- `ValidationException`
- `NotFoundException`
- `AuthenticationException`
- `InsufficientFundsException`
- `InsufficientStockException`
- `DuplicateEmailException`

## Core invariants

1. Product price and stock cannot be negative.
2. Cart item quantities must be valid for operation context.
3. Wallet debit cannot exceed current balance.
4. Review rating must be in range 1..5.
5. IDs must be valid non-empty GUID values in domain constructors.
6. Order total equals sum of order-item snapshots.

## Application-enforced business rules (outside domain constructor scope)

- email uniqueness enforced in auth workflow
- purchased-product eligibility enforced for reviews
- stock/funds/active checks enforced before checkout
- valid order status transitions enforced centrally in order service

## LINQ hotspots tied to domain model

- product search and low-stock selection
- order history sorting
- revenue and status aggregation
- best-seller and rating aggregations
- recommendation and insight heuristics

## Modeling intent

The domain layer is intentionally rich enough to protect core invariants while still delegating workflow orchestration to Application services.
