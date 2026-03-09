# Architecture

## Purpose

This document describes the architecture model used in CommerceConsole, the layer boundaries, and the dependency direction enforced by the implementation.

Architecture statement:

"CommerceConsole uses a layered, domain-centered architecture where presentation, application orchestration, domain rules, and infrastructure adapters are explicitly separated."

## Architecture Classification

Is this full Domain-Driven Design (DDD)?
- No.

Is this domain-centered and DDD-inspired?
- Yes.

Implemented DDD-inspired elements:
- explicit business language in domain types (`Customer`, `Order`, `Payment`, `Review`)
- invariant-protecting behavior methods on entities
- application services that model use cases

Not implemented as full tactical DDD:
- bounded context partitioning
- domain events/event bus
- explicit aggregate catalogs and repositories by aggregate root boundaries

## Dependency Direction

```text
Presentation -> Application -> Domain
Infrastructure -> Application + Domain
Domain -> (no dependency on Presentation/Application/Infrastructure)
```

## Layer Responsibilities

## 1. Presentation

Folders:
- `Presentation/Menus`
- `Presentation/Helpers`
- `Presentation/Commands`
- `Presentation/Workspaces`

Responsibilities:
- menu navigation and user interaction
- rendering and prompt behavior
- command dispatch and workspace routing

Must not contain:
- repository calls
- business rule ownership (stock/funds/checkout policy)
- persistence/export implementation logic

## 2. Application

Folders:
- `Application/Interfaces`
- `Application/Services`
- `Application/Models`

Responsibilities:
- use-case orchestration
- cross-entity workflow sequencing
- repository/service abstraction contracts
- reporting, recommendation, and insights calculations

Must not contain:
- console input/output handling
- direct JSON/file operations

## 3. Domain

Folders:
- `Domain/Entities`
- `Domain/Enums`
- `Domain/Exceptions`

Responsibilities:
- business entities and invariants
- validated state mutation behavior
- domain-specific error types

Must not contain:
- repository or persistence concerns
- UI/presentation concerns

## 4. Infrastructure

Folders:
- `Infrastructure/Repositories`
- `Infrastructure/Repositories/Models`
- `Infrastructure/Persistence`
- `Infrastructure/Data`
- `Infrastructure/Export`

Responsibilities:
- repository implementations
- record-to-domain mapping
- JSON persistence mechanics
- seed data initialization
- report export adapters

Must not contain:
- menu interaction logic
- use-case orchestration policies

## Composition Root

`Program.cs` is the composition root and remains thin.

Runtime bootstrapping responsibilities:
1. instantiate repositories
2. apply seed data
3. instantiate services
4. instantiate menu/workspace objects
5. start `MainMenu`

## Submission 2 Pattern Implementation in Architecture

The implemented design pattern set is:
1. Repository Pattern
2. Strategy Pattern
3. Factory Pattern
4. Command Pattern

Where these appear:
- Repository: `Application/Interfaces/*Repository` + `Infrastructure/Repositories/*`
- Strategy: `IPaymentStrategy`/`WalletPaymentStrategy`, `IReportExporter`/`PdfReportExporter`
- Factory: `IRoleWorkspaceFactory` + `RoleWorkspaceFactory`
- Command: `IMenuCommand` + `MenuCommandDispatcher` and menu command maps

## Architecture Guardrails

1. Presentation routes and renders only.
2. Business workflows are centralized in application services.
3. Domain entities enforce invariants through behavior methods.
4. Infrastructure owns persistence/export implementation details.
5. User-facing flows do not expose internal GUID identifiers.
6. Mutable runtime data is persisted to JSON through repositories.

## Trade-Offs

Trade-off 1: JSON persistence over database
- Advantage: low operational overhead and deterministic coursework setup
- Limitation: no multi-process concurrency controls

Trade-off 2: plaintext password handling in coursework scope
- Advantage: keeps implementation focus on architecture and workflow
- Limitation: not production-grade security posture

Trade-off 3: order transition matrix kept in `OrderService`
- Advantage: centralized and testable policy enforcement
- Limitation: state-object extraction is a future refactor option, not currently required
