<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified live ticket/comments/relations and current-branch source; the contract now relies only on evidenced APIs and treats query-shape diagnostics as one new additive DataVaultDiagnosticsResult member with new public model(s). No child tickets, relation writes, attachments, or planning documents were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Request-unbound Analyze(DbContext) stays on IDataVaultDiagnosticsService; request-bound read diagnostics stay on IDataVaultReadDiagnosticsService and are limited to the visible latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge overloads.
- Current/as-of helper coverage stays in scope through the existing latest-satellite request family because the helper extensions construct DataVaultLatestSatelliteReadRequest or DataVaultRegistryLatestSatelliteReadRequest; this ticket does not add separate current/as-of diagnostics APIs.
- Registry-backed latest-satellite and bridge diagnostics already resolve metadata and normalize to explicit DataVaultLatestSatelliteReadRequest and DataVaultBridgeReadRequest instances before analysis; PIT remains explicit-request only in this ticket.
- Live relations were verified: parentOf from epic 06F492A3MPSGP3KXDNZECN01QM, outgoing blocks to 06F492BNDPWS9P4EDSV0W7G6VM and 06F492C50WM7V2NE0WZB3774XM, and one incoming blocks relation from done ticket 06F492B40K7B0WWPKH8N3PPG3G; because that source ticket is done and this ticket snapshot is isBlocked=false, no relation write is required in this refinement pass.
- The latest blocking PO-critic contract comment 06F4V2FFXA8GKK81NZH62Y6BTW and relation-follow-up comment 06F4V2RDNTNG17W17N8J1Z0X2G were reviewed; no newer human comment changes scope.
- No child tickets, relation writes, attachments, description-side bounded writes, or planning documents were materialized in this refinement pass.

### Scope In
- Add one new additive read-shape/query-shape diagnostics member on DataVaultDiagnosticsResult, with explicit public model(s), for request-bound latest/current/as-of satellite, PIT as-of, and bridge read diagnostics.
- Populate that new member through the existing IDataVaultReadDiagnosticsService.Analyze(...) overloads and the existing registry-backed latest-satellite and bridge diagnostics paths after registry resolution; PIT remains explicit-request only in this ticket.
- Serialize the new additive diagnostics member through the existing DataVaultSupportBundle and DataVaultSupportBundleExporter.ExportJson(...) path.
- Update docs, public API snapshot, unit tests, and integration tests for the additive read-shape diagnostics contract and provider caveat/fallback reporting.

### Scope Out
- No new standalone diagnostics service, telemetry surface, CLI-only API, or replacement of existing IDataVaultDiagnosticsService or IDataVaultReadDiagnosticsService entry points.
- No new registry-backed PIT request type or registry-specific PIT diagnostics overload in this ticket.
- No change to read execution semantics, result row shapes, PIT or bridge maintenance behavior, or provider strategy selection rules.
- No raw SQL dump, live EXPLAIN/query-plan capture, request hash keys, or payload values in the new diagnostics payload.

## Acceptance Criteria
- IDataVaultDiagnosticsService.Analyze(DbContext) remains request-unbound and does not populate the new read-shape/query-shape member, while the existing request-bound IDataVaultReadDiagnosticsService.Analyze(...) overloads populate it for DataVaultLatestSatelliteReadRequest, DataVaultRegistryLatestSatelliteReadRequest, DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and DataVaultRegistryBridgeReadRequest.
- The new read-shape/query-shape member is additive on DataVaultDiagnosticsResult and backed by new explicit public model(s); the existing ReadStrategy member remains the provider-strategy diagnostics surface and is not repurposed.
- Registry-backed latest-satellite and bridge diagnostics resolve metadata first and emit the same read-shape semantics as their equivalent explicit request forms; current/as-of helper coverage continues to ride the latest-satellite request family; no registry-backed PIT diagnostics overload is added.
- Satellite read-shape diagnostics identify current versus as-of semantics from AsOf nullability and capture translated entity/table identity, filter columns, series selection rule, cutoff rule, deterministic ordering, expected translated index baseline, and provider caveat/fallback facts without exposing raw request values.
- PIT read-shape diagnostics identify the PIT entity/table, referenced satellites, parent/as-of filters, PIT row selection rule, snapshot lookup behavior, no-latest-fallback behavior, maintained-PIT prerequisite, expected index baseline, and provider caveat/fallback facts.
- Bridge read-shape diagnostics identify bridge kind, translated entity/table, endpoint filter, optional depth predicate, deterministic ordering, supported endpoint rules, expected traversal index baseline, and provider caveat/fallback facts.
- DataVaultSupportBundleExporter.ExportJson(...) serializes the new member additively with deterministic camelCase redaction-safe output, and docs/tests cover explicit latest-satellite, registry latest-satellite, PIT, explicit bridge, registry bridge, SQLite-selected, and provider-neutral fallback cases.

## Definition of Done
- Public API snapshot and docs show the additive DataVaultDiagnosticsResult expansion and any new supporting public model types without breaking existing diagnostics services or request types.
- Unit coverage exists for read-shape model builders/mappers, registry-to-explicit equivalence, and redaction-safe serialization behavior.
- Integration coverage exists for explicit latest-satellite, registry latest-satellite, PIT, explicit bridge, and registry bridge diagnostics, including SQLite-selected and provider-neutral fallback paths.
- Support-bundle export includes the new member without breaking deterministic JSON ordering, camelCase naming, or existing redaction behavior.

## Implementation Notes
- Add a fresh additive member and supporting public model(s) on DataVaultDiagnosticsResult for read-shape/query-shape diagnostics; do not overload or rename DataVaultReadStrategyDiagnostics.
- Keep ReadStrategy limited to provider dispatch facts and build the new payload from metadata-derived request-shape analysis instead of assuming an existing read-shape type.
- Reuse the normalization already visible in DefaultDataVaultDiagnosticsService: registry latest-satellite diagnostics resolve to DataVaultLatestSatelliteReadRequest and registry bridge diagnostics resolve to DataVaultBridgeReadRequest before shape construction.
- Treat current/as-of helper coverage as latest-satellite-family coverage because the helper extensions already construct DataVaultLatestSatelliteReadRequest and DataVaultRegistryLatestSatelliteReadRequest.
- Thread the new member through the existing DataVaultSupportBundle and DataVaultSupportBundleExporter serialization/redaction pipeline rather than adding a second export surface.
- No planning writes were needed beyond this refinement contract; child-ticket, relation, attachment, and planning-document surfaces were intentionally left untouched.

## Open Questions
- none

## Follow-Up Questions
- Should a later tooling ticket add a condensed query-shape identifier or CLI-friendly summary once the structured payload stabilizes?
- Should future provider-specific optimized read strategies add provider-native explainers beyond the provider-neutral read-shape facts once non-SQLite optimized read strategies exist?

## Risks
- DataVaultDiagnosticsResult and dvault.support-bundle.v1 are stable public surfaces, so the new member and supporting model(s) must remain strictly additive and version-safe.
- If the new payload leaks raw SQL, request hash keys, or payload values, it breaks the existing redaction-safe support-bundle boundary.
- Registry-backed and explicit diagnostics must stay semantically equivalent after normalization or support bundles will diverge for the same logical read.
- Index guidance must stay derived from translated metadata rather than hand-maintained strings or it will drift from actual projected schema.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Expose diagnostics for current/as-of/PIT/bridge read shapes, including joins, filters, ordering assumptions, provider caveats, and indexes that DVault expects consumers to have available.