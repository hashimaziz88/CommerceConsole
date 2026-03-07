# Order History, Tracking, and Status Lifecycle

## Scope

This document explains Prompt 6 implementation for:
- customer order history view
- customer order status tracking
- administrator all-order visibility
- administrator status updates with enforced transition rules

## Customer Order Management

Customer capabilities:
- view order history (`CustomerMenu` -> `View Order History`)
- track one order status (`CustomerMenu` -> `Track Order Status`)

Data source:
- `OrderService.GetCustomerOrders(customerId)`

Presentation behavior:
- orders are shown with index-based selection
- internal identifiers are not displayed
- tracking view shows current order/payment status, totals, item count, and placed time

## Administrator Order Management

Administrator capabilities:
- view all orders (`AdminMenu` -> `View All Orders`)
- update order status (`AdminMenu` -> `Update Order Status`)

Data source:
- `OrderService.GetAllOrders()`

Update behavior:
- admin selects order by index
- menu shows only allowed next statuses for the selected order
- chosen status is applied through `OrderService.UpdateOrderStatus(...)`

## Centralized Transition Rules

Transition rules are centralized in:
- `Application/Services/OrderService.cs`

Allowed transitions:
- `Pending` -> `Paid`, `Cancelled`
- `Paid` -> `Processing`, `Cancelled`
- `Processing` -> `Shipped`, `Cancelled`
- `Shipped` -> `Delivered`
- `Delivered` -> terminal (no further transitions)
- `Cancelled` -> terminal (no further transitions)

Rule enforcement:
- invalid transitions throw `ValidationException`
- missing orders throw `NotFoundException`

## Lifecycle Invariants

- terminal statuses (`Delivered`, `Cancelled`) cannot transition further
- status transitions are deterministic and validated in one place
- menus do not implement transition logic directly

## Thin Menu Design

Menu handlers only:
- route to service methods
- display lists and status summaries
- handle exceptions with user-friendly messages

Business logic stays in application/domain layers.

## Test Coverage

Covered in:
- `Tests/CommerceConsole.Tests/Application/OrderStatusTransitionTests.cs`

Scenarios:
- valid transition sequence (`Paid` -> `Processing` -> `Shipped` -> `Delivered`)
- invalid transition rejection (`Paid` -> `Delivered`)
- terminal-state transition rejection (`Cancelled` -> `Processing`)
