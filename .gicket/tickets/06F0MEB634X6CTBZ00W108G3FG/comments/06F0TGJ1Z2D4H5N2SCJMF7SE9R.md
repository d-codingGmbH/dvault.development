[gicket-bot] PO refinement contract

Summary
- Verified the registry and code-first prerequisites already in the repository and refined this ticket into the additive app-startup and DbContext integration layer that publishes one authoritative metadata registry without changing the optionless default path; no child tickets, planning documents, or relation edits were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already provides DataVaultMetadataRegistry, DataVaultMetadataRegistryBuilder, DataVaultMetadataModel, and the additive root-namespace code-first ApplyDataVaultMetadata(Action<DataVaultCodeFirstModelBuilder>) path, so this ticket wires those existing pieces into startup and EF integration instead of introducing a second registry contract.
- The current optionless AddDVault() baseline and the current UseDataVault()-alone no-table behavior remain valid; registry-backed projection is an additive opt-in path, not a breaking change to ordinary startup.
- The canonical stored artifact for this ticket is DataVaultMetadataRegistry; callers may register either a DataVaultMetadataModel that is converted once into a registry or a prebuilt registry when they need custom provider-profile or CLR-mapping content.
- DbContext consumption must be enabled through an explicit DbContext-scoped integration surface and model annotations so OnModelCreating can stay free of user-authored service-location calls.
- App-level registration is a default source for opted-in contexts; an explicit context- or model-scoped source may override that default, but one EF model must not silently merge two distinct metadata sources.
- No relation cleanup is required in this pass: the incoming done blockers 06F0MEAXT99V0P115P0WEJD4P0 and 06F0ME9PM8KXH3VP59TQR0ETA8 still describe the dependency chain, and the existing outgoing blocks to 06F0MEBFTW8FY5T7PY5HJ5JXJ4, 06F0MECFNF42NK9PND9DWVW9VW, and 06F0MECPFAVBFBNC5XMVDZRQ6M remain unchanged.

Scope In
- Extend DataVaultOptions with the minimum advanced registration surface to accept a DataVaultMetadataModel and a prebuilt DataVaultMetadataRegistry as the app-level default metadata source.
- Add the minimum DbContext-scoped integration surface that lets an opted-in context consume the app-level default registry or an explicit per-context registry without duplicating metadata declarations in OnModelCreating.
- Persist the selected authoritative metadata source into model annotations and reuse the existing provider-aware EF translation baseline for projection.
- Define and validate precedence and conflict behavior across app-level defaults, context-level overrides, and explicit model projection.
- Add targeted tests and visible docs/examples for registration, opt-in consumption, preserved baseline behavior, and conflict diagnostics.

Scope Out
- Any refactor of IDataVaultSaveService or IDataVaultReadService to resolve metadata from the registry; that remains on 06F0MEBFTW8FY5T7PY5HJ5JXJ4 and its downstream typed-helper tickets.
- New typed save/read helper APIs, model-first import or export, or convenience object mappers.
- Provider-specific SQL, save-strategy changes, or non-core provider capability selection redesign.
- Further expansion of the code-first authoring surface beyond consuming the already-produced DataVaultMetadataModel or a prebuilt registry.

Open questions
- none

Follow-up questions
- After this default registry wiring lands, should the primary README quickstart switch to the registry-backed one-time registration flow, or should the explicit ApplyDataVaultMetadata(new DataVaultMetadataModel(...)) sample remain the first example alongside it?
- If later callers want app-level code-first authoring convenience, should that be added as a separate AddDVault/DbContext convenience overload that builds a registry from DataVaultCodeFirstModelBuilder, or should v1 require callers to hand the resulting DataVaultMetadataModel or registry into this ticket's registration surface?

Risks
- If registry-backed projection bypasses the existing metadata-first translation path, schema names, key ordering, or provider annotations can drift from the explicit baseline and break parity expectations.
- If the new integration auto-projects metadata for every UseDataVault() context instead of requiring explicit opt-in, it will break the current tested baseline where UseDataVault() alone creates no DVault tables.
- If source-conflict validation runs after EF entities are already partially added, callers will see opaque duplicate-entity or model-build failures instead of a clear DVault-specific diagnostic.
- If app-level registration silently overrides a different context-level or model-level source, downstream save/read tickets will inherit nondeterministic model selection.

Split recommendations
- No split recommended. The ticket is already bounded to app-level registration, DbContext opt-in integration, and source-precedence validation, while downstream registry consumers are already split into 06F0MEBFTW8FY5T7PY5HJ5JXJ4, 06F0MECFNF42NK9PND9DWVW9VW, and 06F0MECPFAVBFBNC5XMVDZRQ6M.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment