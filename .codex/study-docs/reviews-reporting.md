# Reviews and Reporting

## Purpose

This document explains:
- purchased-only review flow
- rating validation
- average-rating display
- admin reporting aggregates via LINQ
- bonus insight and export alignment

## Review Workflow

## Business Rule: Review Only What You Bought

Enforcement location:
- `ReviewService`

How it works:
1. service gathers customer orders from repository
2. only purchase-eligible statuses are considered (`Paid`, `Processing`, `Shipped`, `Delivered`)
3. product IDs from those orders become review-eligible set
4. customer review menu displays only that filtered set

Result:
- users are not even shown non-purchased products to review
- rule is enforced in both selection list and add-review validation

## Add Review Flow

Inputs:
- selected purchased product
- rating (1-5)
- comment

Validation:
- customer required
- product must exist
- purchase eligibility must be true
- rating must be within 1..5 (domain invariant in `Review`)

Persistence effects:
- review is added to product review collection and product persisted
- review is added to customer review list and user persisted

## Average Rating

Displayed in product views.

Calculation:
- average of `Review.Rating`
- if no reviews, defaults to `0`

Used in:
- `ProductDisplayHelper` rows
- recommendation ranking tie-breaks

## Reporting Workflow

Service:
- `ReportService`

Report outputs:
1. total revenue
2. orders by status
3. best-selling products
4. low-stock products

## LINQ Reasoning by Report Type

## Total Revenue

Question answered:
- "How much money has the system processed?"

Query shape:
- `Sum(order => order.TotalAmount)`

## Orders By Status

Question answered:
- "How many orders are in each lifecycle stage?"

Query shape:
- `GroupBy(order => order.Status)` + dictionary projection
- fills missing enum states with zero for complete dashboard visibility

## Best-Selling Products

Question answered:
- "Which products sold the highest quantity and generated most revenue?"

Query shape:
- flatten with `SelectMany(order => order.Items)`
- group by product ID
- aggregate quantity and revenue with `Sum`
- rank by quantity desc, revenue desc, name asc

## Low-Stock Products

Question answered:
- "What needs restocking soon?"

Query shape:
- `Where(stock <= threshold)`
- sorted by stock asc then name

## Bonus Alignment

## Smart Insights (Heuristic, Not External AI)

Service:
- `InsightsService.GetAdminInsights(...)`

Examples produced:
- revenue snapshot
- top category by units sold
- restock watch list
- review sentiment summary
- fulfillment queue count

Design note:
- deterministic rule-based insights
- works offline
- clearly traceable in architecture review

## PDF Export

Orchestration:
- `ReportExportService`

Abstraction:
- `IReportExporter`

Concrete implementation:
- `PdfReportExporter`

Why this separation is good:
- aggregation remains in `ReportService`
- formatting/export stays outside domain/service aggregation core

## Test Coverage

Main files:
- `Tests/CommerceConsole.Tests/Application/ReviewAndReportServiceTests.cs`
- `Tests/CommerceConsole.Tests/Application/BonusFeaturesServiceTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/PdfReportExporterTests.cs`

Covered themes:
- purchased-only review eligibility
- rating invariants and rejection paths
- revenue/status/best-seller/low-stock correctness
- recommendation and insight behavior
- PDF export output/validation


