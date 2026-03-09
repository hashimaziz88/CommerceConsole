# Product Catalog and Inventory Rules

## Purpose

This document explains product browsing/search for customers and catalog management for administrators, including validation, low-stock behavior, persistence, and UX conventions.

## Architecture Ownership

Presentation:
- shows product lists and selection menus
- uses index-based selection (no GUID input)

Application:
- `ProductService` enforces use-case level logic
- sorting/filtering orchestration and rule checks

Domain:
- `Product` entity enforces invariants (name/category required, no negative price/stock)

Infrastructure:
- product repository persists updates to JSON

## Customer Capabilities

## 1. Browse Active Products

Service call:
- `ProductService.GetActiveProducts()`

Behavior:
- returns only `IsActive == true`
- sorts by name
- rendered via paged helper for large catalogs

UX details:
- active/inactive and low-stock markers are visible
- average rating shown per product row

## 2. Search By Name or Category

Service call:
- `ProductService.SearchProducts(term)`

Behavior:
- empty term falls back to active browse results
- repository search matches name/category case-insensitively
- results still filtered to active products
- sorted by product name

LINQ intent:
- express user question "find matching products" using concise filter logic

## Administrator Capabilities

## 1. Add Product

Menu inputs:
- name, description, category, price, opening stock

Validation sources:
- domain constructor blocks invalid values
- menu input helpers enforce numeric types/ranges

Persistence:
- repository `Add` writes-through to `products.json`

## 2. Update Product

Selection:
- admin selects from numbered list

Behavior:
- service resolves product by ID internally
- domain mutator `UpdateDetails` applies validation
- repository persists updated entity

## 3. Delete Product

Selection + confirmation:
- admin selects index
- confirmation prompt required

Behavior:
- service confirms existence and removes item
- repository persists removal

## 4. Restock Product

Selection:
- admin selects by index

Behavior:
- positive quantity required
- `Product.Restock` updates stock with validation
- repository persists immediately

## 5. View Products and Low-Stock View

View products:
- full catalog list for admin oversight

Low-stock view:
- threshold input required (non-negative)
- service returns items with `StockQuantity <= threshold`
- sorted by stock ascending, then name

## Validation Model

Where validation occurs:
- presentation: input parsing and range loops
- application: threshold/entity existence checks
- domain: invariant checks for entity state

Why layered validation is intentional:
- input hygiene at boundary
- business correctness near state
- workflow safety in orchestration

## Index-Based UX and No-ID Exposure

Internal reality:
- products have GUID identifiers

User experience:
- users/admins choose by index from rendered lists
- no GUIDs shown or typed

Why this matters:
- less user error
- clearer operational flow
- avoids exposing internal identifiers

## Seed Catalog and Scalability

Seed strategy:
- expanded starter catalog across multiple categories
- idempotent insert by product name

Rendering strategy for larger sets:
- pagination (`ItemsPerPage = 6`)
- global numbering across pages for consistent selection

## Persistence Behavior

Mutable operations persisted:
- add
- update
- delete
- restock

File:
- `data/products.json`

## Test Coverage

Main test files:
- `Tests/CommerceConsole.Tests/Application/ProductServiceTests.cs`
- `Tests/CommerceConsole.Tests/Infrastructure/JsonPersistenceTests.cs`
- `Tests/CommerceConsole.Tests/Presentation/ProductDisplayHelperTests.cs`

Representative scenarios:
- add/update/delete/restock correctness
- search/filter behavior
- low-stock threshold behavior
- idempotent reseeding
- paginated rendering and global index visibility


