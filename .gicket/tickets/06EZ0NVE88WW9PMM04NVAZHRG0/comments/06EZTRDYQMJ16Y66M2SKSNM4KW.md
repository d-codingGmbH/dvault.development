[gicket-bot] PO-critic review contract

Summary
- The refined contract is now source-backed and has no open questions, but the persisted ticket metadata still blocks developer routing, so the ticket is not yet ready for handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/description.md:5-15 and :48-49 says this is a docs-only deferred-bridge task, no longer blocked on parent naming, and `## Open Questions` is `none`.
- docs/plans/deferred-data-vault-capabilities.md:24, :37, :47-53, and :65 documents bridge tables as an opt-in deferred capability and lists the current supported baseline as hubs, links, satellites, `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, and the explicit save service.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:29-40 only creates hub/link/satellite entities; src/DCoding.Data.DVault/Modeling/DataVaultModel.cs:447-461 only defines `Hub`, `Link`, and `Satellite`; src/DCoding.Data.DVault/DataVaultAnnotationNames.cs:6-70 defines no bridge-specific annotation constants.
- src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs:16-28, src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs:17-18 and :45-73, and src/DCoding.Data.DVault/DataVaultSaveService.cs:12-35 directly expose the contract vocabulary `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, and `IDataVaultSaveService`.
- `git show --stat --name-only --format=fuller fcf27e1a7151` shows the latest handoff commit only touched `.gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/...` artifacts, confirming this branch state is still ticket-refinement evidence rather than an implementation delivery state.

Blocking findings
- none

Required PO actions
- Republish the handoff after the labels are corrected so the description, comments, and ticket.json all advertise the same routing decision.

Open issues ledger
- critic-item-1 [required-po-action] Republish the handoff after the labels are corrected so the description, comments, and ticket.json all advertise the same routing decision.

Missing examples / edge cases
- The contract requires exactly one conceptual many-to-many scenario but does not pin the business nouns; review should still enforce that only one bounded example appears and that it does not expand into hierarchy, PIT, provider-specific, or multi-active behavior.
- The target documentation page/path is not named. That is not fatal, but review should ensure the final deliverable is a bounded bridge doc page rather than a broad README expansion.

Risky assumptions
- none

AC / test suggestions
- Add a review checkpoint that the finished docs contain exactly one conceptual many-to-many scenario and zero invented bridge API names, annotation names, generated table names, or EF members.
- Add a checklist item that hierarchy behavior, provider-specific behavior, PIT implications, and multi-active implications are each explicitly called out as deferred or unsupported.

Implementation watchouts
- Do not imply a bridge runtime surface: the current source only exposes hubs, links, satellites, `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, and `IDataVaultSaveService`.
- Do not reference `DataVaultTableKind.Bridge` or bridge annotation names; those symbols do not exist on the branch.

Non-blocking notes
- Once the blocking labels are aligned with the refined contract, the ticket otherwise looks ready for developer handoff from a source-evidence standpoint.

Split recommendations
- No split is needed after metadata alignment; this remains a bounded docs-only task.
- If later work needs hierarchy-specific walkthroughs or docs tied to a concrete bridge runtime surface, keep that as a separate follow-up ticket.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment