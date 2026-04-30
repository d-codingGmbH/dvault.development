[gicket-bot] PO refinement contract

Summary
- Restated the contract to use only source-backed branch evidence: visible EF behavior is the conventions-only `ModelBuilder.UseDataVault()` marker, visible inputs are the DVault metadata contracts in `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, and the ticket may create the explicit EF translation entry point plus any minimal supporting public surface if missing. No child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - Replaced inferred existing-API claims with source-backed evidence. `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` shows `ModelBuilder.UseDataVault()` as the visible EF entry point, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs` proves a bare call stores `DCoding.Data.DVault:Conventions` and creates no entity types. The contract now states that this ticket may add the explicit EF translation entry point and any minimal supporting public API if that surface is missing.
- critic-item-2: `answered` - The revised contract no longer assumes any preexisting public EF translation API or EF-specific aggregate type. It anchors the visible input surface to `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference` in `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, and it explicitly allows this ticket to add the translation entry point, minimal aggregate input, annotation identifiers, or equivalent provider-neutral markers if those EF-facing pieces are not already present.
- critic-item-3: `answered` - The unsupported inferred EF API claim is replaced with bounded evidence and explicit creation scope. The visible branch evidence supports the conventions-only EF entry point, the provider-neutral metadata contracts, the closed technical-column role set, and the deterministic naming outputs asserted in `NamingPolicyTests`; it does not require assuming an existing EF translator. The contract now references those proven inputs and outputs and lets this ticket create the explicit EF translator surface if missing.

Clarifications
- Relation evidence is unchanged: `06EXB7FF1J9NR2849WKDR8DKPG` is the parent story, `06EXB7FPZRCFC33RF2M5SXZTK4` blocks this ticket, and this ticket blocks `06EXB7GESWZZTZG7XYAKTTKQRW` plus `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.
- Visible source-backed EF behavior is limited to `ModelBuilder.UseDataVault()` in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`; the matching unit test proves a bare call records `DCoding.Data.DVault:Conventions` and creates no entity metadata.
- Visible provider-neutral input contracts are `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference` in `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, with helper metadata classes for business keys, link participants, and satellite payload columns.
- `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` fixes the deterministic v1 baseline: `HubCustomer`, `SatCustomerContact`, `LinkCustomerOrder`, declared-order columns, one unique hub business-key index, one non-unique link relationship index, one non-unique satellite parent index, and the current primary-key naming/compositions.
- `src/DCoding.Data.DVault/TechnicalMetadataColumnContract.cs`, `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs` fix the technical-column role baseline to `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass.

Scope In
- Preserve `ModelBuilder.UseDataVault()` as the existing conventions marker and add one explicit opt-in EF translation entry point if the current branch does not already expose one.
- Translate the visible provider-neutral metadata contracts from `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` into provider-neutral EF entity, property, key, and index metadata for hubs, links, and satellites.
- Add only the minimal supporting public surface needed to make that translation inspectable, such as a small aggregate input contract or DVault-owned annotation identifiers, if those pieces are missing.
- Reuse the deterministic naming and declared-order baseline already asserted in `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` so repeated translations of equivalent input stay stable.
- Add or update direct EF model inspection tests under the existing `tests/DCoding.Data.DVault.Tests` layout.

Scope Out
- Sqlite- or Postgres-specific relational annotations, schema generation, migrations, store types, table creation, or provider-specific index and constraint naming behavior.
- Foreign-key constraints, navigations, or other relationship metadata beyond the current provider-neutral baseline.
- New public CLR row types, `DbSet` runtime APIs, ingestion flows, or record-loading behavior beyond any minimal translation-supporting contract or annotation surface introduced by this ticket.
- Advanced configuration APIs for custom naming, hashing, record-source, timestamp, or provider overrides beyond the current convention-first defaults.
- Provider capability abstraction or readiness work already separated into downstream ticket `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.

Open questions
- none

Follow-up questions
- Downstream provider-specific work in `06EXB7GESWZZTZG7XYAKTTKQRW` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0` should decide whether relational table, column, index, and constraint names are stored as DVault-owned annotations or regenerated from a shared naming projector, as long as the result stays deterministic and aligned with the current naming baseline.
- A later advanced-configuration ticket can decide whether the EF metadata translation path needs user-supplied naming, hashing, record-source, timestamp, or provider overrides beyond the current convention-first defaults.
- A later documentation or examples ticket can add end-to-end `DbContext.OnModelCreating` examples after the provider-neutral translation path and downstream provider mapping work land.

Risks
- If the EF translation reimplements naming or key and index composition instead of reusing the existing modeling and naming baseline, the generated EF metadata can drift from the visible deterministic `NamingPolicyTests` outputs.
- If this ticket introduces broader public API than the minimal translation-supporting contract or annotation surface required, it will leak advanced-configuration scope that belongs in later work.
- If explicit entity-kind and column-role markers are not carried on the EF model, downstream provider-specific tickets may be forced to infer semantics from generated names and duplicate brittle logic.

Split recommendations
- No additional split is recommended; the current relation graph already isolates this provider-neutral EF metadata foundation from downstream Sqlite mapping in `06EXB7GESWZZTZG7XYAKTTKQRW` and provider-support work in `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment