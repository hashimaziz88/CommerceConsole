# 04. Submission 1 Implementation Blueprint

## Objective

Deliver the full baseline scope with Submission 2-level quality practices from the beginning:
- complete required features
- clean architecture boundaries
- regression tests updated as features evolve
- documentation synchronized with implementation

Monday (2026-03-09) remains pattern-upgrade day only.

## Execution phases

### Phase 1 - bootstrap
- establish layered solution structure
- add domain entities/enums/exceptions
- add repository contracts and JSON-backed in-memory repositories
- seed admin and starter catalog data
- add smoke tests and starter docs

### Phase 2 - authentication and roles
- customer registration
- customer/admin login
- session tracking
- role-based routing to customer/admin menus
- auth tests + auth flow docs

### Phase 3 - catalog management
- browse/search active products
- admin add/update/delete/restock/view products
- low-stock listing
- product tests + catalog docs

### Phase 4 - cart and wallet
- add/view/update/remove cart items
- stock-aware cart validations
- wallet balance + top-up
- cart/wallet tests + docs

### Phase 5 - checkout and orders
- wallet-only checkout
- stock/funds validations
- payment + order record creation
- stock deduction + cart clear
- order history + tracking + admin status updates
- order tests + lifecycle docs

### Phase 6 - reviews and reporting
- review submission for purchased products only
- average rating behavior
- sales reporting via LINQ aggregates
- review/report tests + docs

### Phase 7 - hardening and UX
- input validation loops and boundary exception handling
- consistent console helper usage
- index-based selection UX across user/admin flows
- no GUID exposure in presentation
- regression + docs drift fix

### Phase 8 - pattern day (Monday 2026-03-09)
- introduce explicit Factory/Strategy/State-style abstractions
- preserve behavior parity
- add pattern-focused tests and docs delta

## Mandatory quality gates

1. Menu handlers remain thin (routing/display only).
2. `Program.cs` remains composition root only.
3. Repositories use standalone model files (no nested classes).
4. Mutable data persists to JSON where required.
5. Tests updated in same feature change.
6. Docs updated in same feature change.

## Baseline acceptance checklist

- registration/login/role routing works
- catalog operations work for both roles
- cart and wallet rules enforce correctly
- checkout updates wallet/stock/order/cart correctly
- status transitions enforce allowed matrix only
- reviews restricted to purchased products
- reporting outputs expected aggregates
- invalid input does not crash app
- no user-facing internal IDs
- all relevant tests pass

## Current status snapshot (as of 2026-03-08)

- Baseline phases 1-7: implemented
- Bonus extensions (PDF export, insights, recommendations): implemented
- Pattern day (phase 8): pending targeted refactor phase
