# OOP Design Notes

## Purpose

This document explains OOP choices after the Submission 2 pattern refactor and how they improve maintainability without changing baseline behavior.

## OOP Goals In This Codebase

1. protect invariants at state owners
2. keep orchestration readable and testable
3. isolate algorithms and creation logic behind contracts
4. reduce switch-heavy branching as complexity grows

## Encapsulation (Still Core)

State changes remain method-driven:
- `Customer.AddFunds` / `DebitFunds`
- `Product.Restock` / `ReduceStock`
- `Cart` mutation methods

Public writable state is still restricted (`private set` / getter-only).

## Abstraction and Polymorphism Upgrades

Monday refactor strengthened polymorphism via interfaces:
- `IPaymentStrategy` -> `WalletPaymentStrategy`
- `IOrderTransitionState` -> status-specific state policies
- `IUserWorkspace` + `IRoleMenuFactory`
- `IMenuCommand` command objects for dispatch
- `ISpecification<T>` for reusable query rules

This moves behavior variation from conditionals into replaceable objects.

## Composition Over Conditionals

Before:
- menu switch blocks for dispatch
- role switch in main menu
- static transition map in `OrderService`
- inline filtering predicates repeated in services

After:
- command map dispatch
- role/menu factory resolution
- state-policy objects
- specification-based filtering

## Access Modifier and Class Shape Reasoning

- interfaces remain `public` contracts at architecture boundaries
- concrete implementations are generally `sealed`
- dependencies remain `private readonly`
- static utilities remain in presentation helpers

New pattern classes follow same rule set to keep consistency.

## Specification Pattern and OOP Value

Domain now holds `ISpecification<T>` contracts and product specs.

Why this is good OOP:
- business filter rules are first-class objects
- composition (`AndSpecification`, `OrSpecification`, `NotSpecification`) avoids duplicated predicates
- repository `Find(spec)` enables contract-driven querying

## Strategy and State OOP Value

Strategy:
- payment algorithm extracted from checkout orchestration

State-style:
- lifecycle transition rules represented as policy objects

Combined effect:
- `OrderService` is simpler and coordinates rather than owning all branching details.

## Command + Factory OOP Value In Presentation

Command:
- menu actions become objects, improving dispatch clarity

Factory:
- role routing now resolves workspace by contract

This keeps presentation extensible while preserving thin-menu principles.

## Viva 30-Second Script

"The refactor applies OOP by pushing variation points into contracts and concrete classes: Strategy for payment, State-style policies for transitions, Factory for role workspace resolution, Command for menu dispatch, and Specification for reusable query rules. State and invariants stay protected while orchestration becomes cleaner and easier to extend."
