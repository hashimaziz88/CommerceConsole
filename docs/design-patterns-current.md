# Current Design Patterns in Code

## Purpose

This document lists the design patterns that are already implemented in the current codebase and explains their practical use.

It intentionally distinguishes currently used patterns from larger Monday-focused pattern expansion work.

## Pattern Inventory

| Pattern | How it appears in this project | Key locations | Why it is used |
|---|---|---|---|
| Repository Pattern | Interfaces define data contracts and concrete repositories implement persistence details | `Application/Interfaces/IRepository.cs`, `IUserRepository.cs`, `IProductRepository.cs`, `IOrderRepository.cs`; `Infrastructure/Repositories/InMemory*Repository.cs` | decouples business logic from storage, supports testability and future persistence swaps |
| Service Layer Pattern | Application services coordinate use cases and keep workflows out of menus | `Application/Services/AuthService.cs`, `ProductService.cs`, `CartService.cs`, `WalletService.cs`, `OrderService.cs`, `ReviewService.cs`, `ReportService.cs` | centralizes business workflows and policy logic |
| Dependency Injection (Constructor Injection) | Services/menus receive dependencies through constructors | constructors across `Application/Services/*` and `Presentation/Menus/*` | improves loose coupling and makes collaborators explicit |
| Composition Root | One startup point wires repositories, services, and menus | `Program.cs` | keeps object graph construction in one place and avoids ad-hoc service creation across layers |
| Data Mapper Pattern | Repositories map between persistence records and domain entities with dedicated mapping methods | `InMemoryUserRepository.ToDomain/FromDomain`, `InMemoryProductRepository.ToDomain/FromDomain`, `InMemoryOrderRepository.ToDomain/FromDomain` | isolates JSON schema from domain model, reducing persistence leakage |
| Rich Domain Model (Entity Behavior) | Entities own invariants and state transitions through methods, not public setters | `Domain/Entities/Product.cs`, `Customer.cs`, `Cart.cs`, `Payment.cs`, `Order.cs` | protects domain integrity and keeps rules close to the data they govern |
| Guard Clause Pattern | Constructors and methods fail fast on invalid input/state | constructors and mutator methods in `Domain/Entities/*`; validation checks in services | provides predictable error flow and prevents invalid state propagation |
| Session Context Pattern | A dedicated object stores authenticated user state for the running process | `Application/Services/SessionContext.cs`, `Application/Interfaces/ISessionContext.cs` | avoids passing auth state manually through every call and keeps session responsibility focused |
| Strategy-style Export Seam | Report export behavior is abstracted behind an exporter contract with swappable implementation | `Application/Interfaces/IReportExporter.cs`, `Application/Services/ReportExportService.cs`, `Infrastructure/Export/PdfReportExporter.cs` | enables export-format extension without changing report aggregation services |
| Command-Query Separation (style) | Write operations and read operations are separated by intent and naming | command examples: `AddToCart`, `RestockProduct`, `AddFunds`; query examples: `GetActiveProducts`, `GetCartItems`, `GetOrdersByStatus` | improves readability and reduces accidental side effects |
| Idempotent Seed/Bootstrap Pattern | Startup seed checks existing data before inserting defaults | `Infrastructure/Data/SeedData.cs` | safe repeated startup without duplicate admin/products |

## Pattern Use by Layer

### Presentation
- Uses service-layer APIs instead of repositories directly.
- Keeps flow orchestration and exception boundary handling only.

### Application
- Implements Service Layer with constructor-injected repository abstractions.
- Encapsulates query ordering/filtering and workflow logic.
- Adds exporter and insights abstraction points for extension-friendly bonus features.

### Domain
- Implements rich entities with guard clauses and controlled mutation.

### Infrastructure
- Implements Repository + Data Mapper over JSON file persistence.
- Implements concrete PDF exporter behind application-level interface.

## What Is Still Planned for Monday-focused Expansion

The following are intentionally scoped for further formalization:
- Factory Pattern modules for role-based creation
- broader Strategy variants beyond export seam
- more explicit State-style order transition policy object

## Quick Explanation Script (for demo/viva)

If asked "What patterns are currently in your code?":
- We use Repository + Service Layer + constructor-based DI and a single composition root.
- Repositories use Data Mapper methods so JSON schema stays separate from domain entities.
- Domain entities are rich models with guard clauses to enforce invariants.
- Session state is isolated in `SessionContext`.
- Report exporting uses a strategy-style abstraction (`IReportExporter`) with a concrete PDF implementation.
