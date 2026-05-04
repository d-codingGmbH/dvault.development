Goal: introduce provider-specific persistence optimization for the existing provider projects without weakening the provider-neutral fallback.

Scope:
- Use the existing provider projects for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL as the extension points.
- Keep the core DVault package provider-neutral and deterministic.
- Implement provider capability selection through explicit contracts instead of provider-name string checks in application code.
- Require integration tests or documented opt-in smoke tests for each provider-specific path.
- Require benchmark evidence comparing the optimized path with the provider-neutral fallback and the classic EF baseline where feasible.

Out of scope:
- Automatic package publishing.
- New Data Vault modeling features such as PIT, bridge, or multi-active satellite generation; those are tracked in the deferred capability epic.