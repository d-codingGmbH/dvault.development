[gicket-bot] PO refinement contract

Summary
- Reframed the story as net-new internal Activity instrumentation over source-backed save/read entry points, removing unsupported assumptions about an existing public tracing API or type.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now states explicitly that DVault Activity tracing is net-new work in this branch: implementation may create the shared ActivitySource named DCoding.Data.DVault in repo-owned core code, rather than assuming an existing tracing API or holder type already exists. The public contract is anchored to documented span names and existing save/read entry points, not to an inferred pre-existing Activity type.
- critic-item-2: `answered` - The contract no longer infers a missing public read/save API. It now names the visible existing boundary that instrumentation must cover: IDataVaultSaveService.SaveAsync overloads for explicit saves; IDataVaultReadService.ReadLatestSatelliteRowsAsync and ReadPitRowsAsync for core reads; current/as-of, latest typed projection, PIT typed projection, and bridge helper paths through the existing public extension classes. Bridge tracing is explicitly required across both the DefaultDataVaultReadService branch and the direct bridge-pipeline fallback branch so the story does not rely on a non-existent public bridge method on IDataVaultReadService.
- critic-item-3: `answered` - The contract now explicitly prefers an internal shared ActivitySource holder/helper. A new public ActivitySource surface is optional, not assumed. If implementation later determines that repository API policy requires a public holder, that becomes an intentional public API addition and must update ApiSurfaceSnapshotTests and the approved public API snapshot as part of the same change.

Clarifications
- Current branch evidence shows no existing DVault ActivitySource or Activity tracing implementation, so this ticket is net-new instrumentation work and may introduce the shared ActivitySource holder/helper explicitly.
- The required ActivitySource name remains DCoding.Data.DVault from docs/architecture/dvault-v1-activity-tracing-contract.md, but current branch evidence does not support any pre-existing public holder type for that source.
- IDataVaultReadService currently exposes only ReadLatestSatelliteRowsAsync and ReadPitRowsAsync; current/as-of, latest typed projection, PIT typed projection, and bridge reads are existing public extension helpers over that boundary.
- DataVaultReadServiceCurrentSatelliteExtensions delegates current and as-of helpers into the latest-satellite read path, so all three shapes share one dvault.read.latest_satellite root-span vocabulary.
- DataVaultReadServiceBridgeExtensions dispatches bridge helpers through internal DefaultDataVaultReadService when available and otherwise directly through DataVaultBridgeReadPipeline, so tracing must preserve one dvault.read.bridge Activity across both branches.
- The core package already has public API snapshot enforcement, and the approved snapshot contains no Activity-related type; default the shared tracing holder/helper to internal unless a deliberate API decision requires public exposure.

Scope In
- Introduce a shared System.Diagnostics.ActivitySource named DCoding.Data.DVault in repo-owned core code, defaulting to an internal holder/helper unless the implementation intentionally adds a new public API surface.
- Instrument repo-owned explicit save entry points backed by IDataVaultSaveService.SaveAsync overloads for single-request, ordered bulk, and chunked saves with the exact dvault.save.single_request, dvault.save.bulk_request, and dvault.save.chunked_request Activity names.
- Instrument latest/current/as-of satellite reads, PIT reads, and bridge reads across the existing IDataVaultReadService and public read extension helpers so the closed dvault.read.latest_satellite, dvault.read.pit, and dvault.read.bridge names cover the visible read boundary without duplicate root spans.
- Populate only contract-approved tags and events by reusing existing bounded telemetry summary and diagnostics values for provider name, strategy status/type, fallback causes, counts, duration bucket, outcome, and bounded failure classification.
- Add focused unit and integration coverage for no-listener behavior, sampled-listener behavior, redaction, cancellation/fault status mapping, no duplicate spans from helper wrappers, bridge fallback coverage, and existing telemetry/public-API compatibility.

Scope Out
- PIT and bridge maintenance tracing remains in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.
- No changes to provider selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- No raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces may be captured in Activity names, tags, or events.
- No requirement to instrument consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and the repository's public helper paths.
- No requirement to add a new public ActivitySource holder unless that API change is explicitly chosen and public API snapshots are updated intentionally.

Open questions
- none

Follow-up questions
- If consumers later need a public code-level ActivitySource accessor instead of listener registration by source name, should that be introduced in a separate API-shaping ticket with intentional public API snapshot review?

Risks
- If bridge tracing is added only inside DefaultDataVaultReadService, callers that use the non-default branch in DataVaultReadServiceBridgeExtensions can miss spans.
- If current/as-of, latest typed-projection, PIT typed-projection, or bridge helper layers add wrapper spans instead of relying on the underlying execution path, duplicate root spans can leak into listener output.
- If tag/event construction happens before listener or sampling checks, the implementation can violate the contract's minimal-overhead no-listener baseline even when StartActivity returns null.
- If a public ActivitySource holder is introduced accidentally, ApiSurfaceSnapshotTests will fail and the package surface changes will require deliberate approval.

Split recommendations
- No further split is recommended; save/read tracing is already separated from PIT and bridge maintenance tracing in ticket 06F5Q94D0JDMMWDXSRGWX1E4F0.

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