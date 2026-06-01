[gicket-bot] PO refinement contract

Summary
- Refined the bridge-helper story against the completed architecture contract and current repository baseline. The ticket is already bounded to support-bundle-driven many-to-many and hierarchy bridge helper generation, with documentation rollout left to the downstream docs task.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already fixes the bridge endpoint vocabulary to From, To, Ancestor, and Descendant, and DataVaultBridgeReadRequest already requires bounded maximumDepth for hierarchy bridges while rejecting depth on many-to-many bridges.
- The authoritative design source is docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md; related story 06F7Y0GT7A5QT77TADMRZBVYN8 is done and should be treated as historical contract input rather than a new PO blocker.
- Relation context is coherent for planning: this ticket is a child of epic 06F7Y0FR4JS1V9WHFBP70GX1SM, it currently blocks docs task 06F7Y0HZKHBHMYX9EYDYFRYXZ0, and no new child ticket or planning document is needed because implementation and docs are already separated.

Scope In
- Generate support-bundle-driven typed bridge helpers for supported many-to-many bridge reads using Read{ProducedName}FromAsync and Read{ProducedName}ToAsync.
- Generate support-bundle-driven typed bridge helpers for supported hierarchy bridge reads using Read{ProducedName}AncestorAsync and Read{ProducedName}DescendantAsync with required bounded maximumDepth.
- Emit bridge read-model records and constants that project endpoint hash-key members in generated order and TraversalDepth for hierarchy bridges.
- Preserve deterministic diagnostics and helper isolation so unsupported bridge shapes fail or skip per-entity without suppressing unrelated satellite or bridge helpers.

Scope Out
- Typed PIT helper generation, PIT acceptance criteria, and any PIT-specific read-shape work.
- Bridge or PIT maintenance, read-time refresh, SaveChanges orchestration, provider-specific SQL, or dynamic query compilation.
- Raw dvault.model.v1 parsing, source-visible Code-First inference, or model-first expansion beyond the support-bundle boundary.
- Release-note, README, and read-plan documentation rollout tracked by downstream task 06F7Y0HZKHBHMYX9EYDYFRYXZ0.

Open questions
- none

Follow-up questions
- When downstream docs task 06F7Y0HZKHBHMYX9EYDYFRYXZ0 executes, should release and read-plan wording move typed bridge helpers from future additive contract language into the active implementation baseline?

Risks
- Helper generation is gated by request-bound readShape.bridge support-bundle evidence; redaction or missing endpoint order, filter, or depth facts will intentionally suppress bridge helpers even when runtime bridge metadata exists.
- Hierarchy helpers must preserve the current inclusive maximumDepth boundary exactly; emitting an unbounded overload or widening depth semantics would silently change runtime behavior.
- Deterministic generated-name collisions across bridge types, methods, or constants must still fail with DMV1965 instead of partially emitting broken helper code.

Split recommendations
- No additional split is recommended. The current story is already bridge-only, the upstream contract story is complete, and downstream documentation work is already separated into ticket 06F7Y0HZKHBHMYX9EYDYFRYXZ0.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment