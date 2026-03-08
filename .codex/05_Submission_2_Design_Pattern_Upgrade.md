# 05. Submission 2 Design Pattern Upgrade

## Goal (Monday 2026-03-09)

Introduce explicit design patterns on top of the stable baseline without adding new business scope.

This phase is a controlled refactor for extensibility and architecture marks.

## Non-negotiable constraints

- preserve current user-visible behavior
- do not move business logic into menus
- do not introduce feature backlog catch-up
- do not break persistence contracts without migration notes
- add pattern-focused tests and docs for every pattern change

## Priority pattern targets

### 1. Factory Pattern

Candidate uses:
- role-based menu creation/routing
- user object creation abstraction if beneficial

Expected benefit:
- centralize creation logic and reduce switch branching in UI composition points

### 2. Strategy Pattern

Candidate uses:
- payment processing strategy abstraction
- report generation/export variant strategy

Current seam already available:
- `IReportExporter` -> `PdfReportExporter`

Suggested extension:
- add payment strategy contract and wallet strategy implementation while keeping behavior same

### 3. State-style transition handling

Candidate uses:
- extract order status transitions from map-based rule checks into dedicated transition policy/state objects

Expected benefit:
- clearer transition rules and easier extension/audit of lifecycle logic

### 4. Repository formalization (if needed)

Candidate uses:
- refine repository abstractions only where extension clarity improves
- avoid needless abstraction churn

## Execution sequence

1. Freeze baseline behavior with passing regression tests.
2. Add pattern interfaces and adapters around existing seams.
3. Move branching logic into factories/strategies incrementally.
4. Extract transition policy objects for order lifecycle.
5. Verify parity with focused tests.
6. Document architecture delta and trade-offs.

## Verification checklist

- baseline tests remain green
- new pattern tests cover extension points
- no UI/business boundary regression
- no GUID exposure regression
- docs updated (`architecture`, `design-patterns`, `oop notes`, `study guide`)

## Anti-goals

- no full rewrite
- no framework-heavy pattern overengineering
- no feature expansion hidden inside refactor

## Viva explanation script

"Submission 2 pattern work is an incremental extension of existing seams: we formalize creation and algorithm variation points while preserving baseline behavior, so the refactor improves extensibility without destabilizing delivery."
