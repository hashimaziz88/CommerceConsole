# Order Lifecycle: History, Tracking, and Status Transitions

## Purpose

This document explains order visibility and lifecycle control for customers and administrators, including transition governance.

## Architecture Ownership

Presentation:
- show history/tracking screens
- collect selection and confirmation inputs

Application:
- provide ordered views
- enforce transition rules

Domain:
- hold current order status and update operation

Infrastructure:
- persist updated order state

## Customer Capabilities

## 1. View Order History

Service call:
- `OrderService.GetCustomerOrders(customerId)`

Behavior:
- validates non-empty customer ID
- returns orders sorted by `CreatedAt` descending
- rendered with index-based rows

## 2. Track Specific Order

Flow:
1. customer views selectable order list by index
2. selects one order
3. tracking view displays status, payment status, total, placed time

No GUIDs are shown.

## Administrator Capabilities

## 1. View All Orders

Service call:
- `OrderService.GetAllOrders()`

Behavior:
- returns all orders sorted newest first

## 2. Update Order Status

Flow:
1. admin selects order by index
2. service returns allowed next statuses for current state
3. UI only displays valid options
4. admin confirms and applies update

Result:
- status change persisted to repository

## Transition Rules (Centralized)

Policy source:
- `OrderService` transition map

Allowed transitions:
- `Pending -> Paid | Cancelled`
- `Paid -> Processing | Cancelled`
- `Processing -> Shipped | Cancelled`
- `Shipped -> Delivered`
- `Delivered ->` terminal
- `Cancelled ->` terminal

Why centralized policy is useful now:
- single source of truth
- easy to test
- menu remains free of business transition logic

## Terminal State Behavior

For terminal states (`Delivered`, `Cancelled`):
- no further transitions are allowed
- UI warns admin and stops flow

## Validation and Failure Modes

- empty order ID -> `ValidationException`
- missing order -> `NotFoundException`
- invalid transition -> `ValidationException`

Behavioral note:
- no-op when target status equals current status

## Future Refactor Seam

Current approach:
- transition rules are maintained in one service-level policy map

Future direction:
- transition handling can be extracted into dedicated transition components if lifecycle complexity grows

## Tests

Primary file:
- `Tests/CommerceConsole.Tests/Application/OrderStatusTransitionTests.cs`

Covered scenarios:
- valid progression sequence
- invalid jump rejection
- terminal-state transition rejection


