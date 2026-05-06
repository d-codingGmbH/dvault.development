<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Made the consumed PIT input contract explicit on this ticket, confirmed the live sibling blocks relation already exists, and bounded the work against the repository's current hub/link/satellite-only surface so the ticket can return to PO-critic without new planning writes.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- For this mapping ticket, the authoritative consumed PIT input contract is the minimum contract copied into this ticket: one hub, one or more attached satellites, deterministic PIT naming and key fields, and deterministic failure for out-of-baseline shapes.
- The bounded worked baseline remains hub Customer with satellites declared as [Profile, Status], and that declaration order is authoritative for generated PIT entity naming, primary-key shape, and per-satellite snapshot reference column order.
- Sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M still owns the producer-side PIT modeling and builder API plus broader input validation design; this ticket only owns provider-neutral EF projection and any required PIT output-side public surface.
- Live relations already include 06EZ0NT4FDPC7XTQH40PQS942M blocks 06EZ0NTB26CCYQ7FCN2REEGDGW and the shared PIT story parent relation; no new child tickets, attachments, planning documents, or relation writes were needed in this pass.

### Scope In
- Translate the copied minimum PIT contract for one hub and one or more attached satellites into one provider-neutral shared-type EF entity through ApplyDataVaultMetadata.
- Emit deterministic PIT entity, table, property, key, annotation, and SQLite-baseline schema and queryability behavior for the one-hub plus attached-satellite baseline.
- Add or update PIT output-side public surfaces required by translation, including any entity-kind, property-role, logical-kind, annotation, provider-mapping, or snapshot changes needed to expose translated PIT entities.
- Add positive and negative translation coverage that proves deterministic ordering and deterministic rejection of out-of-contract PIT shapes.

### Scope Out
- Defining or revising PIT input-side modeling types, builder API entry points, naming-policy extension APIs, or broader producer-side validation semantics beyond the minimum consumed contract copied here.
- PIT refresh or population orchestration, PIT query helpers, migrations, provider-specific SQL, or runtime optimization work.
- README or docs and example authoring, which remains in sibling ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Link-based PIT tables, multi-active satellite PIT behavior, and broader deferred-capability expansion beyond the one-hub plus attached-satellite baseline.

## Acceptance Criteria
- Given PIT metadata representing hub Customer with satellites declared as [Profile, Status], ApplyDataVaultMetadata emits one PIT shared-type entity named PitCustomerProfileStatus with ordered columns CustomerHashKey, LoadTimestamp, ProfileLoadTimestamp, and StatusLoadTimestamp, and a primary key named PkPitCustomerProfileStatusCustomerHashKeyLoadTimestamp over CustomerHashKey and LoadTimestamp.
- Per-satellite snapshot reference columns follow satellite declaration order from the copied PIT contract, and ordinary hub, link, and satellite translation remains unchanged when PIT metadata is absent.
- Core translation remains provider-neutral and opt-in: SQLite-specific SQL and provider-name branching do not enter core PIT translation logic, while SQLite tests still prove create-and-read queryability for the baseline PIT shape.
- If PIT projection requires new public output surfaces, the same delivery updates those public members and the affected approved API snapshots in lockstep.
- The translator fails deterministically and explicitly for empty satellite sets, duplicate satellite references, satellites not attached to the declared hub, link-based PIT shapes, multi-active satellite PIT shapes, or any other shape outside the one-hub plus attached-satellite baseline.

## Definition of Done
- Unit translation tests assert PIT entity name, ordered columns, primary-key columns, annotation values, and stable repeated output for the Customer/Profile/Status baseline fixture.
- Negative translator tests cover the explicit out-of-contract PIT shapes named in this contract and prove deterministic failure rather than partial mapping.
- SQLite integration or snapshot coverage proves the PIT table can be created and read through EF for the baseline fixture without regressing existing hub, link, and satellite behavior when PIT metadata is absent.
- Any PIT output-side public surface introduced here is reflected in the same approved API snapshot update.

## Implementation Notes
- Treat the copied minimum PIT input contract in this ticket as the authoritative consumer-side contract until sibling 06EZ0NT4FDPC7XTQH40PQS942M is independently refined; do not make this mapping ticket depend on a mutable sibling revision string.
- Direct repository evidence still shows only hub, link, and satellite public surfaces today: DataVaultMetadata.cs defines hub/link/satellite metadata only, DataVaultEfMetadataTranslator.CreateEntities iterates only metadataModel.Hubs, metadataModel.Links, and metadataModel.Satellites, and DataVaultPropertyRole exposes only Technical, BusinessKey, ParticipantReference, and Payload.
- If PIT projection requires new public enums, annotations, logical-property kinds, provider mappings, or other output-side surface, update the affected approved API snapshots in the same delivery.
- Use the deterministic naming baseline PitCustomerProfileStatus, CustomerHashKey, LoadTimestamp, ProfileLoadTimestamp, StatusLoadTimestamp, and PkPitCustomerProfileStatusCustomerHashKeyLoadTimestamp.
- Keep the implementation bounded to one hub plus attached satellites and shared hub hash key plus PIT LoadTimestamp; do not widen this task into PIT population, query helpers, or provider-specific SQL.

## Open Questions
- none

## Follow-Up Questions
- Sibling ticket 06EZ0NT4FDPC7XTQH40PQS942M should still receive its own durable producer-side delivery contract so its eventual API matches the consumer-side minimum copied here before the existing blocks dependency is cleared.
- Should a later PIT population or read-optimization ticket define null-handling or carry-forward semantics when one included satellite has no row at a PIT instant?
- If later PIT work needs link-based PIT tables or multi-active satellite snapshots, should those remain separate follow-up tickets instead of widening this baseline?

## Risks
- Because the repository currently exposes zero PIT-facing public surface, implementation may require coordinated additions across enums, annotations, provider type mappings, translator tests, and approved API snapshots.
- If sibling 06EZ0NT4FDPC7XTQH40PQS942M lands a producer-side contract that diverges from the consumer-side minimum copied here, this ticket will need a PO re-check before dev handoff even though the live blocks relation already enforces sequencing.

## Split Recommendations
- No new functional split is needed; keep the existing PIT story split between producer-side modeling API ticket 06EZ0NT4FDPC7XTQH40PQS942M, EF mapping and projection ticket 06EZ0NTB26CCYQ7FCN2REEGDGW, and docs or examples ticket 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Retain the existing blocks relation for sequencing instead of creating another coordination ticket or planning document.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: generate provider-neutral EF Core mapping for baseline PIT tables.

Acceptance Criteria:
- Generated mapping covers table name, key columns, hub reference, satellite snapshot references, and load timestamp fields.
- Tests verify the mapping against SQLite as the local baseline without embedding SQLite-specific SQL in core logic.
- Unsupported PIT shapes fail with clear validation errors.