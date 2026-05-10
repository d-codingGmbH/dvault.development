[gicket-bot] PO-critic review contract

Summary
- Ticket contract is sufficiently defined for developer handoff.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06F0MEBV90FB8TQMRXJNH078BM/description.md contains '### PO Handoff' with decision 'ready_for_po_critic' and '## Open Questions' followed by '- none'.
- git diff --name-only develop...HEAD listed only .gicket/tickets/06F0MEBV90FB8TQMRXJNH078BM/* files, and git log --oneline develop..HEAD showed only claim/handoff commits e6c444c78, d4c772cd4, 151da5317, and d08c92860.
- src/DCoding.Data.DVault/IDataVaultHubMapper.cs, IDataVaultLinkMapper.cs, and IDataVaultSatelliteMapper.cs define the typed mapper contracts the story depends on; IDataVaultLinkMapper.cs explicitly documents the v1 same-hub/self-link limitation.
- src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs already exposes SaveHubAsync, SaveHubsAsync, SaveLinkAsync, SaveLinksAsync, SaveOrdinaryHubSatelliteAsync, and SaveOrdinaryHubSatellitesAsync with explicit loadTimestamp and recordSource parameters.
- src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs, DataVaultReadServiceRegistryExtensions.cs, and DataVaultSatelliteProjectionRow.cs provide explicit and registry-backed typed latest/as-of projection helpers, exact-name accessors, and reserved-name validation.
- tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs covers hub-then-ordinary-satellite flow; DataVaultTypedSatelliteReadServiceSqliteTests.cs covers explicit vs registry reads, link-parent reads, multi-active reads, null/missing diagnostics, and load-timestamp normalization; DataVaultSaveStrategySelectionTests.cs includes TypedHubSaveHelperPreservesSqliteOptimizedStrategyDispatch.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- A story-level example showing the two-step hub-save then ordinary-satellite-save flow would speed handoff, although the behavior is already demonstrated in tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs.
- A brief story-level note that as-of behavior rides on the optional AsOf value in DataVaultLatestSatelliteReadRequest and DataVaultRegistryLatestSatelliteReadRequest, not a separately named ReadAsOf method, would reduce naming ambiguity.

Risky assumptions
- This approval assumes the parent story is intended to hand off as story-level coordination/closure work, because the current story branch is ticket-metadata-only while the relevant implementation evidence already lives on develop and in done child tickets.

AC / test suggestions
- Use DataVaultTypedMapperSaveServiceSqliteTests, DataVaultTypedSatelliteReadServiceSqliteTests, and the TypedHubSaveHelperPreservesSqliteOptimizedStrategyDispatch case in DataVaultSaveStrategySelectionTests as the minimum story-level acceptance pack for handoff or closure.
- Keep acceptance explicitly tied to the documented v1 boundaries already present in the repository: ordinary hub-parent save convenience only, explicit loadTimestamp and recordSource inputs, same-hub/self-link typed link exclusion, and reserved-name rejection on typed reads.

Implementation watchouts
- Do not widen convenience save scope beyond ordinary hub-parent satellites; link-parent and multi-active satellite convenience remain outside this v1 story.
- Keep loadTimestamp and recordSource explicit at the helper call site; do not hide them behind ambient policy or SaveChanges interception.
- Preserve the typed link boundary that excludes same-hub/self-link participant shapes unless a separate ticket changes participant identity.
- Keep ParentHashKey, HashDiff, LoadTimestamp, and RecordSource reserved in typed satellite projection name space.

Non-blocking notes
- The existing split already matches the repository seams cleanly: mapper contract, save-helper layer, and read-projection layer are separated across the three child tickets.
- Because develop...HEAD contains only ticket metadata changes, developer handoff should treat this ticket as an umbrella over already-established repository surfaces, not as a branch carrying fresh implementation.

Split recommendations
- No additional split recommended; the existing child-ticket breakdown across 06F0MEC7FEXAD069AJNYZW0DRM, 06F0MECFNF42NK9PND9DWVW9VW, and 06F0MECPFAVBFBNC5XMVDZRQ6M already matches the visible API boundaries in src/DCoding.Data.DVault.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment