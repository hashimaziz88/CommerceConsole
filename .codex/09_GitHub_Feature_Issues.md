# 09. GitHub Feature Issues

Below are 9 copy-paste-ready feature requests using your template.

Milestone lock policy:
- Issues 1 to 2 must stay in milestone `M1-Foundation-and-Auth`.
- Issues 3 to 5 must stay in milestone `M2-Catalog-Cart-and-Checkout`.
- Issues 6 to 8 must stay in milestone `M3-Orders-Reporting-and-Quality`.
- Issue 9 must stay in milestone `M4-Monday-Patterns-2026-03-09`.
- Do not move issues across milestones without explicit approval.

---
name: Feature request
about: Suggest an idea for this project
title: 'Bootstrap clean solution architecture with tests and docs scaffolding'
labels: 'feature,architecture'
assignees: ''
milestone: 'M1-Foundation-and-Auth'

---

**Is your feature request related to a problem? Please describe.**
The project needs a clean and scalable foundation before any features are implemented. Without a strong structure, business logic may mix into console menus and make testing and documentation harder to maintain.

**Describe the solution you'd like**
Create the initial CommerceConsole structure using Presentation, Application, Domain, and Infrastructure folders. Add starter interfaces, entities, enums, exceptions, repositories, services, seed data, menu classes, a test project, and docs placeholders.

**Describe alternatives you've considered**
A flat structure with all classes in one folder would be faster initially, but it would reduce maintainability and increase rework risk.

**Additional context**
Milestone lock: `M1-Foundation-and-Auth`
Branch suggestion: `feature/01-solution-bootstrap`


---
name: Feature request
about: Suggest an idea for this project
title: 'Implement registration, login, and role-based navigation with tests'
labels: 'feature,auth,test'
assignees: ''
milestone: 'M1-Foundation-and-Auth'

---

**Is your feature request related to a problem? Please describe.**
Users cannot access the system meaningfully until authentication and role separation are in place.

**Describe the solution you'd like**
Implement customer registration, login for both customers and administrators, unique email validation, seeded admin credentials, role-based navigation, and related service tests.

**Describe alternatives you've considered**
A single shared menu for all users would be simpler but does not satisfy role separation requirements.

**Additional context**
Milestone lock: `M1-Foundation-and-Auth`
Branch suggestion: `feature/02-auth-and-roles`


---
name: Feature request
about: Suggest an idea for this project
title: 'Build product browsing, searching, and admin catalog management'
labels: 'feature,products,test'
assignees: ''
milestone: 'M2-Catalog-Cart-and-Checkout'

---

**Is your feature request related to a problem? Please describe.**
The platform cannot function as an online shopping backend without a product catalog and administrative inventory controls.

**Describe the solution you'd like**
Implement customer product browsing and searching using LINQ, plus administrator features to add, update, delete, restock, and view products, including low-stock visibility. Add tests for product validation, search, and stock operations.

**Describe alternatives you've considered**
Read-only product browsing would reduce complexity but omits major admin requirements.

**Additional context**
Milestone lock: `M2-Catalog-Cart-and-Checkout`
Branch suggestion: `feature/03-product-catalog`


---
name: Feature request
about: Suggest an idea for this project
title: 'Implement shopping cart and wallet workflows'
labels: 'feature,cart,payments,test'
assignees: ''
milestone: 'M2-Catalog-Cart-and-Checkout'

---

**Is your feature request related to a problem? Please describe.**
Customers need a cart and funding workflow before checkout can work.

**Describe the solution you'd like**
Implement add-to-cart, view-cart, update-cart, remove-item, cart totals, wallet balance, and wallet top-up flows with validation and tests.

**Describe alternatives you've considered**
A direct buy-now flow is simpler but does not match the required cart-based flow.

**Additional context**
Milestone lock: `M2-Catalog-Cart-and-Checkout`
Branch suggestion: `feature/04-cart-management`


---
name: Feature request
about: Suggest an idea for this project
title: 'Add checkout, payments, and order processing'
labels: 'feature,orders,payments,test'
assignees: ''
milestone: 'M2-Catalog-Cart-and-Checkout'

---

**Is your feature request related to a problem? Please describe.**
The system has no way to complete purchases until checkout and order processing are implemented.

**Describe the solution you'd like**
Implement checkout validation, wallet payment simulation, stock deduction, order creation, order item snapshots, and cart clearing after successful checkout. Add tests for success and failure paths.

**Describe alternatives you've considered**
Multiple payment methods were considered, but wallet-first keeps scope aligned while still allowing strategy extraction later.

**Additional context**
Milestone lock: `M2-Catalog-Cart-and-Checkout`
Branch suggestion: `feature/05-orders-and-checkout`


---
name: Feature request
about: Suggest an idea for this project
title: 'Implement order history, tracking, and admin order status updates'
labels: 'feature,orders,admin,test'
assignees: ''
milestone: 'M3-Orders-Reporting-and-Quality'

---

**Is your feature request related to a problem? Please describe.**
Orders become hard to manage after checkout if customers and administrators cannot inspect or update them.

**Describe the solution you'd like**
Allow customers to view order history and track statuses while administrators can view all orders and update statuses. Include tests for valid and invalid status changes.

**Describe alternatives you've considered**
Keeping orders static after checkout is simpler but fails tracking and admin management requirements.

**Additional context**
Milestone lock: `M3-Orders-Reporting-and-Quality`
Branch suggestion: `feature/05-orders-and-checkout`


---
name: Feature request
about: Suggest an idea for this project
title: 'Add reviews, reporting, and documentation alignment'
labels: 'feature,reviews,reports,docs,test'
assignees: ''
milestone: 'M3-Orders-Reporting-and-Quality'

---

**Is your feature request related to a problem? Please describe.**
The specification requires reviews and reporting, and documentation must stay aligned with implemented behavior.

**Describe the solution you'd like**
Implement customer reviews and admin sales reporting with LINQ. Update docs with report definitions and add tests for rating and report calculations.

**Describe alternatives you've considered**
Review-only or report-only delivery would be incomplete relative to rubric expectations.

**Additional context**
Milestone lock: `M3-Orders-Reporting-and-Quality`
Branch suggestion: `feature/06-reviews-and-reporting`


---
name: Feature request
about: Suggest an idea for this project
title: 'Complete quality hardening: validation, exception handling, UX, tests, docs'
labels: 'feature,quality,ux,test,docs'
assignees: ''
milestone: 'M3-Orders-Reporting-and-Quality'

---

**Is your feature request related to a problem? Please describe.**
Even with features complete, marks can be lost if invalid input causes failures or if documentation and tests are incomplete.

**Describe the solution you'd like**
Add robust input validation, custom exception handling, friendly messages, reusable console helpers, regression tests, and final documentation cleanup.

**Describe alternatives you've considered**
Minimal hardening would save time but leaves quality and reliability risks.

**Additional context**
Milestone lock: `M3-Orders-Reporting-and-Quality`
Branch suggestion: `feature/07-quality-hardening`


---
name: Feature request
about: Suggest an idea for this project
title: 'Monday design-pattern implementation (no feature expansion)'
labels: 'feature,refactor,design-patterns,test,docs'
assignees: ''
milestone: 'M4-Monday-Patterns-2026-03-09'

---

**Is your feature request related to a problem? Please describe.**
Once the baseline is complete, pattern-based architecture is needed for extensibility and Submission 2 positioning.

**Describe the solution you'd like**
Introduce Factory, Strategy, Repository formalization, and lightweight State-style order transitions while preserving baseline behavior. Add pattern-focused tests and architecture doc updates.

**Describe alternatives you've considered**
A full rewrite was considered, but incremental pattern introduction is safer and faster.

**Additional context**
Milestone lock: `M4-Monday-Patterns-2026-03-09`
Branch suggestion: `feature/08-design-patterns`
