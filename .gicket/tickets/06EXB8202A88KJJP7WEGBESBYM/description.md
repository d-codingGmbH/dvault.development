## Summary
Define evidence required before publishing the package family to NuGet.

## Current Baseline
- The release candidate is now a package family: `DCoding.Data.DVault` plus SQLite, PostgreSQL, SQL Server, Oracle, and MySQL provider extension packages.
- Package validation must check aligned versions, package dependencies, readme files, XML docs, symbols, and absence of unintended test/helper/benchmark packages.

## Scope
- Create package validation checklist for the full package matrix.
- Add local pack verification.
- Document manual release steps and approval boundaries.

## Acceptance Criteria
- Publication is explicitly gated by tests, docs, examples, and package validation for every packable package.
- No automatic publish occurs before approval.
- The gate distinguishes source/development guidance from future NuGet-first consumer guidance.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.