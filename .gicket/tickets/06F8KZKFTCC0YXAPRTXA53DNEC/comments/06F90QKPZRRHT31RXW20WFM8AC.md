[gicket-bot] PO-critic review contract

Summary
- Persisted contract is clear, bounded by direct repository evidence, and ready for developer handoff as a coordinated v0.28.0 documentation-baseline update rather than an implementation change.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted contract in `.gicket/tickets/06F8KZKFTCC0YXAPRTXA53DNEC/description.md:30-52` defines 5 acceptance criteria, 5 definition-of-done items, and `## Open Questions` = `none`.
- Current target branch `ticket/06F8KZKFTCC0YXAPRTXA53DNEC-task-update-v0-28-0-provider-read-optimization-d` is at `34a2be0ad`; `git diff --name-only develop...HEAD` shows only `.gicket/...` ticket metadata paths, so no documentation work has started yet.
- `docs/releases/v0.28.0.md` is currently missing on the target branch.
- `README.md:25` and `docs/production-adoption-checklist.md:9` still point adopters at `v0.27.0` as the current baseline, while `docs/performance-profiles.md:3-5` and `docs/architecture/dvault-v1-pit-bridge-boundary.md:5` still name `v0.26.0` as the current baseline.
- `README.md:422`, `README.md:987`, `docs/production-adoption-checklist.md:62`, and `docs/architecture/dvault-v1-pit-bridge-boundary.md:12,59` still describe optimized PIT/bridge read paths only for SQLite/PostgreSQL/SQL Server.
- `docs/performance-profiles.md:226-240` already documents SQLite-only optimized latest-satellite reads plus diagnostics-gated PIT/bridge candidates for PostgreSQL, SQL Server, MySQL, and Oracle.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs:15-29` and `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs:15-25` register MySQL and Oracle PIT/bridge read strategies; `src/DCoding.Data.DVault/DataVaultDiagnostics.cs:<redacted>,<redacted>` exposes `IDataVaultReadDiagnosticsService` plus PIT/bridge gate requirements for SQLite/PostgreSQL/SQL Server/MySQL/Oracle.
- `benchmark-summary.csv:19,21,23` records completed SQLite optimized latest-satellite/PIT/bridge reads, while `benchmark-summary.csv:40-51` keeps PostgreSQL, SQL Server, MySQL, and Oracle read rows visible as `skipped` when connection strings are unset and marks non-SQLite latest-satellite rows as `providerSpecificReadStrategy=not registered for latest satellite reads`.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:409-422`, `tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs:235-247`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:22-25,124-127` directly cover the provider-read rows, provider registrations, and PIT/bridge strategy parity for PostgreSQL/SQL Server/MySQL/Oracle.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No blocking PO gap remains, but the contract does not require a concrete redacted example that contrasts a completed SQLite timing row with a skipped optional-provider guidance row; the developer will need to keep that wording disciplined against the benchmark triplet.

Risky assumptions
- The contract phrase `active read-plan architecture note(s)` is treated as at least `docs/architecture/dvault-v1-pit-bridge-boundary.md`; if any other current architecture page repeats the old provider matrix, the developer will need to catch it during the documentation sweep.
- The follow-up question in `.gicket/tickets/06F8KZKFTCC0YXAPRTXA53DNEC/description.md:54-56` leaves older historical README release-note summaries outside this ticket's required scope, so the handoff assumes current-baseline fixes are sufficient even if older embedded summaries still mention the pre-v0.28 matrix.

AC / test suggestions
- Verification should explicitly recheck the four current-baseline surfaces named in the contract plus `docs/releases/v0.28.0.md` so they all say the same thing: SQLite-only optimized latest-satellite reads; PostgreSQL/SQL Server/MySQL/Oracle PIT/bridge strategy candidates only; provider-neutral fallback otherwise.
- When the release note is drafted, validate every evidence link against the checked-in benchmark triplet and the cited tests `BenchmarkScenarioExecutionTests.cs`, `ExplicitDataVaultSaveServiceTests.cs`, and `DataVaultRelationalPitBridgeReadStrategyParityTests.cs` to avoid speculative provider claims.
- Acceptance verification should confirm the fallback guidance uses the existing `IDataVaultReadDiagnosticsService` / `DataVaultReadStrategyFallbackCauseKind` vocabulary for `IncompleteReadShapeEvidence` and `StaleReadModelMaintenance` rather than inventing new compatibility categories.

Implementation watchouts
- This is still a pre-development branch state: `git diff --name-only develop...HEAD` shows only `.gicket/...` paths, so the first developer changes will establish the actual docs edits from scratch.
- The current contradiction is specific and easy to regress: README, checklist, and PIT/bridge architecture note still stop at SQLite/PostgreSQL/SQL Server, but performance profiles, benchmark rows, provider extensions, diagnostics gate requirements, and parity tests already include MySQL/Oracle PIT/bridge candidates.
- The missing `docs/releases/v0.28.0.md` should not expand into a broader historical rewrite; the persisted contract scopes this as a new coordinated v0.28 baseline plus updated current-baseline references.

Non-blocking notes
- Comment history under `.gicket/tickets/06F8KZKFTCC0YXAPRTXA53DNEC/comments/` is bot orchestration and PO-refinement metadata only; no unresolved human discussion was present.
- The PO refinement comment `.gicket/tickets/06F8KZKFTCC0YXAPRTXA53DNEC/comments/06F90NDJYY3ZR0X38BP79N0FA0.md` already states `decision: ready_for_po_critic`, and the persisted contract matches the direct repository evidence I checked.

Split recommendations
- No split recommended; the repository evidence and the persisted contract already bound this as one coordinated documentation-baseline rollover across README, performance guidance, checklist, architecture note, and the new `docs/releases/v0.28.0.md` file.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment