# CommerceConsole

## What Is CommerceConsole?

CommerceConsole is a C# console application for an Online Shopping Backend System.
It delivers core shopping workflows with clean layering, strong validation, and JSON-backed persistence for repeatable demos and coursework delivery.

## Why Choose CommerceConsole?

- Clear architecture: Presentation, Application, Domain, and Infrastructure are separated.
- Reliable workflow rules: cart, wallet, checkout, and order transitions are centrally enforced.
- Practical persistence: mutable runtime data is stored in JSON without database setup overhead.
- Test-backed quality: core business behavior is covered by automated tests.
- Demo-friendly UX: index-based selection flows avoid exposing internal identifiers.

# Documentation

## Software Requirement Specification

### Overview

CommerceConsole provides role-based shopping workflows for customers and administrators, including catalog management, cart and wallet operations, checkout and orders, reviews, reporting, and quality-focused input/exception handling.

### Components And Functional Requirements

**1. Authentication and authorization management**
- Customer registration.
- Customer and administrator login.
- Role-based routing to customer/admin workspaces.

**2. Product catalog and inventory management**
- Customer browse and search (name/category).
- Administrator add, update, delete, and restock product workflows.
- Low-stock product visibility.

**3. Cart and wallet subsystem**
- Add/update/remove cart items.
- Quantity validation against stock.
- Wallet balance view and wallet top-up.

**4. Checkout, payment, and order processing**
- Wallet-only checkout flow.
- Stock and balance validation before payment.
- Stock deduction, payment creation, order snapshot creation, and cart clear on success.

**5. Order management subsystem**
- Customer order history and status tracking.
- Administrator all-orders view and controlled status updates.

**6. Reviews and reporting subsystem**
- Purchased-product-only reviews with rating validation.
- Sales reporting with revenue, order-status counts, best sellers, and low-stock outputs.

**7. Quality and persistence**
- Friendly presentation-layer exception handling.
- Reusable prompt/render helpers.
- JSON persistence for users/products/orders.

**8. Bonus capabilities implemented**
- PDF sales report export.
- Smart heuristic admin insights.
- Customer product recommendations.

### Architecture Summary

CommerceConsole uses a layered architecture:
- `Presentation`: menu routing, prompts, output formatting.
- `Application`: service orchestration and contracts.
- `Domain`: entities, invariants, enums, and domain exceptions.
- `Infrastructure`: repository implementations, JSON file store, seed data, export adapter.

Detailed architecture and design notes are available in `docs/`.

### Quality And Testing

- Automated tests cover domain, application, infrastructure, and presentation helpers.
- Current local test run result: `61` tests passed.
- Validation and exception pathways are included in regression coverage.

## Additional Documentation

- `docs/architecture.md`
- `docs/auth-flow.md`
- `docs/product-catalog.md`
- `docs/cart-wallet.md`
- `docs/checkout-orders.md`
- `docs/order-lifecycle.md`
- `docs/reviews-reporting.md`
- `docs/persistence.md`
- `docs/test-plan.md`

# Running Application

## Prerequisites

- .NET 10 SDK

## Build

```powershell
dotnet build CommerceConsole.csproj
```

## Run

```powershell
dotnet run --project CommerceConsole.csproj
```

## Run Tests

```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```
