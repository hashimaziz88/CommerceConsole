# Test Plan and Quality Strategy

## Purpose

This document explains how testing is structured, what behaviors are covered, how to run tests, and how to extend test quality safely as the project evolves.

## Testing Philosophy

CommerceConsole testing is architecture-aligned:
- domain behavior tested in isolation
- service workflows tested as business units
- persistence/export adapters tested separately
- presentation helpers tested with console harnesses

Principle:
- test the right concern at the right layer.

## Current Test Project Structure

Project:
- `Tests/CommerceConsole.Tests/CommerceConsole.Tests.csproj`

Folders:
- `Domain`
- `Application`
- `Infrastructure`
- `Presentation`

Why this split:
- mirrors production architecture
- makes coverage gaps obvious

## Current Status Snapshot

Latest local run (March 8, 2026):
- Total tests: `61`
- Passed: `61`
- Failed: `0`
- Skipped: `0`

## Coverage by Capability

## Domain invariants

Representative checks:
- invalid product values rejected
- entity constructor guard behavior

## Authentication and session

Covers:
- registration success
- duplicate email rejection
- invalid email rejection
- customer/admin login success paths
- wrong-password rejection
- sign-in/sign-out state behavior

## Catalog and inventory

Covers:
- add/update/delete/restock workflows
- search and active filtering
- low-stock threshold behavior

## Cart and wallet

Covers:
- add/update/remove cart mutations
- stock validation against requested quantities
- wallet top-up validation and persistence

## Checkout and orders

Covers:
- happy path orchestration
- insufficient funds/stock failure paths
- missing product failure path
- order snapshot correctness

## Order lifecycle transitions

Covers:
- valid sequence transitions
- invalid transition rejection
- terminal-state transition rejection

## Reviews and reporting

Covers:
- purchased-only review rules
- rating validation
- average rating, revenue, status counts, best-seller, low-stock calculations

## Bonus features

Covers:
- recommendation filtering/ranking
- admin insight generation
- report export orchestration and PDF output guards

## Presentation helpers

Covers:
- input retry loops and range validation
- confirmation prompts
- paginated product rendering and index visibility

## Test Data and Isolation Strategy

Persistence-related tests:
- use temporary per-test directories
- avoid shared mutable fixtures

Console tests:
- redirect `Console.In` / `Console.Out`
- synchronize where required to avoid global console collision

Benefit:
- deterministic runs with low flakiness risk

## How to Run

```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```

Recommended pre-demo sequence:
```powershell
dotnet build CommerceConsole.csproj
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```

## Regression Checklist for New Features

When adding/changing behavior:
1. add happy-path test
2. add at least one failure/guard test
3. update docs reflecting rule changes
4. verify no existing tests regress
5. include persistence/assertion updates if state model changed

## Current Gaps and Planned Improvements

Known gaps:
- no full interactive end-to-end scripted console session tests
- no cross-process file contention simulation
- no multi-page PDF fidelity tests

Planned additions:
- pattern-focused tests during Submission 2 refactor
- exporter-variant tests if CSV/other exporters added
- deeper audit-log tests if event logging bonus is expanded

## Quick Viva Script

"Tests are organized by architecture layer so each concern is validated at the right level. We cover core happy paths and failure paths for auth, catalog, cart, checkout, lifecycle, reviews, reporting, persistence, and presentation helpers, with deterministic isolation for file and console tests."
