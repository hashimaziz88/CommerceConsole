# 01. Project Overview

## Project summary

The project is a C# console-based online shopping backend simulator. It must support both Customer and Administrator roles and demonstrate object-oriented design, LINQ usage, validation, exception handling, testability, and clean modular structure.

## Core functional requirements

### Customer-facing
- Register
- Login
- Browse products
- Search products
- Add product to cart
- View cart
- Update cart
- Checkout
- View wallet balance
- Add wallet funds
- View order history
- Track orders
- Review products
- Logout

### Administrator-facing
- Add product
- Update product
- Delete product
- Restock product
- View products
- View orders
- Update order status
- View low stock products
- Generate sales reports
- Logout

## Core domain classes required by spec
- `User`
- `Customer : User`
- `Administrator : User`
- `Product`
- `Cart`
- `CartItem`
- `Order`
- `OrderItem`
- `Payment`
- `Review`

## Technical expectations
- Strong object-oriented design
- Inheritance and polymorphism
- Interfaces for important behaviors
- LINQ for querying collections
- `List<T>` and related collection usage
- Input validation and exception handling
- Menu-driven console interface
- Tests and documentation maintained throughout development

## Rubric-driven execution model

### Baseline from day one
1. Implement complete required functionality
2. Keep architecture clean and modular
3. Maintain clear console navigation and UX
4. Use LINQ naturally in search, history, and reporting
5. Apply validation and exception handling early
6. Maintain tests and docs as features are added

### Monday add-on (2026-03-09)
1. Implement design patterns intentionally (Factory, Strategy, State, Repository formalization)
2. Preserve existing behavior while improving extensibility
3. Add pattern-focused tests and docs updates only where pattern behavior is introduced

## Recommended approach

Build the system as Submission 2-ready from the first implementation pass. Monday should be limited to introducing pattern abstractions, not catching up missing features, tests, or documentation.

## Success definition

A strong solution will:
- satisfy every menu item from the specification
- separate domain rules from console UI code
- centralize validation and business workflows
- keep entities focused and service classes small
- use LINQ naturally for searching, filtering, reporting, and order history
- include tests for domain and service behavior from early phases
- keep docs aligned with actual implementation as work progresses
