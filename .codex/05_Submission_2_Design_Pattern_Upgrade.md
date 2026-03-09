# 05. Submission 2 Design Pattern Upgrade

## Goal (Completed on 2026-03-09)

Concretely implement the approved Submission 2 pattern set without feature expansion:
- Repository Pattern
- Strategy Pattern
- Factory Pattern
- Command Pattern

## Implemented Outcomes

## 1. Repository Pattern
- Existing repository contracts and JSON-backed implementations remained the data boundary.
- Added repository contract/parity tests for CRUD and rehydration behavior.

## 2. Strategy Pattern
- Existing export strategy seam retained (`IReportExporter`).
- Added payment strategy seam (`IPaymentStrategy`) with `WalletPaymentStrategy`.
- `OrderService` now delegates payment execution to strategy while preserving checkout orchestration behavior.

## 3. Factory Pattern
- Added role workspace abstractions (`IUserWorkspace`) and role resolver (`IRoleWorkspaceFactory`).
- Main role routing now resolves workspace through factory instead of switch logic.

## 4. Command Pattern
- Added command abstractions (`IMenuCommand`, `MenuCommandDispatcher`, `MenuCommandResult`).
- Replaced main/customer/admin menu selection switches with command map dispatch.
- Added explicit control-flow commands and delegate-backed routine commands.

## Validation Summary

- behavior parity preserved (no feature-scope expansion)
- no repository calls from menus
- no GUID exposure changes in presentation
- full regression suite passing after refactor

## Remaining Future Refactor Candidates (Outside This Completed Scope)

- state-style transition objects for order lifecycle rules
- specification-style query objects for reusable filtering policies

## Viva Script

"Submission 2 was delivered as a controlled refactor: Repository was hardened, Strategy was expanded to payment, Factory was implemented for role workspace routing, and Command was implemented for menu dispatch. Baseline behavior remained stable with full regression pass."
