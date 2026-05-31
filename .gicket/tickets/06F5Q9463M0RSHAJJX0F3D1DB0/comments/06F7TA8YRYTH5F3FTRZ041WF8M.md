[gicket-bot] PO refinement contract

Summary
- Resolved the PO-critic tracing-API concern by grounding save/read coverage in current-branch source evidence and by restating any ActivitySource holder/accessor as new internal implementation unless a deliberate additive public API is introduced. No child-ticket, relation, attachment, planning-document, or ticket-description writes were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now uses source-backed save/read boundaries only: the save scope is the three public IDataVaultSaveService.SaveAsync overloads, the public read interface scope is IDataVaultReadService.ReadLatestSatelliteRowsAsync(...) plus ReadPitRowsAsync(...), and current/as-of, registry, PIT typed projection, bridge, and typed satellite coverage is anchored to existing helper surfaces instead of an inferred tracing API.
- critic-item-2: `answered` - The refined contract no longer relies on an inferred existing public tracing type. Current branch evidence shows public telemetry and read-helper APIs, but no public ActivitySource holder/accessor; the only explicit ActivitySource naming in the supplied context is the tracing contract document, so implementation may introduce a new internal holder/helper if needed.
- critic-item-3: `answered` - The contract explicitly treats any ActivitySource holder/helper as new implementation code that may be introduced internally and keeps a public code-facing tracing API out of scope by default. If implementation intentionally adds a public accessor, that is a new additive API and must update the approved public API snapshot in the same change.

Clarifications
- Current branch evidence shows no public ActivitySource holder/accessor today; this story may add a new internal holder/helper for the DCoding.Data.DVault ActivitySource if needed.
- The v1 read baseline is fixed by current source: IDataVaultReadService exposes latest-satellite and PIT raw methods, while current/as-of, registry, PIT typed projection, bridge, and typed satellite access comes from existing public helper surfaces that should share tracing without new interface members.
- The supplied ticket context shows no recent comments or closure evidence amendments, and the ticket snapshot still lists incoming parentOf from 06F5Q93R4633D41Z21WQW3SVGR, incoming blocks from 06F5Q93YXHSKABD2SABWY85S78, and outgoing blocks to 06F5Q94SQ086B2DZ1AKFDXGV94; no relation changes were needed.
- No child-ticket, relation, attachment, planning-document, or ticket-description writes were materialized in this refinement pass.

Scope In
- Add repo-owned listener-driven Activity tracing with ActivitySource name DCoding.Data.DVault; the holder/helper may be introduced as new internal implementation code.
- Instrument the three public IDataVaultSaveService.SaveAsync overload boundaries in src/DCoding.Data.DVault/DataVaultSaveService.cs.
- Instrument latest/current/as-of satellite reads across IDataVaultReadService plus the existing current/as-of, registry, and typed satellite helper surfaces so each execution yields one dvault.read.latest_satellite root span.
- Instrument PIT reads across IDataVaultReadService.ReadPitRowsAsync(...) and the existing DataVaultReadServicePitExtensions typed projection helper so each execution yields one dvault.read.pit root span.
- Instrument bridge reads across the existing DataVaultReadServiceBridgeExtensions and registry bridge helpers, covering both DefaultDataVaultReadService and DataVaultBridgeReadPipeline fallback execution paths without duplicate root spans.
- Reuse the bounded tracing vocabulary from docs/architecture/dvault-v1-activity-tracing-contract.md and keep compatibility with the existing telemetry observer and meter surfaces documented in docs/releases/v0.16.0.md.

Scope Out
- Do not assume or require any pre-existing public tracing API or public ActivitySource accessor.
- Do not add new IDataVaultReadService members solely for current/as-of, bridge, PIT typed projection, or typed satellite helper tracing; reuse existing helper and delegate paths.
- Do not require a public code-facing tracing API for v1; if one is intentionally added, treat it as a new additive API and update the approved public API snapshot in the same change.
- Do not instrument PIT or bridge maintenance operations; those remain in 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and helper surfaces.

Open questions
- none

Follow-up questions
- If consumers later need a public code-facing ActivitySource accessor instead of listener registration by source name, should that ship in a separate additive API-shaping ticket with explicit public API snapshot review?

Risks
- If tracing is added only around ReadLatestSatelliteRowsAsync(...), typed satellite helper executions can still bypass that hook unless the delegated execution path shares the same span creation.
- If bridge tracing covers only the DefaultDataVaultReadService branch, callers that flow through DataVaultBridgeReadPipeline can miss dvault.read.bridge spans.
- If helper layers add wrapper spans instead of reusing the delegated execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity returns null.
- If implementation adds a public tracing accessor instead of an internal helper without same-change snapshot review, it can widen the package surface beyond this story's default boundary.

Split recommendations
- No split is recommended; current branch evidence supports one bounded story for save and read tracing, while PIT and bridge maintenance tracing remains separate in 06F5Q94D0JDMMWDXSRGWX1E4F0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment