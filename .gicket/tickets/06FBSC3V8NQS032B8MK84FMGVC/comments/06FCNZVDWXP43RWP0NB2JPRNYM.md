[gicket-bot] PO refinement contract

Summary
- Refined the ticket to target a deterministic v1 provider-evidence manifest contract derived from the existing benchmark triplet, closed diagnostics vocabularies, and evidence-matrix postures; no split or persistent planning writes were justified.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The v1 baseline is a deterministic camelCase provider-evidence manifest contract that sits beside the existing benchmark triplet; it does not replace benchmark-summary.json, benchmark-summary.csv, or benchmark-summary.md.
- The manifest must be able to express the current row identity already preserved in benchmark-summary.json: scenario, provider, baseline, strategyFamily, datasetSize, changeRatio, executionStatus, skipReason, and persistedOutcome.
- The manifest must promote provider-specific facts that are currently embedded in deterministic executionDetail text into explicit structured fields, including selected or planned path, selected strategy, bounded fallback causes, and read shape or workload shape.
- The closed vocabularies are already visible in repository evidence and should be reused rather than renamed: read shapes LatestSatellite, PitAsOf, and Bridge; evidence postures from docs/plans/provider-optimization-evidence-matrix.md; and the existing save/read fallback-cause enums.
- No child tickets, relation changes, description updates, attachments, or planning documents were materialized in this refinement run because the current evidence keeps the work bounded to one ticket.

Scope In
- Define and document one provider-evidence manifest row contract that benchmark work and documentation work can both populate without parsing prose-only benchmark notes.
- Cover the current provider optimization evidence row families: provider-native bulk-ingestion save rows and latest-satellite, PIT as-of, and bridge read rows, including completed SQLite rows and skipped optional-provider rows.
- Allow docs-owned rows such as completed-timing, skipped-placeholder, diagnostics-only, and smoke-only postures to reuse the same shape without pretending every row is timing evidence.
- Reuse the current provider, strategy, baseline, dataset, change-ratio, execution-status, read-shape, and fallback-cause terminology already present in repository contracts and tests.

Scope Out
- Do not add new provider strategies, new benchmark scenarios, or new provider-support claims.
- Do not replace the shared benchmark artifact triplet or reopen its timing/allocation metric schema.
- Do not introduce runtime manifest discovery, runtime dispatch, deployment automation, or a new standalone CLI/export lane.
- Do not fold hash-key storage sidecars, compiled-model or pooling rows, or unrelated SQL artifact manifests into this v1 provider-evidence manifest unless they already fit the same provider-evidence row family without new vocabulary.

Open questions
- none

Follow-up questions
- After the v1 shape is stable, should a later ticket emit a dedicated checked-in or generated provider-evidence manifest artifact instead of relying on contract documentation plus row-to-manifest mapping?
- Should hash-key storage evidence and other non-provider optimization rows get a sibling manifest family later, or remain outside this provider-evidence contract?

Risks
- If the new manifest duplicates provider facts that still evolve independently inside executionDetail, benchmark artifacts and docs can drift; implementation should derive both from the same closed vocabularies or shared mapping.
- Docs-side evidence already includes non-timing postures such as diagnostics-only and smoke-only; omitting those from v1 would force follow-up tickets back into parallel ad hoc shapes.
- If the ticket expands beyond provider optimization evidence rows into every benchmark row family, the contract will sprawl and lose the bounded purpose justified by current repository evidence.

Split recommendations
- No split recommended for the current ticket; contract definition and alignment of existing benchmark and docs evidence surfaces are one bounded unit.
- If later work wants a new exporter or checked-in manifest artifact, track that as a follow-up after this ticket lands the shared row contract.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment