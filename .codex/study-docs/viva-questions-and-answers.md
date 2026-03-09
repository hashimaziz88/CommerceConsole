# Viva Questions and Model Answers (Submission 2 Final)

## Purpose

This is the study-focused viva bank for final Submission 2 preparation.
All answers are aligned to the current codebase implementation.

## Architecture Identity

### Q1. What architecture style is implemented?

A:
- Layered, domain-centered architecture.
- Dependency direction is `Presentation -> Application -> Domain`.
- Infrastructure implements application contracts and uses domain types.

### Q2. Is this full DDD?

A:
- No.
- It is DDD-inspired: strong domain entities, invariants, and use-case services.
- It does not include full bounded contexts, domain events, or formal aggregate catalogs.

### Q3. Why use this architecture in a console app?

A:
- Console is only a delivery channel.
- Business logic still needs maintainable boundaries, testability, and extension safety.

### Q4. Where is the composition root?

A:
- `Program.cs`.
- It wires repositories, services, strategies, factory, menus, and starts main flow.

## Folder and Separation Questions

### Q5. What belongs in `Presentation`?

A:
- Menus, prompts, rendering, command dispatch.
- It should never own business rules or repository access.

### Q6. What belongs in `Application`?

A:
- Service interfaces and workflow orchestration.
- It coordinates repositories/domain behavior and exposes use-case operations.

### Q7. What belongs in `Domain`?

A:
- Entities, enums, and domain exceptions.
- Domain owns invariants and validated state mutation behavior.

### Q8. What belongs in `Infrastructure`?

A:
- Repository implementations, persistence details, seed data, exporters.
- It should not own workflow policy or menu logic.

## OOP and Encapsulation

### Q9. Why use `private set` or getter-only properties in entities?

A:
- To prevent uncontrolled external mutation.
- Mutations go through validated behavior methods, which protect invariants.

### Q10. Why are dependency fields usually `private readonly`?

A:
- To prevent accidental reassignment after construction.
- This keeps object behavior stable and predictable.

### Q11. Why is `User` abstract with a protected constructor?

A:
- `User` is a shared base concept and not a direct runtime role.
- Protected constructor allows only valid derived role types to initialize shared state.

### Q12. Why use static helper classes in presentation?

A:
- Helpers are stateless formatting/input utilities.
- Static shape avoids unnecessary object state and allocation.

## Business Rule Placement

### Q13. Why are menus kept thin?

A:
- Menus should route and render only.
- If business rules are in menus, logic duplicates and tests become brittle.

### Q14. Where does checkout orchestration live?

A:
- `OrderService.Checkout`.
- It coordinates cart validation, payment, stock updates, order creation, and cart clear.

### Q15. Why does checkout still use strategy?

A:
- Orchestration remains in `OrderService`, but payment algorithm is delegated to `IPaymentStrategy`.
- This keeps sequencing stable while allowing payment variation.

### Q16. Where are order transition rules enforced?

A:
- Centralized in `OrderService` via allowed transition map.
- Admin UI only offers allowed next statuses.

### Q17. How is purchased-only review enforcement implemented?

A:
- `ReviewService` calculates reviewable products from customer purchase history.
- Customer menu only displays eligible products for review selection.

## LINQ and Query Reasoning

### Q18. Where is LINQ used meaningfully?

A:
- catalog filtering/searching
- reporting aggregates (revenue, status counts, best sellers, low stock)
- purchased-product review eligibility
- recommendations and insights generation

### Q19. Why use LINQ instead of manual loops?

A:
- LINQ maps directly to business query intent and reduces boilerplate.
- It improves readability when expressing filtering, grouping, and aggregation.

## Persistence and Infrastructure

### Q20. Why JSON persistence?

A:
- Low setup overhead and deterministic behavior for coursework and demos.
- Repository contracts preserve future storage replacement options.

### Q21. How is persistence isolated from domain entities?

A:
- Infrastructure uses separate `*Record` models.
- Repositories map between domain entities and storage records.

### Q22. Is seeding safe across reruns?

A:
- Yes, seeding is idempotent for baseline records.

## Pattern Questions (Submission 2)

### Q23. Which design patterns are concretely implemented?

A:
- Repository, Strategy, Factory, and Command.

### Q24. Where is Repository pattern evidence?

A:
- `Application/Interfaces/*Repository.cs`
- `Infrastructure/Repositories/InMemory*Repository.cs`
- persistence tests in `RepositoryContractTests` and `JsonPersistenceTests`

### Q25. Where is Strategy pattern evidence?

A:
- `IPaymentStrategy` + `WalletPaymentStrategy`
- `IReportExporter` + `PdfReportExporter`
- orchestration usage in `OrderService` and `ReportExportService`

### Q26. Where is Factory pattern evidence?

A:
- `IRoleWorkspaceFactory` + `RoleWorkspaceFactory`
- workspace adapters `CustomerWorkspace` and `AdminWorkspace`
- role routing in `MainMenu`

### Q27. Where is Command pattern evidence?

A:
- `IMenuCommand`, `MenuCommandDispatcher`, explicit command classes
- command-map dispatch in `MainMenu`, `CustomerMenu`, and `AdminMenu`

### Q28. Why not claim State or Specification as implemented?

A:
- They are not concretely implemented as dedicated object models.
- They are documented as optional future refactors only.

## Testing and Quality

### Q29. What is the latest regression status?

A:
- 115 total tests, 115 passed, 0 failed, 0 skipped.

### Q30. How is the test suite organized?

A:
- By architecture layer: Domain, Application, Infrastructure, Presentation.

### Q31. Why include presentation tests in a console app?

A:
- Input loops, command dispatch, and rendering helpers affect usability and reliability.
- These are deterministic and testable with console harness redirection.

### Q32. Which tests prove pattern refactor safety?

A:
- Repository: `RepositoryContractTests`
- Strategy: `WalletPaymentStrategyTests` + checkout/export tests
- Factory: `RoleWorkspaceFactoryTests`
- Command: `MenuCommandTests`

## Trade-Off and Risk Questions

### Q33. What are current known limitations?

A:
- plaintext password handling (coursework scope)
- JSON persistence is not designed for concurrent multi-process writes
- order lifecycle still uses centralized transition map instead of state objects

### Q34. Why is this still considered maintainable?

A:
- boundaries are strict, dependencies are explicit, and tests provide regression safety.

### Q35. What changes if persistence moves to a database?

A:
- repository implementations change.
- service/domain/presentation contracts remain largely stable.

### Q36. What changes if UI moves from console to web/API?

A:
- presentation layer is replaced.
- application/domain logic remains mostly reusable.

## Short Scripts for Fast Answers

### 30-second architecture script

"CommerceConsole uses layered, domain-centered architecture: Presentation handles interaction, Application orchestrates use cases, Domain protects business invariants, and Infrastructure handles JSON/export adapters. This keeps behavior centralized and testable."

### 30-second pattern script

"Submission 2 concretely implements Repository, Strategy, Factory, and Command. Repositories isolate storage, strategies isolate algorithm variation, factory centralizes role workspace resolution, and command objects standardize menu dispatch."

### 30-second quality script

"The project has architecture-aligned regression tests across all layers, currently 77 passing tests. This gives confidence that refactors improved structure without breaking baseline behavior."

## Final Self-Check (Before Viva)

You are ready if you can answer clearly:
1. Why this is domain-centered but not full DDD.
2. Where each of the 4 implemented patterns exists in code.
3. Why menus never call repositories directly.
4. How checkout uses both orchestration and strategy.
5. What the real current limits are and how architecture mitigates them.

