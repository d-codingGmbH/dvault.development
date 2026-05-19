[gicket-bot] PO-critic review contract

Summary
- The persisted contract now closes the prior hierarchy-depth ambiguity and is ready for developer handoff; remaining items are implementation and test watchouts, not PO blockers.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/description.md now sets `## Open Questions` to `- none` and defines hierarchy maintenance as one row per ancestor/descendant pair with minimum positive `TraversalDepth`, direct-edge depth `1`, and shorter-path incremental updates.
- The prior blocking review is recorded in .gicket/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/comments/06F3S7FPTNA1R20FWEASW5M9F4.md, and the follow-up refinement in .../comments/06F3S9RKHEP96D5KE0NTV9FMD4.md marks `critic-item-1` through `critic-item-5` as `answered`.
- README.md, docs/releases/v0.7.0.md, and docs/production-adoption-checklist.md all explicitly state that bridge reads operate over already materialized tables and do not maintain bridge rows.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs seeds `BridgeCustomerOrder` and `BridgeSalesRegionHierarchy` with raw SQL `INSERT` statements, confirming current bridge population is manual and this story fills that gap.
- tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt and docs/plans/bridge-metadata-v1-contract.md define `BridgeSalesRegionHierarchy` with a primary key only on ancestor/descendant plus separate `TraversalDepth`, which makes the shortest-path rule materially necessary.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs currently registers `IDataVaultSaveService` and `IDataVaultReadService`; src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs and DataVaultReadServiceRegistryExtensions.cs show the existing bridge surface is read-only and already supports registry-backed bridge-name resolution.
- .gicket/relations/B4/T8/06F2PGP7HM8F39K3J0H5JHB3B4--06F2PGPGXMJ3W8FR9JZHH3PJT8--parentOf.json and .gicket/relations/T8/XC/06F2PGPGXMJ3W8FR9JZHH3PJT8--06F2PGPKXWRFXNPFA1JR0X67XC--blocks.json plus .../T8/VG/...--06F2PGPXVAYRBC94RQ7X5V4DVG--blocks.json confirm the epic and downstream dependency context.
- .gicket/relations/5W/T8/06F2PGMFWSEC95ATBCGZ6HYT5W--06F2PGPGXMJ3W8FR9JZHH3PJT8--blocks.json still exists, but .gicket/tickets/06F2PGMFWSEC95ATBCGZ6HYT5W/ticket.json shows that epic is `done`, so the contract's treatment of it as historical ordering is consistent with repository state.
- .gicket/releases/06F2PH9JDHV1GS17K0Y5128E5W.json names release `v0.15.0 - Maintenance and Query Operations`, and .gicket/milestones/06F2PH9K7EFYSNWBKWWHAMWDTC.json names milestone `v0.15.0 - PIT and bridge maintenance`.
- `git log --oneline develop..HEAD` shows only ticket orchestration commits for 06F2PGPGXMJ3W8FR9JZHH3PJT8, and `git diff --name-only develop...HEAD` lists only `.gicket/tickets/06F2PGPGXMJ3W8FR9JZHH3PJT8/**`, which is consistent with a pre-development PO gate rather than an in-flight implementation review.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No worked example shows how cyclic recursive source-link data or explicit ancestor==descendant source data should behave under the documented `shortest positive path` plus `no implicit self rows` rule.
- No worked example shows the exact deterministic failure shape for registry-backed maintenance when a bridge name is missing versus present-but-unsupported.

Risky assumptions
- Assumes shortest-path semantics plus the `no implicit self rows` rule are sufficient for cyclic source data without another refinement ticket.
- Assumes the v0.15.0 release-note delta will be created during implementation; `docs/releases/` currently stops at `v0.14.0`.
- Assumes the new maintenance API can be introduced beside the current extension-based bridge read surface without forcing a broader public API redesign.

AC / test suggestions
- Keep a hierarchy rebuild test where two recursive paths reach the same ancestor/descendant pair at different depths and the stored `TraversalDepth` equals the minimum hop count.
- Keep an incremental hierarchy test where later link ingestion shortens an existing path and forces a downward depth update, plus an equal-or-longer no-op companion.
- Add a convergence check proving rebuild and incremental maintenance produce identical rows for the same persisted source-link state, including registry-backed bridge-name resolution.
- Add one SQLite integration path that starts from only source-link rows and proves bridge reads work without manual bridge-table seeding.

Implementation watchouts
- `AddDVault()` in `src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs` currently registers only save/read services; bridge maintenance needs explicit DI registration in the same startup path.
- Current bridge reads are extension methods in `DataVaultReadServiceBridgeExtensions.cs` and `DataVaultReadServiceRegistryExtensions.cs`, while `IDataVaultReadService.cs` itself does not declare bridge methods; keep the new maintenance surface consistent without regressing the existing read helper contract.
- Current read-contract tests manually seed bridge rows in `tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs`; add maintenance-specific coverage instead of weakening or deleting those read tests.
- Hierarchy storage is keyed only by ancestor/descendant in `SqliteDataVaultSchemaSnapshot.txt`, so rebuild/incremental logic must suppress duplicate pairs and lower stored depth when a shorter path appears.

Non-blocking notes
- `git diff --stat 405e11b47..44b931f2e` touches only the ticket description, comments, events, and ticket metadata; no relation files changed, which matches the contract statement that no child tickets or relation changes were materialized during refinement.
- The parent epic, downstream blocked stories, and active v0.15.0 release/milestone alignment are already in place, so developer handoff does not require a split or new dependency ticket.

Split recommendations
- No split recommended; sibling tickets already isolate PIT maintenance, query API follow-up, provider-aware read optimization, and broader v0.15.0 documentation.
- If delete-aware or topology-shrinking incremental hierarchy maintenance becomes necessary, track it as a separate follow-up instead of widening this v1 story.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment