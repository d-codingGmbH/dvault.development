[gicket-bot] PO refinement contract

Summary
- Refined the contract to the landed repository surface: v0.24.0 documentation rolls up async streaming, helper convenience, and benchmark evidence, while EF safety stays guidance-only and does not require nonexistent model-cache or pooling diagnostic IDs.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

PO-critic checklist responses
- critic-item-1: `answered` - The contract is revised so this documentation ticket no longer requires model-cache or pooling diagnostic IDs to be named. The v0.24.0 doc rollup must instead describe the landed analyzer surface honestly and route EF safety readers to the existing repository guidance for model-cache isolation and fixed-model compiled or pooled usage.
- critic-item-2: `answered` - The v0.24.0 analyzer README and release notes should use guidance-only EF safety wording. They should point readers to README.md 'Isolate EF model cache entries' for caller-owned IModelCacheKeyFactory responsibilities and to docs/architecture/dvault-ef-compiled-compatibility.md for the fixed-model limits on UseModel(...) and AddDbContextPool<TContext>(...). They should not promise a separate model-cache or pooling diagnostic list.
- critic-item-3: `answered` - The impossible requirement is removed. Developers should not invent model-cache or pooling IDs because the landed EF misuse analyzer surface only covers shared-type-table misuse. EF safety for model-cache, compiled-model, and pooling boundaries is documentation guidance, not a landed analyzer catalog entry in this branch.
- critic-item-4: `answered` - This ticket now explicitly chooses the guidance-only EF safety path and does not wait for a separate diagnostic implementation. Because related ticket 06F7Y0E81P65F9HEPNN72Z0NBW is already closed as no work required and the repository still exposes only DMV1910 and DMV1911 for EF misuse, the documentation rollup should proceed by documenting the existing README and architecture guidance rather than implying pending analyzer work.

Clarifications
- v0.24.0 EF safety documentation is guidance-only and must not require new model-cache or pooling diagnostics to exist.
- The landed EF misuse analyzer surface remains DMV1910 and DMV1911 for generated shared-type-table exposure and direct generated-table writes; those IDs are not model-cache or pooling safety diagnostics.
- README.md section 'Isolate EF model cache entries' is the authoritative baseline for registry-backed UseDataVaultMetadata safety and caller-owned IModelCacheKeyFactory responsibilities when tenant, schema, naming, provider, or profile state changes the realized model shape.
- docs/architecture/dvault-ef-compiled-compatibility.md is the authoritative baseline for the fixed-model lane: UseModel(...) and AddDbContextPool<TContext>(...) are only documented as safe when one fixed realized model shape is in play.
- The landed async write surface is the additive IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...) overload plus convenience helpers over the same explicit save boundary, not a second persistence subsystem or provider-native async execution claim.

Scope In
- Update README.md, docs/production-adoption-checklist.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/performance-profiles.md, and docs/releases/v0.24.0.md to move the coordinated documentation baseline to v0.24.0.
- Document the landed async public surface exactly as implemented: IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...), SaveAsync<TSource>(...), SaveHubsAsync(...), SaveLinksAsync(...), and SaveOrdinaryHubSatellitesAsync(...).
- Document save-path selection across DataVaultBulkSaveRequest, DataVaultChunkedSaveRequest, and the async chunk-source path with preserved ordering, cancellation, caller-owned transaction behavior, and provider-neutral limits.
- Document EF safety as guidance-only: registry-backed UseDataVaultMetadata as the safe built-in baseline, caller-owned IModelCacheKeyFactory when extra discriminators affect model shape, and fixed-model-only guidance for UseModel(...) and AddDbContextPool<TContext>(...).
- Update analyzer README and v0.24.0 release notes so they describe the existing analyzer surface honestly and route EF safety readers to README.md and docs/architecture/dvault-ef-compiled-compatibility.md instead of inventing model-cache or pooling IDs.
- Update release-note and performance-guidance wording to cite the shared benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json evidence, including the customer-profile-streaming-save async-source rows and their run-context caveats.

Scope Out
- No product-code, analyzer-catalog, or benchmark-harness implementation changes.
- No new model-cache or pooling diagnostics, no reopened analyzer work, and no requirement to reopen related ticket 06F7Y0E81P65F9HEPNN72Z0NBW inside this documentation rollup.
- No provider-native async execution, ingestion platform, scheduler, background worker, or SaveChanges streaming claims.
- No runtime proof that a custom IModelCacheKeyFactory includes every caller-owned discriminator, and no new pooling or compiled-model runtime guard.
- No new benchmark artifact schema, provider-specific async matrix, package publication claim, or release automation claim beyond the existing manual publication boundary.

Open questions
- none

Follow-up questions
- If the product later wants analyzer-enforced model-cache or pooling safety, should that be a separate analyzer implementation ticket with catalog and test coverage rather than a documentation add-on?
- After the v0.24.0 doc rollup lands, do we want provider-specific tenant or schema examples for IModelCacheKeyFactory customization, or should public guidance stay provider-neutral?
- Once more benchmark evidence accumulates, should a later docs pass add chunk-size heuristics or profile-based tuning ranges beyond the initial provider-neutral selection guidance?

Risks
- If any v0.24.0 doc surface still asks for model-cache or pooling diagnostic IDs, the developer can only invent diagnostics that do not exist in the landed analyzer catalog.
- If README.md, docs/production-adoption-checklist.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/performance-profiles.md, and docs/releases/v0.24.0.md are not updated together, the repository will continue to expose conflicting v0.23.0 versus v0.24.0 baseline guidance.
- If EF safety wording blurs the line between DVault-owned registry isolation and caller-owned model discriminators, readers may over-assume compiled-model or pooled-context safety for variable model shapes.
- If async streaming prose drops the benchmark run-context caveats or the provider-neutral boundary, the docs can overstate throughput or imply provider-native async behavior that is not part of the landed surface.
- docs/performance-profiles.md already contains v0.24 async-source guidance while other public baseline documents still point to v0.23.0, so partial rollout drift is already visible in the branch snapshot.

Split recommendations
- No split is required for this ticket; keep it as the bounded v0.24.0 documentation and release-note rollup over already-landed async streaming, benchmark evidence, and EF safety guidance.
- If stakeholders later want concrete model-cache or pooling diagnostics, handle that as separate analyzer implementation work rather than expanding this documentation-only ticket.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment