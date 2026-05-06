[gicket-bot] PO refinement contract

Summary
- Refined the story to a bounded, opt-in PIT baseline: hub-only `DataVaultPitMetadata` over attached non-multi-active satellites, provider-neutral EF projection, SQLite queryability proof, and explicit documentation of deferred PIT automation.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This story covers the opt-in PIT baseline only; ordinary hub, link, and satellite modeling and persistence remain unchanged unless PIT metadata is explicitly declared.
- The supported v1 PIT shape is exactly one declared hub parent plus one or more unique satellites already attached to that same hub.
- The generated PIT output is a shared-type EF table/entity projection with the parent hash key, a PIT `LoadTimestamp`, and one `<SatelliteName>LoadTimestamp` snapshot reference column per declared satellite in declaration order.
- Baseline PIT generation is metadata/schema projection only; it does not add PIT refresh automation, save-service behavior, computed views, or late-arriving-data reconciliation.
- The baseline does not create EF relationships or navigations for PIT tables and does not commit to provider-specific indexing or optimization beyond existing capability-profile mappings.

Scope In
- Add or ratify provider-neutral PIT metadata declarations for one hub plus ordered satellite snapshot references.
- Translate PIT metadata through `ApplyDataVaultMetadata` into provider-aware EF shared-type model metadata.
- Generate deterministic PIT table, column, and primary-key names using existing naming conventions and declaration order.
- Map PIT snapshot timestamp columns through existing provider capability profiles as provider-neutral snapshot-reference properties.
- Fail deterministically for unsupported baseline cases such as empty satellite sets, duplicate satellites, missing hub or satellite declarations, wrong-parent satellites, link-based PIT parents, and multi-active satellite references.
- Document the supported PIT baseline, limitations, and a minimal usage example.

Scope Out
- Automatic PIT population, refresh scheduling, recomputation, or maintenance through `IDataVaultSaveService` or any other write path.
- Persisted-versus-computed strategy variants beyond the single generated-table baseline.
- Link-based PITs, PITs over link-attached satellites, and PITs involving multi-active satellite semantics.
- Provider-specific SQL, migrations, extra indexes, concurrency behavior, or performance tuning beyond current provider-capability metadata mapping.
- Changes to the zero-configuration startup path or to existing hub, link, and satellite contracts just to support PIT.

Open questions
- none

Follow-up questions
- Should a later API-shape ticket consolidate or deprecate the older `DataVaultPointInTimeMetadata` abstraction so PIT has one public modeling story?
- If PIT moves beyond metadata generation, should the next phase standardize persisted refresh tables, computed query projections, or both?
- What temporal-grain and late-arriving-data reconciliation rules should future PIT population automation use?
- After the metadata baseline is proven, do any providers need dedicated physical indexes or other read-optimization work for PIT tables?
- Should link-based PIT support and multi-active PIT support remain separate deferred tickets rather than being added to this baseline story?

Risks
- The repository currently contains both `DataVaultPointInTimeMetadata` and `DataVaultPitMetadata`, so documentation must clearly identify which surface this story owns to avoid API confusion.
- Because this baseline is metadata-only, consumers may assume PIT rows are automatically maintained unless the docs explicitly say population and refresh are deferred.
- The no-relationship, no-secondary-index baseline may be functionally correct but still insufficient for real read workloads until later optimization tickets land.
- Users may over-assume PIT coverage unless the ticket and docs explicitly call out that link-based and multi-active scenarios are unsupported in this story.

Split recommendations
- Keep PIT metadata projection and documentation in this story, but reserve PIT row population or refresh orchestration for a separate follow-up ticket.
- Handle provider-specific PIT indexing or physical optimization in provider-owned follow-up tickets once the shared metadata baseline is stable.
- If API cleanup becomes material, split consolidation of `DataVaultPointInTimeMetadata` versus `DataVaultPitMetadata` into its own public-surface ticket rather than expanding this story.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 5
- implementation-notes items: 5

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment