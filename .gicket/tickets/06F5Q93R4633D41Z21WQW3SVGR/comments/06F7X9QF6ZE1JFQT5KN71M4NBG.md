[gicket-bot] PO refinement contract

Summary
- Ticket requires additional substantive product clarification before continuing.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `cannot_answer` - The relation graph and closure evidence are verified, but I cannot certify that the persisted ticket metadata matches that state because the latest persisted ticket read still includes `blocked/dev` and `blocked/test`. No available bounded PO write surface in this session can mutate labels, so the rerun cannot return this epic to PO-critic yet.
- critic-item-2: `answered` - Confirmed. The remaining ambiguity is metadata-level: the closure-only contract, clean relation graph, and done-child evidence all agree, but the persisted stop labels still advertise a blocked implementation state. That contradiction is not clean enough for unattended progression, so the ticket stays in PO clarification until metadata is aligned.

Clarifications
- No child-ticket, relation, attachment, or planning-document writes were materialized in this pass because the live relation graph is already clean and the repository closure baseline is already landed.
- The authoritative closure baseline still points to docs/architecture/dvault-v1-activity-tracing-contract.md, src/DCoding.Data.DVault/DataVaultActivityTracing.cs, docs/performance-profiles.md, benchmark-summary.md, benchmark-summary.csv, benchmark-summary.json, docs/releases/v0.23.0.md, and README.md.
- The blocker is not scope ambiguity: it is the persisted metadata contradiction between the closure-only parent contract and the latest ticket labels that still advertise blocked implementation work.

Scope In
- Closure-only tracking for epic 06F5Q93R4633D41Z21WQW3SVGR against the existing five child tickets and the already-landed v0.23.0 tracing and performance documentation baseline.
- Verification that the live relation graph remains parentOf-only with incomingCount=0 before the epic is allowed to progress toward closure.

Scope Out
- Any new product-code, documentation, tracing, benchmark, or implementation work owned directly by the parent epic.
- Any new child-ticket split, relation reshaping, exporter or observability platform work, or package-publication automation work.

Open questions
- none

Follow-up questions
- Before runtime closes the epic, does a final eligibility check still show incomingCount=0 and the same five done child tickets after any integration activity?

Risks
- If the stale blocked labels remain in persisted metadata, automated handoff continues to look ambiguous even though the scope and relation evidence are already closure-ready.
- If any child ticket is reopened or a new incoming relation appears, the parent epic must stop at closure tracking until the evidence is clean again.

Split recommendations
- No additional split recommended; the existing five-child decomposition remains complete and the parent epic still owns only closure tracking.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 2

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [blocked/po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment