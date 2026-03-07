# Test Plan Starter Notes

## Initial test focus

- Domain invariants (product price/stock, cart quantity, review rating).
- Authentication service scenarios (unique email, login success/failure).
- Session context behavior (sign in/sign out state).
- Repository behavior (add/find/update/remove basics).

## Current auth coverage

- customer registration success
- duplicate email rejection
- invalid email format rejection
- customer login success
- seeded admin login success
- invalid password failure
- session sign-in/sign-out transitions

## Expansion plan by issue

- Issue 3: product search/filter tests.
- Issue 4: cart/wallet service tests.
- Issue 5: checkout happy/failure paths.
- Issue 6: order status transition tests.
- Issue 7: report aggregation and review tests.
- Issue 8: regression coverage and edge-case hardening.
- Issue 9: pattern-focused behavior parity tests.
