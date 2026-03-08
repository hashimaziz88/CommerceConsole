# 07. Git Workflow and Branching

## Branch strategy

Primary branch types:
- `main`: stable release branch
- `develop`: integration branch
- `feature/*`: scoped feature branches aligned to issue milestones

Suggested feature branches:
- `feature/01-solution-bootstrap`
- `feature/02-auth-and-roles`
- `feature/03-product-catalog`
- `feature/04-cart-management`
- `feature/05-orders-and-checkout`
- `feature/06-reviews-and-reporting`
- `feature/07-quality-hardening`
- `feature/08-design-patterns`

## Milestone lock policy (must enforce)

- Issues 1-2 -> `M1-Foundation-and-Auth`
- Issues 3-5 -> `M2-Catalog-Cart-and-Checkout`
- Issues 6-8 -> `M3-Orders-Reporting-and-Quality`
- Issue 9 -> `M4-Monday-Patterns-2026-03-09`

Do not move issues across milestones without explicit approval.

## Commit format

Use concise typed commits:
- `feat: ...`
- `fix: ...`
- `refactor: ...`
- `test: ...`
- `docs: ...`

## Pull request checklist

1. Scope matches target issue/milestone.
2. No architecture boundary violations introduced.
3. No GUID exposure in user-facing screens.
4. Tests added/updated and passing.
5. Docs and `.codex` files updated where relevant.
6. Build passes.

## Safe merge order

1. bootstrap
2. auth
3. catalog
4. cart/wallet
5. checkout/orders
6. reviews/reporting
7. quality hardening
8. pattern upgrade

## Release checkpoints

Before Monday pattern work:
- freeze baseline behavior
- run full regression test suite
- ensure docs reflect current implementation
- tag baseline (example: `release/baseline-ready`)

After Monday pattern work:
- verify behavior parity
- run full regression + pattern-focused tests
- document architecture delta

## Risk control guidance

- prefer small PRs per feature scope
- avoid mixing unrelated refactors with feature changes
- avoid destructive git operations on shared worktrees
