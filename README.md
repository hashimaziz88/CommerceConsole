# CommerceConsole

## What Is CommerceConsole?

CommerceConsole is a C# console-based Online Shopping Backend System.

It is structured as a layered, maintainable codebase for coursework delivery, with:
- Presentation layer (menus and console UX)
- Application layer (services and interfaces)
- Domain layer (entities, enums, exceptions, invariants)
- Infrastructure layer (JSON-backed repositories, seed data, export implementations)
- Test project (domain/application/infrastructure/presentation coverage)

The project is delivered with Submission 2-quality engineering practices from the start, then extended with optional bonus features.

## Why Choose CommerceConsole?

- Clean architecture: business logic is separated from UI concerns.
- Maintainability: services, repositories, and domain rules are isolated by responsibility.
- Testability: workflow logic is service-driven and covered by automated tests.
- Practical persistence: mutable data is stored in JSON files for console-level persistence.
- Security-aware presentation: user-facing screens avoid exposing internal identifiers.
- Demo-friendly terminal UX: framed menus, confirmations, breadcrumbs, and consistent message styling.
- Bonus-ready design: export and insights functionality is isolated behind interfaces.

## Documentation

### Core Project Docs

- `docs/architecture.md`
- `docs/auth-flow.md`
- `docs/product-catalog.md`
- `docs/cart-wallet.md`
- `docs/checkout-orders.md`
- `docs/order-lifecycle.md`
- `docs/reviews-reporting.md`
- `docs/persistence.md`
- `docs/oop-design-notes.md`
- `docs/design-patterns-current.md`
- `docs/test-plan.md`
- `docs/bonus-features.md`

### Planning and Delivery Docs

- `.codex/README.md`
- `.codex/04_Submission_1_Implementation_Blueprint.md`
- `.codex/05_Submission_2_Design_Pattern_Upgrade.md`
- `.codex/09_GitHub_Feature_Issues.md`

## Software Requirement Snapshot

### Baseline Scope (Submission 1)

1. Authentication and authorization management
- customer registration
- customer/admin login
- seeded administrator account
- role-based navigation
Status: Implemented

2. User/account and session management
- session context for active user state
- account-level wallet balance handling
- persistent customer/cart data through repository updates
Status: Implemented

3. Product catalog and inventory management
- browse active products
- search products by name/category (LINQ)
- admin add/update/delete/restock
- low-stock query support
- paged product-list rendering for larger catalogs
Status: Implemented

4. Cart and wallet subsystem
- add to cart, view cart, update quantity, remove on zero
- stock-aware cart validation
- wallet balance and top-up
Status: Implemented

5. Checkout, payments, and order processing
- checkout with wallet-only payment
- pre-checkout stock and wallet validation
- stock deduction and wallet debit on success
- payment/order record creation with snapshot order items
- cart clear after successful checkout
Status: Implemented

6. Order history, tracking, and admin status updates
- customer order history view
- customer order status tracking
- admin all-order visibility
- admin status updates with transition-rule enforcement
Status: Implemented

7. Reviews and reporting
- customer product reviews with rating/comment (purchased products only)
- average rating display
- admin sales report: total revenue, orders by status, best sellers, low stock
Status: Implemented

8. Quality hardening
- stronger validation/exception UX paths
- regression coverage and docs alignment
Status: Implemented

### Bonus Scope (Above Submission 1)

1. PDF sales report export (admin)
- generates one-page PDF report files
- uses `IReportExporter` abstraction and `PdfReportExporter` implementation
Status: Implemented

2. Heuristic smart insights (admin)
- rule-based operational insight lines (revenue/category/restock/sentiment/fulfillment)
- no external AI dependency required
Status: Implemented

3. Customer recommendations
- suggests active, in-stock, not-yet-purchased products
- prioritizes preferred categories and rating signals
Status: Implemented

## Running the Application

### Prerequisites

- .NET 10 SDK
- Windows PowerShell (recommended in this workspace)

### Build

```powershell
dotnet build CommerceConsole.csproj
```

### Run

```powershell
dotnet run --project CommerceConsole.csproj
```

### Run Tests

```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```

## Persistence

Mutable runtime data is persisted to JSON files in `data/`:
- `data/users.json`
- `data/products.json`
- `data/orders.json`

Seed behavior:
- default admin and expanded starter catalog are seeded on startup
- missing seeded products are added idempotently by product name (no duplicates)

Bonus export files:
- report PDFs are generated on demand under a chosen output directory (default `./exports`)

## Milestone Plan (Locked)

1. `M1-Foundation-and-Auth`
- architecture bootstrap
- registration/login/session + tests

2. `M2-Catalog-Cart-and-Checkout`
- catalog management
- cart/wallet
- checkout/payment/order processing

3. `M3-Orders-Reporting-and-Quality`
- order tracking/status updates
- reviews/reporting
- quality hardening and documentation alignment

4. `M4-Monday-Patterns-2026-03-09`
- explicit design pattern implementation without feature expansion

## Current Limitations

- PDF exporter is intentionally simple one-page output
- password storage is plain text (coursework scope trade-off)
- JSON persistence is designed for single-process console use

## Development Notes

- Coding standards: `.codex/06_Coding_Standards.md`
- Git workflow and branching: `.codex/07_Git_Workflow_and_Branching.md`
- Prompt pack and issue mapping: `.codex/08_Codex_Prompt_Pack.md`, `.codex/09_GitHub_Feature_Issues.md`


