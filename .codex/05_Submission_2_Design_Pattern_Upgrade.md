# 05. Monday Design Pattern Implementation Plan

## Goal

Implement design patterns on Monday (2026-03-09) after the baseline system is already feature-complete, tested, and documented.

This is a targeted architectural enhancement, not a rewrite and not a catch-up phase.

## Pattern choices and scope

### 1. Factory Pattern
Use for creating role-based users or menu handlers.

**Where to apply**
- create `Customer` or `Administrator` based on role
- optionally create menu objects by role

**Why it helps**
- removes role-based object construction from UI code
- centralizes object creation decisions

### 2. Strategy Pattern
Use for pluggable behaviors.

**Best choices**
- payment processing strategy
- report generation strategy
- optional product search strategy

**Suggested first implementation**
Start with payment:
- `IPaymentStrategy`
- `WalletPaymentStrategy`

### 3. State-style transition handling
Use for order status transitions.

**Why it fits**
Order statuses naturally move through controlled stages such as Pending, Paid, Processing, Shipped, Delivered, and Cancelled.

**How to scope safely**
A lightweight transition policy is enough; avoid framework-heavy state engines.

### 4. Repository formalization
If not already complete, tighten repository contracts and dependency injection boundaries.

### 5. Builder Pattern (optional)
Only if it clearly improves report result construction or complex seeded object creation.

## Monday execution sequence

### Step 1
Freeze baseline behavior.
- run regression tests
- capture expected outputs for critical flows

### Step 2
Introduce pattern interfaces around selected variation points.
- payment
- report generation
- role-based creation

### Step 3
Move branching logic into factories and strategies.

### Step 4
Introduce explicit order status transition rules.

### Step 5
Add pattern-focused tests and update docs.
- tests prove behavior parity and new extensibility seams
- docs explain pattern intent, boundaries, and trade-offs

## Non-goals for Monday

- adding missing core features
- catching up missing baseline tests
- rewriting menu UX
- introducing unnecessary complexity

## Recommended pattern story for presentation

- Factory centralizes role-based object creation.
- Strategy makes payment and report behavior replaceable without changing core workflows.
- Repository abstractions isolate storage concerns.
- State-style transition handling improves correctness around order status changes.

## Suggested interfaces

```csharp
public interface IPaymentStrategy
{
    Payment Process(Customer customer, decimal amount);
}

public interface IUserFactory
{
    User Create(string fullName, string email, string password, UserRole role);
}

public interface IReportStrategy<T>
{
    T Generate(List<Order> orders, List<Product> products);
}
```

## Verification checklist

- existing user-visible flows remain unchanged
- new pattern abstractions are covered by tests
- no business rules leaked into menu classes
- docs include an architecture delta for pattern changes
