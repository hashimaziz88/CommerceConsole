# OOP Design Notes

## Purpose

This document explains the OOP design decisions in the current `CommerceConsole` implementation so you can justify both architecture and code-level choices during demos, reviews, and viva discussions.

It covers:
- why each major access modifier was chosen
- why classes are `abstract`, `sealed`, or `static`
- specialized method patterns used in domain/application/infrastructure
- where LINQ is used, how query chains work, and why those operators were selected

## Architecture Context

The project follows layered separation:
- Presentation: menus + console helpers (`Presentation/*`)
- Application: use-case orchestration via interfaces/services (`Application/*`)
- Domain: entities, invariants, exceptions (`Domain/*`)
- Infrastructure: repositories + JSON persistence (`Infrastructure/*`)

OOP choices were made to keep each layer cohesive and minimize cross-layer leakage.

## Current Design Patterns Already Present

The codebase already includes concrete pattern usage (not just planned refactors):
- Repository Pattern (`I*Repository` with `InMemory*Repository` implementations)
- Service Layer Pattern (`Application/Services/*`)
- Constructor-based Dependency Injection
- Composition Root in `Program.cs`
- Data Mapper methods (`ToDomain` / `FromDomain`) in repositories
- Rich Domain Model with guard clauses in entities
- Session Context pattern for process-level authenticated user state

For the full pattern inventory and usage mapping, see:
- `docs/design-patterns-current.md`

## Core OOP Pillars Applied

### Encapsulation

Domain state transitions are restricted to explicit methods with guard clauses.

Examples:
- `Product.UpdateDetails(...)`, `Product.Restock(...)`, `Product.ReduceStock(...)`
- `Customer.AddFunds(...)`, `Customer.DebitFunds(...)`
- `Cart.AddItem(...)`, `Cart.UpdateQuantity(...)`, `Cart.RemoveItem(...)`
- `Payment.MarkCompleted()`, `Payment.MarkFailed()`

Impact:
- invalid writes are blocked at the entity boundary
- service and menu code cannot mutate critical fields directly

### Abstraction

Service/repository interfaces expose capabilities without leaking implementation details.

Examples:
- service contracts: `IAuthService`, `IProductService`, `ICartService`, `IWalletService`
- repository contracts: `IRepository<T>`, `IUserRepository`, `IProductRepository`, `IOrderRepository`

Impact:
- use cases depend on behavior, not storage mechanics
- implementations can change with low ripple (e.g., in-memory to database)

### Inheritance

`User` is an abstract base type for shared identity/authentication data.

Concrete types:
- `Customer : User`
- `Administrator : User`

Why inheritance is appropriate here:
- these are true "is-a" relationships
- shared fields/behavior are centralized in one base type
- role-specific behavior/data stays in concrete classes

### Polymorphism

The application layer consumes interfaces and base types, enabling substitution.

Examples:
- `AuthService` depends on `IUserRepository`
- `ProductService` depends on `IProductRepository`
- `MainMenu` routes by `UserRole` using `ISessionContext`

Impact:
- clearer boundaries
- easier testing
- easier refactoring for Submission 2 patterns

## Access Modifier Decisions and Reasoning

### Access modifier strategy summary

| Modifier | Where used | Why it was chosen |
|---|---|---|
| `public` | interfaces, entities, services, menu entry methods | marks the intentional API surface across layers |
| `private` | dependency fields, helper methods, backing lists | hides implementation details and protects invariants |
| `protected` | `User` constructor | allows inheritance while preventing direct `User` instantiation |
| `private set` | mutable domain properties (`WalletBalance`, `Status`, etc.) | allows controlled mutation only through validated methods |
| getter-only (`{ get; }`) | IDs, snapshots, immutable references | prevents accidental reassignment after construction |

### Why `public` is limited to intentional API surface

Public members are used where callers in other layers need access.

Examples:
- service methods consumed by menus: `CartService.AddToCart`, `WalletService.AddFunds`
- domain behavior consumed by services: `Product.Restock`, `Customer.DebitFunds`

Reasoning:
- keeps contract clear
- reduces accidental coupling

### Why dependencies are `private readonly`

Services and menus use private readonly fields for injected collaborators.

Examples:
- `_productRepository` in `ProductService`
- `_productService` in `AdminMenu`

Reasoning:
- dependency cannot be changed after construction
- improves predictability and testability

### Why domain properties avoid public setters

Examples:
- `Product.Price { get; private set; }`
- `Customer.WalletBalance { get; private set; }`
- `Payment.Status { get; private set; }`

Reasoning:
- all changes must pass through methods that enforce rules
- prevents illegal state transitions (negative stock, invalid wallet flows, etc.)

### Why only `User` constructor is `protected`

`User` is abstract and not a valid standalone runtime type.

Reasoning:
- only derived user roles should construct base identity data
- guarantees a valid `UserRole` is provided by concrete constructors

## Class-Level Specialization Choices

### `abstract` for `User`

Reasoning:
- base aggregation of common identity + auth behavior (`VerifyPassword`)
- prevents meaningless direct instantiation

### `sealed` for most concrete classes

Used on concrete entities/services/exceptions/menus.

Reasoning:
- prevents unplanned inheritance chains
- keeps behavior stable and explicit
- avoids subclass-based bypass of business rules

### `static` for stateless utilities

Examples:
- `Program` bootstrap entry point
- `SeedData` bootstrap utility
- `ConsoleInputHelper`, `ProductDisplayHelper`, `CartDisplayHelper`

Reasoning:
- no instance state required
- avoids unnecessary object creation
- signals utility intent clearly

## Specialized Method Usage Patterns

### Guard-clause constructors

Most domain constructors validate immediately and fail fast.

Examples:
- empty IDs rejected in `User`, `Product`, `Order`, `Payment`, `Review`
- invalid ranges rejected (e.g., review rating 1..5)

Why:
- ensures every created instance starts valid
- simplifies downstream logic since invalid states are blocked early

### Command methods for state transitions

Methods that change state are named as domain actions.

Examples:
- `Product.Restock`, `Product.ReduceStock`, `Product.Deactivate`
- `Cart.AddItem`, `Cart.UpdateQuantity`, `Cart.Clear`
- `Payment.MarkCompleted`, `Payment.MarkFailed`

Why:
- method names reflect business intent
- state transitions stay explicit and auditable

### Query methods for read intent

Read-only intent uses `Get*` methods.

Examples:
- `GetCartItems`, `GetCartTotal`, `GetActiveProducts`, `GetOrdersByStatus`

Why:
- clear semantic split between reads and writes
- easier to test and reason about side effects

### Or-throw helper methods

Private helper methods centralize existence checks and message consistency.

Examples:
- `ProductService.FindProductOrThrow(...)`
- `CartService.GetProductOrThrow(...)`

Why:
- avoids duplicated null-check logic
- standardizes exception flow

### Mapping methods for persistence boundary

Infrastructure repositories translate between domain entities and JSON record models.

Examples:
- `ToDomain(...)` and `FromDomain(...)` in all in-memory repositories

Why:
- isolates persistence schema from domain behavior
- keeps repository responsibilities focused and explicit

### Session lifecycle methods

`SessionContext` uses small, explicit methods:
- `SignIn(User user)`
- `SignOut()`

Why:
- clear state transitions for authentication lifecycle
- single responsibility for session process state

### Input specialization in presentation utilities

`ConsoleInputHelper` has dedicated methods:
- `ReadRequiredString`
- `ReadDecimal`
- `ReadInt`
- `ReadSelection`

Why:
- centralizes parsing/validation loops
- removes duplicated input logic from menus
- supports secure index-based selection UX

### Compiled regex for email validation

`AuthService` uses:
- `private static readonly Regex BasicEmailPattern`
- options: `Compiled | CultureInvariant`

Why:
- one reusable compiled pattern instance
- avoids repeated regex creation
- keeps validation logic explicit and testable

## LINQ Deep Dive: Where and How Queries Are Used

### LINQ usage principles followed

- use LINQ where query intent is clearer than manual loops
- materialize with `ToList()` when a stable snapshot is needed
- combine filtering + ordering in service layer to keep menus thin
- use aggregation (`Sum`, `Average`, `GroupBy`) for reporting/calculation semantics

### Query map by location

| Location | Query chain / operator | What it does | Why this operator set |
|---|---|---|---|
| `ProductService.GetActiveProducts` | `GetAll().Where(IsActive).OrderBy(Name).ToList()` | filters customer-visible products and sorts alphabetically | clean browse UX + deterministic ordering |
| `ProductService.SearchProducts` | `Search(term).Where(IsActive).OrderBy(Name).ToList()` | applies term search then enforces active-only visibility | reuses repository search + service visibility policy |
| `ProductService.GetAllProducts` | `GetAll().OrderBy(Name).ToList()` | returns full catalog sorted by name | admin view consistency |
| `ProductService.GetLowStockProducts` | `GetLowStockProducts(threshold).OrderBy(Stock).ThenBy(Name).ToList()` | prioritizes most critical low-stock items | meaningful triage ordering |
| `CartService.AddToCart` | `Items.FirstOrDefault(...)? .Quantity ?? 0` | gets existing cart quantity for merge validation | concise null-safe retrieval for stock guard |
| `Cart.AddItem` | `_items.FirstOrDefault(...)` | locate existing line before add/merge | avoids duplicate line items for same product |
| `Cart.UpdateQuantity` | `_items.FirstOrDefault(...)` | find target line for update/remove | single lookup path with not-found handling |
| `Cart.RemoveItem` | `_items.FirstOrDefault(...)` | find optional line before removal | safe no-op behavior when absent |
| `Cart.CalculateTotal` | `_items.Sum(i => i.LineTotal)` | total cart amount | direct aggregate intent |
| `Order` constructor | `items?.ToList() ?? new List<OrderItem>()` | materialize and null-protect incoming sequence | snapshot semantics for order creation |
| `Order` constructor | `orderItems.Sum(i => i.LineTotal)` | compute immutable total | avoids recalculation logic duplication |
| `ReportService.GetTotalRevenue` | `GetAll().Sum(order => order.TotalAmount)` | total revenue across orders | standard aggregate for financial summary |
| `ReportService.GetOrdersByStatus` | `GetAll().GroupBy(Status).ToDictionary(...)` | status distribution map | natural grouping + direct dictionary output |
| `SeedData.Seed` | `GetAll().Any(user => user is Administrator)` | checks whether admin already exists | efficient existence query for idempotent seed |
| `InMemoryProductRepository.Search` | `_products.Where(name/category contains).ToList()` | case-insensitive search by name/category | concise filter expression |
| `InMemoryProductRepository.GetLowStockProducts` | `_products.Where(stock <= threshold).ToList()` | low-stock filtering at repository level | reusable query path |
| `InMemoryOrderRepository.GetByCustomerId` | `_orders.Where(customerId match).ToList()` | customer order history retrieval | straightforward partition query |
| `InMemoryUserRepository.GetById` | `_users.FirstOrDefault(id match)` | single-user fetch | natural first-match query |
| `InMemoryUserRepository.GetByEmail` | `_users.FirstOrDefault(email equals ignore-case)` | login lookup | concise normalized identity match |
| `InMemory*Repository.Load*` | `records.Select(ToDomain).ToList()` | record-to-domain projection | separation of persistence and domain models |
| `InMemory*Repository.Persist` | `entities.Select(FromDomain).ToList()` | domain-to-record projection | explicit serialization boundary |
| `ProductDisplayHelper.ShowProducts` | `products.ToList()` | materialize enumerable once for count + iteration | prevents multiple enumeration |
| `ProductDisplayHelper.CalculateAverageRating` | `Reviews.Average(r => r.Rating)` | average rating display | direct aggregate meaning |
| `ReviewService.GetProductReviews` | `product.Reviews.ToList()` | returns copy snapshot of reviews | prevents exposing mutable backing list directly |

### How query chains are structured in practice

Common pattern in services:
1. get base dataset from repository
2. apply business filter (`Where`)
3. apply business ordering (`OrderBy` / `ThenBy`)
4. materialize (`ToList`) for menu consumption

This keeps query policy centralized in application services rather than scattered in menus.

### Why `ToList()` is used frequently

`ToList()` is intentionally used to:
- force immediate execution at the service/repository boundary
- return stable snapshots to callers
- avoid deferred enumeration surprises in menu loops

### Why some loops are still explicit `for/foreach`

Not every scenario uses LINQ.

Examples:
- numbered menu rendering in `ShowSelectableProducts` and `ShowSelectableCart`

Reasoning:
- index-aware output is clearer with `for` loops
- imperative flow is simpler than composing index projections for console rendering

## Separation of Concerns and Responsibility Allocation

### Presentation

Responsibilities:
- input/output
- menu flow
- exception-to-message conversion

Non-responsibilities:
- stock rules
- wallet rules
- repository query logic

### Application

Responsibilities:
- orchestration across domain + repository
- policy-level validation
- use-case level ordering/filtering

### Domain

Responsibilities:
- invariants
- controlled state mutation
- domain exceptions

### Infrastructure

Responsibilities:
- persistence mechanics
- mapping domain <-> records
- JSON read/write behavior

## Composition vs Inheritance Decisions

### Composition-first (preferred default)

Used extensively:
- `Customer` has `Cart`, `Orders`, `Reviews`
- `Order` has `OrderItem` snapshots and `Payment`
- `Product` has `Reviews`

Reasoning:
- avoids deep class hierarchies
- models real "has-a" relationships
- keeps each type focused

### Inheritance only where domain semantics demand it

Used for:
- `User` base with role-specific subtypes

Reasoning:
- shared identity/auth behavior is genuinely common
- role differences are type-safe

## Exception and Error-Handling Strategy (OOP Perspective)

Custom exception types encode intent and support clean layer boundaries:
- `ValidationException`
- `NotFoundException`
- `AuthenticationException`
- `InsufficientFundsException`
- `InsufficientStockException`
- `DuplicateEmailException`

Pattern:
- domain/application throw typed exceptions
- presentation catches and converts to friendly messages

This keeps domain/application free from console concerns while still producing good UX.

## Trade-offs and Current Limitations

- `OrderService.Checkout(...)` remains a placeholder pending Issue 5.
- Session state is in-memory process state only.
- Repository persistence is single-process oriented (no cross-process locking policy).

These are deliberate scope choices, not accidental OOP gaps.

## Submission-Ready Justification Summary

If asked to justify design quickly:
- access modifiers are chosen to minimize mutable surface area and enforce invariants
- `abstract`/`sealed`/`static` are used intentionally to express role and lifecycle of each type
- specialized methods are intent-driven (`AddFunds`, `Restock`, `FindProductOrThrow`) and keep rules centralized
- LINQ is used in services/repositories for readable filtering/ordering/aggregation, with `ToList()` for deterministic snapshots
- presentation remains thin and identifier-safe, while business logic stays in domain/application


