# Authentication Flow and Assumptions

## Registration flow

1. User selects `Register` from the main menu.
2. System collects full name, email, and password.
3. `AuthService.RegisterCustomer(...)` validates required fields and email format.
4. Service checks for email uniqueness through `IUserRepository.GetByEmail(...)`.
5. On success, a `Customer` is created and stored.

## Login flow

1. User selects `Login` from the main menu.
2. System collects email and password.
3. `AuthService.Login(...)` validates inputs and authenticates credentials.
4. On success, `SessionContext.SignIn(...)` stores the current user.
5. Main menu routes to the correct menu based on role:
   - `Customer` -> `CustomerMenu`
   - `Administrator` -> `AdminMenu`
6. User can log out from role menu, which clears session state via `SessionContext.SignOut()`.

## Seeded admin account

- Email: `admin@commerce.local`
- Password: `admin123`
- Created by `Infrastructure/Data/SeedData.cs` when no admin exists.

## Validation and exception assumptions

- Missing or invalid registration fields raise `ValidationException`.
- Duplicate registration email raises `DuplicateEmailException`.
- Invalid login credentials raise `AuthenticationException`.
- Menus catch and print friendly messages at presentation boundary.

## Session assumptions

- Single in-process session context (`SessionContext`) is enough for console scope.
- No persistent sessions are stored between app runs.
