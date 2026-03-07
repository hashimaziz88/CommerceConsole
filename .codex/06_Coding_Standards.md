# 06. Coding Standards

This document adapts the provided Boxfusion C# coding guidance to this console shopping project.

## Core standards to follow

### 1. Write code that is easy to read
Favor clarity over cleverness.

### 2. Add comments where intent is not obvious
- All classes should have a purpose comment unless their role is obvious.
- All public methods should have a short summary comment.
- Non-obvious logic should be explained briefly above the code.

### 3. Keep classes short
Avoid god classes. Split responsibilities into services, repositories, helpers, and entities.

### 4. Keep methods short
If a method is hard to scan, extract helpers.

### 5. Order class members logically
Suggested order:
1. constants
2. fields
3. constructors
4. public properties
5. public methods
6. private helper methods

### 6. Use guard clauses
Fail fast for invalid arguments and invalid states.

```csharp
if (quantity <= 0)
{
    throw new ValidationException("Quantity must be greater than zero.");
}
```

### 7. Avoid excessive nesting
Prefer early returns and extracted methods.

### 8. Use clear naming
- `GetProductById` not `FetchP`
- `UpdateOrderStatus` not `DoUpdate`
- `walletAmountToAdd` not `x`

### 9. Replace magic numbers with constants or enums
Examples:
- low stock threshold
- max review rating
- menu option numbers when reused

### 10. Do not repeat yourself
Extract shared prompts, validation logic, and display formatting.

### 11. Keep it simple
Do not build an unnecessary framework.

### 12. Format code consistently
Keep braces and spacing consistent.

### 13. Remove dead code
Delete unused helpers, stale paths, and commented-out code.

## Project-specific rules

### Naming conventions
- Classes: `PascalCase`
- Methods: `PascalCase`
- Public properties: `PascalCase`
- Private fields: `_camelCase`
- Local variables and parameters: `camelCase`
- Interfaces: prefix with `I`
- Enums: singular names where possible

### Layering rules
- no domain logic in presentation/menu classes
- no DTOs inside domain folder
- repositories should not print to console
- services should not ask for console input directly

### File and folder rules
- one public class per file
- keep folders aligned to architecture
- do not mix entities and services in the same folder

### Error handling rules
- throw domain/application exceptions with useful messages
- catch exceptions at menu/screen boundary
- never swallow exceptions silently

### LINQ rules
Use LINQ where it improves readability, not to tick a box.

Good fits:
- filtering products
- grouping orders
- calculating averages and totals
- sorting history or reports

## Test standards (from day one)

- add or update tests in the same feature PR where behavior changes
- cover domain invariants and core service workflows first
- include negative-path tests for validation and exceptions
- keep tests deterministic with explicit seed data
- do not postpone baseline tests to a later phase

## Documentation standards (from day one)

- update docs when behavior, architecture, or assumptions change
- keep `docs/test-plan.md` aligned with actual coverage focus
- record intentional trade-offs in short, explicit notes
- avoid stale TODO docs that are not actioned

## Definition of done for each class/feature

- clear single responsibility
- meaningful name
- no duplicated logic
- validation present where needed
- comments added for non-obvious behavior
- methods small enough to scan quickly
- tests updated for changed behavior
- docs updated where relevant
