[gicket-bot] PO-critic review contract

Summary
- Approve for developer handoff: the epic is closed at PO level and backed by landed child outcomes plus matching repository code, tests, benchmark artifacts, and release documentation.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- Persisted ticket 06F5Q8WVYMV8KQPAENPEEE3YM4 has Open Questions = none, PO Handoff = ready_for_po_critic, and clarifies that child tickets 06F5Q8X261DQHG7N1445NGXB5W, 06F5Q8X8Q72TQ5B7F2JSAJWPR8, 06F5Q8XF9DPKFW9VY0F3Y32BH4, 06F5Q8XPXEQPJTKGJ7BQGCY438, 06F5Q8XXSBGW1B8RDRMGVF557W, and 06F5Q8Y3WW9FFV7HA289VHCEAM are the authoritative split.
- src/DCoding.Data.DVault/DataVaultSaveService.cs on the current branch defines IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken), DataVaultChunkedSaveRequest, DataVaultSaveChunk, and DefaultChunkedRetainedSatelliteSeriesLimit = 10000.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs keeps AddDVault() as the explicit registration path and registers IDataVaultSaveService.
- tests/DCoding.Data.DVault.Tests/Unit/StreamingExplicitSaveContractSnapshotTests.cs locks the streaming explicit-save contract and names the expected compatibility evidence set.
- rg -n on tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs found production DataVaultChunkedSaveRequest coverage at lines 864, 867, 922, 979, 1018, 1051, 1118, 1163, 1244, 1316, 1380, and 1438, with retained-state fallback assertions at lines <redacted> for RetainedSatelliteSeriesLimitReached and RetainedSatelliteSeriesLimitExceeded.
- benchmark-summary.csv lines 11-13 contain customer-profile-streaming-save rows for materialized-explicit-bulk, chunked-save-bounded-10, and chunked-save-bounded-5, each exposing chunk counts and retainedStateHighWater in executionDetail.
- docs/releases/v0.19.0.md states intended release date 2026-05-25, documents the additive chunked-save baseline, and points to the root benchmark triplet as the v0.19.0 streaming-save evidence.
- gicket-read-ticket-comments for 06F5Q8WVYMV8KQPAENPEEE3YM4 returned only automation/lease and PO handoff comments; no human clarification thread or reopen signal was present.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- none

Risky assumptions
- Workflow automation should treat this as a coordination epic over landed child work; the current owner-branch head is the PO-critic claim commit, so epic readiness depends on the repository baseline and done child tickets rather than a new epic-specific implementation diff.
- Future work must continue to keep provider-specific chunk optimization, ingestion/orchestration, and NuGet publication approval outside this v0.19.0 epic baseline, as the persisted contract and release docs currently do.

AC / test suggestions
- If the epic is ever reopened, require fresh acceptance criteria that explicitly create separate follow-up tickets for provider-specific chunk optimization or ingestion/orchestration instead of widening this roll-up.
- Keep future release-facing performance claims tied to checked-in before/after artifact bundles that satisfy docs/plans/performance-evidence-benchmark-artifact-contract.md, not just summary prose.

Implementation watchouts
- Treat the epic as a roll-up ticket; the technical proof already lives in the child tickets and repository artifacts named in the contract.
- Do not use this epic to reopen API shape, provider-native chunk optimization, background ingestion, scheduler/queue/file/CDC orchestration, or publication-approval scope that the contract explicitly marks out.

Non-blocking notes
- The review surface matches the provided scratch source exactly at e0bd6526c878a1fd89f7d3f93a7e4b66615a1544.

Split recommendations
- No additional split is needed for the current v0.19.0 streaming-save baseline.
- Open separate future epic/story work for provider-specific chunk optimization or loader orchestration instead of widening 06F5Q8WVYMV8KQPAENPEEE3YM4.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment