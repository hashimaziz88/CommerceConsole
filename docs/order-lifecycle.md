# Order Lifecycle: History, Tracking, and Status Transitions

## Purpose

This document explains order visibility and lifecycle control for customers and administrators, including transition governance.

## Architecture Ownership

Presentation:
- show history/tracking screens
- collect selection and confirmation inputs

Application:
- provide ordered views
- enforce status transition policy

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

## Transition Policy (Centralized)

Policy source:
- `OrderService` static transition map

Allowed transitions:
- `Pending -> Paid | Cancelled`
- `Paid -> Processing | Cancelled`
- `Processing -> Shipped | Cancelled`
- `Shipped -> Delivered`
- `Delivered ->` terminal
- `Cancelled ->` terminal

Why centralized policy is good now:
- single source of truth
- easy to test
- menu remains free of business-state logic

## Terminal-State Behavior

For terminal states (`Delivered`, `Cancelled`):
- no further transitions are allowed
- UI warns admin and stops flow

## Validation and Failure Modes

- empty order ID -> `ValidationException`
- missing order -> `NotFoundException`
- invalid transition -> `ValidationException`

Behavioral note:
- no-op when target status equals current status

## Why This Is Not Yet Full State Pattern

Current approach:
- transition map dictionary in service

Why acceptable now:
- small lifecycle with clear progression
- simpler for baseline scope

Future upgrade path:
- extract `IOrderStateTransitionPolicy`/state objects per status
- keep same menu/service contract with internal refactor

## Tests

Primary file:
- `Tests/CommerceConsole.Tests/Application/OrderStatusTransitionTests.cs`

Covered scenarios:
- valid progression sequence
- invalid jump rejection
- terminal-state transition rejection

## Quick Viva Script

"Order lifecycle rules are centralized in `OrderService` so policy is consistent and testable. Admin UI only shows allowed next statuses, preventing invalid updates at source. Customers only track orders through read-only views."
