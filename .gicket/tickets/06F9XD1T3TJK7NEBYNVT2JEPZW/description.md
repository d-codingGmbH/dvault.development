Use the full external-provider benchmark run as concrete evidence for provider save strategy tuning in v0.32.0.

Context:
- Seed evidence exists under `artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606`.
- The completed matrix had 120/120 rows across SQLite, PostgreSQL, SQL Server, MySQL, and Oracle.
- Findings to turn into bounded work:
  - SQL Server optimized rows frequently report `SqlServerMinimumOperationThreshold` or `SqlServerMaximumSatelliteOperationThreshold`.
  - Oracle `customer-profile-scale-10000x10` reports `OracleMaximumSatelliteOperationThreshold` and stays close to provider-neutral fallback.
  - PostgreSQL and MySQL have small-batch rows where the optimized path is slower than provider-neutral fallback.
- This story is not a new platform/tool-suite, not automatic stored-procedure deployment, and not DB administration automation.

Acceptance criteria:
- The work defines which provider thresholds or eligibility rules are safe to tune and which must remain documented-only.
- Any tuning decision is backed by before/after benchmark artifacts following the v0.32.0 artifact/evidence requirements.
- The test plan explicitly uses the existing Podman containers for PostgreSQL, SQL Server, MySQL, and Oracle; PostgreSQL must be reached through the Podman network, not assumed to be reachable as localhost from every runner.
- DB2 remains out of scope for this story unless the benchmark harness has first gained DB2 provider support in the DB2 release lane.
- The implementation preserves EF Core-facing library behavior: transactions, cancellation, ordering, hash keys/hash diffs, load timestamps, idempotency, and fallback diagnostics remain correct.