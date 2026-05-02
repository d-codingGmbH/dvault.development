[gicket-bot] PO refinement contract

Summary
- Reconfirmed the parent as a coordination-only umbrella for the shared C-100 comparison scenario, with existing child ownership and repository evidence already in place; no new child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - A developer handoff is not desired for this parent. Keep it as a coordination-only umbrella: child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF slice, child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault slice, and the parent should close or advance after PO-critic rather than define a third implementation or test slice.
- critic-item-2: `answered` - The parent is intentionally not a developer-owned implementation ticket. The comparison baseline is already covered through the two child-owned repository surfaces: `PlainEfCustomerProfileHistorySqliteTests` asserts exactly two persisted C-100 history rows, and `DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite` asserts one `HubCustomer` row plus two `SatCustomerProfile` rows for the same two events.
- critic-item-3: `answered` - Live metadata still reads as `todo` with `blocked/dev`, `blocked/test`, and `needs-po`, but the refined contract treats that mismatch as metadata reconciliation rather than evidence of remaining parent-owned work. The parent stays coordination-only and should close or advance after PO-critic instead of going to development.

Clarifications
- This parent story is an umbrella coordination ticket for the shared customer-profile comparison scenario, not a third implementation ticket.
- Child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF comparison slice and child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault comparison slice.
- Persisted `parentOf` relations already link 06EXB7RPKGTEW4RZKYQ2DXS554 to both child tickets.
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` remains the authoritative shared two-event C-100 comparison contract.
- `tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` already provide the repository evidence aligned to that shared contract.
- Live ticket metadata currently still shows `todo` plus dev and test blocking labels, but that does not reopen a parent-owned developer slice.
- No new child tickets, relation writes, attachments, or planning documents were created in this refinement run.

Scope In
- Keep the parent as umbrella coordination for the shared two-event C-100 customer-profile comparison across the existing plain EF and DVault child tickets.
- Maintain the authoritative shared scenario and persisted-outcome contract in `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md`.
- Ratify alignment between the shared contract and the existing repository evidence in `tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- Record explicitly that the child tickets own the storage-specific implementation details and the parent owns no separate repository implementation slice.

Scope Out
- Any new parent-owned `src/` or `tests/` implementation work for this story.
- Re-implementing behavior already owned by child ticket 06EXB7RYFJ3YQDB1E4QHPP8034 or child ticket 06EXB7S6DB97GVVTS2GGZ3CCX8.
- A third implementation pass under the parent ticket.
- Standalone sample applications, a new `examples/` surface, or broader relationship demos in this ticket.
- Additional timestamps, replay or dedup variants, or deferred Data Vault capabilities beyond the locked two-event contract.

Open questions
- none

Follow-up questions
- After this umbrella story is closed or advanced, should the same comparison scenario also be promoted from test-only coverage into a runnable example or documentation sample?
- If more comparison scenarios are added later, should a follow-up ticket introduce shared fixtures or assertion helpers so the plain EF and DVault baselines stay synchronized in code as well as in the planning contract?

Risks
- Scope confusion could return if the parent is later reopened as a third implementation ticket instead of remaining an umbrella coordination story.
- The comparison loses value if either child implementation or the shared planning document drifts from the locked two-event C-100 contract.
- If the SQLite DVault baseline or naming conventions change, the shared contract and both test surfaces may require coordinated updates.

Split recommendations
- No further split is recommended; keep the existing split into child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8 and do not add a new parent-owned implementation slice.
- If stakeholders later want a runnable example, broader relationship demos, or additional history variants, create separate follow-up tickets instead of reopening this parent story as developer work.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment