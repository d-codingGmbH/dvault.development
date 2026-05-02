[gicket-bot] PO-critic review contract

Summary
- The parent ticket has aligned child-ticket repository evidence, but it is not ready for developer handoff because the persisted contract and live ticket metadata still describe a coordination-only umbrella story with no remaining parent-owned dev slice, and one branch-diff evidence statement is factually overstated.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md` states this parent is an umbrella coordination story with "no remaining parent-owned implementation slice" and `## Open Questions` is `- none`.
- `docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:12-57` fixes the shared two-event `C-100` scenario and exact persisted-outcome contract for both child tickets.
- `tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs:10-52` defines the two shared events and asserts exactly 2 persisted history rows for `C-100`.
- `tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:353-476` asserts 1 `HubCustomer` row and 2 `SatCustomerProfile` rows for the same two events, and `src/DCoding.Data.DVault/DataVaultSaveService.cs:10-21,35-66,198-210` directly defines the public `IDataVaultSaveService`, `DataVaultSaveRequest`, and `DataVaultSatelliteSaveOperation` boundary referenced by the contract.
- `git diff --name-only f09f90eb355182c23033b1bba082c44cc75ee9c4...HEAD` returned only `.gicket/**` paths, while `git diff --name-only f09f90eb355182c23033b1bba082c44cc75ee9c4...HEAD -- . ':(exclude).gicket/**'` returned no files. There is no parent-owned repo implementation delta outside ticket metadata.
- `git rev-parse HEAD` returned `551345f154eb9c1689dadacaef20b9e2bb286c2c`, and `git diff --name-only 551345f154eb9c1689dadacaef20b9e2bb286c2c...HEAD` was empty.

Blocking findings
- Approving this ticket for dev would hand a coordination-only umbrella story to the developer path even though the persisted contract explicitly says no separate parent-owned implementation slice remains.
- The contract's Implementation Notes claim an "empty diff" from `f09f90eb355182c23033b1bba082c44cc75ee9c4` to `HEAD`, but that ref range is not literally empty; it contains `.gicket` metadata changes. The intended evidence is narrower: no non-`.gicket` implementation diff.

Required PO actions
- Persist the correct terminal workflow for this umbrella parent: either close/advance it without a dev handoff, or explicitly define a real parent-owned implementation slice if developer work is still intended.
- Correct the stale diff evidence in the contract so it matches observed refs and outputs, including the distinction between `.gicket` ticket metadata changes and repository implementation changes.

Open issues ledger
- critic-item-1 [required-po-action] Persist the correct terminal workflow for this umbrella parent: either close/advance it without a dev handoff, or explicitly define a real parent-owned implementation slice if developer work is still intended.
- critic-item-2 [required-po-action] Correct the stale diff evidence in the contract so it matches observed refs and outputs, including the distinction between `.gicket` ticket metadata changes and repository implementation changes.
- critic-item-3 [blocking-finding] Approving this ticket for dev would hand a coordination-only umbrella story to the developer path even though the persisted contract explicitly says no separate parent-owned implementation slice remains.
- critic-item-4 [blocking-finding] The contract's Implementation Notes claim an "empty diff" from `f09f90eb355182c23033b1bba082c44cc75ee9c4` to `HEAD`, but that ref range is not literally empty; it contains `.gicket` metadata changes. The intended evidence is narrower: no non-`.gicket` implementation diff.

Missing examples / edge cases
- The contract does not define the ticket-level exit case for an umbrella parent whose child tickets are already done and whose branch has no parent-owned implementation delta.

Risky assumptions
- It assumes readers will interpret the cited "empty diff" as "no implementation diff" despite the stated ref range containing ticket-metadata changes.
- It assumes the existing title/labeling around "example" will not pull this umbrella ticket back into runnable-sample scope that the contract explicitly scopes out.

AC / test suggestions
- Add a ticket-level acceptance criterion for the umbrella parent's workflow outcome after both child tickets are done, so future critics do not have to infer whether the parent should go to dev, close, or skip implementation roles.
- If branch-diff evidence remains part of the contract, phrase it as "no non-.gicket repository implementation diff" and name the exact verified ref range.

Implementation watchouts
- Do not reopen this parent as a third implementation ticket; any new runnable example or broader scenario should be a separate follow-up ticket.
- If this ticket is pushed through dev unchanged, it risks duplicate work or contract drift because the actual implementation ownership already sits with child tickets 06EXB7RYFJ3YQDB1E4QHPP8034 and 06EXB7S6DB97GVVTS2GGZ3CCX8.

Non-blocking notes
- The prompt snapshot said recent comments were absent, but the persisted ticket has current comments under `.gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/comments/`, including the PO refinement contract comment and the po-critic claim/handoff comments.
- The repository evidence itself is aligned; the blocking issue is workflow and evidence precision on the parent ticket, not missing child implementation coverage.

Split recommendations
- Keep the existing split: the plain EF work stays in 06EXB7RYFJ3YQDB1E4QHPP8034 and the DVault work stays in 06EXB7S6DB97GVVTS2GGZ3CCX8.
- If stakeholders want a runnable example, broader relationship demo, or more comparison variants, create a new follow-up ticket instead of routing this umbrella parent to dev.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment