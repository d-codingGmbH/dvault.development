Goal: add shared contract tests for the optimized provider execution boundary.

Acceptance Criteria:
- Contract tests validate parameter binding, transaction participation, cancellation propagation, and fallback behavior.
- Tests are provider-agnostic and reusable by SQLite plus external provider smoke suites.
- The tests avoid asserting SQLite-specific SQL syntax for non-SQLite providers.