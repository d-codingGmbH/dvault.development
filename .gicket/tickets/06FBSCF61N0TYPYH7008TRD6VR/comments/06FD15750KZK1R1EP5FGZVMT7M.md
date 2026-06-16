[gicket-bot] PO-critic review contract

Summary
- Ticket contract is repository-backed, has no open questions, and is ready for developer handoff as a criteria story.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCF61N0TYPYH7008TRD6VR/description.md:30-67` persists 6 acceptance criteria, 5 definition-of-done items, 7 implementation-note items, and `## Open Questions` = `none` at lines 54-55.
- `.gicket/tickets/06FBSCF61N0TYPYH7008TRD6VR/comments/06FD1361GH3PSE9Y15RJ7Z23BR.md:6-15` records PO handoff decision `ready_for_po_critic`, clarifies SQLite-only latest-satellite optimization, separates PIT/bridge evidence lanes, and states the live graph already contains `06FBSCGBG8CJ0QNRX4JZJA638G` plus the five latest-satellite gap tickets.
- `.gicket/tickets/06FBSCF61N0TYPYH7008TRD6VR/ticket.json:7-19` shows the ticket is `todo`, carries `critic-needed`, has no assignees, and `is-blocked=false`.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md:11-13` states PIT/bridge are explicit maintained read models, `AddDVaultSqlite/Postgres/SqlServer/MySql/Oracle/Db2` register diagnostics-gated PIT/bridge candidates, SQLite is the only optimized latest-satellite path, and unsupported or non-SQLite latest-satellite requests fall back to provider-neutral reads.
- `benchmark-summary.csv:19` records a completed SQLite optimized `latest-satellite-read` row with `selectedStrategy=SqliteDataVaultReadStrategy`; `benchmark-summary.csv:42,45,48,51,54` record PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite guidance rows with `providerSpecificReadStrategy=not registered for latest satellite reads` and `persistedOutcome=not executed`.
- `benchmark-summary.csv:43-44,55-56`, `docs/plans/provider-optimization-evidence-matrix.md:250-270`, and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:452-468` preserve PIT/bridge guidance rows with planned provider read strategies while keeping external-provider read evidence as skipped placeholders.
- `tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs:68-95` and `145-175` show PIT and bridge parity coverage for Postgres, SqlServer, MySql, Oracle, and Db2 against provider-neutral fallback on supported maintained shapes.
- `src/DCoding.Data.DVault/DataVaultReadStrategyFallbackCauseKind.cs:14-64`, `src/DCoding.Data.DVault/DataVaultProviderReadStrategyGateEvaluator.cs:641-676,693-721,767-833`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs:411-486` anchor the finite fail-closed fallback vocabulary, including unsupported shapes, `IncompleteReadShapeEvidence`, and `StaleReadModelMaintenance`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Approval assumes the items under `## Follow-Up Questions` remain informational and do not need PO answers before downstream tickets apply the documented gates, because `## Open Questions` is explicitly `none` in `description.md:54-59`.

AC / test suggestions
- Require downstream closure notes to cite the exact matrix row identity (`scenario`, `provider`, `baseline`, `posture`) plus request-bound `IDataVaultReadDiagnosticsService` output when classifying a provider lane as implement, `no-work-required`, or defer.
- For any downstream ticket that adds measured benchmark rows, restate the regression-budget rule from `docs/plans/performance-evidence-benchmark-artifact-contract.md:126-129` and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:471-476`: targeted metric improves or holds, required SQLite non-target regressions above 5% fail by default, and optional-provider regressions above 10% require explicit justification.

Implementation watchouts
- Do not treat parity-only, `diagnostics-only`, `smoke-only`, or `skipped-placeholder` evidence as completed external-provider timing proof.
- Non-SQLite latest-satellite tickets remain provider-neutral or `no-work-required` unless strategy registration, diagnostics selection, and completed benchmark evidence all exist.
- PIT and bridge claims stay limited to explicitly maintained supported shapes; no closure text should imply automatic maintenance, raw SQL exposure, query-plan guarantees, or provider-specific physical-design promises.
- DB2 remains narrower than the other provider lanes; PIT/bridge candidate or smoke evidence does not imply DB2 latest-satellite optimization or completed DB2 timing.

Non-blocking notes
- Branch history is consistent with a ticket-only refinement pass: `1d4c7a38c` is the PO handoff commit and `d368c58b6` is a later PO-critic lease claim; `git diff --stat 1d4c7a38c..d368c58b6` touches only `.gicket` comment/event/ticket metadata.

Split recommendations
- No new split recommended. Keep `06FBSCGBG8CJ0QNRX4JZJA638G` as the PIT/bridge audit lane and the five latest-satellite gap tasks `06FBSCFDFFYQXBK17RT3E8W4CM`, `06FBSCFKWGQMBEF5Q96AZ5Q0X0`, `06FBSCFVT3SBHKMDGNEXWVWFXG`, `06FBSCG18KBRT1FTHDRX073EF4`, and `06FBSCG6C40X9CV3FFEHHKS6G0` as the existing split.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment