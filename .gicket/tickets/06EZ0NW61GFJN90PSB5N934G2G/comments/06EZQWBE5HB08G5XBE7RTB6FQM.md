[gicket-bot] PO-critic review contract

Summary
- Return to PO: this persistence task is internally refined, but it still depends on unresolved ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W and the repo has no driving-key or multi-active opt-in public surface yet.
- decision: `return_to_po`
- meaning: ticket must return to Product Owner refinement
- Assurance level: `low` (`assurance/low`)

Evidence
- .gicket/tickets/06EZ0NW61GFJN90PSB5N934G2G/description.md says this task "implements persistence against the driving-key contract from ticket 06EZ0NVX3RYPTFZKYCYEH9HB8W" and its Open Questions section is "- none".
- .gicket/tickets/06EZ0NVX3RYPTFZKYCYEH9HB8W/description.md only has three generic bullets; a repo check returned NO_REFINEMENT_CONTRACT_OR_HANDOFF_MARKERS and there is no sibling comments directory.
- tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt exposes DataVaultSatelliteBuilder.Payload(string), DataVaultSatelliteMetadata(string name, DataVaultMetadataReference parent, IEnumerable<string> descriptiveAttributeNames), and DataVaultSatelliteSaveOperation(..., parentHashKey, payloadValues, hashDiff); no driving-key or multi-active opt-in member is present.
- src/DCoding.Data.DVault/DataVaultEfMetadataTranslator.cs creates satellite primary keys and indexes on [parentHashKeyColumnName, loadTimestampColumnName], and tests/DCoding.Data.DVault.Tests/Integration/Snapshots/SqliteDataVaultSchemaSnapshot.txt shows PkSatCustomerContactCustomerHashKeyLoadTimestamp and IxSatCustomerContactSatelliteParentCustomerHashKeyLoadTimestamp.
- src/DCoding.Data.DVault/DataVaultSaveService.cs tracks latest satellite hash diffs in Dictionary<string, LatestSatelliteHashDiff> keyed only by ParentHashKey, confirming current duplicate suppression is parent-only.
- src/DCoding.Data.DVault.Postgres/PostgresDataVaultSaveStrategy.cs CanSave(...) checks only provider name and pending tracked changes, so current optimized strategies do not yet decline based on multi-active request shape.

Blocking findings
- Ticket 06EZ0NW61GFJN90PSB5N934G2G explicitly depends on the driving-key contract from 06EZ0NVX3RYPTFZKYCYEH9HB8W, but that sibling ticket is still needs-po and does not contain a handoff-ready contract. Approving this task would force developers to define the contract this ticket says it will not redefine.
- The current repo has no public modeling or save-request surface for driving keys or multi-active opt-in. Because the approved public API snapshot only shows parentHashKey, payloadValues, and hashDiff for satellite saves, the task cannot be developed without inventing caller-visible behavior outside its stated scope.

Required PO actions
- Refine 06EZ0NVX3RYPTFZKYCYEH9HB8W first and make it handoff-ready with the exact opt-in declaration, save-request/value-passage shape, validation rules, and deterministic ordering rules for multi-column driving-key sets.
- Update 06EZ0NW61GFJN90PSB5N934G2G to cite that finalized contract explicitly so "opt-in multi-active satellite" and "driving-key value set" are anchored to a concrete source of truth.

Open issues ledger
- critic-item-1 [required-po-action] Refine 06EZ0NVX3RYPTFZKYCYEH9HB8W first and make it handoff-ready with the exact opt-in declaration, save-request/value-passage shape, validation rules, and deterministic ordering rules for multi-column driving-key sets.
- critic-item-2 [required-po-action] Update 06EZ0NW61GFJN90PSB5N934G2G to cite that finalized contract explicitly so "opt-in multi-active satellite" and "driving-key value set" are anchored to a concrete source of truth.
- critic-item-3 [blocking-finding] Ticket 06EZ0NW61GFJN90PSB5N934G2G explicitly depends on the driving-key contract from 06EZ0NVX3RYPTFZKYCYEH9HB8W, but that sibling ticket is still needs-po and does not contain a handoff-ready contract. Approving this task would force developers to define the contract this ticket says it will not redefine.
- critic-item-4 [blocking-finding] The current repo has no public modeling or save-request surface for driving keys or multi-active opt-in. Because the approved public API snapshot only shows parentHashKey, payloadValues, and hashDiff for satellite saves, the task cannot be developed without inventing caller-visible behavior outside its stated scope.

Missing examples / edge cases
- There is no concrete example of a multi-active satellite with more than one driving-key field, even though the ticket repeatedly refers to a driving-key value set.
- There is no example of how a caller opts a satellite into multi-active behavior at modeling time and then supplies driving-key values separately from payload and hash diff at save time.
- There is no ticket-level example of what observable outcome proves provider-specific decline and provider-neutral fallback.

Risky assumptions
- This ticket assumes the sibling contract can land without expanding public surfaces already snapshotted in DCoding.Data.DVault.approved.txt; current repo evidence suggests builder, metadata, and save-operation changes are likely.
- This ticket assumes optimized provider strategies can safely detect and decline multi-active batches once the contract exists; current CanSave implementations do not inspect request shape.
- This ticket assumes later save implies a later load timestamp; the follow-up question already notes same-series same-timestamp changed-row conflict behavior remains undefined.

AC / test suggestions
- Add one acceptance example with a two-column driving-key set and expected persisted column order/content so deterministic value-set behavior is directly testable.
- Add an acceptance statement for how provider-specific strategies prove decline/fallback while preserving RowsWritten and saved-record ordering.
- Tie this ticket to the exact sibling contract revision or public API snapshot expectation that authorizes the required surface changes.

Implementation watchouts
- Current satellite schema, provider-neutral save logic, and optimized strategies are all parent-only today; once the dependency is defined, every path must either partition by parent-plus-driving-key or decline.
- Because DataVaultSatelliteSaveOperation currently carries only parentHashKey, payloadValues, and hashDiff, the eventual contract is likely caller-visible and not just an internal persistence detail.
- There is no branch-local clarification to lean on because the ticket branch currently matches scratch-source-ref 2eca5eda0de4e7cd2e9982a3a7c75a2ad3913ca9.

Non-blocking notes
- The current ticket itself is otherwise well structured: its persisted contract has explicit scope in/out, 5 acceptance criteria, 4 definition-of-done items, and Open Questions set to none.
- The PO refinement comment 06EZQSXFPAP60Z75WFVMZ1TGX8.md already captured the optimized-strategy risk; the main gap is dependency sequencing, not missing risk awareness.
- The scoped-out docs/tests sibling 06EZ0NWCA6NEZH8VBJNGW4FVHG is still needs-po, but that is not the primary blocker for this ticket.

Split recommendations
- No new split is needed; enforce sequencing so 06EZ0NVX3RYPTFZKYCYEH9HB8W is refined and handed off before this persistence task returns to PO-critic.

Policy outcome
- Blocking gaps were found. Escalation to refinement is required.
- Label plan: added [blocked/dev, blocked/test, needs-po, automation/bot-ready]; removed [automation/bot-ready, critic-needed].
- Assignee plan: no assignee changes.
- Status plan: keep status unchanged.

Run mode
- apply: planned updates are applied after this comment