# Codex Agents Guide

## Mission

Implement and maintain CommerceConsole with strict layered architecture, strong OOP boundaries, continuous tests/docs, and milestone-locked delivery.

Execution model:
- baseline quality from day one
- Monday (2026-03-09) reserved for pattern-focused refactor

## Source-of-truth read order

Read in this order before coding:
1. `01_Project_Overview.md`
2. `02_Architecture_and_Solution_Structure.md`
3. `03_Domain_Model.md`
4. `04_Submission_1_Implementation_Blueprint.md`
5. `06_Coding_Standards.md`
6. `07_Git_Workflow_and_Branching.md`
7. `08_Codex_Prompt_Pack.md`
8. `09_GitHub_Feature_Issues.md`
9. `05_Submission_2_Design_Pattern_Upgrade.md` (Monday phase only)

When documentation detail is required, also read project docs:
- `../docs/architecture.md`
- `../docs/oop-design-notes.md`
- `../docs/design-patterns-current.md`
- `../docs/folder-structure-rationale.md`
- `../docs/access-modifiers-and-class-design.md`
- `../docs/demo-study-guide.md`
- `../docs/viva-questions-and-answers.md`

## Agent roles

### 1. Architecture Agent
Focus:
- placement correctness by layer
- dependency direction checks
- boundary violations

### 2. Domain Agent
Focus:
- entities/enums/exceptions
- invariants and guard clauses
- encapsulation and mutation safety

### 3. Application Services Agent
Focus:
- orchestration logic
- workflow validation
- interface-driven use cases

### 4. Infrastructure Agent
Focus:
- repository implementation
- mapper/persistence model hygiene
- JSON persistence and seed behavior
- exporter adapter implementations

### 5. Presentation Agent
Focus:
- menu routing and display
- input recovery loops
- index-based UX
- no GUID exposure

### 6. Quality and Documentation Agent
Focus:
- tests updated with behavior changes
- docs updated with behavior/architecture changes
- consistency across `docs` and `.codex`

### 7. Pattern Upgrade Agent (Monday)
Focus:
- explicit Factory/Strategy/State-style modules
- behavior parity and low-risk refactor
- pattern-focused tests + docs deltas

## Mandatory constraints for all agents

1. No repository calls from menus.
2. No business logic in `Program.cs`.
3. No nested classes in repository implementation files.
4. No user-facing GUID entry points.
5. Keep mutable workflows persisted to JSON repositories.
6. Keep tests/docs synchronized with every change.

## Task completion criteria

A task is done only when:
- architecture boundaries are respected
- code compiles and behavior is coherent
- relevant tests are updated/passing
- docs are updated with accurate rationale
- scope matches target issue/milestone

## Scope discipline

- Follow issue-to-milestone lock policy in `09_GitHub_Feature_Issues.md`.
- Do not move work across milestones without explicit approval.
- Monday pattern phase must not become feature catch-up.
