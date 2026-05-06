[gicket-bot] PO refinement contract

Summary
- Resolved the child as a docs-only deferred-capability task: document the current bridge baseline now using existing repository vocabulary and one conceptual many-to-many scenario, without blocking on parent bridge API naming.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Accepted as a non-blocking follow-up. This child now documents the current deferred bridge baseline. If parent story 06EZ0NTV4SVAKV98C418T8A3CC later publishes concrete bridge naming or shape details, refresh this child in a later docs sync before or after implementation as needed.
- critic-item-2: `answered` - Confirmed and fixed in the contract. The prior wording made parent refinement a prerequisite for developer handoff; the updated contract removes that prerequisite and instead constrains the deliverable to current source-backed repository evidence.
- critic-item-3: `answered` - Confirmed. There is no authoritative bridge runtime surface on the branch today, so the updated contract forbids inventing bridge-specific APIs, annotations, generated names, or table shapes. The example must stay conceptual and architecture-level.

Clarifications
- Bridge tables remain an opt-in v0.5 deferred capability; this child documents that current deferred baseline rather than a not-yet-implemented bridge API.
- The authoritative sources for this child are the current repository baseline and the cited planning and architecture documents already in context, not a future parent-only naming artifact.
- The example is limited to one bounded conceptual many-to-many traversal scenario and must not invent bridge-specific API names, annotations, EF metadata, or generated table contracts not present on the branch.
- If parent story 06EZ0NTV4SVAKV98C418T8A3CC later introduces concrete bridge naming or shape details, that becomes a follow-up docs alignment pass rather than a prerequisite for this child.

Scope In
- Document bridge tables as an opt-in v0.5 deferred capability layered on the current hub, link, and satellite baseline.
- Explain that the visible repository baseline exposes no bridge-specific EF metadata translator output and no bridge-specific annotation contract today.
- Provide exactly one minimal many-to-many traversal scenario described in current repository vocabulary and clearly marked as a deferred bridge-use-case example, not as a source-backed API walkthrough.
- Reuse the current public vocabulary around AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), and IDataVaultSaveService.

Scope Out
- Implementing bridge modeling, EF metadata projection, save behavior, validation, or tests.
- Inventing bridge-specific APIs, annotations, EF metadata members, generated table names, or provider behaviors not proven on the branch.
- Hierarchy-depth semantics, recursive traversal behavior, provider-specific DDL or migrations, PIT interactions, and multi-active interactions beyond explicit deferred notes.
- Runnable sample applications or broad README expansion beyond the bounded bridge documentation task.

Open questions
- none

Follow-up questions
- If parent story 06EZ0NTV4SVAKV98C418T8A3CC later publishes concrete bridge names or shapes, should a small follow-up docs sync mirror those terms?
- After this child is implemented, should README.md add a short cross-link to the bridge documentation page?
- If hierarchy-style traversal later needs a worked example, should that become a separate follow-up docs ticket rather than expanding this page?

Risks
- A later bridge implementation ticket may introduce concrete naming or runtime semantics that require a small terminology sync in the docs.
- The docs could drift into speculative API design unless they stay anchored to the current source-backed deferred baseline.

Split recommendations
- No split is required for the current bounded docs-only task.
- If later work needs hierarchy-specific walkthroughs, runnable samples, or docs tied to implemented bridge APIs, create separate follow-up docs tickets.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 3
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment