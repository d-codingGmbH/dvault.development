[gicket-bot] PO refinement contract

Summary
- Source-backed refinement complete: this story is explicitly net-new internal Activity instrumentation over existing save/read boundaries, with no assumed pre-existing public tracing API.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced the inferred existing tracing API claim with a source-backed contract: the branch evidence supports the required ActivitySource name and span vocabulary, but it does not show any existing DVault ActivitySource holder or Activity-related public type. The ticket therefore treats tracing as net-new implementation work and explicitly allows adding the missing shared holder/helper, defaulting it to internal unless a deliberate public API change is made and snapshots are updated intentionally.
- critic-item-2: `answered` - The contract no longer depends on any pre-existing public tracing API or type. Instrumentation is anchored to source-backed entry points already visible in the branch: the three IDataVaultSaveService.SaveAsync overloads for single, bulk, and chunked saves; IDataVaultReadService.ReadLatestSatelliteRowsAsync and ReadPitRowsAsync; current/as-of/latest typed helpers that delegate into those paths; and bridge helper paths that execute through DefaultDataVaultReadService or directly through DataVaultBridgeReadPipeline.
- critic-item-3: `answered` - The reframed story as net-new internal Activity instrumentation is now source-backed. Current branch evidence shows no existing Activity tracing implementation to reuse, while the existing bounded telemetry surfaces already expose the provider, strategy, count, duration, and fallback data that the tracing contract can reuse for tags and events without inventing a new public API baseline.

Clarifications
- Current branch evidence shows no existing DVault ActivitySource holder or Activity-related public API, so this ticket explicitly introduces tracing as net-new repo-owned core code and defaults any shared holder/helper to internal unless a deliberate public API change is chosen.
- IDataVaultSaveService is the source-backed explicit save boundary and already exposes three overloads for single-request, ordered bulk, and chunked saves; those are the save entry points this ticket instruments.
- IDataVaultReadService currently exposes only ReadLatestSatelliteRowsAsync and ReadPitRowsAsync. Current/as-of helpers, latest typed projection helpers, and PIT typed projection helpers are public delegates over those underlying paths rather than separate interface members.
- Bridge reads are public extension helpers over IDataVaultReadService that execute through DefaultDataVaultReadService when available and otherwise fall back directly to DataVaultBridgeReadPipeline, so one dvault.read.bridge root span must cover both branches.
- Existing telemetry surfaces already provide bounded provider, strategy, count, duration, and fallback data; tracing must reuse those bounded values instead of creating a parallel diagnostics vocabulary or emitting raw request data.

Scope In
- Add a shared System.Diagnostics.ActivitySource named DCoding.Data.DVault in repo-owned core code, defaulting the holder/helper to internal unless an intentional public API decision is made.
- Instrument the repo-owned explicit save entry points behind IDataVaultSaveService.SaveAsync for single-request, ordered bulk, and chunked saves with the exact span names dvault.save.single_request, dvault.save.bulk_request, and dvault.save.chunked_request.
- Instrument latest/current/as-of satellite reads, PIT reads, and bridge reads across the existing explicit read service and public helper delegates so the visible read boundary emits one root span per explicit execution without duplicate wrapper spans.
- Populate only contract-approved tags and events by reusing existing bounded telemetry and diagnostics values for provider name, strategy status and type, fallback causes, counts, duration bucket, outcome, and bounded failure classification.
- Add focused unit and integration coverage for no-listener behavior, sampled-listener behavior, fault and cancellation completion, bridge fallback coverage, no duplicate spans from helper wrappers, redaction, telemetry compatibility, and public API snapshot compatibility.

Scope Out
- PIT and bridge maintenance tracing remains in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require instrumentation of consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and the repository's public helper paths.
- Do not add a new public ActivitySource holder unless that API change is chosen intentionally and the public API snapshots are updated in the same change.

Open questions
- none

Follow-up questions
- If consumers later need a public code-facing ActivitySource accessor instead of listener registration by source name, should that be handled in a separate API-shaping ticket with intentional public API snapshot review?

Risks
- If bridge tracing is added only inside DefaultDataVaultReadService, callers that hit the direct DataVaultBridgeReadPipeline fallback branch can miss spans.
- If current, as-of, latest typed projection, PIT typed projection, or bridge helper layers add wrapper spans instead of relying on the underlying execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.
- If a public ActivitySource holder is introduced accidentally, ApiSurfaceSnapshotTests will fail and the package surface change will require deliberate review.

Split recommendations
- No further split is recommended; save and read tracing is already separated from PIT and bridge maintenance tracing in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

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