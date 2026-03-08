# Current Design Patterns in CommerceConsole

## Purpose

This document captures the pattern footprint **after the Monday Submission 2 refactor**.
It includes:
- patterns that were already present
- patterns that were missing before Monday
- what was implemented in Monday refactor
- why each pattern is used and what trade-off it introduces

## Pattern Audit Snapshot

## Before Monday (already used)

1. Repository Pattern
2. Service Layer Pattern
3. Constructor Injection
4. Composition Root
5. Data Mapper Pattern
6. Rich Domain Model
7. Guard Clauses
8. Session Context Pattern
9. Strategy-style report export seam (`IReportExporter`)
10. Idempotent Seed Pattern

## Before Monday (missing / not formalized)

1. Factory Pattern for role/menu routing
2. Payment Strategy formalization (wallet flow was inline in `OrderService`)
3. State-style order transition policies (static transition map in service)
4. Command Pattern for menu action dispatch
5. Specification Pattern for reusable query/filter rules

## Implemented in Monday refactor

1. Factory Pattern (`IRoleMenuFactory`, `RoleMenuFactory`, `IUserWorkspace`)
2. Strategy Pattern (formal `IPaymentStrategy` + `WalletPaymentStrategy`)
3. State-style transitions (`IOrderTransitionState` + per-status states + factory)
4. Command Pattern (`IMenuCommand`, `DelegateMenuCommand`, `MenuCommandDispatcher`)
5. Specification Pattern (Domain `ISpecification<T>`, product specs, repository `Find(spec)`)

## Pattern Details

## 1. Repository Pattern

Where:
- `IRepository<T>` and specialized repository interfaces
- in-memory repository implementations in infrastructure

Why:
- isolates persistence mechanics from business orchestration.

Trade-off:
- interface/mapping overhead.

## 2. Service Layer Pattern

Where:
- `Application/Services/*`

Why:
- centralizes use-case orchestration away from menus.

Trade-off:
- increased service count with growth.

## 3. Constructor Injection + Composition Root

Where:
- service/menu constructors and `Program.cs`

Why:
- explicit dependency graph and test-friendly setup.

Trade-off:
- longer constructors and startup wiring.

## 4. Data Mapper Pattern

Where:
- `ToDomain` / `FromDomain` mapping in repositories
- standalone persistence `*Record` models

Why:
- domain model independence from JSON schema.

Trade-off:
- explicit mapping maintenance.

## 5. Rich Domain Model + Guard Clauses

Where:
- domain entities and invariants

Why:
- fail fast and protect business state.

Trade-off:
- repetitive validation if not organized.

## 6. Session Context Pattern

Where:
- `ISessionContext` / `SessionContext`

Why:
- central session state management for authenticated runtime.

Trade-off:
- process-local session only.

## 7. Factory Pattern (Monday)

Where:
- `IUserWorkspace`
- `IRoleMenuFactory`
- `RoleMenuFactory`
- `MainMenu` role routing now resolves workspace through factory

Problem solved:
- removes role-switch routing from main menu.

Trade-off:
- additional abstraction layer.

## 8. Strategy Pattern (Monday formalization)

Where:
- `IPaymentStrategy`
- `WalletPaymentStrategy`
- `OrderService` delegates payment execution to strategy
- existing report export strategy seam retained (`IReportExporter`)

Problem solved:
- decouples payment algorithm from checkout orchestration.

Trade-off:
- extra strategy contracts and wiring.

## 9. State-style transition handling (Monday)

Where:
- `IOrderTransitionState`
- per-status state classes
- `IOrderTransitionStateFactory`
- `OrderService` transition validation via resolved state policy

Problem solved:
- transitions are policy objects, not inline map logic.

Trade-off:
- more classes for lifecycle policy.

## 10. Command Pattern (Monday)

Where:
- `IMenuCommand`
- `DelegateMenuCommand`
- `MenuCommandDispatcher`
- menu loops now dispatch via command map, not switch blocks

Problem solved:
- cleaner action dispatch and reduced switch complexity.

Trade-off:
- command map setup overhead in menus.

## 11. Specification Pattern (Monday)

Where:
- Domain specs (`ISpecification<T>`, `AndSpecification`, etc.)
- product-specific specs (`Active`, `Search`, `LowStock`, `InStock`)
- repository `Find(spec)` contract and implementations
- service query flows now use spec-based filtering where applicable

Problem solved:
- reusable, composable query rules without duplicated inline predicates.

Trade-off:
- additional abstractions and classes.

## Why this pattern set is now stronger

- Role routing is factory-driven.
- Checkout payment is strategy-driven.
- Lifecycle transitions are state-policy-driven.
- Menu action loops are command-driven.
- Catalog/report filtering is specification-driven.

Result:
- cleaner extension points for future redesign with behavior parity preserved.

## Quick Viva Script

"Before Monday, we had strong foundational patterns but missing explicit Factory, payment Strategy, State-style transitions, Command dispatch, and Specification-based query formalization. Monday refactor introduced all five in a controlled way, preserving baseline behavior while improving extensibility and architectural clarity."
