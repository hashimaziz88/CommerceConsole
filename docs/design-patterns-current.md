# Design Patterns (Submission 2 Scope)

## Purpose

This document records the Submission 2 pattern implementation scope in public `docs`.
The concrete pattern set is:
1. Repository Pattern
2. Strategy Pattern
3. Factory Pattern
4. Command Pattern

## Repository Pattern

Status:
- implemented and hardened with repository contract tests

Where used:
- `IRepository<T>`, `IUserRepository`, `IProductRepository`, `IOrderRepository`
- JSON-backed implementations in `Infrastructure/Repositories`

Why it matters:
- decouples storage mechanics from application workflows
- keeps menus free from data-access responsibilities

## Strategy Pattern

Status:
- implemented for export and payment behavior

Where used:
- export strategy seam: `IReportExporter` -> `PdfReportExporter` via `ReportExportService`
- payment strategy seam: `IPaymentStrategy` -> `WalletPaymentStrategy` via `OrderService`

Why it matters:
- separates algorithm choice from orchestration
- allows extension without rewriting core workflow code

## Factory Pattern

Status:
- implemented for role-to-workspace routing

Where used:
- `IRoleWorkspaceFactory`, `RoleWorkspaceFactory`
- `IUserWorkspace`, `CustomerWorkspace`, `AdminWorkspace`
- `MainMenu` resolves workspace through factory instead of role switch dispatch

Why it matters:
- centralizes creation/resolution logic
- reduces branching in entry/routing flow

## Command Pattern

Status:
- implemented for menu action dispatch

Where used:
- `IMenuCommand`, `MenuCommandDispatcher`, `MenuCommandResult`, `DelegateMenuCommand`
- explicit control-flow commands: `MainLoginRouteCommand`, `MainExitMenuCommand`, `WorkspaceLogoutCommand`
- `MainMenu`, `CustomerMenu`, and `AdminMenu` use command map dispatch instead of selection switches

Why it matters:
- keeps menu loops smaller and more consistent
- isolates action behavior behind command objects

## Submission Guidance

For Submission 2 explanation:
- claim these four patterns as concrete implementation
- map each claim to code-level evidence and tests
- keep architecture explanation focused on behavior parity + extensibility gains
