[gicket-bot] PO refinement contract

Summary
- Reconfirmed the parent as a coordination-only umbrella, ratified that child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8 are already done, and answered the PO-critic findings with repository evidence showing no remaining parent-owned development slice.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - No new parent-owned implementation slice is being added. The parent remains a coordination-only umbrella, stays unassigned to development, and should close or otherwise advance after PO-critic instead of being handed to a developer.
- critic-item-2: `answered` - Developer handoff remains out of contract. Both implementation-owning child tickets are already done, the shared comparison contract and integration tests already cover the plain EF and DVault slices, and the reviewed diff range contains only .gicket metadata changes, so the parent has no remaining implementation slice to send to development.

Clarifications
- Ticket 06EXB7RPKGTEW4RZKYQ2DXS554 remains a coordination-only umbrella for the shared two-event C-100 customer-profile comparison and is not a third implementation ticket.
- Child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF comparison slice and is already done.
- Child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault comparison slice and is already done.
- Persisted parentOf relations already link the parent to both child tickets.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md remains the authoritative shared two-event C-100 comparison contract.
- The parent remains unassigned to development; current workflow labels do not reopen a parent-owned implementation slice.
- No new child tickets, relation writes, attachments, or planning documents were created in this refinement run.

Scope In
- Keep the parent as umbrella coordination for the shared two-event C-100 customer-profile comparison across child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- Maintain the authoritative shared scenario and persisted-outcome contract in docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md.
- Ratify that the existing repository evidence in tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs already satisfies the two child-owned implementation slices.
- Record explicitly that the parent owns no separate src/ or tests/ implementation slice and should not be assigned to development unless a new parent-owned slice is intentionally added later.

Scope Out
- Any new parent-owned src/ or tests/ implementation work for this story.
- Re-implementing behavior already owned by child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 or child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8.
- A third implementation pass under the parent ticket.
- Standalone sample applications, a new examples/ surface, or broader relationship demos in this ticket.
- Additional timestamps, replay or dedup variants, or deferred Data Vault capabilities beyond the locked two-event contract.

Open questions
- none

Follow-up questions
- After this umbrella story is closed or otherwise advanced, should the same comparison scenario also be promoted from test-only coverage into a runnable example or documentation sample?
- If more comparison scenarios are added later, should a follow-up ticket introduce shared fixtures or assertion helpers so the plain EF and DVault baselines stay synchronized in code as well as in the planning contract?

Risks
- Scope confusion could return if the parent is later reopened as a third implementation ticket instead of remaining an umbrella coordination story.
- The comparison loses value if either child implementation or the shared planning document drifts from the locked two-event C-100 contract.
- Operational risk remains if downstream workflow ignores the coordination-only contract and tries to route this parent to development even though both child-owned implementation slices are already complete.
- If the SQLite DVault baseline or naming conventions change, the shared contract and both test surfaces may require coordinated updates.

Split recommendations
- No further split is recommended; keep the existing split into child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8 and do not add a new parent-owned implementation slice.
- If stakeholders later want a runnable example, broader relationship demos, or additional history variants, create separate follow-up tickets instead of reopening this parent story as developer work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment