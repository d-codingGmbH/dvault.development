## Summary
Ensure repeated writes reuse existing hub and link rows.

## Scope
- Use hash keys and uniqueness constraints to avoid duplicates.

## Acceptance Criteria
- Repeated write tests keep row counts stable.
- Concurrency assumptions are documented.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.