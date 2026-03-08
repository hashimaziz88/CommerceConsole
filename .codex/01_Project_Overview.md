# 01. Project Overview

## Project summary

CommerceConsole is a C# console-based Online Shopping Backend System built with strict layering and strong OOP boundaries.

Delivery state in this branch:
- baseline features implemented
- bonus features implemented
- Submission 2 pattern refactor implemented

## Scope summary

### Customer-facing capabilities
- register and login
- browse and search active products
- add/update/remove cart items
- wallet balance and wallet top-up
- wallet checkout and order creation
- order history and status tracking
- purchased-only product reviews
- recommendation suggestions (bonus)

### Administrator-facing capabilities
- add/update/delete/restock products
- view catalog and low-stock products
- view all orders and update statuses
- sales reports (revenue, status counts, best sellers, low stock)
- smart insights (bonus)
- PDF sales report export (bonus)

## Core architecture expectations

- clear Presentation/Application/Domain/Infrastructure boundaries
- no business logic in menus
- no repository calls from menus
- index-based UX (no GUID entry in user-facing screens)
- JSON persistence for mutable runtime state
- docs and tests synchronized with implementation

## Pattern implementation status

### Previously present
- Repository
- Service Layer
- Constructor Injection
- Composition Root
- Data Mapper
- Rich Domain Model
- Guard Clauses
- Session Context
- export strategy seam

### Added in Submission 2 refactor
- Factory (role/workspace resolution)
- Strategy (payment processing formalization)
- State-style order transition policies
- Command menu dispatch
- Specification pattern + repository `Find(spec)` formalization

## Success definition

A successful delivery in this branch means:
- required workflows still behave correctly
- pattern seams are explicit and test-covered
- architecture boundaries remain intact
- docs/codex provide viva-ready explanation of before/after design
