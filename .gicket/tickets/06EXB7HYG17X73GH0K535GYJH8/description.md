## Summary
Prepare provider abstractions and optional Postgres tests while keeping Docker setup outside implementation scope.

## Scope
- Design provider capability abstraction.
- Support local Postgres tests when environment configuration is present.

## Acceptance Criteria
- Sqlite remains the default test path.
- Postgres tests are skipped unless explicitly enabled.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.