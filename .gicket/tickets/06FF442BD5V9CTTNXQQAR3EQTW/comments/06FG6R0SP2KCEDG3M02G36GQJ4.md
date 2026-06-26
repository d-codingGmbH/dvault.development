[gicket-bot] PO refinement contract

Summary
- Fresh repository inspection shows this is a bounded docs-alignment task: ratify the shipped effectivity baseline as generic link-parent satellite metadata, ratify the current support-bundle-driven typed read-helper baseline, and document deferred same-hub/dependent-child parity shapes explicitly.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Effectivity is already documented and implemented as caller-owned link-parent satellite state declared with `Link(...).Satellite<TSatellite>(...)`, `Payload(...)`, and optional `DrivingKey(...)`; this ticket does not introduce a first-class effectivity builder, metadata kind, or table family.
- Repeated same-hub links are already part of the implemented fluent metadata surface, but same-hub typed link-mapper/source-generator parity is still outside the current public claim set.
- Current typed read-helper coverage is the v1 support-bundle-driven generator contract for satellite latest/current/as-of, PIT as-of, and bounded bridge traversal helpers; raw `dvault.model.v1` files and source-visible declarations are not direct generator inputs.

Scope In
- Review and align the current documentation that sets adopter expectations for repeated same-hub links, link-parent satellites/effectivity, typed read helpers, and typed mapper/source-generator boundaries.
- Make the docs explicitly separate implemented fluent/model metadata shapes from generated helper coverage and from deferred additive shapes.
- Correct or supersede any lingering satellite-only typed-helper wording by pointing to the current implemented support-bundle-driven satellite/PIT/bounded bridge helper contract.

Scope Out
- Any new runtime, analyzer, or public API implementation for effectivity-specific builders, dependent child keys, same-hub typed mapper parity, or new helper shapes.
- PIT or bridge maintenance behavior changes, runtime read semantics changes, or provider behavior changes.
- Broad documentation cleanup outside the effectivity and typed-helper boundary touched by this ticket.

Open questions
- none

Follow-up questions
- Should a later ticket add runnable Code-First same-hub and effectivity examples once the boundary docs are aligned?
- Should future additive roadmap work separate dependent child key modeling from same-hub typed mapper/source-generator parity so each gap is tracked independently?

Risks
- This boundary is described across current architecture docs, adoption guidance, model-first governance, release notes, and historical plans; partial edits could leave contradictory reader guidance.
- Readers may conflate typed read helpers with typed save mappers/source-generator parity unless the updated docs consistently use precise terminology and cross-links.

Split recommendations
- none

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