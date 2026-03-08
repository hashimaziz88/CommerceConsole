# OOP Design Notes

## Purpose

This is the long-form OOP reasoning document for CommerceConsole.
It explains not just what classes exist, but why they are shaped this way.

Use this as your core revision source for design and architecture questions.

## OOP Goals for This Project

The codebase uses OOP to achieve five goals:
1. protect business invariants where state lives
2. keep workflows readable and testable
3. reduce coupling between UI, business logic, and infrastructure
4. support extension without destabilizing baseline behavior
5. make design decisions explainable under viva questioning

## Encapsulation: The Most Important Rule Here

Encapsulation strategy:
- domain objects hold state
- state changes are made through explicit behavior methods
- direct setters are restricted (`private set` or getter-only)

Examples:
- stock changes only through `Product.Restock` / `Product.ReduceStock`
- wallet changes only through `Customer.AddFunds` / `Customer.DebitFunds`
- cart mutation only through `Cart.AddItem` / `Cart.UpdateQuantity` / `Cart.RemoveItem`

Why this matters:
- there is a single controlled gate for each important business mutation.

## Abstraction and Contracts

The application layer uses interfaces to abstract behavior from implementation.

Contract examples:
- repositories: `IUserRepository`, `IProductRepository`, `IOrderRepository`
- services: `IAuthService`, `IOrderService`, `IReviewService`, etc.
- extension seam: `IReportExporter`

Benefits:
- implementations can change without changing callers
- tests can isolate behavior more easily
- architecture boundaries stay explicit

## Inheritance and Polymorphism (Used Carefully)

Inheritance exists where domain meaning is strong:
- `User` (abstract) -> `Customer`, `Administrator`

Polymorphism examples:
- role-based behavior via `UserRole` and derived user type checks in menus
- repository polymorphism through interfaces (could swap implementation)
- exporter polymorphism through `IReportExporter`

Why not heavy inheritance everywhere:
- deep inheritance hierarchies increase fragility
- composition keeps responsibilities more explicit

## Composition Over Inheritance

Composition-heavy design in practice:
- `Customer` contains `Cart`, order history, and review history
- `Order` contains item snapshots and payment
- `Product` contains its reviews

Benefits:
- local reasoning
- easier replacement and testing
- fewer inheritance-related side effects

## Guard Clauses and Invariants

Guard clauses are used in constructors and mutators to fail fast.

Invariant examples:
- no negative product price or stock
- review rating always between 1 and 5
- order must have at least one item
- payment amount must be > 0
- user and entity IDs must not be empty GUIDs

Design result:
- impossible or unsafe states are blocked at source.

## Why `sealed`, `abstract`, and `static` Were Chosen

`abstract`:
- used where base type should not be instantiated (`User`)

`sealed`:
- used on most concrete classes to prevent accidental inheritance drift

`static`:
- used for stateless helpers (`ConsoleInputHelper`, `ConsoleTheme`, `MenuActionHelper`, etc.)

Rationale:
- class shape communicates intent and controls extensibility.

## Separation of Concerns Through OOP Boundaries

Presentation concerns:
- prompting, selection, rendering, friendly errors

Application concerns:
- workflow orchestration, cross-entity coordination, query aggregation

Domain concerns:
- entity correctness and local business behavior

Infrastructure concerns:
- persistence/export mechanics and mapping

This separation is the practical reason the architecture feels "clean" despite being a console app.

## LINQ Usage and Query Thinking

LINQ is heavily used in application/infrastructure for expressive query logic.

Where LINQ appears and why:
- `ProductService.SearchProducts`: filter + sort catalog quickly
- `ReportService`: group/aggregate revenue, status counts, and best sellers
- `ReviewService`: compute purchased-only review eligibility
- `InsightsService`: recommendation ranking and insight summarization

How to explain in viva:
"LINQ lets us express domain questions directly: What sold most? What is low stock? What did this customer purchase? The query shape mirrors the business question."

## Persistence Model Separation (OOP Boundary Hygiene)

Domain types are not serialized directly as repository schema contracts.

Instead:
- infrastructure uses `*Record` types for JSON transport
- repositories map between domain entities and record models

Why this is strong OOP design:
- domain model remains business-focused
- persistence model can evolve with less domain impact
- mapping logic is explicit and testable

## Session Context as Application State Boundary

`SessionContext` centralizes authenticated runtime user state.

Benefits:
- avoids passing user references manually through many methods
- keeps sign-in/sign-out behavior explicit
- prevents ad-hoc static global state

## How OOP Choices Improve Testability

Testability gains from design:
- rich domain invariants can be tested in isolation
- services can be tested without console interaction
- persistence can be tested with temporary directories
- presentation helpers can be tested with input/output harness

Current result:
- broad workflow coverage with fast unit-style tests

## What This OOP Design Enables Next

Low-risk next steps:
- introduce factory abstractions for role/menu creation
- introduce strategy variants for payment/export/reporting
- extract state-style order transition policies
- replace JSON repositories with DB-backed repositories behind same contracts

Because boundaries are already strict, these upgrades are incremental rather than rewrite-driven.

## Study Drills (Use This to Internalize)

Drill 1:
- pick one entity and list its invariants, mutators, and why setters are restricted.

Drill 2:
- trace one workflow (`Checkout`) across layers and state which layer owns each step.

Drill 3:
- explain one pattern from code in 30 seconds (Repository, Service Layer, Data Mapper).

Drill 4:
- justify why this is DDD-inspired but not full tactical DDD.

## 30-Second OOP Defense Script

"OOP in this project is used to protect state and keep responsibilities clear. Entities encapsulate invariants with guard clauses, services orchestrate use cases through interfaces, and infrastructure handles technical concerns via mappers and adapters. Access modifiers and class shapes are intentional to keep mutation safe and evolution low-risk."
