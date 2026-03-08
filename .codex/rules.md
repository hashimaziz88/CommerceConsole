# Codex Rules

These rules are mandatory for any coding agent working in this repository.

## 1. Core project objective

Maintain CommerceConsole as a clean, modular C# console shopping backend with:
- customer/admin role separation
- catalog/cart/wallet/checkout/orders/reviews/reporting workflows
- robust validation and exception handling
- JSON persistence
- test and documentation discipline

## 2. Source-of-truth precedence

Always follow this order:
1. `01_Project_Overview.md`
2. `02_Architecture_and_Solution_Structure.md`
3. `03_Domain_Model.md`
4. `04_Submission_1_Implementation_Blueprint.md`
5. `06_Coding_Standards.md`
6. `07_Git_Workflow_and_Branching.md`
7. `08_Codex_Prompt_Pack.md`
8. `09_GitHub_Feature_Issues.md`
9. `05_Submission_2_Design_Pattern_Upgrade.md` (Monday only)

## 3. Architecture rules

1. `Program.cs` must remain composition root only.
2. Menus must remain routing/display only.
3. Domain must not contain console I/O.
4. Presentation must not call repositories directly.
5. Infrastructure must not leak UI concerns.

## 4. UX and security rules

1. Never expose internal GUIDs in user-facing flows.
2. Use index-based selections for product/cart/order actions.
3. Keep prompts and error messages user-friendly and consistent.

## 5. Repository and persistence rules

1. Keep repository model classes in standalone files.
2. Do not use nested classes inside repository implementation files.
3. Persist mutable changes through repository writes.
4. Keep JSON mapping explicit and readable.

## 6. Validation and exception rules

- use guard clauses in domain/services
- throw meaningful custom exceptions
- catch at presentation boundary for friendly output
- do not swallow exceptions silently

## 7. Testing and docs rules

1. Behavior changes require tests in same change set.
2. Behavior/architecture changes require doc updates in same change set.
3. Keep `.codex` and `docs` synchronized.
4. Prefer deterministic tests with isolated state.

## 8. Milestone discipline

Milestone lock policy:
- Issues 1-2 -> `M1-Foundation-and-Auth`
- Issues 3-5 -> `M2-Catalog-Cart-and-Checkout`
- Issues 6-8 -> `M3-Orders-Reporting-and-Quality`
- Issue 9 -> `M4-Monday-Patterns-2026-03-09`

No cross-milestone movement without explicit approval.

## 9. Monday pattern rule

On Monday (2026-03-09):
- implement explicit design patterns only
- preserve baseline behavior
- avoid feature expansion
- add pattern-focused tests/docs

## 10. Forbidden actions

Do not:
- collapse logic into `Program.cs`
- move business rules into menus
- rewrite stable baseline unnecessarily
- defer tests/docs for "later cleanup"
- break milestone scope boundaries silently

## 11. Definition of done

A task is complete only if:
- architecture boundaries are intact
- behavior is validated by tests
- docs are accurate and updated
- user-facing UX keeps index-based, no-GUID interaction
- changes are scoped to the intended milestone/issue
