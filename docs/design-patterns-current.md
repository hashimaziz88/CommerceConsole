# Current Design Patterns in Code

## Purpose

This document lists the design patterns that are already implemented in the current codebase and explains their practical use.

It intentionally excludes planned future refactors (for example explicit Factory/Strategy/State work planned for the Monday pattern phase).

## Pattern Inventory

| Pattern | How it appears in this project | Key locations | Why it is used |
|---|---|---|---|
| Repository Pattern | Interfaces define data contracts and concrete repositories implement persistence details | `Application/Interfaces/IRepository.cs`, `IUserRepository.cs`, `IProductRepository.cs`, `IOrderRepository.cs`; `Infrastructure/Repositories/InMemory*Repository.cs` | decouples business logic from storage, supports testability and future persistence swaps |
| Service Layer Pattern | Application services coordinate use cases and keep workflows out of menus | `Application/Services/AuthService.cs`, `ProductService.cs`, `CartService.cs`, `WalletService.cs`, `ReportService.cs` | centralizes business workflows and policy logic |
| Dependency Injection (Constructor Injection) | Services/menus receive dependencies through constructors | constructors across `Application/Services/*` and `Presentation/Menus/*` | improves loose coupling and makes collaborators explicit |
| Composition Root | One startup point wires repositories, services, and menus | `Program.cs` | keeps object graph construction in one place and avoids ad-hoc service creation across layers |
| Data Mapper Pattern | Repositories map between persistence records and domain entities with dedicated mapping methods | `InMemoryUserRepository.ToDomain/FromDomain`, `InMemoryProductRepository.ToDomain/FromDomain`, `InMemoryOrderRepository.ToDomain/FromDomain` | isolates JSON schema from domain model, reducing persistence leakage |
| Rich Domain Model (Entity Behavior) | Entities own invariants and state transitions through methods, not public setters | `Domain/Entities/Product.cs`, `Customer.cs`, `Cart.cs`, `Payment.cs`, `Order.cs` | protects domain integrity and keeps rules close to the data they govern |
| Guard Clause Pattern | Constructors and methods fail fast on invalid input/state | constructors and mutator methods in `Domain/Entities/*`; validation checks in services | provides predictable error flow and prevents invalid state propagation |
| Session Context Pattern | A dedicated object stores authenticated user state for the running process | `Application/Services/SessionContext.cs`, `Application/Interfaces/ISessionContext.cs` | avoids passing auth state manually through every call and keeps session responsibility focused |
| Command-Query Separation (style) | Write operations and read operations are separated by intent and naming | command examples: `AddToCart`, `RestockProduct`, `AddFunds`; query examples: `GetActiveProducts`, `GetCartItems`, `GetOrdersByStatus` | improves readability and reduces accidental side effects |
| Idempotent Seed/Bootstrap Pattern | Startup seed checks existing data before inserting defaults | `Infrastructure/Data/SeedData.cs` | safe repeated startup without duplicate admin/products |

## Pattern Use by Layer

### Presentation
- Uses service-layer APIs instead of repositories directly.
- Keeps flow orchestration and exception boundary handling only.

### Application
- Implements Service Layer with constructor-injected repository abstractions.
- Encapsulates query ordering/filtering and workflow logic.

### Domain
- Implements rich entities with guard clauses and controlled mutation.

### Infrastructure
- Implements Repository + Data Mapper over JSON file persistence.

## What Is Not Yet Implemented (By Design)

The following are planned but not yet formalized as explicit pattern modules:
- Factory Pattern
- Strategy Pattern
- State-style order transition policy

Current code is intentionally structured so these can be added without rewriting working features.

## Quick Explanation Script (for demo/viva)

If asked "What patterns are currently in your code?":
- We already use Repository + Service Layer + constructor-based DI.
- We use Data Mapper in repositories to separate domain from JSON schema.
- Domain entities are rich models with guard clauses, so invariants are enforced at source.
- `Program.cs` acts as the composition root and `SessionContext` handles runtime auth state.
