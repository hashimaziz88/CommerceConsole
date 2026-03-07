# Test Plan

## Goal

Ensure core authentication, domain safety checks, and persistence behavior are correct and stable while features are incrementally added.

## Test Projects and Structure

Current test project:
- `Tests/CommerceConsole.Tests/CommerceConsole.Tests.csproj`

Current test folders:
- `Tests/CommerceConsole.Tests/Domain`
- `Tests/CommerceConsole.Tests/Application`
- `Tests/CommerceConsole.Tests/Infrastructure`

## Current Coverage Summary

### Domain tests
- product negative price validation

### Application tests
- registration success
- duplicate email rejection
- invalid email rejection
- customer login success
- seeded admin login success
- wrong-password rejection
- session sign-in/sign-out state behavior

### Infrastructure tests
- seed persistence to JSON files
- idempotent reseeding behavior
- persisted user reload across repository instances

## Test Data Strategy

- each auth/persistence test creates its own temporary data directory
- tests clean up temp directories after execution
- this avoids shared state and file-lock collisions

## How to Run

- Build: `dotnet build CommerceConsole.csproj`
- Test: `dotnet test Tests/CommerceConsole.Tests/CommerceConsole.Tests.csproj`

## Current Risks and Gaps

Not yet covered (planned in next phases):
- checkout success/failure path matrix
- wallet debit/top-up behavior matrix
- order status transition policy matrix
- reporting aggregation correctness matrix
- menu-driven input parsing behavior tests

## Expansion Plan by Issue

- Issue 3: product search/filter and low-stock tests
- Issue 4: cart and wallet tests
- Issue 5: checkout and order creation tests
- Issue 6: order history/tracking/status tests
- Issue 7: reviews/reporting tests
- Issue 8: regression and edge-case hardening tests
- Issue 9: design-pattern parity tests
