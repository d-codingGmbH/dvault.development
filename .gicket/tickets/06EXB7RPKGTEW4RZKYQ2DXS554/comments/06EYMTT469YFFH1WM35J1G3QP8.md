[gicket-bot] PO-critic review contract

Summary
- Ticket contract requires substantive product-owner changes before development.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EXB7RPKGTEW4RZKYQ2DXS554/description.md says this parent is an umbrella coordination ticket, that both child tickets own the storage-specific implementation slices, that `## Open Questions` is `none`, and that the correct parent resolution is close or advance after PO-critic rather than a separate developer handoff.
- .gicket/workflow.json allows `todo -> ready-for-integration -> done`, so a close or advance path exists in repo workflow, but it has not been applied to the parent ticket.
- .gicket/relations/54/34/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7RYFJ3YQDB1E4QHPP8034--parentOf.json and .gicket/relations/54/X8/06EXB7RPKGTEW4RZKYQ2DXS554--06EXB7S6DB97GVVTS2GGZ3CCX8--parentOf.json directly persist the two child links.
- docs/plans/customer-profile-comparison-contract-06EXB7RY-06EXB7S6.md fixes the shared two-event `C-100` scenario and exact persisted outcomes for child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- tests/DCoding.Data.DVault.Tests/Integration/PlainEfCustomerProfileHistorySqliteTests.cs defines the same two events and asserts exactly 2 persisted `CustomerProfileHistory` rows for `C-100`.
- tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs contains `DefaultSaveServicePersistsCustomerProfileSatelliteHistoryThroughSqlite`, which asserts 1 `HubCustomer` row and 2 `SatCustomerProfile` rows for the same two events; src/DCoding.Data.DVault/DataVaultSaveService.cs directly defines `IDataVaultSaveService`, `DataVaultSaveRequest`, and `DataVaultSatelliteSaveOperation`.
- git -C /mnt/c/Projects/DVault rev-parse HEAD returned `40738eb0dbc827c21e4cd6bf90c4d4ffdab0ccf9`, matching the prompt scratch-source-ref; `git diff --name-only f09f90eb355182c23033b1bba082c44cc75ee9c4...HEAD` listed only `.gicket/**` files, and the same diff excluding `.gicket/**` returned no files.

Blocking findings
- The ticket is not ready for developer handoff because its own persisted contract says the parent owns no separate implementation slice and must close or advance after PO-critic rather than go to dev.
- The live ticket metadata has not been reconciled with that contract: it remains `todo` and still carries dev/test blocking and critic-routing labels instead of a terminal or advance state.

Required PO actions
- If a developer handoff is actually desired, rewrite the delivery contract so the parent owns a concrete implementation/test slice; otherwise keep the coordination-only contract and close or advance the parent umbrella.

Open issues ledger
- critic-item-1 [required-po-action] If a developer handoff is actually desired, rewrite the delivery contract so the parent owns a concrete implementation/test slice; otherwise keep the coordination-only contract and close or advance the parent umbrella.
- critic-item-2 [blocking-finding] The ticket is not ready for developer handoff because its own persisted contract says the parent owns no separate implementation slice and must close or advance after PO-critic rather than go to dev.
- critic-item-3 [blocking-finding] The live ticket metadata has not been reconciled with that contract: it remains `todo` and still carries dev/test blocking and critic-routing labels instead of a terminal or advance state.

Missing examples / edge cases
- None identified at the parent ticket level; the shared contract and both child-owned test surfaces already cover the locked two-event `C-100` scenario.

Risky assumptions
- Assuming future reviewers will not reopen this umbrella as a third implementation ticket despite the legacy story framing and `area/examples` label.

AC / test suggestions
- After status and label cleanup, add a short ticket comment confirming the parent is coordination-only and that no parent-owned dev/test work remains.

Implementation watchouts
- Do not reopen this parent as a third implementation ticket; keep storage-specific work on child tickets `06EXB7RYFJ3YQDB1E4QHPP8034` and `06EXB7S6DB97GVVTS2GGZ3CCX8`.
- Do not widen this umbrella into runnable examples or extra comparison variants without a separate follow-up ticket.

Non-blocking notes
- The stale diff wording appears corrected: the current contract now matches the observed branch history by distinguishing `.gicket` metadata changes from non-`.gicket` implementation files.

Split recommendations
- No further split recommended; keep the current umbrella-plus-two-children structure and resolve the parent through workflow/status cleanup rather than new implementation tickets.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment