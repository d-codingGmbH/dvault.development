[gicket-bot] PO-critic review contract

Summary
- The parent contract is substantively refined and locally evidenced, but the implementation split is not yet propagated into the child tickets the contract says must carry this boundary.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- `docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` exists on branch `ticket/06F0ME976PM5455JK04S6GPNNW-task-design-fluent-hub-satellite-and-link-api-co`; `git log --oneline -- docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` shows commit `7ac040ab3` added the design note.
- `.gicket/tickets/06F0ME976PM5455JK04S6GPNNW/description.md` contains `## Open Questions` followed by `- none`, so the persisted parent contract has no unresolved open questions.
- The same parent description also states in `## Definition of Done` that `Child implementation tickets continue to reference this contract as their boundary`.
- `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` directly exposes the existing EF entry points `UseDataVault(...)` and `ApplyDataVaultMetadata(..., DataVaultMetadataModel, ...)`, which supports the parent contract's additive-API compatibility claim.
- `src/DCoding.Data.DVault/Modeling/DataVaultModel.cs` and `src/DCoding.Data.DVault/Modeling/DataVaultModelBuilder.cs` show the existing `DCoding.Data.DVault.Modeling.DataVaultModelBuilder` surface remains present.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` shows `DataVaultLinkMetadata` rejects fewer than two hub endpoints and `DataVaultSatelliteMetadata` already has a driving-key constructor and a hub-or-link `Parent`, matching the compatibility and multi-active boundary described by the contract.
- Relation files `.gicket/relations/NW/A8/...--blocks.json`, `.gicket/relations/NW/3W/...--blocks.json`, and `.gicket/relations/NW/8G/...--blocks.json` show the parent ticket blocks all three implementation children; `.gicket/relations/1R/*/...--parentOf.json` shows the common parent story relation is already in place.
- `git diff --name-status develop..HEAD -- .gicket/tickets/06F0ME976PM5455JK04S6GPNNW docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md .gicket/tickets/06F0ME9PM8KXH3VP59TQR0ETA8 .gicket/tickets/06F0MEA1FF743S14XQW02H4A3W .gicket/tickets/06F0MEAD1BAA5QEVM3F9QJA38G .gicket/relations` shows this branch changed only the parent ticket artifacts and added the contract note; it did not update any child ticket files.
- `rg -n "06F0ME976PM5455JK04S6GPNNW|06F0ME8NFJX6CD20MEA10J761R|fluent-code-first-api-contract" .gicket/tickets/06F0ME9PM8KXH3VP59TQR0ETA8 .gicket/tickets/06F0MEA1FF743S14XQW02H4A3W .gicket/tickets/06F0MEAD1BAA5QEVM3F9QJA38G -S` returned `rg exit=1`, so the child ticket content does not currently reference the parent contract or plan by id/path.
- `.gicket/tickets/06F0ME9PM8KXH3VP59TQR0ETA8/description.md` covers hub business keys and satellite payloads only; `rg -n "DrivingKey|multi-active"` across the three child ticket directories returned no matches, so the child split does not currently carry the parent contract's explicit `DrivingKey(...)` / multi-active boundary.

Blocking findings
- The persisted parent Definition of Done requires the child implementation tickets to reference this contract as their boundary, but the current child ticket content does not reference the parent ticket id, parent story id, or `docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` at all.
- The hub/satellite child ticket does not currently carry the parent contract's reserved `DrivingKey(...)` / multi-active opt-in scope, and the parity child also does not mention validating that covered multi-active shape. That leaves part of the approved parent boundary unassigned in the implementation split.

Required PO actions
- Update the three child implementation tickets so they explicitly reference ticket `06F0ME976PM5455JK04S6GPNNW` and/or `docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` as the authoritative boundary.
- Amend child scope/acceptance text so the hub/satellite implementation ticket explicitly includes `DrivingKey(...)` multi-active opt-in selector capture and validation, and the parity ticket explicitly includes parity coverage for that covered shape.

Open issues ledger
- critic-item-1 [required-po-action] Update the three child implementation tickets so they explicitly reference ticket `06F0ME976PM5455JK04S6GPNNW` and/or `docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` as the authoritative boundary.
- critic-item-2 [required-po-action] Amend child scope/acceptance text so the hub/satellite implementation ticket explicitly includes `DrivingKey(...)` multi-active opt-in selector capture and validation, and the parity ticket explicitly includes parity coverage for that covered shape.
- critic-item-3 [blocking-finding] The persisted parent Definition of Done requires the child implementation tickets to reference this contract as their boundary, but the current child ticket content does not reference the parent ticket id, parent story id, or `docs/plans/06F0ME976PM5455JK04S6GPNNW-fluent-code-first-api-contract.md` at all.
- critic-item-4 [blocking-finding] The hub/satellite child ticket does not currently carry the parent contract's reserved `DrivingKey(...)` / multi-active opt-in scope, and the parity child also does not mention validating that covered multi-active shape. That leaves part of the approved parent boundary unassigned in the implementation split.

Missing examples / edge cases
- No child-ticket-level coverage currently calls out repeated `DrivingKey(...)` ordering with more than one driving key.
- No child-ticket-level coverage currently calls out the ambiguous-participant case for links when a CLR type could resolve to more than one hub declaration.
- No child-ticket-level coverage currently calls out duplicate selector rejection by ordinal member name across business keys, payloads, and driving keys.

Risky assumptions
- Assuming relation files alone satisfy the parent DoD's requirement that child tickets `reference this contract as their boundary`.
- Assuming developers will infer multi-active `DrivingKey(...)` scope from the parent design note even though the assigned child ticket text does not currently mention it.
- Assuming parity coverage will automatically include all covered Code-First shapes without the parity ticket naming those shapes explicitly.

AC / test suggestions
- Child acceptance text should explicitly require parity for ordinary satellites, multi-active opt-in via `DrivingKey(...)`, explicit link names, and derived default link names.
- Child validation acceptance text should explicitly require actionable failures for anonymous-object, method-call, constant, collection-navigation, and nested-navigation selectors, matching the parent contract.
- Parity coverage should name canonical-order assertions for repeated business keys, repeated payloads, repeated driving keys, and link participant order.

Implementation watchouts
- Keep the new Code-First builder family additive in `DCoding.Data.DVault`; do not blur it with the existing `DCoding.Data.DVault.Modeling` builder surface the parent contract preserves.
- Keep the fluent path projecting through `DataVaultMetadataModel` and the existing provider-aware EF translator path, because that compatibility point is directly supported by current source.
- Do not let implementation or child ticket wording imply `SaveChanges` interception or link-parent satellite support in this v1 slice.

Non-blocking notes
- The parent contract itself is well grounded: the durable design note exists, the parent description has `Open Questions: none`, and the current source does support the cited metadata-first compatibility anchors.
- Relation state is already consistent with the intended split; the issue is boundary propagation into the child tickets, not relation cleanup or the need for a new split.

Split recommendations
- No additional split is needed. Keep the existing three-child decomposition, but return this ticket to PO until the child tickets are updated to carry the parent contract boundary explicitly.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment