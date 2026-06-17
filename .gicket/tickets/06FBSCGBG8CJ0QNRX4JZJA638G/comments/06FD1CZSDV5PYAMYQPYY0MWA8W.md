[gicket-bot] PO-critic review contract

Summary
- Repository evidence supports the SQLite/PostgreSQL/SQL Server/MySQL/Oracle/DB2 classifications, but the ticket is not ready because its split and defer plan conflicts with the already-persisted downstream ticket graph.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/description.md` contains `## Open Questions` = `none` and explicitly records SQLite=no-op, PostgreSQL/SQL Server/MySQL/Oracle=implement, DB2=defer.
- `git log --oneline -5 ticket/06FBSCGBG8CJ0QNRX4JZJA638G-task-audit-provider-pit-and-bridge-read-gaps` shows HEAD `ef367a097`; `git diff --name-only fa2048371...ef367a097` changes only `.gicket/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/**`, so this branch is a closure-only metadata branch with no repo code/doc edits.
- `docs/plans/provider-optimization-gap-matrix.md` classifies PIT rows `P2.01-P2.05` and bridge rows `P3.01-P3.05` as evidence gaps, while SQLite PIT/bridge rows remain `completed-timing` reference baselines.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` each register `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`; `src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs` registers SQLite optimized read, PIT, and bridge strategies.
- `benchmark-summary.md:54` and `benchmark-summary.md:56` show completed SQLite optimized PIT/bridge rows, while `benchmark-summary.md:76-89` keeps PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT/bridge rows visible as `skipped` with unset connection-string reasons and `not executed` outcomes.
- `docs/plans/provider-optimization-evidence-matrix.md:255-270` records PostgreSQL, SQL Server, MySQL, Oracle, and DB2 PIT/bridge rows as `skipped-placeholder`; the same file and `docs/releases/v0.34.0.md` keep DB2 PIT/bridge in `diagnostics-only`/`smoke-only` posture with no completed DB2 timing claim.
- Existing `blocks` relations already point from this ticket to five provider-specific child tickets in `.gicket/relations/8G/*/06FBSCGBG8CJ0QNRX4JZJA638G--...--blocks.json`, and comment `.gicket/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/comments/06FD1BC6MWY55TW8SQSF8CG4H4.md` queues owner-branch follow-up for all five child tickets.
- `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:455-468` preserves expected external-provider PIT/bridge guidance rows for `PostgresDataVaultReadStrategy`, `SqlServerDataVaultReadStrategy`, `MySqlDataVaultReadStrategy`, `OracleDataVaultReadStrategy`, and `Db2DataVaultReadStrategy`.

Blocking findings
- The delivery contract's split plan does not match the persisted ticket graph. This ticket recommends grouped PostgreSQL+SQL Server and MySQL+Oracle follow-up plus DB2 defer, but the repository already has five provider-specific downstream `blocks` tickets (`06FBSCGGN528A2NC6TTA5A99X0`, `06FBSCGNY2R6PC7P4Y91RD0HVR`, `06FBSCGVAZ5G8NP1TRXFNEP6DW`, `06FBSCH0M358R5J3RGFB6GRDM4`, `06FBSCH65R88BT6PS7XV32NQ1M`).
- The DB2 defer decision is not reconciled with the persisted DB2 child ticket. `.gicket/tickets/06FBSCH65R88BT6PS7XV32NQ1M/description.md` still asks to `Implement or reject DB2 PIT/bridge read strategy improvements based on the audit`, which conflicts with this ticket's `keep DB2 out of the v0.41 implementation batch` guidance.
- All existing downstream provider tickets still carry `needs-po`, so approving this closure-only audit now would hand off an unresolved downstream ticket topology instead of one authoritative follow-up plan.

Required PO actions
- Reconcile `## Split Recommendations` with the already-persisted provider-specific child tickets: either update this ticket to endorse the five existing child tickets as the authoritative split, or explicitly supersede/replace them with a new grouped split plan.
- Explicitly disposition the DB2 follow-up ticket `06FBSCH65R88BT6PS7XV32NQ1M`: close it as deferred/no-work, convert it into a deferred planning ticket, or revise this ticket if DB2 is actually intended to remain in active follow-up scope.
- Update the downstream PostgreSQL/SQL Server/MySQL/Oracle child ticket descriptions or labels so they clearly state `provider-configured PIT/bridge timing evidence for existing strategy candidates` instead of generic `implement or reject` wording.

Open issues ledger
- critic-item-1 [required-po-action] Reconcile `## Split Recommendations` with the already-persisted provider-specific child tickets: either update this ticket to endorse the five existing child tickets as the authoritative split, or explicitly supersede/replace them with a new grouped split plan.
- critic-item-2 [required-po-action] Explicitly disposition the DB2 follow-up ticket `06FBSCH65R88BT6PS7XV32NQ1M`: close it as deferred/no-work, convert it into a deferred planning ticket, or revise this ticket if DB2 is actually intended to remain in active follow-up scope.
- critic-item-3 [required-po-action] Update the downstream PostgreSQL/SQL Server/MySQL/Oracle child ticket descriptions or labels so they clearly state `provider-configured PIT/bridge timing evidence for existing strategy candidates` instead of generic `implement or reject` wording.
- critic-item-4 [blocking-finding] The delivery contract's split plan does not match the persisted ticket graph. This ticket recommends grouped PostgreSQL+SQL Server and MySQL+Oracle follow-up plus DB2 defer, but the repository already has five provider-specific downstream `blocks` tickets (`06FBSCGGN528A2NC6TTA5A99X0`, `06FBSCGNY2R6PC7P4Y91RD0HVR`, `06FBSCGVAZ5G8NP1TRXFNEP6DW`, `06FBSCH0M358R5J3RGFB6GRDM4`, `06FBSCH65R88BT6PS7XV32NQ1M`).
- critic-item-5 [blocking-finding] The DB2 defer decision is not reconciled with the persisted DB2 child ticket. `.gicket/tickets/06FBSCH65R88BT6PS7XV32NQ1M/description.md` still asks to `Implement or reject DB2 PIT/bridge read strategy improvements based on the audit`, which conflicts with this ticket's `keep DB2 out of the v0.41 implementation batch` guidance.
- critic-item-6 [blocking-finding] All existing downstream provider tickets still carry `needs-po`, so approving this closure-only audit now would hand off an unresolved downstream ticket topology instead of one authoritative follow-up plan.

Missing examples / edge cases
- The ticket does not say how the existing five provider-specific child tickets should be handled if the grouped split recommendation is kept.
- The ticket does not give a concrete ticket-level disposition for the already-open DB2 child if DB2 remains deferred under the v0.34 posture.

Risky assumptions
- Assumes downstream teams will infer how to reinterpret or regroup the five existing child tickets without explicit ticket updates.
- Assumes the existing DB2 child ticket will be treated as deferred even though its current persisted text still invites implementation-or-reject work.

AC / test suggestions
- State the exact disposition of each existing downstream ticket ID in the acceptance criteria or definition of done.
- For PostgreSQL, SQL Server, MySQL, and Oracle, require downstream tickets to preserve the PIT/bridge stop conditions from `docs/plans/provider-optimization-gap-matrix.md` and `docs/architecture/dvault-v1-pit-bridge-boundary.md`.
- For DB2, require any future follow-up ticket to cite `docs/releases/v0.34.0.md` and `docs/plans/provider-optimization-evidence-matrix.md` and to forbid completed-timing claims until provider-configured benchmark artifacts exist.

Implementation watchouts
- Do not reinterpret `skipped-placeholder`, `diagnostics-only`, or `smoke-only` rows as completed external-provider timing evidence.
- Preserve the explicit-maintenance, incomplete-read-shape, stale-maintenance, and unsupported-shape fallback boundaries from `docs/architecture/dvault-v1-pit-bridge-boundary.md`.
- DB2 latest-satellite optimization, staged bulk, provider-native chunk execution, and completed DB2 PIT/bridge timing remain out of scope under the current baseline.

Non-blocking notes
- The repository evidence does support the provider decision logic itself: SQLite has completed PIT/bridge timing evidence, PostgreSQL/SQL Server/MySQL/Oracle have registered PIT/bridge candidates plus skipped root timing rows, and DB2 remains bounded to diagnostics/smoke-backed posture.
- Because `git diff --name-only fa2048371...ef367a097` is ticket-metadata-only, the lack of repo implementation changes is not a blocker for this closure-only audit ticket by itself.

Split recommendations
- Pick one authoritative downstream topology and record it explicitly: either keep the five existing provider-specific child tickets and refine them, or supersede them with grouped evidence tickets plus a separate deferred DB2 planning ticket.
- If the grouped plan is kept, explicitly supersede `06FBSCGGN528A2NC6TTA5A99X0`, `06FBSCGNY2R6PC7P4Y91RD0HVR`, `06FBSCGVAZ5G8NP1TRXFNEP6DW`, `06FBSCH0M358R5J3RGFB6GRDM4`, and `06FBSCH65R88BT6PS7XV32NQ1M` so there is one unambiguous follow-up plan.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment