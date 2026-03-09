# Viva Questions and Model Answers

## Purpose

This document is a high-value viva revision bank: likely questions, concise answers, and practical framing tied to the actual codebase.

## Architecture and Design Identity

### Q1. What architecture style does this project use?

A:
- Layered architecture with clean boundaries.
- DDD-inspired rich domain modeling.
- Not full tactical DDD.

### Q2. Why not call it full DDD?

A:
- We do not yet model full aggregate boundaries, domain events, or bounded contexts.
- We intentionally implemented DDD-inspired discipline suited to scope.

### Q3. Why layered architecture for a console app?

A:
- Console is only a UI channel; business logic still needs structure.
- Layering prevents menu code from becoming untestable monolith logic.

### Q4. What is the dependency direction?

A:
- `Presentation -> Application -> Domain`
- `Infrastructure` implements `Application` contracts and uses `Domain`.
- `Domain` does not depend on higher layers.

### Q5. Where is the composition root?

A:
- `Program.cs`.
- It wires repositories, services, exporters, session, and menus.

## Folder and Naming Choices

### Q6. Why these specific folders?

A:
- Each folder maps to one responsibility: interaction, workflows, business rules, technical adapters, testing, docs, process governance.

### Q7. Why are interfaces named with `I` and services with `Service` suffix?

A:
- To make abstraction and orchestration roles immediately discoverable.

### Q8. Why are repository record types in separate files?

A:
- To avoid nested-class bloat and to separate persistence schema from domain behavior cleanly.

### Q9. Why hide GUIDs in UI?

A:
- Better UX, fewer errors, and reduced internal identifier exposure.

## OOP, Encapsulation, and Modifiers

### Q10. Why use `private set` on many domain properties?

A:
- To force updates through validated methods and protect invariants.

### Q11. Why `private readonly` dependencies?

A:
- Stable constructor wiring and no accidental reassignment.

### Q12. Why is `User` abstract with a protected constructor?

A:
- `User` is a shared concept, not a concrete role.
- Protected constructor allows only derived role types to initialize it.

### Q13. Why are many classes `sealed`?

A:
- To avoid uncontrolled inheritance and invariant bypass via overrides.

### Q14. Why static helper classes?

A:
- They are stateless utilities for presentation formatting/input handling.

## Business Rule Placement

### Q15. Why keep menus thin?

A:
- Menus should route and render only.
- Business logic in menus becomes duplicated, hard to test, and fragile.

### Q16. Where does checkout orchestration live and why?

A:
- In `OrderService.Checkout`.
- It coordinates multi-entity updates consistently.

### Q17. Where are order status transition rules enforced?

A:
- Centralized in `OrderService` transition map.
- Admin UI only displays allowed next statuses.

### Q18. How do you enforce purchased-only reviews?

A:
- `ReviewService` filters eligible products from purchase history and validates at add-review call.

## LINQ and Query Thinking

### Q19. Where is LINQ used meaningfully?

A:
- catalog search/filtering
- reporting aggregates (revenue/status/best-sellers)
- review eligibility
- recommendations and insights

### Q20. Why LINQ here instead of manual loops?

A:
- Query intent mirrors business questions and reduces boilerplate.

## Persistence and Infrastructure

### Q21. Why JSON persistence?

A:
- Scope-fit persistence with low setup overhead and demo reliability.
- Repository contracts preserve future DB swap option.

### Q22. How do you avoid partial file writes?

A:
- `JsonFileStore` writes to temp file then replaces target file.

### Q23. What happens if JSON is malformed?

A:
- load falls back to empty list to keep app usable.

### Q24. Is seeding repeat-safe?

A:
- Yes, seeding is idempotent for admin and products.

## Testing and Quality

### Q25. What is your current test evidence?

A:
- 61 passing tests in the architecture-mirrored test project.

### Q26. How are tests organized?

A:
- By architecture layer: Domain, Application, Infrastructure, Presentation.

### Q27. Why test failure paths so heavily?

A:
- Robustness depends on invalid-input handling and rule enforcement, not only happy paths.

### Q28. How are console helpers tested?

A:
- With input/output redirection harness to validate prompts and retry loops deterministically.

## Design Patterns

### Q29. Which patterns are currently implemented?

A:
- Repository, Service Layer, Constructor Injection, Composition Root, Data Mapper, Rich Domain Model, Guard Clauses, Session Context, strategy-style exporter seam.

### Q30. Which patterns are planned next?

A:
- Factory extraction, broader Strategy variants, and State-style lifecycle transitions.

### Q31. Why not introduce every pattern now?

A:
- Pattern usage is requirement-driven; overengineering would reduce clarity and delivery speed.

## Trade-Off and Risk Questions

### Q32. What are known limitations?

A:
- plain-text passwords
- JSON single-process assumption
- centralized transition map not yet extracted to state objects

### Q33. How is the code prepared for change despite those limits?

A:
- boundaries and interfaces isolate change impact.

### Q34. If asked to move to database tomorrow, what changes?

A:
- implement repository interfaces with DB adapters; service/domain/menu contracts stay largely stable.

### Q35. If asked to replace console with web API/UI, what changes?

A:
- replace presentation layer; keep application/domain and most infrastructure intact.

## Rapid Answer Scripts

## 30-second architecture answer

"It is a layered, DDD-inspired design: Presentation handles interaction, Application orchestrates use cases, Domain enforces invariants, and Infrastructure handles persistence/export adapters. This keeps rules centralized and change-risk low."

## 30-second OOP answer

"State is protected by restricted setters and behavior methods with guard clauses. Services coordinate workflows across entities, and interfaces keep implementations replaceable and test-friendly."

## 30-second pattern answer

"Repository and Service Layer separate workflows from storage, Data Mapper isolates schema, and composition root plus constructor injection keep dependencies explicit. We already have seams for future factory/strategy/state upgrades."

## Quick Self-Test Questions

Use these to prepare before viva:
1. Why is this DDD-inspired but not full DDD?
2. Why do menus not call repositories?
3. Why are GUIDs hidden in UI?
4. Why does checkout belong in `OrderService`?
5. How do repository record models protect domain design?
6. What pattern gives easiest Submission 2 win first, and why?

If you can answer these confidently, you are ready for most architecture and design questioning.
