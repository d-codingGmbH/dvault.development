[gicket-bot] PO refinement contract

Summary
- Removed the unsupported closure-only/shipped-baseline claims. The visible branch evidence does not prove an existing public ReadShape API, so this ticket remains a bounded implementation story for additive read-shape diagnostics rather than obsolete duplicate work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract no longer assumes an existing public ReadShape type or API. It now treats query-shape and index-hint diagnostics as additive work to be created or formalized in the existing diagnostics area if that surface is missing.
- critic-item-2: `answered` - The persisted refinement no longer infers a shipped public ReadShape API from unavailable source lines. The only source-backed baseline carried forward is that diagnostics code lives in DataVaultDiagnostics.cs and diagnostics tests live in DataVaultDiagnosticsTests.cs.
- critic-item-3: `answered` - The duplicate-work closure rationale was removed. Because the visible snapshot does not prove shipped query-shape diagnostics API, docs, and tests, this ticket stays as an implementation story instead of closure-only obsolete work.

Clarifications
- Visible repository evidence supports an existing diagnostics ownership area in src/DCoding.Data.DVault/DataVaultDiagnostics.cs and matching diagnostics tests in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs, but it does not visibly prove the previously claimed public ReadShape type/API.
- This ticket should therefore be treated as additive diagnostics scope to create or formalize the bounded read-shape performance surface, not as closure-only obsolete work.
- No bounded child-ticket, relation, attachment, planning-document, or description write was materialized in this pass; the persisted ticket description still contains stale closure-only text until a later write-capable pass updates it.

Scope In
- Add or formalize a bounded read-diagnostics surface in the existing diagnostics area for latest/current/as-of satellite reads, PIT reads, and bridge reads.
- Expose provider-neutral performance guidance such as projected-column shape or counts, join shape or count, predicate or row-selection shape, ordering shape, likely index-baseline hints, and provider-specific caveat or fallback summaries.
- Add or refresh unit coverage in tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs for explicit and registry-backed read paths across the supported read families.
- Document the new diagnostics surface only to the extent needed for user-facing behavior, without overstating it as a full database advisor.

Scope Out
- Raw SQL text, database execution plans, live schema inspection, automatic index creation, or full query-advisor behavior.
- Unrelated save-diagnostics changes, analyzer work, release-publication mechanics, or provider-package feature changes beyond bounded caveat reporting.
- Emission of sensitive request values, parent hash keys, or raw as-of values in serialized diagnostics payloads.

Open questions
- none

Follow-up questions
- If product later wants database-advisor-style recommendations, live index verification, or provider-specific tuning beyond bounded hints, should that be split into a separate follow-up ticket instead of expanding this story?

Risks
- The persisted ticket description still contains stale closure-only language because no gicket-update-ticket-description write was materialized in this pass.
- If a later evidence pass reveals deeper in-branch read-shape implementation than the visible snapshot exposed, developers should reconcile that code with this explicit contract rather than reintroducing undocumented shipped-baseline assumptions.

Split recommendations
- No split is required from current visible evidence; keep this as one bounded diagnostics story unless future work expands into database-advisor behavior or provider-specific tuning recommendations.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment