[gicket-bot] PO refinement contract

Summary
- Replaced unsupported API inferences with source-backed save/read surface details and explicit net-new internal tracing wording; no child tickets, relation edits, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated to anchor scope only to source-visible save/read interfaces and helper extensions on the current branch, and to treat any tracing holder/helper as net-new code instead of an inferred existing API.
- critic-item-2: `answered` - The refined contract no longer implies an unsupported public read or tracing API. Bridge coverage is explicitly scoped to existing public helper extensions and their internal DefaultDataVaultReadService or pipeline execution branches, while the tracing source/helper may be created as new internal code.
- critic-item-3: `answered` - Source-backed read coverage is now enumerated precisely: IDataVaultReadService.ReadLatestSatelliteRowsAsync, current/as-of helpers, typed latest/as-of projection helpers, registry latest-satellite helpers, IDataVaultReadService.ReadPitRowsAsync, DataVaultReadServicePitExtensions.ReadPitAsync, and public bridge raw/typed helpers in the bridge and registry extension classes.
- critic-item-4: `answered` - The contract now says no visible pre-existing public tracing API is required. This story may introduce a new internal ActivitySource holder/helper using the contract source name DCoding.Data.DVault, and any deliberate public tracing surface must update the approved public API snapshots in the same change.

Clarifications
- This story may add a new internal ActivitySource holder/helper using the contract source name DCoding.Data.DVault; no pre-existing public tracing API is assumed.
- Save coverage is source-backed by the three public IDataVaultSaveService.SaveAsync overloads over DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest in src/DCoding.Data.DVault/DataVaultSaveService.cs.
- Latest-satellite helper coverage is source-backed by IDataVaultReadService.ReadLatestSatelliteRowsAsync plus the current/as-of, typed-projection, and registry latest-satellite helpers in DataVaultReadServiceCurrentSatelliteExtensions.cs, DataVaultReadServiceTypedProjectionExtensions.cs, and DataVaultReadServiceRegistryExtensions.cs.
- PIT helper coverage is source-backed by IDataVaultReadService.ReadPitRowsAsync plus DataVaultReadServicePitExtensions.ReadPitAsync.
- Bridge helper coverage is source-backed by the public raw and typed bridge helpers in DataVaultReadServiceBridgeExtensions.cs and DataVaultReadServiceRegistryExtensions.cs; those helpers dispatch through internal DefaultDataVaultReadService bridge methods when available and otherwise fall back to DataVaultBridgeReadPipeline.
- No child tickets, relation edits, attachments, or planning documents were materialized; live relations were reviewed and left unchanged in this refinement pass.

Scope In
- Add one repo-owned System.Diagnostics.ActivitySource named DCoding.Data.DVault as net-new internal core code unless a deliberate public API addition is intentionally snapshot-reviewed.
- Instrument the three explicit IDataVaultSaveService.SaveAsync boundaries with span names dvault.save.single_request, dvault.save.bulk_request, and dvault.save.chunked_request.
- Instrument latest-satellite and PIT explicit read boundaries plus the existing public helper surfaces for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads so each execution emits one root save/read span without duplicate wrapper spans.
- Reuse existing bounded telemetry and diagnostics surfaces for provider name, strategy status and type, request counts, row counts, chunk counts, duration bucket, fallback causes, outcome, and bounded failure classification.
- Add focused tests for no-listener behavior, sampled-listener behavior, success, fault, cancellation, bridge fallback, helper non-duplication, redaction, telemetry compatibility, and public API snapshot compatibility.

Scope Out
- Do not assume or require any pre-existing public tracing API; if a holder/helper is added by this story it stays internal unless an intentional public API change is snapshot-reviewed.
- Do not add new public bridge or tracing methods to IDataVaultReadService solely for this story; bridge coverage stays on the existing helper surfaces unless a separate API-shaping change is intentionally reviewed.
- Do not instrument PIT or bridge maintenance operations; those remain in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and the repository helper surfaces.

Open questions
- none

Follow-up questions
- If consumers later need a public code-facing ActivitySource accessor instead of listener registration by source name, should that be handled in a separate API-shaping ticket with intentional snapshot review?

Risks
- If tracing is added only around ReadLatestSatelliteRowsAsync, typed or helper-based latest-satellite executions can bypass that hook and miss spans because they may execute through IDataVaultSatelliteProjectionReadService or DataVaultSatelliteReadPipeline.
- If bridge tracing is added only inside one branch, callers that hit the other branch in DataVaultReadServiceBridgeExtensions can miss dvault.read.bridge spans.
- If helper layers add wrapper spans instead of reusing the underlying explicit or projection execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.

Split recommendations
- No split is recommended; current branch evidence supports one bounded story for explicit save/read tracing, while PIT and bridge maintenance tracing remains separate in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment