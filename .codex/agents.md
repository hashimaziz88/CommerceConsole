# Codex Agents Guide

This file tells a coding agent how to use the documentation in this repository to build the project safely and in the right order.

## Mission

Build the Boxfusion-style Online Shopping Backend System as a clean, modular C# console application with Submission 2-quality practices from day one.

Execution model:
- Baseline cycle: complete features, validation, tests, and docs.
- Monday cycle (2026-03-09): implement design patterns only.

Required scope includes customer/admin role separation, product catalog management, cart workflows, wallet checkout, order tracking, reviews, reporting, LINQ usage, exception handling, and menu-driven UX.

## Primary source order

Always use docs in this order when making implementation decisions:

1. `../01_Project_Overview.md` - business scope and execution model
2. `../02_Architecture_and_Solution_Structure.md` - folder layout and responsibilities
3. `../03_Domain_Model.md` - entities, invariants, and domain rules
4. `../04_Submission_1_Implementation_Blueprint.md` - baseline implementation sequence
5. `../06_Coding_Standards.md` - coding style, tests, and docs quality bar
6. `../07_Git_Workflow_and_Branching.md` - branch plan and merge sequence
7. `../05_Submission_2_Design_Pattern_Upgrade.md` - Monday pattern implementation
8. `../08_Codex_Prompt_Pack.md` - task-sized generation prompts
9. `../09_GitHub_Feature_Issues.md` - issue scope and branch mapping

If two docs conflict, prefer the earlier item unless explicitly overridden.

## Agent roles

### 1. Solution Architect Agent
Use for:
- defining project structure
- validating separation of concerns
- enforcing Presentation/Application/Domain/Infrastructure boundaries

Read first:
- `../02_Architecture_and_Solution_Structure.md`
- `../06_Coding_Standards.md`

Output:
- solution/folder layout
- interface boundaries
- dependency direction

### 2. Domain Modeling Agent
Use for:
- entity creation
- enums
- business rules
- validation rules inside domain methods or constructors

Read first:
- `../03_Domain_Model.md`
- `../01_Project_Overview.md`

Output:
- entities
- enums
- domain exceptions
- domain-safe behavior without console I/O

### 3. Application Services Agent
Use for:
- service interfaces
- orchestration logic
- checkout flow
- authentication logic
- reports and review logic

Read first:
- `../04_Submission_1_Implementation_Blueprint.md`
- `../02_Architecture_and_Solution_Structure.md`
- `../06_Coding_Standards.md`

Output:
- service interfaces and implementations
- use-case methods
- validation and exception flow
- tests for core behaviors

### 4. Repository and Seed Data Agent
Use for:
- in-memory repositories
- repository contracts
- seed data for admin and products

Read first:
- `../02_Architecture_and_Solution_Structure.md`
- `../04_Submission_1_Implementation_Blueprint.md`

Output:
- repository interfaces and implementations
- seeded admin user and products
- deterministic fixtures for tests

### 5. Console UI Agent
Use for:
- menu classes
- screen flow
- input handling
- output formatting helpers

Read first:
- `../01_Project_Overview.md`
- `../04_Submission_1_Implementation_Blueprint.md`
- `../06_Coding_Standards.md`

Output:
- thin menu classes
- robust numeric input handling
- no embedded business logic beyond routing/display

### 6. Quality and Docs Agent
Use throughout all phases.

Read first:
- `../06_Coding_Standards.md`
- `../04_Submission_1_Implementation_Blueprint.md`

Output:
- focused tests for changed behavior
- docs updates for behavior/architecture deltas
- regression checks for major workflows

### 7. Design Pattern Agent (Monday only)
Use during the Monday pattern cycle.

Read first:
- `../05_Submission_2_Design_Pattern_Upgrade.md`
- `../02_Architecture_and_Solution_Structure.md`
- `../06_Coding_Standards.md`

Target patterns:
- Factory
- Strategy
- State-style transition handling
- Repository formalization
- optional Builder where justified

Output:
- incremental pattern implementation
- behavior-preserving refactor
- pattern-focused tests and docs notes

## Implementation sequence

Follow this order unless the user overrides it:

1. solution bootstrap
2. domain entities and enums
3. exceptions and validation primitives
4. repository interfaces and in-memory implementations
5. seed data
6. registration/login/session handling
7. role-based menus
8. product browsing/search/admin catalog management
9. cart and wallet management
10. checkout and order creation
11. order history and tracking
12. reviews and reporting
13. validation hardening and UX polish
14. regression tests and docs alignment
15. Monday pattern implementation

Reference:
- `../04_Submission_1_Implementation_Blueprint.md`
- `../07_Git_Workflow_and_Branching.md`

## Task sizing rules

When generating code:
- work feature-by-feature, not giant full-project dumps
- prefer compilable slices
- keep file outputs grouped by layer
- explain assumptions briefly when needed
- preserve namespace consistency
- include tests/docs updates for changed behavior

## Done criteria

A task is complete only when:
- it compiles logically with the current architecture
- it matches relevant documentation
- business logic is not pushed into menus or `Program.cs`
- validation and exceptions are handled cleanly
- names are clear and consistent
- LINQ is used where appropriate
- tests are updated for the change
- docs are updated where needed

## Branch-aware delivery

Prefer feature order from:
- `../07_Git_Workflow_and_Branching.md`
- `../09_GitHub_Feature_Issues.md`

When asked to implement work for a branch, limit changes to that branch scope.

## What the agent must avoid

Do not:
- add database or web API complexity unless explicitly requested
- mix DTOs into domain entities
- put console reads/writes inside domain classes
- create giant manager/god classes
- hardcode branching logic everywhere when service abstractions exist
- postpone tests/docs as a separate future phase
- perform a full rewrite during Monday pattern work

## Best prompt entry points

For code generation sessions, start from:
- `../08_Codex_Prompt_Pack.md`

For scope and acceptance checks, use:
- `../01_Project_Overview.md`
- `../04_Submission_1_Implementation_Blueprint.md`
- `../09_GitHub_Feature_Issues.md`
