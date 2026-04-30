[gicket-bot] PO refinement contract

Summary
- Restated the ticket contract to remove unsupported inferred API/type claims, anchor `ModelBuilder.UseDataVault()` to current branch evidence, and let this ticket add the explicit EF translation entry point and any minimal missing public surface it needs; no child tickets, relations, attachments, or planning documents were created.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract no longer assumes an existing explicit DVault-to-EF translation API or a fully proven existing public input type set. It now anchors only the visible baseline from the current branch and explicitly allows this ticket to add the minimal translation entry point and any missing public input or annotation surface needed for hubs, links, satellites, references, or role markers.
- critic-item-2: `answered` - The revised contract no longer infers a separate existing public translation API or type from the current branch. Any missing explicit translation entry point or missing public hub, link, satellite, reference, or annotation type is treated as part of this ticket's bounded deliverable instead of being assumed to preexist.
- critic-item-3: `answered` - The prior unsupported acceptance criterion is replaced. Source-backed current behavior is that `ModelBuilder.UseDataVault()` only sets the `DCoding.Data.DVault:Conventions` annotation and returns the same builder; the revised contract requires a separate explicit translation entry point to be added by this ticket before any DVault entity, property, key, or index metadata is created.

Clarifications
- Relation evidence keeps 06EXB7FF1J9NR2849WKDR8DKPG as the parent story, 06EXB7FPZRCFC33RF2M5SXZTK4 as an upstream blocker of this ticket, and 06EXB7GESWZZTZG7XYAKTTKQRW plus 06EXB7J6HCA9QZ3DPP5Z03YGJ0 as downstream tickets blocked by this provider-neutral EF metadata foundation.
- Current branch evidence already shows a root EF convention marker at `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` where `ModelBuilder.UseDataVault()` sets the `DCoding.Data.DVault:Conventions` annotation and returns the same `ModelBuilder`.
- Current branch evidence also shows the provider-neutral modeling area under `src/DCoding.Data.DVault/Modeling`, deterministic naming policy code/tests under `src/DCoding.Data.DVault/Modeling` and `tests/DCoding.Data.DVault.Tests/Modeling/DefaultNamingPolicyTests.cs`, and a reusable technical metadata contract surface under `src/DCoding.Data.DVault/TechnicalMetadataColumnContract.cs` with tests in `tests/DCoding.Data.DVault.Tests/TechnicalMetadataColumnContractTests.cs`.
- No child tickets, relations, attachments, or planning documents were created in this refinement pass.

Scope In
- Add an explicit provider-neutral DVault-to-EF translation entry point on top of the existing `ModelBuilder.UseDataVault()` conventions marker.
- Translate the current provider-neutral DVault modeling surface under `src/DCoding.Data.DVault/Modeling` into provider-neutral EF Core model metadata; if one required public input or annotation type is missing, add the minimal documented public surface in this ticket.
- Materialize one explicit EF entity metadata definition per hub, link, and satellite described by the translation input.
- Materialize provider-neutral property metadata for business-key columns, link participant hash-key references, satellite payload columns, and required technical metadata columns.
- Materialize provider-neutral primary-key and secondary-index metadata that matches the current deterministic naming and composition baseline.
- Add direct EF model inspection tests under the existing `tests/DCoding.Data.DVault.Tests` layout to prove deterministic translation and explicit opt-in behavior.

Scope Out
- Sqlite or Postgres relational annotations, table creation, schema generation, migrations, store types, or provider-specific index and constraint naming behavior.
- Provider capability abstractions, provider-specific branching, or readiness work owned by 06EXB7J6HCA9QZ3DPP5Z03YGJ0.
- Foreign-key constraints, navigation graph behavior, or other relationship metadata not already required for the provider-neutral baseline.
- New public CLR row types, `DbSet` runtime APIs, ingestion flows, or record-loading behavior unless a minimal public input or annotation type is required to complete the explicit translation surface.
- Advanced configuration surfaces or overloads for naming, hashing, record source, timestamp, or provider customization beyond the current convention-first defaults.

Open questions
- none

Follow-up questions
- Downstream provider-specific work in 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7J6HCA9QZ3DPP5Z03YGJ0 should decide whether relational table, column, index, and constraint names are stored as DVault-owned annotations or regenerated from a shared naming projector, as long as the result stays deterministic and aligned with current naming tests.
- A later advanced-configuration ticket can decide whether the EF metadata translation path needs user-supplied naming, hashing, record-source, timestamp, or provider overrides beyond the current convention-first defaults.
- A later documentation/examples ticket can add end-to-end `DbContext.OnModelCreating` usage examples after this provider-neutral translation path and downstream provider mapping work land.

Risks
- If EF translation reimplements naming or key/index composition separately from the current naming baseline and tests, provider-neutral EF metadata can drift from the repository's visible deterministic v1 behavior.
- If the explicit translation path adds broader public API than the minimal surface needed for hubs, links, satellites, references, and role markers, the ticket can leak advanced configuration scope that belongs in later work.
- If provider-neutral concept and role markers are not carried on the EF model, downstream tickets 06EXB7GESWZZTZG7XYAKTTKQRW and 06EXB7J6HCA9QZ3DPP5Z03YGJ0 may be forced to infer semantics from names and duplicate brittle logic.

Split recommendations
- No additional split is recommended; relation evidence already isolates this provider-neutral EF metadata foundation from downstream Sqlite mapping in 06EXB7GESWZZTZG7XYAKTTKQRW and provider-support work in 06EXB7J6HCA9QZ3DPP5Z03YGJ0.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 8

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment