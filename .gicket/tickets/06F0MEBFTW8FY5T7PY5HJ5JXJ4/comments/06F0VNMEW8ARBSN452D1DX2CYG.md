[gicket-bot] PO refinement contract

Summary
- Verified the ticket snapshot, comments, live relation state, and current registry/model baseline. The ticket is bounded as registry-backed save/read consumption work on top of the existing explicit services, and no child-ticket, relation, or planning-document writes were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Registry-backed behavior should use the same authoritative metadata source already selected for the DbContext through AddDVault(...)/UseDataVaultMetadata(...), including a context-level metadata model or registry override when one is configured.
- Explicit request-based save/read APIs remain the advanced path and must keep their current source-compatible contract and deterministic behavior when callers provide metadata directly.
- This ticket removes duplicate caller-side metadata construction for ordinary service usage; it does not introduce typed object mapping.
- Adjacent typed-helper scope remains in existing tickets 06F0MECFNF42NK9PND9DWVW9VW (typed explicit save helpers) and 06F0MECPFAVBFBNC5XMVDZRQ6M (typed latest/as-of read projections).
- Live relation state was verified and left unchanged: parent 06F0MEANEV00QSYHMSGWX1X0R4, incoming blocks from 06F0MEAXT99V0P115P0WEJD4P0 and 06F0MEB634X6CTBZ00W108G3FG.
- No child tickets, relation edits, or planning documents were materialized because the current evidence already keeps this ticket bounded.

Scope In
- Registry-backed overloads or companion adapters for existing save-service flows that resolve hub/link/satellite metadata from the authoritative DVault metadata registry when the caller chooses the ordinary path.
- Registry-backed overloads or companion adapters for existing read-service flows that resolve the same metadata from the authoritative registry for common latest/as-of style reads without changing the existing low-level result model.
- Deterministic precedence and validation rules between registry-resolved metadata and explicit caller-supplied metadata.
- Regression coverage proving explicit low-level APIs keep their current behavior and registry-backed paths fail before write orchestration starts when required metadata is missing.

Scope Out
- Typed object mapping or DTO projection work.
- The typed helper tickets already tracked separately in 06F0MECFNF42NK9PND9DWVW9VW and 06F0MECPFAVBFBNC5XMVDZRQ6M.
- Provider-specific save or read optimization changes.
- Changes to the existing metadata-source conflict rules owned by AddDVault(...)/UseDataVaultMetadata(...).

Open questions
- none

Follow-up questions
- After this refactor lands, should the README quickstart switch its ordinary save/read examples to the registry-backed entry points while keeping explicit request examples for advanced usage?
- Once the separate typed helper tickets land, should they build directly on these registry-backed entry points or continue to compose the explicit request APIs themselves?

Risks
- The live ticket still has incoming blocks relations from 06F0MEAXT99V0P115P0WEJD4P0 and 06F0MEB634X6CTBZ00W108G3FG, so implementation sequencing depends on those upstream tickets or later relation cleanup.
- If registry-backed calls accidentally diverge from the explicit validation path, ordinary and advanced callers could see inconsistent diagnostics or write ordering; regression tests need to pin this down.

Split recommendations
- No additional split is recommended now; the current ticket is already bounded to registry-backed metadata consumption, while typed save-helper and typed read-projection work is already separated into neighboring tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment