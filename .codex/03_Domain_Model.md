# 03. Domain Model

## Goal

Capture business entities, invariants, and domain-level query specifications after Submission 2 refactor.

## Entity model (unchanged core)

- `User` (abstract)
- `Customer`
- `Administrator`
- `Product`
- `Cart`
- `CartItem`
- `Order`
- `OrderItem`
- `Payment`
- `Review`

## Core invariants

- Product price/stock cannot be negative.
- Cart quantities must be positive on insert/update.
- Wallet debit/top-up must be valid.
- Review rating must be 1..5.
- Order requires at least one item.

## Domain specifications (new)

Added reusable domain query contracts and product specs:
- `ISpecification<T>`
- `AndSpecification<T>`
- `OrSpecification<T>`
- `NotSpecification<T>`
- `ActiveProductSpecification`
- `SearchProductSpecification`
- `LowStockProductSpecification`
- `InStockProductSpecification`

Purpose:
- formalize query rules as reusable domain objects
- reduce duplicated inline filtering logic in services
- support repository `Find(spec)` formalization

## Exceptions

Domain exceptions remain:
- `ValidationException`
- `NotFoundException`
- `AuthenticationException`
- `InsufficientFundsException`
- `InsufficientStockException`
- `DuplicateEmailException`

## Design note

This remains DDD-inspired, not full tactical DDD.
Refactor improved extension seams without changing business scope.
