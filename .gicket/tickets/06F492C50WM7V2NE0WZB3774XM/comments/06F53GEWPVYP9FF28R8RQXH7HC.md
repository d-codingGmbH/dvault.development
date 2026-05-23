[gicket-bot] PO refinement contract

Summary
- Source-backed PO refinement confirms the existing read diagnostics baseline and narrows this story to additive performance-stage extensions on the current `ReadShape` contract; no child tickets, relation writes, attachments, or planning docs were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is now restated against source-backed baseline only: `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and `IDataVaultReadDiagnosticsService` exist today, while any stage-oriented performance model introduced by this story is new additive API under that existing family rather than a presumed preexisting surface.
- critic-item-2: `answered` - The contract no longer infers an existing performance-stage API or type. The existing public baseline is limited to the current `ReadShape` member and the current latest-satellite, PIT, and bridge read-shape records; any new stage records or stage properties are explicit additive work for this ticket.
- critic-item-3: `answered` - Verified current branch source: `DataVaultDiagnosticsResult.ReadShape` exists, `IDataVaultReadDiagnosticsService` exposes overloads for explicit latest satellite, registry latest satellite, PIT as-of, explicit bridge, and registry bridge diagnostics, and current unit/integration tests exercise those entry points.
- critic-item-4: `answered` - Verified current branch source: `DataVaultReadShapeDiagnostics` is already a public record family with `Satellite`, `Pit`, and `Bridge` branches. The story should extend that family additively, potentially by adding new subordinate records or properties for stage facts, instead of assuming those members already exist.
- critic-item-5: `answered` - Verified current branch source: `DataVaultDiagnosticsResult.ReadShape` is the existing public request-bound member, and the read-shape builder already populates it for latest-satellite, PIT as-of, and bridge requests. The contract therefore stays on additive changes in `DataVaultDiagnostics.cs` rather than a second diagnostics member or a new diagnostics service.

Clarifications
- Verified live relation context from `.gicket/relations`: epic `06F492BTNHRPBC7D24E13ECFKM` is parent of this ticket; this ticket blocks `06F492CAB2293R7BGJWMWMRKT4` and `06F492D05THPGQVT3B3K7853A0`; incoming `blocks` from done tickets `06F492BZPP5YT9SJSPDHQBGF3R` and `06F492B9PR036PDNN52S06S9BC` remain unchanged in this pass.
- Current branch source proves existing public read diagnostics only for `DataVaultDiagnosticsResult.ReadShape`, `DataVaultReadShapeDiagnostics`, and the request-bound `IDataVaultReadDiagnosticsService` overload set. It does not prove any preexisting performance-stage record model, so that part of the API may be created additively within this story.
- `IDataVaultDiagnosticsService.Analyze(DbContext)` already leaves `ReadShape` null for request-unbound diagnostics, so this ticket stays on the request-bound read diagnostics path.
- `DataVaultSupportBundle` already wraps `DataVaultDiagnosticsResult`, and `DataVaultSupportBundleExporter.ExportJson(...)` is the existing deterministic redacted serialization path.
- No child tickets, description writes, relation writes, attachments, or planning documents were materialized in this refinement pass.

Scope In
- Add additive stage-oriented performance facts under the existing public `DataVaultReadShapeDiagnostics` family in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`, covering projected columns, join or step shape, predicate shape, ordering or row-selection posture, likely index needs, and bounded provider caveats.
- Populate those additive facts through the existing `IDataVaultReadDiagnosticsService.Analyze(...)` overloads for `DataVaultLatestSatelliteReadRequest`, `DataVaultRegistryLatestSatelliteReadRequest`, `DataVaultPitAsOfReadRequest`, `DataVaultBridgeReadRequest`, and `DataVaultRegistryBridgeReadRequest`.
- Allow PIT and provider-neutral fallback diagnostics to emit multiple deterministic stages when the request shape is materially multi-step.
- Serialize the expanded contract through `DataVaultSupportBundleExporter.ExportJson(...)` and keep the public API snapshot, unit tests, and integration tests aligned.

Scope Out
- No new top-level diagnostics service, no replacement of `DataVaultDiagnosticsResult.ReadShape`, and no parallel performance-only payload beside the existing request-bound read-shape surface.
- No new public entry-point overloads beyond the current explicit latest satellite, registry latest satellite, PIT as-of, explicit bridge, and registry bridge diagnostics set.
- No raw SQL, query plans, live schema or live index inspection, automatic index creation, request hash keys, parent hash-key values, or raw as-of values in diagnostics.
- No save-path diagnostics expansion, no read execution tuning or provider threshold changes, and no benchmark-policy or release-note rollout work.

Open questions
- none

Follow-up questions
- After the additive stage model lands, should a later tooling ticket add a condensed stage summary or query-shape fingerprint for CLI or telemetry use?
- If provider-specific optimized read strategies later diverge beyond today's SQLite-selected and provider-neutral baselines, should a follow-up ticket add provider-native explanatory fields on top of the bounded cross-provider stage model?
- Should release or benchmark artifacts later link representative read-shape diagnostics snapshots beside benchmark evidence once tuning work exists?

Risks
- Non-additive changes to `DataVaultDiagnosticsResult.ReadShape` or the public `DataVaultReadShapeDiagnostics` family will break the public API snapshot and existing consumers.
- PIT and provider-neutral fallback can be materially multi-step; flattening them into one synthetic stage narrative will misrepresent the visible request shape.
- Provider caveats must stay derived from read-strategy status, fallback causes, provider-behavior profile, and translated metadata; otherwise diagnostics will over-promise optimizer behavior.
- Support-bundle export is redaction-sensitive; leaking raw parent hash keys, raw as-of values, or SQL text through new performance fields would violate the current export contract.

Split recommendations
- No split recommended; the current source-backed API baseline supports one additive ticket on the existing `ReadShape` surface.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment