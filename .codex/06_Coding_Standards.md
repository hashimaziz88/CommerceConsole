# 06. Coding Standards

## Purpose

Mandatory coding standards for CommerceConsole, including Submission 2 pattern constraints.

## Core quality rules

1. Prefer clarity over cleverness.
2. Keep classes focused and methods short.
3. Use guard clauses for invalid inputs/states.
4. Preserve clear naming conventions.
5. Remove duplication with reusable abstractions.

## Layering rules (strict)

- Presentation must not contain business-policy logic.
- Presentation must not access repositories directly.
- Application services orchestrate workflows and coordinate patterns.
- Domain holds invariants and domain specifications.
- Infrastructure holds persistence/export implementation details.

## Submission 2 pattern rules

### Factory
- Use factory for role/workspace resolution; avoid role switch logic in main flow.

### Strategy
- Keep algorithm variants behind interfaces.
- Do not add user-visible payment features unless explicitly scoped.

### State-style transitions
- Transition rules must be represented by policy/state objects, not hard-coded maps in services.

### Command
- Menu action dispatch should be command-based for maintainability.
- Command objects may call MenuActionHelper for boundary-safe exceptions.

### Specification
- Query/filter rules should be reusable specifications when reused across services.
- Repositories expose `Find(spec)` where formalized.

## Presentation safety rules

1. Never expose internal GUIDs in user-facing screens.
2. Keep index-based selection UX.
3. Keep input recovery loops deterministic.
4. Catch custom exceptions at presentation boundaries.

## Repository/persistence rules

1. No nested model classes in repository files.
2. Persistence models remain standalone files.
3. Mutable workflows must persist through repository updates.
4. Mapping remains explicit (`ToDomain` / `FromDomain`).

## Testing and docs rules

1. Add/update tests with behavior/architecture changes.
2. Add pattern-focused tests when pattern seams are introduced.
3. Keep docs and `.codex` synchronized with implementation.

## Definition of done

A change is complete only when:
- boundaries are respected
- no GUID exposure regression exists
- tests are passing
- docs/codex are accurate and current
