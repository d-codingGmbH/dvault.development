[gicket-bot] PO-critic review contract

Summary
- The delivery contract is repository-backed, bounded to the existing PIT/bridge read surface, has no unresolved open questions, and is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F8KZJNZ999C8NKY0S92VBDN0/description.md:33-57 defines the acceptance criteria and definition of done, and its Open Questions section is explicitly `none`.
- src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-27 registers provider behavior plus MySqlStagedDataVaultSaveStrategy/MySqlDataVaultSaveStrategy only; it does not register IDataVaultProviderPitReadStrategy or IDataVaultProviderBridgeReadStrategy.
- src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:15-23 registers provider behavior plus OracleDataVaultSaveStrategy only; it does not register PIT or bridge read strategies.
- src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs:15-25 and src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs:15-25 already register PostgresDataVaultReadStrategy and SqlServerDataVaultReadStrategy for PIT/bridge reads.
- src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted> and <redacted> currently wire read-gate evaluation and known-strategy diagnostics for SQLite, PostgreSQL, and SQL Server only; MySQL and Oracle are not yet present on the read side.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:156-270 and tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:12-138 already show the exact gate, fail-closed, and parity-test pattern the story asks MySQL/Oracle to extend.
- docs/architecture/dvault-v1-pit-bridge-boundary.md:10-12 and 57-61 keep PIT/bridge reads on IDataVaultReadService over caller-maintained read-model tables, with provider-neutral fallback when no provider strategy applies.
- git diff --name-only develop..HEAD lists only .gicket/tickets/06F8KZJNZ999C8NKY0S92VBDN0/** paths, so the branch currently contains ticket-state changes only and no implementation changes yet, which is normal for this pre-development gate.
- .gicket/tickets/06F8KZK2MSFQP9G2DBM61ZVGD4/ticket.json and .gicket/tickets/06F8KZKFTCC0YXAPRTXA53DNEC/ticket.json exist as separate downstream benchmark and documentation tasks, supporting the contract's scope split.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract does not give an explicit example for a partial per-provider outcome such as PIT implemented but bridge declined; the current wording reads as full PIT+bridge support or full fallback-only posture per provider.
- The contract requires evidence-backed limitation notes for a deliberate decline but does not name the exact repository artifact where those notes should live.

Risky assumptions
- Assumes a deliberate-decline outcome can satisfy this story without widening into README/performance-profile work, because broad documentation follow-up is already split to 06F8KZKFTCC0YXAPRTXA53DNEC.
- Assumes one provider may ship a candidate while the other ships a decline within this same story, because the acceptance criteria are phrased per provider and split-by-provider remains optional.
- Assumes benchmark-row work remains downstream and non-blocking for this story, because 06F8KZK2MSFQP9G2DBM61ZVGD4 already exists as a separate task.

AC / test suggestions
- If MySQL is implemented, add explicit registration and gate coverage for both Pomelo.EntityFrameworkCore.MySql and MySql.EntityFrameworkCore, not just one provider name.
- Mirror the PostgreSQL/SQL Server gate tests for provider mismatch, supported-shape selection, unsupported-shape fallback, incomplete read-shape evidence fallback, and stale-maintenance fallback.
- Mirror the existing parity tests for raw PIT/bridge rows and typed projections against the provider-neutral fallback path.
- If either provider is deliberately declined, add explicit diagnostics and registration assertions proving the package remains fallback-only and that the limitation is repository-visible.

Implementation watchouts
- The developer will need to update both provider registration and DataVaultProviderReadStrategyGateEvaluator; today the read diagnostics switch and known-strategy lists only know SQLite/PostgreSQL/SQL Server.
- MySQL must preserve the existing dual-provider match surface shown by src/DCoding.Data.DVault.MySql/MySqlDataVaultSaveStrategy.cs:13-14 and DataVaultDiagnostics.cs:<redacted>.
- The existing PIT/bridge boundary forbids automatic maintenance, SaveChanges refresh, background orchestration, or public IDataVaultReadService API changes.
- Benchmark and broad documentation work are intentionally split out; avoid letting provider-matrix wording or benchmark artifact edits expand this ticket.

Non-blocking notes
- The Follow-Up Questions in .gicket/tickets/06F8KZJNZ999C8NKY0S92VBDN0/description.md:59-62 are downstream routing questions, not unresolved Open Questions.
- The current story branch is still ticket-metadata-only relative to develop; no implementation has started yet, which is expected before developer handoff.
- Sibling tasks for benchmark and documentation follow-up already exist, so the story is appropriately bounded instead of missing obvious split work.

Split recommendations
- Keep the story whole while the work stays inside MySQL/Oracle read-strategy registration, gate evaluation, diagnostics, parity coverage, or explicit decline evidence.
- Split by provider only if one provider becomes decline-only or needs materially different validation effort than the other.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment