Goal: implement and validate a PostgreSQL-specific optimized save path in the existing PostgreSQL provider project.

Scope:
- Use PostgreSQL-appropriate SQL for set-based hub, link, and satellite existence checks and insert-only writes.
- Preserve the provider-neutral fallback for unsupported model shapes.
- Add opt-in integration coverage that can run against a configured PostgreSQL instance.
- Add benchmark evidence comparing PostgreSQL optimized behavior with fallback behavior where the environment is available.

Acceptance Criteria:
- The PostgreSQL provider registers its optimized capability explicitly.
- Tests cover generated SQL shape or behavior without making local default test runs require PostgreSQL.
- Benchmarks or documented smoke results show whether the optimized path improves large insert/change scenarios.