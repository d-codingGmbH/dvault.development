[gicket-bot] PO-critic review contract

Summary
- Repository and ticket evidence show the story content is already satisfied by the shared contract and two done child tickets, but the parent ticket is explicitly coordination-only and its current persisted state still points at developer/test routing, so it should return to PO for ticket-level closure-path correction.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- repository-read-text on docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md shows the authoritative two-event C-100 comparison contract and exact persisted outcomes for the two child-owned slices.
- repository-read-text on tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs shows ConventionalEfBaselinePersistsExactCustomerProfileHistoryRowsThroughSqlite asserts exactly 2 persisted customer-profile history rows for C-100 matching Alice Adams/prospect and Alice Baker/active.
- repository-read-text on tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs shows DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite asserts 1 HubCustomer row, 2 SatCustomerProfile rows, secondHubResult.RowsWritten == 0, and secondSatelliteResult.RowsWritten == 1 for the same two events.
- repository-read-text on src/DCoding.Data.DVault/DataVaultSaveService.cs directly exposes IDataVaultSaveService, DataVaultSaveRequest, and DataVaultSatelliteSaveOperation, matching the public boundary cited by the parent contract.
- git -C /mnt/c/Projects/DVault ls-files .gicket/relations | rg '06EXB7RPKGTEW4RZKYQ2DXS554.*(06EXB7RYFJ3YQDB1E4QHPP8034|06EXB7S6DB97GVVTS2GGZ3CCX8).*parentOf' returned .gicket/relations/54/34/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7RYFJ3YQDB1E4QHPP8034--parentOf.json and .gicket/relations/54/X8/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7S6DB97GVVTS2GGZ3CCX8--parentOf.json.
- git -C /mnt/c/Projects/DVault rev-parse HEAD returned c91bdfadcffeb3e4d780779297b7216c1a143e44, git -C /mnt/c/Projects/DVault diff --name-only c91bdfadcffeb3e4d780779297b7216c1a143e44..HEAD returned no paths, and git -C /mnt/c/Projects/DVault show --stat --summary --format=fuller HEAD shows HEAD is the po-critic lease-claim commit touching only .gicket files.

Blocking findings
- The persisted delivery contract defines 06EXB7RPKGTEW4RZKYQ2DXS554 as a coordination-only umbrella with no parent-owned implementation slice, so approving it for developer handoff would misroute a ticket that has no remaining developer work.

Required PO actions
- Update the parent ticket's status and handoff metadata so it follows a coordination-only completion or closure path instead of the po-critic-to-dev route.
- Keep the parent scoped as umbrella coordination only; do not create or reopen a parent-owned src/ or tests/ implementation slice.

Open issues ledger
- critic-item-1 [required-po-action] Update the parent ticket's status and handoff metadata so it follows a coordination-only completion or closure path instead of the po-critic-to-dev route.
- critic-item-2 [required-po-action] Keep the parent scoped as umbrella coordination only; do not create or reopen a parent-owned src/ or tests/ implementation slice.
- critic-item-3 [blocking-finding] The persisted delivery contract defines 06EXB7RPKGTEW4RZKYQ2DXS554 as a coordination-only umbrella with no parent-owned implementation slice, so approving it for developer handoff would misroute a ticket that has no remaining developer work.

Missing examples / edge cases
- The ticket intentionally leaves runnable-example or documentation-sample promotion as a follow-up question; if that work is desired, it should be captured in a separate ticket rather than treated as unresolved scope on this umbrella.

Risky assumptions
- Assuming downstream automation or reviewers will infer 'no developer work remains' from the narrative alone despite the current status and labels still signaling dev/test blockage.
- Assuming this umbrella can safely enter a developer queue without being reopened as a third implementation ticket.

AC / test suggestions
- If the intended outcome is non-developer closure, make that closure path explicit in ticket-level handoff/status metadata, not only in the delivery-contract narrative.
- Keep parent acceptance focused on the shared contract, done child tickets, and existing parentOf links; place any future runnable-example goal in a separate follow-up ticket.

Implementation watchouts
- Do not hand this parent to dev/test automation as if it owns code work; all storage-specific implementation ownership already sits with 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- If stakeholders later want a runnable example or broader demo, create separate follow-up work so this umbrella is not mistaken for a third implementation pass.

Non-blocking notes
- Repository evidence already covers the substantive scenario on both the plain EF and DVault sides; the blocker here is ticket workflow/state, not missing repository implementation evidence.
- The parent contract's Open Questions section is 'none', so there is no unresolved contract ambiguity preventing ticket-level completion once routing metadata is corrected.

Split recommendations
- No further split is recommended for this umbrella; keep implementation ownership with child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.
- If stakeholders still want a runnable example, broader relationship demo, or additional history variants, create separate follow-up tickets instead of reopening this parent for developer work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment