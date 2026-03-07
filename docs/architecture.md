# Architecture Notes

## Purpose

This document explains how the current console implementation is structured, why responsibilities are split this way, and how runtime data flows through the system.

## Layered Design

### Presentation layer

Responsible for:
- menu display
- input capture
- user navigation
- friendly error messaging
- product list rendering and input parsing helpers

Current classes:
- `Presentation/Menus/MainMenu.cs`
- `Presentation/Menus/CustomerMenu.cs`
- `Presentation/Menus/AdminMenu.cs`
- `Presentation/Helpers/ProductDisplayHelper.cs`
- `Presentation/Helpers/ConsoleInputHelper.cs`

Rules followed:
- no repository access from menus
- no domain mutation logic directly in menu handlers

### Application layer

Responsible for:
- use-case orchestration
- service contracts and abstractions
- authentication and session coordination
- catalog management use cases

Current contracts:
- `Application/Interfaces/*`

Current services:
- `AuthService`
- `SessionContext`
- `ProductService`
- `CartService`
- `WalletService`
- `OrderService` (checkout implementation still pending by issue scope)
- `ReviewService`
- `ReportService`

### Domain layer

Responsible for:
- entities
- enums
- guard clauses and invariants
- custom exceptions

Examples of enforced invariants:
- `Product` prevents negative price and stock
- `Review` enforces rating range 1..5
- `CartItem` enforces positive quantity
- `User` enforces required identity fields

### Infrastructure layer

Responsible for:
- repository implementations
- JSON persistence
- seed data

Current data access:
- `InMemoryUserRepository`
- `InMemoryProductRepository`
- `InMemoryOrderRepository`

Persistence utility:
- `Infrastructure/Persistence/JsonFileStore.cs`

Repository persistence model files:
- `Infrastructure/Repositories/Models/*.cs`

## Startup Flow

1. `Program.Main()` creates repository instances.
2. Repositories load persisted JSON from `./data` (if files exist).
3. `SeedData.Seed(...)` ensures baseline admin and products exist.
4. Application services are created.
5. `SessionContext` is created.
6. `MainMenu.Run()` starts register/login navigation.

## Authentication and Routing Flow

1. Unauthenticated users see register/login/exit.
2. Registration is handled by `AuthService.RegisterCustomer(...)`.
3. Login is handled by `AuthService.Login(...)`.
4. Successful login updates `SessionContext`.
5. Routing is role-based:
- customer -> `CustomerMenu`
- administrator -> `AdminMenu`
6. Logout clears session context.

## Product Catalog Flow (Prompt 3)

Customer:
- browse active products
- search products by name/category

Administrator:
- add product
- update product
- delete product
- restock product
- view all products
- view low-stock products (threshold based)

Business rules are centralized in `ProductService` + domain entity guards.

## Persistence Design

Persisted files:
- `data/users.json`
- `data/products.json`
- `data/orders.json`

Behavior:
- repositories deserialize records on initialization
- add/update/remove immediately writes through to JSON
- malformed JSON is recovered as empty list (non-crashing fallback)

## Design Decisions and Rationale

- Repository APIs were kept unchanged to preserve application-layer stability.
- Persistence models were split into standalone files to avoid nested classes and improve readability.
- Session state is in-memory only by design for current scope.
- Seed logic is idempotent so restarts do not duplicate baseline data.
- Catalog actions route through `ProductService` to centralize validation and mutations.

## Known Limitations (Current Scope)

- `OrderService.Checkout(...)` is still a planned implementation item.
- Historical timestamps are not fully hydrated from persistence records yet.
- No file locking across multiple app instances (single-process assumption).

## Next Evolution Steps

- Implement checkout/order workflows (Issue 5).
- Add persistence for cart/wallet/order-history projections where needed.
- Introduce stricter transition policies for order status updates.
