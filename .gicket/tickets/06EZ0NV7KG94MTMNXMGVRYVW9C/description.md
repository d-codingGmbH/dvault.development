<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Current-ticket clarification is resolved by ratifying the already attached Bridge Metadata V1 Contract as the authoritative sibling input, keeping the live blocks dependency, and fixing one many-to-many plus one hierarchy worked example for provider-neutral EF mapping. No new relation, attachment, or planning write was needed in this pass.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket remains the provider-neutral EF projection sibling under bridge story 06EZ0NTV4SVAKV98C418T8A3CC beside metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 and documentation ticket 06EZ0NVE88WW9PMM04NVAZHRG0.
- The authoritative metadata and public API input for this ticket is the existing plan document docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, already attached to 06EZ0NV0Y81AE1Z1Q3223TX2S4 as attachment 06EZSK9Q43V2J6P9SQVTRY3W3R.
- Live sequencing is already persisted by 06EZ0NV0Y81AE1Z1Q3223TX2S4 blocks 06EZ0NV7KG94MTMNXMGVRYVW9C.
- The baseline many-to-many example for this ticket is BridgeCustomerOrder with ordered columns CustomerHashKey then OrderHashKey, primary key PkBridgeCustomerOrderCustomerHashKeyOrderHashKey, and index IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey.
- The baseline hierarchy example for this ticket is BridgeSalesRegionHierarchy with ordered columns AncestorSalesRegionHashKey, DescendantSalesRegionHashKey, TraversalDepth, primary key PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey, and traversal indexes IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth and IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey.
- Validation ownership stays with sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4, while this ticket owns only provider-neutral EF projection and translation-boundary not-supported diagnostics.
- No new child tickets, attachments, planning documents, or relation writes were created in this pass because the needed contract document and blocks relation already exist.

### Scope In
- Consume sibling bridge declarations for baseline many-to-many and hierarchy bridges and translate them into provider-neutral EF shared-type entities through ApplyDataVaultMetadata.
- Project deterministic bridge table names, ordered columns, primary keys, secondary indexes, ProducedName, EntityKind, MetadataName annotations, participant-reference metadata, and provider-profile storage annotations without regressing hub, link, or satellite behavior.
- Add provider-capability handling for the new integer hierarchy-depth logical property kind required by the sibling contract.
- Add unit and SQLite schema coverage that locks the CustomerOrder and SalesRegionHierarchy examples plus bounded translator-time not-supported diagnostics.

### Scope Out
- Defining bridge metadata model types, endpoint-binding validation rules, naming inputs, or public API ownership; those stay with 06EZ0NV0Y81AE1Z1Q3223TX2S4.
- Bridge documentation and end-user examples owned by 06EZ0NVE88WW9PMM04NVAZHRG0.
- Save-path changes, DataVaultSaveRequest or IDataVaultSaveService changes, runtime loading, migrations, schema-refresh automation, or provider-specific DDL or SQL behavior.
- Advanced bridge capabilities such as effectivity windows, path payload columns, closure maintenance, consumer-specific query models, or EF relationship graphs.

## Acceptance Criteria
- ApplyDataVaultMetadata can project sibling-defined many-to-many and hierarchy bridge metadata into shared-type EF entities with no implicit foreign keys or navigations and without regressing existing hub, link, or satellite outputs.
- Many-to-many example CustomerOrder projects entity BridgeCustomerOrder, columns CustomerHashKey then OrderHashKey, primary key PkBridgeCustomerOrderCustomerHashKeyOrderHashKey, secondary index IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey, EntityKind Bridge, MetadataName CustomerOrder, and participant-reference property annotations with ProducedName equal to each column name.
- Hierarchy example SalesRegionHierarchy projects entity BridgeSalesRegionHierarchy, columns AncestorSalesRegionHashKey, DescendantSalesRegionHashKey, TraversalDepth, primary key PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey, secondary indexes IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth and IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey, and uses a distinct integer bridge-depth logical property kind or annotation for TraversalDepth.
- Translator-time failures are limited to otherwise valid bridge metadata outside the bounded provider-neutral projection baseline; missing references, wrong reference kinds, malformed endpoint bindings, ambiguous recursive roles, and cycle rules remain sibling-ticket validation concerns.
- Unit and SQLite baseline tests lock the exact bridge outputs, annotations, column order, key and index names, and no-relationship posture beside the existing translation and schema test suites.

## Definition of Done
- The current ticket contract explicitly references docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md as authoritative sibling input and preserves the live blocks relation from 06EZ0NV0Y81AE1Z1Q3223TX2S4 until the dependency is actually resolved.
- Translator changes remain additive to the existing shared-type bridge-less baseline in DataVaultEfMetadataTranslator, DataVaultAnnotationNames, and DataVaultProviderCapabilities.
- DataVaultEfMetadataTranslationTests and SqliteDataVaultSchemaTests cover both bridge worked examples and translation-boundary not-supported diagnostics without regressing existing assertions.
- No save-path behavior, provider-specific bridge logic, migrations, EF relationship graph generation, or advanced bridge capability expansion is introduced.

## Implementation Notes
- The authoritative input is the existing bridge contract document at docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, already attached to sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 as attachment 06EZSK9Q43V2J6P9SQVTRY3W3R.
- Current repo evidence shows DataVaultEfMetadataTranslator creates only hub, link, and satellite entities today, while DataVaultAnnotationNames and DataVaultProviderCapabilities have no bridge-specific entity kind or logical property kind coverage yet; bridge work here is therefore additive mapping only.
- Preserve the current shared-type and no-relationship posture proven by tests/DCoding.Data.DVault.Tests/Unit/DataVaultEfMetadataTranslationTests.cs and tests/DCoding.Data.DVault.Tests/Integration/SqliteDataVaultSchemaTests.cs.
- Many-to-many bridges project only endpoint hash-key columns; hierarchy bridges add only TraversalDepth and must not introduce new load timestamp, record source, or hash diff families.
- Validation ownership stays with sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4; this ticket owns only provider-neutral EF projection, produced-name annotations, provider capability mappings, and translation-boundary not-supported diagnostics.
- No new child tickets, attachments, planning documents, or relation changes were materialized in this pass.

## Open Questions
- none

## Follow-Up Questions
- After baseline bridge translation lands, should effectivity-window, path-payload, or closure-maintenance bridge variants become separate follow-up tickets?
- If downstream consumers later need EF foreign keys or navigations for bridge entities, should that remain a separate capability ticket instead of expanding the shared-type baseline?
- After provider-neutral bridge mapping lands, do any provider packages need separate optimization tickets for bridge-specific storage or indexing beyond the common baseline?

## Risks
- If sibling implementation or later ticket text diverges from docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, the mapping and documentation siblings may target different bridge contracts.
- If the new hierarchy-depth property is modeled by reusing existing payload or technical semantics instead of a distinct logical kind or annotation, provider capability mappings and tests will become ambiguous.
- If endpoint declaration order is not preserved exactly, produced column order and deterministic key and index names will drift from the ratified examples.
- If this ticket introduces EF relationships or provider-specific physical behavior, the existing shared-type and SQLite baselines may regress.

## Split Recommendations
- No further split is recommended now; the existing bridge story plus metadata, mapping, and documentation siblings remain the right decomposition.
- Treat richer bridge capability families such as effectivity windows, closure maintenance, query-model helpers, or navigation graph generation as separate follow-up tickets rather than broadening this provider-neutral mapping slice.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: generate provider-neutral EF Core mapping for baseline bridge tables.

Acceptance Criteria:
- Generated mapping covers table name, key columns, traversal references, and effective/load timestamp fields where applicable.
- Tests verify the mapping through the local SQLite baseline.
- Unsupported bridge shapes fail with clear validation messages.