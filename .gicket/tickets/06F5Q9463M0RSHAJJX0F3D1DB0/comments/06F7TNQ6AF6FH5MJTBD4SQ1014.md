[gicket-bot] PO refinement contract

Summary
- Refined the contract to explicitly cover latest-satellite typed projection helpers and to pin latest-satellite root-span ownership to the terminal repo-owned execution path for row versus projection reads; no child-ticket, relation, attachment, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Scope In, Acceptance Criteria, and Implementation Notes now explicitly include DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection> and the public registry typed latest overload recorded in the public API snapshot, so typed latest/current/as-of coverage is part of the contract instead of being implied through row helpers only.
- critic-item-2: `answered` - The single dvault.read.latest_satellite root span is owned only by the terminal repo-owned latest-satellite execution boundary for the path actually used: row reads at the ReadLatestSatelliteRowsAsync execution path, typed projection reads at the projection execution path reached from DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection> via IDataVaultSatelliteProjectionReadService.ReadLatestSatelliteProjectionRowsAsync(...) or DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(...). Current/as-of and registry helpers stay span-transparent wrappers and must not create wrapper root spans.
- critic-item-3: `answered` - The contract now explicitly anchors the existing public latest-satellite typed projection helper surface in src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs and treats it as required tracing coverage because typed current/as-of and registry helpers delegate into that public latest helper surface.
- critic-item-4: `answered` - Because typed latest-satellite reads do not route through IDataVaultReadService.ReadLatestSatelliteRowsAsync(...), the amended contract anchors projection-path tracing at the typed latest projection execution boundary instead of the row path alone, which prevents both missed coverage and duplicate wrapper spans across current/as-of and registry typed helpers.

Clarifications
- Current branch evidence shows three public save boundaries in src/DCoding.Data.DVault/DataVaultSaveService.cs, two public read-interface members in src/DCoding.Data.DVault/IDataVaultReadService.cs, and a separate public latest-satellite typed projection helper in src/DCoding.Data.DVault/DataVaultReadServiceTypedProjectionExtensions.cs plus the public registry typed latest overload recorded in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953 and :956.
- ReadCurrentSatelliteAsync(...), ReadAsOfSatelliteAsync(...), and their registry typed variants delegate into the latest-satellite typed helper path, so they inherit latest-satellite tracing from that terminal execution path and must not create wrapper root spans.
- No child-ticket, relation, attachment, or planning-document writes were materialized in this pass.

Scope In
- Instrument the three public IDataVaultSaveService.SaveAsync overloads in src/DCoding.Data.DVault/DataVaultSaveService.cs with listener-driven Activity tracing that uses the ActivitySource name DCoding.Data.DVault from docs/architecture/dvault-v1-activity-tracing-contract.md.
- Instrument repo-owned latest-satellite row and typed projection execution paths so each explicit latest/current/as-of satellite execution emits exactly one dvault.read.latest_satellite root span, including IDataVaultReadService.ReadLatestSatelliteRowsAsync(...), DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>, and the public registry typed latest overload recorded in tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt:953 and :956.
- Treat ReadCurrentSatelliteRowsAsync(...), ReadAsOfSatelliteRowsAsync(...), ReadCurrentSatelliteAsync(...), ReadAsOfSatelliteAsync(...), and the registry latest/current/as-of helper variants as in-scope delegating callers whose executions must be covered by that same latest-satellite root span without creating wrapper root spans.
- Instrument IDataVaultReadService.ReadPitRowsAsync(...) and DataVaultReadServicePitExtensions.ReadPitAsync(...) so each execution emits one dvault.read.pit root span.
- Instrument DataVaultReadServiceBridgeExtensions.ReadBridgeRowsAsync(...), DataVaultReadServiceBridgeExtensions.ReadBridgeAsync(...), and registry bridge helpers so each execution emits one dvault.read.bridge root span across both DefaultDataVaultReadService and DataVaultBridgeReadPipeline branches.
- Keep tracing compatible with the existing telemetry observer and meter baseline documented in docs/releases/v0.16.0.md.

Scope Out
- Do not add new IDataVaultReadService members solely for latest/current/as-of satellite, PIT typed projection, registry, or bridge tracing; reuse the existing helper and delegate paths.
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
- If implementation adds wrapper Activities in DataVaultReadServiceCurrentSatelliteExtensions.cs or DataVaultReadServiceRegistryExtensions.cs instead of only at the terminal latest-satellite execution boundary, duplicate root spans can leak into listener output.
- If typed projection tracing is added only to IDataVaultReadService.ReadLatestSatelliteRowsAsync(...) and not to the projection execution path used by DataVaultReadServiceTypedProjectionExtensions.ReadLatestSatelliteAsync<TProjection>, typed current/as-of and registry helpers will miss dvault.read.latest_satellite coverage.
- If tag or event construction happens before listener or sampling checks, the implementation can violate the no-listener minimal-overhead baseline even when StartActivity(...) returns null.
- If implementation adds a public tracing accessor instead of an internal helper without same-change snapshot review, it can widen the package surface beyond this story's default boundary.

Split recommendations
- No split is recommended; current branch evidence still supports one bounded story for save and read tracing, while PIT and bridge maintenance tracing remains separate in 06F5Q94D0JDMMWDXSRGWX1E4F0.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment