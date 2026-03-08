# 05. Submission 2 Design Pattern Upgrade

## Goal (Monday 2026-03-09)

Implement explicit extension patterns on top of stable baseline behavior.

Status in this branch: **implemented**.

## Implemented pattern set

### 1. Factory Pattern

Implemented:
- `IUserWorkspace`
- `IRoleMenuFactory`
- `RoleMenuFactory`

Applied to:
- role-based workspace routing in `MainMenu`

### 2. Strategy Pattern

Implemented:
- `IPaymentStrategy`
- `WalletPaymentStrategy`
- existing exporter strategy seam retained (`IReportExporter`)

Applied to:
- checkout payment step in `OrderService`

### 3. State-style transition handling

Implemented:
- `IOrderTransitionState`
- per-status transition state classes
- `IOrderTransitionStateFactory`
- `OrderTransitionStateFactory`

Applied to:
- order status validation in `OrderService`

### 4. Command Pattern (extra)

Implemented:
- `IMenuCommand`
- `DelegateMenuCommand`
- `MenuCommandDispatcher`

Applied to:
- main, customer, and admin menu action dispatch

### 5. Specification Pattern + repo formalization (extra)

Implemented:
- domain `ISpecification<T>` and product specifications
- repository contract `Find(spec)`
- all in-memory repositories implement `Find(spec)`

Applied to:
- product/report/insight filtering paths

## Behavior parity outcomes

Preserved:
- business feature scope
- role/menu capabilities
- no GUID exposure policy
- persistence contracts

Allowed minor changes:
- dispatch/routing internals and minor messaging consistency

## Verification

- Full test suite passes after refactor.
- Pattern-focused tests added for Factory, Strategy, State, Command, and Specification.

## Anti-goals respected

- no feature expansion beyond agreed scope
- no menu-to-repository leakage
- no architectural boundary regressions
