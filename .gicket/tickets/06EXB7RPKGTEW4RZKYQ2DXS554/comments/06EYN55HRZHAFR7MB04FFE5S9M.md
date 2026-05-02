[gicket-bot] PO-critic review contract

Summary
- The parent story is refined and repository-backed, but it is not ready for developer handoff because the persisted contract defines it as a coordination-only umbrella with no remaining parent-owned implementation slice.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md states the parent remains a coordination-only umbrella, should only advance through coordination completion, and has `## Open Questions` -> `- none`.
- .gicket/relations/54/34/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7RYFJ3YQDB1E4QHPP8034--parentOf.json and .gicket/relations/54/X8/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7S6DB97GVVTS2GGZ3CCX8--parentOf.json persist the parent->child links claimed by the contract.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md fixes the shared two-event `C-100` scenario; tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs asserts exactly two persisted history rows, and tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs method `DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite` asserts one `HubCustomer` row plus two `SatCustomerProfile` rows with hub reuse on the second event.
- src/DCoding.Data.DVault/DataVaultSaveService.cs directly exposes `IDataVaultSaveService`, `DataVaultSaveRequest`, and `DataVaultSatelliteSaveOperation`, matching the parent contract's cited public boundary.
- `git diff --name-only f09f90eb355182c23033b1bba082c44cc75ee9c4...HEAD` produced only `.gicket` paths, filtering for non-`.gicket` files returned no output, and `git show --stat --summary --format=fuller HEAD -- .gicket` shows HEAD `00762884e6913392bb41498528bf4cde3a966d3e` is a po-critic lease-claim commit that only touched `.gicket` metadata files.

Blocking findings
- none

Required PO actions
- Update the parent ticket's status, labels, and handoff metadata so it follows a coordination-only closure path instead of a developer handoff path.

Open issues ledger
- critic-item-1 [required-po-action] Update the parent ticket's status, labels, and handoff metadata so it follows a coordination-only closure path instead of a developer handoff path.

Missing examples / edge cases
- None blocking at parent level; the contract already locks the scope to the two-event `C-100` comparison and explicitly scopes out runnable examples, replay/dedup variants, and extra history cases.

Risky assumptions
- Assuming downstream automation or reviewers will infer 'no developer work remains' from the narrative alone, despite the persisted ticket still looking like a normal `todo` story with dev/test blocker labels.
- Assuming this umbrella can safely pass through a developer queue without being reopened as a third implementation ticket.

AC / test suggestions
- Add an explicit ticket-level closure criterion that the parent exits PO-critic without any parent-owned dev/test slice or new non-`.gicket` repository diff.
- Add a ticket-field acceptance check that the coordination-only parent's blocker labels are removed as part of the completion transition.

Implementation watchouts
- Do not reopen this parent for runnable example or documentation-sample work; the refined contract says those belong in separate follow-up tickets.
- If the shared comparison contract or child evidence changes later, update or create child/follow-up tickets rather than turning this umbrella into a third storage-specific implementation ticket.

Non-blocking notes
- The prompt snapshot said recent comments were `<none>`, but the local .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/comments/ directory contains current PO and po-critic orchestration comments; repository comment history is the reliable source here.
- The latest parent comment set includes .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/comments/06EYN24WJ24632JQW543VQVF9R.md, which records the PO handoff to `po-critic`, and .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/comments/06EYN44EZD52B585VM14XJBGDR.md, which records the current po-critic claim.

Split recommendations
- No further split for this umbrella. Keep child ownership with 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- If stakeholders still want a runnable example, broader relationship demo, or more history variants, create a new follow-up ticket instead of routing this parent to development.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment