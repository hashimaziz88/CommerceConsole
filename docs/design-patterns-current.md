# Current Design Patterns in CommerceConsole

## Purpose

This document maps patterns currently present in code, explains why they were used, and clarifies which upgrades are still planned for Submission 2.

Important framing:
- Not every pattern here is a Gang-of-Four pattern.
- Some are architectural patterns or implementation techniques.
- In viva, that is acceptable if you define the category clearly.

## Pattern Inventory at a Glance

Implemented now:
1. Repository Pattern
2. Service Layer Pattern
3. Constructor Injection
4. Composition Root
5. Data Mapper Pattern
6. Rich Domain Model
7. Guard Clauses
8. Session Context Pattern
9. Strategy-style Export Abstraction
10. Idempotent Seed Pattern (startup reliability technique)

Planned next (Submission 2 style):
- Factory Pattern (menu/user creation extraction)
- broader Strategy Pattern (payment/report variants)
- State-style transition objects for orders

## 1. Repository Pattern

Definition:
- Provides collection-like access to domain objects while hiding data-source details.

Where:
- contracts in `Application/Interfaces` (`IUserRepository`, `IProductRepository`, `IOrderRepository`)
- implementations in `Infrastructure/Repositories`

Why used:
- services should reason about business operations, not JSON I/O internals.

Problem solved:
- decouples business workflows from storage technology.

Trade-off:
- additional interfaces and mapping logic.

## 2. Service Layer Pattern

Definition:
- Centralizes use-case orchestration in dedicated service classes.

Where:
- `Application/Services/*`

Why used:
- keeps menus thin and prevents workflow duplication across UI handlers.

Problem solved:
- avoids business logic leakage into presentation.

Trade-off:
- more service classes to maintain as scope grows.

## 3. Constructor Injection

Definition:
- dependencies are passed through constructors rather than created internally.

Where:
- application services and menus

Why used:
- explicit dependency graphs
- easy test construction

Problem solved:
- avoids hidden coupling and hard-wired implementation creation.

Trade-off:
- constructors become longer for coordinator types.

## 4. Composition Root

Definition:
- single entry point where object graph is assembled.

Where:
- `Program.cs`

Why used:
- all runtime wiring is visible in one place.

Problem solved:
- avoids object creation scattered across menus/services.

Trade-off:
- startup file grows as dependencies increase.

## 5. Data Mapper Pattern

Definition:
- maps between domain entities and storage record models.

Where:
- `ToDomain(...)` / `FromDomain(...)` methods in repositories
- `Infrastructure/Repositories/Models/*Record.cs`

Why used:
- domain model stays independent from JSON schema shape.

Problem solved:
- prevents persistence concerns from polluting domain entities.

Trade-off:
- explicit mapper code overhead.

## 6. Rich Domain Model

Definition:
- entities contain both data and behavior/invariant enforcement.

Where:
- `Domain/Entities` (`Product`, `Cart`, `Customer`, `Order`, `Payment`, `Review`)

Why used:
- business rules should be enforced by the object that owns state.

Problem solved:
- reduces invalid-state bugs from unrestricted property writes.

Trade-off:
- requires disciplined method design to avoid entity bloat.

## 7. Guard Clauses

Definition:
- fail-fast validation at method/constructor boundaries.

Where:
- domain constructors/mutators
- service entry methods

Why used:
- immediate feedback and simpler debugging.

Problem solved:
- blocks invalid state early.

Trade-off:
- repetitive checks if not consistently organized.

## 8. Session Context Pattern

Definition:
- dedicated holder for current authenticated runtime user state.

Where:
- `ISessionContext` + `SessionContext`

Why used:
- avoids passing user state through every call manually.

Problem solved:
- centralizes sign-in/sign-out state management.

Trade-off:
- currently process-local only.

## 9. Strategy-Style Export Abstraction

Definition:
- output/export behavior is hidden behind an interface and selected implementation.

Where:
- `IReportExporter`
- `ReportExportService`
- `PdfReportExporter`

Why used:
- report aggregation (`ReportService`) should not know PDF formatting details.

Problem solved:
- separates what to export from how to export.

Trade-off:
- one more abstraction for small scope.

Note:
- this is a practical strategy seam, even if only one concrete exporter exists currently.

## 10. Idempotent Seed Pattern

Definition:
- startup seed can run repeatedly without duplicate inserts.

Where:
- `SeedData.Seed(...)`

Why used:
- predictable restarts and repeat demos.

Problem solved:
- avoids duplicate admin/products after relaunch.

Trade-off:
- small startup check overhead.

## Pattern Interactions (How They Work Together)

Typical flow:
1. Menu (Presentation) calls service.
2. Service (Service Layer) uses repository interfaces.
3. Repository implementation maps domain <-> record models (Data Mapper).
4. File store persists JSON (Infrastructure detail).
5. Domain entities enforce invariants via guard clauses throughout.

This interaction is the core maintainability story of the project.

## What Is Not Fully Patternized Yet (Intentional)

Not yet extracted:
- factory classes for role/menu creation
- state objects for order transitions
- multiple payment strategies

Reason:
- baseline delivery and reliability first
- pattern extraction planned as controlled refactor phase (Monday milestone)

## How to Explain Pattern Choice in Viva

Good phrasing:
"We used patterns that solve immediate structural problems: Repository for storage decoupling, Service Layer for workflow centralization, Data Mapper for schema isolation, and composition root plus constructor injection for explicit wiring. Pattern adoption is incremental and driven by concrete needs, not overengineering."

## 30-Second Pattern Defense Script

"Current patterns isolate responsibilities: repositories abstract storage, services own use-case orchestration, mappers separate domain from JSON schema, and composition root keeps wiring explicit. We intentionally added an exporter strategy seam and kept future factory/state extractions as low-risk next steps."
