[gicket-bot] PO-critic review contract

Summary
- Return to PO: the ticket is close, but the SQL Server failure/cancellation cleanup contract is not defined well enough for deterministic implementation and tests.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- Fresh repo inspection found HEAD `9ae8dcc3d9333b3921445ac4c585a8045da79db0` on branch `ticket/06FE4RJZ4PA0DZ3HXDSEG2BQMM-task-prototype-sql-server-pit-rebuild-insert-sel`; `git log -n 3` shows the last functional handoff commit was `ec5ec5ff55` and the current head is only the PO-critic lease claim.
- `.gicket/tickets/06FE4RJZ4PA0DZ3HXDSEG2BQMM/description.md` still has `## Open Questions` = `none` and Acceptance Criteria explicitly require SQL Server candidate selection, provider-neutral fallback, parity, observability, and `failure or cancellation cleanup behavior` coverage.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` registers `IDataVaultPitMaintenanceService` to `DefaultDataVaultPitMaintenanceService`, while `src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs` currently adds SQL Server save/read strategies only and no PIT maintenance strategy registration.
- `src/DCoding.Data.DVault/DefaultDataVaultPitMaintenanceService.cs` shows the current rebuild baseline reads satellite rows into memory, generates PIT rows in process, deletes PIT rows with `ExecuteDeleteAsync`, detaches tracked PIT dictionaries, then inserts regenerated rows with `SaveChangesAsync`.
- `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs` contains `PitMaintenanceActivityTracingRecordsFaultWithoutRawDiagnostics` and `PitMaintenanceActivityTracingRecordsCancellationThroughSqlite`, which verify tracing on fault/cancellation but do not establish persisted PIT table cleanup expectations after a failed or canceled rebuild.
- Repository contracts still describe provider-specific PIT maintenance as outside the current baseline in `docs/plans/pit-maintenance-service-v1-contract.md`, `docs/architecture/dvault-v1-pit-bridge-boundary.md` under `Unsupported In V1`, and `docs/production-adoption-checklist.md`.

Blocking findings
- The ticket requires tests for `failure or cancellation cleanup behavior`, but it never states the expected post-failure outcome. From the observed baseline, `DefaultDataVaultPitMaintenanceService.RebuildAsync(...)` deletes PIT rows with `ExecuteDeleteAsync` and later writes regenerated rows with `SaveChangesAsync`, and existing repo tests only assert tracing outcomes. The delivery contract does not say whether the SQL Server prototype must roll back to the pre-rebuild PIT contents, may leave the PIT empty on failure/cancellation, or only must clean up temporary SQL artifacts. That makes the acceptance target non-deterministic.

Required PO actions
- Add one explicit cleanup rule for the SQL Server prototype failure/cancellation path. Example decision points: `rebuild is atomic and pre-existing PIT rows remain intact on failure/cancellation`, or `temporary/staging SQL artifacts must be cleaned up but PIT contents may reflect the delete-before-insert baseline`.
- Update the acceptance criteria and test language to match that rule so the expected verification surface is concrete rather than inferred from current implementation details.

Open issues ledger
- critic-item-1 [required-po-action] Add one explicit cleanup rule for the SQL Server prototype failure/cancellation path. Example decision points: `rebuild is atomic and pre-existing PIT rows remain intact on failure/cancellation`, or `temporary/staging SQL artifacts must be cleaned up but PIT contents may reflect the delete-before-insert baseline`.
- critic-item-2 [required-po-action] Update the acceptance criteria and test language to match that rule so the expected verification surface is concrete rather than inferred from current implementation details.
- critic-item-3 [blocking-finding] The ticket requires tests for `failure or cancellation cleanup behavior`, but it never states the expected post-failure outcome. From the observed baseline, `DefaultDataVaultPitMaintenanceService.RebuildAsync(...)` deletes PIT rows with `ExecuteDeleteAsync` and later writes regenerated rows with `SaveChangesAsync`, and existing repo tests only assert tracing outcomes. The delivery contract does not say whether the SQL Server prototype must roll back to the pre-rebuild PIT contents, may leave the PIT empty on failure/cancellation, or only must clean up temporary SQL artifacts. That makes the acceptance target non-deterministic.

Missing examples / edge cases
- The contract does not name a concrete representative ordinary hub-parent PIT parity fixture. Citing the existing ordinary PIT shape used by current PIT maintenance tests would reduce ambiguity about the intended supported baseline.
- The ticket says `the context is clean` but does not spell out whether that means `ChangeTracker.HasChanges()==false` or the narrower save-strategy-style `no Added/Modified/Deleted entries` definition.

Risky assumptions
- Assuming the SQL Server path should strengthen cleanup semantics beyond the provider-neutral baseline without PO approval.
- Assuming any one of activity tracing, diagnostics, or execution detail is equally acceptable for fallback-cause observability without naming the preferred surface.

AC / test suggestions
- Add an explicit AC for failure/cancellation state, not just test presence.
- Anchor parity on one named ordinary hub-parent PIT fixture and require comparison against the existing `AddDVault()` provider-neutral rebuild result.
- Define the clean-context gate in repository terms so SQL Server candidate selection and fallback tests assert the same condition.

Implementation watchouts
- Current authoritative docs say provider-specific PIT maintenance is outside the baseline, so any implementation will need careful doc/comment alignment to avoid presenting this prototype as a general PIT maintenance model.
- There is no existing PIT maintenance provider-strategy seam in `AddDVaultSqlServer()` today; the implementation will need to introduce an internal dispatch point without changing the public `IDataVaultPitMaintenanceService` request/result contract.
- Parity needs to match the current provider-neutral row-generation rule in `DefaultDataVaultPitMaintenanceService.cs`, including ordinary-vs-multi-active/link-parent closed-fallback behavior.

Non-blocking notes
- The latest persisted ticket description already has `## Open Questions` = `none`.
- Post-handoff branch changes since `ec5ec5ff55` are only PO-critic claim/lease metadata under `.gicket`; no new implementation evidence was added after PO handoff.

Split recommendations
- Keep benchmark evidence or public performance-promotion work in a follow-up ticket, as the current contract already recommends.
- Keep multi-active and link-parent PIT rebuild optimization split from this ordinary hub-parent SQL Server prototype.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment