# CommerceConsole

## What Is CommerceConsole?

CommerceConsole is a C# console-based Online Shopping Backend System.

It is structured as a layered, maintainable codebase for coursework delivery, with:
- Presentation layer (menus and console UX)
- Application layer (services and interfaces)
- Domain layer (entities, enums, exceptions, invariants)
- Infrastructure layer (JSON-backed in-memory repositories and seed data)
- Test project (domain and application validation)

The project is being delivered with Submission 2 quality standards from the start, then enhanced with explicit design patterns in the Monday phase.

## Why Choose CommerceConsole?

- Clean architecture: business logic is separated from UI concerns.
- Maintainability: services, repositories, and domain rules are isolated by responsibility.
- Testability: workflow logic is service-driven and covered by automated tests.
- Practical persistence: mutable data is stored in JSON files for console-level persistence.
- Security-aware presentation: user-facing screens avoid exposing internal identifiers.
- Incremental delivery: milestones are scoped to reduce rewrite risk.

## Documentation

### Core Project Docs

- `docs/architecture.md`
- `docs/auth-flow.md`
- `docs/product-catalog.md`
- `docs/cart-wallet.md`
- `docs/checkout-orders.md`
- `docs/order-lifecycle.md`
- `docs/reviews-reporting.md`
- `docs/oop-design-notes.md`
- `docs/design-patterns-current.md`
- `docs/test-plan.md`

### Planning and Delivery Docs

- `.codex/README.md`
- `.codex/04_Submission_1_Implementation_Blueprint.md`
- `.codex/05_Submission_2_Design_Pattern_Upgrade.md`
- `.codex/09_GitHub_Feature_Issues.md`

## Software Requirement Specification (SRS) Snapshot

### Overview

CommerceConsole simulates an online shopping backend with role-based access for customers and administrators. The current scope includes authentication, catalog management, cart and wallet workflows, checkout and order processing, order lifecycle management, reviews, reporting, and JSON persistence.

### Components and Functional Requirements

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

6. Order history, tracking, and admin order status updates
- customer order history view
- customer order status tracking
- admin all-order visibility
- admin order status updates with transition-rule enforcement
Status: Implemented

7. Reviews and reporting
- customer product reviews with rating/comment (purchased products only)
- review menu only displays purchased products as selectable options
- rating validation (1 to 5)
- average rating display
- admin sales report: total revenue, orders by status, best sellers, low stock
Status: Implemented

8. Quality hardening
- stronger validation/exception UX paths
- regression coverage and docs alignment
Status: In progress across features

9. Design pattern enrichment
- explicit Factory/Strategy/State-style enhancements
- pattern-focused tests and architecture updates
Status: Planned for Monday phase (Milestone 4)

## Architecture and Design Artefacts

### Current Artefacts

- Layered architecture notes: `docs/architecture.md`
- OOP design rationale: `docs/oop-design-notes.md`
- Current design pattern inventory: `docs/design-patterns-current.md`
- Checkout invariants and behavior: `docs/checkout-orders.md`
- Order lifecycle and transition rules: `docs/order-lifecycle.md`
- Review/report definitions and examples: `docs/reviews-reporting.md`
- Domain and submission planning references: `.codex/03_Domain_Model.md`, `.codex/04_Submission_1_Implementation_Blueprint.md`

### Planned Artefacts (when required by submission pack)

- use-case diagrams
- architecture diagram visual
- state diagrams
- additional design visuals/wireframes where applicable

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
- default admin and sample products are seeded only when missing
- seeding is idempotent to avoid duplicate bootstrap records

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

- advanced report strategy variants are not extracted yet
- password storage is plain text (coursework scope trade-off)
- JSON persistence is designed for single-process console use

## Development Notes

- Coding standards: `.codex/06_Coding_Standards.md`
- Git workflow and branching: `.codex/07_Git_Workflow_and_Branching.md`
- Prompt pack and issue mapping: `.codex/08_Codex_Prompt_Pack.md`, `.codex/09_GitHub_Feature_Issues.md`

