# Design Patterns: Current State and Submission Positioning

## Purpose

This is the deep internal reference for pattern reasoning, viva prep, and Submission 2 planning.

Important documentation split:
- `docs/design-patterns-current.md` is intentionally narrow for Submission 1 claims.
- `.codex/design-patterns-current.md` is intentionally deeper for study and refactor planning.

## Submission 1 Claim-Safe Pattern Set

For Submission 1, the official pattern narrative is restricted to:
1. Repository Pattern
2. Strategy Pattern
3. Factory Pattern

This keeps claims clear, defensible, and aligned to marker expectations.

## 1) Repository Pattern

Implementation status:
- implemented and production-critical in current codebase

Evidence in code:
- generic contract: `Application/Interfaces/IRepository.cs`
- specialized contracts: `IUserRepository`, `IProductRepository`, `IOrderRepository`
- concrete adapters: `Infrastructure/Repositories/InMemoryUserRepository.cs`, `InMemoryProductRepository.cs`, `InMemoryOrderRepository.cs`
- persistence utility: `Infrastructure/Persistence/JsonFileStore.cs`

Why it is used:
- isolates storage concerns from business workflows
- keeps menus free of data access
- enables future persistence replacement with reduced ripple

Trade-off:
- adds interface and mapping overhead

## 2) Strategy Pattern

Implementation status:
- implemented as an export-behavior seam

Evidence in code:
- strategy contract: `Application/Interfaces/IReportExporter.cs`
- concrete strategy: `Infrastructure/Export/PdfReportExporter.cs`
- orchestration: `Application/Services/ReportExportService.cs`

Why it is used:
- report calculation and report formatting are separated
- new export modes can be added without changing report aggregation logic

Current limitation:
- one concrete exporter currently exists, but seam is explicit and working

## 3) Factory Pattern

Implementation status:
- documented refactor target for the next phase

Planned use:
- centralize role/menu creation and workspace resolution
- remove creation branching from startup/menu entry paths

Why it matters:
- clearer construction boundaries
- easier extension when role/workspace variants increase

## Deep Study Context (Not Part of Submission 1 Pattern Claims)

The codebase also uses several strong architecture techniques.
Treat these as design techniques for explanation, not as additional formal pattern claims for Submission 1:
- layered separation of concerns
- interface-first service/repository boundaries
- explicit startup wiring in `Program.cs`
- record-to-domain mapping in repository adapters
- fail-fast validation at entity/service boundaries
- centralized runtime session context

Use this wording to stay accurate:
"These techniques improve maintainability and testability, but our formal Submission 1 pattern set is intentionally restricted to Repository, Strategy, and Factory."

## Submission 2 Plan (Pattern Implementation Focus)

Primary goals:
1. complete Factory implementation for role/menu/workspace creation flow
2. broaden Strategy usage to payment processing options while preserving current wallet behavior
3. keep Repository contracts stable while refining query seams only when needed

Change safety rules:
- no feature-scope expansion during pattern refactor
- business behavior parity must be preserved
- tests and docs updated in the same cycle

## Pattern-Focused Test Checklist for Submission 2

Repository:
- contract behavior parity across repository implementations
- persistence correctness after add/update/remove

Strategy:
- exporter strategy selection/use behavior
- payment strategy success/failure paths (when added)

Factory:
- role-to-workspace/menu resolution
- invalid role handling behavior

Regression protection:
- auth/routing remains stable
- catalog/cart/checkout/reviews/reporting behavior unchanged
- no repository calls from presentation
- no GUID exposure in user-facing flows

## Viva Positioning Scripts

## 30-second Submission 1 script

"For Submission 1 we intentionally keep pattern claims focused: Repository for storage decoupling, Strategy for export behavior decoupling, and Factory as the next controlled refactor target. This keeps our claims precise, evidence-based, and easy to defend."

## 30-second Submission 2 script

"Submission 2 turns planned seams into concrete implementations by completing Factory creation flow and broadening Strategy usage, while preserving baseline behavior through regression tests and unchanged service boundaries."

## Common Mistake to Avoid in Viva

Do not claim "many patterns" without code-level proof.
Preferred approach:
- claim only what is explicitly present
- point to exact files
- describe trade-offs honestly
- show a staged plan for next-step patterns

## Extended Study Catalog (All Patterns And Design Techniques)

This section is study-focused and intentionally broader than Submission 1 claim scope.
Use it to learn and explain architecture depth, while still keeping formal rubric claims precise.

## A) Patterns/Techniques Already Present In Code

## 1. Repository Pattern

Definition:
- Encapsulates data access behind collection-like interfaces.

Where in this project:
- `Application/Interfaces/IRepository.cs`
- `Application/Interfaces/IUserRepository.cs`
- `Application/Interfaces/IProductRepository.cs`
- `Application/Interfaces/IOrderRepository.cs`
- `Infrastructure/Repositories/InMemory*Repository.cs`

Why used here:
- isolates JSON and file concerns from business workflows
- keeps services and menus storage-agnostic

Trade-off:
- additional interfaces and mapping code.

## 2. Strategy Pattern (Export Behavior Seam)

Definition:
- Encapsulates interchangeable algorithms behind a shared contract.

Where in this project:
- `Application/Interfaces/IReportExporter.cs`
- `Infrastructure/Export/PdfReportExporter.cs`
- `Application/Services/ReportExportService.cs`

Why used here:
- separates report-data generation from export formatting/output rules.

Trade-off:
- extra abstraction for a currently small exporter set.

## 3. Service Layer Pattern (Architecture Technique)

Definition:
- Organizes use-case orchestration in dedicated application services.

Where in this project:
- `Application/Services/AuthService.cs`
- `Application/Services/ProductService.cs`
- `Application/Services/OrderService.cs`
- `Application/Services/ReviewService.cs`
- `Application/Services/ReportService.cs`

Why used here:
- keeps menus thin
- centralizes business workflows for testing and consistency.

Trade-off:
- service count grows with feature surface.

## 4. Constructor Injection (DI Technique)

Definition:
- Dependencies are provided via constructors instead of being created inside classes.

Where in this project:
- all major services and menus are built with constructor dependencies from `Program.cs`.

Why used here:
- explicit dependencies
- simpler unit tests
- reduced hidden coupling.

Trade-off:
- longer constructor signatures in orchestration classes.

## 5. Composition Root (Wiring Pattern)

Definition:
- Single location where the object graph is assembled.

Where in this project:
- `Program.cs`

Why used here:
- runtime wiring is visible and predictable
- easier to reason about environment setup and startup order.

Trade-off:
- startup file grows as system grows.

## 6. Data Mapper (Persistence Mapping Technique)

Definition:
- Explicit mapping between domain entities and storage record models.

Where in this project:
- repository conversion functions and `Infrastructure/Repositories/Models/*Record.cs`.

Why used here:
- keeps domain objects free of persistence schema concerns.

Trade-off:
- boilerplate mapping code that must stay synchronized.

## 7. Rich Domain Model (Domain Modeling Style)

Definition:
- Entities encapsulate both state and behavior/invariant enforcement.

Where in this project:
- `Domain/Entities/Product.cs`, `Cart.cs`, `Customer.cs`, `Order.cs`, `Review.cs`, `Payment.cs`.

Why used here:
- invalid state is blocked at source by behavior methods.

Trade-off:
- requires discipline to avoid oversized entities.

## 8. Guard Clauses (Validation Style)

Definition:
- Fail-fast parameter and state checks at method/constructor boundaries.

Where in this project:
- domain constructors/mutators and service entry points.

Why used here:
- immediate, predictable validation behavior.

Trade-off:
- repeated checks if not consistently structured.

## 9. Session Context (Runtime Context Holder)

Definition:
- Centralized holder for current authenticated user in runtime session.

Where in this project:
- `Application/Interfaces/ISessionContext.cs`
- `Application/Services/SessionContext.cs`

Why used here:
- avoids passing current user through every menu/action method.

Trade-off:
- process-local context, not distributed session management.

## 10. Adapter Style (Infrastructure Integration Technique)

Definition:
- Wraps external/technical details behind application contracts.

Where in this project:
- repositories adapt JSON file storage
- PDF exporter adapts report model to external output format.

Why used here:
- protects application/domain from technical implementation detail churn.

Trade-off:
- more adapter classes to maintain.

## 11. Idempotent Seed Technique

Definition:
- Seed operations can run repeatedly without duplicating baseline records.

Where in this project:
- `Infrastructure/Data/SeedData.cs`

Why used here:
- deterministic startup and repeatable demos/tests.

Trade-off:
- additional startup checks.

## B) Patterns Intentionally Planned For Submission 2 Refactor

These are part of evolution planning and should be explained as controlled refactors, not baseline feature changes.

## 12. Factory Pattern (Formalization Target)

Planned role:
- centralize role-to-workspace/menu creation and remove branching from entry flow.

Expected contract style:
- `IRoleMenuFactory` and role-specific workspace/menu creators.

Main benefit:
- cleaner creation logic and easier extension for role variants.

## 13. Strategy Pattern Expansion (Payment)

Planned role:
- move wallet payment behavior behind `IPaymentStrategy` (wallet first, future options optional).

Main benefit:
- algorithm variation without touching checkout orchestration shape.

## 14. State-Style Transition Handling (Orders)

Planned role:
- extract transition logic into status-focused transition handlers while preserving transition matrix.

Main benefit:
- clearer lifecycle policy ownership and easier extension/testing for new statuses.

## 15. Command Pattern (Menu Dispatch)

Planned role:
- map menu actions to command handlers instead of large switch blocks.

Main benefit:
- thinner menus and clearer single-responsibility action handlers.

## 16. Specification Pattern (Query Reuse)

Planned role:
- reusable query specifications for product/search/report filters.

Main benefit:
- less duplicate filtering logic and clearer query intent in services.

## C) How To Speak About This Safely In Viva

Use this framing:
1. "Our formal Submission 1 pattern claims are Repository, Strategy, and Factory."
2. "Additional listed items are architecture/design techniques and planned controlled refactors used for learning and extension planning."
3. "We avoid overclaiming; every claim is mapped to concrete files and behavior."

## D) Quick Revision Checklist

Before demo/viva, be ready to answer:
1. What problem each pattern/technique solves here.
2. Where it appears in code (file-level proof).
3. Why it was chosen over a simpler alternative.
4. What trade-off it introduces.
5. How it supports safe Submission 2 evolution.

## E) Full Restored Pattern Matrix (Including Previously Removed Names)

This matrix is intentionally exhaustive for study.
It includes patterns currently implemented, partially represented, and planned.

| Pattern / Technique | Status | Where / Evidence | Why It Matters |
| --- | --- | --- | --- |
| Repository Pattern | Implemented | `IRepository<T>`, `IUserRepository`, `IProductRepository`, `IOrderRepository`, `InMemory*Repository` | Decouples storage from workflows |
| Strategy Pattern (Export + Payment) | Implemented | `IReportExporter`, `PdfReportExporter`, `ReportExportService`, `IPaymentStrategy`, `WalletPaymentStrategy` | Separates algorithm choice from orchestration |
| Factory Pattern | Implemented | `IRoleWorkspaceFactory`, `RoleWorkspaceFactory`, `IUserWorkspace` adapters | Centralizes role workspace routing and removes entry-point branching |
| Service Layer Pattern | Implemented | `Application/Services/*Service.cs` | Keeps business logic out of menus |
| Constructor Injection | Implemented | Constructors across services/menus + `Program.cs` wiring | Explicit dependencies, testability |
| Composition Root | Implemented | `Program.cs` | One place for graph wiring |
| Data Mapper Pattern | Implemented | `Infrastructure/Repositories/Models/*Record.cs` + repo mapping methods | Isolates domain from persistence schema |
| Rich Domain Model | Implemented | `Domain/Entities/*` behavior methods and invariants | Protects business state at source |
| Guard Clauses | Implemented | Constructors/mutators/service entry guards | Fail-fast validation and safer mutations |
| Session Context Pattern | Implemented | `ISessionContext`, `SessionContext` | Central auth state boundary |
| Adapter Pattern | Implemented (Style) | Repository adapters + export adapter | Wraps technical details behind contracts |
| Idempotent Seed Pattern | Implemented (Technique) | `SeedData.Seed(...)` uniqueness checks | Safe reruns and deterministic startup |
| State Pattern (Order Lifecycle) | Partial / Planned Upgrade | Current transition map in `OrderService` | Candidate to extract into state objects |
| Command Pattern (Menu Dispatch) | Implemented | `IMenuCommand`, `MenuCommandDispatcher`, command maps in Main/Customer/Admin menus | Standardizes dispatch and reduces menu switch complexity |
| Specification Pattern (Query Rules) | Planned / Conceptual | Current LINQ predicates in services/repos | Candidate for reusable filtering rules |
| Facade Pattern (Application as facade to UI) | Partial | Menus call service interfaces as simplified entry points | Reduces UI-facing complexity |
| Template Method (Workflow Skeleton) | Partial (informal) | Checkout orchestration order in `OrderService` | Stable workflow sequence with variable checks |

### How to use this matrix in viva

1. If asked "which patterns are in production now?" answer with rows marked `Implemented`.
2. If asked "what are next refactor patterns?" answer with rows marked `Planned` or `Partial / Planned Upgrade`.
3. Keep Submission 1 claims strict, but use this section for deep study and future defense.

## Submission 2 Implementation Delta (2026-03-09)

This section supersedes earlier "planned" notes where applicable.

Implemented in code now:
1. Repository Pattern
- repository contracts and JSON-backed implementations are active
- repository contract/parity tests were added

2. Strategy Pattern
- export strategy seam remains active (`IReportExporter` -> `PdfReportExporter`)
- payment strategy is now active (`IPaymentStrategy` -> `WalletPaymentStrategy`)
- `OrderService` now delegates payment behavior to strategy

3. Factory Pattern
- role workspace routing now uses `IRoleWorkspaceFactory`
- role-to-workspace adapters implemented via `IUserWorkspace`, `CustomerWorkspace`, and `AdminWorkspace`
- main menu role switch routing is removed

4. Command Pattern
- menu dispatch now uses `IMenuCommand` + `MenuCommandDispatcher`
- main/customer/admin selection switches are replaced with command maps
- explicit command classes cover key control flow (`MainLoginRouteCommand`, `MainExitMenuCommand`, `WorkspaceLogoutCommand`)

Remaining future candidates (not part of current completed four-pattern scope):
- State-style lifecycle extraction for order transitions
- Specification-style reusable query objects

Viva-safe short statement:
"Submission 2 concretely implemented Repository, Strategy, Factory, and Command patterns while preserving baseline behavior and passing full regression tests."

