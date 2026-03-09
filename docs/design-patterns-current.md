# Design Patterns (Submission 2 Scope)

## Purpose

This document formally records the design patterns that are concretely implemented in the Submission 2 codebase.

Scope rule:
- only implemented patterns are listed here for assessor-facing documentation.

## Implemented Pattern Set

1. Repository Pattern
2. Strategy Pattern
3. Factory Pattern
4. Command Pattern

---

## Repository Pattern

### Implementation Boundary
Repository contracts are defined in the application layer and implemented in infrastructure with JSON-backed persistence.

### Code Evidence
- Contracts:
  - `Application/Interfaces/IRepository.cs`
  - `Application/Interfaces/IUserRepository.cs`
  - `Application/Interfaces/IProductRepository.cs`
  - `Application/Interfaces/IOrderRepository.cs`
- Implementations:
  - `Infrastructure/Repositories/InMemoryUserRepository.cs`
  - `Infrastructure/Repositories/InMemoryProductRepository.cs`
  - `Infrastructure/Repositories/InMemoryOrderRepository.cs`
- Persistence adapter:
  - `Infrastructure/Persistence/JsonFileStore.cs`

### Test Evidence
- `Tests/CommerceConsole.Tests/Infrastructure/RepositoryContractTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/JsonPersistenceTests.cs`

### Outcome
- Data access is decoupled from application workflows.
- Menus do not call repositories directly.
- Persistence can be replaced with lower impact on services.

---

## Strategy Pattern

### Implementation Boundary
Algorithm variation is expressed through interfaces selected by orchestration services.

### Code Evidence
- Payment strategy:
  - `Application/Interfaces/IPaymentStrategy.cs`
  - `Application/Services/WalletPaymentStrategy.cs`
  - `Application/Services/OrderService.cs` (delegates payment execution)
- Export strategy:
  - `Application/Interfaces/IReportExporter.cs`
  - `Infrastructure/Export/PdfReportExporter.cs`
  - `Application/Services/ReportExportService.cs`

### Test Evidence
- `Tests/CommerceConsole.Tests/Application/WalletPaymentStrategyTests.cs`
- `Tests/CommerceConsole.Tests/Application/OrderServiceTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/PdfReportExporterTests.cs`

### Outcome
- Checkout orchestration remains in `OrderService` while payment logic is isolated.
- Report generation remains separate from output format concerns.

---

## Factory Pattern

### Implementation Boundary
Role-to-workspace resolution is centralized behind a dedicated factory abstraction.

### Code Evidence
- Contracts/adapters:
  - `Presentation/Workspaces/IUserWorkspace.cs`
  - `Presentation/Workspaces/IRoleWorkspaceFactory.cs`
  - `Presentation/Workspaces/CustomerWorkspace.cs`
  - `Presentation/Workspaces/AdminWorkspace.cs`
- Factory implementation:
  - `Presentation/Workspaces/RoleWorkspaceFactory.cs`
- Usage:
  - `Presentation/Menus/MainMenu.cs` (`TryResolve` routing)

### Test Evidence
- `Tests/CommerceConsole.Tests/Presentation/RoleWorkspaceFactoryTests.cs`
- `Tests/CommerceConsole.Tests/Presentation/MenuCommandTests.cs` (routing behavior)

### Outcome
- Main routing no longer depends on a role-switch creation block.
- Workspace construction rules are explicit and extendable.

---

## Command Pattern

### Implementation Boundary
Menu actions are mapped to command objects and dispatched through a common dispatcher.

### Code Evidence
- Command contracts/infrastructure:
  - `Presentation/Commands/IMenuCommand.cs`
  - `Presentation/Commands/MenuCommandDispatcher.cs`
  - `Presentation/Commands/MenuCommandResult.cs`
  - `Presentation/Commands/DelegateMenuCommand.cs`
- Explicit control-flow commands:
  - `Presentation/Commands/MainLoginRouteCommand.cs`
  - `Presentation/Commands/MainExitMenuCommand.cs`
  - `Presentation/Commands/WorkspaceLogoutCommand.cs`
- Menu usage:
  - `Presentation/Menus/MainMenu.cs`
  - `Presentation/Menus/CustomerMenu.cs`
  - `Presentation/Menus/AdminMenu.cs`

### Test Evidence
- `Tests/CommerceConsole.Tests/Presentation/MenuCommandTests.cs`

### Outcome
- Large selection `switch` blocks are removed from menu loops.
- Dispatch and control-flow semantics are consistent across workspaces.

---

## Verification Snapshot

Latest local regression execution (March 9, 2026):
- `dotnet test Tests\\CommerceConsole.Tests\\CommerceConsole.Tests.csproj --no-build`
- Result: 115 passed, 0 failed, 0 skipped.

## Scope Note

Other architecture techniques exist in the codebase (for example Service Layer, constructor injection, composition root, and data mapping), but the formal Submission 2 pattern claim set in this document is limited to the four implemented patterns above.

