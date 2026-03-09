# Folder Structure Rationale

## Purpose

This document explains folder ownership, placement rules, naming conventions, and the architectural reasoning behind the current project shape.

Think of this as the "map legend" for the repository.

## Top-Level Folders and Their Jobs

## `Presentation`

Primary mission:
- user interaction and display

Allowed:
- menu routing (`MainMenu`, `CustomerMenu`, `AdminMenu`)
- prompt loops and input recovery (`ConsoleInputHelper`)
- output theming/rendering (`ConsoleTheme`, display helpers)
- confirmation/pausing behavior for UX polish

Forbidden:
- repository calls
- domain-policy branching (stock/funds/order status rules)
- JSON or file writes

Why this boundary exists:
- UI should be swappable (console today, maybe API/web tomorrow) without rewriting core workflows.

## `Application`

Primary mission:
- use-case orchestration and policy coordination

Allowed:
- service interfaces and implementations
- workflow validation and orchestration
- repository abstraction contracts
- reporting/recommendation query composition

Forbidden:
- console reads/writes
- persistence implementation details

Why this boundary exists:
- keeps business behavior centralized and testable.

## `Domain`

Primary mission:
- business model and invariants

Allowed:
- entities with behavior
- domain enums and exceptions
- state transitions protected by method-level guards

Forbidden:
- repository access
- menu/display logic
- exporter logic

Why this boundary exists:
- domain correctness must not depend on UI or storage technology.

## `Infrastructure`

Primary mission:
- technical adapters and implementations

Allowed:
- repository implementations
- persistence file-store utility
- record-model mapping
- seed data provisioning
- export adapter implementation

Forbidden:
- user interaction concerns
- cross-use-case policy orchestration

Why this boundary exists:
- technology choices (JSON, PDF, storage mechanics) should remain replaceable.

## `Tests`

Primary mission:
- executable quality evidence

Allowed:
- architecture-mirrored test suites (`Domain`, `Application`, `Infrastructure`, `Presentation`)
- deterministic test harnesses

Forbidden:
- production runtime logic
- stale one-off experiments committed as test files

Why this boundary exists:
- mirrors architecture, making gaps visible and regression safety measurable.

## `docs`

Primary mission:
- explain behavior and design intent for maintainers and reviewers

Allowed:
- architecture notes
- feature behavior specs
- OOP and design rationale


Forbidden:
- inaccurate or stale behavior claims
- placeholder docs that no longer map to code

Why this boundary exists:
- documentation is part of deliverable quality, not an afterthought.

## `.codex`

Primary mission:
- process governance for implementation standards and milestones

Allowed:
- coding standards
- implementation blueprint
- prompt pack
- issue/milestone definitions

Forbidden:
- production source code

Why this boundary exists:
- delivery process should be explicit and separate from runtime code.

## Naming Conventions and Why They Matter

## Folders

Naming style:
- singular responsibility names (`Domain`, `Application`, `Infrastructure`)
- avoid ambiguous names like `Utils` at top-level

Why:
- folder names teach architecture intent to new contributors immediately.

## Interfaces

Convention:
- prefix `I` (`IOrderService`, `IReportExporter`)

Why:
- quickly signals abstraction vs implementation
- simplifies code reading and test double setup

## Services

Convention:
- suffix `Service` (`OrderService`, `ReviewService`)

Why:
- marks orchestration/use-case boundary types clearly.

## Repository Implementations

Convention:
- implementation prefix + `Repository` suffix (`InMemoryUserRepository`)

Why:
- concrete storage strategy is explicit in class name.

## Persistence Models

Convention:
- suffix `Record` (`UserRecord`, `OrderRecord`)

Why:
- separates transport/storage shape from domain entity shape.

## Menu/Display Helpers

Convention:
- `*Helper`, `*Renderer`, `*Prompt`

Why:
- presentation concerns are centralized and discoverable.

## Placement Rules: "What Goes Where"

Decision checklist when adding a new class:
1. Does it read/write console? -> `Presentation`
2. Does it orchestrate use-case flow? -> `Application/Services`
3. Does it enforce business invariant/state mutation? -> `Domain/Entities`
4. Does it persist/serialize/map/export? -> `Infrastructure`
5. Is it only test support? -> `Tests`
6. Is it explanation content? -> `docs`

## Common Mistakes (And Current Protection)

Mistake 1: menu directly calls repository
- Risk: UI and storage coupling, hidden business drift.
- Protection: repository interfaces are injected into services, not menus.

Mistake 2: domain class handles file I/O
- Risk: domain polluted with infrastructure concerns.
- Protection: `JsonFileStore` and repositories isolate I/O in infrastructure.

Mistake 3: nested persistence classes inside repositories
- Risk: bloated files and poor reuse.
- Protection: persistence models live in `Infrastructure/Repositories/Models`.

Mistake 4: exposing GUID entry to end users
- Risk: poor UX and accidental identifier leakage.
- Protection: index-based selection in menus/helpers.



