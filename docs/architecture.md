# Architecture Deep Dive

## Purpose

This document explains CommerceConsole architecture and how the Monday Submission 2 refactor changed extension points while preserving behavior.

## Architecture Identity

CommerceConsole remains:
- Layered architecture
- DDD-inspired (rich domain + invariants)
- not full tactical DDD

Layer boundaries are unchanged:
- Presentation -> Application -> Domain
- Infrastructure implements application contracts and persists data

## Monday Refactor Delta (What Changed)

The Monday work formalized five missing pattern areas:
1. Factory for role/workspace routing
2. Strategy for payment processing (wallet)
3. State-style transition policies for order lifecycle
4. Command for menu action dispatch
5. Specification pattern for reusable query rules

What did not change:
- business scope
- persistence format contracts (JSON files)
- no-GUID user-facing UX rule
- service/domain responsibility split

## Current Layer Responsibilities

## Presentation

Now contains:
- menus
- render/input helpers
- command abstractions (`IMenuCommand`, dispatcher)
- workspace abstractions and role factory

Still must not contain:
- repository access
- business-policy logic

## Application

Now contains:
- service orchestration
- payment strategy contracts/implementation
- order transition state contracts/factory

Still must not contain:
- console I/O
- low-level file persistence

## Domain

Now contains:
- entities, enums, exceptions
- specification contracts and product specifications

Still must not contain:
- repository implementation
- presentation concerns

## Infrastructure

Now contains:
- repository implementations including `Find(spec)` support
- JSON persistence utilities
- data mapping and exporter adapter

## Composition Root

`Program.cs` now wires:
- repositories
- services
- payment strategy
- transition state factory
- role/menu factory
- menus and main menu

Benefit:
- new abstractions are still assembled in one place.

## Pattern-Driven Flow Examples

## Login and role routing

- `MainMenu` authenticates user
- role resolution uses `IRoleMenuFactory`
- returned `IUserWorkspace` runs role-specific menu

## Checkout

- `OrderService` validates cart and stock
- payment processing delegated to `IPaymentStrategy`
- status transition rules resolved from state policy objects
- order persisted and cart cleared

## Catalog/report filtering

- service query logic uses repository `Find(spec)` with domain specifications

## Why this architecture is now more robust

- less switch-heavy routing/dispatch logic
- cleaner algorithm variation points
- reusable, composable filtering rules
- transition policy classes easier to extend and test

## Trade-offs introduced

- more abstractions/classes to maintain
- slightly larger wiring surface in composition root

These trade-offs are intentional for extensibility and architecture marks.

## Guardrails still enforced

- menus do not call repositories
- business rules stay in domain/application
- no internal GUID exposure in UI
- docs and tests updated alongside behavior refactors

## Quick Viva Script

"The Monday refactor preserved layered boundaries but formalized key extension seams. Role routing is factory-driven, payment is strategy-driven, lifecycle transitions are state-policy-driven, menu dispatch is command-driven, and filtering is specification-driven. Behavior remained stable while extensibility improved significantly."
