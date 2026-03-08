# Test Plan and Quality Strategy

## Purpose

This plan tracks quality coverage after Submission 2 pattern refactor and ensures behavior parity while extension seams evolve.

## Current Status Snapshot

Latest local run:
- Total tests: `75`
- Passed: `75`
- Failed: `0`
- Skipped: `0`

## Coverage Structure

Test folders mirror architecture:
- `Domain`
- `Application`
- `Infrastructure`
- `Presentation`

## Core Workflow Coverage (unchanged scope)

Covered areas:
- authentication and role routing
- product catalog operations
- cart and wallet behavior
- checkout and order persistence effects
- order status transition validation
- review eligibility and reporting aggregates
- JSON persistence and export behavior
- presentation input and rendering helpers

## Pattern-Focused Coverage Added

## Factory

- role menu factory resolves known role workspace
- unknown roles return no workspace

## Strategy

- wallet payment strategy success path
- wallet payment strategy insufficient funds rejection
- checkout parity still validated through `OrderService` tests

## State-style transitions

- transition state factory resolution for each order status
- existing valid/invalid/terminal transition tests remain green through `OrderService`

## Specification + repository formalization

- repository `Find(spec)` behavior tests (products/users)
- service outputs still validated through product/report/application tests

## Command

- command dispatcher executes mapped command
- command-based logout flow behavior validation

## How To Run

```powershell
dotnet test Tests\CommerceConsole.Tests\CommerceConsole.Tests.csproj
```

## Regression Requirements

For every pattern refactor step:
1. existing behavior tests remain green
2. new abstraction has direct tests
3. no presentation-layer repository usage introduced
4. no GUID user-facing regression introduced

## Current Known Gaps

Still not covered end-to-end:
- full interactive console session script replay
- multi-process file contention scenarios

These remain acceptable for current coursework scope.

## Quick Viva Script

"We preserved baseline workflow coverage and added dedicated tests for each introduced pattern seam: Factory, Strategy, State-style transitions, Specification-backed repository querying, and Command dispatch. This proves refactor correctness without relying on manual testing."
