# Architecture Starter Notes

## Layered structure

- `Presentation`: menu/input/output flow only.
- `Application`: use-case orchestration and service contracts.
- `Domain`: entities, invariants, and exceptions.
- `Infrastructure`: in-memory repositories and seed data.

## Dependency direction

`Presentation -> Application -> Domain`

`Infrastructure -> Domain`

The bootstrap keeps `Program.cs` thin by wiring services and menus only.

## Auth and role-routing update

- Registration and login are handled by `AuthService`.
- Session state is isolated in `SessionContext`.
- Role routing happens in `MainMenu` and dispatches to `CustomerMenu` or `AdminMenu`.
- Menu classes remain presentation-only and do not own auth business rules.
