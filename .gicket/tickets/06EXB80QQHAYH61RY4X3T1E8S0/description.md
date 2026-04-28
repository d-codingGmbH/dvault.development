## Summary
Separate required Sqlite integration tests from opt-in Postgres tests.

## Scope
- Default CI runs Sqlite.
- Postgres runs only when configured.

## Acceptance Criteria
- Skipped Postgres tests explain missing configuration.
- Sqlite tests validate the sample scenarios.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.