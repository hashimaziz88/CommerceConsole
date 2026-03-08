# Bonus Features Above Submission 1

## Purpose

This document captures the optional bonus scope implemented above the Submission 1 baseline.

Baseline workflows remain unchanged. Bonus features are additive and modular.

## Baseline vs Bonus

Baseline (Submission 1):
- authentication and role navigation
- catalog and inventory management
- cart and wallet
- checkout and order lifecycle
- reviews and reporting
- JSON persistence

Bonus (Submission 1+):
- PDF sales report export for admins
- heuristic smart insights for admins
- customer recommendation view based on purchase patterns and ratings

## Selected Bonus Features

## 1. PDF sales report export

What it does:
- generates a one-page PDF summary file for sales reporting
- includes generated timestamp, total revenue, status counts, best sellers, and low-stock rows

Architecture:
- orchestration: `Application/Services/ReportExportService.cs`
- abstraction: `Application/Interfaces/IReportExporter.cs`
- concrete exporter: `Infrastructure/Export/PdfReportExporter.cs`

Why this design:
- reporting aggregation stays in `ReportService`
- export formatting is isolated behind an interface
- future export formats (for example CSV) can be added without changing reporting logic

## 2. Heuristic smart admin insights

What it does:
- returns concise operational insights using rule-based heuristics
- examples: revenue snapshot, top category by units sold, low-stock watchlist, review sentiment, fulfillment queue alert

Architecture:
- contract: `Application/Interfaces/IInsightsService.cs`
- implementation: `Application/Services/InsightsService.cs`
- display: `Presentation/Helpers/ReportDisplayHelper.cs`

Design note:
- this is intentionally heuristic (local rules), not external LLM integration
- behavior works offline and degrades gracefully without network dependencies

## 3. Customer recommendation view

What it does:
- proposes active, in-stock products the customer has not already purchased
- prioritizes products in categories from purchase history
- uses average ratings and stock signals as tie-breakers
- shows human-readable reason text for each suggestion

Architecture:
- recommendation generation in `InsightsService`
- customer rendering in `ProductDisplayHelper.ShowRecommendations(...)`
- routing through `CustomerMenu`

## Runtime Integration

Composition root wiring in `Program.cs`:
- `InsightsService` created once and shared with admin/customer menus
- `PdfReportExporter` injected into `ReportExportService`
- menus only call service interfaces

## Persistence and Safety

- no baseline persistence contract was changed
- bonus features read existing repository data and write only optional export files
- no internal IDs are shown in user-facing bonus views

## Tests Added

- `Tests/CommerceConsole.Tests/Application/BonusFeaturesServiceTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/PdfReportExporterTests.cs`

Covered scenarios:
- recommendation filtering and ranking
- admin insight headline generation
- report-export orchestration and snapshot delegation
- PDF file generation and validation guards

## Demo Talking Points

1. Baseline flow works exactly as before.
2. Admin opens smart insights for instant operational guidance.
3. Admin exports the same report to PDF for submission evidence.
4. Customer opens recommendations and sees "why" explanations per product.
5. Mention that all bonus logic is isolated in services/exporters, not menu code.
