# 06. Coding Standards

## Purpose

This is the mandatory coding standard for CommerceConsole. It extends the original standards with project-specific constraints from implementation history.

## Core coding quality rules

1. Prefer clarity over cleverness.
2. Keep classes small and focused.
3. Keep methods short and intention-revealing.
4. Use guard clauses for invalid inputs/states.
5. Use clear names; avoid ambiguous abbreviations.
6. Remove duplication through helper extraction.
7. Keep member ordering consistent (constants, fields, constructors, public members, private helpers).
8. Use comments for non-obvious intent only.

## Layering rules (strict)

- Presentation must not contain business rules.
- Presentation must not access repositories directly.
- Application services must orchestrate workflows.
- Domain must hold invariants and mutation rules.
- Infrastructure must hold persistence/export details only.

## Presentation safety rules

1. Never expose internal GUIDs in user-facing screens.
2. Use index-based selections for menus and entity picking.
3. Keep input recovery loops friendly and deterministic.
4. Catch custom exceptions at presentation boundaries and render clear messages.

## Repository and persistence rules

1. Repositories must not contain nested helper/model classes.
2. Persistence models must live as standalone files under repository model folders.
3. Mutable data workflows must persist through repository updates.
4. JSON schema mapping must be explicit (`ToDomain` / `FromDomain` style).

## OOP and modifier rules

- prefer `private readonly` for dependencies
- prefer `private set` for mutable domain properties
- use `protected` only when inheritance boundaries require it
- use `sealed` for stable concrete types unless extensibility is intentional
- use `static` only for stateless utility concerns

## LINQ usage rules

Use LINQ when it improves readability and query intent:
- filtering active/low-stock products
- grouping orders by status
- aggregating revenue and ratings
- ranking products/reports

Do not force LINQ where loops are clearer for index-based rendering.

## Testing rules

1. Add/update tests in the same change as behavior updates.
2. Cover both happy and negative paths.
3. Use deterministic fixtures and isolated temp directories.
4. Keep presentation helper tests isolated via console harness patterns.

## Documentation rules

1. Update docs whenever behavior/architecture/tests change.
2. Keep docs accurate enough for live code walkthrough and viva defense.
3. Avoid vague placeholders; include concrete rationale and trade-offs.
4. Keep `.codex` and `docs` synchronized to prevent process drift.

## Definition of done

A change is complete only when:
- architecture boundaries are respected
- no GUID exposure is introduced
- tests are updated and passing
- docs are updated and accurate
- code remains readable and maintainable
