Capture a clean v0.32.0 all-provider benchmark baseline before threshold or provider-path tuning begins.

Scope:
- Run the benchmark harness with `--provider all --scale --iterations 5 --warmup 1`.
- Preserve the artifact triplet under a v0.32.0/ticket-specific path, and keep the prior seed run `artifacts/benchmarks/v0.31.0-scale-5-all-providers-20260606` available for comparison.
- Verify that the external-provider cleanup fix for `SatCustomerStatu` keeps bridge traversal rows green across shared external databases.

Podman test environment:
- Use the existing containers named `postgres`, `sqlserver`, `mysql`, and `oracle`.
- PostgreSQL must be reached through the Podman network. When using a .NET SDK container, run it on the Podman network and point `DVAULT_TEST_POSTGRES_CONNECTION_STRING` at the PostgreSQL container IP or resolvable Podman network name.
- SQL Server, MySQL, and Oracle should likewise use the Podman container endpoints rather than relying on an unrelated developer machine service.

Acceptance criteria:
- The run reports completed rows for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, or records an explicit operational skip/failure reason in the artifacts.
- The artifact summary calls out fallback causes for SQL Server, Oracle, PostgreSQL, and MySQL provider-optimized rows.
- The ticket comment or committed docs identify the concrete rows that justify subsequent threshold-tuning tasks.
- No product behavior is changed in this baseline-only task.