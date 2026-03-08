# CommerceConsole Codex Pack

This folder is the internal delivery and governance pack for AI-assisted implementation.
It now reflects the completed Submission 2 pattern refactor baseline in this branch.

## Purpose Of This Pack

- Keep architecture, standards, prompts, and milestone policies synchronized with code.
- Provide a stable execution map for future feature work and demos.
- Keep deep architecture/process details separate from root README.

## What This Pack Contains

- `01_Project_Overview.md`: project scope and delivery state
- `02_Architecture_and_Solution_Structure.md`: current layer and folder map
- `03_Domain_Model.md`: entity and specification model
- `04_Submission_1_Implementation_Blueprint.md`: baseline sequence history
- `05_Submission_2_Design_Pattern_Upgrade.md`: implemented refactor record
- `06_Coding_Standards.md`: mandatory coding and documentation constraints
- `07_Git_Workflow_and_Branching.md`: branch and milestone discipline
- `08_Codex_Prompt_Pack.md`: execution prompts
- `09_GitHub_Feature_Issues.md`: milestone-locked issue map
- `agents.md`: role-oriented agent guidance
- `rules.md`: hard constraints and delivery guardrails

## Current Project State Assumptions

- Submission 1 baseline features are implemented and stable.
- JSON persistence and no-GUID presentation flows are preserved.
- Submission 2 refactor patterns now implemented in branch:
  - Factory (role/menu routing)
  - Strategy (payment + existing exporter seam)
  - State-style order transitions
  - Command menu dispatch
  - Specification + repository `Find(spec)` formalization
- Full test suite passes after refactor.

## Maintenance Rule

If implementation changes and this pack is not updated, process drift occurs.
Update `.codex` files in the same delivery cycle as behavior and architecture changes.
