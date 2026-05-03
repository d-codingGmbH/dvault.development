## Summary
Separate required local provider checks from opt-in external database checks.

## Current Baseline
- SQLite has a required local integration path and an optimized provider package registration path through `AddDVaultSqlite`.
- PostgreSQL, SQL Server, Oracle, and MySQL provider packages exist, but external database integration checks must be opt-in unless configured.

## Scope
- Default CI runs SQLite integration tests and provider registration smoke tests.
- PostgreSQL, SQL Server, Oracle, and MySQL external integration tests run only when configured.
- Tests make clear when `AddDVaultSqlite` is required for optimized SQLite behavior versus the core `AddDVault` fallback path.

## Acceptance Criteria
- Skipped external provider tests explain missing configuration.
- SQLite tests validate the sample scenarios and the optimized provider path.
- Provider packages without external database configuration have registration/package smoke coverage.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.