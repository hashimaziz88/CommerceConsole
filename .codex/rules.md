# Codex Rules

These rules are mandatory for any coding agent working in this project.

## 1. Source-of-truth rules

Always consult these files before coding:

- `../README.md`
- `../01_Project_Overview.md`
- `../02_Architecture_and_Solution_Structure.md`
- `../03_Domain_Model.md`
- `../04_Submission_1_Implementation_Blueprint.md`
- `../06_Coding_Standards.md`
- `../07_Git_Workflow_and_Branching.md`

Use these when relevant:
- `../05_Submission_2_Design_Pattern_Upgrade.md`
- `../08_Codex_Prompt_Pack.md`
- `../09_GitHub_Feature_Issues.md`

## 2. Project objective rules

The application must remain a C# console-based online shopping backend system with:
- customer and administrator role separation
- product catalog management
- shopping cart workflows
- wallet-based payment simulation
- order placement and tracking
- inventory management
- product reviews and ratings
- administrative reporting and analytics
- strong OOP, LINQ, validation, and exception handling

## 3. Execution model rules

### Baseline rule (always active)
Implement to Submission 2 quality from day one:
- complete feature scope
- clean modular architecture
- tests maintained continuously
- docs maintained continuously

Do not defer tests/docs as a later clean-up activity.

### Monday rule (2026-03-09)
Use `../05_Submission_2_Design_Pattern_Upgrade.md` for pattern implementation only.

Preferred patterns:
- Factory
- Strategy
- State-style transition handling
- Repository formalization
- optional Builder

Rule: refactor incrementally. Do not rewrite a working baseline unless explicitly asked.

## 4. Architecture rules

Follow `../02_Architecture_and_Solution_Structure.md`.

### Allowed responsibility split
- Presentation: menus, input, output, navigation
- Application: use cases, orchestration, service contracts
- Domain: entities, enums, business rules, exceptions
- Infrastructure: in-memory persistence, seed data, utilities

### Mandatory boundaries
- `Program.cs` must stay thin.
- Menu classes must not own business rules.
- Domain classes must not contain console I/O.
- DTOs must not replace domain entities in the core model.
- Infrastructure must not leak UI concerns.

## 5. Baseline deliverables rules

Mandatory deliverables:
- registration
- login
- customer/admin menus
- browse/search products
- admin add/update/delete/restock/view products
- cart add/view/update/remove
- wallet balance and funding
- checkout with wallet and stock validation
- order history and tracking
- admin order viewing and status updates
- review and rating flow
- LINQ-based reporting
- input validation and exception handling
- test coverage for core domain and service workflows
- documentation aligned with implemented behavior

## 6. Coding standards rules

Follow `../06_Coding_Standards.md` at all times.

Required style:
- choose clarity over cleverness
- use meaningful names
- keep classes short
- keep methods short
- use guard clauses
- avoid duplication
- order members logically
- comment non-obvious intent
- keep one class per file where practical

## 7. Domain modeling rules

Use `../03_Domain_Model.md`.

Mandatory domain expectations:
- `Product` prevents negative price and stock
- `Cart` supports add/update/remove/clear/calculate total
- `Order` stores snapshot-based order items
- `Review` rating is constrained to 1..5
- `Customer` owns wallet/cart/order history state

## 8. LINQ rules

Use LINQ where it meaningfully improves querying, especially for:
- product search
- low-stock views
- order lookups
- sales summaries
- top-selling products
- average ratings

Do not force LINQ where simple loops are clearer.

## 9. Exception and validation rules

Validation must be explicit and user-friendly.

Use custom exceptions for:
- validation errors
- authentication failures
- duplicate email
- insufficient funds
- insufficient stock
- not found cases

Catch exceptions near the presentation boundary and show clear console messages.

## 10. Testing and documentation rules

From the first feature branch onward:
- add/update tests in the same PR as behavior changes
- include negative-path tests for key validation rules
- keep deterministic test data
- update docs when behavior, assumptions, or architecture changes
- prevent doc drift

## 11. Repository rules

Use repository abstractions for stored entities.

- prefer in-memory repositories
- seed demo data for products and admin user
- keep persistence simple and predictable

Do not introduce a database unless explicitly requested.

## 12. Git and branch rules

Use implementation flow from `../07_Git_Workflow_and_Branching.md` and issue scopes from `../09_GitHub_Feature_Issues.md`.

Preferred branch sequence:
- `feature/01-solution-bootstrap`
- `feature/02-auth-and-roles`
- `feature/03-product-catalog`
- `feature/04-cart-management`
- `feature/05-orders-and-checkout`
- `feature/06-reviews-and-reporting`
- `feature/07-quality-hardening`
- `feature/08-design-patterns`

## 13. Output rules for coding agents

When generating code:
- produce compilable code where possible
- group files logically
- preserve namespaces and folder conventions
- mention assumptions briefly
- include tests/docs updates for changed behavior
- prefer incremental delivery over huge unreviewable dumps

When generating explanations:
- reference relevant project docs by file name
- keep suggestions aligned to current branch scope

## 14. Forbidden moves

Do not:
- collapse all logic into `Program.cs`
- put checkout logic directly inside menu handlers
- mix domain and infrastructure responsibilities
- introduce unnecessary frameworks for a console app
- defer tests/docs to a future phase without explicit approval
- change public behavior without updating tests/docs

## 15. Fast decision guide

When unsure:
- check `../01_Project_Overview.md` for scope
- check `../02_Architecture_and_Solution_Structure.md` for placement
- check `../03_Domain_Model.md` for rules
- check `../04_Submission_1_Implementation_Blueprint.md` for execution order
- check `../06_Coding_Standards.md` for code style and quality bar
- check `../05_Submission_2_Design_Pattern_Upgrade.md` only for Monday pattern work
