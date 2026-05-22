[gicket-bot] PO refinement contract

Summary
- Source-backed the diagnostics API claims on the current branch and tightened the contract to add a new additive query-shape payload instead of inferring an existing one; no child tickets or planning docs were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced inferred API references with source-backed ones. The current branch defines IDataVaultReadDiagnosticsService with request-bound Analyze overloads for DataVaultLatestSatelliteReadRequest, DataVaultRegistryLatestSatelliteReadRequest, DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and DataVaultRegistryBridgeReadRequest. Request-unbound Analyze(DbContext) remains on IDataVaultDiagnosticsService, so the contract now distinguishes those services instead of implying a missing read-diagnostics overload.
- critic-item-2: `answered` - Restated the delivery contract so the query-shape payload is new additive public API to create, not an already-existing member. DataVaultDiagnosticsResult currently exposes Validation, Explain, SaveStrategy, Issues, and additive ReadStrategy only. The existing export surface is DataVaultSupportBundleExporter.ExportJson(DataVaultDiagnosticsResult, ...), so this ticket adds a new read-shape member/model on DataVaultDiagnosticsResult and flows it through that exporter rather than claiming an existing read-shape type or property.
- critic-item-3: `answered` - Source-backed the supported explicit and registry-backed read-diagnostics entry points. Existing public read diagnostics cover explicit latest/as-of satellite requests, explicit PIT requests, explicit bridge requests, and registry-backed latest-satellite and bridge requests. The registry overloads resolve metadata and delegate to explicit request shapes, and current/as-of convenience helpers already construct DataVaultRegistryLatestSatelliteReadRequest. There is still no registry-backed PIT request or diagnostics overload on the current branch, so that remains out of scope.

Clarifications
- Request-unbound Analyze(DbContext) is on IDataVaultDiagnosticsService; request-bound read diagnostics stay on IDataVaultReadDiagnosticsService.
- The existing public read-diagnostics inputs on this branch are DataVaultLatestSatelliteReadRequest, DataVaultRegistryLatestSatelliteReadRequest, DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and DataVaultRegistryBridgeReadRequest.
- DataVaultDiagnosticsResult already exists but currently only exposes ReadStrategy for read-bound data; the query-shape payload in this ticket is a new additive member/model that must be introduced explicitly.
- Current and as-of satellite helper APIs already collapse into the latest-satellite request family by constructing DataVaultRegistryLatestSatelliteReadRequest and then DataVaultLatestSatelliteReadRequest.
- No bounded ticket writes, child tickets, relation updates, attachments, or planning documents were applied in this refinement pass.

Scope In
- Add an additive query-shape/read-shape diagnostics member on DataVaultDiagnosticsResult for request-bound latest/current/as-of satellite, PIT as-of, and bridge reads.
- Cover both explicit diagnostics requests and the existing registry-backed diagnostics overloads for latest-satellite and bridge reads after registry resolution.
- Serialize the new diagnostics additively through the existing DataVaultSupportBundle and DataVaultSupportBundleExporter.ExportJson(...) path.
- Update docs, API snapshot, unit tests, and integration tests for the new read-shape diagnostics contract and provider-caveat/fallback behavior.

Scope Out
- No new standalone explain, telemetry, or CLI-only diagnostics API.
- No new registry-backed PIT read request or PIT diagnostics overload in this ticket.
- No change to read execution semantics, result record shapes, PIT or bridge maintenance behavior, or provider strategy selection rules.
- No raw SQL dump, live database EXPLAIN, query-plan capture, or emission of request hash-key or payload values.

Open questions
- none

Follow-up questions
- Should later tooling expose a condensed query-shape identifier or CLI-friendly summary once the structured payload stabilizes?
- Should future provider-specific read strategies add provider-native shape explainers when non-SQLite optimized read strategies exist?

Risks
- DataVaultDiagnosticsResult and dvault.support-bundle.v1 are stable public surfaces, so the new diagnostics member must remain additive and version-safe.
- Because registry-backed and explicit diagnostics normalize through shared request types, any divergence between those paths would create conflicting support-bundle evidence for the same logical read.
- If the payload includes raw SQL, request hash keys, or payload values, it will violate the current redaction-safe support-bundle boundary.
- Index guidance must stay derived from translated EF metadata rather than hand-maintained strings or it will drift from actual projected schema.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment