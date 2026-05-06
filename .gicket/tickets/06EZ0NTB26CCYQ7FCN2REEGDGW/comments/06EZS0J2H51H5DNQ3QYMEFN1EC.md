[gicket-bot] PO refinement contract

Summary
- Made the consumed PIT input contract explicit on this ticket, confirmed the live sibling blocks relation already exists, and bounded the work against the repository's current hub/link/satellite-only surface so the ticket can return to PO-critic without new planning writes.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - This ticket now treats the minimum PIT input it consumes as its own persisted consumer-side contract instead of depending on the sibling's mutable revision string. The copied contract is one hub, one or more attached satellites, deterministic PIT naming and key fields, and deterministic rejection of missing hub references, empty satellite sets, duplicate satellite references, and other out-of-baseline shapes. That keeps the dependency concrete even though gicket-read-ticket still shows sibling 06EZ0NT4FDPC7XTQH40PQS942M at revision 06EZRZ7HZX7DD7Q3T7VJ5HW1JG with only a legacy draft.
- critic-item-2: `answered` - Machine-readable sequencing already exists in live relations: 06EZ0NT4FDPC7XTQH40PQS942M blocks 06EZ0NTB26CCYQ7FCN2REEGDGW. This ticket no longer needs to treat that dependency as a hypothetical follow-up or prose-only coordination item.
- critic-item-3: `answered` - Direct source evidence still shows no PIT-facing public surface today: DataVaultMetadata.cs defines only hub, link, and satellite metadata, DataVaultEfMetadataTranslator.CreateEntities iterates only Hubs, Links, and Satellites, and DataVaultPropertyRole currently exposes only Technical, BusinessKey, ParticipantReference, and Payload. The contract therefore keeps PIT input and modeling API work out of scope here, keeps PIT output projection and any required output-side public additions in scope, and relies on the copied minimum input contract plus the live blocks relation so the developer does not invent PIT behavior by assumption.

Clarifications
- For this mapping ticket, the authoritative consumed PIT input contract is the minimum contract copied into this ticket: one hub, one or more attached satellites, deterministic PIT naming and key fields, and deterministic failure for out-of-baseline shapes.
- The bounded worked baseline remains hub Customer with satellites declared as [Profile, Status], and that declaration order is authoritative for generated PIT entity naming, primary-key shape, and per-satellite snapshot reference column order.
- Sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M still owns the producer-side PIT modeling and builder API plus broader input validation design; this ticket only owns provider-neutral EF projection and any required PIT output-side public surface.
- Live relations already include 06EZ0NT4FDPC7XTQH40PQS942M blocks 06EZ0NTB26CCYQ7FCN2REEGDGW and the shared PIT story parent relation; no new child tickets, attachments, planning documents, or relation writes were needed in this pass.

Scope In
- Translate the copied minimum PIT contract for one hub and one or more attached satellites into one provider-neutral shared-type EF entity through ApplyDataVaultMetadata.
- Emit deterministic PIT entity, table, property, key, annotation, and SQLite-baseline schema and queryability behavior for the one-hub plus attached-satellite baseline.
- Add or update PIT output-side public surfaces required by translation, including any entity-kind, property-role, logical-kind, annotation, provider-mapping, or snapshot changes needed to expose translated PIT entities.
- Add positive and negative translation coverage that proves deterministic ordering and deterministic rejection of out-of-contract PIT shapes.

Scope Out
- Defining or revising PIT input-side modeling types, builder API entry points, naming-policy extension APIs, or broader producer-side validation semantics beyond the minimum consumed contract copied here.
- PIT refresh or population orchestration, PIT query helpers, migrations, provider-specific SQL, or runtime optimization work.
- README or docs and example authoring, which remains in sibling ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Link-based PIT tables, multi-active satellite PIT behavior, and broader deferred-capability expansion beyond the one-hub plus attached-satellite baseline.

Open questions
- none

Follow-up questions
- Sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M should still receive its own durable producer-side delivery contract so its eventual API matches the consumer-side minimum copied here before the existing blocks dependency is cleared.
- Should a later PIT population or read-optimization ticket define null-handling or carry-forward semantics when one included satellite has no row at a PIT instant?
- If later PIT work needs link-based PIT tables or multi-active satellite snapshots, should those remain separate follow-up tickets instead of widening this baseline?

Risks
- Because the repository currently exposes zero PIT-facing public surface, implementation may require coordinated additions across enums, annotations, provider type mappings, translator tests, and approved API snapshots.
- If sibling 06EZ0NT4FDPC7XTQH40PQS942M lands a producer-side contract that diverges from the consumer-side minimum copied here, this ticket will need a PO re-check before dev handoff even though the live blocks relation already enforces sequencing.

Split recommendations
- No new functional split is needed; keep the existing PIT story split between producer-side modeling API ticket 06EZ0NT4FDPC7XTQH40PQS942M, EF mapping and projection ticket 06EZ0NTB26CCYQ7FCN2REEGDGW, and docs or examples ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Retain the existing blocks relation for sequencing instead of creating another coordination ticket or planning document.

Persisted contract coverage
- acceptance-criteria items: 5
- definition-of-done items: 4
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment