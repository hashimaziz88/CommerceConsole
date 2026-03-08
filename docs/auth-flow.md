# Authentication Flow and Assumptions

## Purpose

This document explains how registration, login, role-routing, and session handling work end-to-end in CommerceConsole.

## Architecture Position of Auth

Presentation responsibilities:
- collect credentials and user input
- display success/error messages
- route to role-specific menu after login

Application responsibilities:
- validate auth inputs
- enforce unique email rule on registration
- verify credentials on login
- return authenticated domain user

Infrastructure responsibilities:
- persist/retrieve users via repository JSON storage

Domain responsibilities:
- user identity model and role type
- invariant checks for user construction

## Actors

- Guest (unauthenticated)
- Customer (authenticated)
- Administrator (authenticated)

## Seeded Administrator Account

Created via `SeedData` if none exists:
- Email: `admin@commerce.local`
- Password: `admin123`
- Name: `System Admin`

Design intent:
- guaranteed admin path for demos
- idempotent seed behavior to avoid duplicates

## Registration Flow

Entry point:
- `MainMenu` option `Register New Customer`

Sequence:
1. Presentation collects full name, email, password.
2. `AuthService.RegisterCustomer` validates required fields.
3. Basic email regex validation is applied.
4. `IUserRepository.GetByEmail` checks uniqueness.
5. `Customer` entity is created and persisted.
6. Success message is shown.

Rules enforced:
- name cannot be empty
- email cannot be empty
- email must match basic format
- password cannot be empty
- email must be unique (case-insensitive lookup)

Failure outcomes:
- invalid data -> `ValidationException`
- duplicate email -> `DuplicateEmailException`

## Login Flow

Entry point:
- `MainMenu` option `Login`

Sequence:
1. Presentation collects email and password.
2. `AuthService.Login` validates non-empty inputs.
3. Repository retrieves user by email.
4. Password is verified by `User.VerifyPassword`.
5. `SessionContext.SignIn` stores active user.
6. Menu routes by `UserRole`.

Failure outcomes:
- invalid input -> `ValidationException`
- wrong credentials -> `AuthenticationException`

## Role-Based Routing

`MainMenu.RouteByRole` routes immediately after successful login:
- `Customer` -> `CustomerMenu.Run`
- `Administrator` -> `AdminMenu.Run`

Defense-in-depth check:
- each menu verifies expected role before entering loop

## Session Model

Component:
- `SessionContext` (`ISessionContext` implementation)

Behavior:
- `CurrentUser` is set on sign-in
- `IsAuthenticated` derives from `CurrentUser != null`
- sign-out clears current user

Scope:
- process-local session only
- not persisted to JSON

Why this is correct for current scope:
- simple console runtime model
- explicit session state without global static leakage

## Exception Handling Boundary

Where exceptions are translated for UX:
- `MenuActionHelper.Execute`

Mapping examples:
- `ValidationException` -> validation error output
- `AuthenticationException` -> login error output
- `DuplicateEmailException` -> registration error output

Why this pattern helps:
- services stay free of presentation text concerns
- user gets consistent error format

## Security Notes (Current Scope)

Current constraints:
- passwords are stored in plain text for coursework scope
- no lockout/rate limiting
- no password reset

How to explain this in viva:
"Security hardening is acknowledged and isolated for future work; architecture already supports adding hashing at auth/service/repository boundaries without rewriting menus."

## Tests Covering Auth

Primary test file:
- `Tests/CommerceConsole.Tests/Application/AuthServiceTests.cs`

Covered scenarios:
- successful registration
- duplicate email rejection
- invalid email rejection
- customer login success
- seeded admin login success
- wrong-password rejection
- session sign-in/sign-out behavior

## Quick Viva Script

"Auth is menu-triggered but service-owned. Registration and login validation live in `AuthService`, persistence lives in repositories, session state lives in `SessionContext`, and role routing is handled in main menu with role checks inside each workspace."
