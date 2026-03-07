# 02. Architecture and Solution Structure

## Goal

Design the solution so that:
- the console UI is thin
- business logic is testable and reusable
- domain objects hold rules and state
- services orchestrate use cases
- data storage starts in memory
- tests and documentation are maintained from day one
- design patterns can be added on Monday without breaking behavior

## Recommended project structure

```text
CommerceConsole/
|-- Program.cs
|-- Application/
|   |-- Interfaces/
|   |   |-- IAuthService.cs
|   |   |-- IProductService.cs
|   |   |-- ICartService.cs
|   |   |-- IOrderService.cs
|   |   |-- IWalletService.cs
|   |   |-- IReviewService.cs
|   |   |-- IReportService.cs
|   |   |-- IRepository.cs
|   |   `-- Pattern interfaces (added Monday where needed)
|   |-- Services/
|   |   |-- AuthService.cs
|   |   |-- ProductService.cs
|   |   |-- CartService.cs
|   |   |-- OrderService.cs
|   |   |-- WalletService.cs
|   |   |-- ReviewService.cs
|   |   `-- ReportService.cs
|   |-- Dtos/
|   `-- Validators/
|-- Domain/
|   |-- Entities/
|   |   |-- User.cs
|   |   |-- Customer.cs
|   |   |-- Administrator.cs
|   |   |-- Product.cs
|   |   |-- Cart.cs
|   |   |-- CartItem.cs
|   |   |-- Order.cs
|   |   |-- OrderItem.cs
|   |   |-- Payment.cs
|   |   `-- Review.cs
|   |-- Enums/
|   |   |-- UserRole.cs
|   |   |-- OrderStatus.cs
|   |   `-- PaymentStatus.cs
|   |-- Exceptions/
|   `-- Rules/
|-- Infrastructure/
|   |-- Repositories/
|   |   |-- InMemoryUserRepository.cs
|   |   |-- InMemoryProductRepository.cs
|   |   |-- InMemoryOrderRepository.cs
|   |   `-- ...
|   |-- Data/
|   |   `-- SeedData.cs
|   `-- Utilities/
|-- Presentation/
|   |-- Menus/
|   |   |-- MainMenu.cs
|   |   |-- CustomerMenu.cs
|   |   `-- AdminMenu.cs
|   |-- Screens/
|   `-- Helpers/
|-- Tests/
|   |-- Domain/
|   |-- Application/
|   `-- Integration/
`-- docs/
    |-- architecture.md
    |-- test-plan.md
    `-- changelog.md
```

## Layer responsibilities

### Presentation
Only responsible for:
- showing menus
- reading console input
- calling services
- formatting output
- handling user navigation loops

Must not contain:
- stock rules
- checkout rules
- wallet payment logic
- order creation logic
- complex LINQ queries

### Domain
Contains business concepts and rules:
- entities and their state
- enums
- domain exceptions
- simple business invariants

Examples:
- a `Cart` should not allow quantity less than 1
- a `Product` should not have negative price or stock
- an `Order` should contain immutable order items after creation

### Application
Contains use-case orchestration:
- login flow
- adding to cart
- checkout process
- report generation
- order updates
- validation coordination

### Infrastructure
Contains data access and technical plumbing:
- in-memory repositories
- seed data
- utilities

## Why this structure fits the standards

The coding standards emphasize short classes, understandable methods, clear naming, guard clauses, avoiding duplication, and keeping domain logic in the right layer. This structure also keeps the project ready for pattern adoption without delaying tests or docs.

## Recommended service breakdown

- `AuthService` - registration, login, uniqueness checks
- `ProductService` - catalog CRUD, search, stock visibility, low-stock lookup
- `CartService` - add, update, remove, clear, view totals
- `WalletService` - add funds, get balance, debit funds
- `OrderService` - checkout, order history, tracking, admin status updates
- `ReviewService` - add reviews, fetch ratings, validate ownership or purchase rules if enforced
- `ReportService` - revenue, product performance, low-stock analytics

## Dependency direction

```text
Presentation -> Application -> Domain
Presentation -> Application -> Infrastructure (via interfaces)
Infrastructure -> Domain
```

Prefer the application layer to depend on abstractions like repositories and service contracts.

## Data strategy from day one

Use repositories backed by `List<T>` collections with JSON-file persistence for runtime durability. This keeps implementation simple while ensuring data survives app restarts and remains deterministic for tests.

## Monday pattern implementation path

On Monday (2026-03-09), add patterns on top of working behavior:
- Factory for role-based object creation
- Strategy for payment/report/search variation points
- State-style transition rules for order lifecycle
- Repository formalization if additional abstractions are needed

No feature backlog should remain by this step.

