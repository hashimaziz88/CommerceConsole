# 07. Git Workflow and Branching

## Branching strategy

### Main branches
- `main` - stable final branch
- `develop` - integration branch for current progress

### Feature branches
Use this naming format:

```text
feature/<number>-<short-description>
```

Examples:
- `feature/01-solution-bootstrap`
- `feature/02-auth-and-roles`
- `feature/03-product-catalog`
- `feature/04-cart-management`

## Suggested delivery workflow

### Phase 1
Create repository, baseline architecture, test project, and docs skeleton.
- branch: `feature/01-solution-bootstrap`

### Phase 2
Build authentication and role menus.
- branch: `feature/02-auth-and-roles`

### Phase 3
Build product browsing and admin catalog management.
- branch: `feature/03-product-catalog`

### Phase 4
Build cart and wallet management.
- branch: `feature/04-cart-management`

### Phase 5
Build checkout, orders, and tracking.
- branch: `feature/05-orders-and-checkout`

### Phase 6
Build reviews and sales reporting.
- branch: `feature/06-reviews-and-reporting`

### Phase 7
Hardening: validation, exception handling, UX polish, regression tests, docs alignment.
- branch: `feature/07-quality-hardening`

### Phase 8 (Monday 2026-03-09)
Design pattern implementation only.
- branch: `feature/08-design-patterns`

## Commit format

```text
<type>: <brief summary>
```

Examples:
- `feat: add user registration and login flow`
- `feat: implement cart add and update operations`
- `refactor: introduce payment strategy abstraction`
- `test: add checkout insufficient-funds scenario`
- `docs: update architecture for factory strategy adoption`

## Pull request checklist

- feature works end to end
- code follows folder structure
- no duplicated logic introduced
- no business rules inside menu classes
- exceptions handled gracefully
- tests added/updated for changed behavior
- documentation updated if behavior or architecture changed
- solution builds cleanly

## GitHub issue to branch mapping

1. solution bootstrap -> `feature/01-solution-bootstrap`
2. auth and roles -> `feature/02-auth-and-roles`
3. product catalog -> `feature/03-product-catalog`
4. cart management -> `feature/04-cart-management`
5. checkout and orders -> `feature/05-orders-and-checkout`
6. reviews and reporting -> `feature/06-reviews-and-reporting`
7. quality hardening -> `feature/07-quality-hardening`
8. design patterns (Monday only) -> `feature/08-design-patterns`

## Safe merge order

Always merge in this order where possible:
1. bootstrap
2. auth
3. catalog
4. cart
5. checkout
6. reviews/reporting
7. quality hardening
8. design patterns

## Release recommendation

Before Monday pattern work:
- merge baseline work into `develop`
- run full regression test suite
- ensure docs reflect actual behavior
- create backup tag or branch like `release/baseline-ready`

Then branch for Monday implementation:
- `feature/08-design-patterns`
