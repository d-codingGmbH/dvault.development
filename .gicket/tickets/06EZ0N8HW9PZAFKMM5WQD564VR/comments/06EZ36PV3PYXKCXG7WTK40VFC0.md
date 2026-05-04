[gicket-bot] PO-critic review contract

Summary
- Refinement is concrete, bounded to existing source surfaces, and ready for developer handoff; remaining gaps are implementation/test watchouts already captured in the contract rather than PO ambiguities.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0N8HW9PZAFKMM5WQD564VR/description.md has Open Questions = none and explicitly states descending Priority evaluation, first-compatible win, registration-order tie behavior, and SQLite-only v0.5 optimization scope.
- .gicket/tickets/06EZ0N8HW9PZAFKMM5WQD564VR/comments/06EZ34ZAW18T24B6Z1GG0RGX18.md records PO handoff decision ready_for_po_critic; comment 06EZ350NQBJM42DM93FEP11SWC.md says the durable contract and labels were updated.
- src/DCoding.Data.DVault/DataVaultSaveService.cs sorts provider strategies with OrderByDescending(strategy => strategy.Priority) and falls back to the provider-neutral writer when no strategy CanSave.
- src/DCoding.Data.DVault.Sqlite/DVaultSqliteServiceCollectionExtensions.cs registers SqliteDataVaultSaveStrategy; src/DCoding.Data.DVault.Postgres/DVaultPostgresServiceCollectionExtensions.cs, src/DCoding.Data.DVault.SqlServer/DVaultSqlServerServiceCollectionExtensions.cs, src/DCoding.Data.DVault.Oracle/DVaultOracleServiceCollectionExtensions.cs, and src/DCoding.Data.DVault.MySql/DVaultMySqlServiceCollectionExtensions.cs only call services.AddDVault().
- docs/architecture/dvault-v1-explicit-save-service.md marks SQLite as the only v0.5 provider with required optimized save behavior and set-based existence checks; PostgreSQL, SQL Server, Oracle, and MySQL are compatibility-only.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultSaveStrategySelectionTests.cs already covers missing-registration fallback, unknown-provider rejection, and compatible SQLite selection; rg search for equal-priority/registration-order/tie-break matched only src/DCoding.Data.DVault/DataVaultSaveService.cs:373, so same-priority determinism is still an explicit work item rather than an unstated PO gap.
- tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs and docs/quality/api-surface-snapshots.md confirm public API snapshot gates exist for DCoding.Data.DVault and all five provider packages if this story changes public contracts.
- git diff --stat 559f1b2f625a6f2902cc997ca4d85fc9567bb305..HEAD was empty, and git diff --stat develop..HEAD -- . ':(exclude).gicket' was also empty; the story branch currently contains ticket workflow commits, not code drift.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- Concrete same-provider, same-Priority, both-compatible strategy example to prove registration-order winner selection.
- Batch-request case where CanSave depends on the ordered IReadOnlyList<DataVaultSaveRequest>, not just DbContext.ProviderName.
- Compatible-provider but dirty ChangeTracker case, because SqliteDataVaultSaveStrategy declines when entries are Added, Modified, or Deleted and the fallback path should stay deterministic.

Risky assumptions
- Equal-priority tie behavior currently depends on stable LINQ ordering plus DI registration order; source shows the sort, but the repo does not yet have a direct equal-priority proof outside the ticket contract.
- DataVaultEfMetadataTranslator currently hardcodes DataVaultProviderCapabilityProfiles.Sqlite, so implementers could accidentally widen provider-aware EF metadata scope while touching provider contracts.
- Because only SQLite currently has an optimized executor, some shared-contract or dispatch edge cases may stay latent until a second provider adopts the strategy boundary.

AC / test suggestions
- Require one test with two compatible strategies at the same Priority and a deterministic winner assertion by registration order.
- Require one test where a strategy declines because of request-batch shape, not provider mismatch, to prove CanSave evaluates the ordered request list.
- If any public core type or member changes, name the exact snapshot files expected to change under tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/.

Implementation watchouts
- Keep provider-name checks and SQL inside provider packages; the core dispatcher in src/DCoding.Data.DVault/DataVaultSaveService.cs should remain strategy-agnostic.
- SqliteDataVaultSaveStrategy opens or joins DbConnection and DbTransaction directly and forwards CancellationToken; shared contract work must not leak provider-specific execution details into public API without snapshot updates.
- Non-SQLite provider packages currently expose only AddDVaultXxx registration surfaces, so compatibility-only baseline behavior must remain intact when adding richer strategy tests or docs.

Non-blocking notes
- Child tickets 06EZ0N90QDR6X6XDMSK88X5NBR, 06EZ0N9AM9AJ3AB8DQ6Y1JBS28, and 06EZ0N9KXZY8BPQN84NV3WDYCG are already done, which supports the current docs/test split.
- The current story branch is ticket-metadata-only relative to develop, so PO review is evaluating the persisted contract against the already-merged repository baseline rather than an unmerged code branch.

Split recommendations
- Keep the current split: one contract story, bounded docs/test child tasks, and separate provider-specific optimization stories.
- Only split further if provider-capability-profile expansion or non-SQLite metadata translation is pulled into scope; both are explicitly deferred today.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment