# Access Modifiers and Class Design

## Purpose

This guide explains why access modifiers and class-shape choices were made in CommerceConsole, and how they protect business invariants and maintain architecture boundaries.

## Access Modifier Strategy in This Codebase

## `public`

Used for:
- interfaces and service contracts
- menu entry methods
- domain methods that intentionally expose business actions

Reasoning:
- only publish what other layers are meant to call.

Example:
- `OrderService.Checkout(...)` is public because presentation needs that use case.

## `private`

Used for:
- helper methods
- internal collections and implementation details

Reasoning:
- prevent accidental use of internal behavior from outside class boundaries.

Example:
- `OrderService.GetOrderOrThrow(...)` is private because it is an internal service detail.

## `private readonly`

Used for:
- constructor-injected dependencies

Reasoning:
- dependencies are fixed after construction
- prevents hidden reassignment bugs
- communicates immutable object graph wiring

Example:
- `_orderRepository`, `_productRepository`, `_userRepository` in `OrderService`.

## `private set`

Used for:
- mutable properties that should only change through validated behavior methods

Reasoning:
- blocks arbitrary external mutation
- funnels updates through invariant checks

Examples:
- `Product.Price`, `Product.StockQuantity`
- `Customer.WalletBalance`
- `Payment.Status`

## getter-only (`{ get; }`)

Used for:
- identity and immutable snapshot data

Reasoning:
- identity should not be reassigned after creation.

Examples:
- `Order.Id`, `OrderItem.ProductName`, `User.Id`.

## `protected`

Used for:
- abstract base constructor in `User`

Reasoning:
- allows derived classes to initialize shared state
- prevents invalid direct instantiation of base abstraction

## `internal`

Current usage:
- minimal in this codebase

Design note:
- `internal` would be appropriate for assembly-scoped helpers if needed later.

## Class-Shape Decisions and Why

## `abstract` classes

Used:
- `User`

Why:
- models a shared concept that should not exist as a standalone runtime instance.

## `sealed` classes

Used:
- most concrete services/entities/menus

Why:
- prevents uncontrolled inheritance extension
- reduces risk of invariant bypass via subclass overrides
- keeps behavior stable in coursework scope

## `static` classes

Used:
- presentation helpers (`ConsoleTheme`, `ConsoleInputHelper`, display/render helpers)
- seed helper (`SeedData`)

Why:
- no instance state required
- utility semantics are explicit

## record models

Used:
- application read/report DTO-like models (`SalesReportSnapshot`, `ProductSalesReportItem`, etc.)

Why:
- immutable/value-centric data carriers for aggregation/export/display

## Specialized Method Usage (Why methods, not direct writes)

Examples:
- `Customer.AddFunds` and `Customer.DebitFunds`
- `Product.Restock`, `Product.ReduceStock`, `Product.UpdateDetails`
- `Cart.AddItem`, `Cart.UpdateQuantity`, `Cart.Clear`

Why this matters:
- method names describe intent
- each method is a policy gate for validation
- keeps invariants close to state owner

## Encapsulation and Invariant Protection

Protection model:
1. constructor guards reject invalid object creation
2. mutator method guards reject invalid transitions
3. restricted setters block bypass edits
4. services orchestrate, domain validates

Outcome:
- invalid states fail early and predictably.

## Composition vs Inheritance in This Project

Inheritance used narrowly:
- `Customer : User`
- `Administrator : User`

Composition preferred broadly:
- `Customer` owns `Cart`, `Orders`, `Reviews`
- `Order` owns `OrderItem` snapshots and `Payment`
- `Product` owns `Reviews`

Reasoning:
- composition avoids fragile deep hierarchies.

## LINQ and Method-Level Readability

LINQ is used where query intent is clearer than manual loops:
- search/filter (`ProductService`, repositories)
- status and revenue aggregation (`ReportService`)
- review eligibility and recommendation ranking (`ReviewService`, `InsightsService`)

Why:
- concise expression of filter/group/aggregate intent
- easier to reason about in viva when tied to business questions

## Practical Design Rules You Can Apply in Future Projects

When adding a property:
1. should external code set this directly?
2. if no, use `private set` + behavior method.

When adding a class:
1. should this type be inherited? if no, consider `sealed`.
2. does it hold no state? if yes, consider `static`.

When adding a workflow:
1. does it cross entities/repositories? put it in application service.
2. does it protect entity state? put it in domain method.

## Viva-Ready 30-Second Script

"Access modifiers are used to protect invariants and reduce coupling. Public APIs are intentional, mutable state is controlled with private setters and domain methods, dependencies are private readonly for stable wiring, and class shapes (abstract/sealed/static) communicate design intent clearly. This keeps behavior safe, testable, and maintainable."
