[gicket-bot] PO-critic review contract

Summary
- Return to PO: the contract still frames this as new dev work, but the current branch already contains public latest/as-of and bridge helper APIs, docs, release notes, and tests; the ticket needs duplicate/closure cleanup or a narrowly defined remaining gap.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F1XPXJW79K94G4WG86AG2X6M/description.md still asks to add explicit public helper APIs for latest/current, as-of, and bridge reads and says the incoming blocks relation from 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 remains unresolved risk.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs already exposes public ReadLatestSatelliteAsync<TProjection>(..., DataVaultLatestSatelliteReadRequest, ...) over IDataVaultReadService.
- src/DCoding.Data.DVault/DataVaultReadServiceBridgeExtensions.cs already exposes public ReadBridgeRowsAsync(...) and ReadBridgeAsync<TProjection>(...), and src/DCoding.Data.DVault/DataVaultReadServiceRegistryExtensions.cs already exposes the registry-backed bridge and latest/as-of adapters.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt lines 588-598 include ReadBridgeRowsAsync, ReadBridgeAsync, registry-backed ReadBridgeRowsAsync, registry-backed ReadLatestSatelliteAsync, and explicit ReadLatestSatelliteAsync in the approved public API.
- README.md already documents typed latest/as-of reads and bridge reads, including exact bridge column names and TraversalDepth; docs/releases/v0.7.0.md documents the same helper surfaces as implemented branch baseline.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedSatelliteReadServiceSqliteTests.cs covers explicit latest and registry-backed as-of typed reads, tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs covers many-to-many and hierarchy bridge helpers, and tests/DCoding.Data.DVault.Tests/Unit/DataVaultBridgeReadServiceTests.cs covers unsupported-shape diagnostics.
- Branch history shows no implementation delta on this PO branch: git diff --name-only 73b48ebf7c6b5c3e2ee233b050160b67104c8403..HEAD returned no files, and git log --oneline -5 shows only workflow commits above develop commit 9d88fd394 [06F1XPXY7QKTYAW43JTT3BM704] AUTO-INTEGRATION squash into develop.
- A direct source search for ReadCurrent, CurrentSatellite, Current.*Read, Latest/current, and current/latest under src, README.md, docs, and tests returned no matches, so there is no direct source evidence of a separate current-named public API beyond the existing latest/as-of helper surface.

Blocking findings
- The delivery contract is stale against the current branch baseline: it asks dev to add helper APIs, docs, examples, and diagnostics that are already present in source, public API snapshot, README, release notes, and tests.
- The ticket's live-planning narrative is outdated: it still treats 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 as unresolved risk and keeps 06F1XPXY7QKTYAW43JTT3BM704 as ongoing implementation context, but both related tickets are already persisted as done.
- The remaining product delta is ambiguous: the repo already has latest/as-of and bridge helpers, but there is no direct source evidence of a separate current-named API, so PO must say whether this ticket is duplicate/retirement work or a new narrow naming/docs gap.

Required PO actions
- Rewrite this ticket as duplicate/already-satisfied backlog cleanup, or replace it with a narrowly scoped new ticket that names the exact missing delta instead of re-requesting the existing helper surface.
- Remove or correct stale relation/risk prose that says blocker 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 is unresolved and that child 06F1XPXY7QKTYAW43JTT3BM704 is still live implementation scope.
- If PO keeps the ticket open, state explicitly whether a separate current-named public API is required beyond ReadLatestSatelliteAsync(...), and name the exact missing docs/examples/tests that are not already covered by the current baseline.

Open issues ledger
- critic-item-1 [required-po-action] Rewrite this ticket as duplicate/already-satisfied backlog cleanup, or replace it with a narrowly scoped new ticket that names the exact missing delta instead of re-requesting the existing helper surface.
- critic-item-2 [required-po-action] Remove or correct stale relation/risk prose that says blocker 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 is unresolved and that child 06F1XPXY7QKTYAW43JTT3BM704 is still live implementation scope.
- critic-item-3 [required-po-action] If PO keeps the ticket open, state explicitly whether a separate current-named public API is required beyond ReadLatestSatelliteAsync(...), and name the exact missing docs/examples/tests that are not already covered by the current baseline.
- critic-item-4 [blocking-finding] The delivery contract is stale against the current branch baseline: it asks dev to add helper APIs, docs, examples, and diagnostics that are already present in source, public API snapshot, README, release notes, and tests.
- critic-item-5 [blocking-finding] The ticket's live-planning narrative is outdated: it still treats 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 as unresolved risk and keeps 06F1XPXY7QKTYAW43JTT3BM704 as ongoing implementation context, but both related tickets are already persisted as done.
- critic-item-6 [blocking-finding] The remaining product delta is ambiguous: the repo already has latest/as-of and bridge helpers, but there is no direct source evidence of a separate current-named API, so PO must say whether this ticket is duplicate/retirement work or a new narrow naming/docs gap.

Missing examples / edge cases
- The contract gives no concrete example of a caller need that is still unmet by the existing ReadLatestSatelliteAsync(...) and ReadBridgeAsync(...) surfaces.
- The contract does not show a ticket-level example that distinguishes a desired current alias from the already documented latest/as-of semantics, so devs cannot tell whether any real behavior or API gap remains.

Risky assumptions
- Assuming existing latest/as-of and bridge helper APIs still count as future implementation work instead of already delivered baseline.
- Assuming current can be treated as a synonym for latest without explicitly deciding whether product wants a separate public API name, only docs language, or no additional work.
- Assuming the incoming blocker risk is still active even though 06F1XPRY3ZDB6W1WQ9ABRRJ2V4/ticket.json is already done.

AC / test suggestions
- If this becomes a replacement ticket, acceptance criteria should cite the exact missing symbol or artifact, for example a current-named alias, a specific missing example, or a specific missing diagnostic, and should treat ReadLatestSatelliteAsync, ReadBridgeAsync, README advanced-read docs, and existing tests as baseline evidence rather than new work.
- A narrowed ticket should explicitly say whether existing bridge helper diagnostics in DataVaultBridgeReadServiceTests and read-strategy diagnostics in DataVaultDiagnosticsIntegrationTests are sufficient baseline or whether a named additional case is required.

Implementation watchouts
- Do not reopen PIT-backed reads, PIT/bridge maintenance, provider-specific read optimization, or custom IQueryable scope when cleaning up this ticket.
- Do not send this to dev as a broad read-helper implementation story unless PO first removes the overlap with already shipped latest/as-of and bridge helper surfaces.

Non-blocking notes
- Open Questions are already none in .gicket/tickets/06F1XPXJW79K94G4WG86AG2X6M/description.md.
- The branch tip already equals the provided scratch source ref 73b48ebf7c6b5c3e2ee233b050160b67104c8403, so this PO branch added ticket-workflow state, not new source changes for the requested helper surface.

Split recommendations
- Do not split new dev work out of this ticket until PO first decides whether the correct action is retirement/closure or a new narrowly scoped gap ticket.
- If a real remaining gap exists, create one fresh ticket for that exact gap instead of reusing this broad implementation story whose scope overlaps current repository baseline.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment