[gicket-bot] PO refinement contract

Summary
- Restated the ticket on a source-backed diagnostics baseline: the current branch proves only the existing `DataVaultDiagnosticsResult.ReadShape` surface and current read-diagnostics overloads, so any performance-stage model is new additive work under that surface; no child tickets, relation changes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is restated to rely only on source-backed existing APIs. Current branch evidence proves `DataVaultDiagnosticsResult.ReadShape` and `DataVaultReadShapeDiagnostics` as the existing request-bound public surface; it does not prove any preexisting public performance-stage record, so this story may introduce that model additively under the existing `ReadShape` surface.
- critic-item-2: `answered` - The refined contract no longer assumes an existing public performance-stage API. It treats `ReadShape` as the only current request-bound public diagnostics member and constrains this story to additive records/properties on that surface plus public API snapshot updates.
- critic-item-3: `answered` - Current source proves `IDataVaultReadDiagnosticsService` overloads only for explicit latest satellite, registry latest satellite, PIT as-of, bridge, and registry bridge requests. Registry overloads normalize to explicit request objects before analysis. No preexisting performance-stage record model is evidenced, so that model may be created additively within this story.
- critic-item-4: `answered` - Current source proves `DataVaultDiagnosticsResult.ReadShape` remains the single public request-bound diagnostics member, and current `CreateReadShapeDiagnostics(...)` logic populates only `Satellite`, `Pit`, or `Bridge` branches. This story therefore extends that existing surface additively and must not introduce a second top-level diagnostics payload.

Clarifications
- Source-backed baseline: `DataVaultReadShapeDiagnostics` currently exposes `Kind`, `Provider`, and optional `Satellite`, `Pit`, and `Bridge` branches; the current branch does not evidence any existing public performance-stage record type.
- `IDataVaultReadDiagnosticsService` already exposes the five request-bound read overloads for explicit latest satellite, registry latest satellite, PIT as-of, explicit bridge, and registry bridge diagnostics, and the registry overloads normalize to explicit request objects before analysis.
- `IDataVaultDiagnosticsService.Analyze(DbContext)` calls the shared analysis path with `readRequest: null`, so request-unbound diagnostics continue to leave `ReadShape` unset.
- `DataVaultSupportBundle` and `DataVaultSupportBundleExporter.ExportJson(...)` remain the existing support-bundle serialization path, with camelCase JSON and built-in redaction over serialized string values.
- No bounded child-ticket, relation, attachment, description, or planning-document write was materialized in this refinement pass.

Scope In
- Introduce additive performance-stage records and/or properties under the existing `DataVaultDiagnosticsResult.ReadShape` -> `DataVaultReadShapeDiagnostics` surface in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`.
- Populate those additive facts through the existing `IDataVaultReadDiagnosticsService.Analyze(...)` overloads for `DataVaultLatestSatelliteReadRequest`, `DataVaultRegistryLatestSatelliteReadRequest`, `DataVaultPitAsOfReadRequest`, `DataVaultBridgeReadRequest`, and `DataVaultRegistryBridgeReadRequest`.
- Describe projected columns, filter/predicate shape, row-selection or ordering posture, likely index baselines, and bounded provider caveats for latest-satellite, PIT, and bridge diagnostics.
- Allow PIT and other materially multi-step read paths to report multiple deterministic stages when that is the most faithful additive representation of the visible request shape.

Scope Out
- No new top-level diagnostics service, no second request-bound diagnostics payload, and no replacement of `DataVaultDiagnosticsResult.ReadShape`.
- No new public read-diagnostics entry-point overloads beyond the existing explicit latest satellite, registry latest satellite, PIT as-of, bridge, and registry bridge overloads.
- No raw SQL text, query plans, live schema/index inspection, automatic index creation, request values, parent hash keys, or raw as-of values in diagnostics output.
- No save-path diagnostics expansion, read execution tuning, provider-threshold changes, release-note rollout, or benchmark-policy work in this ticket.

Open questions
- none

Follow-up questions
- After the additive stage model lands, should a later tooling ticket add a condensed stage summary or query-shape fingerprint for CLI or telemetry use?
- If provider-specific optimized read strategies later diverge beyond today's SQLite-selected and provider-neutral baselines, should a follow-up ticket add provider-native explanatory fields on top of the bounded cross-provider stage model?
- Should later release or benchmark artifacts link representative read-shape diagnostics snapshots beside benchmark evidence once tuning work exists?

Risks
- Non-additive changes to `DataVaultDiagnosticsResult.ReadShape` or the current public `DataVaultReadShapeDiagnostics` surface will break the public API snapshot and existing consumers.
- Materially multi-step PIT or fallback flows can be misrepresented if the new additive model is flattened into one synthetic stage narrative.
- Provider caveats must stay derived from read-strategy status, fallback causes, provider-behavior profile, and translated metadata; otherwise diagnostics will over-promise optimizer behavior.
- Support-bundle export is redaction-sensitive; new fields must not leak raw parent hash keys, raw as-of values, or SQL text through the expanded diagnostics payload.

Split recommendations
- No split recommended; current source-backed evidence supports one additive ticket on the existing `ReadShape` surface.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment