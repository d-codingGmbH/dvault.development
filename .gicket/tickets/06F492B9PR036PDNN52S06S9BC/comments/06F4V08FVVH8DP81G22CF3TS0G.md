[gicket-bot] PO refinement contract

Summary
- Revalidated the ticket against current branch source. IDataVaultReadDiagnosticsService already exposes request-bound Analyze overloads for latest-satellite, registry latest-satellite, PIT, bridge, and registry bridge requests, while DataVaultDiagnosticsResult currently exposes constructor fields Validation/Explain/SaveStrategy/Issues plus init-only ReadStrategy. The contract is narrowed so query-shape diagnostics are introduced as a new additive member and supporting model(s), not inferred as preexisting. No child tickets, relation writes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Source-backed the existing diagnostics API split on the current branch. Request-unbound Analyze(DbContext) remains on IDataVaultDiagnosticsService, while request-bound read diagnostics are already exposed on IDataVaultReadDiagnosticsService for DataVaultLatestSatelliteReadRequest, DataVaultRegistryLatestSatelliteReadRequest, DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and DataVaultRegistryBridgeReadRequest. The contract now treats query-shape diagnostics as new additive API to introduce explicitly instead of inferring a missing existing overload or type.
- critic-item-2: `answered` - Restated the delivery contract so it no longer implies an existing query-shape/read-shape member on DataVaultDiagnosticsResult. Current source shows DataVaultDiagnosticsResult with constructor members Validation, Explain, SaveStrategy, and Issues, plus an init-only ReadStrategy property; the new query-shape payload must therefore be introduced as a separate additive member with supporting model(s).
- critic-item-3: `answered` - Confirmed the only existing read-bound diagnostics member on DataVaultDiagnosticsResult is ReadStrategy, typed as DataVaultReadStrategyDiagnostics, and it represents provider-strategy dispatch rather than query-shape payload. The contract now keeps ReadStrategy unchanged and requires the query-shape data to be added through a fresh additive member/model instead of implying one already exists.
- critic-item-4: `answered` - Made the additive-model requirement explicit: the new read-shape/query-shape payload must be implemented as a fresh property and supporting model(s) on DataVaultDiagnosticsResult and must not overload DataVaultReadStrategyDiagnostics. Existing support-bundle export already serializes DataVaultDiagnosticsResult through DataVaultSupportBundleExporter.ExportJson(...), so the new payload must flow additively through that path rather than altering the meaning of the current strategy diagnostics type.

Clarifications
- Request-unbound Analyze(DbContext) stays on IDataVaultDiagnosticsService; request-bound read diagnostics stay on IDataVaultReadDiagnosticsService.
- The source-backed existing public read-diagnostics request inputs on this branch are DataVaultLatestSatelliteReadRequest, DataVaultRegistryLatestSatelliteReadRequest, DataVaultPitAsOfReadRequest, DataVaultBridgeReadRequest, and DataVaultRegistryBridgeReadRequest.
- Current source-backed DataVaultDiagnosticsResult shape is constructor members Validation, Explain, SaveStrategy, and Issues plus init-only ReadStrategy; this ticket must add a separate additive read-shape/query-shape member and supporting model(s) instead of reusing an implied existing one.
- Current and as-of helper APIs already ride the latest-satellite request family by constructing DataVaultRegistryLatestSatelliteReadRequest, and registry-backed diagnostics already normalize latest-satellite and bridge requests to explicit request forms before analysis.
- Current parent/blocker relations were reviewed and left unchanged; no relation cleanup was needed for this contract-only refinement.
- No child tickets, relation writes, attachments, description-side bounded writes, or planning documents were applied in this refinement pass.

Scope In
- Introduce one new additive read-shape/query-shape diagnostics section on DataVaultDiagnosticsResult, with supporting public model(s), for request-bound latest/current/as-of satellite, PIT as-of, and bridge read diagnostics.
- Cover both explicit diagnostics requests and the existing registry-backed diagnostics overloads for latest-satellite and bridge requests after registry resolution; PIT remains explicit-request only in this ticket.
- Serialize the new additive diagnostics section through the existing DataVaultSupportBundle and DataVaultSupportBundleExporter.ExportJson(...) path.
- Update docs, public API snapshot, unit tests, and integration tests for the additive read-shape diagnostics contract and provider-caveat/fallback reporting.

Scope Out
- No new standalone diagnostics service, telemetry surface, CLI-only API, or replacement of existing IDataVaultDiagnosticsService or IDataVaultReadDiagnosticsService entry points.
- No new registry-backed PIT request type or registry-specific PIT diagnostics overload in this ticket.
- No change to read execution semantics, result row shapes, PIT or bridge maintenance behavior, or provider strategy selection rules.
- No raw SQL dump, live EXPLAIN/query-plan capture, request hash keys, or payload values in the new diagnostics payload.

Open questions
- none

Follow-up questions
- Should a later tooling ticket add a condensed query-shape identifier or CLI-friendly summary once the structured payload stabilizes?
- Should future provider-specific optimized read strategies add provider-native explainers beyond the provider-neutral read-shape facts once non-SQLite optimized read strategies exist?

Risks
- DataVaultDiagnosticsResult and dvault.support-bundle.v1 are stable public surfaces, so the new member and supporting model(s) must remain strictly additive and version-safe.
- If the new payload leaks raw SQL, request hash keys, or payload values, it breaks the existing redaction-safe support-bundle boundary.
- Registry-backed and explicit diagnostics must stay semantically equivalent after normalization or support bundles will diverge for the same logical read.
- Index guidance must stay derived from translated metadata rather than hand-maintained strings or it will drift from actual projected schema.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment