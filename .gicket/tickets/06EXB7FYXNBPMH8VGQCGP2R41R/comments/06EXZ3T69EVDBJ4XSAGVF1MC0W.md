[gicket-bot] PO refinement contract

Summary
- Replaced inferred EF API assumptions with source-backed branch evidence: the current branch only exposes `ModelBuilder.UseDataVault()` as a conventions marker plus provider-neutral modeling and metadata types under `src/DCoding.Data.DVault/Modeling`, so this ticket may introduce the explicit EF translation entry point and any minimal supporting public surface; no child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract now cites only source-backed current-branch APIs and types: root `ModelBuilder.UseDataVault()` as a conventions-only EF extension, provider-neutral metadata types `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference`, plus the existing naming/modeling baseline in `DataVaultModel`, `DataVaultModelBuilder`, `DefaultNamingPolicy`, and `TechnicalMetadataColumnContract`. Any missing explicit EF translation API or annotation type is treated as work this ticket may add, not as an existing branch guarantee.
- critic-item-2: `answered` - The contract no longer infers an existing public EF translation API. Repository evidence shows only two `UseDataVault` extensions today: the root EF `ModelBuilder` extension and the separate `Modeling.DataVaultModelBuilder` extension. The explicit EF translation entry point is therefore stated as new scope for this ticket rather than an already-existing branch surface.
- critic-item-3: `answered` - Unsupported inferred API claims were replaced with branch-backed behavior. The contract now anchors current EF evidence to `ModelBuilder.UseDataVault()` as a conventions marker, anchors current provider-neutral metadata inputs to the visible modeling types, and anchors deterministic naming and key/index composition to the existing naming tests. The missing explicit EF translation surface is explicitly left for this ticket to create.

Clarifications
- Relation evidence remains unchanged: 06EXB7FF1J9NR2849WKDR8DKPG is the parent story, 06EXB7FPZRCFC33RF2M5SXZTK4 blocks this ticket, and this ticket blocks 06EXB7GESWZZTZG7XYAKTTKQRW plus 06EXB7J6HCA9QZ3DPP5Z03YGJ0.
- Current branch evidence shows the root EF surface at `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs`; `tests/DCoding.Data.DVault.Tests/Unit/DataVaultModelBuilderExtensionsTests.cs` proves bare `ModelBuilder.UseDataVault()` only records `DCoding.Data.DVault:Conventions` and creates no entity types.
- Current branch evidence also shows source-backed provider-neutral DVault input surfaces under `src/DCoding.Data.DVault/Modeling`: `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference` in `DataVaultMetadata.cs`, plus `DataVaultModel.Create(...)` and `DataVaultModelBuilder` in `DataVaultModel.cs`.
- `tests/DCoding.Data.DVault.Tests/Modeling/NamingPolicyTests.cs` fixes the visible v1 deterministic baseline today: `HubCustomer`, `SatCustomerContact`, `LinkCustomerOrder`, declared-order business keys and link participants, one unique business-key index for hubs, one non-unique relationship index for links, one non-unique parent index for satellites, and the current primary-key compositions.
- `src/DCoding.Data.DVault/TechnicalMetadataColumnContract.cs`, `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultMetadataTests.cs` fix the reusable technical-column role baseline to `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass.

Scope In
- Introduce one explicit EF translation entry point on top of the existing root `ModelBuilder.UseDataVault()` conventions marker; no preexisting EF translation API is assumed to exist on the branch.
- Translate the current provider-neutral DVault metadata declarations `DataVaultHubMetadata`, `DataVaultLinkMetadata`, `DataVaultSatelliteMetadata`, and `DataVaultMetadataReference` into provider-neutral EF model metadata; if an aggregate input type or EF annotation identifier is still missing, add only the minimal documented public surface required.
- Reuse the current deterministic naming and structural baseline from `DataVaultModel`, `DataVaultModelBuilder`, `DefaultNamingPolicy`, and `DefaultDataVaultNamingPolicy` so hub, link, and satellite names plus key/index compositions match visible v1 behavior.
- Materialize one EF entity metadata definition per hub, link, and satellite declaration, with provider-neutral role markers sufficient for direct model inspection to distinguish entity kind and column roles.
- Materialize provider-neutral property metadata for business-key columns, link participant hash-key references, satellite payload columns, and the required technical metadata columns defined by the current branch contracts.
- Materialize provider-neutral primary keys and secondary indexes only, and add direct EF model inspection tests under the existing `tests/DCoding.Data.DVault.Tests` layout.

Scope Out
- Sqlite or Postgres relational annotations, table creation, schema generation, migrations, store types, or provider-specific index and constraint naming behavior.
- Provider capability abstractions, provider-specific branching, or readiness work owned by 06EXB7J6HCA9QZ3DPP5Z03YGJ0.
- Foreign-key constraints, navigation graph behavior, or other relationship metadata outside the current provider-neutral baseline.
- New public CLR row types, `DbSet` runtime APIs, ingestion flows, or record-loading behavior unless a minimal aggregate input or annotation type is required to complete the explicit translation surface.
- Advanced configuration surfaces or overloads for naming, hashing, record source, timestamp, or provider customization beyond the current convention-first defaults.

Open questions
- none

Follow-up questions
- Downstream provider-specific work in 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7J6HCA9QZ3DPP5Z03YGJ0 should decide whether relational table, column, index, and constraint names are stored as DVault-owned annotations or regenerated from a shared naming projector, as long as the result stays deterministic and aligned with the current naming tests.
- A later advanced-configuration ticket can decide whether the EF metadata translation path needs user-supplied naming, hashing, record-source, timestamp, or provider overrides beyond the current convention-first defaults.
- A later documentation or examples ticket can add end-to-end `DbContext.OnModelCreating` usage examples after this provider-neutral translation path and downstream provider mapping work land.

Risks
- If EF translation reimplements naming or key and index composition separately from the current `DataVaultModel` and naming-test baseline, provider-neutral EF metadata can drift from the repository's visible deterministic v1 behavior.
- If the explicit translation path adds broader public API than the minimal aggregate input or annotation surface needed for hubs, links, satellites, references, and role markers, the ticket can leak advanced configuration scope that belongs in later work.
- If provider-neutral entity-kind and column-role markers are not carried on the EF model and the implementation falls back to inferring semantics only from generated names, downstream tickets 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7J6HCA9QZ3DPP5Z03YGJ0 may be forced to duplicate brittle logic.

Split recommendations
- No additional split is recommended; current relation evidence already isolates this provider-neutral EF metadata foundation from downstream Sqlite mapping in 06EXB7GESWZZTZG7XYAKTTKQRW and provider-support work in 06EXB7J6HCA9QZ3DPP5Z03YGJ0.

Persisted contract coverage
- acceptance-criteria items: 9
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment