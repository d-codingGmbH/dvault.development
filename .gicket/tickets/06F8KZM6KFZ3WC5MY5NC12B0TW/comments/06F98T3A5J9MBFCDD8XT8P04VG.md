[gicket-bot] PO refinement contract

Summary
- Blocked pending bounded ticket/relation verification reads required by the PO refinement instructions.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `cannot_answer` - The required closure evidence anchor is already stated in the supplied ticket snapshot, but the workflow instructions require bounded live ticket/comment/relation verification before finalizing. Those reads have not been executed in this turn yet.
- critic-item-2: `cannot_answer` - The supplied snapshot and critic finding both say the parent is closure-only with no residual developer slice, but I have not yet completed the bounded live verification step required before returning a final refinement contract.

Clarifications
- Before finalizing that contract for PO-critic, the runtime must execute the requested bounded reads for the current ticket, comments, relations, and the four cited child tickets.

Scope In
- Confirm the closure-only contract remains aligned with current ticket comments and relations.

Scope Out
- Any new developer implementation under this epic.
- Any final PO-critic handoff decision before the required bounded verification reads complete.

Open questions
- Can the runtime execute the requested bounded ticket/comment/relation reads so the refinement can be finalized on verified live state?

Follow-up questions
- none

Risks
- Returning ready_for_po_critic without the required bounded verification step would violate the interactive PO refinement instructions.

Split recommendations
- No split recommendation yet; the supplied snapshot already indicates the existing four completed child tickets cover the epic scope.

Persisted contract coverage
- acceptance-criteria items: 2
- definition-of-done items: 2
- implementation-notes items: 1

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment