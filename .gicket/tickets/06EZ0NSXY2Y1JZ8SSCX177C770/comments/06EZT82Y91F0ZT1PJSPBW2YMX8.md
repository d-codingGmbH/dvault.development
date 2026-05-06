[gicket-bot] PO refinement contract

Summary
- Clarified that this story is canonically anchored on `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits`, explicitly scoped the older `PointInTime` API out of this story, and required docs to call out the naming split; no new child tickets, relation changes, or planning documents were materialized.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The canonical public surface for this story's minimal example and acceptance boundary is the new translator-facing PIT path only: `DataVaultPitMetadata`, ordered `DataVaultPitSatelliteReferenceMetadata`, and `DataVaultMetadataModel.Pits`, as consumed by `ApplyDataVaultMetadata`. The older public `PointInTime` path is not part of this story's canonical example or acceptance boundary.
- critic-item-2: `answered` - The older public `DataVaultPointInTimeMetadata` / `DataVaultModelBuilder.PointInTime(...)` surface is explicitly out of scope for this story. Clarifications, Scope Out, and documentation acceptance now require the docs to say that this story's example uses only the `DataVaultPitMetadata` path and does not reconcile the older surface.
- critic-item-3: `answered` - No same-ticket coexistence reconciliation was added for the older surface because this refinement explicitly narrows the story to the `DataVaultPitMetadata` path and requires docs to call out the older `PointInTime` API as separate and unchanged. Any consolidation, deprecation, or formal coexistence contract remains a later API-shape follow-up rather than part of this story's acceptance boundary.
- critic-item-4: `answered` - The story's minimal example and developer scope are now anchored only on the new `DataVaultPitMetadata` EF-translation path. The older `PointInTime` / `DataVaultPointInTimeMetadata` path is not the canonical example for this ticket.
- critic-item-5: `answered` - Docs and examples for this story must use only the EF PIT naming `[<Hub>HashKey, LoadTimestamp, <Satellite>LoadTimestamp...]` and must explicitly note that the older `PointInTime` surface remains separate and keeps its own `PitLoadTimestamp` naming semantics, which this ticket does not change.

Clarifications
- The canonical public surface for this story's minimal example, developer scope, and acceptance boundary is the translator-facing PIT path: `DataVaultPitMetadata`, ordered `DataVaultPitSatelliteReferenceMetadata`, and `DataVaultMetadataModel.Pits`, as consumed by `ApplyDataVaultMetadata`.
- The older public `DataVaultPointInTimeMetadata` / `DataVaultModelBuilder.PointInTime(...)` surface is not the acceptance boundary for this story and is not reconciled, renamed, or deprecated here.
- Story documentation and examples must use only the canonical `DataVaultPitMetadata` path and must explicitly call out that the repository still contains the older `PointInTime` surface as a separate public API outside this ticket's scope.
- Within this story, PIT column naming examples are `[<Hub>HashKey, LoadTimestamp, <Satellite>LoadTimestamp...]`; `PitLoadTimestamp` belongs to the older `PointInTime` surface and is not the canonical story example.

Scope In
- Anchor the story's PIT modeling example, documentation, tests, and acceptance boundary on `DataVaultMetadataModel.Pits` and `DataVaultPitMetadata`.
- Add or ratify provider-neutral PIT metadata declarations for one hub plus ordered satellite snapshot references.
- Translate PIT metadata through `ApplyDataVaultMetadata` into provider-aware EF shared-type model metadata.
- Generate deterministic PIT table, column, and primary-key names using existing naming conventions and satellite declaration order.
- Map PIT snapshot timestamp columns through existing provider capability profiles as provider-neutral snapshot-reference properties.
- Document the supported PIT baseline, limitations, canonical example, and explicit separation from the older `PointInTime` surface.

Scope Out
- Automatic PIT population, refresh scheduling, recomputation, or maintenance through `IDataVaultSaveService` or any other write path.
- Persisted-versus-computed strategy variants beyond the single generated-table baseline.
- Link-based PITs, PITs over link-attached satellites, and PITs involving multi-active satellite semantics.
- Provider-specific SQL, migrations, extra indexes, concurrency behavior, or performance tuning beyond current provider-capability metadata mapping.
- Changes to the zero-configuration startup path or to existing hub, link, and satellite contracts just to support PIT.
- Reconciling, renaming, deprecating, or otherwise changing `DataVaultPointInTimeMetadata`, `DataVaultModelBuilder.PointInTime(...)`, `DataVaultModel`, or the older `PitLoadTimestamp` naming semantics just to align them with the new PIT translation path.

Open questions
- none

Follow-up questions
- Should a later API-shape ticket consolidate, deprecate, or formally document coexistence between `DataVaultPointInTimeMetadata` / `PointInTime(...)` and `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits`?
- If PIT moves beyond metadata generation, should the next phase standardize persisted refresh tables, computed query projections, or both?
- What temporal-grain and late-arriving-data reconciliation rules should future PIT population automation use?
- After the metadata baseline is proven, do any providers need dedicated physical indexes or other read-optimization work for PIT tables?
- Should link-based PIT support and multi-active PIT support remain separate deferred tickets rather than being added to this baseline story?

Risks
- If docs or examples mix `LoadTimestamp` and `PitLoadTimestamp`, consumers may assume the two public PIT surfaces are interchangeable even though this story intentionally treats them as separate.
- Because this baseline is metadata-only, consumers may assume PIT rows are automatically maintained unless the docs explicitly say population and refresh are deferred.
- The no-relationship, no-secondary-index baseline may be functionally correct but still insufficient for real read workloads until later optimization tickets land.
- Users may over-assume PIT coverage unless the ticket and docs explicitly call out that link-based and multi-active scenarios are unsupported in this story.

Split recommendations
- Keep PIT metadata projection, canonical `DataVaultPitMetadata` examples, and documentation in this story, but reserve PIT row population or refresh orchestration for a separate follow-up ticket.
- If public PIT API cleanup becomes material, split consolidation or deprecation of `DataVaultPointInTimeMetadata` / `PointInTime(...)` versus `DataVaultPitMetadata` / `DataVaultMetadataModel.Pits` into its own public-surface ticket rather than expanding this story.
- Handle provider-specific PIT indexing or physical optimization in provider-owned follow-up tickets once the shared metadata baseline is stable.

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