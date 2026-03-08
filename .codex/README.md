# CommerceConsole Codex Pack

This folder is the internal delivery and governance pack for AI-assisted implementation.

It exists to keep architecture, coding standards, milestone scope, and prompt workflows synchronized with the live codebase.

## What this pack contains

- `01_Project_Overview.md`: project scope, execution model, and rubric intent
- `02_Architecture_and_Solution_Structure.md`: layer boundaries and folder ownership
- `03_Domain_Model.md`: entity model, invariants, and exception model
- `04_Submission_1_Implementation_Blueprint.md`: baseline execution sequence
- `05_Submission_2_Design_Pattern_Upgrade.md`: Monday pattern-only upgrade plan
- `06_Coding_Standards.md`: non-negotiable coding and documentation standards
- `07_Git_Workflow_and_Branching.md`: branch strategy, milestone locks, PR checklist
- `08_Codex_Prompt_Pack.md`: copy-paste prompts (core + extension)
- `09_GitHub_Feature_Issues.md`: milestone-locked issue templates
- `agents.md`: role-oriented agent execution guide
- `rules.md`: hard constraints and guardrails

## Usage order

1. Read `01`, `02`, `03` for context and boundaries.
2. Execute work using `04` and `06`.
3. Follow milestone/branch discipline in `07` and `09`.
4. Use `08` prompts for task slicing.
5. Only apply `05` during the Monday pattern phase.

## Current project state assumptions

- Baseline Submission 1 features are implemented.
- UX hardening, JSON persistence, and no-GUID presentation flows are implemented.
- Bonus features are implemented (PDF export, insights, recommendations).
- Monday pattern work remains a controlled refactor phase, not feature catch-up.

## Maintenance rule

Any behavior, architecture, testing, or milestone change must be reflected in this folder immediately to avoid process drift.
