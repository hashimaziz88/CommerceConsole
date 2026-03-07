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
