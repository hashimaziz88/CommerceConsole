# Domain Model

## Purpose

This document defines the business-domain model for CommerceConsole, including entity relationships, aggregate boundaries, and rule ownership.

## Domain Modeling Approach

CommerceConsole uses a domain-centered model with rich entities and guarded mutations.

Model characteristics:

- entities own business state and behavior
- invariants are enforced in constructors and mutation methods
- workflows in the application layer orchestrate entities, but do not bypass entity rules
- persistence models are kept separate in infrastructure (`*Record` types)

This is DDD-inspired modeling, but not full tactical DDD (no bounded-context split or domain events).

## Core Aggregates And Responsibilities

## 1) Customer Account Aggregate

Aggregate root:

- `Customer`

Owned/associated concepts:

- `Cart` (owned composition)
- wallet balance (owned value state)
- order history references (`List<Order>`)
- review history references (`List<Review>`)

Key rules:

- wallet top-up amount must be greater than zero
- wallet debit amount must be greater than zero and not exceed balance
- cart is always tied to exactly one customer ID

## 2) Catalog Aggregate

Aggregate root:

- `Product`

Owned/associated concepts:

- review collection (`List<Review>`)
- stock and activity state

Key rules:

- product ID must be non-empty GUID
- product name/category required
- price and stock cannot be negative
- restock quantity must be greater than zero
- reduce-stock quantity must be greater than zero and within available stock

## 3) Order Aggregate

Aggregate root:

- `Order`

Owned composition:

- `OrderItem` snapshots
- `Payment`

Key rules:

- order must contain at least one item
- order total is derived from item snapshots
- order status is mutation-controlled through workflow policy
- payment amount must be greater than zero

## Invariant Ownership Matrix

| Entity      | Invariant / Rule                                              | Enforced In                                |
| ----------- | ------------------------------------------------------------- | ------------------------------------------ |
| `User`      | valid ID, required full name/email/password, normalized email | `User` constructor                         |
| `Customer`  | positive top-up, valid debit, sufficient funds                | `Customer.AddFunds`, `Customer.DebitFunds` |
| `Cart`      | valid owner ID, positive add quantity, remove on zero update  | `Cart` constructor and mutation methods    |
| `CartItem`  | valid product ID/name, non-negative price, positive quantity  | `CartItem` constructor/mutator             |
| `Product`   | non-negative price/stock, valid naming, stock safety          | `Product` constructor/mutators             |
| `Order`     | valid IDs, non-empty item list, derived total                 | `Order` constructor                        |
| `OrderItem` | valid snapshot fields, positive quantity                      | `OrderItem` constructor                    |
| `Payment`   | valid IDs, positive amount, method required                   | `Payment` constructor                      |
| `Review`    | valid IDs, rating in range 1..5                               | `Review` constructor                       |

## Detailed Mermaid Domain Model

```mermaid
classDiagram
    class User {
        <<abstract>>
        +Guid Id
        +string FullName
        +string Email
        +string Password
        +UserRole Role
        +VerifyPassword(password) bool
    }

    class Customer {
        +decimal WalletBalance
        +Cart Cart
        +List~Order~ Orders
        +List~Review~ Reviews
        +AddFunds(amount)
        +DebitFunds(amount)
    }

    class Administrator

    class Product {
        +Guid Id
        +string Name
        +string Description
        +string Category
        +decimal Price
        +int StockQuantity
        +bool IsActive
        +List~Review~ Reviews
        +UpdateDetails(name, description, category, price)
        +Restock(quantity)
        +ReduceStock(quantity)
        +Deactivate()
    }

    class Cart {
        +Guid CustomerId
        +IReadOnlyList~CartItem~ Items
        +AddItem(productId, productName, unitPrice, quantity)
        +UpdateQuantity(productId, quantity)
        +RemoveItem(productId)
        +Clear()
        +CalculateTotal() decimal
    }

    class CartItem {
        +Guid ProductId
        +string ProductName
        +decimal UnitPrice
        +int Quantity
        +LineTotal decimal
        +UpdateQuantity(quantity)
    }

    class Order {
        +Guid Id
        +Guid CustomerId
        +List~OrderItem~ Items
        +decimal TotalAmount
        +OrderStatus Status
        +DateTime CreatedAt
        +Payment Payment
        +UpdateStatus(newStatus)
    }

    class OrderItem {
        +Guid ProductId
        +string ProductName
        +decimal UnitPrice
        +int Quantity
        +LineTotal decimal
    }

    class Payment {
        +Guid Id
        +Guid OrderId
        +decimal Amount
        +string Method
        +PaymentStatus Status
        +DateTime? PaidAt
        +MarkCompleted()
        +MarkFailed()
    }

    class Review {
        +Guid Id
        +Guid ProductId
        +Guid CustomerId
        +int Rating
        +string Comment
        +DateTime CreatedAt
    }

    class UserRole {
        <<enumeration>>
        Customer
        Administrator
    }

    class OrderStatus {
        <<enumeration>>
        Pending
        Paid
        Processing
        Shipped
        Delivered
        Cancelled
    }

    class PaymentStatus {
        <<enumeration>>
        Pending
        Completed
        Failed
    }

    Customer --|> User
    Administrator --|> User

    Customer "1" *-- "1" Cart : owns
    Customer "1" o-- "*" Order : places
    Customer "1" o-- "*" Review : writes

    Cart "1" *-- "*" CartItem : contains
    Order "1" *-- "*" OrderItem : snapshots
    Order "1" *-- "1" Payment : paid by
    Product "1" o-- "*" Review : receives

    CartItem --> Product : references by ProductId
    OrderItem --> Product : snapshot source only
    Review --> Product
    Review --> Customer

    User --> UserRole
    Order --> OrderStatus
    Payment --> PaymentStatus

    note for Product "Rules: price>=0, stock>=0, restock>0, reduce<=stock"
    note for Review "Rule: rating range is 1..5"
    note for Order "Must contain at least one OrderItem"
```

## Lifecycle Notes

- Cart lifecycle: mutable while shopping; cleared after successful checkout.
- Order lifecycle: created as `Pending`, then moved by workflow policy through valid statuses.
- Payment lifecycle: starts `Pending`, then set to `Completed` or `Failed`.
- Product lifecycle: active by default; can be deactivated and remains in admin-managed catalog state.

## Why This Model Supports Maintainability

- Invariants are protected at entity boundaries, reducing invalid runtime states.
- Snapshot-based `OrderItem` prevents historical order corruption when product catalog data changes.
- Composition in `Cart` and `Order` keeps high-cohesion domains easy to test.
- Separation from infrastructure record models allows persistence schema changes with lower domain risk.
