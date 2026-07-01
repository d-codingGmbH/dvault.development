[gicket-bot] PO-critic review contract

Summary
- Ticket 06FH8RC9F0QEWF356WF7YYNNGM is ready for developer handoff: the save-only scope is explicit, `## Open Questions` is `none`, and the current repository evidence, thresholds, and benchmark-detail assertions match the delivery contract.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Ticket snapshot for 06FH8RC9F0QEWF356WF7YYNNGM includes `PO Handoff` decision `ready_for_po_critic`, `## Open Questions` = `none`, and both recent comments and closure evidence amendments are `<none>`.
- `docs/plans/provider-optimization-gap-matrix.md:14-16,56-60` says PostgreSQL, SQL Server, MySQL, Oracle, and DB2 `provider-native-bulk-ingestion` rows are already closed by repository-backed evidence and restates the same save boundaries used in this ticket.
- `docs/plans/provider-optimization-evidence-matrix.md:10,39-43` names the 2026-06-23 closure bundle as the current completed-timing source and records the same provider save gates: PostgreSQL 60-operation staged COPY, SQL Server <redacted>, MySQL retained-vs-staged-vs-provider-neutral windows, Oracle <redacted> direct-only, and DB2 1000-row cap.
- `src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs:17,47-52` sets PostgreSQL staged bulk at 60-plus operations; `src/DCoding.Data.DVault/DataVaultProviderSaveStrategyGateEvaluator.cs:12-22,76-90,104-140,425-499` encodes SQL Server <redacted> plus MySQL and Oracle save gates/fallbacks; `src/DCoding.Data.DVault.Db2/Db2DataVaultSaveStrategy.cs:15-17,30-35` keeps the DB2 1000-row set-based lane.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:780-796,825-868` asserts execution-detail coverage for PostgreSQL staged vs below-60, MySQL retained vs staged vs provider-neutral-above-303, Oracle direct-only with `stagedOracleBulk=not-selected-no-measured-win`, and DB2 `selectedStrategy=Db2DataVaultSaveStrategy` with `stagedBulkBoundary=not-supported`.
- `sqlserver-threshold-decision.md:7-15,27-48,58-62` is the current SQL Server authority and explicitly supersedes the older historical 50-operation threshold bundle with the 100 total / 900 mixed / 500 satellite decision.
- Branch inspection confirmed `git branch --show-current` = `ticket/06FH8RC9F0QEWF356WF7YYNNGM-task-close-selected-provider-save-strategy-parit`, `git rev-parse --verify aadca0bd3f2a09e52d2a423e85491bc079694c3c` resolved, and `git log --oneline -n 3 -- [relevant paths]` returned integrated history entries `bf6e0052e`, `24f9ff8ff`, and `691464dc1` on the reviewed surfaces.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Developer handoff must treat this as a save-only parity ticket; adjacent PIT/read/provider-maintenance code already in the repository is not authorization to widen scope.
- Reviewers and implementers must use the root `sqlserver-threshold-decision.md` as the live SQL Server authority and not fall back to the older historical diagnostics bundle's superseded 50-operation minimum.

AC / test suggestions
- Keep one explicit acceptance/test check per provider save lane using the existing benchmark vocabulary: PostgreSQL below-60 vs 60-plus, SQL Server <redacted>, MySQL below-100 vs 100-to-303 staged vs above-303 provider-neutral, Oracle direct-only with `stagedOracleBulk=not-selected-no-measured-win`, and DB2 selected-strategy plus 1000-row-cap evidence.

Implementation watchouts
- Do not reopen latest-satellite, PIT, bridge, or PIT-maintenance implementation inside this ticket; the contract and evidence matrices keep those as separate lanes.
- Preserve the current diagnostics contract, especially `selectedStrategy` tokens and explicit provider-neutral fallback wording for declined lanes.
- For SQL Server, treat `artifacts/benchmarks/06F9XD2M71D1XFT7FJX62KD8HM-sqlserver-save-threshold-diagnostics/sqlserver-threshold-decision.md` as historical context only, not as the current gate decision.

Non-blocking notes
- The repository already contains the aligned save gates, evidence docs, and benchmark-detail assertions, so dev work should preserve the bounded contract rather than rediscover thresholds.
- No recent ticket comments or closure evidence amendments were present in the provided ticket snapshot, so the delivery contract is the operative source of intent.

Split recommendations
- none

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment