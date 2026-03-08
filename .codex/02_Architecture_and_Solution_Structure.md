# 02. Architecture and Solution Structure

## Goal

Keep the console app clean, testable, and explainable by enforcing strict layer responsibilities and dependency direction.

## Current solution structure

```text
CommerceConsole/
|-- Program.cs
|-- Application/
|   |-- Interfaces/
|   |   |-- IAuthService.cs
|   |   |-- ICartService.cs
|   |   |-- IInsightsService.cs
|   |   |-- IOrderRepository.cs
|   |   |-- IOrderService.cs
|   |   |-- IProductRepository.cs
|   |   |-- IProductService.cs
|   |   |-- IReportExporter.cs
|   |   |-- IReportExportService.cs
|   |   |-- IReportService.cs
|   |   |-- IRepository.cs
|   |   |-- IReviewService.cs
|   |   |-- ISessionContext.cs
|   |   |-- IUserRepository.cs
|   |   `-- IWalletService.cs
|   |-- Models/
|   |   |-- LowStockReportItem.cs
|   |   |-- ProductRecommendationItem.cs
|   |   |-- ProductSalesReportItem.cs
|   |   `-- SalesReportSnapshot.cs
|   `-- Services/
|       |-- AuthService.cs
|       |-- CartService.cs
|       |-- InsightsService.cs
|       |-- OrderService.cs
|       |-- ProductService.cs
|       |-- ReportExportService.cs
|       |-- ReportService.cs
|       |-- ReviewService.cs
|       |-- SessionContext.cs
|       `-- WalletService.cs
|-- Domain/
|   |-- Entities/
|   |-- Enums/
|   `-- Exceptions/
|-- Infrastructure/
|   |-- Data/
|   |   `-- SeedData.cs
|   |-- Export/
|   |   `-- PdfReportExporter.cs
|   |-- Persistence/
|   |   `-- JsonFileStore.cs
|   `-- Repositories/
|       |-- InMemoryOrderRepository.cs
|       |-- InMemoryProductRepository.cs
|       |-- InMemoryUserRepository.cs
|       `-- Models/
|-- Presentation/
|   |-- Helpers/
|   `-- Menus/
|-- Tests/
|   `-- CommerceConsole.Tests/
|       |-- Application/
|       |-- Domain/
|       |-- Infrastructure/
|       `-- Presentation/
|-- docs/
`-- .codex/
```

## Layer responsibilities

### Presentation
Allowed:
- menu navigation
- input parsing and prompt loops
- formatting output
- calling service methods
- boundary exception messaging

Not allowed:
- direct repository access
- checkout/order/stock business policies
- JSON/export technical concerns
- user-facing GUID exposure

### Application
Allowed:
- use-case orchestration
- workflow validations
- dependency on interfaces
- service-level LINQ queries and aggregations

Not allowed:
- console input/output
- low-level file I/O implementation

### Domain
Allowed:
- entities and value semantics
- state-transition methods
- guard clauses and invariants
- domain enums and exceptions

Not allowed:
- repository or file operations
- menu concerns
- formatter/export details

### Infrastructure
Allowed:
- repository implementations
- persistence records and data mapping
- seed logic
- exporter implementation details

Not allowed:
- UI routing/formatting
- business policy branching that belongs in services

## Dependency direction

Required direction:
- `Presentation -> Application -> Domain`
- `Infrastructure -> Domain`
- `Infrastructure` implements contracts from `Application/Interfaces`

Forbidden direction:
- `Domain -> Infrastructure`
- `Presentation -> Infrastructure repositories`
- `Domain -> Presentation`

## Boundary hard rules

1. `Program.cs` remains a composition root, not a business workflow host.
2. Menus are thin and index-based.
3. Repository models stay in standalone files (no nested classes in repository files).
4. Domain mutation must happen through validated methods.
5. Docs and tests must update when behavior changes.

## Why this structure is redesign-friendly

This design supports low-risk incremental upgrades:
- Factory extraction for role/menu creation
- Strategy extraction for payment/report variants
- State-style policy extraction for order transitions
- repository backend swaps (JSON to DB) behind existing interfaces
- UI replacement without rewriting domain workflows
