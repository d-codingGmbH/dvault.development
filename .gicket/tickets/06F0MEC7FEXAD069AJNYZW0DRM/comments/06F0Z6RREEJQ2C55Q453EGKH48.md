[gicket-bot] PO refinement contract

Summary
- Refined the ticket to keep the registry-backed mapper baseline, explicitly reject repeated same-hub/self-link typed link targets in v1, and align missing-required-value validation with the existing IDataVaultSaveService pipeline instead of inventing a new validator.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - V1 resolves repeated same-hub participants, including two-participant self-links, by marking them explicitly unsupported for typed link mappers. IDataVaultLinkMapper<TSource> still returns DataVaultRegistryLinkSaveOperation, but that output is valid only when participant hub metadata names are unique by StringComparer.Ordinal; attempts to emit duplicate participant names are a documented contract failure covered by tests. Role-, ordinal-, or alias-based participant identity is deferred to a later ticket.
- critic-item-2: `answered` - PO chooses the bounded baseline and relaxes the ticket's validation promise to the existing save-pipeline boundary instead of adding a thin registry-aware validator or factory. Missing required hub business-key names, link participant names, and satellite payload names remain owned by the existing explicit save-service pipeline after registry resolution and before database commit; immediate constructor-level validation stays limited to null names, null values, duplicate names, and invalid driving-key sets.
- critic-item-3: `answered` - The under-specification is closed by ratifying the visible repository baseline: self-links remain valid metadata shapes in DVault overall, but v1 typed link mappers do not claim to support them. Because the canonical registry-backed link operation is keyed only by participant hub metadata name, the contract explicitly rejects repeated same-hub/self-link typed link mappings instead of pretending they are representable.
- critic-item-4: `answered` - Validation ownership is now explicit: duplicate-name, null-name, null-value, and multi-active driving-key-set diagnostics stay on the existing save-operation constructors, while missing required hub business-key, link participant, and satellite payload names stay on the existing explicit save-service pipeline during plan creation. This ticket does not add a new public validating abstraction; it documents and tests the current boundary instead.

Clarifications
- The canonical v1 mapper outputs remain DataVaultRegistryHubSaveOperation, DataVaultRegistryLinkSaveOperation, and DataVaultRegistrySatelliteSaveOperation.
- V1 IDataVaultLinkMapper<TSource> support is limited to links whose participant hub metadata names are unique by StringComparer.Ordinal.
- Repeated same-hub participants, including ordinary self-links, are explicitly unsupported in v1 typed link mapping because the chosen registry-backed link operation is keyed only by participant hub metadata name and rejects duplicate names.
- Missing required hub business-key names, link participant names, and satellite payload names are not assigned to a new public validator or factory in this ticket.
- V1 instead reuses the existing explicit save boundary: registry-backed mapper outputs are resolved into DataVaultSaveRequest, and missing required names surface through the current ArgumentException diagnostics during save-plan creation before DbContext.SaveChangesAsync in the provider-neutral baseline.
- Null source inputs, null mapped values, duplicate mapped names, and invalid multi-active driving-key sets remain immediate contract failures at mapper or supporting-operation construction time.
- LoadTimestamp and RecordSource remain explicit request-level inputs when later helpers assemble DataVaultRegistrySaveRequest or DataVaultRegistryBulkSaveRequest.

Scope In
- Define IDataVaultHubMapper<TSource>, IDataVaultLinkMapper<TSource>, and IDataVaultSatelliteMapper<TSource> in DCoding.Data.DVault with one-source-to-one-row-operation mapping into the existing registry-backed save-operation family.
- Define the exact logical-name target identity rules for hub, link, and satellite mapper outputs, including parent-scoped satellite resolution for both hub-parent and link-parent satellites.
- Define the v1 typed-link boundary that supports only links with unique participant hub metadata names and explicitly rejects repeated same-hub or self-link shapes through the existing duplicate-name operation surface.
- Define the request-boundary rule that LoadTimestamp and RecordSource stay outside row mappers and remain explicit when later helpers assemble save requests.
- Define validation and diagnostic expectations for null source inputs, null mapped values, duplicate output names, missing or extra driving-key names, and missing required values at the current save-service boundary.
- Define additive API placement, XML-doc expectations, API-snapshot coverage, and contract tests that prove manual mappers can feed the existing registry-backed save path.

Scope Out
- Supporting repeated same-hub or self-link typed link mappings through role-, ordinal-, or alias-based participant identity in v1.
- Adding a new public registry-aware validator, mapper factory, or helper whose sole purpose is to precompute missing required hub/link/payload names before the existing save-service pipeline runs.
- Implementing typed save-helper APIs that submit mapped operations through IDataVaultSaveService.
- Typed latest or as-of read projections, DTO materializers, or read-side mapper contracts.
- Automatic SaveChanges interception, source generation, reflection-based auto-mapping, or hidden hashing conveniences beyond what the mapper explicitly returns.
- Provider-specific save-strategy changes or changes to IDataVaultSaveService, DataVaultSaveRequest, DataVaultRegistrySaveRequest, or existing metadata-object-based APIs.

Open questions
- none

Follow-up questions
- If typed link support for same-hub or self-link relationships becomes necessary later, should the follow-up contract use participant role names, explicit ordinals, or stable aliases as the public participant identity?
- After the row-mapper contract lands, should a later convenience layer add composite request mappers that map one source object to a hub-plus-satellite request, or should v1 stay with one-row mappers composed by the helper?
- Should a later convenience layer add optional participant business-key-to-link hash-key derivation, or should link mappers continue to supply participant hash keys explicitly?
- Should future code-first or model-first tooling auto-emit mapper implementations or registry CLR mappings so callers can opt into metadata lookup by source CLR type without changing this v1 contract?
- If downstream consumers want earlier missing-required-value feedback than the current save-service boundary, should that be a separate validator ticket rather than an expansion of this mapper-contract ticket?

Risks
- If implementation tries to accept same-hub or self-link typed links without changing the participant identity shape, distinct participants will collapse because the current registry-backed operation is keyed only by participant hub metadata name and rejects duplicates.
- If docs or tests keep promising missing required values before save orchestration begins, the ticket will misstate the existing repository boundary and force an unplanned validator abstraction.
- If implementation targets metadata-object-based save operations instead of the chosen registry-backed operation family, typed helpers will drift from the authoritative-registry path already established in the repository.
- If hidden CLR-type metadata inference is added in v1, metadata-first or code-first registrations without DataVaultMetadataClrMapping will fail unpredictably even though current repository evidence makes CLR mappings optional.
- Because current operation inputs are string-based, weak coverage around mapper-produced string values could still allow inconsistent caller-side business-key, participant-hash-key, or hash-diff formatting unless tests pin the contract down clearly.

Split recommendations
- No split is needed for this v1 contract ticket as refined.
- If same-hub or self-link typed link support becomes a real requirement, open a separate follow-up ticket for participant role-, ordinal-, or alias-based identity and any necessary save-operation shape changes instead of stretching this mapper v1 contract.

Persisted contract coverage
- acceptance-criteria items: 7
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment