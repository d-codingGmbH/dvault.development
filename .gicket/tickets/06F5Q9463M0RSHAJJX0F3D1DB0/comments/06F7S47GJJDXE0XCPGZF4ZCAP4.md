[gicket-bot] PO refinement contract

Summary
- Refinement removes the unsupported existing-tracing-API assumption and constrains the story to source-backed save/read boundaries; no child tickets, relation changes, attachments, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated so tracing is net-new implementation work. The branch shows the required tracing contract document and existing save/read boundaries, but it does not show any existing DVault ActivitySource holder or public tracing API; this story may add that missing holder/helper as internal core code.
- critic-item-2: `answered` - The revised contract no longer depends on a pre-existing public tracing type. It anchors instrumentation to source-backed boundaries already present on the branch: the three IDataVaultSaveService.SaveAsync overloads, IDataVaultReadService.ReadLatestSatelliteRowsAsync, IDataVaultReadService.ReadPitRowsAsync, and the existing bridge helper paths.
- critic-item-3: `answered` - The unsupported inferred API claim is removed. Current, as-of, typed-projection, registry, PIT, and bridge helper surfaces are evidenced as extension or projection paths over the explicit read boundary, while existing telemetry types and service-registration helpers provide the bounded provider, strategy, count, duration, and fallback data that tracing must reuse.

Clarifications
- This story explicitly introduces DVault Activity tracing as net-new repo-owned core code; any shared ActivitySource holder/helper may be created by this ticket and should stay internal unless a deliberate public API change is made with snapshot updates.
- Source-backed save coverage is the three public IDataVaultSaveService.SaveAsync overloads over DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest.
- Source-backed read coverage is the public IDataVaultReadService latest-satellite and PIT methods plus the existing public current/as-of, typed-projection, registry, and bridge extension helpers already present in src/DCoding.Data.DVault.
- Bridge helper execution is bifurcated today: DataVaultReadServiceBridgeExtensions routes through DefaultDataVaultReadService when available and otherwise falls back directly to DataVaultBridgeReadPipeline; tracing must cover both branches as one dvault.read.bridge contract.
- This refinement pass only tightens the contract to source-backed evidence; no child tickets, relation edits, attachments, or planning documents were materialized.

Scope In
- Add one repo-owned System.Diagnostics.ActivitySource named DCoding.Data.DVault as net-new internal core code unless a deliberate public API addition is intentionally made and snapshot-reviewed.
- Instrument the three explicit IDataVaultSaveService.SaveAsync boundaries with span names dvault.save.single_request, dvault.save.bulk_request, and dvault.save.chunked_request.
- Instrument latest-satellite and PIT explicit read execution plus the existing current/as-of, typed-projection, registry, and bridge helper paths so each explicit execution emits one root save/read span without duplicate wrapper spans.
- Reuse existing bounded telemetry and diagnostics surfaces for provider name, strategy status and type, request counts, row counts, chunk counts, duration bucket, fallback causes, outcome, and bounded failure classification.
- Add focused tests for no-listener behavior, sampled-listener behavior, success, fault, cancellation, bridge fallback, helper non-duplication, redaction, telemetry compatibility, and public API snapshot compatibility.

Scope Out
- Do not assume or require any pre-existing public tracing API; creating the missing ActivitySource holder/helper is part of this story when kept internal or intentionally snapshot-reviewed.
- Do not instrument PIT or bridge maintenance operations; those remain in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and the repository's helper surfaces.

Open questions
- none

Follow-up questions
- If consumers later need a public code-facing ActivitySource accessor instead of listener registration by source name, should that be handled in a separate API-shaping ticket with intentional snapshot review?

Risks
- If tracing is added only around ReadLatestSatelliteRowsAsync, typed latest-satellite projection helpers can bypass that hook and miss spans because they may execute through IDataVaultSatelliteProjectionReadService or DataVaultSatelliteReadPipeline.
- If bridge tracing is added only inside one branch, callers that hit the other branch in DataVaultReadServiceBridgeExtensions can miss dvault.read.bridge spans.
- If helper layers add wrapper spans instead of reusing the underlying explicit or projection execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.

Split recommendations
- No split is recommended; current branch evidence supports one bounded story for explicit save/read tracing, while PIT and bridge maintenance tracing remains separate in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment