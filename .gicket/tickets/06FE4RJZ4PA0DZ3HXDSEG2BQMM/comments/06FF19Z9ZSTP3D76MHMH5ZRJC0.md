[gicket-bot] PO-critic review contract

Summary
- Ready for developer handoff. The contract is bounded, Open Questions is closed, and repo inspection confirms the public PIT maintenance seam plus existing SQL Server transaction and strategy-observability patterns it builds on.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket 06FE4RJZ4PA0DZ3HXDSEG2BQMM is todo, labeled area/performance, area/read-models, automation/bot-ready, provider/sqlserver, type/task, and its Delivery Contract sets Open Questions to none.
- /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs exposes public IDataVaultPitMaintenanceService.RebuildAsync(...) and MaintainParentsAsync(...), and /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt snapshots the same public surface.
- /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultPitMaintenanceResult.cs shows the public result contract is limited to PIT/table/count summary plus IsNoOp, matching the no-public-contract-change requirement.
- /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs registers provider-neutral IDataVaultPitMaintenanceService as DefaultDataVaultPitMaintenanceService, while /mnt/c/Projects/DVault/src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs layers AddDVault() plus SQL Server provider strategies through AddDVaultSqlServer().
- /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs currently rebuilds PITs by reading satellite rows, deleting PIT rows with ExecuteDeleteAsync, then inserting regenerated dictionary rows via SaveChangesAsync, which matches the ticket's stated provider-neutral baseline.
- /mnt/c/Projects/DVault/src/DCoding.Data.DVault.SqlServer/SqlServerDataVaultSaveStrategy.cs already uses Database.CurrentTransaction when present, otherwise opens a local transaction and rolls it back on failure; /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration/SqlServerDataVaultSmokeTests.cs verifies caller-transaction participation and cancellation-before-write behavior.
- /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultActivityTracing.cs, /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs, /mnt/c/Projects/DVault/src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs, and /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs show existing deterministic selectedStrategy and fallbackCauses observability patterns, including SQL Server PIT read rows.
- /mnt/c/Projects/DVault/tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs already covers PIT maintenance success, fault, and cancellation activity tracing, which matches the ticket note that new SQL Server work needs persisted rollback and cleanup assertions beyond tracing-only coverage.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not name one exact observability surface for candidate-selected versus fallback maintenance attempts; it only requires deterministic diagnostics or execution detail.
- The contract does not give an explicit example for fault or cancellation behavior while RebuildAsync is already inside a caller-open transaction versus the no-ambient-transaction path.
- The contract does not show an explicit zero-row or empty-PIT eligible rebuild example.

Risky assumptions
- Assumes clean context means no pending tracked EF changes, consistent with existing provider gate semantics in the repository.
- Assumes the SQL Server prototype can reuse an internal observability pattern similar to existing selectedStrategy and fallbackCauses reporting without changing IDataVaultPitMaintenanceService or DataVaultPitMaintenanceResult.
- Assumes helper-object cleanup may be satisfied either by explicit removal before return or by transaction-backed discard that leaves no leftover artifact after the failed attempt.

AC / test suggestions
- Pin at least one positive SQL Server rebuild case that proves the candidate path was actually selected, not just that eligible inputs can fall back.
- Pin negative gate cases for AddDVault-only registration, provider mismatch, dirty context, MaintainParentsAsync, multi-active PIT, and link-parent PIT with deterministic fallback visibility.
- Verify preloaded PIT rows survive both a faulted candidate attempt and a canceled candidate attempt, and assert that no helper objects remain afterward.
- Cover both transaction modes: existing caller transaction and locally created transaction.

Implementation watchouts
- AddDVaultSqlServer() currently adds SQL Server save and read strategies, while provider-neutral PIT maintenance still comes from AddDVault(); the prototype must preserve provider-neutral behavior when the new gate declines.
- The current provider-neutral PIT rebuild path is delete-then-insert in DefaultDataVaultPitMaintenanceService; rollback-preservation is new SQL Server-candidate behavior and should not be silently claimed for fallback.
- The public PIT maintenance request and result surface is already snapshotted; any candidate-path observability must avoid accidental public contract expansion.

Non-blocking notes
- The ticket is already tightly bounded: MaintainParentsAsync, multi-active PITs, link-parent PITs, non-SQL Server providers, and broader benchmark or promotion work are explicitly out of scope.
- The delivery contract is developer-ready because the public seam compatibility claims in the ticket are directly visible in the repository and Open Questions is closed.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment