# Provider Optimization Closure-Alignment Follow-Up

Parent epic: `06EZ0MHBC3DGRJCHQ91E89HABM`

## Purpose

Persist the exact remaining closure-alignment work that still blocks the provider-optimization epic from returning to PO-critic as a clean tracking-only closure epic.

## Why This Exists

The current epic contract already narrows the parent to tracking-only closure work, but the remaining blockers are still carried only as parent prose:

- done story `06EZ0N8HW9PZAFKMM5WQD564VR` still describes a SQLite-only optimization baseline and compatibility-only posture for PostgreSQL, SQL Server, Oracle, and MySQL
- done story `06EZ0NB4965QZZYG0Z1PG5YY7C` still claims Oracle capability registration that the visible source does not prove
- done story `06EZ0NCAFFJSSRFFEG66AYG8XC` and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` still describe SQL Server, Oracle, and MySQL as compatibility-only packages

The current source and docs baseline is narrower and more specific:

- `README.md` and `docs/architecture/dvault-v1-explicit-save-service.md` describe five provider-specific save-strategy entry points
- `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` and `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs` are the only visible provider startup paths that call `DataVaultProviderCapabilityProfileSelection.Register(...)`
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs` register save strategies but do not prove provider-name capability-profile auto-registration
- `src/DCoding.Data.DVault.Oracle/OracleDataVaultSaveStrategy.cs` proves an Oracle-optimized path only for clean Oracle hub/link batches and keeps unsupported shapes on provider-neutral fallback

## Required Follow-Up Ticket Contract

Create one follow-up story with a title equivalent to:

`Story: Align provider optimization closure contracts and release posture`

That story should:

1. Supersede the stale closure narrative in `06EZ0N8HW9PZAFKMM5WQD564VR`, `06EZ0NB4965QZZYG0Z1PG5YY7C`, and `06EZ0NCAFFJSSRFFEG66AYG8XC` for epic-closure purposes.
2. Align `README.md`, `docs/architecture/dvault-v1-explicit-save-service.md`, and `benchmarks/DCoding.Data.DVault.Benchmarks/README.md` to one release posture.
3. Preserve the distinction between:
   - five provider-specific save-strategy entry points with provider-neutral fallback
   - the narrower provider-name capability-profile auto-registration surface currently evidenced only for SQLite and MySQL
4. State that Oracle optimization is intentionally narrower than the other provider stories:
   - clean `Oracle.EntityFrameworkCore` hub/link batches are optimized
   - unsupported shapes fall back through the provider-neutral writer

## Acceptance Boundary For The Follow-Up

- no closure prose says SQL Server, Oracle, or MySQL are compatibility-only packages in the current save-strategy baseline
- no closure prose claims Oracle provider-name capability-profile auto-registration that the visible source does not prove
- the benchmark README is consistent with the root README and the architecture note on benchmark scope and release posture
- epic `06EZ0MHBC3DGRJCHQ91E89HABM` can cite this follow-up as the persisted owner of the remaining closure blocker

## Materialization

This plan has been materialized as follow-up story `06EZEHCCMBFDGW35YGR5D20EEW`.

The follow-up is assigned to the same provider-specific release and milestone as the parent epic, is related through `parentOf`, and blocks parent epic `06EZ0MHBC3DGRJCHQ91E89HABM` until the closure-alignment work is complete.

The earlier runtime attempt to create this ticket through `gicket-create-ticket` was rejected with `BOT-LOCAL-TOOL-TRUST-BLOCKED`; the bot fix should preserve persistent planning artifacts during stop-the-line writeback so this kind of document is not left as an untracked local file again.
