# Architecture Deep Dive

## Purpose

This document explains exactly what architectural style CommerceConsole uses, why the folders are arranged this way, and how to defend these choices in a demo or viva.

If you only remember one line, remember this:

"CommerceConsole is a layered, DDD-inspired console architecture with rich domain entities, service-layer orchestration, and infrastructure adapters for persistence/export concerns."

## Architecture Identity

## Is this Domain-Driven Design (DDD)?

Short answer:
- **Partly yes (DDD-inspired)**
- **Not full tactical DDD**

What is DDD-like in this project:
- business language is explicit in domain types (`Customer`, `Order`, `Payment`, `Review`)
- invariants live in the model (constructor/mutator guard clauses)
- behavior is attached to entities (`Product.Restock`, `Customer.DebitFunds`, `Cart.UpdateQuantity`)
- application services express use cases, not raw CRUD scripts

What is not yet full tactical DDD:
- no explicit aggregate root boundaries formally modeled
- no domain events
- no dedicated value-object catalog
- no bounded-context split
- repository contracts are in `Application/Interfaces` for pragmatic coursework layering

Conclusion:
- This is best described as **Layered Architecture + Rich Domain Model + DDD-inspired modeling discipline**.

## Architectural Style in Practical Terms

Primary style:
- Layered architecture with inward dependency flow

Supporting principles:
- separation of concerns
- composition root wiring
- repository abstraction
- explicit mapping between domain and persistence models

Dependency direction:

```text
Presentation -> Application -> Domain
Infrastructure -> (implements Application interfaces, uses Domain)
Domain -> (no dependency on higher layers)
```

## Layer Responsibilities (With Real Project Examples)

## 1. Presentation Layer

Folders:
- `Presentation/Menus`
- `Presentation/Helpers`

Responsible for:
- user navigation
- input parsing and retry loops
- index-based selection UX
- user-facing messaging and formatting

Must never do:
- direct repository calls
- business-policy decisions (stock, funds, lifecycle rules)
- persistence/file/export behavior

Examples:
- `MainMenu` routes by authenticated role
- `CustomerMenu` and `AdminMenu` only call services
- `MenuActionHelper` catches exceptions at boundary

## 2. Application Layer

Folders:
- `Application/Interfaces`
- `Application/Services`
- `Application/Models`

Responsible for:
- use-case orchestration
- cross-entity workflow rules
- querying, filtering, and aggregation logic
- defining contracts for repositories and services

Must never do:
- console I/O
- direct JSON/file operations

Examples:
- `OrderService.Checkout` orchestrates checkout invariants and persistence writes
- `ReviewService` enforces purchased-only review eligibility
- `ReportService` computes revenue/status/best-seller/low-stock outputs via LINQ

## 3. Domain Layer

Folders:
- `Domain/Entities`
- `Domain/Enums`
- `Domain/Exceptions`

Responsible for:
- core business concepts
- invariants and safe state transition methods
- domain-specific error types

Must never do:
- repository/file interactions
- UI rendering/input
- export formatting

Examples:
- `Product` blocks negative price/stock
- `Review` blocks ratings outside 1..5
- `Cart` controls add/update/remove behavior

## 4. Infrastructure Layer

Folders:
- `Infrastructure/Repositories`
- `Infrastructure/Repositories/Models`
- `Infrastructure/Persistence`
- `Infrastructure/Data`
- `Infrastructure/Export`

Responsible for:
- repository implementations
- JSON storage mechanics
- mapping between records and domain entities
- seed data bootstrap
- report export adapter implementation

Must never do:
- console interaction
- core business orchestration

Examples:
- `InMemoryProductRepository` persists mutable catalog to `products.json`
- `JsonFileStore` handles safe temp-file writes
- `PdfReportExporter` generates PDF output from report snapshot

## Composition Root and Runtime Wiring

`Program.cs` is the composition root.

Startup flow:
1. Build repository instances.
2. Seed missing admin/products.
3. Build application services with constructor injection.
4. Build menus with injected services.
5. Run `MainMenu` loop.

Why this matters:
- no hidden object creation across menus
- dependency graph is easy to read and explain
- test setup mirrors production wiring cleanly

## Folder and Naming Strategy

Folder naming intent:
- names represent responsibility, not technology hype
- each folder is a boundary with "allowed" and "not allowed" behaviors

Class naming intent:
- contracts start with `I` (`IOrderService`, `IProductRepository`)
- orchestration types end with `Service`
- adapter implementations are explicit (`InMemory*Repository`, `PdfReportExporter`)
- persistence transport types end in `Record`
- read/report transport types use `*Item` and `*Snapshot`

Benefit:
- reviewers can infer class purpose from name before reading code
- onboarding becomes faster and less error-prone

## Why GUIDs Are Hidden in UI but Used Internally

Internally:
- GUIDs provide identity stability and persistence safety.

User-facing screens:
- only index-based selection is shown.

Why:
- improves usability
- avoids exposing internal identifiers
- keeps workflows demo-friendly and less error-prone

## Architecture Constraints (Guardrails)

Hard constraints enforced by standards:
1. Menus do not call repositories.
2. Menus do not host business rules.
3. Domain state mutations go through validated methods.
4. Repository record classes are separate files (no nested class anti-pattern).
5. Mutable runtime data persists in JSON stores.
6. Docs and tests must evolve when behavior changes.

## Trade-Offs and Why They Are Acceptable

Trade-off 1: JSON over database
- Pro: simple setup, deterministic demos, low operational overhead.
- Con: limited concurrency controls and migration strategy.

Trade-off 2: plain-text passwords in scope
- Pro: keeps focus on architecture and workflow delivery.
- Con: not production security posture.

Trade-off 3: centralized transition map in service
- Pro: simple and testable now.
- Con: eventually better extracted to State-style policy objects.

## How This Architecture Supports Submission 2

Ready seams already present:
- Factory: role/menu creation can move from switch logic to factory classes.
- Strategy: export/payment choices can be plugged via interfaces.
- State-style transitions: lifecycle map can be extracted to transition handlers.
- Persistence swap: repository contracts isolate data-store replacement risk.

## Quick Viva Defense Script

"The architecture is layered and intentionally strict: Presentation handles interaction, Application orchestrates use cases, Domain protects business invariants, and Infrastructure owns technical details like JSON persistence and PDF export. It is DDD-inspired through rich entities and explicit business language, but not full tactical DDD yet."
