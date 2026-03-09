# Study Docs

This folder is the personal study pack synchronized with the current Submission 2 codebase.

## Purpose

- keep study-oriented explanations separate from assessor-facing root `docs`
- preserve deeper reasoning for architecture, patterns, and viva preparation
- maintain a revision-friendly documentation set that tracks code changes

## Key Files

- `design-patterns-current.md` (full pattern catalog: implemented, partial, and future candidates)
- `demo-study-guide.md` (demo rehearsal flow and technical checklist)
- `viva-questions-and-answers.md` (model viva answers aligned to current implementation)
- mirrored copies of root `docs/*.md` for integrated revision context

## Boundary Rule

- Root `docs/`: formal facilitator/assessor documentation.
- `.codex/study-docs/`: expanded learning and revision documentation.

## Maintenance Rule

Whenever architecture, behavior, or test baselines change:
1. update root `docs` first
2. sync mirrored study copies
3. refresh `design-patterns-current.md`, `demo-study-guide.md`, and `viva-questions-and-answers.md`
