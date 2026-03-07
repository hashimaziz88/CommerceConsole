# OOP Design Notes

## Purpose

Explain key OOP decisions in the current codebase so implementation choices are easy to justify in reviews, demos, and technical discussions.

## Core Principles Applied

### Encapsulation

- Entity state changes are controlled through methods and guarded setters.
- Examples:
- `Product.UpdateDetails(...)`, `Product.Restock(...)`, `Product.ReduceStock(...)`
- `Customer.AddFunds(...)`, `Customer.DebitFunds(...)`
- `Cart.AddItem(...)`, `Cart.UpdateQuantity(...)`

Why this matters:
- invalid state is blocked at the domain boundary
- call sites cannot bypass business guards accidentally

### Abstraction

- Service interfaces define behavior contracts without exposing implementation details.
- Repository interfaces abstract storage mechanics away from application logic.

Examples:
- `IAuthService`, `IOrderService`, `IWalletService`
- `IRepository<T>`, `IUserRepository`, `IProductRepository`, `IOrderRepository`

### Inheritance

- `User` is an abstract base type.
- `Customer` and `Administrator` extend `User` with role-specific identity.

Rationale:
- these are true "is-a" relationships
- shared identity/auth behavior is centralized

### Polymorphism

- Application code depends on interface contracts rather than concrete classes.
- Repository/service implementations can be swapped (for tests or later design patterns).

Current practical polymorphism:
- `AuthService` depends on `IUserRepository`
- `ReportService` depends on `IOrderRepository`

## Access Modifier Choices

- `public` on contracts and externally consumed types
- `private` for internal fields and helper methods
- `protected` constructor in `User` to enforce inheritance semantics
- `private set` on entity properties to prevent uncontrolled writes

## Static vs Instance Decisions

Static type:
- `SeedData` is static because it is stateless bootstrap orchestration.

Instance types:
- repositories/services are instance-based to support composition and test substitution.

## Separation of Concerns (SoC)

- Presentation handles interaction only.
- Application coordinates use cases.
- Domain enforces rules/invariants.
- Infrastructure handles persistence and technical concerns.

This separation reduces coupling and makes each layer simpler to reason about.

## Composition Over Inheritance

Composition is used where appropriate:
- `Customer` owns a `Cart`
- `Order` owns `OrderItem` snapshots and a `Payment`
- `Product` owns review collection

Why it matters:
- improves flexibility
- avoids deep inheritance hierarchies

## Where This Supports Future Pattern Work

Current structure is ready for pattern introduction without rewrite:
- Factory for role-based creation
- Strategy for payment/report variants
- State-style order transition policy

These can be layered on top of existing contracts.
