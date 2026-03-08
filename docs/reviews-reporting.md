# Reviews and Reporting

## Scope

This document explains Prompt 7 implementation for:
- customer product reviews
- review rating validation
- purchase eligibility validation for reviews
- average rating display
- administrator sales reporting

It also records Prompt 11 bonus additions:
- smart heuristic insights
- PDF sales-report export

## Review Workflow

Customer capability:
- submit a product review with rating and comment from `CustomerMenu`
- review selection menu only lists products previously purchased by the current customer
- if no purchased products are reviewable, the menu exits with a friendly message

Service:
- `ReviewService.AddReview(Customer customer, Guid productId, int rating, string comment)`

Validation:
- customer must have purchased the product before reviewing
- `rating` must be between `1` and `5` (enforced by `Review` domain entity)
- product must exist (`NotFoundException`)
- customer must be valid (`ValidationException`)

Purchase eligibility rule:
- review eligibility is derived from customer order history in `IOrderRepository`
- only orders in customer-paid lifecycle states are considered valid purchases
- users cannot review products they never bought

Persistence:
- product reviews are persisted through `InMemoryProductRepository.Update(...)`
- review is also attached to the in-memory customer review collection

## Average Rating

Average ratings are calculated with LINQ:
- `ProductDisplayHelper` uses `Average(review => review.Rating)`
- `ReviewService.GetAverageRating(...)` also provides service-level calculation

Display behavior:
- customer and admin product views show average rating per product

## Sales Reporting Workflow

Administrator capability:
- view consolidated sales report from `AdminMenu`

Service:
- `ReportService`

Report includes:
- total revenue
- orders grouped by status
- best-selling products
- low-stock products

### Report Definitions

1. Total revenue
- Definition: sum of all `Order.TotalAmount`
- LINQ: `Sum(order => order.TotalAmount)`

2. Orders by status
- Definition: count of orders per `OrderStatus`
- LINQ: `GroupBy(order => order.Status)` + `ToDictionary(...)`
- includes zero-count statuses for complete visibility

3. Best-selling products
- Definition: products ranked by total quantity sold (then revenue)
- LINQ:
- flatten order items with `SelectMany`
- group by product with `GroupBy`
- aggregate quantity/revenue with `Sum`
- rank with `OrderByDescending`/`ThenByDescending`

4. Low-stock products
- Definition: products with `StockQuantity <= threshold`
- LINQ: `Where` + `OrderBy` + `ThenBy` + projection to report rows

## Bonus: Smart Insights

Admin bonus capability:
- view rule-based operational insights from the reporting section

Service:
- `InsightsService.GetAdminInsights(int lowStockThreshold)`

Insight examples:
- revenue snapshot
- top category by units sold
- restock watch summary
- review sentiment summary
- fulfillment alert for paid/processing orders

Design choice:
- this is heuristic and local (no external AI dependency)
- it remains deterministic and demo-friendly in offline environments

## Bonus: PDF Report Export

Admin bonus capability:
- export a PDF version of the report from the reporting section

Architecture:
- orchestration: `ReportExportService`
- exporter abstraction: `IReportExporter`
- concrete implementation: `PdfReportExporter`

Export content:
- title and generation timestamp
- total revenue
- order counts by status
- best-selling products
- low-stock products

Constraints:
- report aggregation remains in `ReportService`
- export formatting logic stays outside domain entities

## Example Output Interpretation

Given historical orders:
- Order A: Laptop x2, Mouse x1
- Order B: Laptop x1, Mouse x1

Report interpretation:
- total revenue = sum of both order totals
- best-seller ranking = Laptop first (qty 3), Mouse second (qty 2)
- order status counts reflect current stored statuses

## Key Files

- `Application/Services/ReviewService.cs`
- `Application/Services/ReportService.cs`
- `Application/Services/InsightsService.cs` (bonus)
- `Application/Services/ReportExportService.cs` (bonus)
- `Application/Interfaces/IReviewService.cs`
- `Application/Interfaces/IReportService.cs`
- `Application/Interfaces/IInsightsService.cs` (bonus)
- `Application/Interfaces/IReportExporter.cs` (bonus)
- `Infrastructure/Export/PdfReportExporter.cs` (bonus)
- `Application/Models/ProductSalesReportItem.cs`
- `Application/Models/LowStockReportItem.cs`
- `Application/Models/ProductRecommendationItem.cs` (bonus)
- `Application/Models/SalesReportSnapshot.cs` (bonus)
- `Presentation/Menus/CustomerMenu.cs`
- `Presentation/Menus/AdminMenu.cs`
- `Presentation/Helpers/ReportDisplayHelper.cs`

## Test Coverage

Covered in:
- `Tests/CommerceConsole.Tests/Application/ReviewAndReportServiceTests.cs`
- `Tests/CommerceConsole.Tests/Application/BonusFeaturesServiceTests.cs` (bonus)
- `Tests/CommerceConsole.Tests/Infrastructure/PdfReportExporterTests.cs` (bonus)

Scenarios:
- valid review add for purchased product + persistence + average rating
- blocked review for unpurchased product
- invalid review rating rejection
- revenue calculation
- orders-by-status calculation
- best-selling aggregation/ranking
- low-stock filtering/sorting
- bonus recommendation filtering/ranking
- bonus admin insight generation
- bonus PDF export orchestration and file output
