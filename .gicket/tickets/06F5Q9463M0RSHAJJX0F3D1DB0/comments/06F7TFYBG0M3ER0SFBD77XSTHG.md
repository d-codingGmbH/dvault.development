[gicket-bot] PO refinement contract

Summary
- Restated the tracing contract against current-branch source evidence: save/read scope is now tied to the existing public SaveAsync and read/helper surfaces, no existing public tracing accessor is assumed, and no child-ticket, relation, attachment, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is now anchored only to source-backed boundaries from the current branch: IDataVaultSaveService exposes three public SaveAsync overloads for DataVaultSaveRequest, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest; IDataVaultReadService exposes ReadLatestSatelliteRowsAsync(...) and ReadPitRowsAsync(...); current/as-of satellite helpers, PIT typed projection, registry adapters, and bridge helpers are existing public helper surfaces. No existing public tracing holder/accessor is required by the contract; if tracing needs one, it is new internal implementation code.
- critic-item-2: `answered` - The refined contract no longer depends on an inferred existing public tracing API or type. Current-branch evidence shows public save/read surfaces and helper extensions, but no public ActivitySource or ActivityListener-based accessor in source or in the approved public API snapshot. The documented ActivitySource name is contract guidance from docs/architecture/dvault-v1-activity-tracing-contract.md, not evidence of an already-shipped public type.
- critic-item-3: `answered` - The contract now states that any tracing holder/helper needed for the documented ActivitySource name may be created explicitly as new internal implementation and does not require any already-existing public tracing accessor. A public code-facing tracing API remains out of scope by default; if one is intentionally introduced, it is a separate additive surface that must update the approved public API snapshot in the same change.

Clarifications
- Current branch evidence supports a bounded baseline: three public save boundaries in src/DCoding.Data.DVault/DataVaultSaveService.cs; two public read-interface members in src/DCoding.Data.DVault/IDataVaultReadService.cs; current/as-of, PIT typed projection, registry, and bridge coverage come from existing helper extensions.
- Repository search for ActivitySource, ActivityListener, System.Diagnostics.Activity, and Tracing over src/DCoding.Data.DVault and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt returned no matches, so the contract must not depend on an already-shipped public tracing holder/accessor.
- No child-ticket, relation, attachment, or planning-document writes were materialized in this pass; existing relations remain incoming parentOf from 06F5Q93R4633D41Z21WQW3SVGR, incoming blocks from 06F5Q93YXHSKABD2SABWY85S78, and outgoing blocks to 06F5Q94SQ086B2DZ1AKFDXGV94.

Scope In
- Instrument the three public IDataVaultSaveService.SaveAsync overloads in src/DCoding.Data.DVault/DataVaultSaveService.cs with listener-driven Activity tracing that uses the ActivitySource name DCoding.Data.DVault from docs/architecture/dvault-v1-activity-tracing-contract.md.
- Instrument latest/current/as-of satellite executions that flow through IDataVaultReadService.ReadLatestSatelliteRowsAsync(...) and the existing current/as-of and registry helper extensions so each execution emits one dvault.read.latest_satellite root span.
- Instrument IDataVaultReadService.ReadPitRowsAsync(...) and DataVaultReadServicePitExtensions.ReadPitAsync(...) so each execution emits one dvault.read.pit root span.
- Instrument DataVaultReadServiceBridgeExtensions.ReadBridgeRowsAsync(...), DataVaultReadServiceBridgeExtensions.ReadBridgeAsync(...), and registry bridge helpers so each execution emits one dvault.read.bridge root span across both DefaultDataVaultReadService and DataVaultBridgeReadPipeline branches.
- Keep tracing compatible with the existing telemetry observer and meter baseline documented in docs/releases/v0.16.0.md.

Scope Out
- Do not require any already-existing public tracing holder/accessor; none is evidenced in source or the public API snapshot, so any helper needed for the documented ActivitySource name is new internal implementation unless a deliberate additive public API is introduced.
- Do not add new IDataVaultReadService members solely for current/as-of satellite, PIT typed projection, registry, or bridge tracing; reuse the existing helper and delegate paths.
- Do not introduce a public code-facing tracing API by default; if one is deliberately added, treat it as additive public surface and update tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt in the same change.
- Do not instrument PIT or bridge maintenance operations; those remain in 06F5Q94D0JDMMWDXSRGWX1E4F0.
- Do not change provider strategy selection, SQL shape, batching thresholds, persistence semantics, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements.
- Do not capture raw SQL, metadata names, table names, hash keys, payload values, record-source values, exception messages, or stack traces in Activity names, tags, or events.
- Do not require tracing coverage for consumer-owned custom IDataVaultSaveService or IDataVaultReadService implementations beyond repo-owned core code and helper surfaces.

Open questions
- none

Follow-up questions
- If consumers later need a code-facing ActivitySource accessor instead of listener registration by source name, should that ship in a separate additive API ticket with explicit public API snapshot review?

Risks
- If tracing is added only around IDataVaultReadService.ReadLatestSatelliteRowsAsync(...), bridge helper executions will still miss coverage because they use separate bridge helper paths rather than that interface member.
- If bridge tracing is added only in the DefaultDataVaultReadService branch, callers that flow through DataVaultBridgeReadPipeline can miss dvault.read.bridge spans.
- If helper layers add wrapper Activities instead of reusing the delegated execution path, duplicate root spans can leak into listener output.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity(...) returns null.
- If implementation adds a public tracing accessor instead of an internal helper without same-change snapshot review, it can widen the package surface beyond this story's default boundary.

Split recommendations
- No split is recommended; current branch evidence supports one bounded story for save and read tracing, while PIT and bridge maintenance tracing remains separate in 06F5Q94D0JDMMWDXSRGWX1E4F0.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment