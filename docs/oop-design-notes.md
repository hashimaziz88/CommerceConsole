# OOP Design Starter Notes

## Access modifier choices

- `public` for service contracts, entities, and menu entry points consumed across layers.
- `private` for internal state and helper implementation details.
- `protected` constructor in `User` to enforce inheritance-only creation.
- `private set` on mutable entity properties to protect invariants.

## Static choices

- `SeedData` is static because it represents a stateless bootstrap concern.
- Most services/repositories are instance-based to support dependency injection and testability.

## Polymorphism choices

- `User` is abstract with `Customer` and `Administrator` subclasses.
- Repository interfaces (`IRepository<T>`, specialized repos) enable implementation polymorphism.
- Service interfaces enable swapping implementations during testing and pattern upgrades.

## Separation of concerns (SoC)

- Menus handle user interaction only.
- Services coordinate use cases.
- Entities enforce business rules/invariants.
- Repositories isolate storage operations.

## Inheritance choices

- Inheritance is used only where there is a strong "is-a" relationship (`Customer : User`, `Administrator : User`).
- Composition is preferred elsewhere (for example, `Customer` owns `Cart` and order history).
