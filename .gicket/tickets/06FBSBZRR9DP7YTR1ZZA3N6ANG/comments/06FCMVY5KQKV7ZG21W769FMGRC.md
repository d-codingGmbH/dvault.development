[gicket-bot] PO refinement contract

Summary
- Delivery contract refined and ready for PO-critic review.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - 06FBSBZRR9DP7YTR1ZZA3N6ANG is now a closure/tracking reconciliation ticket, not an executable story. All six split child tickets are done, and the landed repository already contains the API, documentation, diagnostics, and benchmark evidence the parent originally sought.
- critic-item-2: `answered` - The parent description has been replaced with an authoritative delivery contract that names the remaining parent-level outcome as evidence aggregation and closure reconciliation only, lists the authoritative repository artifacts, and explains how completed child tickets roll up into parent acceptance. No separate parent implementation artifact remains.
- critic-item-3: `answered` - Because the parent is closure-only, it should not return to a developer queue. The authoritative contract now states that no new developer implementation remains on the parent; runtime-managed status and label reconciliation should move it out of the developer handoff path after this PO contract is accepted.
- critic-item-4: `answered` - The inconsistency is confirmed and bounded. The maintenance comment says develop removed needs-po and marked the parent tracking/parent plus tracking/waiting-on-children, but the current ticket snapshot still shows todo with blocked/dev, blocked/test, and needs-po while all six split children are done. The updated contract resolves the scope ambiguity by treating the parent as closure-only; the remaining metadata cleanup is runtime-managed rather than a product-scope blocker.

Clarifications
- No new child tickets, attachments, or planning documents were created because the existing split children 06FBSBZY1XEJYK1DRV4RV2ZN88, 06FBSC03KAGDABNFGPK9D95QKR, 06FBSC08W24BJGFZ87RSFS21WC, 06FBSC0EJHAY200E7PXNRGV7XR, 06FBSC0MNH0YAWQ4NY2WSC8KJG, and 06FBSC0TMZBXVVECGQGESWPCY4 are already done.

Scope In
- Reconcile the parent ticket so it authoritatively records the closure-only roll-up of already completed child work.
- Name the repository artifacts that satisfy the original story across APIs, documentation, diagnostics, and benchmark/adoption evidence.
- Preserve the bounded product decision that named binary-first APIs are the new-project path while the compatible default remains HexString unless callers opt in.

Scope Out
- New product code, new documentation, new diagnostics, new benchmark work, or new migration tooling beyond the already landed child deliverables.
- Reopening the runtime default, promising automatic migration or backfill, promising dual-write behavior, or exposing public byte[] hash-key values.
- Creating further split tickets, attachments, or planning documents for this parent.

Open questions
- none

Follow-up questions
- After this parent is closed, decide separately whether historical reporting should restore explicit parent-child roll-up links for the completed split set or continue relying on ticket history plus child descriptions.

Risks
- none

Split recommendations
- No further split is justified. The parent already has six child tickets, and all six are done.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment