[gicket-bot] PO-critic review contract

Summary
- Approve: this ticket now has a closed, concrete delivery contract, and its upstream metadata dependency is explicitly anchored by the attached sibling bridge contract plus the persisted blocks relation.
- decision: `approve_for_dev`
- meaning: ticket is approved for developer handoff
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/description.md now contains an authoritative contract block with Open Questions = none, names docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md as the authoritative sibling input, and pins exact BridgeCustomerOrder and BridgeSalesRegionHierarchy outputs.
- .gicket/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/comments/06EZT27RHV6TCDPQH0WEM9DQZW.md records critic-item-1 through critic-item-6 as answered and repeats the exact many-to-many and hierarchy mapping expectations for PO handoff.
- .gicket/tickets/06EZ0NV0Y81AE1Z1Q3223TX2S4/attachments/manifest.json attaches 06EZSK9Q43V2J6P9SQVTRY3W3R to the sibling ticket, and docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md defines the authoritative bridge contract, including a bridge collection on DataVaultMetadataModel, DataVaultTableKind.Bridge, and a new hierarchy-depth logical kind/semantic.
- .gicket/tickets/06EZ0NV0Y81AE1Z1Q3223TX2S4/events/06EZRE8KMBZ54RSN9GV1C3V2E8.json persists relation 06EZ0NV0Y81AE1Z1Q3223TX2S4--06EZ0NV7KG94MTMNXMGVRYVW9C--blocks, so sequencing is explicit in repository state.
- src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs currently exposes only Hubs, Links, and Satellites; src/DCoding.Data.DVault/Modeling/DataVaultModel.cs defines DataVaultTableKind with only Hub, Link, and Satellite; src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs defines no bridge-specific logical property kind today.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs CreateEntities iterates only metadataModel.Hubs, metadataModel.Links, and metadataModel.Satellites, confirming the repository is still on the hub/link/satellite-only translator baseline that this ticket describes as additive.
- tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs asserts only hub/link/satellite entities plus AssertNoRelationships, and tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs currently expects only HubCustomer|HubOrder|LinkCustomerOrder|SatCustomerContact|SatCustomerOrderState in SQLite schema output.
- git show 25867e83510b shows the latest substantive handoff on this branch changed only .gicket ticket files for 06EZ0NV7KG94MTMNXMGVRYVW9C, while git show 6fa6984af9a1218feceadd124bb53ea216cb4c2e shows the authoritative bridge contract doc and sibling attachment manifest were added in branch history.

Blocking findings
- none

Required PO actions
- none

Open issues ledger
- none

Missing examples / edge cases
- No concrete translator-time worked example is pinned for one otherwise-valid unsupported bridge shape such as effectivity-window or path-payload metadata; the boundary is defined in prose, not by a named example.

Risky assumptions
- This ticket assumes the sibling implementation will land DataVaultTableKind.Bridge, a bridge collection on DataVaultMetadataModel, and a distinct hierarchy-depth semantic before work starts here, because current source does not contain them yet.
- This approval assumes execution ordering continues to honor persisted relation 06EZ0NV0Y81AE1Z1Q3223TX2S4--06EZ0NV7KG94MTMNXMGVRYVW9C--blocks so the mapping ticket is not started before its prerequisite API exists.

AC / test suggestions
- Add one explicit unit assertion that TraversalDepth uses the sibling-defined bridge-depth semantic and integer CLR/provider mapping, rather than reusing Payload/PayloadText semantics.
- Keep explicit no-foreign-key, no-navigation assertions for both bridge worked examples in unit coverage and SQLite schema coverage, not only for existing hub/link/satellite entities.
- Add one bounded not-supported test that uses otherwise-valid bridge metadata outside the baseline so the translator-time failure boundary is proven by example, not only by prose.

Implementation watchouts
- Do not overload existing satellite payload semantics for TraversalDepth; the sibling contract requires a distinct bridge-depth meaning.
- Preserve declared endpoint order exactly, because column order drives deterministic primary-key and index names in both worked examples.
- Keep bridge translation shared-type and additive only; no EF relationships, save-path changes, migrations, or provider-specific bridge logic belong in this slice.

Non-blocking notes
- The legacy draft at the bottom of .gicket/tickets/06EZ0NV7KG94MTMNXMGVRYVW9C/description.md still mentions effective/load timestamp bridge fields, but the authoritative contract block above it supersedes that older text and scopes those families out.
- Current branch HEAD db959160d4f3d113f462d06acf15f182c672da13 is only the po-critic claim commit; the substantive ticket-spec changes relevant to this review are the earlier handoff/refinement commits 25867e83510b and 6fa6984af9a1218feceadd124bb53ea216cb4c2e.

Split recommendations
- No additional split is needed for this ticket now; keep richer bridge families such as effectivity windows, path payload columns, closure maintenance, query helpers, and navigation graph generation as separate follow-up tickets.

Policy outcome
- Ticket is approved for developer handoff. Non-blocking notes stay visible for downstream roles.
- Label plan: added [needs-dev]; removed [blocked/dev, blocked/test, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment