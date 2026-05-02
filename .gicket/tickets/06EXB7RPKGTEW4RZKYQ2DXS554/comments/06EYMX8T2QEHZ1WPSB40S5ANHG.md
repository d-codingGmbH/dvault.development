[gicket-bot] PO-critic review contract

Summary
- Repository evidence supports the coordination-only umbrella contract, but the persisted parent ticket still routes to developer/test work even though the contract and child ticket state show no remaining parent-owned implementation slice.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md` says the parent is coordination-only, both child tickets own storage-specific work, `## Open Questions` is `none`, and the intended post-review resolution is to close or advance this umbrella rather than hand it to development.
- `.gicket/relations/54/34/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7RYFJ3YQDB1E4QHPP8034--parentOf.json` and `.gicket/relations/54/X8/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7S6DB97GVVTS2GGZ3CCX8--parentOf.json` confirm the parent-child links.
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md` fixes the shared two-event `C-100` contract and exact persisted outcomes for both the plain EF and DVault child tickets.
- `tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs` asserts exactly 2 `CustomerProfileHistory` rows for `C-100` and checks the `Alice Adams/prospect/crm-import` and `Alice Baker/active/crm-change` rows in order.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs` contains `DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite`, which writes one `HubCustomer` row, then two `SatCustomerProfile` rows, with the second hub save writing `0` rows and the second satellite save writing `1` row.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs` directly exposes `IDataVaultSaveService`, `DataVaultSaveRequest`, and `DataVaultSatelliteSaveOperation`, matching the contract's cited public boundary.
- `git -C /mnt/c/Projects/DVault diff --name-only f09f90eb355182c23033b1bba082c44cc75ee9c4...HEAD` returned only `.gicket/...` files for the parent and child tickets; no non-`.gicket` repository implementation files were in the reviewed range.

Blocking findings
- Sending this ticket to dev would contradict the current ownership model: both implementation-owning child tickets are already `done`, and the reviewed diff range contains only `.gicket` metadata changes.

Required PO actions
- Keep the parent ticket unassigned to development unless a new parent-owned implementation slice is intentionally added to the delivery contract.

Open issues ledger
- critic-item-1 [required-po-action] Keep the parent ticket unassigned to development unless a new parent-owned implementation slice is intentionally added to the delivery contract.
- critic-item-2 [blocking-finding] Sending this ticket to dev would contradict the current ownership model: both implementation-owning child tickets are already `done`, and the reviewed diff range contains only `.gicket` metadata changes.

Missing examples / edge cases
- none

Risky assumptions
- The current workflow may assume every PO-critic success routes to `dev`, but this ticket's contract explicitly says the correct resolution is umbrella closure/advance rather than developer work.

AC / test suggestions
- none

Implementation watchouts
- Do not reopen this parent as a third implementation pass; any runnable example, broader demo, or extra history variants should remain follow-up ticket work.

Non-blocking notes
- `## Open Questions` is `none`; the remaining `## Follow-Up Questions` in the description are appropriately non-blocking.
- The current branch is `ticket/06EXB7RPKGTEW4RZKYQ2DXS554-story-build-example-scenario-for-customer-profil` at `cc7ef341`, which is a PO-critic lease-claim commit rather than a repository implementation change.

Split recommendations
- Keep the existing split: parent 06EXB7RPKGTEW4RZKYQ2DXS554 stays coordination-only, child 06EXB7RYFJ3YQDB1E4QHPP8034 owns the plain EF slice, and child 06EXB7S6DB97GVVTS2GGZ3CCX8 owns the DVault slice.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment