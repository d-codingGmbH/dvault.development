[gicket-bot] PO-critic review contract

Summary
- Approve for development: the persisted contract is specific, bounded, and directly grounded in the current PostgreSQL PIT source, tests, and docs, with no open questions left for PO refinement.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- The authoritative contract in `.gicket/tickets/06FF43GFC5F2VAA0Q7CS9KTX68/description.md` sets `PO Handoff` to `ready_for_po_critic`, lists four acceptance criteria and four DoD items, and has `## Open Questions` -> `none`.
- `src/DCoding.Data.DVault.Postgres/PostgresDataVaultPitMaintenanceStrategy.cs` currently reuses `dbContext.Database.CurrentTransaction?.GetDbTransaction()` and only rolls back `localTransaction`, which directly matches the ticket's ambient-transaction concern.
- `src/DCoding.Data.DVault/DataVaultProviderPitMaintenanceStrategyGateEvaluator.cs` currently evaluates PostgreSQL for provider name, dirty context, shape, and maintenance-shape evidence only; `src/DCoding.Data.DVault/DataVaultPitMaintenanceStrategyFallbackCauseKind.cs` has no ambient-transaction/savepoint-specific fallback cause today.
- `tests/DCoding.Data.DVault.Tests/Integration/PostgresPitMaintenanceServiceTests.cs` covers supported ordinary, multi-active, and link-parent rebuild parity only; targeted repository search found no ambient-transaction, savepoint, rollback, or cancellation assertions there, while `tests/DCoding.Data.DVault.Tests/Unit/SqlServerDataVaultPitMaintenanceServiceTests.cs` already proves the SQL Server no-savepoint decline precedent.
- Current docs still describe PostgreSQL clean full-rebuild support without caller-transaction qualification in `docs/architecture/dvault-v1-pit-bridge-boundary.md`, `docs/performance-profiles.md`, `docs/releases/v0.45.0.md`, and `CHANGELOG.md`.
- `git --no-pager diff --name-only a0e5d80ecc..HEAD` shows only `.gicket/tickets/06FF43GFC5F2VAA0Q7CS9KTX68/**` changes on the branch, so this remains a pre-development handoff review rather than an implementation review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- If the savepoint lane is chosen, the contract could be slightly stronger about whether both a fault path and a cancellation path need distinct live PostgreSQL assertions instead of one representative failure case.
- If the decline/fallback lane is chosen, one explicit example of the expected observable fallback evidence surface (gate result, activity tag, or fallback-cause name) would make later review faster, but the current contract is still sufficient for dev handoff.

Risky assumptions
- Assuming Npgsql ambient transactions are rollback-clean without live savepoint proof would recreate the exact ambiguity this ticket is intended to remove.
- Assuming a generic decline explanation is enough would weaken the stated requirement for an explicit caller-transaction boundary and a distinct fallback cause.

AC / test suggestions
- Keep one ambient caller-transaction integration case that faults after PIT rows are deleted but before rebuild completion, so pre-rebuild PIT rows are either restored via savepoint or preserved by provider-neutral fallback.
- Keep one ambient caller-transaction cancellation case in addition to happy-path parity coverage for ordinary hub-parent, shared-driving-key multi-active hub-parent, and link-parent non-multi-active rebuilds.
- If fallback is the chosen lane, assert the exact fallback cause/name so callers can distinguish this boundary from provider mismatch or dirty-context fallback.

Implementation watchouts
- `DefaultDataVaultPitMaintenanceService` selects provider maintenance through `strategy.CanRebuild(...)` before execution, so the ambient-transaction decision must be enforced at that gate to guarantee fallback before provider DELETE/INSERT work starts.
- The current PostgreSQL fallback vocabulary is generic; adding an ambient-transaction/savepoint-specific cause may require coordinated updates to both `DataVaultProviderPitMaintenanceStrategyGateEvaluator` and `DataVaultPitMaintenanceStrategyFallbackCauseKind`.
- Documentation updates likely need to cover not only the minimum files in the contract but also the current public summary in `CHANGELOG.md`, which still states PostgreSQL full-rebuild support without the caller-transaction qualifier.

Non-blocking notes
- All persisted comments under `.gicket/tickets/06FF43GFC5F2VAA0Q7CS9KTX68/comments/*.md` are bot orchestration/refinement comments; no later human comment reopens scope or introduces a new unresolved question.
- Comment `06FFEM73YR2SW5YZ5EV4QADSQR.md` queued a follow-up against related ticket `06FF43JEA6C3HNJ6AQA9XY7EC8` and reported `blocking diagnostics: 0`; that relation work does not change this ticket's dev-readiness.

Split recommendations
- No split recommended; the transaction-boundary decision, explicit fallback/savepoint behavior, tests, and documentation corrections remain one coherent bounded change.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment