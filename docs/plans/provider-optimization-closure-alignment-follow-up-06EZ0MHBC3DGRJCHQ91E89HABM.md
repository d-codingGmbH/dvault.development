# Provider Optimization Closure-Alignment Follow-Up

Parent epic: `06EZ0MHBC3DGRJCHQ91E89HABM`

## Purpose

Persist the closure-alignment work that blocked the provider-optimization epic from returning to PO-critic as a clean tracking-only closure epic.

## Why This Exists

The epic contract already narrowed the parent to tracking-only closure work, but at ticket creation the remaining blockers were carried only as parent prose and older done-story closure text:

- done story `06EZ0N8HW9PZAFKMM5WQD564VR` described an outdated SQLite-only optimization baseline for provider-optimization closure
- done story `06EZ0NB4965QZZYG0Z1PG5YY7C` claimed broader Oracle provider-name registration that the visible source does not prove
- done story `06EZ0NCAFFJSSRFFEG66AYG8XC` and the benchmark README treated absent SQL Server, Oracle, and MySQL benchmark rows as provider release posture instead of benchmark artifact scope

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

- no closure prose classifies SQL Server, Oracle, or MySQL as lacking provider-specific optimized save strategies in the current save-strategy baseline
- no closure prose claims Oracle provider-name capability-profile auto-registration that the visible source does not prove
- the benchmark README is consistent with the root README and the architecture note on benchmark scope and release posture
- epic `06EZ0MHBC3DGRJCHQ91E89HABM` can cite this follow-up as the persisted owner of the remaining closure blocker

## Materialization

This plan has been materialized as follow-up story `06EZEHCCMBFDGW35YGR5D20EEW`.

The follow-up is assigned to the same provider-specific release and milestone as the parent epic, is related through `parentOf`, and blocks parent epic `06EZ0MHBC3DGRJCHQ91E89HABM` until the closure-alignment work is complete.

The earlier runtime attempt to create this ticket through `gicket-create-ticket` was rejected with `BOT-LOCAL-TOOL-TRUST-BLOCKED`; the bot fix should preserve persistent planning artifacts during stop-the-line writeback so this kind of document is not left as an untracked local file again.

## Closure Alignment Result

Story `06EZEHCCMBFDGW35YGR5D20EEW` is the persisted owner of the closure-alignment blocker for epic `06EZ0MHBC3DGRJCHQ91E89HABM`. Its documentation pass aligns the root README, the explicit save-service architecture note, and the benchmark README around the same release posture: five provider-specific save-strategy entry points exist with provider-neutral fallback, provider-name capability-profile auto-registration is visibly present only for SQLite and MySQL, Oracle optimization is limited to clean `Oracle.EntityFrameworkCore` hub/link batches, and SQL Server, Oracle, and MySQL are absent from the v1 benchmark artifact as a benchmark-scope decision rather than as a statement that their provider packages lack optimized save strategies.
