# CommerceConsole

CommerceConsole is a layered C# console implementation of an online shopping backend.
It is built to be:
- submission-ready
- demo/viva explainable
- safe to evolve into Submission 2 patterns without rewrite-heavy risk

## What Architecture Is This Exactly?

Short answer:
- It is a **Layered Architecture** with **Clean Architecture principles**.
- It is **DDD-inspired**, but **not full tactical Domain-Driven Design**.

Why "DDD-inspired":
- domain entities are rich (behavior + invariants, not just data)
- rules are protected through guard clauses and encapsulation
- business language is explicit (`Customer`, `Order`, `Payment`, `Review`)

Why it is not full tactical DDD:
- no explicit aggregate root boundaries and aggregate repositories
- no domain events
- no explicit value-object catalog (beyond primitive-centric model)
- no bounded-context split yet

If asked in viva:
"This is a layered, DDD-inspired console architecture. We use rich domain entities and business invariants, but we intentionally stopped short of full tactical DDD complexity for scope and delivery speed."

## Folder and Naming Philosophy

Top-level folders:
- `Presentation`: menus, prompts, rendering helpers
- `Application`: use-case orchestration services + contracts
- `Domain`: entities, enums, business exceptions, invariants
- `Infrastructure`: repository implementations, JSON store, seed data, exporters
- `Tests`: architecture-mirrored automated tests
- `docs`: architecture, OOP, workflow, and study pack notes
- `.codex`: implementation standards, prompts, issue/milestone plan

Naming conventions used:
- interfaces: `I*` (`IOrderService`, `IProductRepository`)
- services: `*Service`
- repositories: implementation-style prefix (`InMemory*Repository`)
- persistence records: `*Record`
- menu helpers: `*Helper` and `*Renderer`
- read/report models: concise `*Item` / `*Snapshot` records

## Core Scope Implemented

Submission 1 baseline:
1. Registration, login, and role-based routing.
2. Product browse/search + full admin catalog management.
3. Cart and wallet workflows with stock/funds validation.
4. Checkout with wallet simulation, payment record, order snapshots.
5. Order history/tracking + admin status transitions.
6. Reviews (purchased products only) + reporting via LINQ.
7. Validation hardening, friendly exception boundaries, improved UX.

Bonus scope above baseline:
1. PDF sales report export (`IReportExporter` + `PdfReportExporter`).
2. Heuristic smart admin insights (`IInsightsService`).
3. Customer recommendations with reason text.

## Why This Design Holds Up

- Menus are thin and index-driven (no GUID input).
- Business rules live in services/domain, not in I/O handlers.
- Persistence is isolated behind repository contracts.
- Domain state changes happen through intention-revealing methods.
- Tests cover success + failure paths across layers.
- Docs map directly to implementation for demo confidence.

## How To Run

Prerequisites:
- .NET 10 SDK

Build:
```powershell
dotnet build CommerceConsole.csproj
```

Run:
```powershell
dotnet run --project CommerceConsole.csproj
```

Run tests:
```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```

Current passing count:
- `61` tests (as of March 8, 2026 local workspace run)

## Persistence Model

Runtime state is persisted to JSON:
- `data/users.json`
- `data/products.json`
- `data/orders.json`

Seed behavior is idempotent:
- admin is created only if missing
- seed products are inserted only if missing by name

Bonus export artifact:
- sales report PDF files are generated under chosen output directory (default `./exports`)

## Study Path (Recommended)

If your goal is deep understanding (not just marks), use this order:
1. `docs/architecture.md`
2. `docs/folder-structure-rationale.md`
3. `docs/oop-design-notes.md`
4. `docs/access-modifiers-and-class-design.md`
5. `docs/design-patterns-current.md`
6. Feature docs (`auth-flow`, `product-catalog`, `cart-wallet`, `checkout-orders`, `order-lifecycle`, `reviews-reporting`, `persistence`)
7. `docs/test-plan.md`
8. `docs/demo-study-guide.md`
9. `docs/viva-questions-and-answers.md`

## Milestones (Locked)

1. `M1-Foundation-and-Auth`
2. `M2-Catalog-Cart-and-Checkout`
3. `M3-Orders-Reporting-and-Quality`
4. `M4-Monday-Patterns-2026-03-09`

## Known Trade-Offs

- Password storage is plain text in current coursework scope.
- JSON store is optimized for single-process usage.
- Order transition map is centralized in service (State pattern extraction is planned for Submission 2).
- Manual PDF generator is intentionally simple and deterministic.

## 30-Second Architecture Defense

"CommerceConsole is layered: Presentation handles interaction, Application orchestrates workflows, Domain enforces business invariants, and Infrastructure handles JSON/export mechanics. It is DDD-inspired through rich entities and explicit business rules, while remaining lightweight enough for coursework delivery and safe pattern evolution."
