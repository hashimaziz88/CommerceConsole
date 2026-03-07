# 08. Codex Prompt Pack

Use the following prompts with ChatGPT Codex or any coding agent. Paste one prompt at a time.

Prompt-to-issue mapping:
- Prompt 1 -> Issue 1
- Prompt 2 -> Issue 2
- Prompt 3 -> Issue 3
- Prompt 4 -> Issue 4
- Prompt 5 -> Issue 5
- Prompt 6 -> Issue 6
- Prompt 7 -> Issue 7
- Prompt 8 -> Issue 8
- Prompt 9 -> Issue 9

## Prompt 1 - Bootstrap clean solution architecture with tests and docs scaffolding (DONE) feature/01-solution-boootstrap

```text
Create a C# console application called CommerceConsole for an Online Shopping Backend System.

Use this architecture:
- Presentation layer for menus and console interaction
- Application layer for services and interfaces
- Domain layer for entities, enums, and exceptions
- Infrastructure layer for in-memory repositories and seed data
- Tests project for domain and application coverage
- docs folder for architecture and test plan notes

Create folders and starter files for:
- Domain/Entities: User, Customer, Administrator, Product, Cart, CartItem, Order, OrderItem, Payment, Review
- Domain/Enums: UserRole, OrderStatus, PaymentStatus
- Domain/Exceptions: ValidationException, NotFoundException, AuthenticationException, InsufficientFundsException, InsufficientStockException, DuplicateEmailException
- Application/Interfaces: IRepository, IUserRepository, IProductRepository, IOrderRepository, IAuthService, IProductService, ICartService, IOrderService, IWalletService, IReviewService, IReportService
- Application/Services skeleton implementations
- Infrastructure/Repositories in-memory implementations
- Infrastructure/Data/SeedData
- Presentation/Menus for MainMenu, CustomerMenu, AdminMenu
- Tests/Domain and Tests/Application starter test files

Rules:
- Keep Program.cs thin
- Use namespaces consistently
- Use comments on classes and public methods
- Keep classes small and responsibilities clear
- Use guard clauses and meaningful names
- Add short starter notes in docs/architecture.md and docs/test-plan.md
- Add short starter notes in docs/oop-design-notes.md covering access modifiers, static usage, polymorphism, SoC, and inheritance choices
```

## Prompt 2 - Implement registration, login, and role-based navigation with tests feature/02-auth-and-roles

```text
Implement registration and login for the CommerceConsole project.

Requirements:
- Register new customers
- Allow login for both customers and administrators
- Seed one admin user in SeedData
- Enforce unique email addresses
- Add current session tracking
- Route users to customer or admin menus based on role
- Keep business logic in services, not in Program.cs
- Use custom exceptions for validation and authentication failures

Also:
- Add service tests for registration/login scenarios
- Update docs with auth flow and assumptions

Return code for required services, repositories, menu integration, and tests.
```

## Prompt 3 - Build product browsing, searching, and admin catalog management

```text
Implement product browsing, searching, and administrator product management.

Customer features:
- Browse active products
- Search by name or category using LINQ

Admin features:
- Add product
- Update product
- Delete product
- Restock product
- View products
- View low stock products using LINQ

Rules:
- Keep validation centralized
- Avoid duplicated console formatting code
- Use in-memory repositories
- Keep methods short and readable

Also:
- Add tests for product validation, search, and catalog mutations
- Update docs with product rules and low-stock behavior
```

## Prompt 4 - Implement shopping cart and wallet workflows

```text
Implement cart and wallet workflows for the CommerceConsole project.

Requirements:
- Add to cart
- View cart
- Update cart quantity
- Remove cart items if quantity becomes zero
- Validate cart quantities against stock rules
- View wallet balance
- Add wallet funds

Rules:
- Keep presentation layer thin
- Use custom exceptions for invalid operations
- Keep cart and wallet logic in services/domain, not menus

Also:
- Add tests for cart mutations and wallet top-up validation
- Update docs with cart and wallet behavior
```

## Prompt 5 - Add checkout, payments, and order processing

```text
Implement checkout, payment simulation, and order processing for the CommerceConsole project.

Requirements:
- Checkout with wallet-only payment
- Validate stock before checkout
- Validate wallet balance before checkout
- Reduce stock on successful checkout
- Create payment and order records
- Ensure order stores snapshot-based order items
- Clear cart after successful checkout

Architecture rules:
- Checkout orchestration must live in OrderService
- Use custom exceptions
- Keep presentation layer thin

Also:
- Add tests for checkout happy path and failure cases
- Update docs with checkout behavior and invariants
```

## Prompt 6 - Implement order history, tracking, and admin order status updates

```text
Implement order management views and status updates for the CommerceConsole project.

Requirements:
- Customer can view order history
- Customer can track order statuses
- Admin can view all orders
- Admin can update order statuses
- Enforce valid status transitions

Rules:
- Keep transition rules centralized
- Keep menu handlers thin and focused on routing/display

Also:
- Add tests for valid and invalid order status transitions
- Update docs with order lifecycle rules
```

## Prompt 7 - Add reviews, reporting, and documentation alignment

```text
Implement product review and reporting features for the CommerceConsole project.

Requirements:
- Customers can add reviews with rating and comment
- Review rating must be between 1 and 5
- Show average product rating using LINQ
- Generate sales reports for admin using LINQ
- Include total revenue, orders by status, best-selling products, and low-stock products

Also:
- Add tests for review validation and report calculations
- Update docs with report definitions and examples

Keep code modular and ready for future strategy extraction.
```

## Prompt 8 - Complete quality hardening (validation, exceptions, UX, tests, docs)

```text
Perform a quality-hardening pass on CommerceConsole.

Focus areas:
- Input validation coverage across all menus
- Friendly exception handling at presentation boundaries
- Reusable console helpers and consistent UX messaging
- Removal of duplicated logic
- Method/class readability and naming quality
- Regression test additions for critical workflows
- Documentation drift fixes versus implementation

Then provide a focused improvement patch with code, tests, and docs updates.
```

## Prompt 9 - Monday design-pattern implementation (no feature expansion)

```text
Implement design patterns in the CommerceConsole project on top of existing behavior.

Patterns to introduce:
- Factory Pattern for role-based user or menu creation
- Strategy Pattern for payment processing and optionally reporting
- Repository Pattern formalization if needed
- Lightweight State-style order status transition handling

Constraints:
- Preserve existing functionality
- Keep console interface unchanged where possible
- Do not introduce new business features
- Add pattern-focused tests
- Update docs to explain where and why each pattern is used
- Do not over-engineer the solution
```

