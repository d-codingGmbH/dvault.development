[gicket-bot] PO-critic review contract

Summary
- Child-ticket and repository evidence are complete, but this parent umbrella cannot be approved for developer handoff because its own contract says no parent-owned development slice remains and the current workflow state would still route it to dev.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md:12 says the parent remains a coordination-only umbrella; :54 says it should close or otherwise advance after PO-critic instead of being handed to development; :56-57 show `## Open Questions` = `none`.
- .gicket/relations/54/34/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7RYFJ3YQDB1E4QHPP8034--parentOf.json and .gicket/relations/54/X8/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7S6DB97GVVTS2GGZ3CCX8--parentOf.json persist the two child links.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md:12,38-39,54-55 fixes the shared `C-100` two-event scenario and the exact plain-EF and DVault stored outcomes.
- tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs:26 defines `ConventionalEfBaselinePersistsExactCustomerProfileHistoryRowsThroughSqlite`, and :49 asserts exactly 2 persisted history rows.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs:353 defines `DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite`; :431 asserts `0` new hub rows on the second event; :459 asserts exactly 2 `SatCustomerProfile` rows.
- src/DCoding.Data.DVault/DataVaultSaveService.cs:10,27,190 defines the public `IDataVaultSaveService`, `DataVaultSaveRequest`, and `DataVaultSatelliteSaveOperation` boundary used by the DVault child test.
- `git diff --name-only f09f90eb355182c23033b1bba082c44cc75ee9c4...HEAD` returned only `.gicket/...` paths, and `git show --stat --name-only --format=fuller 2ade9400e231` shows the PO->PO-critic handoff commit touched only `.gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/...` files.
- .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/comments/06EYMYNYZMXEDJ36VEXP2NXENC.md records the explicit operational risk that workflow might route this parent to development even though both child-owned implementation slices are already complete.

Blocking findings
- Approving this ticket for dev would contradict the persisted contract: the parent is explicitly coordination-only and has no remaining parent-owned repository work to hand to a developer.
- The parent's current ticket state still advertises active downstream work (`todo`, `critic-needed`, `blocked/dev`, `blocked/test`), so the automation surface is not aligned with the intended non-dev next step.

Required PO actions
- Change the parent ticket's workflow outcome so PO-critic success closes or otherwise advances the umbrella without routing it to `dev`.
- Update the parent status/labels to remove the misleading dev/test/critic blockers once the non-dev advance path is chosen.
- If stakeholders actually want new work on the parent, add a new explicit parent-owned scope before any developer handoff instead of relying on the current umbrella contract.

Open issues ledger
- critic-item-1 [required-po-action] Change the parent ticket's workflow outcome so PO-critic success closes or otherwise advances the umbrella without routing it to `dev`.
- critic-item-2 [required-po-action] Update the parent status/labels to remove the misleading dev/test/critic blockers once the non-dev advance path is chosen.
- critic-item-3 [required-po-action] If stakeholders actually want new work on the parent, add a new explicit parent-owned scope before any developer handoff instead of relying on the current umbrella contract.
- critic-item-4 [blocking-finding] Approving this ticket for dev would contradict the persisted contract: the parent is explicitly coordination-only and has no remaining parent-owned repository work to hand to a developer.
- critic-item-5 [blocking-finding] The parent's current ticket state still advertises active downstream work (`todo`, `critic-needed`, `blocked/dev`, `blocked/test`), so the automation surface is not aligned with the intended non-dev next step.

Missing examples / edge cases
- None blocking on this umbrella ticket; the current contract intentionally defers any runnable example or broader demo to follow-up work rather than a parent-owned implementation slice.

Risky assumptions
- Automation or manual triage will honor the description-level instruction to close/advance the parent instead of following the configured `po-critic.on-success: dev` path.
- The `area/examples` story framing will not cause the parent to be reopened as a third implementation ticket despite the contract saying the implementation lives only in the two done child tickets.

AC / test suggestions
- Make the non-dev close/advance condition machine-checkable in ticket metadata or workflow policy so the parent does not depend on description text alone.

Implementation watchouts
- Do not route this parent to a developer as a third implementation pass; any runnable example, broader demo, or extra history variant should be a separate follow-up ticket.
- Do not clear or blur the existing child ownership boundaries when fixing the parent workflow state.

Non-blocking notes
- The shared comparison contract and both repository test surfaces are aligned on the same locked two-event `C-100` scenario.
- The persisted delivery contract has no unresolved `## Open Questions`.

Split recommendations
- No further split is needed for the current umbrella ticket.
- If a runnable example or documentation sample is desired later, create a new follow-up ticket instead of reopening this parent for development.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment