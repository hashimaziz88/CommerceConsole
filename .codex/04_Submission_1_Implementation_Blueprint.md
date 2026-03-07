# 04. Baseline Implementation Blueprint

## Objective

Deliver the full required system with Submission 2-quality engineering practices from the start:
- complete feature scope
- clean architecture
- tests as work progresses
- documentation kept current

Monday (2026-03-09) is reserved for design pattern implementation only.

## Build order

### Phase 1 - bootstrap and architecture
- Create solution and folder structure
- Add domain entities and enums
- Add custom exceptions
- Add repository interfaces and in-memory implementations
- Add seed data for products and admin account
- Create test project and initial smoke tests
- Create `docs/architecture.md` and `docs/test-plan.md`

### Phase 2 - authentication and role navigation
- Registration flow
- Login flow
- Role-based routing to customer or administrator menus
- Session tracking for current logged-in user
- Add auth service tests
- Update docs with auth flow and assumptions

### Phase 3 - product catalog
- Browse products
- Search products
- Admin add/update/delete/restock products
- View low-stock products
- Add product service tests and repository tests
- Update docs with catalog behavior and validation rules

### Phase 4 - cart and wallet
- Add to cart
- View cart
- Update cart quantity
- Remove from cart
- Add wallet funds
- View balance
- Add cart and wallet tests
- Update docs with cart rules and wallet behavior

### Phase 5 - checkout and orders
- Validate stock
- Validate wallet funds
- Debit wallet
- Create payment record
- Create order record
- Reduce stock
- Clear cart
- Customer order history
- Customer order tracking
- Admin order viewing
- Admin order status updates
- Add order workflow tests (happy path and failure paths)
- Update docs with checkout and status transition rules

### Phase 6 - reviews and reports
- Customer review products
- Product average rating display
- Sales reports using LINQ
- Revenue totals
- Top products
- Low-stock analytics
- Add review/report tests with deterministic data fixtures
- Update docs with report definitions and formulas

### Phase 7 - hardening and release readiness
- Improve validation messages
- Protect against invalid menu input
- Add helper methods for console formatting
- Refactor long methods
- Run full regression test pass
- Complete user-facing and developer docs
- Prepare release branch/tag for pattern day

### Phase 8 - Monday pattern implementation (2026-03-09)
- Implement Factory, Strategy, and State-oriented abstractions
- Keep user-visible behavior unchanged
- Add pattern-focused tests
- Update docs with pattern rationale and trade-offs

## Menu design recommendation

### Main menu
1. Register
2. Login
3. Exit

### Customer menu
1. Browse Products
2. Search Products
3. Add Product to Cart
4. View Cart
5. Update Cart
6. Checkout
7. View Wallet Balance
8. Add Wallet Funds
9. View Order History
10. Track Orders
11. Review Products
12. Logout

### Administrator menu
1. Add Product
2. Update Product
3. Delete Product
4. Restock Product
5. View Products
6. View Orders
7. Update Order Status
8. View Low Stock Products
9. Generate Sales Reports
10. Logout

## Example use-case flow: Checkout

1. Read current customer cart
2. Ensure cart is not empty
3. Re-fetch products for stock verification
4. Validate every cart item against current stock
5. Calculate total amount
6. Ensure wallet balance is sufficient
7. Deduct wallet funds
8. Create payment record with status completed
9. Create order with order item snapshot
10. Reduce product inventory
11. Save changes to repositories
12. Clear cart
13. Show success message and order summary

## Exception strategy

Create custom exceptions such as:
- `ValidationException`
- `AuthenticationException`
- `NotFoundException`
- `InsufficientFundsException`
- `InsufficientStockException`
- `DuplicateEmailException`

Catch them in the presentation layer and show friendly console messages.

## Validation checklist

- no empty strings for names and categories
- valid email format or basic email check
- positive product price
- non-negative stock quantity
- valid numeric menu choices
- valid product ID selection
- quantity must be positive integer
- wallet top-up amount must be positive
- review rating must be 1-5

## Minimum evidence of LINQ

Use LINQ in at least these areas:
- product search
- product sorting
- average product rating
- customer order history
- low-stock report
- sales totals grouped by product

## Baseline quality checklist

Before sign-off confirm:
- every menu item works
- no unhandled crash on bad input
- role separation works correctly
- stock changes after checkout
- wallet balance changes after checkout and top-up
- reports run without errors
- tests cover core domain and service workflows
- docs reflect the actual behavior and architecture
- code is organized by responsibility
- class and method names are clear
