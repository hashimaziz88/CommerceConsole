# Architecture Deep Dive

## Purpose

This document explains the architecture style used by CommerceConsole, why folders are separated the way they are, and how to explain those decisions clearly in a demo.

If you only remember one line, use this:

"CommerceConsole is a layered, domain-centered console architecture where UI, business workflows, core rules, and technical adapters are kept separate."

## Architecture Identity

## Is this Domain-Driven Design (DDD)?

Short answer:
- partly yes (DDD-inspired)
- not full tactical DDD

What is DDD-like in this project:
- business language is explicit in domain types (`Customer`, `Order`, `Payment`, `Review`)
- invariants are enforced where state is owned
- behavior lives on entities (`Product.Restock`, `Customer.DebitFunds`, `Cart.UpdateQuantity`)
- application services express use cases, not menu-level scripts

What is not full tactical DDD:
- no formal aggregate catalog
- no domain events
- no bounded-context split

Conclusion:
- best described as layered architecture with strong domain modeling discipline.

## Dependency Direction

```text
Presentation -> Application -> Domain
Infrastructure -> (implements Application interfaces, uses Domain)
Domain -> (no dependency on higher layers)
```

## Layer Responsibilities

## 1. Presentation

Folders:
- `Presentation/Menus`
- `Presentation/Helpers`

Responsible for:
- navigation, prompts, retries, and output rendering
- index-based selection flows (no GUID entry)

Must never do:
- direct repository calls
- business policy decisions (stock, funds, order lifecycle)
- JSON/file/export operations

## 2. Application

Folders:
- `Application/Interfaces`
- `Application/Services`
- `Application/Models`

Responsible for:
- use-case orchestration
- cross-entity validation and sequencing
- query/filter/report calculations
- repository and service contracts

Must never do:
- console I/O
- direct file operations

## 3. Domain

Folders:
- `Domain/Entities`
- `Domain/Enums`
- `Domain/Exceptions`

Responsible for:
- core business types and rules
- safe mutation methods and invariants
- domain-specific exceptions

Must never do:
- persistence/file interactions
- UI rendering

## 4. Infrastructure

Folders:
- `Infrastructure/Repositories`
- `Infrastructure/Repositories/Models`
- `Infrastructure/Persistence`
- `Infrastructure/Data`
- `Infrastructure/Export`

Responsible for:
- repository implementations
- JSON persistence mechanics
- record/domain conversion
- seed data and export adapters

Must never do:
- user interaction
- business workflow orchestration

## Startup Wiring

`Program.cs` stays thin and only wires dependencies and menu entry flow.

Startup order:
1. build repositories
2. seed missing baseline data
3. build services from interfaces/concrete implementations
4. build menus
5. run main loop

## Design Patterns in Submission 2 Scope

The Submission 2 pattern set is concretely implemented as:

## Repository Pattern
- where: repository interfaces in `Application/Interfaces`, implementations in `Infrastructure/Repositories`
- why: keeps storage mechanics out of services and menus

## Strategy Pattern
- where: export (`IReportExporter`) and payment (`IPaymentStrategy`) strategy seams
- why: keeps algorithm variation separate from orchestration logic

## Factory Pattern
- where: role workspace resolution through `IRoleWorkspaceFactory` and `IUserWorkspace`
- why: centralizes role-to-workspace creation/routing

## Command Pattern
- where: `IMenuCommand` command maps + `MenuCommandDispatcher` in Main/Customer/Admin menus
- why: replaces large selection switches with command dispatch objects`r`n`r`n## Architectural Guardrails

1. menus do not call repositories
2. menus do not own business rules
3. domain mutations go through validated behavior methods
4. repository record models are separate files (no nested classes)
5. mutable runtime data persists in JSON
6. docs and tests are updated when behavior changes

## Trade-Offs

Trade-off 1: JSON over database
- pro: simple setup and predictable demos
- con: limited multi-process and migration capabilities

Trade-off 2: plaintext passwords in current coursework scope
- pro: keeps focus on architecture and workflow delivery
- con: not production-grade security

Trade-off 3: centralized transition rules in service
- pro: simple and easy to test
- con: may be split into dedicated transition handlers later

## Quick Viva Script

"The architecture is layered and strict: Presentation handles interaction, Application handles use-case orchestration, Domain protects core rules, and Infrastructure handles JSON/export adapters. For Submission 2 we concretely implement Repository, Strategy, Factory, and Command while preserving baseline behavior."


