[gicket-bot] PO refinement contract

Summary
- Refinement ratified the bounded v1 hash-key storage-profile contract: existing diagnostics, explain, and support-bundle surfaces already carry the selected storage facts, so this ticket should stay focused on explicit coverage and redaction-safe acceptance language.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence fixes the v1 storage-profile vocabulary to HexString and Binary; this ticket should not introduce a third persisted storage-profile token.
- Selected hash-key storage facts are already modeled on explain type mappings and translated hash-key/reference properties, and support bundles serialize that same explain payload.
- No child tickets, description updates, attachments, or relation changes were materialized in this refinement pass; the existing blocks relation to 06FBSC0TMZBXVVECGQGESWPCY4 remains unchanged.

Scope In
- Report selected hash-key storage facts through the existing diagnostics/explain/support-bundle surfaces for hash keys and participant references.
- Keep HexString versus Binary selection visible in both structured and human-readable diagnostics without exposing secret-bearing values.
- Add or ratify tests that distinguish the supported selection scenarios called out by the ticket wording while staying inside the bounded v1 storage vocabulary.

Scope Out
- Any new public hash-key value type, new diagnostics API surface, or third v1 storage-profile vocabulary item.
- Raw business keys, raw hash-key values, request values, SQL text, provider plans, or other secret-bearing diagnostics payloads.
- Migration, backfill, repair, dual-write, or provider-side hashing behavior outside the already selected storage metadata facts.

Open questions
- none

Follow-up questions
- After this ticket lands, should public adopter guidance include one concrete binary diagnostics/support-bundle example for migration and support playbooks?

Risks
- The acceptance wording can be misread as requiring a third storage-profile enum; without the bounded clarification above, implementation could accidentally widen the v1 contract.
- If tests only cover HexString and one Binary selection path, a regression in a provider/profile-preselected Binary path could escape even though the shared diagnostics surface is the same.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment