<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Restated the contract to use only source-backed branch evidence: visible EF behavior is the conventions-only `ModelBuilder.UseDataVault()` marker, visible inputs are the DVault metadata contracts in `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, and the ticket may create the explicit EF translation entry point plus any minimal supporting public surface if missing. No child tickets, relations, attachments, or planning documents were created.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Relation evidence is unchanged: `06EXB7FF1J9NR2849WKDR8DKPG` is the parent story, `06EXB7FPZRCFC33RF2M5SXZTK4` blocks this ticket, and this ticket blocks `06EXB7GESWZZTZG7XYAKTTKQRW` plus `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.
- Visible source-backed EF behavior is limited to `ModelBuilder.UseDataVault()` in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`; the matching unit test proves a bare call records `DCoding.Data.DVault:Conventions` and creates no entity metadata.
- Visible provider-neutral input contracts are `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference` in `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs`, with helper metadata classes for business keys, link participants, and satellite payload columns.
- `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` fixes the deterministic v1 baseline: `HubCustomer`, `SatCustomerContact`, `LinkCustomerOrder`, declared-order columns, one unique hub business-key index, one non-unique link relationship index, one non-unique satellite parent index, and the current primary-key naming/compositions.
- `src/DCoding.Data.DVault/TechnicalMetadataColumnContract.cs`, `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs` fix the technical-column role baseline to `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass.

### Scope In
- Preserve `ModelBuilder.UseDataVault()` as the existing conventions marker and add one explicit opt-in EF translation entry point if the current branch does not already expose one.
- Translate the visible provider-neutral metadata contracts from `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` into provider-neutral EF entity, property, key, and index metadata for hubs, links, and satellites.
- Add only the minimal supporting public surface needed to make that translation inspectable, such as a small aggregate input contract or DVault-owned annotation identifiers, if those pieces are missing.
- Reuse the deterministic naming and declared-order baseline already asserted in `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` so repeated translations of equivalent input stay stable.
- Add or update direct EF model inspection tests under the existing `tests/DCoding.Data.DVault.Tests` layout.

### Scope Out
- Sqlite- or Postgres-specific relational annotations, schema generation, migrations, store types, table creation, or provider-specific index and constraint naming behavior.
- Foreign-key constraints, navigations, or other relationship metadata beyond the current provider-neutral baseline.
- New public CLR row types, `DbSet` runtime APIs, ingestion flows, or record-loading behavior beyond any minimal translation-supporting contract or annotation surface introduced by this ticket.
- Advanced configuration APIs for custom naming, hashing, record-source, timestamp, or provider overrides beyond the current convention-first defaults.
- Provider capability abstraction or readiness work already separated into downstream ticket `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.

## Acceptance Criteria
- A bare `ModelBuilder.UseDataVault()` call continues to only store the `DCoding.Data.DVault:Conventions` annotation and create no entity types; only an explicit translation API introduced or used by this ticket may create DVault entity, property, key, or index metadata.
- If the current branch lacks an EF translation entry point, this ticket introduces one explicit opt-in translation surface and any minimal supporting public API or annotation identifiers required to project the existing provider-neutral metadata contracts.
- The translation path accepts the visible provider-neutral inputs `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference` directly or through a new minimal aggregate input contract created by this ticket, without assuming any other preexisting EF-facing API.
- Translating a hub declaration produces one provider-neutral EF entity with a primary key on the hub hash-key property, declared-order business-key properties, load-timestamp and record-source properties, and a unique business-key index aligned with the visible `NamingPolicyTests` baseline.
- Translating a link declaration produces one provider-neutral EF entity with a primary key on the relationship hash-key property, declared-order participant hash-key reference properties, load-timestamp and record-source properties, and a non-unique relationship index aligned with the visible `NamingPolicyTests` baseline.
- Translating a satellite declaration produces one provider-neutral EF entity with the parent hash-key property, declared-order payload properties, `HashDiff`, `LoadTimestamp`, and `RecordSource`, plus a primary key over parent hash key and load timestamp and a non-unique parent lookup index aligned with the visible `NamingPolicyTests` baseline.
- Equivalent translation input produces the same entity, property, key, index, and role-marker shape across repeated runs, preserving declared order for business keys, link participants, and satellite descriptive attributes.
- Tests inspect the provider-neutral EF model directly and can distinguish hub, link, and satellite entity kinds plus business-key, payload, participant-reference, and technical-column roles without relying on provider-specific relational APIs.
- The default provider-neutral EF translation adds primary keys and secondary indexes only and does not introduce foreign keys or navigation requirements.

## Definition of Done
- Changes stay within `src/DCoding.Data.DVault` and `tests/DCoding.Data.DVault.Tests` and reuse the repository `net10.0` and EF Core 10 baseline from `src/DCoding.Data.DVault/DCoding.Data.DVault.csproj`.
- Any new public API surface, public constants, or public annotation identifiers introduced for the translation path include XML documentation that satisfies `GenerateDocumentationFile` and `CS1591` requirements.
- Automated tests covering hubs, links, satellites, technical columns, declared-order determinism, and explicit opt-in behavior are added or updated in the existing test layout and pass.
- `dotnet test DVault.slnx --nologo` and `bash tools/check-format.sh` pass with the new EF metadata translation coverage included.
- No provider-specific relational mapping, capability abstraction, or advanced-configuration surface is introduced as part of this ticket.

## Implementation Notes
- Treat `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` as the current EF root enabling surface and preserve the existing `DCoding.Data.DVault:Conventions` annotation behavior.
- `src/DCoding.Data.DVault/Modeling/DataVaultMetadata.cs` is the visible provider-neutral input contract surface for this ticket; `src/DCoding.Data.DVault/TechnicalMetadataColumnContract.cs` defines the reusable technical roles that the EF translation must carry forward.
- The current branch `src/DCoding.Data.DVault/Modeling` directory already contains `DataVaultConventions.cs`, `DataVaultModel.cs`, `DataVaultModelBuilder.cs`, `DataVaultModelBuilderExtensions.cs`, `DefaultDataVaultNamingPolicy.cs`, and `DefaultNamingPolicy.cs`; implementation should inspect and reuse the existing modeling and naming code rather than duplicating deterministic naming rules.
- Use the visible outputs asserted in `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` as the binding v1 baseline for entity names, technical-column names, key composition, and index composition.
- Preserve declared order for hub business keys, link participants, and satellite descriptive attributes so repeated translation runs stay deterministic.
- Keep the projection provider-neutral by using EF metadata plus DVault-owned annotations or equivalent provider-neutral markers rather than relational provider APIs, foreign keys, or navigations.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass.

## Open Questions
- none

## Follow-Up Questions
- Downstream provider-specific work in `06EXB7GESWZZTZG7XYAKTTKQRW` and `06EXB7J6HCA9QZ3DPP5Z03YGJ0` should decide whether relational table, column, index, and constraint names are stored as DVault-owned annotations or regenerated from a shared naming projector, as long as the result stays deterministic and aligned with the current naming baseline.
- A later advanced-configuration ticket can decide whether the EF metadata translation path needs user-supplied naming, hashing, record-source, timestamp, or provider overrides beyond the current convention-first defaults.
- A later documentation or examples ticket can add end-to-end `DbContext.OnModelCreating` examples after the provider-neutral translation path and downstream provider mapping work land.

## Risks
- If the EF translation reimplements naming or key and index composition instead of reusing the existing modeling and naming baseline, the generated EF metadata can drift from the visible deterministic `NamingPolicyTests` outputs.
- If this ticket introduces broader public API than the minimal translation-supporting contract or annotation surface required, it will leak advanced-configuration scope that belongs in later work.
- If explicit entity-kind and column-role markers are not carried on the EF model, downstream provider-specific tickets may be forced to infer semantics from generated names and duplicate brittle logic.

## Split Recommendations
- No additional split is recommended; the current relation graph already isolates this provider-neutral EF metadata foundation from downstream Sqlite mapping in `06EXB7GESWZZTZG7XYAKTTKQRW` and provider-support work in `06EXB7J6HCA9QZ3DPP5Z03YGJ0`.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

## Summary
Bridge provider-neutral DVault metadata into EF metadata.

## Scope
- Map hubs, links, satellites, keys, indexes, and technical columns.

## Acceptance Criteria
- Tests inspect the generated EF model.
- The mapping is deterministic.

## Definition of Done
- The work satisfies the acceptance criteria.
- Shared standards from the charter attachment are followed.