# Persistence Model (JSON)

## Purpose

This document explains how data persistence works in CommerceConsole, why JSON was chosen, and what consistency guarantees and limitations currently exist.

## Persistence Strategy Overview

CommerceConsole uses JSON file persistence through repository adapters.

Design goals:
- keep setup simple for coursework and evaluation
- preserve state across restarts
- keep persistence behind interfaces so it can be swapped later

## Persisted Runtime Files

Default folder:
- `./data`

Files:
- `users.json` (users, wallet balances, cart snapshots)
- `products.json` (catalog, stock, reviews)
- `orders.json` (orders, order items, payment status)

## Ownership by Repository

- `InMemoryUserRepository` -> `users.json`
- `InMemoryProductRepository` -> `products.json`
- `InMemoryOrderRepository` -> `orders.json`

Each repository loads once at construction and writes-through on mutation.

## Why "InMemory + JSON" Naming Is Accurate

Behavior model:
- live state is held in in-memory lists during runtime
- mutations persist list snapshots to JSON immediately

So this is:
- in-memory primary working set
- file-backed durability layer

## Record Model Separation

Persistence models are separate from domain entities:
- `UserRecord`, `ProductRecord`, `OrderRecord`, etc.

Why this matters:
- storage schema can evolve without forcing domain redesign
- domain invariants stay in domain, not in serialization DTOs
- repository mapping code stays explicit and testable

## JsonFileStore Behavior

Core utility:
- `Infrastructure/Persistence/JsonFileStore.cs`

Load behavior:
- if file missing -> returns empty list
- if file malformed -> catches `JsonException` and returns empty list

Save behavior:
1. serialize list to JSON
2. write to temp file
3. atomically replace target file

Benefit:
- reduces partial-write risk

## Seeding and Persistence Interaction

`SeedData.Seed(...)` runs on startup after repositories initialize.

Idempotency rules:
- admin seeded only if missing
- products seeded only if missing by product name

Effect:
- safe repeated app launches
- expanded seed catalog can be introduced without duplicate growth

## Consistency Model and Limits

Current consistency assumptions:
- single-process normal usage
- immediate write-through after repository mutation

Limitations:
- no cross-process locking coordination
- no schema versioning/migration pipeline
- no cryptographic protection for sensitive user fields

## Why This Is Still Architecturally Strong

Even with simple JSON storage:
- application/domain logic is not coupled to JSON APIs
- storage can be replaced by new repositories behind same interfaces
- tests can run deterministically with temp data directories

## Practical Data Reset Guidance

For clean environment resets:
1. close app
2. remove `data/users.json`, `data/products.json`, `data/orders.json` (or clear folder)
3. restart app to reseed baseline admin/products

## Future Upgrades (Low-Risk)

1. add password hashing migration path
2. add file-level lock policy or move to DB backend
3. add schema version metadata and migration steps
4. add audit trail records for operational events


