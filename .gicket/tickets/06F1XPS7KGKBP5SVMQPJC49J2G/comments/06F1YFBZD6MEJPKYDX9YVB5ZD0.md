[gicket-bot] PO refinement contract

Summary
- Refinement ratifies the current DMV#### diagnostic baseline, incorporates the completed catalog-infrastructure child ticket as the first implementation slice, and leaves no blocking PO questions; no new child tickets, relation changes, attachments, or planning documents were materialized in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the v1 diagnostic-id baseline as the existing DMV#### family emitted by the model-artifact importer/projection path, so this story should stabilize and document that shipped baseline instead of renaming it to a new DVLT prefix.
- Completed child ticket 06F1XPSSFYJQS3BTGSYAX32198 is the authoritative first implementation slice and seeds the current 18 importer/projection diagnostics DMV1001, DMV1002, DMV1101, DMV1102, DMV1103, DMV1201, DMV1202, DMV1203, DMV1301, DMV1302, DMV1303, DMV1401, DMV1501, DMV1502, DMV1601, DMV1602, DMV1701, and DMV1801.
- Each v1 catalog entry must own stable definition metadata on the definition object itself: code, severity, category, summary/title, explanation, and remediation guidance.
- Affected-location data remains emitted-instance context rather than static catalog metadata; for the seeded importer/projection slice that means preserving JSON pointer and logical source path behavior where available.
- This story remains the diagnostic-code foundation under epic 06F1XPRY3ZDB6W1WQ9ABRRJ2V4 for downstream migration, design-time, drift, and analyzer tickets that already expect stable codes.
- No ticket relations, child tickets, attachments, or planning documents were added or changed during this refinement pass.

Scope In
- Stabilize and document the current DMV#### diagnostic-id format and category conventions as the v1 baseline for DVault diagnostics.
- Maintain a central deterministic core-package catalog for stable diagnostic definitions, seeded first with the 18 importer/projection diagnostics already captured by child ticket 06F1XPSSFYJQS3BTGSYAX32198.
- Require per-entry remediation guidance and clear affected-location reporting where the emitting path can supply it.
- Use catalog-backed definitions in at least one existing validation path without changing observed ids, categories, or location behavior.
- Update repository documentation with the diagnostic contract and representative examples.

Scope Out
- Any Roslyn analyzer package or IDE-specific diagnostic delivery.
- Repository-wide migration of every existing and future diagnostic emitter in this story; downstream migration, design-time, drift, and analyzer families stay in their own tickets.
- Localization or message-framework expansion beyond current repository practice.
- Renaming already-shipped DMV#### diagnostics to a new prefix.

Open questions
- none

Follow-up questions
- Should later diagnostic families outside importer/projection continue the same DMV#### prefix with reserved code bands, or should the repository define an explicit cross-family allocation policy once those tickets are ready?
- After the internal catalog contract is proven, should a separate documentation ticket publish a consumer-facing supported diagnostic list beyond the repository-internal examples?
- Do downstream migration, design-time, drift, and analyzer tickets need a shared guidance document for how they claim new codes from the stabilized catalog baseline?

Risks
- If downstream tickets add diagnostics before an explicit code-allocation convention is documented, separate families may drift in numbering or category usage.
- Documentation can lag the seeded catalog baseline unless story completion treats docs as a required deliverable rather than a follow-on convenience.
- The current seeded baseline covers importer/projection diagnostics first; blocked downstream guardrail tickets still depend on later catalog adoption for full consistency.

Split recommendations
- No additional split is required for PO-critic readiness; the completed child ticket 06F1XPSSFYJQS3BTGSYAX32198 already captures the bounded catalog-infrastructure slice and the remaining story scope is documentation plus story-level ratification.
- If implementation later needs a separate delivery boundary, create a docs-only follow-up ticket rather than expanding this story into unrelated diagnostic-family migrations.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment