# Demo Study Guide

## Purpose

This guide is designed for two outcomes:
1. help you run a clear, confident demo
2. help you study architecture/design deeply (not memorize shallow scripts)

Pair this with:
- `docs/architecture.md`
- `docs/oop-design-notes.md`
- `docs/design-patterns-current.md`
- `docs/viva-questions-and-answers.md`

## Study Strategy (3 Phases)

## Phase 1: System Map (30-45 min)

Read in order:
1. `README.md`
2. `docs/architecture.md`
3. `docs/folder-structure-rationale.md`

Goal:
- be able to explain where each type of code belongs and why.

Checkpoint question:
- "Is this full DDD? If not, what exactly is it?"

Expected answer:
- layered, DDD-inspired architecture with rich domain model, not full tactical DDD.

## Phase 2: Behavior Mastery (60-90 min)

Read in order:
1. `docs/auth-flow.md`
2. `docs/product-catalog.md`
3. `docs/cart-wallet.md`
4. `docs/checkout-orders.md`
5. `docs/order-lifecycle.md`
6. `docs/reviews-reporting.md`
7. `docs/persistence.md`

Goal:
- explain any workflow end-to-end across layers.

Checkpoint drill:
- trace checkout from menu to JSON write in your own words.

## Phase 3: Design Defense (45-60 min)

Read in order:
1. `docs/access-modifiers-and-class-design.md`
2. `docs/oop-design-notes.md`
3. `docs/design-patterns-current.md`
4. `docs/test-plan.md`
5. `docs/viva-questions-and-answers.md`

Goal:
- justify choices, trade-offs, and future-ready seams.

## Pre-Demo Technical Checklist

Run before demo:
```powershell
dotnet build CommerceConsole.csproj
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
dotnet run --project CommerceConsole.csproj
```

Confirm:
- tests pass
- seed admin login works
- seeded catalog visible
- add-to-cart and checkout path works
- order status updates work
- review list shows purchased products only
- report and bonus features render correctly

## Suggested 10-12 Minute Demo Script

## Minute 1: Orientation

Say:
- "This is a layered, DDD-inspired console backend with strict boundaries."

Show:
- quick folder map (`Presentation`, `Application`, `Domain`, `Infrastructure`)

## Minute 2-3: Authentication

Show:
- register customer
- login as customer

Say:
- "Auth validation is service-owned, not in menu handlers."

## Minute 3-5: Catalog + Cart + Wallet

Show:
- browse/search with paged rendering
- add product to cart by index
- top up wallet

Say:
- "No internal IDs are exposed; all selections are index-based."

## Minute 5-7: Checkout + Order History

Show:
- checkout success
- order history and tracking

Say:
- "Checkout orchestration is centralized in `OrderService` for consistency."

## Minute 7-8: Admin Lifecycle Management

Show:
- admin login
- view all orders
- update order status via allowed transitions

Say:
- "Transition rules are centralized and menu only renders valid next steps."

## Minute 8-9: Reviews + Reporting

Show:
- customer review flow with purchased-only list
- admin sales report

Say:
- "Reporting uses LINQ for business aggregates."

## Minute 9-11: Bonus Differentiators

Show:
- smart insights
- PDF export
- customer recommendations

Say:
- "Bonus features are additive and modular behind service/interfaces."

## Minute 11-12: Architecture Defense

Say:
- "Layered boundaries plus rich domain rules gave us stable baseline delivery and low-risk extension seams for Submission 2."

## High-Value Talking Points

Use these if time is short:
1. Menus are thin and never call repositories.
2. Domain invariants prevent invalid states at source.
3. JSON persistence is isolated behind repositories and mappers.
4. No GUID exposure in user-facing UX.
5. 61 tests cover critical workflows and failure paths.

## Handling Hard Questions Live

Question: "Why not full DDD?"
Answer:
- "Scope and delivery constraints. We intentionally adopted DDD-inspired practices (rich domain, business language, invariants) without full bounded-context/event complexity."

Question: "Why interfaces in a console app?"
Answer:
- "To decouple behavior from storage/export implementation and keep tests/refactors safe."

Question: "Why JSON instead of DB?"
Answer:
- "Lower setup overhead for coursework, while architecture still supports repository swap later."

## If Demo Goes Wrong

Recovery protocol:
1. explain which layer owns the issue
2. show unaffected tests still pass
3. continue with a fallback flow

This demonstrates architecture maturity and resilience.

## 30-Second Closing Script

"CommerceConsole delivers the required workflows with clear separation of concerns, rich domain invariants, and consistent test coverage. The design is intentionally DDD-inspired and extension-ready, which lets us add bonus features and future patterns without destabilizing baseline behavior."
