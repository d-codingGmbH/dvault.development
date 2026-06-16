[gicket-bot] PO refinement contract

Summary
- Narrowed the ticket to a ticket-level Oracle bulk recommendation: keep the current direct batching/array-binding baseline and 50-operation/10000-satellite gate, explicitly keep gap-matrix row P1.04 open as an evidence-gap backlog item, and replace the prior inaccurate closure-only wording about description updates.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The ticket is narrowed rather than closed: P1.04 remains the canonical Oracle save evidence-gap backlog row, and this ticket only records the current keep-as-is recommendation until new provider-configured Oracle evidence exists.
- critic-item-2: `answered` - The expected deliverable is ticket-level recommendation only. This ticket does not edit the canonical planning surfaces or authorize new implementation work; downstream ticket 06FBSCAJ5HDJH6CR0HZQ4B7H30 already says to implement only if the spike accepts an improvement and otherwise close no-work-required.
- critic-item-3: `answered` - The earlier PO contract was factually wrong about description updates. Repository history shows that commit 7c29bd76c updated the current ticket description and comment 06FCWF63ZWP8E59NABEH1QS42G reported that update; this refinement explicitly supersedes that inaccurate statement.
- critic-item-4: `answered` - There is no remaining mismatch once the ticket stops claiming to retire P1.04: the canonical gap matrix stays authoritative as an evidence gap, while this ticket records the current Oracle implementation baseline and the evidence boundary for any later reopening.

Clarifications
- A previous PO run already refreshed the durable ticket description in commit 7c29bd76c, and comment 06FCWF63ZWP8E59NABEH1QS42G explicitly reported that update; this refinement replaces the inaccurate no-description-update wording rather than repeating it.
- The canonical planning surface remains docs/plans/provider-optimization-gap-matrix.md:59, where P1.04 is still an Oracle provider-native-bulk-ingestion evidence gap; this ticket no longer claims to close or reclassify that backlog row.
- The repository baseline remains: OracleDataVaultSaveStrategy keeps DirectOracleBatching with optional ArrayBindCount array binding, and staged Oracle bulk remains not-selected-no-measured-win.
- The Oracle optimized-save boundary remains provider name Oracle.EntityFrameworkCore, clean DbContext, no multi-active satellite operations, minimum 50 total operations, and maximum 10000 satellite operations; outside that gate the product falls back to the provider-neutral writer.
- The root benchmark triplet Oracle save rows are skipped placeholders when DVAULT_TEST_ORACLE_CONNECTION_STRING is unset, so they cannot be used to retire P1.04; measured Oracle timing discussion must stay anchored to the checked-in provider-configured v0.32 artifacts.
- The only durable planning change expected from accepting this refinement is refreshing the current ticket description again; no child tickets, relation edits, attachments, or planning documents are needed.
- Downstream blocked ticket 06FBSCAJ5HDJH6CR0HZQ4B7H30 already covers the two valid outcomes for later implementation work: implement only if an improvement is accepted, otherwise close no-work-required.

Scope In
- Reframe this ticket as a bounded Oracle bulk evaluation and recommendation, not as closure of canonical backlog row P1.04.
- Document the current Oracle direct batching and array-binding baseline together with the existing 50-operation and 10000-satellite fallback gate.
- State the evidence boundary clearly: P1.04 stays open until provider-configured Oracle benchmark evidence justifies changing the canonical gap-matrix posture.

Scope Out
- Editing docs/plans/provider-optimization-gap-matrix.md or otherwise retiring P1.04 in this ticket.
- Implementing staged Oracle bulk, raising the 10000-satellite cap, or tuning the current threshold gates.
- Re-running benchmarks or treating skipped root benchmark-summary Oracle rows as completed timing evidence.
- Creating child tickets, relation rewrites, attachments, or planning documents unless later evidence changes the bounded recommendation.

Open questions
- none

Follow-up questions
- If a later provider-configured Oracle benchmark triplet shows a measured win for staged bulk or for a higher satellite threshold over both provider-neutral fallback and the retained direct Oracle path, should that reopen P1.04 as implementation work rather than pure evidence collection?
- After this narrowed recommendation is accepted, should downstream ticket 06FBSCAJ5HDJH6CR0HZQ4B7H30 be closed no-work-required unless new Oracle evidence changes the decision?

Risks
- P1.04 remains an open evidence-gap backlog item, so product messaging must not present this ticket as closure of Oracle save benchmarking work.
- The root benchmark-summary Oracle rows are skipped placeholders; treating them as completed timing evidence would recreate the same canonical-planning mismatch that triggered the critic block.
- Oracle workloads above 10000 satellite operations or with multi-active satellite shapes still rely on fallback behavior and remain unproven optimization territory.
- The checked-in v0.32 high-volume artifact still shows the <redacted>-satellite fallback case as a reason not to widen the current Oracle path without fresh provider-configured evidence.

Split recommendations
- No split is required for this ticket; keep it as a bounded PO recommendation and evidence-boundary clarification.
- Do not create a backlog-alignment child ticket now because the canonical gap matrix already reflects the correct evidence-gap posture for P1.04.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment