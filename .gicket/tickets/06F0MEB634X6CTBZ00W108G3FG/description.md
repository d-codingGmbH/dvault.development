<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Verified the registry and code-first prerequisites already in the repository and refined this ticket into the additive app-startup and DbContext integration layer that publishes one authoritative metadata registry without changing the optionless default path; no child tickets, planning documents, or relation edits were materialized.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Repository evidence already provides DataVaultMetadataRegistry, DataVaultMetadataRegistryBuilder, DataVaultMetadataModel, and the additive root-namespace code-first ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) path, so this ticket wires those existing pieces into startup and EF integration instead of introducing a second registry contract.
- The current optionless AddDVault() baseline and the current UseDataVault()-alone no-table behavior remain valid; registry-backed projection is an additive opt-in path, not a breaking change to ordinary startup.
- The canonical stored artifact for this ticket is DataVaultMetadataRegistry; callers may register either a DataVaultMetadataModel that is converted once into a registry or a prebuilt registry when they need custom provider-profile or CLR-mapping content.
- DbContext consumption must be enabled through an explicit DbContext-scoped integration surface and model annotations so OnModelCreating can stay free of user-authored service-location calls.
- App-level registration is a default source for opted-in contexts; an explicit context- or model-scoped source may override that default, but one EF model must not silently merge two distinct metadata sources.
- No relation cleanup is required in this pass: the incoming done blockers 06F0MEAXT99V0P115P0WEJD4P0 and 06F0ME9PM8KXH3VP59TQR0ETA8 still describe the dependency chain, and the existing outgoing blocks to 06F0MEBFTW8FY5T7PY5HJ5JXJ4, 06F0MECFNF42NK9PND9DWVW9VW, and 06F0MECPFAVBFBNC5XMVDZRQ6M remain unchanged.

### Scope In
- Extend DataVaultOptions with the minimum advanced registration surface to accept a DataVaultMetadataModel and a prebuilt DataVaultMetadataRegistry as the app-level default metadata source.
- Add the minimum DbContext-scoped integration surface that lets an opted-in context consume the app-level default registry or an explicit per-context registry without duplicating metadata declarations in OnModelCreating.
- Persist the selected authoritative metadata source into model annotations and reuse the existing provider-aware EF translation baseline for projection.
- Define and validate precedence and conflict behavior across app-level defaults, context-level overrides, and explicit model projection.
- Add targeted tests and visible docs/examples for registration, opt-in consumption, preserved baseline behavior, and conflict diagnostics.

### Scope Out
- Any refactor of IDataVaultSaveService or IDataVaultReadService to resolve metadata from the registry; that remains on 06F0MEBFTW8FY5T7PY5HJ5JXJ4 and its downstream typed-helper tickets.
- New typed save/read helper APIs, model-first import or export, or convenience object mappers.
- Provider-specific SQL, save-strategy changes, or non-core provider capability selection redesign.
- Further expansion of the code-first authoring surface beyond consuming the already-produced DataVaultMetadataModel or a prebuilt registry.

## Acceptance Criteria
- A caller can register DVault metadata once during service setup through AddDVault(...) by supplying either a DataVaultMetadataModel or a prebuilt DataVaultMetadataRegistry, and the resulting default registry is immutable and deterministic.
- An opted-in DbContext can project the registered metadata through ordinary model configuration without recreating the same metadata declarations in OnModelCreating; a context that uses only the existing UseDataVault() baseline without the new opt-in surface continues to create no DVault tables.
- Registry-backed projection uses the existing provider-aware metadata translation baseline for the same metadata source, so the produced entities, columns, keys, indexes, and DVault annotations match the current explicit metadata path.
- Source selection is deterministic: an explicit context-scoped source overrides the app-level default for that context, but a single EF model that receives two distinct metadata sources fails fast with an actionable validation error that identifies the conflicting source kinds.
- When a caller explicitly applies metadata through the existing model-level path and a different registry-backed source is also configured for the same model, DVault throws before silent divergence or duplicate projection occurs.
- Automated tests cover app-level model registration, prebuilt registry registration, context opt-in consumption, preserved UseDataVault() no-table baseline, and conflict diagnostics.

## Definition of Done
- Public API and snapshot coverage reflect the additive startup and DbContext integration surface while keeping the current optionless AddDVault() and explicit ApplyDataVaultMetadata(...) entry points source-compatible.
- The implementation stores one authoritative registry selection per EF model and validates source conflicts before translation begins.
- Tests prove registry-backed projection and explicit metadata projection produce the same schema shape for the same metadata source, and prove the no-opt-in baseline still leaves UseDataVault() annotation-only.
- README or equivalent visible docs show the one-time registration flow and the no-service-location DbContext/model usage.
- No child tickets, planning documents, or relation mutations are required to complete this refinement pass.

## Implementation Notes
- Use the existing DataVaultMetadataRegistry as the single stored artifact for model integration; when callers provide only a DataVaultMetadataModel, build the registry once during registration rather than rebuilding it per context or per model.
- Keep DataVaultOptions as the advanced configuration entry point so the optionless AddDVault() path remains untouched for users who stay on the explicit metadata path.
- Add the minimum DbContext options and annotation bridge needed to flow the chosen registry into UseDataVault() or equivalent model configuration without requiring callers to resolve services inside OnModelCreating.
- Do not fork the EF translator: registry-backed projection should unwrap to the existing metadata-first translation path so naming, ordering, provider profiles, and validation behavior stay centralized.
- Use the repository's current exact-ordinal registry semantics and existing provider-profile defaulting behavior; this ticket should not weaken the SQLite-default baseline or invent case-insensitive metadata lookup.
- Downstream registry consumers remain on the already-created blocked tickets 06F0MEBFTW8FY5T7PY5HJ5JXJ4, 06F0MECFNF42NK9PND9DWVW9VW, and 06F0MECPFAVBFBNC5XMVDZRQ6M; this ticket only establishes the registration and model-consumption layer they depend on.

## Open Questions
- none

## Follow-Up Questions
- After this default registry wiring lands, should the primary README quickstart switch to the registry-backed one-time registration flow, or should the explicit ApplyDataVaultMetadata(new DataVaultMetadataModel(...)) sample remain the first example alongside it?
- If later callers want app-level code-first authoring convenience, should that be added as a separate AddDVault/DbContext convenience overload that builds a registry from DataVaultCodeFirstModelBuilder, or should v1 require callers to hand the resulting DataVaultMetadataModel or registry into this ticket's registration surface?

## Risks
- If registry-backed projection bypasses the existing metadata-first translation path, schema names, key ordering, or provider annotations can drift from the explicit baseline and break parity expectations.
- If the new integration auto-projects metadata for every UseDataVault() context instead of requiring explicit opt-in, it will break the current tested baseline where UseDataVault() alone creates no DVault tables.
- If source-conflict validation runs after EF entities are already partially added, callers will see opaque duplicate-entity or model-build failures instead of a clear DVault-specific diagnostic.
- If app-level registration silently overrides a different context-level or model-level source, downstream save/read tickets will inherit nondeterministic model selection.

## Split Recommendations
- No split recommended. The ticket is already bounded to app-level registration, DbContext opt-in integration, and source-precedence validation, while downstream registry consumers are already split into 06F0MEBFTW8FY5T7PY5HJ5JXJ4, 06F0MECFNF42NK9PND9DWVW9VW, and 06F0MECPFAVBFBNC5XMVDZRQ6M.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Goal

Wire the registry into ordinary application startup and EF model configuration without forcing users into a service-location pattern.

## Scope In

- AddDVault options for registry/model registration.
- DbContext/model annotation integration for configured metadata.
- Clear precedence rules when explicit metadata and registry-backed metadata are both present.

## Scope Out

- Model-first import.
- Typed save/read helpers.
- Provider-specific SQL changes.

## Acceptance Criteria

- A typical app can register the Data Vault model once during service setup.
- EF model projection can consume the registered model without duplicating declarations.
- Conflicting model sources produce a validation error rather than silent divergence.