# 08. Codex Prompt Pack

Use the following prompts with ChatGPT Codex or any coding agent. Paste one prompt at a time.

## Prompt 1 - Generate the solution skeleton

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
- Add a short architecture note in docs/architecture.md
```

## Prompt 2 - Implement domain entities cleanly

```text
Implement the domain entities and enums for the CommerceConsole project.

Requirements:
- Use OOP properly
- Apply validation in constructors or mutator methods where appropriate
- Product must prevent negative price and stock
- Cart must support add/update/remove/clear/calculate total
- Order must capture snapshot-based order items
- Review rating must be between 1 and 5
- Customer should include wallet balance, cart, and order history

Also:
- Add unit tests for domain invariants
- Update docs/test-plan.md with what is covered

Do not add console input/output in domain classes.
Provide clean, compilable C# code.
```

## Prompt 3 - Build authentication and role-based navigation

```text
Implement registration and login for the CommerceConsole project.

Requirements:
- Register new customers
- Allow login for both customers and administrators
- Seed one admin user in SeedData
- Enforce unique email addresses
- Add current session tracking
- Route users to customer or admin menus based on role
- Keep all business logic in services, not in Program.cs
- Use custom exceptions for validation and authentication failures

Also:
- Add service tests for registration/login scenarios
- Update docs with auth flow and assumptions

Return code for required services, repositories, menu integration, and tests.
```

## Prompt 4 - Build catalog and admin product management

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
- Add tests for product search and catalog mutations
- Update docs with product rules and low-stock behavior
```

## Prompt 5 - Build cart, wallet, checkout, and orders

```text
Implement cart, wallet, checkout, and order workflows for the CommerceConsole project.

Requirements:
- Add to cart
- View cart
- Update cart quantity
- Remove cart items if quantity becomes zero
- View wallet balance
- Add wallet funds
- Checkout with wallet-only payment
- Validate stock before checkout
- Validate wallet balance before checkout
- Reduce stock on successful checkout
- Create payment and order records
- Clear cart after successful checkout
- View order history
- Track orders
- Admin can view all orders and update order status

Architecture rules:
- Checkout orchestration must live in OrderService
- Use custom exceptions
- Keep presentation layer thin

Also:
- Add tests for happy path and failure cases
- Update docs with checkout and order transition behavior
```

## Prompt 6 - Build reviews and reporting

```text
Implement product review and reporting features for the CommerceConsole project.

Requirements:
- Customers can add reviews with rating and comment
- Show average product rating using LINQ
- Generate sales reports for admin using LINQ
- Include total revenue, orders by status, best-selling products, and low-stock products

Also:
- Add tests for review validation and report calculations
- Update docs with report definitions and examples

Keep code modular and ready for future strategy extraction.
```

## Prompt 7 - Monday design pattern implementation

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

## Prompt 8 - Final quality verification

```text
Review the CommerceConsole project for rubric alignment and maintainability.

Check for:
- Long methods
- Long classes
- Duplicated logic
- Missing validation
- Missing exception handling
- Weak naming
- Poor separation of concerns
- Missing/weak tests for core workflows
- Documentation drift versus implementation

Then provide a focused improvement patch with code, tests, and docs updates.
```
