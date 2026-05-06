[gicket-bot] PO refinement contract

Summary
- Republished the source-backed docs-only bridge refinement for PO-critic routing; scope remains unchanged and no split or planning write was needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This handoff republishes the existing ready_for_po_critic routing decision. The contract already resolves the docs-only bridge scope with no open questions; the earlier mismatch was workflow metadata because the persisted ticket still carried PO labels while the description already advertised PO-critic routing. Per the runtime-managed handoff policy, accepting this contract is the point that realigns the persisted routing metadata to the PO-critic label set.

Clarifications
- Bridge tables remain an opt-in v0.5 deferred capability; this ticket documents the current deferred baseline rather than a concrete bridge runtime API.
- The authoritative inputs for this ticket are the current repository baseline and the cited planning and architecture documents already in context, not a future parent-only naming artifact.
- The documentation example is limited to exactly one conceptual many-to-many traversal scenario and must not invent bridge-specific APIs, annotations, EF metadata members, or generated table contracts not present on the branch.
- The existing parentOf relation from 06EZ0NTV4SVAKV98C418T8A3CC already captures the ticket tree, so no relation changes were needed.
- No child tickets, planning documents, or attachments were materialized in this refinement pass because the existing bounded contract already covered the work.

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