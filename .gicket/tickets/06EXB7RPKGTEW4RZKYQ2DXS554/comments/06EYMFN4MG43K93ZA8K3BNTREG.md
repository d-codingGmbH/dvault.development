[gicket-bot] PO refinement contract

Summary
- Restated parent ticket 06EXB7RPKGTEW4RZKYQ2DXS554 as an umbrella coordination story with no remaining parent-owned implementation slice; both child tickets are already done and repository evidence already contains the shared comparison contract plus matching plain EF and DVault tests.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The parent is restated as an umbrella coordination story, not a third implementation ticket. Both child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8 are already done, so this parent should not define or hand off a separate developer slice.
- critic-item-2: `answered` - If the parent remains open for workflow purposes, its only remaining ticket-level scope is umbrella alignment of the shared two-event contract and the completed child-owned baselines. There is no additional parent-owned src/ or tests/ implementation slice beyond that coordination role.
- critic-item-3: `answered` - Confirmed. Repository evidence already contains the child-owned plain EF and DVault implementation coverage, and current branch diff evidence shows no parent-owned repository delta beyond ticket metadata. The contract is corrected to make that explicit so the parent is not treated as duplicate developer work.

Clarifications
- This parent story is an umbrella coordination ticket for the shared customer-profile comparison scenario, not a separate third implementation ticket.
- Child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF baseline and is already done.
- Child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault baseline and is already done.
- Persisted parentOf relations already link 06EXB7RPKGTEW4RZKYQ2DXS554 to both child tickets.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md remains the authoritative shared two-event C-100 comparison contract.
- Current branch evidence shows no additional parent-owned repository implementation delta beyond ticket metadata, so no new parent dev slice is defined here.
- No new child tickets, relation writes, attachments, or planning documents were needed in this refinement run.

Scope In
- Keep the parent as umbrella coordination for the shared two-event C-100 customer-profile comparison across the existing plain EF and DVault child tickets.
- Maintain the authoritative shared scenario and persisted-outcome contract in docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md.
- Ratify alignment between the shared contract and the existing automated repository evidence under tests/DCoding.Data.DVault.Tests.
- Record explicitly that the child tickets own the storage-specific implementation details and the parent owns no separate repository implementation slice.

Scope Out
- Any new parent-owned src/ or tests/ implementation work for this story.
- Re-implementing behavior already completed in child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 or child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8.
- A third implementation pass under the parent ticket.
- Standalone sample applications, a new examples/ surface, or broader relationship demos for v1.
- Additional timestamps, replay or dedup variants, or deferred Data Vault capabilities beyond the locked two-event contract.

Open questions
- none

Follow-up questions
- After this umbrella story is advanced or closed, should the same comparison scenario also be promoted from test-only coverage into a runnable example or documentation sample?
- If more comparison scenarios are added later, should a separate follow-up ticket introduce shared fixtures or assertion helpers so the plain EF and DVault baselines stay synchronized in code as well as in the planning contract?

Risks
- Scope confusion could return if the parent is later treated as a third implementation ticket instead of an umbrella coordination story.
- The comparison loses value if either child implementation or the shared planning document drifts from the locked two-event C-100 contract.
- If the SQLite DVault baseline or naming conventions change, the shared contract and both test surfaces may require coordinated updates.

Split recommendations
- No further split is recommended; the correct resolution is to keep the existing split into child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8 and not add a new parent-owned implementation slice.
- If stakeholders later want a runnable example, broader relationship demos, or additional history variants, create separate follow-up tickets instead of reopening this parent story as developer work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment