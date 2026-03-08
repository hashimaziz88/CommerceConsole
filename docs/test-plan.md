# Test Plan

## Goal

Ensure core authentication, catalog, cart/wallet, checkout/order lifecycle, reviews/reporting, persistence, presentation handling, and bonus features remain correct and stable while features evolve.

## Test Projects and Structure

Current test project:
- `Tests/CommerceConsole.Tests/CommerceConsole.Tests.csproj`

Current test folders:
- `Tests/CommerceConsole.Tests/Domain`
- `Tests/CommerceConsole.Tests/Application`
- `Tests/CommerceConsole.Tests/Infrastructure`
- `Tests/CommerceConsole.Tests/Presentation`

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
- cart null-customer guard
- cart update-to-zero removal
- cart negative quantity rejection
- cart update stock violation
- wallet top-up success + persistence
- wallet top-up validation failure
- wallet null-customer guard
- checkout happy path (wallet debit + stock deduction + order/payment creation + cart clear)
- checkout failure on insufficient funds
- checkout failure on insufficient stock
- checkout failure on missing product in cart
- checkout snapshot integrity for order items
- customer-order lookup empty-ID guard
- status update empty-ID guard
- valid order status transition sequence
- invalid order status transition rejection
- terminal-state transition rejection
- review add success + persistence + average rating
- review blocked for unpurchased product
- reviewable-products list contains purchased products only
- review rating validation failure
- report total revenue calculation
- report orders-by-status calculation
- report best-selling product aggregation/ranking
- report low-stock filtering/sorting
- bonus recommendation filtering/ranking and purchase exclusion
- bonus recommendation validation (invalid max count)
- bonus admin insight summary generation
- bonus export orchestration delegates snapshot to exporter

### Infrastructure tests
- seed persistence to JSON files
- idempotent reseeding behavior for expanded catalog
- persisted user reload across repository instances
- bonus PDF exporter file creation and header validation
- bonus PDF exporter validation guard for blank output directory

### Presentation tests
- menu selection range validation/retry behavior
- positive integer input retry behavior
- non-negative integer input behavior
- positive decimal input retry behavior
- invalid input-range configuration guard
- confirmation prompt yes/no/default behavior
- paged product rendering for larger lists
- global index visibility in selectable product views

## Test Data Strategy

- each test creates its own temporary data directory where repository persistence is involved
- tests clean up temp directories after execution
- console input/output tests isolate `Console.In`/`Console.Out` per case with synchronization

## How to Run

- Build: `dotnet build CommerceConsole.csproj`
- Test: `dotnet test Tests/CommerceConsole.Tests/CommerceConsole.Tests.csproj`

## Current Risks and Gaps

Not yet covered (planned in next phases):
- full end-to-end interactive console script regression suite
- multi-process file concurrency behavior
- multi-page PDF export rendering behavior

## Expansion Plan

1. Continue regression coverage as UX and bonus options evolve.
2. Add tests for future export formats (for example CSV) if introduced.
3. Expand pattern-focused tests during Monday pattern milestone work.
