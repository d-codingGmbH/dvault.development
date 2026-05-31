[gicket-bot] PO refinement contract

Summary
- Verified the blocking PO-critic findings against the current ticket text, live relations, and current-branch source/public API snapshot. The corrected contract now names only source-backed save/read surfaces, treats the done contract ticket's blocks relation as historical, and needs no additional planning or ticket writes before returning to PO-critic.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract has already been narrowed to source-backed surfaces. IDataVaultReadService publicly exposes only ReadLatestSatelliteRowsAsync and ReadPitRowsAsync, while current/as-of satellite, latest-satellite typed projection, registry latest-satellite, PIT typed projection, and bridge raw/typed access are existing helper surfaces. No further ticket mutation was needed in this pass.
- critic-item-2: `answered` - The unsupported public-API inference is resolved by naming only visible surfaces. Bridge coverage is not a method on IDataVaultReadService; it is an existing public helper surface, and the contract now treats it that way.
- critic-item-3: `answered` - The read coverage can be anchored to visible code paths without inventing new APIs. Current/as-of satellite helpers delegate into ReadLatestSatelliteRowsAsync or ReadLatestSatelliteAsync; typed latest-satellite projection uses internal IDataVaultSatelliteProjectionReadService or DataVaultSatelliteReadPipeline; PIT typed projection delegates to ReadPitRowsAsync; bridge helpers dispatch through DefaultDataVaultReadService.ReadBridgeRowsAsync or ReadBridgeProjectionRowsAsync when available and otherwise DataVaultBridgeReadPipeline. The contract therefore requires one root span per execution across those helper delegations, not additional wrapper APIs.
- critic-item-4: `answered` - No existing public tracing API is visible in the current branch. The story may add a new internal ActivitySource holder/helper, and any deliberate public tracing surface would require same-change public API snapshot review.

Clarifications
- Current ticket description already contains the corrected source-backed restatement, and this pass applied no child-ticket, relation, attachment, description, or planning-document writes.
- IDataVaultReadService itself remains limited to ReadLatestSatelliteRowsAsync and ReadPitRowsAsync; bridge coverage stays on existing public helper extensions.
- Current/as-of satellite helpers, latest-satellite typed projection, registry latest-satellite helpers, PIT typed projection, and bridge raw/typed helpers are all confirmed in current source and the approved public API snapshot.

Scope In
- Add one repo-owned System.Diagnostics.ActivitySource named DCoding.Data.DVault as net-new internal core code unless a deliberate public API addition is intentionally snapshot-reviewed.
- Instrument the three explicit IDataVaultSaveService.SaveAsync boundaries with spans named dvault.save.single_request, dvault.save.bulk_request, and dvault.save.chunked_request.
- Instrument the explicit latest-satellite and PIT read boundaries plus the existing helper surfaces for current/as-of satellite reads, latest-satellite typed projection, registry latest-satellite helpers, PIT typed projection, and bridge reads so each execution emits exactly one root save or read span without duplicate wrapper spans.
- Reuse the existing bounded telemetry and diagnostics vocabulary for provider name, strategy status and type, request counts, row counts, chunk counts, duration bucket, fallback causes, outcome, and bounded failure classification.
- Add focused coverage for no-listener behavior, sampled-listener behavior, success, fault, cancellation, bridge fallback, helper non-duplication, redaction, telemetry compatibility, and public API snapshot compatibility.

Scope Out
- Do not assume or require any pre-existing public tracing API; any new holder/helper stays internal unless a public API change is intentionally snapshot-reviewed.
- Do not add new public bridge or tracing methods to IDataVaultReadService solely for this story; bridge coverage stays on the existing helper surfaces.
- Do not instrument PIT or bridge maintenance operations; those remain in 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and repository helper surfaces.

Open questions
- none

Follow-up questions
- If consumers later need a public code-facing ActivitySource accessor instead of listener registration by source name, should that be handled in a separate API-shaping ticket with intentional snapshot review?

Risks
- If tracing is added only around ReadLatestSatelliteRowsAsync, typed or helper-based latest-satellite executions can bypass that hook because they may run through IDataVaultSatelliteProjectionReadService or DataVaultSatelliteReadPipeline.
- If bridge tracing covers only one branch, callers that hit the other branch in DataVaultReadServiceBridgeExtensions can miss dvault.read.bridge spans.
- If helper layers add wrapper spans instead of reusing the underlying explicit or projection execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.

Split recommendations
- No split is recommended; current branch evidence supports one bounded story for explicit save and read tracing, while PIT and bridge maintenance tracing remains separate in 06F5Q94D0JDMMWDXSRGWX1E4F0.

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