# 09. GitHub Feature Issues

Below are 9 copy-paste-ready feature issues.

Milestone lock policy:
- Issues 1-2 must stay in `M1-Foundation-and-Auth`.
- Issues 3-5 must stay in `M2-Catalog-Cart-and-Checkout`.
- Issues 6-8 must stay in `M3-Orders-Reporting-and-Quality`.
- Issue 9 must stay in `M4-Monday-Patterns-2026-03-09`.
- Do not move issues across milestones without explicit approval.

---
name: Feature request
about: Suggest an idea for this project
title: 'Bootstrap clean solution architecture with tests and docs scaffolding'
labels: 'feature,architecture'
assignees: ''
milestone: 'M1-Foundation-and-Auth'
---

**Problem**
The project needs a scalable structure before features are implemented.

**Solution**
Create layered `Presentation/Application/Domain/Infrastructure` structure with starter interfaces, entities, services, repositories, tests, and docs.

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

**Problem**
Role-based access is required before meaningful system usage.

**Solution**
Implement registration, customer/admin login, unique email checks, seeded admin credentials, session tracking, and role navigation with tests.

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

**Problem**
The system needs customer catalog browsing and admin inventory controls.

**Solution**
Implement browse/search (LINQ), add/update/delete/restock, view products, and low-stock visibility with tests.

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

**Problem**
Checkout cannot function without cart and wallet foundations.

**Solution**
Implement cart mutations, totals, stock-aware quantity checks, wallet balance/top-up, and tests.

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

**Problem**
Purchases cannot be finalized without checkout orchestration.

**Solution**
Implement wallet-only checkout validation, stock deduction, payment/order records, snapshot order items, and cart clearing with tests.

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

**Problem**
Orders become unmanageable without tracking and admin lifecycle control.

**Solution**
Implement customer history/tracking, admin all-order view, and status updates with valid transition enforcement and tests.

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

**Problem**
Rubric requires review and reporting features with aligned documentation.

**Solution**
Implement purchased-product reviews, LINQ reporting metrics, and synchronized docs/tests.

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

**Problem**
Feature completion alone is not enough without robust UX/validation and documentation quality.

**Solution**
Harden input handling, exception boundaries, reusable UX helpers, regression tests, and final docs alignment.

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

**Problem**
Pattern-based extensibility is needed for Submission 2 positioning.

**Solution**
Introduce Factory, Strategy, repository formalization (if needed), and State-style transitions while preserving baseline behavior.

**Additional context**
Milestone lock: `M4-Monday-Patterns-2026-03-09`
Branch suggestion: `feature/08-design-patterns`
