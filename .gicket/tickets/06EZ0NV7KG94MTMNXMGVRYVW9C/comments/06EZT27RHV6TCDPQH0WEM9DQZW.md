[gicket-bot] PO refinement contract

Summary
- Current-ticket clarification is resolved by ratifying the already attached Bridge Metadata V1 Contract as the authoritative sibling input, keeping the live blocks dependency, and fixing one many-to-many plus one hierarchy worked example for provider-neutral EF mapping. No new relation, attachment, or planning write was needed in this pass.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarification resolution
- resolution-decision: `resolved`
- critic-item-1: `answered` - Treat docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, already attached to sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4, as the durable authoritative bridge metadata and public API contract for this mapping ticket. It fixes the bridge declaration shapes, requires bridge modeling types plus DataVaultTableKind.Bridge and a bridge-depth logical property kind, and assigns missing-reference, wrong-kind, ambiguous-binding, and cycle validation to the sibling ticket.
- critic-item-2: `answered` - Sequencing is already persisted by live relation 06EZ0NV0Y81AE1Z1Q3223TX2S4--06EZ0NV7KG94MTMNXMGVRYVW9C--blocks, so this mapping ticket remains downstream of the sibling metadata work and no additional relation write is needed in this pass.
- critic-item-3: `answered` - The supported worked examples are now fixed to CustomerOrder many-to-many and SalesRegionHierarchy hierarchy. Their expected entity names, ordered columns, primary keys, secondary indexes, annotations, and failure ownership come directly from the bridge metadata v1 contract and should be copied into the current ticket contract and tests.
- critic-item-4: `answered` - The repository still exposes only hub, link, and satellite translation baselines today, so developers must not invent bridge metadata or public API inside this ticket. This ticket only consumes the sibling contract and adds provider-neutral EF shared-type projection for those validated bridge shapes.
- critic-item-5: `answered` - The dependency is no longer implicit: the existing blocks relation already expresses that 06EZ0NV0Y81AE1Z1Q3223TX2S4 must land before 06EZ0NV7KG94MTMNXMGVRYVW9C. The current contract should explicitly ratify that live relation rather than describing sequencing as missing.
- critic-item-6: `answered` - Concrete mapping expectations now include exact BridgeCustomerOrder and BridgeSalesRegionHierarchy projections plus a clear ownership split: sibling validation handles missing or wrong references, malformed endpoint bindings, ambiguous recursive roles, and cycle rules; translator-time failures are limited to otherwise valid metadata outside the bounded projection baseline such as effectivity windows, path payload columns, closure maintenance state, or EF relationship graph generation.

Clarifications
- This ticket remains the provider-neutral EF projection sibling under bridge story 06EZ0NTV4SVAKV98C418T8A3CC beside metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 and documentation ticket 06EZ0NVE88WW9PMM04NVAZHRG0.
- The authoritative metadata and public API input for this ticket is the existing plan document docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, already attached to 06EZ0NV0Y81AE1Z1Q3223TX2S4 as attachment 06EZSK9Q43V2J6P9SQVTRY3W3R.
- Live sequencing is already persisted by 06EZ0NV0Y81AE1Z1Q3223TX2S4 blocks 06EZ0NV7KG94MTMNXMGVRYVW9C.
- The baseline many-to-many example for this ticket is BridgeCustomerOrder with ordered columns CustomerHashKey then OrderHashKey, primary key PkBridgeCustomerOrderCustomerHashKeyOrderHashKey, and index IxBridgeCustomerOrderTraversalOrderHashKeyCustomerHashKey.
- The baseline hierarchy example for this ticket is BridgeSalesRegionHierarchy with ordered columns AncestorSalesRegionHashKey, DescendantSalesRegionHashKey, TraversalDepth, primary key PkBridgeSalesRegionHierarchyAncestorSalesRegionHashKeyDescendantSalesRegionHashKey, and traversal indexes IxBridgeSalesRegionHierarchyTraversalAncestorSalesRegionHashKeyTraversalDepth and IxBridgeSalesRegionHierarchyTraversalDescendantSalesRegionHashKeyAncestorSalesRegionHashKey.
- Validation ownership stays with sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4, while this ticket owns only provider-neutral EF projection and translation-boundary not-supported diagnostics.
- No new child tickets, attachments, planning documents, or relation writes were created in this pass because the needed contract document and blocks relation already exist.

Scope In
- Consume sibling bridge declarations for baseline many-to-many and hierarchy bridges and translate them into provider-neutral EF shared-type entities through ApplyDataVaultMetadata.
- Project deterministic bridge table names, ordered columns, primary keys, secondary indexes, ProducedName, EntityKind, MetadataName annotations, participant-reference metadata, and provider-profile storage annotations without regressing hub, link, or satellite behavior.
- Add provider-capability handling for the new integer hierarchy-depth logical property kind required by the sibling contract.
- Add unit and SQLite schema coverage that locks the CustomerOrder and SalesRegionHierarchy examples plus bounded translator-time not-supported diagnostics.

Scope Out
- Defining bridge metadata model types, endpoint-binding validation rules, naming inputs, or public API ownership; those stay with 06EZ0NV0Y81AE1Z1Q3223TX2S4.
- Bridge documentation and end-user examples owned by 06EZ0NVE88WW9PMM04NVAZHRG0.
- Save-path changes, DataVaultSaveRequest or IDataVaultSaveService changes, runtime loading, migrations, schema-refresh automation, or provider-specific DDL or SQL behavior.
- Advanced bridge capabilities such as effectivity windows, path payload columns, closure maintenance, consumer-specific query models, or EF relationship graphs.

Open questions
- none

Follow-up questions
- After baseline bridge translation lands, should effectivity-window, path-payload, or closure-maintenance bridge variants become separate follow-up tickets?
- If downstream consumers later need EF foreign keys or navigations for bridge entities, should that remain a separate capability ticket instead of expanding the shared-type baseline?
- After provider-neutral bridge mapping lands, do any provider packages need separate optimization tickets for bridge-specific storage or indexing beyond the common baseline?

Risks
- If sibling implementation or later ticket text diverges from docs/plans/06EZ0NV0Y81AE1Z1Q3223TX2S4-bridge-metadata-v1-contract.md, the mapping and documentation siblings may target different bridge contracts.
- If the new hierarchy-depth property is modeled by reusing existing payload or technical semantics instead of a distinct logical kind or annotation, provider capability mappings and tests will become ambiguous.
- If endpoint declaration order is not preserved exactly, produced column order and deterministic key and index names will drift from the ratified examples.
- If this ticket introduces EF relationships or provider-specific physical behavior, the existing shared-type and SQLite baselines may regress.

Split recommendations
- No further split is recommended now; the existing bridge story plus metadata, mapping, and documentation siblings remain the right decomposition.
- Treat richer bridge capability families such as effectivity windows, closure maintenance, query-model helpers, or navigation graph generation as separate follow-up tickets rather than broadening this provider-neutral mapping slice.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment