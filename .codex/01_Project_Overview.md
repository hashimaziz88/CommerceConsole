# 01. Project Overview

## Project summary

CommerceConsole is a C# console-based Online Shopping Backend System built with clean layering and strong OOP boundaries.

The delivery model is:
- baseline features, tests, and docs implemented from the start
- Monday (2026-03-09) reserved for explicit pattern upgrades only

## Scope summary

### Customer-facing capabilities
- register and login
- browse and search active products
- add to cart, view cart, update quantity, remove by zero quantity
- view wallet balance and top up wallet
- checkout with wallet-only payment
- view order history and track order status
- review purchased products only
- view recommendation suggestions (bonus)

### Administrator-facing capabilities
- add, update, delete, and restock products
- view all products and low-stock products
- view all orders and update order statuses
- view sales reports (revenue, status counts, best sellers, low stock)
- view heuristic smart insights (bonus)
- export sales report PDF (bonus)

## Core non-functional expectations

- clean architecture boundaries (Presentation/Application/Domain/Infrastructure)
- robust validation and custom exception handling
- index-based user selection UX (no GUID entry in user-facing flows)
- JSON persistence for mutable runtime data
- LINQ usage in search, ordering, and reporting
- tests and docs updated continuously with behavior changes

## Core domain types

- `User`
- `Customer : User`
- `Administrator : User`
- `Product`
- `Cart`
- `CartItem`
- `Order`
- `OrderItem`
- `Payment`
- `Review`

## Rubric-aligned implementation strategy

### Baseline phase
1. Build complete required features with quality hardening from day one.
2. Keep `Program.cs` thin and menus focused on routing/display.
3. Centralize business rules in Application and Domain.
4. Keep persistence concerns in Infrastructure only.
5. Keep docs accurate enough for demo/viva explanation.

### Monday pattern phase (2026-03-09)
1. Introduce explicit Factory, Strategy, and State-style modules.
2. Preserve baseline behavior (no feature expansion during pattern refactor).
3. Add pattern-focused tests and architecture deltas in docs.

## Success definition

A successful delivery:
- satisfies all required menu flows and constraints
- avoids architecture leakage across layers
- protects domain invariants through encapsulated methods and guard clauses
- demonstrates maintainability with readable code and clear naming
- provides regression confidence through automated tests
- provides viva-ready explanation through synchronized documentation
