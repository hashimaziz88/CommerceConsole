# CommerceConsole Codex Pack

This folder is the internal delivery and governance pack for AI-assisted implementation.
It is intentionally deeper than the root `README.md` and acts as the study and execution control center.

## Purpose Of This Pack

- Keep architecture, standards, prompts, and milestone policies synchronized with the live codebase.
- Provide a stable handoff guide for implementation agents and collaborators.
- Keep deep study and viva-oriented guidance out of the root README while preserving it in one place.

## What This Pack Contains

- `01_Project_Overview.md`: project scope, execution model, and rubric intent
- `02_Architecture_and_Solution_Structure.md`: layer boundaries and folder ownership
- `03_Domain_Model.md`: entity model, invariants, and exception model
- `04_Submission_1_Implementation_Blueprint.md`: baseline execution sequence
- `05_Submission_2_Design_Pattern_Upgrade.md`: Monday pattern-only upgrade plan
- `06_Coding_Standards.md`: coding and documentation standards
- `07_Git_Workflow_and_Branching.md`: branch strategy, milestone locks, PR checks
- `08_Codex_Prompt_Pack.md`: copy-paste prompt workflow
- `09_GitHub_Feature_Issues.md`: milestone-locked issue set
- `design-patterns-current.md`: deep pattern inventory, evidence, and phased positioning
- `viva-questions-and-answers.md`: study Q&A scripts and speaking prompts
- `agents.md`: role-oriented agent execution guidance
- `rules.md`: hard constraints and delivery guardrails

## Recommended Usage Order

1. Read `01`, `02`, `03` for system context.
2. Execute work with `04` and `06` as non-negotiable guardrails.
3. Use `08` for task slicing and `09` for milestone alignment.
4. Follow branch and milestone discipline in `07`.
5. Apply `05` only during the pattern-refactor phase.

## Submission Documentation Boundary

Use this split strictly:
- Root `docs/` should stay marker-facing and claim-safe.
- `.codex/` should carry deeper study, trade-offs, and refactor planning detail.

Current rule for Submission 2 public pattern scope:
- `Repository Pattern`, `Strategy Pattern`, `Factory Pattern`, and `Command Pattern` are formally named in `docs`.

## Study Navigation And Revision Path

Use this order for deep understanding:
1. `docs/architecture.md`
2. `docs/folder-structure-rationale.md`
3. `docs/oop-design-notes.md`
4. `docs/access-modifiers-and-class-design.md`
5. `docs/design-patterns-current.md`
6. feature behavior docs (`auth-flow`, `product-catalog`, `cart-wallet`, `checkout-orders`, `order-lifecycle`, `reviews-reporting`, `persistence`)
7. `docs/test-plan.md`
8. `docs/demo-study-guide.md`
9. `.codex/viva-questions-and-answers.md`
10. `.codex/design-patterns-current.md`

## Architecture Rationale References

Primary references:
- `docs/architecture.md`
- `docs/folder-structure-rationale.md`
- `02_Architecture_and_Solution_Structure.md`

Key positioning:
- layered architecture with domain-centered modeling discipline
- strong invariants and service orchestration with strict folder boundaries

## OOP And Pattern Explanation Pointers

Use these together when preparing explanations:
- `docs/oop-design-notes.md`
- `docs/access-modifiers-and-class-design.md`
- `docs/design-patterns-current.md`
- `.codex/design-patterns-current.md`
- `03_Domain_Model.md`

## Demo And Viva Preparation Pointers

- demo flow and speaking structure: `docs/demo-study-guide.md`
- likely questions and concise answers: `.codex/viva-questions-and-answers.md`
- regression confidence references: `docs/test-plan.md`

## Governance And Process Notes

- Root `README.md` stays concise and marker-facing.
- Deep reasoning and implementation governance stay in `.codex` and focused docs.
- Milestone locks in `09_GitHub_Feature_Issues.md` are mandatory unless explicitly approved changes are made.
- All behavior, architecture, or scope updates must be reflected in both docs and codex guidance.

## Current Project State Assumptions

- Baseline Submission 1 features are implemented.
- UX hardening, JSON persistence, and no-GUID presentation flows are implemented.
- Bonus features are implemented (PDF export, insights, recommendations).
- Submission 2 pattern work remains a controlled refactor phase.

## Maintenance Rule

If implementation changes and this pack is not updated, process drift occurs.
Update `.codex` files in the same delivery cycle as behavior/documentation changes.

