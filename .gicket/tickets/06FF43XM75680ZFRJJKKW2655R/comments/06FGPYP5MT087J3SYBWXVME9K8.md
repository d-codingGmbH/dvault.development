[gicket-bot] PO refinement contract

Summary
- Confirmed the persisted ticket body at revision 06FGPY452D6MTAS5T8CHTZGW5C was rewritten in this PO pass and now consistently presents the aggregate same-hub contract; the stale follow-up and risk text about a missing description rewrite is gone.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The delivery-contract history now matches the persisted state: the PO Summary says this PO pass rewrote the ticket body, and the ticket body itself is now the authoritative aggregate handoff surface.
- critic-item-2: `answered` - The stale follow-up and risk wording was removed or restated. The only remaining follow-up is the future additive model-first/public-naming question, and the remaining risks no longer claim the description still needs to be rewritten.
- critic-item-3: `answered` - The persisted contract is no longer inconsistent about description changes. Definition of Done and Implementation Notes now explicitly say the authoritative ticket description contains the aggregate contract and that this refinement pass rewrote the description.
- critic-item-4: `answered` - The live description already contains the aggregate contract block, and the handoff surface now reflects that live state. It no longer asks whether a later pass should write the contract into the ticket body, and it no longer describes the current description as the short legacy draft.

Clarifications
- This PO pass already rewrote the ticket description, and the aggregate delivery contract now lives in the authoritative ticket body.
- Repeated same-hub v1 scope stays bounded to explicit relationship names and distinct produced participant names carried through modeling, generated mappers, and explicit-save persistence.
- Typed helper generation remains support-bundle-driven and does not parse raw dvault.model.v1 files or source-visible declarations directly.
- Dependent child key modeling remains deferred, and effectivity remains the existing link-parent satellite pattern.

Scope In
- Ratify the finite v1 same-hub story boundary across support-bundle facts, generated typed link-mapper parity, and documentation or contract alignment.
- Require explicit relationship names and distinct role-bearing produced participant names for repeated same-hub links so metadata names, produced columns, and generated bindings stay deterministic.
- Keep same-hub generator parity provider-neutral and on the existing IDataVaultLinkMapper<TSource> plus IDataVaultSaveService explicit-save boundary.
- Carry forward the already-decided nearby boundaries for deferred dependent child modeling and effectivity-as-link-parent-satellite guidance.

Scope Out
- Ambiguous repeated same-hub links that omit explicit roles or reuse the same produced participant name.
- New implicit persistence behavior, SaveChanges-driven write paths, provider-specific SQL generation, or a separate same-hub save contract.
- Raw dvault.model.v1 direct typed-helper generation, source-visible direct helper inference, or wider typed-helper parity beyond this same-hub story.
- New dependent child metadata concepts, effectivity-specific fluent APIs, or other broader modeling expansions.

Open questions
- none

Follow-up questions
- If product later wants model-first same-hub typed mapper generation or clearer public naming than ParticipantHubName, handle it as a separate additive compatibility ticket rather than widening this bounded v1 story.

Risks
- Public names such as ParticipantHubName and ParticipantHubNames remain semantically awkward for same-hub role-bearing mappings, so incomplete documentation alignment could still make the supported pattern harder to discover.
- Historical duplicate-scope noise may still make some aggregate views harder to read even though the bounded v1 contract itself is now explicit in the ticket body.

Split recommendations
- No additional split recommended; the existing child-ticket breakdown already covers support-bundle facts, generated mapper parity, documentation alignment, and the nearby deferred-scope decisions.

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