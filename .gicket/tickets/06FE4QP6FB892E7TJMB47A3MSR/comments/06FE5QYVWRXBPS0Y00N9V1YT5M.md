[gicket-bot] PO-critic review contract

Summary
- Approve for dev: no open questions remain, the repository already shows the normalized latest-satellite lane baseline, and the downstream split is concrete.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- benchmark-summary.md latest-satellite rows exist for PostgreSQL, SQL Server, MySQL, Oracle, and DB2 as skipped optimized baselines, and benchmark-summary.json mirrors them with plannedReadStrategy, readShape=LatestSatellite, iterations 0, and persistedOutcome not executed.
- docs/plans/provider-optimization-evidence-matrix.md says SQLite dvault-adddvaultsqlite-optimized is the only completed-timing optimized latest-satellite row; PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite rows are skipped-placeholder guidance.
- docs/plans/provider-optimization-gap-matrix.md P0.01-P0.05 already separates PostgreSQL latest-satellite as a closed capability gap and SQL Server, MySQL, Oracle, and DB2 as evidence gaps with explicit fallback boundaries.
- docs/releases/v0.42.0.md dated 2026-06-20 states the root triplet is the quick SQLite plus skipped optional-provider baseline and that SQLite remains the only completed optimized latest-satellite timing row.
- benchmarks/DCoding.Data.DVault.Benchmarks/README.md documents visible skipped optional-provider latest-satellite/PIT/bridge rows with planned execution detail, and tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs asserts plannedReadStrategy and persistedOutcome behavior for PostgreSQL, SQL Server, MySQL, Oracle, and DB2.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- This is still worth sending to dev even though the repository already contains the stated artifact, docs, and test baseline and the branch has no implementation diff yet.
- The README sentence excluding DB2 from the hash-key-storage matrix lane set is scoped to that mode only and does not redefine the root latest-satellite lane contract.

AC / test suggestions
- Keep acceptance tied to row identity and placeholder semantics across benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json, not to measured non-SQLite timing.
- Keep explicit verification that skipped latest-satellite rows preserve selectedStrategy, plannedReadStrategy, readShape=LatestSatellite, blank/null metrics, and persistedOutcome not executed for all five optional providers.

Implementation watchouts
- Reuse the existing v0.42.0 evidence-promotion wording; do not reopen provider naming or promotion rules in this ticket.
- Do not promote skipped PostgreSQL, SQL Server, MySQL, Oracle, or DB2 latest-satellite rows into completed timing claims without a provider-configured artifact bundle.
- Keep DB2 bounded to normalized lane visibility and diagnostics/smoke posture; broader DB2 promotion belongs to 06FE4QPEZW97YR6YT7MQD1MXTG.

Non-blocking notes
- Outgoing blocks follow-ups already exist for 06FE4QPR8TF8R6PXNM3RMXN8JG, 06FE4QQ0YTHD7624MGVPKKK1C0, 06FE4QQ9VF7B74E60CXEHSS5XW, and 06FE4QQJCJH7J9AWQTPDR5DSSG.
- The current owner branch is still at claim/handoff state, so the developer will be starting from a branch with ticket metadata only.

Split recommendations
- No additional split is recommended. Keep shared latest-satellite lane normalization in this ticket, keep PostgreSQL/SQL Server/MySQL/Oracle follow-up work in 06FE4QPR8TF8R6PXNM3RMXN8JG, 06FE4QQ0YTHD7624MGVPKKK1C0, 06FE4QQ9VF7B74E60CXEHSS5XW, and 06FE4QQJCJH7J9AWQTPDR5DSSG, and keep DB2 promotion broadening in 06FE4QPEZW97YR6YT7MQD1MXTG.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment