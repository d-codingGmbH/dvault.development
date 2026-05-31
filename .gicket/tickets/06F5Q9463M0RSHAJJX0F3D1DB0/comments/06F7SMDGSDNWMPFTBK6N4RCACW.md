[gicket-bot] PO refinement contract

Summary
- Verified the current-branch save/read surfaces against repository source, confirmed the authoritative description update already removed unsupported API inferences, and made no additional child-ticket, relation, attachment, or planning-document writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract has been restated with source-backed boundaries from the current branch. It now cites the actual save and read surfaces and explicitly treats any tracing holder/helper as net-new internal code unless a separate public API change is intentionally snapshot-reviewed.
- critic-item-2: `answered` - The persisted contract no longer infers an existing public tracing API or bridge method on IDataVaultReadService. Public save coverage is anchored to the three IDataVaultSaveService.SaveAsync overloads, and public read coverage is anchored to IDataVaultReadService.ReadLatestSatelliteRowsAsync and ReadPitRowsAsync plus the already-public helper extensions for current/as-of, typed projection, registry latest-satellite, PIT projection, and bridge reads.
- critic-item-3: `answered` - The previously unsupported helper list is now backed by concrete source. Current/as-of helpers delegate to ReadLatestSatelliteRowsAsync or ReadLatestSatelliteAsync, PIT typed projection delegates to ReadPitRowsAsync, and bridge raw or typed helpers dispatch through public extension methods to internal DefaultDataVaultReadService bridge methods when available or to DataVaultBridgeReadPipeline otherwise. The contract therefore requires one root span per execution across those entry points without asserting a nonexistent public bridge boundary on IDataVaultReadService itself.
- critic-item-4: `answered` - Confirmed. Current branch evidence does not show an existing public tracing API or ActivitySource accessor. The contract now states that this story may add a new internal ActivitySource holder/helper named DCoding.Data.DVault, and any intentional public tracing surface would require the same-change public API snapshot update and review.

Clarifications
- The authoritative ticket description was already updated on the ticket owner branch in revision 06F7SJN5DZ6DJ5X2B6S8AYC3QC to replace unsupported API inferences with source-backed save/read surfaces.
- Current public read-interface evidence is narrower than the earlier inference: IDataVaultReadService itself exposes ReadLatestSatelliteRowsAsync and ReadPitRowsAsync, while bridge coverage lives on existing public extension helpers rather than a bridge method on the interface.
- Current/as-of satellite helpers, latest-satellite typed projection, registry latest-satellite helpers, PIT typed projection, and bridge raw or typed helpers are all visible in the current branch and public API snapshot, so those helper entry points can be named explicitly without inventing new public surfaces.
- No additional child tickets, relation edits, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Add one repo-owned System.Diagnostics.ActivitySource named DCoding.Data.DVault as net-new internal core code unless a deliberate public API addition is intentionally snapshot-reviewed.
- Instrument the three explicit IDataVaultSaveService.SaveAsync boundaries with span names dvault.save.single_request, dvault.save.bulk_request, and dvault.save.chunked_request.
- Instrument the explicit latest-satellite and PIT read boundaries plus the existing public helper surfaces for current/as-of satellite reads, latest-satellite typed projection, registry latest-satellite helpers, PIT typed projection, and bridge reads so each execution emits exactly one root save or read span without duplicate wrapper spans.
- Reuse the existing bounded telemetry and diagnostics vocabulary for provider name, strategy status and type, request counts, row counts, chunk counts, duration bucket, fallback causes, outcome, and bounded failure classification.
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
- No split is recommended; current branch evidence supports one bounded story for explicit save and read tracing, while PIT and bridge maintenance tracing remains separate in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

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