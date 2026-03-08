# Product Catalog and Inventory Rules

## Scope

This document explains Prompt 3 implementation for:
- customer product browsing
- customer product search
- administrator product management
- low-stock reporting behavior
- larger seeded catalog UX handling

## Customer Features

### Browse active products

- Customer menu option: `Browse Active Products`
- Data source: `IProductService.GetActiveProducts()`
- Visibility rule: only `IsActive == true` products are shown
- Output includes name, category, price, stock, status, and average rating
- Internal identifiers are never displayed in presentation output

### Search by name/category

- Customer menu option: `Search Products`
- Data source: `IProductService.SearchProducts(term)`
- Search implementation uses LINQ via repository search + active-product filtering
- Empty search term falls back to active product list
- UX hint guides users to narrow large catalogs quickly

## Administrator Features

### Add product

Menu action collects:
- name
- description
- category
- price
- initial stock

Validation is centralized in domain constructor (`Product`) and surfaced through `ValidationException`.

### Update product

Menu action uses numbered product selection + updated fields.

Behavior:
- product must exist (`NotFoundException` if missing)
- details updated through `Product.UpdateDetails(...)`
- repository persists update to JSON

### Delete product

Behavior:
- product is selected by number from the product list
- product must exist (`NotFoundException` if missing)
- repository removes product row
- persisted file is updated immediately

### Restock product

Behavior:
- product is selected by number from the product list
- product must exist
- restock quantity must be positive
- `Product.Restock(...)` applies guard clause
- repository persists updated stock

### View products

Shows all products for admin oversight, including active/inactive state and stock.

### View low-stock products

- Admin provides threshold
- service validates threshold is non-negative
- repository returns products with `StockQuantity <= threshold`
- results are sorted by stock ascending then name

## Larger Catalog UX Behavior

When product lists are long:
- `ProductDisplayHelper` renders paged sections (`Page X/Y`)
- numbering remains global across pages to keep selection simple
- selection menus include hints explaining the index usage

This preserves index-based UX and avoids exposing internal IDs.

## Seed Catalog Behavior

`SeedData` now provides a broader starter catalog across multiple categories.

Idempotency rule:
- missing seeded product names are added
- existing product names are not duplicated

This means older data files can be upgraded with new defaults safely on startup.

## Validation Centralization

Validation sources:
- Domain (`Product`) for business invariants
- Application (`ProductService`) for missing entity and threshold checks
- Presentation catches and prints friendly error messages

## Persistence Notes

Any mutable product action (`add`, `update`, `delete`, `restock`) persists to:
- `data/products.json`

## Key Classes

- `Infrastructure/Data/SeedData.cs`
- `Application/Services/ProductService.cs`
- `Presentation/Menus/CustomerMenu.cs`
- `Presentation/Menus/AdminMenu.cs`
- `Presentation/Helpers/ProductDisplayHelper.cs`
- `Infrastructure/Repositories/InMemoryProductRepository.cs`

## Test Coverage

Covered in:
- `Tests/CommerceConsole.Tests/Application/ProductServiceTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/JsonPersistenceTests.cs`
- `Tests/CommerceConsole.Tests/Presentation/ProductDisplayHelperTests.cs`

Coverage includes:
- add validation
- search behavior
- update/delete/restock mutations
- low-stock threshold behavior
- idempotent multi-product seeding behavior
- paged product-view rendering for larger catalogs
