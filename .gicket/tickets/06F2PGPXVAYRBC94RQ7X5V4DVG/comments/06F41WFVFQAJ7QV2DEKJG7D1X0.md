[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- All inspected comment files under `.gicket/tickets/06F2PGPXVAYRBC94RQ7X5V4DVG/comments/*.md` are bot claim/refinement/handover entries; no human scope-change comment was found.
- `README.md:208-273` already documents `ReadCurrentSatelliteAsync(...)` and `ReadAsOfSatelliteAsync(...)`, but `README.md:277` still says `PIT rows remain caller-populated`.
- `docs/releases/v0.15.0.md:20-80` is still bridge-centric and says `PIT maintenance remains outside this release` at `docs/releases/v0.15.0.md:58`.
- `docs/production-adoption-checklist.md:46` still tells adopters PIT rows must already be populated and says current helpers do not provide provider-specific PIT or bridge read optimization.
- `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:28-31` registers `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`; `src/DCoding.Data.DVault/DataVaultPitMaintenanceService.cs:5-31`, `DataVaultPitRebuildRequest.cs:5-23`, and `DataVaultPitParentMaintenanceRequest.cs:5-32` expose the PIT maintenance surface the docs are supposed to describe.
- `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs:10-119` implements `IDataVaultProviderPitReadStrategy` and `IDataVaultProviderBridgeReadStrategy`, which supports the ticket's SQLite-only optimized PIT/bridge read claim.
- Existing tests already back the four documented deltas: `tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs:18-144`, `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs:13-166`, `tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs:118-146`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultPitMaintenanceServiceTests.cs:34-41`.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The docs pass should explicitly distinguish PIT full rebuild from bounded parent maintenance, including late-arriving history correction, because the verified PIT tests cover both behaviors.
- The docs should keep hierarchy bridge maintenance edge cases visible: incremental maintenance can lower `TraversalDepth`, but topology shrinkage still requires rebuild.
- At least one additional current-baseline guide still points at v0.14.0: `docs/model-first-governance.md:3-5`. The implementation should confirm whether that page is in the intended user-facing baseline set for this ticket.

Risky assumptions
- Only SQLite should be documented as the repository-proven optimized PIT/bridge read provider; anything broader would outrun `src/DCoding.Data.DVault.Sqlite/SqliteDataVaultReadStrategy.cs` and the current test evidence.
- PIT maintenance documentation must stay on the explicit service surface that was verified locally; no public registry-backed PIT maintenance adapter was confirmed in source.
- Historical or architecture pages that still mention v0.14.0 can only be left untouched if they are not serving as current-baseline adopter guidance.

AC / test suggestions
- Acceptance verification should explicitly confirm the updated docs cite actual PIT, bridge, current/as-of, and SQLite strategy evidence files rather than only prose summaries.
- A reviewer check should confirm no touched v0.15.0-facing doc still describes PIT rows as caller-populated or frames v0.15.0 as bridge-only.
- A reviewer check should confirm any current-baseline guide that still names v0.14.0 as current is either updated in this ticket or intentionally excluded with clear rationale.

Implementation watchouts
- Revise `docs/releases/v0.15.0.md` in place; do not create a parallel release-note file.
- Keep PIT maintenance, bridge maintenance, and PIT/bridge reads separated as explicit caller-invoked surfaces; do not imply read-time refresh, automatic scheduling, or `SaveChanges` side effects.
- Keep provider claims bounded to SQLite optimization plus provider-neutral fallback elsewhere.
- Use the already-shipped source and tests as the evidence boundary; do not document unverified registry-backed PIT maintenance or non-SQLite PIT/bridge optimizations.

Non-blocking notes
- The branch is still at a pre-implementation state: `git show --stat --oneline --no-patch ticket/06F2PGPXVAYRBC94RQ7X5V4DVG-task-update-v0-15-0-documentation-and-release-no` returned `08da25d61 [06F2PGPXVAYRBC94RQ7X5V4DVG] lease claim po-critic`, and `git diff --name-only 08da25d61f1c104155b9cf1811fd15276e801877..ticket/06F2PGPXVAYRBC94RQ7X5V4DVG-task-update-v0-15-0-documentation-and-release-no` returned no files.
- The refined contract is already more specific than the legacy draft and is sufficient for a docs-only dev handoff.

Split recommendations
- No split recommended. This remains a bounded docs-only consolidation across README, `docs/releases/v0.15.0.md`, and current-baseline adopter guidance.
- If the v0.14.0-to-v0.15.0 cleanup expands into a wider architecture-doc sweep beyond the current baseline, track that as a separate follow-up instead of widening this ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment