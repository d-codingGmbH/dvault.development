## Summary
Define and implement a balanced unit and integration test strategy.

## Current Baseline
- Required local coverage should include the core `AddDVault` fallback path and the `AddDVaultSqlite` optimized provider path where behavior differs.
- PostgreSQL, SQL Server, Oracle, and MySQL external database checks must remain opt-in unless the environment is explicitly configured.
- Provider packages that do not yet have external integration coverage still need registration and package smoke coverage.

## Scope
- Cover metadata, hashing, EF model building, SQLite integration, provider registration, and optional external provider checks.

## Acceptance Criteria
- Test categories are documented.
- Default tests do not require external services.
- Provider-package tests make clear which checks are local smoke checks and which checks require a configured database server.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.