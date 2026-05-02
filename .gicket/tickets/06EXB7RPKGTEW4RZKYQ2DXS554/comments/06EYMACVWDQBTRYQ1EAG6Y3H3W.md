[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md:12-17,38-49` says the story is already decomposed into child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`, with those children owning the storage-specific implementation details and no new child/relation/planning writes needed.
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:12-57` fixes the exact `C-100` two-event contract and the expected plain-EF and DVault persisted outcomes for the two child tickets.
- `tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs:10-23,25-53` hard-codes the shared `C-100` / Alice Adams / Alice Baker event sequence and asserts exactly two persisted plain-EF history rows.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21,27-92,187-210` provides direct source evidence for the public explicit DVault save boundary (`IDataVaultSaveService`, `DataVaultSaveRequest`, `DataVaultSatelliteSaveOperation`), and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:353-477` uses that API to assert one `HubCustomer` row, two `SatCustomerProfile` rows, and `secondHubResult.RowsWritten == 0` for the second event.
- `git diff --name-status 550473c9..94c31711` reports only `.gicket/tickets/...` comment/event/ticket metadata changes and no implementation changes under `src/`, `tests/`, or `docs/` for this parent-story branch.

Blocking findings
- Branch-history evidence shows no parent-owned repository implementation delta beyond ticket metadata. Handing this parent story to a developer would duplicate or no-op work unless PO defines a new remaining slice.

Required PO actions
- Correct the parent ticket workflow so it matches the observed state: either close/advance `06EXB7RPKGTEW4RZKYQ2DXS554` as an umbrella story or restate a concrete remaining parent-owned slice before sending it to `dev`.
- If the parent must stay open, add explicit ticket-level scope describing what work remains that is not already owned and completed by child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`.

Open issues ledger
- critic-item-1 [required-po-action] Correct the parent ticket workflow so it matches the observed state: either close/advance `06EXB7RPKGTEW4RZKYQ2DXS554` as an umbrella story or restate a concrete remaining parent-owned slice before sending it to `dev`.
- critic-item-2 [required-po-action] If the parent must stay open, add explicit ticket-level scope describing what work remains that is not already owned and completed by child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- critic-item-3 [blocking-finding] Branch-history evidence shows no parent-owned repository implementation delta beyond ticket metadata. Handing this parent story to a developer would duplicate or no-op work unless PO defines a new remaining slice.

Missing examples / edge cases
- none

Risky assumptions
- none

AC / test suggestions
- If PO wants parent-level developer work, add a parent-owned acceptance criterion that is not already covered by `tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs` and `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs`.
- Otherwise keep this story as a tracking umbrella and let the existing child-ticket test surfaces remain the implementation proof.

Implementation watchouts
- Do not reopen the plain-EF or DVault implementation on the parent story; that work is already represented by completed child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- Do not widen this parent ticket into a standalone runnable example or documentation sample unless PO intentionally creates a separate follow-up scope.

Non-blocking notes
- `.gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md:51-52` shows `## Open Questions` is `none`.
- This read-only PO-critic run did not execute tests; the assessment is based on committed ticket state, comments, source/test files, and branch history.

Split recommendations
- No further split is needed.
- Prefer a PO workflow/status correction on the existing parent story over creating another developer slice for already integrated work.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment