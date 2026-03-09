# Test Plan and Quality Strategy

## Purpose

This document defines the current test strategy, execution status, coverage scope, and known quality limits for CommerceConsole.

## Test Strategy

Tests are organized to mirror production architecture so each concern is validated in the correct layer:
- `Domain`: entity invariants and core rule safety
- `Application`: use-case orchestration and business workflow behavior
- `Infrastructure`: repository contract behavior, JSON persistence, exporter behavior
- `Presentation`: menu command dispatch and input/output helper behavior

Principles:
1. validate domain invariants at source
2. validate service happy-path and failure-path behavior
3. validate persistence behavior independently from UI flow
4. keep presentation tests focused on interaction helpers and dispatch contracts

## Current Status Snapshot

Latest local run (March 9, 2026):
- Command used: `dotnet test Tests\\CommerceConsole.Tests\\CommerceConsole.Tests.csproj --no-build`
- Total: `115`
- Passed: `115`
- Failed: `0`
- Skipped: `0`

## Coverage by Capability

## Domain invariants

Representative checks:
- invalid constructor/mutator inputs are rejected
- domain guard behavior prevents invalid entity state

## Authentication and session

Covered behavior:
- customer registration success
- duplicate email rejection
- invalid credential rejection
- customer/admin login success
- sign-in/sign-out session handling

## Catalog and inventory

Covered behavior:
- add/update/delete/restock workflows
- active catalog filtering and search behavior
- low-stock query behavior

## Cart and wallet

Covered behavior:
- add/update/remove cart operations
- quantity validation against stock constraints
- wallet top-up validation and persistence

## Checkout and orders

Covered behavior:
- checkout success path
- insufficient funds and insufficient stock failure paths
- missing-product validation path
- order snapshot correctness

## Order lifecycle transitions

Covered behavior:
- valid status transitions
- invalid transition rejection
- terminal-state transition protection

## Reviews and reporting

Covered behavior:
- purchased-only review eligibility
- rating boundary validation
- report aggregates (revenue, status counts, best-sellers, low stock)

## Bonus features

Covered behavior:
- recommendation filtering and ranking
- admin insights generation
- PDF export orchestration and file creation guards

## Pattern-focused coverage

Repository Pattern:
- `Infrastructure/RepositoryContractTests.cs`
- `Infrastructure/JsonPersistenceTests.cs`

Strategy Pattern:
- `Application/WalletPaymentStrategyTests.cs`
- checkout integration in `Application/OrderServiceTests.cs`
- export behavior in `Infrastructure/PdfReportExporterTests.cs`

Factory Pattern:
- `Presentation/RoleWorkspaceFactoryTests.cs`

Command Pattern:
- `Presentation/MenuCommandTests.cs`

## Test Isolation and Determinism

Persistence-related tests:
- use temporary directories and isolated files
- avoid shared mutable fixtures

Console-related tests:
- use console input/output redirection harness
- avoid cross-test console interference

## Execution Commands

```powershell
dotnet build CommerceConsole.csproj
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```

If the app executable is currently running and locking rebuild output, execute:

```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj --no-build
```

## Regression Checklist for Any Change

1. add/update tests for the changed behavior
2. include at least one guard/failure-path assertion
3. confirm persistence behavior if mutable state changed
4. confirm no presentation-to-repository direct coupling was introduced
5. update formal docs when behavior or architecture contracts change
6. run full test suite and confirm zero regression

## Known Limits (Current)

1. No full interactive end-to-end console scenario tests.
2. No cross-process JSON contention simulation tests.
3. No visual fidelity assertions for multi-page PDF formatting.

These limits are accepted for current coursework scope and do not affect core functional or architectural verification.

