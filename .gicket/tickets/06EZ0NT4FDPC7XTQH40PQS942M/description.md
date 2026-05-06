<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined this PIT child task against the current provider-neutral modeling surfaces and the existing PIT story split; no child-ticket, relation, attachment, or planning-document writes were needed.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This ticket remains the metadata-and-builder child under PIT story 06EZ0NSXY2Y1JZ8SSCX177C770; sibling task 06EZ0NTB26CCYQ7FCN2REEGDGW owns provider-neutral EF mapping and sibling task 06EZ0NTJZEMVA5RPR01V0KNVMR owns docs/example work.
- Repository evidence shows no existing PIT implementation surface in src/DCoding.Data.DVault, so this task should introduce a new provider-neutral PIT contract rather than retrofit provider-specific behavior.
- PIT remains an explicit opt-in deferred capability per docs/plans/deferred-data-vault-capabilities.md and must not change default hub/link/satellite modeling when no PIT is declared.
- The PIT contract should mirror the current split between provider-neutral metadata declarations and a convention-first model-generation builder surface, instead of collapsing PIT directly into EF-only mapping code.
- Live relation state remains unchanged: the ticket still has the incoming parentOf relation from 06EZ0NSXY2Y1JZ8SSCX177C770, and no relation writes were materialized in this pass.

### Scope In
- Provider-neutral PIT metadata declarations for one PIT table, exactly one hub reference, and one or more satellite references.
- Aggregate-model validation that resolves PIT references against declared hub and satellite metadata.
- A convention-first PIT builder API used by model generation and flowing through the existing naming-policy override surface.
- The provider-neutral PIT generated table and field shape needed for deterministic names and key-field assertions.
- Unit and public-API coverage for PIT metadata, validation, and model-generation output.

### Scope Out
- Provider-neutral EF Core mapping and annotation projection for PIT tables; that work already belongs to 06EZ0NTB26CCYQ7FCN2REEGDGW.
- README/docs examples and end-user PIT guidance; that work already belongs to 06EZ0NTJZEMVA5RPR01V0KNVMR.
- Refresh scheduling, late-arriving-data reconciliation, persisted-versus-computed PIT materialization policy, provider-specific SQL, and migrations.
- Any change that makes PIT required for AddDVault(), UseDataVault(), ApplyDataVaultMetadata(), or ordinary hub/link/satellite declarations.
- Bridge, multi-active, hook, or save-service behavior outside the PIT metadata and model-generation contract.

## Acceptance Criteria
- The API can declare a PIT table that references exactly one declared hub and a non-empty ordered set of declared satellites for that hub.
- Model-wide validation fails fast with clear errors when the PIT hub reference is missing, when a PIT satellite reference is missing or does not belong to the declared hub, when the satellite set is empty, or when the same satellite is referenced more than once.
- The builder surface can express the same PIT declaration through the convention-first model-generation API without requiring provider-specific options or advanced hook configuration.
- The PIT contract exposes deterministic provider-neutral names and key-field descriptors for the PIT table, the hub hash-key reference, the PIT load timestamp, and the per-satellite snapshot load-timestamp references used by later mapping work.
- Repeated builds of equivalent PIT input produce identical names and key-field ordering, and naming-policy overrides flow through the PIT surface the same way they do for hubs, links, and satellites.
- Public API snapshot and unit tests cover the new PIT metadata and builder surface plus its validation behavior.

## Definition of Done
- New PIT public types and builder members are added to the approved public API snapshot with XML comments describing their role and constraints.
- Validation tests cover unresolved hub references, unresolved satellite references, empty satellite sets, cross-hub satellite misuse, and duplicate satellite references.
- Model-generation tests prove deterministic PIT table and field names, key-field ordering, and naming-policy override behavior across repeated runs.
- Using PIT remains opt-in; existing hub/link/satellite behavior and tests stay unchanged when PIT is not declared.
- The resulting PIT contract is specific enough that ticket 06EZ0NTB26CCYQ7FCN2REEGDGW can implement EF mapping without redefining the provider-neutral PIT field model.

## Implementation Notes
- Treat PIT as a first-class provider-neutral artifact, analogous to other top-level opt-in declarations such as links, because it references existing hub and satellite declarations rather than defining new satellite payloads.
- Extend the aggregate metadata model with PIT participation and perform reference resolution there, so raw PIT declarations can stay small while validation remains deterministic.
- Keep PIT naming on the existing IDataVaultNamingPolicy and DataVaultModelOptions path so default names and custom overrides stay consistent with current model-generation behavior.
- Do not force PIT-specific fields into the closed shared TechnicalMetadataColumnRole set unless there is no smaller provider-neutral alternative; PIT-owned snapshot fields can carry PIT-specific semantics without redefining ingest metadata roles.
- Use stable declaration order for PIT satellite references so duplicate detection, generated field ordering, and repeated model builds remain deterministic.
- Keep the PIT key and reference contract aligned between the pure model builder and later EF mapping work; the current branch already shows satellite index-shape drift between DataVaultModel tests and DataVaultEfMetadataTranslator tests, and PIT should not repeat that split.

## Open Questions
- none

## Follow-Up Questions
- When PIT generation moves beyond the metadata and builder contract, which v0.5 refresh strategy should land first: full recompute, incremental refresh, or a provider-owned materialization path?
- Should a later PIT behavior ticket define persisted-only PIT tables, computed query-time PIT projections, or both?
- How should late-arriving satellite rows be reconciled in PIT refresh logic once materialization behavior is in scope?

## Risks
- If the implementation reuses existing satellite technical metadata or key-role abstractions too aggressively, PIT-specific fields may leak into the closed v1 ingest contract and create unnecessary public-API churn.
- If this ticket does not pin one provider-neutral PIT key and reference baseline, sibling mapping work may invent a different field shape and create model-builder versus EF-mapping drift.
- Current branch evidence already contains satellite index-shape differences between pure model generation and EF translation, so PIT tests need explicit cross-surface assertions to prevent the same divergence.

## Split Recommendations
- No additional split recommended. The parent story is already bounded across 06EZ0NT4FDPC7XTQH40PQS942M for metadata and builder work, 06EZ0NTB26CCYQ7FCN2REEGDGW for EF mapping, and 06EZ0NTJZEMVA5RPR01V0KNVMR for docs/example work.
- No child tickets, relation changes, attachments, or planning documents were materialized in this refinement pass.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

Goal: define the PIT metadata model and builder API used by model generation.

Acceptance Criteria:
- The API can describe a PIT table for one hub and multiple satellites.
- The model validates missing hub references, empty satellite sets, and duplicate satellite references.
- Names and key fields are deterministic and documented.