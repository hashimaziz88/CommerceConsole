# Bonus Features Above Submission 1

## Purpose

This document distinguishes baseline requirements from bonus capabilities, and explains bonus architecture choices in a modular, low-risk way.

## Baseline vs Bonus Boundary

Baseline (required):
- auth and role routing
- product catalog + admin inventory
- cart + wallet
- checkout + order lifecycle
- reviews + reporting
- JSON persistence and UX hardening

Bonus (optional, implemented):
1. PDF sales report export
2. smart heuristic admin insights
3. customer recommendations with explanation reasons

Design principle:
- bonus features are additive and isolated; they do not rewrite baseline rules.

## Bonus Feature 1: PDF Sales Report Export

What it does:
- exports report snapshot into a one-page PDF
- includes timestamp, revenue, status counts, best sellers, low-stock list

Architecture:
- orchestration: `ReportExportService`
- abstraction: `IReportExporter`
- concrete adapter: `PdfReportExporter`

Why this design is strong:
- report computation remains in `ReportService`
- export format is decoupled behind interface
- new exporter types can be added without service rewrite

## Bonus Feature 2: Smart Admin Insights (Rule-Based)

What it does:
- generates actionable text insights from current data

Examples:
- revenue snapshot
- top category by units sold
- low-stock watch list
- review sentiment summary
- fulfillment queue count

Architecture:
- contract: `IInsightsService`
- implementation: `InsightsService`
- presentation: `ReportDisplayHelper` and admin menu route

Why heuristic instead of external AI:
- deterministic, offline-safe, and demo-friendly
- no network dependency risk
- logic remains explainable in viva

## Bonus Feature 3: Customer Recommendations

What it does:
- recommends active, in-stock, not-yet-purchased products
- prioritizes categories from past purchases
- uses average rating/stock/name tie-breakers
- displays "why" reason for each recommendation

Architecture:
- produced by `InsightsService.GetCustomerRecommendations`
- rendered by `ProductDisplayHelper.ShowRecommendations`

Why this is valuable:
- user-facing personalization above baseline
- still modular and easy to test

## Baseline Safety Guarantees

Bonus implementation does not:
- move business rules into menus
- expose GUIDs to users
- bypass repository/service boundaries
- alter mandatory baseline workflow semantics

## Tests for Bonus Features

Coverage files:
- `Application/BonusFeaturesServiceTests.cs`
- `Infrastructure/PdfReportExporterTests.cs`

Covered outcomes:
- recommendation filtering/ranking correctness
- insight generation and validation behavior
- export orchestration contract behavior
- PDF output creation and guard behavior

## Demo Script for Bonus Value

1. complete baseline flow quickly (auth -> cart -> checkout).
2. open admin insights and explain each line as actionable operations data.
3. export report PDF and show output location.
4. open customer recommendations and explain reason text.
5. conclude: baseline stayed stable while bonus delivered extra value.

## Future Bonus Extensions (Optional)

Low-risk candidates:
- CSV exporter via another `IReportExporter` implementation
- audit event log repository + admin viewer
- advanced filter query service for catalog exploration

## Quick Viva Script

"Bonus features were implemented as modular extensions, not baseline rewrites. Export uses an interface-backed adapter, insights are deterministic and offline-safe, and recommendations are rule-based and explainable. This improves demo impact without destabilizing required behavior."
