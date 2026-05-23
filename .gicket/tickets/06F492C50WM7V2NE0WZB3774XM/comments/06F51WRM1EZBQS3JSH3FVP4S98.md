[gicket-bot] PO refinement contract

Summary
- Refined this story into an additive extension of the shipped v0.17 ReadShape diagnostics: add bounded performance-stage signals, likely index guidance, and provider caveats on the existing request-bound read diagnostics surface without reopening benchmark policy or creating a new diagnostics API.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v0.17 baseline already ships DataVaultDiagnosticsResult.ReadShape plus public read-shape model types and support-bundle export; this ticket extends that existing public surface additively instead of introducing a second diagnostics member or service.
- Scope stays on the current request-bound read families: latest/current/as-of satellite via the existing latest-satellite request family, explicit PIT as-of, and explicit or registry bridge reads; no new registry-backed PIT request or new diagnostics entry point is added here.
- Because IDataVaultReadDiagnosticsService.Analyze(...) receives read requests rather than projector delegates, projected columns in this ticket mean the columns DVault fetches or materializes for the analyzed read path, not arbitrary consumer DTO members.
- Performance evidence and SQL-capture policy are already defined by done ticket 06F492BZPP5YT9SJSPDHQBGF3R; this ticket reuses that contract and does not invent a second artifact or benchmark policy.
- The output remains deterministic and redaction-safe guidance only: no raw SQL, no query plans, no live schema or index inspection, no request hash keys or as-of values, and no claim that DVault is acting as a full database advisor.

Scope In
- Add additive performance-oriented facts to the existing read-shape diagnostics for projected columns, join count, predicate shape, ordering or row-selection posture, likely index needs, and finite provider caveats.
- Represent performance hints at deterministic query-stage granularity so multi-step paths such as PIT reads can describe PIT-row lookup and referenced-satellite lookups without fabricating one synthetic query plan.
- Populate the extended diagnostics through the existing IDataVaultReadDiagnosticsService.Analyze(...) overloads and the current registry normalization paths for latest-satellite and bridge requests.
- Keep index guidance derived from translated metadata, selected read strategy or provider-neutral fallback, and the visible provider capability profile rather than live database inspection.
- Serialize the new fields through the existing DataVaultSupportBundleExporter.ExportJson(...) path and keep public API snapshot, unit tests, and integration tests aligned with the expanded contract.

Scope Out
- No new top-level diagnostics service, telemetry-only surface, CLI-only output path, or replacement of IDataVaultReadDiagnosticsService or DataVaultDiagnosticsResult.ReadShape.
- No raw SQL capture, live EXPLAIN or query-plan capture, live schema or live index inspection, automatic index creation, or DDL recommendation engine.
- No new registry-backed PIT diagnostics overload, no expansion beyond latest/current/as-of satellite, PIT as-of, and bridge reads, and no save-path diagnostics work.
- No behavioral tuning of read execution, provider strategy thresholds, PIT or bridge maintenance, compiled-query work, compiled-model work, or DbContext pooling work already owned by sibling tickets.
- No broad v0.18 release-note or adoption-documentation rollout; that remains on ticket 06F492D05THPGQVT3B3K7853A0.

Open questions
- none

Follow-up questions
- After the structured performance shape stabilizes, should a later tooling ticket add a condensed stage summary or query-shape fingerprint for CLI or telemetry output?
- If future provider-specific optimized read strategies for PostgreSQL, SQL Server, MySQL, or Oracle diverge materially from today's visible baseline, should they add provider-native stage explainers beyond the current bounded capability and fallback caveats?
- Once downstream tuning work lands, should benchmark artifact bundles link representative diagnostics snapshots beside the required SQL-capture evidence for query or index-focused claims?

Risks
- The read-shape records are already public snapshot-backed API and part of support-bundle JSON, so non-additive or loosely specified changes will break consumers and downstream documentation.
- PIT and provider-neutral fallback paths are multi-stage rather than single-query shapes; collapsing them into one synthetic join count or one synthetic index hint would mislead consumers.
- SQLite-selected and provider-neutral fallback paths differ materially, so guidance that ignores ReadStrategyStatus will produce incorrect performance hints.
- Index guidance that drifts from translated metadata or provider capability rules, especially included-index handling differences, will misdirect downstream tuning and documentation work.

Split recommendations
- none

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