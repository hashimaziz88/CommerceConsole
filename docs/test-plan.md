# Test Plan

## Goal

Ensure core authentication, product catalog workflows, cart/wallet workflows, checkout/order processing behavior, order lifecycle transitions, domain safety checks, and persistence behavior are correct and stable while features are incrementally added.

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
- product add success
- product add validation failure
- product search by category with active filtering
- product update success
- product update not-found failure
- product delete success
- product restock success
- low-stock filtering by threshold
- cart add success
- cart add stock violation
- cart update-to-zero removal
- cart update stock violation
- wallet top-up success + persistence
- wallet top-up validation failure
- checkout happy path (wallet debit + stock deduction + order/payment creation + cart clear)
- checkout failure on insufficient funds
- checkout failure on insufficient stock
- checkout failure on missing product in cart
- checkout snapshot integrity for order items
- valid order status transition sequence
- invalid order status transition rejection
- terminal-state transition rejection

### Infrastructure tests
- seed persistence to JSON files
- idempotent reseeding behavior
- persisted user reload across repository instances

## Test Data Strategy

- each auth/product/cart/wallet/checkout/order-transition/persistence test creates its own temporary data directory
- tests clean up temp directories after execution
- this avoids shared state and file-lock collisions

## How to Run

- Build: `dotnet build CommerceConsole.csproj`
- Test: `dotnet test Tests/CommerceConsole.Tests/CommerceConsole.Tests.csproj`

## Current Risks and Gaps

Not yet covered (planned in next phases):
- reporting aggregation correctness matrix
- review workflow correctness matrix
- menu-driven input parsing behavior tests

## Expansion Plan by Issue

- Issue 7: reviews/reporting tests
- Issue 8: regression and edge-case hardening tests
- Issue 9: design-pattern parity tests
