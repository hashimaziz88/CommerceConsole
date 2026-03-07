# Authentication Flow and Assumptions

## Objective

Provide role-aware authentication for customer and administrator users while keeping UI logic thin and business logic centralized.

## Actors

- Guest (not authenticated)
- Customer (authenticated role)
- Administrator (authenticated role)

## Seeded Administrator

Default admin is created by `SeedData` only when missing:
- Email: `admin@commerce.local`
- Password: `admin123`

This seed record is persisted in `data/users.json`.

## Registration Flow

1. User selects `Register` in `MainMenu`.
2. UI captures full name, email, and password.
3. `AuthService.RegisterCustomer(...)` validates:
- non-empty full name
- non-empty email
- basic email format
- non-empty password
4. Service checks for duplicate email via `IUserRepository.GetByEmail(...)`.
5. On success, new `Customer` is saved and persisted.
6. UI prints a success message.

Exceptions surfaced to UI:
- `ValidationException`
- `DuplicateEmailException`

## Login Flow

1. User selects `Login` in `MainMenu`.
2. UI captures email and password.
3. `AuthService.Login(...)` validates non-empty inputs.
4. Repository lookup confirms user exists and password matches.
5. On success, `SessionContext.SignIn(...)` stores current user.
6. Main menu routes by role.

Exceptions surfaced to UI:
- `ValidationException`
- `AuthenticationException`

## Role-Based Routing

Routing happens in `MainMenu.RouteByRole()`:
- `UserRole.Customer` -> `CustomerMenu.Run(...)`
- `UserRole.Administrator` -> `AdminMenu.Run(...)`

Each role menu validates the expected role before continuing.

## Session Lifecycle

`SessionContext` behavior:
- `CurrentUser` set on login
- `IsAuthenticated` derived from `CurrentUser`
- `SignOut()` clears session

Session is process-local and non-persistent by current design.

## Security and Scope Notes

Current implementation is demo-oriented:
- passwords are stored in plain text
- no account lockout/throttling
- no password reset
- no multi-session support

These are acceptable for current console coursework scope and can be upgraded later.

## Traceability to Code

- Service logic: `Application/Services/AuthService.cs`
- Session state: `Application/Services/SessionContext.cs`
- UI routing: `Presentation/Menus/MainMenu.cs`
- Admin bootstrap: `Infrastructure/Data/SeedData.cs`
