[gicket-bot] PO refinement contract

Summary
- Current bridge-mapping ticket remains blocked on sibling metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4; the live blocks relation already persists sequencing, but the authoritative bridge metadata/public API contract and concrete worked examples are still missing.

PO handoff
- decision: `needs_po_clarification`
- meaning: ticket needs more clarification before PO-critic review

PO-critic checklist responses
- critic-item-1: `needs_human_input` - Not resolved in this ticket. Sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 is still todo with needs-po and only a legacy goal/acceptance block, so the authoritative bridge metadata shapes, validation ownership, and required public API additions do not yet exist as a durable contract. This mapping ticket remains blocked on that sibling refinement.
- critic-item-2: `answered` - Sequencing is already persisted in live ticket relations: 06EZ0NV0Y81AE1Z1Q3223TX2S4 blocks 06EZ0NV7KG94MTMNXMGVRYVW9C, so no new relation write is needed in this pass and the contract should explicitly preserve that dependency.
- critic-item-3: `needs_human_input` - Still unresolved. The current ticket can preserve the mapping-side expectations once metadata exists, but it still lacks authoritative worked many-to-many and hierarchy examples with exact bridge table names, column set, primary key/index layout, annotations, and the split between metadata-validation failures and translator-time not-supported failures. Those examples must be anchored to the sibling metadata contract first.
- critic-item-4: `answered` - Confirmed as the current blocker. This ticket still cannot be treated as developer-ready because the repository baseline exposes only hub/link/satellite metadata and translation surfaces, while this ticket delegates bridge-shape validity to sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4.
- critic-item-5: `answered` - Resolved as to live ticket state: the sequencing dependency is already persisted through a blocks relation from 06EZ0NV0Y81AE1Z1Q3223TX2S4 to this ticket. The refinement contract should now ratify that live relation instead of repeating the older no-relation-write posture.
- critic-item-6: `needs_human_input` - Still unresolved for PO-critic readiness. The current repo proves the existing translator/test pattern for deterministic table, column, key, index, annotation, and no-relationship outputs, but it does not yet provide authoritative bridge worked examples. Those examples should be added after sibling ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 defines the exact bridge metadata shapes and naming/public API baseline.

Clarifications
- This ticket remains the provider-neutral EF projection child under bridge story 06EZ0NTV4SVAKV98C418T8A3CC beside metadata ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 and documentation ticket 06EZ0NVE88WW9PMM04NVAZHRG0.
- Live ticket relations already persist the required sequencing: 06EZ0NV0Y81AE1Z1Q3223TX2S4 blocks 06EZ0NV7KG94MTMNXMGVRYVW9C.
- Repository evidence still shows only hub/link/satellite model, translator, and SQLite-schema baselines today, so bridge work here remains additive provider-neutral EF translation work once the sibling metadata contract exists.
- No new child tickets, attachments, or planning documents were created in this pass because the immediate blocker is the missing durable sibling metadata contract, not a further decomposition gap.

Scope In
- After 06EZ0NV0Y81AE1Z1Q3223TX2S4 defines the authoritative bridge metadata contract, translate its validated baseline many-to-many and hierarchy bridge shapes into provider-neutral EF shared-type metadata.
- Project deterministic bridge table names, ordered columns, primary keys, secondary indexes, produced-name annotations, and provider-profile storage metadata through the existing ApplyDataVaultMetadata path without regressing hub/link/satellite behavior.
- Add unit and SQLite schema coverage that locks down bridge outputs and translation-boundary not-supported diagnostics for each sibling-defined baseline bridge shape.

Scope Out
- Defining bridge metadata types, naming inputs, missing-reference validation, cycle rules, ambiguous-relationship rules, or public API additions; those stay with 06EZ0NV0Y81AE1Z1Q3223TX2S4.
- Bridge documentation and user-facing examples owned by 06EZ0NVE88WW9PMM04NVAZHRG0.
- Changes to IDataVaultSaveService, DataVaultSaveRequest, runtime loading, migrations, schema-refresh automation, or provider-specific SQL/DDL behavior.
- Advanced bridge capability expansion such as effectivity windows, closure maintenance, consumer-specific query models, or EF navigation graphs.

Open questions
- What exact bridge metadata/public API contract will ticket 06EZ0NV0Y81AE1Z1Q3223TX2S4 publish for baseline many-to-many and hierarchy bridges, including validation ownership?
- After that sibling contract exists, what exact many-to-many and hierarchy worked examples should this mapping ticket ratify for produced table/column names, primary key/index layout, annotations, and translator-time failure cases?

Follow-up questions
- After baseline bridge translation lands, should richer effectivity-window or closure-maintenance variants become separate follow-up capability tickets?
- If downstream consumers later need EF foreign keys or navigations for bridge entities, should that be a new capability ticket instead of broadening the shared-type projection baseline?

Risks
- If developers act before 06EZ0NV0Y81AE1Z1Q3223TX2S4 publishes a durable bridge metadata contract, they will have to invent bridge metadata/public API and naming semantics locally, creating churn across sibling tickets.
- If this ticket absorbs metadata-validation rules instead of only translation-specific failures, ownership boundaries will blur and bridge-contract decisions will be duplicated.
- If bridge mapping introduces new technical timestamp families, provider logical kinds, or EF relationships prematurely, the current provider-capability and translator-test baselines may regress.

Split recommendations
- No further split is recommended now. The parent bridge story plus metadata, mapping, and documentation siblings remain the right decomposition; the immediate need is to refine 06EZ0NV0Y81AE1Z1Q3223TX2S4, not to create more child tickets.
- If later bridge work needs effectivity windows, closure maintenance, or consumer-specific query optimization, create separate follow-up tickets instead of broadening this provider-neutral mapping slice.

Persisted contract coverage
- acceptance-criteria items: 4
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Keep labels unchanged.
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment