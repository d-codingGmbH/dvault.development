[gicket-bot] PO-critic review contract

Summary
- Delivery contract is clear, bounded, and internally consistent for pre-development handoff; direct ticket, repo, source, and branch evidence support the MySQL/Oracle/DB2 PIT-matrix decisions, and `## Open Questions` is `none`.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- `docs/architecture/dvault-v1-pit-bridge-boundary.md` states the current PIT-maintenance baseline is PostgreSQL via `IDataVaultProviderPitMaintenanceStrategy` and SQL Server via `SqlServerDataVaultPitMaintenanceService`; the same file records MySQL as a future ordinary hub-parent-only strategy candidate and keeps PIT read timing separate from maintenance proof.
- `src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs`, `src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs`, and `src/DCoding.Data.DVault.Db2/DVaultDb2ServiceCollectionExtensions.cs` register provider behavior/save/read/PIT-read/bridge-read services only; none registers `IDataVaultProviderPitMaintenanceStrategy` or replaces `IDataVaultPitMaintenanceService`.
- `src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs` adds `IDataVaultProviderPitMaintenanceStrategy`, while `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` replaces `IDataVaultPitMaintenanceService`; `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` only evaluates the known PostgreSQL strategy today.
- `docs/plans/db2-pit-maintenance-full-rebuild-feasibility.md` accepts one future `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` slice through the provider-strategy seam and keeps multi-active, link-parent, `MaintainParentsAsync(...)`, dirty-context, provider-mismatch, incomplete-shape, and ambient-transaction cases deferred or fallback-only.
- `docs/performance-profiles.md` and `docs/plans/provider-optimization-gap-matrix.md` keep MySQL/Oracle/DB2 PIT read timing as read-side evidence over maintained PIT rows and do not promote it to write-side maintenance proof.
- Direct read of `.gicket/tickets/06FFDG522514HX2J17GT9VE77W/ticket.json` shows the MySQL follow-up ticket exists with title `Task: Implement MySQL ordinary hub-parent PIT full-rebuild maintenance strategy`; an `rg` scan over `.gicket/tickets/*/ticket.json` found the DB2 evaluation ticket but no DB2 implementation ticket yet.
- `git show --stat` for branch head `58719a99caabe17e5b3fc480932eadad6a9f7257` shows only `.gicket/tickets/06FF43F283QFQ56290AVJ3AXSM` comment/event/ticket metadata changes and no edits to the cited docs, consistent with this being a pre-development handoff gate.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- The future DB2 follow-up may be referenced descriptively without a persisted ticket id during this ticket's implementation; that matches the current contract, but downstream docs will need a concrete id once the child is created.

AC / test suggestions
- Keep each provider row explicitly split into current registration surface, accepted/deferred/fallback-only maintenance shapes, rollback gate, and a statement that PIT read timing is not maintenance proof.
- State once in the matrix that historical incoming `blocks` links from the done evaluation tickets are completion context, not active blockers.

Implementation watchouts
- Do not widen MySQL beyond the `MySql.EntityFrameworkCore` ordinary hub-parent full-rebuild candidate in the first slice; Pomelo remains deferred for live maintenance validation.
- Do not turn Oracle read timing or registration evidence into an Oracle implementation recommendation; the current disposition is defer.
- Keep MySQL and DB2 on the existing provider-strategy seam, not a SQL Server-style `IDataVaultPitMaintenanceService` replacement, unless a later ticket establishes new source and live evidence.
- Any accepted provider lane must preserve pre-rebuild PIT rows on fault or cancellation when it owns the transaction and fall back when ambient savepoint safety is unproven.

Non-blocking notes
- The owner branch currently carries ticket metadata/lease activity, not the documentation change itself; that is a developer-handoff watchout, not a PO blocker under the stated pre-development gate.

Split recommendations
- Reuse existing MySQL follow-up ticket 06FFDG522514HX2J17GT9VE77W for the ordinary hub-parent lane.
- Create one separate DB2 implementation ticket limited to `IBM.EntityFrameworkCore` ordinary hub-parent `RebuildAsync(...)` push-down through `IDataVaultProviderPitMaintenanceStrategy`.
- Keep Oracle deferred and do not create an Oracle implementation child from this matrix.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment