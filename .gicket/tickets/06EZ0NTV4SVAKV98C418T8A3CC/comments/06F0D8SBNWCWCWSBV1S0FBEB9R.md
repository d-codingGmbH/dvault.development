[gicket-bot] PO refinement contract

Summary
- Refinement ratified the existing bridge-table v1 split and repository-backed contract; no new child tickets, relation updates, or planning documents were needed.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository-backed v1 scope is already fixed by docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md and current source: bridge support is opt-in, additive, and limited to many-to-many plus hierarchy bridge metadata over existing hubs and links.
- Provider-neutral EF projection is the bounded implementation target: bridge tables are shared-type entities with deterministic produced names, ordered endpoint hash-key columns, bridge-specific keys/indexes, annotations, and no implicit EF foreign keys, navigations, save-service changes, migrations, or provider-specific DDL behavior in v1.
- Existing parentOf split is already materialized on the parent ticket: 06EZ0NV0Y81AE1Z1Q3223TX2S4 owns bridge metadata and validation, 06EZ0NV7KG94MTMNXMGVRYVW9C owns translator/generation behavior, 06EZ0NVE88WW9PMM04NVAZHRG0 is the historical docs child, and done follow-up 06F03T9R8QK81VQCC158NJ62YG is the documentation-reconciliation gate before story closure.
- No relation cleanup, new child-ticket creation, or planning-document write was required in this PO refinement pass.

Scope In
- Opt-in bridge metadata declarations in the modeling surface and DataVaultMetadataModel bridge collection.
- Baseline bridge kinds limited to many-to-many traversal and hierarchy traversal over existing hubs and links.
- Provider-neutral EF metadata generation for bridge shared-type entities, including deterministic table/column/key/index naming and bridge/property annotations.
- Validation for missing or wrong references, invalid endpoint bindings, incorrect many-to-many link membership, invalid recursive hierarchy shapes, and other unsupported bounded bridge definitions.
- Tests and documentation that prove and explain the supported baseline, minimal examples, and explicit v1 limitations.

Scope Out
- Bridge row population, traversal maintenance, closure-state management, or any runtime orchestration beyond EF metadata projection.
- EF relationship graph generation, implicit foreign keys or navigations, save-service changes, migrations, or provider-specific DDL/optimization behavior.
- Effectivity windows, path payload columns, advanced hierarchy semantics, complex traversal variants, and PIT or multi-active interactions.
- Any default-on behavior that changes existing link semantics when no bridge metadata is declared.

Open questions
- none

Follow-up questions
- When bridge projection baseline is complete, should a later ticket define how bridge rows are populated and maintained at runtime, or should bridge consumers be expected to manage data population externally in the near term?
- Which deferred bridge expansions, if any, should be prioritized next: provider-specific physical optimization, effectivity/path payload support, advanced hierarchy semantics, or PIT/multi-active interactions?

Risks
- The parent ticket still has incoming blocks relations from 06EZ0NSBM3GD7DY11Y4PZMXD28 and 06EZ0NWKC9ZME5BSCJFSQEQ02R, so delivery sequencing may remain constrained even though PO refinement is ready.
- If documentation drifts back toward a deferred-only description, consumers may misread the implemented baseline or assume unsupported runtime bridge behavior that this story does not deliver.

Split recommendations
- No additional split is recommended: the parent story already has the required child decomposition and live parentOf relations for metadata/validation, translator/generation, documentation, and post-critic docs reconciliation.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 4

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment