## Summary
Automate the checks needed to trust the package candidate.

## Current Baseline
- CI must validate the full solution and every packable package in the current package matrix.
- SQLite remains the required local database integration path; PostgreSQL, SQL Server, Oracle, and MySQL external checks remain opt-in.

## Scope
- Run build, tests, formatting checks, documentation checks, and package verification.
- Run local SQLite/provider-registration checks by default and skip external-provider checks unless configured.

## Acceptance Criteria
- CI does not require external database services by default.
- Failures point to reproducible local commands.
- Pack verification covers core and provider packages, including symbols and readme/docs content.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.