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
- product/cart/order/report rendering helpers

Current classes:
- `Presentation/Menus/MainMenu.cs`
- `Presentation/Menus/CustomerMenu.cs`
- `Presentation/Menus/AdminMenu.cs`
- `Presentation/Helpers/ProductDisplayHelper.cs`
- `Presentation/Helpers/CartDisplayHelper.cs`
- `Presentation/Helpers/OrderDisplayHelper.cs`
- `Presentation/Helpers/ReportDisplayHelper.cs`
- `Presentation/Helpers/ConsoleInputHelper.cs`
- `Presentation/Helpers/MenuActionHelper.cs`
- `Presentation/Helpers/ConsoleTheme.cs`
- `Presentation/Helpers/MenuFrameRenderer.cs`
- `Presentation/Helpers/ConfirmationPrompt.cs`

Rules followed:
- no repository access from menus
- no domain mutation logic directly in menu handlers
- no exposure of internal identifiers in user-facing screens
- menu options use bounded numeric selection parsing
- exception handling is centralized through `MenuActionHelper`
- menu layout framing is centralized through `MenuFrameRenderer`
- confirmations use `ConfirmationPrompt` and consistent theming uses `ConsoleTheme`

### Application layer

Responsible for:
- use-case orchestration
- service contracts and abstractions
- authentication/session coordination
- catalog, cart, wallet, checkout, order lifecycle, review, and report rules

Current contracts:
- `Application/Interfaces/*`

Current services:
- `AuthService`
- `SessionContext`
- `ProductService`
- `CartService`
- `WalletService`
- `OrderService`
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
- update product (numbered product selection)
- delete product (numbered product selection)
- restock product (numbered product selection)
- view all products
- view low-stock products (threshold based)

Business rules are centralized in `ProductService` + domain entity guards.

## Cart and Wallet Flow (Prompt 4)

Customer:
- add to cart from numbered active-product selections
- view cart
- update cart quantities from numbered cart-item selections (zero quantity removes)
- view wallet balance
- add wallet funds

Rules centralized in services:
- stock-aware quantity checks in `CartService`
- wallet amount validation in `WalletService` and `Customer`
- presentation only handles input/output and exception display without exposing internal identifiers

## Checkout and Order Processing Flow (Prompt 5)

Customer:
- checks out cart with wallet-only payment

Orchestration in `OrderService`:
- validates cart content, product existence/active state, stock, and wallet balance
- debits wallet and reduces stock on success
- creates payment and snapshot-based order items
- persists order record and clears cart

Detailed behavior and invariants:
- `docs/checkout-orders.md`

## Order History and Status Lifecycle Flow (Prompt 6)

Customer:
- views personal order history
- tracks selected order status

Administrator:
- views all orders
- updates order statuses

Status transition enforcement:
- centralized in `OrderService`
- admin can only select allowed next statuses
- invalid transitions throw `ValidationException`

Lifecycle rules and transition matrix:
- `docs/order-lifecycle.md`

## Reviews and Reporting Flow (Prompt 7)

Customer:
- adds product reviews with rating and comment for purchased products only
- review selection only displays products from customer purchase history
- sees average ratings in product views

Administrator:
- generates sales report with:
- total revenue
- orders by status
- best-selling products
- low-stock products

Service orchestration:
- `ReviewService` validates and persists reviews
- `ReportService` performs LINQ-based aggregations and report projections

Report definitions and examples:
- `docs/reviews-reporting.md`


## Quality Hardening Additions (Prompt 8)

Implemented hardening updates:
- shared presentation-boundary exception handling through `MenuActionHelper`
- stricter typed input helpers (`ReadPositiveInt`, `ReadNonNegativeInt`, `ReadIntInRange`, `ReadPositiveDecimal`, `ReadNonNegativeDecimal`)
- menu option handling switched to bounded numeric parsing instead of raw string switches
- additional service guard clauses for null customer and empty-ID cases (`CartService`, `WalletService`, `OrderService`)
- regression tests extended for helper parsing and new guard paths

## Terminal UX Enhancement Additions (Prompt 10)

Implemented UX-focused presentation improvements:
- framed menus with breadcrumb-style paths and grouped action sections
- consistent message conventions (`[INFO]`, `[OK]`, `[WARN]`, `[ERROR]`, `[TIP]`)
- confirmation prompts before destructive or high-impact actions (delete, checkout, status updates, exit)
- improved product/cart/order/report rendering for demo readability
- optional pause points between menu actions for guided walkthroughs
- role-aware workspace headers and welcome banner

## Persistence Design

Persisted files:
- `data/users.json`
- `data/products.json`
- `data/orders.json`

Behavior:
- repositories deserialize records on initialization
- add/update/remove immediately writes through to JSON
- malformed JSON is recovered as empty list (non-crashing fallback)
- user persistence includes wallet balance and cart snapshots
- checkout/order workflows persist wallet/cart, stock, and order/payment records
- review additions persist through product repository updates

## Design Decisions and Rationale

- Repository APIs were kept stable to avoid cascading service changes.
- Persistence models are standalone files (no nested classes) for readability.
- Session state is in-memory only by design for current scope.
- Seed logic is idempotent so restarts do not duplicate baseline data.
- Core workflow rules are centralized in services to keep menus thin.
- Report rows are represented by dedicated models for future strategy extraction.

## Current Design Patterns

Patterns already implemented in the current baseline:
- Repository Pattern (`I*Repository` + `InMemory*Repository`)
- Service Layer Pattern (`Application/Services/*`)
- Constructor-based Dependency Injection
- Composition Root (`Program.cs`)
- Data Mapper (`ToDomain` / `FromDomain` in repositories)
- Rich Domain Model with Guard Clauses (`Domain/Entities/*`)
- Session Context pattern (`SessionContext`)

Detailed mapping and use-cases are documented in:
- `docs/design-patterns-current.md`

## Known Limitations (Current Scope)

- reporting and review menus are implemented, but advanced analytics variants are not yet extracted into strategies
- historical timestamps are not fully hydrated from persistence records yet
- no file locking across multiple app instances (single-process assumption)

## Next Evolution Steps

- continue quality hardening and regression expansion (Issue 8)
- introduce explicit Factory/Strategy/State modules (Issue 9)

