[gicket-bot] PO refinement contract

Summary
- Verified current branch source: `DataVaultDiagnosticsResult.ReadShape`, the public read-shape model types, request-bound `IDataVaultReadDiagnosticsService` overloads, and support-bundle export already exist, so this story remains an additive extension of that surface with performance-stage signals and index/provider guidance. No child tickets, relation writes, attachments, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced the unsupported inference with source-backed baseline evidence: the current branch already defines `DataVaultDiagnosticsResult.ReadShape`, public read-shape model records, and request-bound `IDataVaultReadDiagnosticsService` overloads. The contract therefore keeps this ticket additive to the existing `ReadShape` surface instead of inventing a new diagnostics API.
- critic-item-2: `answered` - The delivery contract no longer depends on an inferred missing public API. `DataVaultDiagnosticsResult.ReadShape` is already in the current branch public surface and is populated for latest/as-of satellite, PIT, and bridge requests, so this story extends that public member additively with performance-oriented fields.
- critic-item-3: `answered` - Source evidence supports the existing-API claim: the current branch already ships `DataVaultDiagnosticsResult.ReadShape`, public read-shape model types, and support-bundle exposure for request-bound read diagnostics. This ticket should extend that existing member and nested public types additively rather than add a second diagnostics member or a new service.

Clarifications
- Current branch source already defines `DataVaultDiagnosticsResult.ReadShape` and the public read-shape record family in `src/DCoding.Data.DVault/DataVaultDiagnostics.cs`, so this story extends the existing request-bound read-shape contract additively rather than creating a second diagnostics payload or service.
- `IDataVaultReadDiagnosticsService` already exposes latest/as-of satellite, registry latest/as-of satellite, PIT as-of, bridge, and registry bridge analysis overloads; current/as-of helper coverage stays within the existing latest-satellite request family rather than introducing separate helper-specific diagnostics APIs.
- `DataVaultSupportBundle` already carries the full `DataVaultDiagnosticsResult`, and unit coverage proves `DataVaultSupportBundleExporter.ExportJson(...)` emits `readShape` without raw request values; new performance data must stay on that same deterministic redaction-safe export path.
- Existing unit and integration tests already cover explicit and registry-backed read-shape diagnostics across latest/as-of satellite, PIT, and bridge requests, including provider-neutral fallback and SQLite-selected behavior, so this ticket extends a visible baseline instead of inventing a new one.
- No child tickets, relation changes, attachments, description-side bounded writes, or planning documents were materialized in this refinement pass.

Scope In
- Extend the existing public `DataVaultReadShapeDiagnostics` family and nested public read-shape model types with additive performance-stage facts for projected columns, join count, predicate shape, ordering or row-selection posture, likely index needs, and bounded provider caveats.
- Populate those additive fields through the existing `IDataVaultReadDiagnosticsService.Analyze(...)` overloads and current registry normalization paths for latest-satellite and bridge reads; PIT remains explicit-request only in this story.
- Represent multi-step performance on the existing request-bound read-shape surface as one or more deterministic stages, especially for PIT and any provider-neutral fallback path that performs materially separate work.
- Serialize the new fields through `DataVaultSupportBundleExporter.ExportJson(...)` and keep the public API snapshot, unit tests, and integration tests aligned with the expanded contract.

Scope Out
- No new top-level diagnostics service, no replacement of `DataVaultDiagnosticsResult.ReadShape`, and no parallel performance-only payload beside the existing request-bound read-shape surface.
- No raw SQL, query plans, live schema or live index inspection, automatic index creation, request hash keys, parent hash-key values, or as-of values in the diagnostics payload.
- No new registry-backed PIT diagnostics overload, no expansion beyond latest/current/as-of satellite, PIT as-of, and bridge reads, and no save-path diagnostics work.
- No read execution tuning, provider strategy threshold changes, PIT or bridge maintenance changes, benchmark-policy changes, or release-note rollout work.

Open questions
- none

Follow-up questions
- After the structured performance-stage model stabilizes, should a later tooling ticket add a condensed stage summary or query-shape fingerprint for CLI or telemetry output?
- If future provider-specific optimized read strategies diverge materially beyond today's SQLite-selected and provider-neutral behavior, should later tickets add provider-native performance explainers on top of the bounded cross-provider stage model?
- Once downstream tuning work lands, should benchmark artifact bundles link representative read-shape diagnostics snapshots beside SQL-capture evidence for query or index-focused claims?

Risks
- `DataVaultDiagnosticsResult.ReadShape` and the public read-shape records are already public snapshot-backed API, so any non-additive change will break consumers and the public API snapshot.
- PIT and provider-neutral fallback paths can perform materially separate steps; collapsing them into one synthetic join count or one synthetic index hint will mislead consumers.
- Provider caveats that drift from capability profiles, provider behavior profiles, or read-strategy fallback facts will over-promise provider behavior and misdirect tuning work.
- Support-bundle JSON is redaction-safe by design; leaking raw request keys, raw as-of values, or SQL text through new performance fields would violate the existing support contract.

Split recommendations
- No split recommended; current branch evidence supports one additive extension ticket on the existing `ReadShape` surface.

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