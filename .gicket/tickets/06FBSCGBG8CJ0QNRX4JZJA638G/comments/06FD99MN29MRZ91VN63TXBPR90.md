[gicket-bot] PO-critic review contract

Summary
- The parent contract now matches the persisted five-child provider graph, and the child owner branches already carry the refined evidence-only / DB2-planning descriptions, so the ticket is ready for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/description.md` now has `## Open Questions` = `none` and ratifies the five-child split: PostgreSQL `06FBSCGGN528A2NC6TTA5A99X0`, SQL Server `06FBSCGNY2R6PC7P4Y91RD0HVR`, MySQL `06FBSCGVAZ5G8NP1TRXFNEP6DW`, Oracle `06FBSCH0M358R5J3RGFB6GRDM4`, DB2 `06FBSCH65R88BT6PS7XV32NQ1M`.
- Previous blocker `06FD1CZSDV5PYAMYQPYY0MWA8W.md` returned the ticket because the split/defer plan conflicted with the persisted child graph; PO comment `06FD6CP2YS4HE1EJJZEJ3NMEPM.md` explicitly answers critic-items 1-6 by ratifying the existing five-child graph and aligning DB2 as planning-only follow-up.
- `rg` over `.gicket/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/events/*.json` found five persisted `--blocks` relation ids for this parent to the same five child tickets (`06FBSD04K692P36ZJR86SZC9WM`, `06FBSD06B6676MSY3DANE5QTBG`, `06FBSD07WJNKK9TS20NS55DX6R`, `06FBSD09DQ8S637ZYS3C2NKGT4`, `06FBSD0B0T83AP353584PQYC14`).
- `git show` on the child owner branches now returns aligned descriptions: PostgreSQL branch `ticket/06FBSCGGN528A2NC6TTA5A99X0-task-close-postgresql-pit-and-bridge-read-gaps` says `Produce provider-configured PIT/bridge timing evidence... PostgresDataVaultReadStrategy`; SQL Server/MySQL/Oracle branches say the same for `SqlServerDataVaultReadStrategy`, `MySqlDataVaultReadStrategy`, and `OracleDataVaultReadStrategy`; DB2 branch `ticket/06FBSCH65R88BT6PS7XV32NQ1M-task-close-db2-pit-and-bridge-read-gaps` says `Deferred planning ticket for DB2 PIT/bridge timing evidence`.
- `git log --oneline --max-count=3` on the five child owner branches shows 2026-06-17 `audit-only mutation outbox po (update-ticket-description)` commits (`bb4fa49a4`, `61159b191`, `dfa69515b`, `11621a52b`, `19651c9f8`) followed by `audit-only relation follow up po (owner-branch-queue)` commits, matching the parent ticket's queued replay / durable outbox notes.
- `docs/architecture/dvault-v1-pit-bridge-boundary.md:13,60,89` says `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, `AddDVaultOracle()`, and `AddDVaultDb2()` already register diagnostics-gated PIT/bridge strategy candidates, and that external-provider PIT/bridge rows remain skipped guidance when connection strings are unset.
- `benchmark-summary.md:75-89`, `docs/plans/provider-optimization-gap-matrix.md:61-70`, and `tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs:454-468` keep PostgreSQL/SQL Server/MySQL/Oracle/DB2 PIT and bridge rows as skipped-placeholder guidance with planned read strategies, while non-SQLite latest-satellite remains unregistered; this matches the parent ticket's evidence-collection scope instead of new API/strategy design scope.
- `git diff --stat fa2048371f1a6491f37b7c5f8fd05acea002b6b7..74a911ae1edf02d0b15c9a1ddcbd0bd302442e55` changes only `.gicket/tickets/06FBSCGBG8CJ0QNRX4JZJA638G/**`, confirming this branch is ticket-metadata-only rather than a code-change ticket.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The parent ticket still leaves the exact PostgreSQL/SQL Server/MySQL/Oracle benchmark environments as a follow-up question instead of fixing them in the delivery contract.
- The parent ticket still leaves post-replay workflow timing for the child tickets as a follow-up question: immediate PO-critic re-entry versus waiting for an implementing owner.

Risky assumptions
- Approval assumes the child owner-branch refs inspected with `git show ticket/...` are the authoritative persisted state for the child tickets; the stale child descriptions visible on the current parent branch are not.

AC / test suggestions
- Keep downstream child tickets tied to provider-configured benchmark artifacts and do not treat skipped-placeholder, diagnostics-only, or smoke-only rows as completed timing evidence.
- Keep downstream PIT/bridge tickets explicit about the repository stop conditions already documented in `docs/plans/provider-optimization-gap-matrix.md:61-70` and `docs/architecture/dvault-v1-pit-bridge-boundary.md:13,60`.

Implementation watchouts
- Do not widen PostgreSQL, SQL Server, MySQL, or Oracle follow-up into new public API, new read-shape design, or alternative strategy invention; the repository already proves the candidate strategies.
- Do not promote DB2 diagnostics-only, smoke-only, or skipped-placeholder PIT/bridge posture into completed timing claims without later approved environment-backed benchmark evidence.
- Keep non-SQLite latest-satellite work out of this PIT/bridge audit split; the benchmark and boundary docs still record no provider-specific latest-satellite strategy for PostgreSQL/SQL Server/MySQL/Oracle/DB2.

Non-blocking notes
- The parent description still talks about queued child-description replays, but the inspected child owner-branch refs already contain the refined descriptions as of 2026-06-17; that wording is historical rather than a current blocker.
- The child tickets still carry `needs-po` on their own owner branches, but the parent comment `06FD6CP2YS4HE1EJJZEJ3NMEPM.md` explicitly treats that as each child ticket's own refinement flow rather than unresolved ambiguity in this parent audit ticket.

Split recommendations
- Use child `06FBSCGGN528A2NC6TTA5A99X0` for PostgreSQL provider-configured PIT and bridge timing evidence against `PostgresDataVaultReadStrategy`.
- Use child `06FBSCGNY2R6PC7P4Y91RD0HVR` for SQL Server provider-configured PIT and bridge timing evidence against `SqlServerDataVaultReadStrategy`.
- Use child `06FBSCGVAZ5G8NP1TRXFNEP6DW` for MySQL provider-configured PIT and bridge timing evidence against `MySqlDataVaultReadStrategy`.
- Use child `06FBSCH0M358R5J3RGFB6GRDM4` for Oracle provider-configured PIT and bridge timing evidence against `OracleDataVaultReadStrategy`.
- Use child `06FBSCH65R88BT6PS7XV32NQ1M` only as deferred DB2 planning until explicit DB2 evidence scope and environment-backed benchmark work are approved.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment