# OOP Design Notes

## Purpose

This document explains the object-oriented design choices in CommerceConsole and how those choices protect correctness, readability, and maintainability.

## OOP Goals for This Project

The codebase uses OOP to achieve five goals:
1. protect business invariants where state lives
2. keep workflows readable and testable
3. reduce coupling between UI, business logic, and infrastructure
4. support extension without destabilizing baseline behavior
5. make design decisions easy to defend in a demo/viva

## Encapsulation: The Core Rule

Encapsulation approach:
- domain objects own their state
- state changes happen through explicit behavior methods
- direct mutation is restricted (`private set` or getter-only)

Examples:
- stock changes through `Product.Restock` / `Product.ReduceStock`
- wallet changes through `Customer.AddFunds` / `Customer.DebitFunds`
- cart changes through `Cart.AddItem` / `Cart.UpdateQuantity` / `Cart.RemoveItem`

Why this matters:
- each business mutation has one controlled entry point.

## Abstraction Through Interfaces

The application layer uses interfaces to separate contracts from implementations.

Contract examples:
- repositories: `IUserRepository`, `IProductRepository`, `IOrderRepository`
- services: `IAuthService`, `IOrderService`, `IReviewService`, etc.
- export seam: `IReportExporter`

Benefits:
- implementations can change without changing callers
- tests can isolate behavior cleanly
- boundaries remain explicit and enforceable

## Inheritance and Polymorphism (Used Carefully)

Inheritance is used where domain meaning is strong:
- `User` (abstract) -> `Customer`, `Administrator`

Polymorphism examples:
- role behavior based on `UserRole` and concrete user type
- interface-based repository replacement
- interface-based export replacement

Why not deep inheritance trees:
- deep hierarchies increase coupling and fragility
- composition keeps responsibilities clearer

## Composition Over Inheritance

Composition-heavy examples:
- `Customer` contains `Cart`, order history, and review history
- `Order` contains order item snapshots and payment details
- `Product` contains review collection and rating behavior

Benefits:
- easier local reasoning
- fewer hidden side effects
- easier test setup

## Guarded Validation and Invariants

Validation is applied in constructors and mutators to fail fast.

Invariant examples:
- no negative product price/stock
- review rating always in range 1..5
- order must contain at least one item
- payment amount must be greater than zero
- IDs must not be empty GUIDs

Design result:
- invalid state is blocked at source instead of repaired later.

## Why `sealed`, `abstract`, and `static` Were Chosen

`abstract`:
- used where base type should not be instantiated (`User`)

`sealed`:
- used on concrete types where extension is not required

`static`:
- used for stateless helpers (`ConsoleInputHelper`, `ConsoleTheme`, `MenuActionHelper`)

Rationale:
- class shape communicates intent and controls extension points.

## Separation of Concerns Through OOP Boundaries

Presentation concerns:
- prompt/read/render/retry behavior

Application concerns:
- workflow orchestration and cross-entity coordination

Domain concerns:
- state ownership and business rule enforcement

Infrastructure concerns:
- persistence and export mechanics

This separation is why the architecture stays clean even in a console app.

## LINQ Usage and Query Thinking

LINQ is used heavily for readable business queries.

Where LINQ appears and why:
- `ProductService.SearchProducts`: expressive filtering/sorting
- `ReportService`: grouped and aggregated sales metrics
- `ReviewService`: purchased-product eligibility filtering
- `InsightsService`: ranked recommendations and admin insights

How to explain in viva:
"LINQ lets query code mirror business questions like top sellers, low stock, and customer purchase history."

## Persistence Boundary Hygiene

Domain entities are not used as direct JSON schema contracts.

Instead:
- infrastructure uses `*Record` types
- repositories map between domain entities and records

Why this is strong design:
- domain model stays business-focused
- storage schema can evolve with less impact
- mapping logic is explicit and testable

## Session Context for Auth Flow

`SessionContext` stores current authenticated user context during runtime.

Benefits:
- clear sign-in/sign-out state handling
- avoids passing current-user references through every method
- keeps user context management centralized

## How OOP Choices Improve Testability

Testability benefits:
- domain invariants can be tested in isolation
- services can be tested without console I/O
- persistence can be tested with temporary data folders
- presentation helpers can be tested with controlled input/output harnesses

## Submission Pattern Scope

For Submission 1 documentation, only these pattern names are formally claimed:
- Repository Pattern
- Strategy Pattern
- Factory Pattern

Current status in code:
- Repository is implemented across all mutable workflows
- Strategy is implemented for report export behavior
- Factory is documented as a controlled refactor seam for the next phase

## Study Drills

Drill 1:
- pick one entity and list its invariants, mutators, and why setters are restricted.

Drill 2:
- trace checkout across layers and explain which layer owns each step.

Drill 3:
- explain Repository, Strategy, and Factory usage/status in this codebase.

Drill 4:
- justify why this architecture is DDD-inspired, but not full tactical DDD.

## 30-Second OOP Defense Script

"OOP here is used to protect state and keep responsibilities clear. Entities enforce invariants through controlled methods, services orchestrate workflows through interfaces, and infrastructure handles technical adapters like JSON persistence and export formatting. This keeps behavior safe, testable, and easier to evolve."
