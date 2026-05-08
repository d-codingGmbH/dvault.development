[gicket-bot] PO-critic review contract

Summary
- Docs, bridge public surface, and child-ticket integrations are present, but hierarchy validation still allows unsupported recursive-link shapes, so the parent story is not ready for developer handoff.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `.gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/description.md` has `## Open Questions` with `- none`, so the review is not blocked by unresolved contract questions.
- `git -C /mnt/c/Projects/DVault log --oneline --grep "06EZ0NTV4SVAKV98C418T8A3CC\|06EZ0NV0Y81AE1Z1Q3223TX2S4\|06EZ0NV7KG94MTMNXMGVRYVW9C\|06EZ0NVE88WW9PMM04NVAZHRG0\|06F03T9R8QK81VQCC158NJ62YG" --all` showed child integrations `1f05cc3f6` (metadata), `39f928442` (translator), `6678b14be` (docs), and `48b4d9705` (docs reconciliation); `git -C /mnt/c/Projects/DVault diff --name-only develop...HEAD` listed only `.gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/**`, so current repository source/docs are inherited from `develop`.
- Parent AC in `.gicket/tickets/06EZ0NTV4SVAKV98C418T8A3CC/description.md:32` requires hierarchy bridges over `one recursive self-link` with explicit ancestor/descendant roles, and the contract in `docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md:32` assigns validation of `source link is not a two-participant self-link over one hub type` to ticket `06EZ0NV0Y81AE1Z1Q3223TX2S4`.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadataModel.cs:485-493` only checks that the selected hub appears `at least twice` in the link and has no check for `link.Participants.Count == 2` or for every participant being the same hub type.
- `src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs:319-345` projects hierarchy bridges from `bridge.Endpoints` alone and does not re-check source-link shape, so an over-broadly accepted hierarchy declaration would still translate.
- `rg -n "two-participant self-link|self-link over one hub type|at least twice|extra participant|mixed hub|three participant" tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs` returned no matches; the observed bridge validation tests in `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs:624-723` cover unknown references, ambiguous endpoint selection, and self-cycle, but no explicit negative case for a mixed-hub or extra-participant recursive link.
- Docs are not the blocking gap: `README.md:206` and `docs/plans/deferred-data-vault-capabilities.md:105-139` now describe the implemented bridge baseline, and `.gicket/tickets/06F03T9R8QK81VQCC158NJ62YG/comments/06F0CXRWXFYJVRBYC99T32YF2M.md` records tester verification `4/4` acceptance criteria and `3/3` definition-of-done for the docs-reconciliation follow-up.

Blocking findings
- Current hierarchy validation is broader than the parent story and bridge-contract boundary. `ValidateHierarchyBridge` accepts any link where the chosen hub appears at least twice, but the contract requires rejection when the source link is not a two-participant self-link over one hub type.
- Because hierarchy translation assumes prior validation and no direct negative test was found for mixed-hub or extra-participant recursive links, the parent story does not yet have source-backed proof that unsupported hierarchy shapes are excluded from the implemented baseline.

Required PO actions
- Reopen `06EZ0NV0Y81AE1Z1Q3223TX2S4` or create one narrow follow-up child that explicitly covers hierarchy source-link-shape validation for `exactly two participants` and `one hub type`, with matching negative tests.
- Update the parent story contract/handoff comments so the remaining gap is tracked against that metadata-validation child before this parent returns to closure flow.

Open issues ledger
- critic-item-1 [required-po-action] Reopen `06EZ0NV0Y81AE1Z1Q3223TX2S4` or create one narrow follow-up child that explicitly covers hierarchy source-link-shape validation for `exactly two participants` and `one hub type`, with matching negative tests.
- critic-item-2 [required-po-action] Update the parent story contract/handoff comments so the remaining gap is tracked against that metadata-validation child before this parent returns to closure flow.
- critic-item-3 [blocking-finding] Current hierarchy validation is broader than the parent story and bridge-contract boundary. `ValidateHierarchyBridge` accepts any link where the chosen hub appears at least twice, but the contract requires rejection when the source link is not a two-participant self-link over one hub type.
- critic-item-4 [blocking-finding] Because hierarchy translation assumes prior validation and no direct negative test was found for mixed-hub or extra-participant recursive links, the parent story does not yet have source-backed proof that unsupported hierarchy shapes are excluded from the implemented baseline.

Missing examples / edge cases
- A hierarchy bridge over a recursive link that has three participants of the same hub type.
- A hierarchy bridge over a link that includes the recursive hub twice plus an unrelated third hub type.
- A negative test/example proving those shapes are rejected before translation.

Risky assumptions
- Assuming prior child tester handoffs fully closed the parent contract is unsafe; the current source still broadens hierarchy validation beyond the documented two-participant self-link boundary.
- Assuming the parent risk about incoming blockers still affects sequencing is outdated unless the relation text is refreshed, because both referenced blocker stories are already `done`.

AC / test suggestions
- Add an auditable acceptance/test case that rejects a hierarchy bridge whose source link is `Employee, Employee, Department` even when ancestor and descendant ordinals are distinct.
- Add an auditable acceptance/test case that rejects a hierarchy bridge whose source link is `Employee, Employee, Employee` with three participants rather than the contracted two-participant self-link.

Implementation watchouts
- Keep this scoped to metadata validation ownership in `06EZ0NV0Y81AE1Z1Q3223TX2S4`; the bridge contract assigns this boundary to the modeling/validation child, not the translator child.
- A translator-only fix is insufficient because `CreateHierarchyBridgeEntity` currently trusts bridge endpoints and does not inspect the source link.

Non-blocking notes
- The bridge public surface and provider-neutral projection baseline are present in source: `DataVaultBridgeMetadata`, `DataVaultMetadataModel.Bridges`, `DataVaultTableKind.Bridge`, `DataVaultPropertyRole.BridgeDepth`, and bridge schema snapshots/tests are all directly observable in `src/DCoding.Data.DVault` and `tests/DCoding.Data.DVault.Tests`.

Split recommendations
- No broader re-split is needed. Use one narrow metadata-validation reopen/follow-up under the existing parent, rather than creating a new translator or docs child.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment