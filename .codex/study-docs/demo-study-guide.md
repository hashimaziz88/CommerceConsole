# Demo Study Guide (Submission 2 Final)

## Purpose

This guide helps you do two things:
1. deliver a clean, confident demo
2. explain architecture and design decisions with accurate technical depth

Use this together with:
- `README.md`
- `docs/architecture.md`
- `docs/design-patterns-current.md`
- `docs/test-plan.md`
- `.codex/study-docs/design-patterns-current.md`
- `.codex/study-docs/viva-questions-and-answers.md`

## Fast Study Path (Recommended)

## Phase 1: Architecture map (30 minutes)

Read:
1. `docs/architecture.md`
2. `docs/folder-structure-rationale.md`
3. `docs/oop-design-notes.md`

Goal:
- explain every top-level folder and dependency direction without hesitation.

## Phase 2: Workflow trace (60 minutes)

Read:
1. `docs/auth-flow.md`
2. `docs/product-catalog.md`
3. `docs/cart-wallet.md`
4. `docs/checkout-orders.md`
5. `docs/order-lifecycle.md`
6. `docs/reviews-reporting.md`
7. `docs/persistence.md`

Goal:
- trace each workflow from menu action to service orchestration to repository persistence.

## Phase 3: Pattern and quality defense (45 minutes)

Read:
1. `docs/design-patterns-current.md`
2. `docs/test-plan.md`
3. `.codex/study-docs/design-patterns-current.md`
4. `.codex/study-docs/viva-questions-and-answers.md`

Goal:
- defend implementation choices with code-level evidence and test evidence.

## Pre-Demo Technical Checklist

Run before any demo session:

```powershell
dotnet build CommerceConsole.csproj
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
dotnet run --project CommerceConsole.csproj
```

If the app is already running and locks rebuild output, run tests with:

```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj --no-build
```

Current expected baseline:
- 115 total tests
- 115 passed
- 0 failed

Also verify manually:
- seeded admin can log in
- customer registration/login works
- no GUID input is required in user-facing flows
- product/cart/order operations work via index-based selection
- purchased-only reviews are enforced
- report view, insights, and PDF export work

## Suggested 12-Minute Demo Flow

## Minute 1: Architecture orientation

Show:
- top-level folders (`Presentation`, `Application`, `Domain`, `Infrastructure`, `Tests`, `docs`)

Say:
- "This is a layered, domain-centered console architecture with strict dependency direction."

## Minute 2-3: Authentication and role routing

Show:
- register customer
- login as customer
- logout and login as admin

Say:
- "Routing to role workspaces is factory-based (`IRoleWorkspaceFactory`) instead of hardcoded role switches."

## Minute 3-5: Catalog + cart + wallet

Show:
- browse/search active products
- add by index to cart
- update cart quantity
- top up wallet

Say:
- "Menus stay thin; service/domain layers enforce stock and wallet rules."

## Minute 5-7: Checkout and order history

Show:
- checkout confirmation
- successful order creation
- order history and status tracking

Say:
- "Checkout orchestration is centralized in `OrderService`, with payment delegated through `IPaymentStrategy`."

## Minute 7-8: Admin order lifecycle

Show:
- admin view all orders
- update status only through allowed transitions

Say:
- "Transition rules are centralized and validated before updates are persisted."

## Minute 8-9: Reviews and reporting

Show:
- customer review flow (purchased-only product list)
- admin sales report metrics

Say:
- "Report aggregates are LINQ-based and remain in the application layer."

## Minute 9-10: Pattern implementation proof

Show quickly:
- `docs/design-patterns-current.md`
- `Presentation/Commands/*`
- `Presentation/Workspaces/*`
- `Application/Services/WalletPaymentStrategy.cs`

Say:
- "Submission 2 concretely implements Repository, Strategy, Factory, and Command."

## Minute 10-11: Bonus features

Show:
- smart insights output
- recommendations
- PDF export

Say:
- "Bonus features are additive and modular, without breaking baseline workflows."

## Minute 11-12: Quality and close

Show:
- `dotnet test` passing baseline

Say:
- "Regression coverage is architecture-aligned across domain, application, infrastructure, and presentation."

## High-Value Talking Points

1. Menus do not call repositories directly.
2. Domain invariants protect business correctness.
3. JSON persistence is isolated in infrastructure adapters.
4. User-facing flows avoid exposing internal GUIDs.
5. Pattern refactors improved extensibility while preserving behavior.

## Failure Recovery Script (If Something Goes Wrong)

If a live demo path fails:
1. identify which layer owns the issue (presentation/application/domain/infrastructure)
2. show test status to prove baseline stability
3. continue demo using an alternate path (admin or customer flow)

This demonstrates engineering control rather than memorized execution.

## 30-Second Closing Script

"CommerceConsole delivers a complete Submission 2 implementation with strict layered boundaries, enforced domain rules, JSON persistence adapters, and verified pattern implementation for Repository, Strategy, Factory, and Command. The architecture stays stable under extension, and full regression tests confirm behavior parity."

