# Design Patterns: Complete Study Reference (Submission 2 Final)

## Purpose

This is the full study version of pattern documentation for personal revision.
It is intentionally broader than assessor-facing `docs/design-patterns-current.md`.

Use this file to:
1. understand what is concretely implemented now
2. distinguish implemented patterns from architecture techniques
3. prepare accurate viva answers without over-claiming
4. plan low-risk future refactors

## Current Implementation Baseline (March 9, 2026)

- Submission stage: Submission 2 implementation complete
- Core implemented pattern set:
  1. Repository
  2. Strategy
  3. Factory
  4. Command
- Regression status: `115/115` tests passing

---

## A) Core Implemented Patterns (Concretely in Code)

## 1. Repository Pattern

Definition:
- Encapsulates data-access behavior behind repository interfaces.
- In practice here, it creates a boundary between domain/application logic and persistence mechanics (JSON records, file IO, mapping, and storage shape).
- It gives services a stable contract focused on business intent (users, products, orders), not storage internals.

Where implemented:
- Contracts:
  - `Application/Interfaces/IRepository.cs`
  - `IUserRepository.cs`, `IProductRepository.cs`, `IOrderRepository.cs`
- Implementations:
  - `Infrastructure/Repositories/InMemoryUserRepository.cs`
  - `Infrastructure/Repositories/InMemoryProductRepository.cs`
  - `Infrastructure/Repositories/InMemoryOrderRepository.cs`
- Persistence utility:
  - `Infrastructure/Persistence/JsonFileStore.cs`

Why used here:
- keeps services and menus storage-agnostic
- isolates JSON and file mechanics from business workflows

Expanded explanation:
- Application services (`UserService`, `ProductService`, `OrderService`) coordinate use-cases. They should answer business questions, not decide how files are loaded/saved.
- Repository interfaces let the application layer ask for domain entities while infrastructure handles conversion to/from persistence record models.
- The `JsonFileStore` concern stays localized to infrastructure, which reduces accidental coupling between business rules and storage details.

Why this choice is strong for Submission 2:
- Submission 2 increased orchestration complexity (command dispatch, strategy seams, role workspaces). Keeping persistence concerns behind repositories prevents that complexity from leaking across layers.
- Repository contract tests lock expected behavior regardless of implementation details, so refactors are safer.
- It preserves a low-risk path for future storage replacement (for example, database-backed repositories) without rewriting menu/service orchestration.

Trade-off:
- extra interfaces and mapping maintenance overhead

Common arguments and rebuttals:
- Argument: "For a small console app, repositories are over-engineering."
- Rebuttal: The project already has multiple aggregates (`User`, `Product`, `Order`) and non-trivial workflows; repository seams paid off immediately through contract tests and cleaner services.
- Argument: "Just use one generic repository and skip specific interfaces."
- Rebuttal: Aggregate-specific interfaces communicate intent and prevent leaking query/storage concerns into services; this keeps domain rules readable and cohesive.
- Argument: "Services could call `JsonFileStore` directly."
- Rebuttal: That would mix IO, mapping, and business decisions in the same classes, increasing churn and making tests brittle.

Evidence in tests:
- `Tests/CommerceConsole.Tests/Infrastructure/RepositoryContractTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/JsonPersistenceTests.cs`
## 2. Strategy Pattern

Definition:
- Encapsulates interchangeable algorithms behind a common contract.
- In this codebase, strategy boundaries isolate "how a variable action is executed" from "when and why that action is invoked."

Where implemented (payment):
- `Application/Interfaces/IPaymentStrategy.cs`
- `Application/Services/WalletPaymentStrategy.cs`
- `Application/Services/OrderService.cs` (delegates payment execution)

Where implemented (report export):
- `Application/Interfaces/IReportExporter.cs`
- `Infrastructure/Export/PdfReportExporter.cs`
- `Application/Services/ReportExportService.cs`

Why used here:
- payment behavior varies independently from checkout orchestration
- export format varies independently from report aggregation

Expanded explanation:
- `OrderService` owns checkout workflow and business guards; payment execution itself is delegated to `IPaymentStrategy`.
- `ReportExportService` owns report assembly/use-case flow; output-format behavior is delegated to `IReportExporter`.
- This separation keeps orchestrators stable while allowing policy/format algorithms to change at their own pace.

Why this choice is strong for Submission 2:
- Submission 2 formalized payment and export seams as concrete extension points, improving design clarity without changing user-facing behavior.
- Strategy contracts reduce regression risk by allowing focused unit tests on each algorithm implementation.
- The architecture remains open for additional payment/export options with minimal code movement in core workflows.

Trade-off:
- abstraction overhead when only one concrete strategy exists in a seam

Common arguments and rebuttals:
- Argument: "There is only one payment strategy now; an interface is unnecessary."
- Rebuttal: The seam is intentional for volatility. Payment rules are likely to evolve first, and strategy makes that change additive instead of invasive.
- Argument: "A simple `if/else` in `OrderService` is easier."
- Rebuttal: It is easier short-term, but it couples orchestration and algorithm branches, making checkout harder to test and maintain as options grow.
- Argument: "Export strategy is premature since only PDF exists."
- Rebuttal: Export is a classic variability axis; isolating it now keeps reporting logic independent from output tooling and avoids later refactor pressure.

Evidence in tests:
- `Tests/CommerceConsole.Tests/Application/WalletPaymentStrategyTests.cs`
- `Tests/CommerceConsole.Tests/Application/OrderServiceTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/PdfReportExporterTests.cs`
## 3. Factory Pattern

Definition:
- Centralizes creation/resolution logic so callers do not branch on concrete types.
- Here, the factory resolves runtime role context into the correct workspace object behind a stable interface.

Where implemented:
- `Presentation/Workspaces/IUserWorkspace.cs`
- `Presentation/Workspaces/IRoleWorkspaceFactory.cs`
- `Presentation/Workspaces/RoleWorkspaceFactory.cs`
- `Presentation/Workspaces/CustomerWorkspace.cs`
- `Presentation/Workspaces/AdminWorkspace.cs`
- Used by `Presentation/Menus/MainMenu.cs`

Why used here:
- removes role-based workspace routing logic from main flow
- keeps role-to-workspace mapping explicit and testable

Expanded explanation:
- `MainMenu` should coordinate interaction flow, not know every workspace construction/routing detail.
- `RoleWorkspaceFactory` is a single decision point for role-to-workspace mapping (`CustomerWorkspace` vs `AdminWorkspace`).
- Workspace contracts (`IUserWorkspace`) keep menu code focused on behavior, while factory logic stays isolated and easy to verify.

Why this choice is strong for Submission 2:
- Submission 2 added commandized menus; pairing that with centralized workspace resolution keeps control flow predictable.
- Factory tests provide direct proof that role routing behavior is correct and stable.
- It avoids scattering role checks across presentation classes, reducing change risk when roles/workspaces evolve.

Trade-off:
- one more abstraction layer for simple role count

Common arguments and rebuttals:
- Argument: "A direct `switch` in `MainMenu` would be shorter."
- Rebuttal: Shorter initially, but it mixes orchestration with role resolution and encourages duplicated branching as menus evolve.
- Argument: "Dependency injection alone can resolve workspace instances."
- Rebuttal: DI creates objects, but the role-based selection policy still needs a clear owner; factory is that policy boundary.
- Argument: "Factory hides too much logic."
- Rebuttal: In this design, logic is explicit and test-covered in one place, which is more transparent than branching spread across multiple callers.

Evidence in tests:
- `Tests/CommerceConsole.Tests/Presentation/RoleWorkspaceFactoryTests.cs`
- routing assertions in `Tests/CommerceConsole.Tests/Presentation/MenuCommandTests.cs`
## 4. Command Pattern

Definition:
- Encapsulates actions as command objects and dispatches by key/context.
- In this project, each menu action is represented as a command with a standardized execution contract and result model.

Where implemented:
- Contracts/infrastructure:
  - `Presentation/Commands/IMenuCommand.cs`
  - `Presentation/Commands/MenuCommandDispatcher.cs`
  - `Presentation/Commands/MenuCommandResult.cs`
  - `Presentation/Commands/DelegateMenuCommand.cs`
- Explicit control-flow commands:
  - `Presentation/Commands/MainLoginRouteCommand.cs`
  - `Presentation/Commands/MainExitMenuCommand.cs`
  - `Presentation/Commands/WorkspaceLogoutCommand.cs`
- Usage:
  - `Presentation/Menus/MainMenu.cs`
  - `Presentation/Menus/CustomerMenu.cs`
  - `Presentation/Menus/AdminMenu.cs`

Why used here:
- removes large selection `switch` blocks
- standardizes menu dispatch behavior
- keeps menu loops thinner

Expanded explanation:
- `MenuCommandDispatcher` maps user selections to commands and enforces consistent dispatch behavior across menus.
- `IMenuCommand` + `MenuCommandResult` unify how commands signal outcomes (continue, route, exit/logout).
- Dedicated commands for key control-flow transitions (`MainLoginRouteCommand`, `MainExitMenuCommand`, `WorkspaceLogoutCommand`) make navigation rules explicit rather than hidden inside menu loops.

Why this choice is strong for Submission 2:
- Submission 2 expanded menu behavior while preserving business flow. Command dispatch kept growth manageable without bloated switch blocks.
- It enables targeted command-level tests and routing assertions, improving confidence in UI control flow.
- New menu actions can be added with minimal edits to existing menus, lowering regression risk.

Trade-off:
- more types compared to direct branching

Common arguments and rebuttals:
- Argument: "Switch statements are easier to read for console menus."
- Rebuttal: They are fine for tiny static menus, but readability degrades quickly when each case grows behavior, validation, and routing branches.
- Argument: "Too many command classes add boilerplate."
- Rebuttal: `DelegateMenuCommand` reduces boilerplate where a full class is unnecessary, while explicit classes remain for important control-flow actions.
- Argument: "Command indirection hurts performance."
- Rebuttal: For this IO-driven console app, the overhead is negligible compared to gains in testability, extensibility, and maintainability.

Evidence in tests:
- `Tests/CommerceConsole.Tests/Presentation/MenuCommandTests.cs`
---

## B) Additional Patterns and Architecture Techniques Implemented

These are present in code and valid to explain in study/viva. They are not part of the narrow four-pattern assessor claim set.

## Service Layer Pattern

Where:
- `Application/Services/*Service.cs`

Why:
- centralizes use-case orchestration
- keeps presentation thin

## Constructor Injection

Where:
- constructors across services, menus, factories
- composition in `Program.cs`

Why:
- explicit dependencies and improved testability

## Composition Root

Where:
- `Program.cs`

Why:
- one place to wire object graph and runtime flow

## Data Mapper Pattern

Where:
- `Infrastructure/Repositories/Models/*Record.cs`
- map logic inside repository implementations

Why:
- domain entities stay persistence-agnostic

## Rich Domain Model

Where:
- domain behavior methods in `Domain/Entities/*`

Why:
- invariants enforced near owned state

## Guard Clauses

Where:
- constructors/mutators/service entry methods

Why:
- fail-fast rule enforcement and predictable errors

## Session Context Pattern

Where:
- `Application/Interfaces/ISessionContext.cs`
- `Application/Services/SessionContext.cs`

Why:
- central authenticated-user runtime context

## Adapter Style (Infrastructure)

Where:
- JSON/persistence adapters in repositories
- exporter adapter `PdfReportExporter`

Why:
- isolates technical integration details behind contracts

## Idempotent Seeding Technique

Where:
- `Infrastructure/Data/SeedData.cs`

Why:
- repeatable startup without duplicate baseline records

---

## C) Partial / Informal Pattern Shapes in Current Code

## Facade-like Application Boundary (Informal)

Observation:
- Menus call service interfaces as simplified access points to complex workflows.

Value:
- presentation code interacts with fewer objects and less orchestration detail.

## Template-like Workflow Skeleton (Informal)

Observation:
- `OrderService.Checkout` follows fixed orchestration stages:
  1. validate customer/cart
  2. build checkout lines and stock checks
  3. execute payment strategy
  4. reduce stock and persist products
  5. create and persist order
  6. clear cart and persist customer

Value:
- stable workflow sequence with isolated variation points.

---

## D) Future Pattern Candidates (Not Yet Implemented)

These are reasonable future upgrades and should be described as optional refactors, not current implementations.

## State Pattern (Order Lifecycle Extraction)

Current state:
- transition matrix is centralized in `OrderService` static dictionary.

Potential refactor:
- state handlers per order status implementing transition policy.

Why later:
- current matrix is small and already test-covered.

## Specification Pattern (Reusable Queries)

Current state:
- LINQ predicates are embedded inside services/repositories.

Potential refactor:
- reusable query specifications for active/in-stock/low-stock/search cases.

Why later:
- useful when query reuse and cross-service duplication grow.

---

## E) Submission 1 -> Submission 2 Pattern Evolution

Submission 1 emphasis:
- repository seams and basic strategy/factory narrative

Submission 2 completed implementation:
- payment strategy concretely integrated
- role workspace factory concretely integrated
- command dispatch concretely integrated across all menus
- repository contract behavior hardened with dedicated tests

Result:
- improved extensibility without feature-scope expansion
- preserved business behavior and UX rules

---

## F) Viva-Safe Pattern Claims

Use this exact framing:

1. "Our formal implemented pattern set is Repository, Strategy, Factory, and Command."
2. "Additional items I describe are architecture techniques already present in code (Service Layer, DI, Data Mapper, Rich Domain Model, Guard Clauses, Session Context)."
3. "Future candidates like State and Specification are documented as optional refactors, not claimed as implemented."

---

## G) Full Study Matrix (All Relevant Patterns/Techniques)

| Pattern / Technique | Status | Evidence | Why It Matters |
| --- | --- | --- | --- |
| Repository Pattern | Implemented | `IRepository<T>`, repo interfaces + `InMemory*Repository` | Decouples storage from workflows |
| Strategy Pattern (Payment) | Implemented | `IPaymentStrategy`, `WalletPaymentStrategy`, `OrderService` | Keeps payment algorithm separate from checkout orchestration |
| Strategy Pattern (Export) | Implemented | `IReportExporter`, `PdfReportExporter`, `ReportExportService` | Keeps report calculation separate from output format |
| Factory Pattern | Implemented | `IRoleWorkspaceFactory`, `RoleWorkspaceFactory`, `IUserWorkspace` adapters | Centralizes role workspace resolution |
| Command Pattern | Implemented | `IMenuCommand`, `MenuCommandDispatcher`, command maps in all menus | Replaces selection switches with composable actions |
| Service Layer Pattern | Implemented | `Application/Services/*` | Keeps business logic out of presentation |
| Constructor Injection | Implemented | constructor wiring + `Program.cs` | Explicit dependencies and testability |
| Composition Root | Implemented | `Program.cs` | Single object-graph assembly point |
| Data Mapper Pattern | Implemented | repo mapping + `*Record` models | Separates domain from persistence schema |
| Rich Domain Model | Implemented | domain behavior methods and guards | Protects invariants at entity boundary |
| Guard Clauses | Implemented | constructors/mutators/service guards | Fail-fast validation |
| Session Context Pattern | Implemented | `ISessionContext`, `SessionContext` | Central session state handling |
| Adapter Style | Implemented | repository/export adapters | Isolates technical concerns |
| Idempotent Seeding | Implemented | `SeedData` uniqueness checks | Stable reruns/demos |
| Facade-like Service Boundary | Partial (informal) | menu -> service interface usage | Simplifies UI interaction surface |
| Template-like Checkout Flow | Partial (informal) | fixed orchestration in `OrderService.Checkout` | Stable workflow sequencing |
| State Pattern (Order transitions) | Candidate | transition map in `OrderService` | Cleaner lifecycle policy object model if complexity grows |
| Specification Pattern (Query reuse) | Candidate | LINQ predicates in services | Reusable query intent objects if duplication grows |

## Final Revision Reminder

Before any demo/viva:
1. quote implemented patterns only when asked for concrete implementation
2. map every claim to exact files and tests
3. clearly label candidates as "future refactor" to avoid over-claiming



