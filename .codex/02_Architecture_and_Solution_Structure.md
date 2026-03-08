# 02. Architecture and Solution Structure

## Goal

Keep the console app clean, testable, and refactor-friendly by enforcing strict layer boundaries and explicit extension seams.

## Current solution structure (post Submission 2 refactor)

```text
CommerceConsole/
|-- Program.cs
|-- Application/
|   |-- Interfaces/
|   |   |-- IRepository.cs
|   |   |-- IPaymentStrategy.cs
|   |   |-- IOrderTransitionState.cs
|   |   |-- IOrderTransitionStateFactory.cs
|   |   `-- existing service/repository contracts
|   `-- Services/
|       |-- Payments/
|       |   `-- WalletPaymentStrategy.cs
|       |-- OrderTransitions/
|       |   |-- OrderTransitionStateFactory.cs
|       |   `-- *OrderTransitionState.cs
|       `-- existing services
|-- Domain/
|   |-- Entities/
|   |-- Enums/
|   |-- Exceptions/
|   `-- Specifications/
|       |-- ISpecification.cs
|       |-- And/Or/NotSpecification.cs
|       `-- product specifications
|-- Infrastructure/
|   |-- Repositories/
|   |   `-- in-memory repos implementing Find(spec)
|   |-- Persistence/
|   |-- Data/
|   `-- Export/
|-- Presentation/
|   |-- Commands/
|   |-- Factories/
|   |-- Interfaces/
|   |-- Helpers/
|   `-- Menus/
|-- Tests/
|   `-- CommerceConsole.Tests/
`-- docs/
```

## Layer responsibilities (unchanged principles)

### Presentation
Allowed:
- menu routing and rendering
- command dispatch and role workspace resolution

Not allowed:
- repository access
- business-policy logic

### Application
Allowed:
- workflow orchestration
- payment strategy and transition state policies

Not allowed:
- console input/output
- low-level file I/O

### Domain
Allowed:
- entities and invariants
- reusable domain specifications

Not allowed:
- persistence and UI concerns

### Infrastructure
Allowed:
- repository implementations
- JSON persistence and mapping
- exporter adapters

Not allowed:
- UI and business-policy orchestration

## Boundary hard rules

1. Program remains composition root.
2. Menus remain thin and repository-free.
3. User-facing flows stay index-based (no GUID exposure).
4. Domain mutation remains method-driven with guards.
5. Docs and tests must track behavior and architecture changes.
