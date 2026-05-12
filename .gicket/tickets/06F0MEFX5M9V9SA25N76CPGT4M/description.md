<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the ticket around a deterministic, in-process model-to-EF metadata drift reporter that compares the dvault.model.v1 expected logical model against generated/current EF metadata without requiring a live database.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- The expected-model baseline is the v1 JSON-first dvault.model.v1 contract and its default naming policy; YAML authoring, migration execution, and database introspection remain outside this ticket.
- The current/generated baseline is the EF Core model metadata already carrying DVault annotations such as EntityKind, MetadataName, ProducedName, Ordinal, PropertyRole, TechnicalColumnRole, ProviderProfile, ProviderLogicalPropertyKind, ProviderStorageType, ProviderValueFormat, MetadataSourceKind, and MetadataSourceFingerprint.
- The v1 report should be deterministic: stable ordering, ordinal string comparisons where applicable, stable severity classification, and no dependency on current culture, machine state, database connectivity, or provider-generated live schema inspection.
- For v1, the bounded provider-capability baseline is the visible capability-profile annotation surface and existing provider profile concepts; the report should compare declared/projected provider logical storage metadata where present, not invent provider-specific DDL validation.

### Scope In
- Add a provider-neutral comparison API that accepts an expected Data Vault model representation and an EF Core generated/current model metadata representation.
- Compare logical tables/entities for hubs, links, satellites, PITs, and bridges where supported by the existing metadata model.
- Compare columns/properties, keys, indexes, technical Data Vault roles, logical/physical names, declaration ordinals, timestamp storage metadata, and provider capability/profile annotations that are available in the EF model.
- Classify differences into informational and blocking categories with deterministic difference codes and severity.
- Return both human-readable and machine-readable report forms that identify affected elements by logical metadata name and produced/generated physical name.
- Add focused tests for representative drift cases including missing table/entity, missing column/property, role mismatch, type/storage mismatch, key/index mismatch, timestamp storage mismatch, provider capability/profile mismatch, and informational name-only or metadata-source differences.

### Scope Out
- Executing live database migrations or comparing against a live database schema.
- Automated CI gating or build-failure policy decisions based on drift output.
- Changing the dvault.model.v1 schema contract, adding YAML ingestion, or adding new provider-specific DDL contracts.
- Implementing broad advanced configuration hooks beyond consuming the existing default naming, timestamp, and provider metadata decisions.
- Creating subtickets or expanding this ticket into end-to-end deployment governance.

## Acceptance Criteria
- A deterministic drift report can be produced from in-memory model metadata without a live database connection.
- Machine-readable output includes stable difference identifiers, severity, logical element kind/name, produced or physical name when available, expected value, actual value, and a concise message.
- Human-readable output groups or orders differences consistently so repeated runs over the same inputs produce the same content order.
- Blocking differences include missing required generated tables/entities, missing required properties, incompatible key/index definitions, incompatible property roles, incompatible timestamp storage, and incompatible provider logical storage/profile metadata.
- Informational differences are distinguished from blocking incompatibilities and do not prevent the report from representing the full drift set.
- Reports identify affected model elements using both logical Data Vault metadata names and generated EF/physical names when both are available.
- Representative tests cover at least one no-drift case, one informational-only case, and multiple blocking drift cases without requiring live database migration or database introspection.

## Definition of Done
- Public or internal APIs needed by downstream tooling are named and documented consistently with the existing DVault metadata and diagnostics style.
- The diff engine uses existing DVault naming policy and EF annotations instead of duplicating independent naming rules where repository APIs already expose the produced names.
- Report generation is culture-invariant, deterministic, and stable under repeated runs.
- Unit tests or metadata-only integration tests demonstrate report contents and severity classification for representative table, column, key, index, timestamp, and provider capability drift.
- No live database execution, migration application, or CI gate behavior is introduced as part of this ticket.

## Implementation Notes
- Treat dvault.model.v1 plus the default naming policy as the expected logical contract and the EF Core IModel/DVault annotation surface as the generated/current contract.
- Use existing annotations in DataVaultAnnotationNames as the authoritative comparison inputs for metadata source, entity kind, logical metadata name, produced name, ordinals, property roles, technical column roles, and provider profile/storage/value-format decisions.
- Compare values with ordinal string semantics and emit differences in a stable order such as element kind, logical name, produced name, difference code, and property path.
- Prefer small immutable report records for machine-readable output and a formatter layer for human-readable text so later tooling can consume the structured form directly.
- Timestamp storage comparison should honor the v1 loadTimestampStorage tokens and the repository default/provider profile metadata already present rather than performing provider DDL inspection.
- Provider capability drift should be limited to the finite visible provider profile/logical storage annotations available on the generated EF metadata for v1.

## Open Questions
- none

## Follow-Up Questions
- Should a later ticket add CI gating policies or command-line exit codes based on blocking drift severity?
- Should later provider packages add optional live database schema comparison using provider-specific introspection after the metadata-only drift report exists?
- Should later governance work define a versioned machine-readable report schema for external tools beyond the initial internal structured report records?

## Risks
- If some generated metadata paths do not yet annotate keys or indexes with enough DVault-owned information, implementation may need narrowly scoped metadata extraction from EF Core key/index APIs while keeping output deterministic.
- Provider capability comparison must avoid overclaiming provider-specific DDL guarantees because the v1 contract is provider-neutral and the visible baseline is annotation/profile metadata rather than live schema inspection.

## Split Recommendations
- none

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Provide a deterministic comparison between the expected Data Vault model and generated/current EF table metadata so drift is visible before deployment.

## Scope In

- Table, column, key, index, timestamp storage, and provider capability differences.
- Human-readable and machine-readable report forms.
- Tests for representative drift cases.

## Scope Out

- Live database migration execution.
- Automated CI gating.

## Acceptance Criteria

- Drift reports distinguish informational differences from blocking incompatibilities.
- Reports identify affected model elements by logical and physical names.
- Basic checks do not require a live database.