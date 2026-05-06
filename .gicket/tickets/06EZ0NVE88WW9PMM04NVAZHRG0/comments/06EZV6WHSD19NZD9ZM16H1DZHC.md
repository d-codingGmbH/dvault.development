[gicket-bot] PO-critic review contract

Summary
- The ticket is now source-backed, label-aligned for PO-critic routing, and bounded enough for developer handoff as a docs-only deferred-bridge task.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/description.md contains the authoritative Delivery Contract with `## Open Questions` = `none`, five acceptance-criteria items, and implementation notes tied to concrete repository sources.
- PO republished the handoff in .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0/comments/06EZTT5Y4CAZKRR93YHKTVZWNW.md, explicitly answered `critic-item-1`, kept `decision: ready_for_po_critic`, and planned the label correction.
- `git show --stat e8459f19a26b583d2d7215035d3c809c9e819a7d` shows the latest substantive handoff commit updated only this ticket’s .gicket artifacts, including description.md and ticket.json; `git log --oneline -n 3 -- .gicket/tickets/06EZ0NVE88WW9PMM04NVAZHRG0` shows HEAD `8a200b32` is only the later po-critic lease-claim commit.
- docs/plans/deferred-data-vault-capabilities.md states bridge tables are a deferred opt-in v0.5 capability and not part of ordinary hub/link/satellite setup.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs `CreateEntities` iterates only hubs, links, and satellites; src/DCoding.Data.DVault/Modeling/DataVaultModel.cs defines `DataVaultTableKind` values `Hub`, `Link`, and `Satellite`; src/DCoding.Data.DVault/DataVaultAnnotationNames.cs defines no bridge-specific annotation constants.
- Direct source evidence for the required public vocabulary exists on branch: src/DCoding.Data.DVault/DVaultServiceCollectionExtensions.cs exposes `AddDVault()`, src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs exposes `UseDataVault()` and `ApplyDataVaultMetadata()`, and src/DCoding.Data.DVault/DataVaultSaveService.cs defines `IDataVaultSaveService`.
- The ticket-tree relation already exists in .gicket/relations/CC/G0/06EZ0NTV4SVAKV98C418T8A3CC--06EZ0NVE88WW9PMM04NVAZHRG0--parentOf.json.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- The contract requires exactly one conceptual many-to-many scenario but does not pin the business nouns; acceptance should enforce one bounded example and reject hierarchy, PIT, provider-specific, or multi-active expansion.
- The target documentation page/path is not named; implementation should stay to one bounded bridge doc page rather than broad README expansion.

Risky assumptions
- Parent story `06EZ0NTV4SVAKV98C418T8A3CC` is still `needs-po` in .gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/ticket.json; this child assumes any later parent-specific bridge names or shapes can be handled as a follow-up docs sync without invalidating the current deferred-baseline page.

AC / test suggestions
- Review the finished docs against the existing source-backed surface only: `AddDVault()`, `UseDataVault()`, `ApplyDataVaultMetadata()`, and `IDataVaultSaveService`.
- Verify the page states that bridge EF projection and bridge annotation names are not exposed on the current branch.
- Verify the page includes exactly one conceptual many-to-many example and explicitly marks hierarchy, provider-specific, PIT, and multi-active behavior as deferred or unsupported.

Implementation watchouts
- Do not imply a concrete bridge runtime surface, generated bridge table shape, or bridge-specific EF metadata because the visible source only projects hubs, links, and satellites.
- Do not reference nonexistent symbols such as `DataVaultTableKind.Bridge` or bridge annotation constants.
- Keep any example anchored to repository nouns already present in docs, benchmarks, README, or tests, such as customer-order or order-product relationships.

Non-blocking notes
- The repo already contains existing relationship nouns in README.md, docs/architecture/mvp-data-vault-concepts.md, benchmark docs, and tests, so the conceptual example can be source-aligned without inventing new APIs.

Split recommendations
- No split is required for the current bounded docs-only task.
- If later work needs hierarchy-specific walkthroughs, runnable samples, or docs tied to a concrete bridge implementation surface, keep that as a separate follow-up ticket.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment