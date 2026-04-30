[gicket-bot] PO refinement contract

Summary
- Refined the story to the already-visible EF metadata translation contract and confirmed no additional split is needed because the conventions-only EF entry point and the downstream provider-neutral translation task are already separated in existing ticket work.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Repository evidence already shows the EF model-building surface exists in `src/DCoding.Data.DVault/DataVaultModelBuilderExtensions.cs` with a conventions-only `UseDataVault()` entry point and an explicit `ApplyDataVaultMetadata()` translation entry point.
- Repository evidence already shows DVault-owned provider-neutral EF annotation keys in `src/DCoding.Data.DVault/DataVaultAnnotationNames.cs`, including conventions, produced-name, entity-kind, metadata-name, parent-reference, ordinal, property-role, and technical-column-role markers.
- Repository evidence already shows the current translation path creates provider-neutral EF shared-type entities for hubs, links, and satellites from `DataVaultMetadataModel` and projects primary keys, secondary indexes, ordinals, entity kinds, metadata names, and property roles without provider-specific relational APIs.
- The visible technical-column baseline is already finite and fixed in source: `HashKey`, `HashDiff`, `LoadTimestamp`, and `RecordSource`; load-timestamp properties are projected as `DateTimeOffset`, while the other visible projected properties are string-based in the current branch.
- The current story should be treated as the story-level umbrella for the EF model-building integration slice, while the already-refined downstream tasks cover the conventions-only entry point and the provider-neutral metadata translation details; no child tickets, relations, attachments, or planning documents were created in this pass.

Scope In
- Integrate DVault with EF Core model building through the public `DCoding.Data.DVault` model-builder extensions already visible in the branch.
- Preserve the conventions-only `ModelBuilder.UseDataVault()` behavior as a provider-neutral opt-in marker that records DVault conventions on the EF model without creating entity metadata by itself.
- Translate `DataVaultMetadataModel` hub, link, and satellite declarations into provider-neutral EF metadata through the explicit translation path, including entity, property, primary-key, and secondary-index metadata.
- Carry DVault semantics on the EF model through DVault-owned annotations for entity kind, metadata names, parent references, ordinals, property roles, and technical-column roles so downstream work can inspect semantics without provider-specific APIs.
- Add or maintain direct EF model inspection tests under the existing test layout to prove deterministic projected shape and the non-DVault baseline behavior.

Scope Out
- Provider-specific relational mapping details such as store types, migrations, generated schema, SQL dialect behavior, relational annotations, or database-specific index and constraint behavior.
- Foreign keys, navigation properties, runtime loading APIs, ingestion flows, or row materialization behavior beyond provider-neutral EF metadata projection.
- Advanced configuration hooks or overloads for custom naming, hashing, record-source, timestamp, or provider behavior beyond the current convention-first defaults.
- Changes to the non-EF modeling pipeline beyond reusing its established naming, metadata, and deterministic-baseline contracts where needed.
- New ticket splits for this story; the visible downstream task structure is already sufficient for the bounded scope.

Open questions
- none

Follow-up questions
- Downstream provider-specific work should decide whether physical relational names are consumed from DVault-owned annotations or regenerated from the shared naming policy, as long as the mapping stays deterministic and reversible.
- A later advanced-configuration ticket can decide whether the EF integration path needs customization hooks for naming, hashing, record source, timestamp handling, or provider behavior after the default path is complete.
- A later documentation ticket can add end-to-end `DbContext.OnModelCreating` examples that show both the conventions-only marker and metadata translation usage.

Risks
- If implementation or tests drift from the established naming-policy baseline, EF metadata shape can become inconsistent with the existing deterministic model contracts.
- If downstream provider-specific work relies on inferred names instead of the explicit DVault-owned annotations, semantics may become brittle across future naming changes.
- Because this story sits above already-refined EF tasks, description drift between the story and those downstream contracts is the main planning risk; the story should remain an umbrella over the existing bounded task split rather than reopening scope.

Split recommendations
- No additional split is recommended; the current story is already bounded by the visible separation between the conventions-only EF entry-point task and the provider-neutral EF metadata translation task.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 6
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment