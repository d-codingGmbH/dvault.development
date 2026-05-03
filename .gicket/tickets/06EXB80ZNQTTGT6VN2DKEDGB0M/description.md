## Summary
Keep the public API documented, stable, and reviewable.

## Current Baseline
- Public API now spans the core package plus provider extension packages for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- The SQLite provider package exposes the optimized save strategy through `AddDVaultSqlite`; the other provider packages expose fallback registration helpers.

## Scope
- Enforce XML docs for public and protected APIs in every packable package.
- Track API surface changes per package so core API changes and provider package changes are reviewed deliberately.
- Respect one-member-per-file expectations across core and provider source projects.

## Acceptance Criteria
- Missing public/protected docs are detected for each packable package.
- API changes are visible in review and can distinguish core, provider, and test-only surfaces.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.