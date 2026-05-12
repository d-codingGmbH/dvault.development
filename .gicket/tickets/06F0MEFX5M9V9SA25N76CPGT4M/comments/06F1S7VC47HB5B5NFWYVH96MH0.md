[gicket-bot] PO refinement contract

Summary
- Refined the ticket around a deterministic, in-process model-to-EF metadata drift reporter that compares the dvault.model.v1 expected logical model against generated/current EF metadata without requiring a live database.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- The expected-model baseline is the v1 JSON-first dvault.model.v1 contract and its default naming policy; YAML authoring, migration execution, and database introspection remain outside this ticket.
- The current/generated baseline is the EF Core model metadata already carrying DVault annotations such as EntityKind, MetadataName, ProducedName, Ordinal, PropertyRole, TechnicalColumnRole, ProviderProfile, ProviderLogicalPropertyKind, ProviderStorageType, ProviderValueFormat, MetadataSourceKind, and MetadataSourceFingerprint.
- The v1 report should be deterministic: stable ordering, ordinal string comparisons where applicable, stable severity classification, and no dependency on current culture, machine state, database connectivity, or provider-generated live schema inspection.
- For v1, the bounded provider-capability baseline is the visible capability-profile annotation surface and existing provider profile concepts; the report should compare declared/projected provider logical storage metadata where present, not invent provider-specific DDL validation.

Scope In
- Add a provider-neutral comparison API that accepts an expected Data Vault model representation and an EF Core generated/current model metadata representation.
- Compare logical tables/entities for hubs, links, satellites, PITs, and bridges where supported by the existing metadata model.
- Compare columns/properties, keys, indexes, technical Data Vault roles, logical/physical names, declaration ordinals, timestamp storage metadata, and provider capability/profile annotations that are available in the EF model.
- Classify differences into informational and blocking categories with deterministic difference codes and severity.
- Return both human-readable and machine-readable report forms that identify affected elements by logical metadata name and produced/generated physical name.
- Add focused tests for representative drift cases including missing table/entity, missing column/property, role mismatch, type/storage mismatch, key/index mismatch, timestamp storage mismatch, provider capability/profile mismatch, and informational name-only or metadata-source differences.

Scope Out
- Executing live database migrations or comparing against a live database schema.
- Automated CI gating or build-failure policy decisions based on drift output.
- Changing the dvault.model.v1 schema contract, adding YAML ingestion, or adding new provider-specific DDL contracts.
- Implementing broad advanced configuration hooks beyond consuming the existing default naming, timestamp, and provider metadata decisions.
- Creating subtickets or expanding this ticket into end-to-end deployment governance.

Open questions
- none

Follow-up questions
- Should a later ticket add CI gating policies or command-line exit codes based on blocking drift severity?
- Should later provider packages add optional live database schema comparison using provider-specific introspection after the metadata-only drift report exists?
- Should later governance work define a versioned machine-readable report schema for external tools beyond the initial internal structured report records?

Risks
- If some generated metadata paths do not yet annotate keys or indexes with enough DVault-owned information, implementation may need narrowly scoped metadata extraction from EF Core key/index APIs while keeping output deterministic.
- Provider capability comparison must avoid overclaiming provider-specific DDL guarantees because the v1 contract is provider-neutral and the visible baseline is annotation/profile metadata rather than live schema inspection.

Split recommendations
- none

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment