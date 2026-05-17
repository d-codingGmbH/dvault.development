[gicket-bot] PO refinement contract

Summary
- Confirmed the ticket is already covered by existing repository tests and should stay on the closure/no-work-required path rather than reopening development.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now keeps this ticket on the closed/no-work-required path: existing repository coverage already satisfies the requested link-parent projection checks, so no dev or test implementation work remains. Runtime-managed closure should clear any dev/test blocking workflow labels instead of routing the ticket back to development.
- critic-item-2: `answered` - Current repository and ticket evidence do not justify reopening this ticket for new fluent code-first link-parent satellite support. The implemented baseline remains metadata-first for link-parent satellites, so any future fluent code-first support must be opened as a separate feature ticket rather than reusing this closure-only ticket.

Clarifications
- Existing unit coverage already asserts the link-parent translation contract for satellite State over link CustomerOrder, including ParentReferenceKind=Link, ParentReferenceName=CustomerOrder, produced table SatCustomerOrderState, and the expected primary-key and index shape.
- Existing shared live-schema fixture coverage already includes SatCustomerOrderState in the deterministic schema snapshot surface, so no new snapshot-style test pass is needed for this ticket.
- The supported v1 baseline for link-parent satellites remains metadata-first; the current fluent code-first builder surface does not declare link-parent satellites.
- This ticket is closure-only from the current evidence: no child tickets, relation edits, attachments, or planning documents were materialized in this run.
- The provided ticket snapshot shows no recent human comments and no provided relation context that reopens scope or justifies a split.

Scope In
- Confirm whether link-parent satellite projection coverage already exists in the repository.
- Ratify the supported baseline for this topic as metadata-first EF projection rather than fluent code-first declaration.
- Refine the ticket as already covered/no-work-required and keep any future feature intent separate from this closure ticket.

Scope Out
- Adding new product code, tests, or documentation for the already-covered link-parent satellite projection behavior.
- Reopening this ticket for fluent code-first link-parent satellite declaration support.
- Broad test hardening, provider-matrix expansion, diagnostics work, or release-note changes beyond confirming the already-covered baseline.

Open questions
- none

Follow-up questions
- If the roadmap now wants fluent code-first declaration support for link-parent satellites, should a separate feature ticket be created for that capability?
- If broader provider-specific or additional scenario hardening beyond the current translation test plus shared snapshot fixture is still desired, should that be tracked as a separate test-hardening ticket?

Risks
- If the original human intent was a new fluent code-first capability instead of confirming existing projection coverage, closing this ticket will not deliver that future feature and a separate feature ticket will be needed.

Split recommendations
- No split on this closure ticket. Keep it no-work-required/already covered. If needed later, open a separate feature ticket for fluent code-first link-parent satellite support and a separate hardening ticket for any broader coverage expansion.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 3
- implementation-notes items: 3

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment