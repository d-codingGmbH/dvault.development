Goal: add opt-in PostgreSQL integration tests for the optimized provider path.

Acceptance Criteria:
- Tests are skipped with a clear reason unless PostgreSQL connection configuration is present.
- Tests verify insert-only behavior, unchanged satellite suppression, and changed satellite insertion.
- The default local test suite remains green without PostgreSQL installed.