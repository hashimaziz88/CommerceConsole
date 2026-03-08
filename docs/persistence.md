# JSON Persistence Notes

## Purpose

Provide lightweight runtime persistence so key data survives app restarts without introducing a database.

## Persisted Files

Default directory: `./data`

Files:
- `users.json`
- `products.json`
- `orders.json`

## Repository Ownership

- `InMemoryUserRepository` owns `users.json`
- `InMemoryProductRepository` owns `products.json`
- `InMemoryOrderRepository` owns `orders.json`

Each repository:
- loads JSON into memory at construction time
- writes full list back to file on add/update/remove

## Serialization Models

Persistence models are defined separately in:
- `Infrastructure/Repositories/Models/UserRecord.cs`
- `Infrastructure/Repositories/Models/ProductRecord.cs`
- `Infrastructure/Repositories/Models/ProductReviewRecord.cs`
- `Infrastructure/Repositories/Models/OrderRecord.cs`
- `Infrastructure/Repositories/Models/OrderItemRecord.cs`
- `Infrastructure/Repositories/Models/PaymentRecord.cs`

This separation avoids nested classes and keeps repository logic easier to maintain.

## Write Strategy

`JsonFileStore` writes to a unique temp file and then replaces the target file.

Benefits:
- avoids partially-written target files
- reduces file-collision risk across rapid writes

## Recovery Behavior

If JSON is malformed:
- load falls back to empty list for that file
- app remains usable instead of crashing

## Seeding Behavior with Persistence

- seeding runs at startup
- if persisted admin/products already exist, duplicates are not inserted
- first run creates baseline admin and products and persists them


## Bonus Export Artifacts

Bonus PDF reports are generated on demand by `PdfReportExporter`.

Notes:
- default export location is `./exports` when admin chooses default
- export files are read-only artifacts for sharing/demo evidence
- these files do not change repository JSON contracts

## Assumptions

- single process writes to data files in normal usage
- file path is writable by the app runtime
- persistence scope is local machine only

## Known Limitations

- no cross-process file locking policy yet
- no schema versioning/migration yet
- no encryption for persisted credentials in current phase

## Suggested Future Improvements

- password hashing + migration strategy
- optimistic concurrency tokens for writes
- explicit schema version + migration pipeline


