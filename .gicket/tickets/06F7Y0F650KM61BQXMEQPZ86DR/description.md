<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the contract to the landed repository surface: v0.24.0 documentation rolls up async streaming, helper convenience, and benchmark evidence, while EF safety stays guidance-only and does not require nonexistent model-cache or pooling diagnostic IDs.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- v0.24.0 EF safety documentation is guidance-only and must not require new model-cache or pooling diagnostics to exist.
- The landed EF misuse analyzer surface remains DMV1910 and DMV1911 for generated shared-type-table exposure and direct generated-table writes; those IDs are not model-cache or pooling safety diagnostics.
- README.md section 'Isolate EF model cache entries' is the authoritative baseline for registry-backed UseDataVaultMetadata safety and caller-owned IModelCacheKeyFactory responsibilities when tenant, schema, naming, provider, or profile state changes the realized model shape.
- docs/architecture/dvault-ef-compiled-compatibility.md is the authoritative baseline for the fixed-model lane: UseModel(...) and AddDbContextPool<TContext>(...) are only documented as safe when one fixed realized model shape is in play.
- The landed async write surface is the additive IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...) overload plus convenience helpers over the same explicit save boundary, not a second persistence subsystem or provider-native async execution claim.

### Scope In
- Update README.md, docs/production-adoption-checklist.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/performance-profiles.md, and docs/releases/v0.24.0.md to move the coordinated documentation baseline to v0.24.0.
- Document the landed async public surface exactly as implemented: IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...), SaveAsync<TSource>(...), SaveHubsAsync(...), SaveLinksAsync(...), and SaveOrdinaryHubSatellitesAsync(...).
- Document save-path selection across DataVaultBulkSaveRequest, DataVaultChunkedSaveRequest, and the async chunk-source path with preserved ordering, cancellation, caller-owned transaction behavior, and provider-neutral limits.
- Document EF safety as guidance-only: registry-backed UseDataVaultMetadata as the safe built-in baseline, caller-owned IModelCacheKeyFactory when extra discriminators affect model shape, and fixed-model-only guidance for UseModel(...) and AddDbContextPool<TContext>(...).
- Update analyzer README and v0.24.0 release notes so they describe the existing analyzer surface honestly and route EF safety readers to README.md and docs/architecture/dvault-ef-compiled-compatibility.md instead of inventing model-cache or pooling IDs.
- Update release-note and performance-guidance wording to cite the shared benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json evidence, including the customer-profile-streaming-save async-source rows and their run-context caveats.

### Scope Out
- No product-code, analyzer-catalog, or benchmark-harness implementation changes.
- No new model-cache or pooling diagnostics, no reopened analyzer work, and no requirement to reopen related ticket 06F7Y0E81P65F9HEPNN72Z0NBW inside this documentation rollup.
- No provider-native async execution, ingestion platform, scheduler, background worker, or SaveChanges streaming claims.
- No runtime proof that a custom IModelCacheKeyFactory includes every caller-owned discriminator, and no new pooling or compiled-model runtime guard.
- No new benchmark artifact schema, provider-specific async matrix, package publication claim, or release automation claim beyond the existing manual publication boundary.

## Acceptance Criteria
- README.md, docs/production-adoption-checklist.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/performance-profiles.md, and docs/releases/v0.24.0.md tell one consistent story that v0.24.0 is the current coordinated documentation baseline for async streaming and EF safety guidance.
- The updated docs name the landed async APIs exactly as implemented and explain them as additive convenience over the existing explicit IDataVaultSaveService boundary rather than a second persistence abstraction.
- The updated docs explain when callers should keep a fully materialized DataVaultBulkSaveRequest, when bounded DataVaultChunkedSaveRequest is the right fit, and when the IAsyncEnumerable<DataVaultSaveChunk> path is preferable for already-asynchronous chunk producers, without implying provider-native async writes or background continuation.
- The updated docs explain the settled EF safety boundary: registry-backed UseDataVaultMetadata is safe by default, extra caller-owned model-shape discriminators belong in an application-owned IModelCacheKeyFactory, and UseModel(...) plus AddDbContextPool<TContext>(...) are only documented as safe for one fixed realized model shape.
- The analyzer README and v0.24.0 release notes do not require, invent, or imply model-cache or pooling diagnostic IDs; they route EF safety readers to README.md 'Isolate EF model cache entries' and docs/architecture/dvault-ef-compiled-compatibility.md.
- Where analyzer IDs are named, the docs use only implemented repository IDs and distinguish DMV1910 and DMV1911 shared-type-table misuse guidance from the separate guidance-only model-cache, compiled-model, and pooling posture.
- Performance guidance and release notes cite the shared benchmark-summary.md, benchmark-summary.csv, and benchmark-summary.json evidence, preserve the recorded run-context caveats, and describe the customer-profile-streaming-save async-source rows as provider-neutral bounded streaming evidence.
- The v0.24.0 release notes include coordinated package scope, async API additions, compatibility posture, validation evidence, benchmark evidence references, and non-goals while preserving the manual publication boundary.

## Definition of Done
- The targeted documentation files are updated and internally consistent on terminology, version baseline, links, and EF safety wording.
- docs/releases/v0.24.0.md exists as the new current release-note baseline, and older release notes are referenced only as historical feature-introduction records where needed.
- The async save guidance, helper naming, benchmark evidence wording, and EF safety examples or links match the already-landed repository surface and do not reopen analyzer or runtime design work.
- Documentation text does not mislabel DMV1910 or DMV1911 as model-cache or pooling diagnostics and does not invent any new diagnostic IDs.
- No PO-level ambiguity remains about the documentation-only boundary for this ticket, and no additional split is required before PO-critic review.

## Implementation Notes
- Use docs/releases/v0.23.0.md as the immediate structure baseline for package scope, compatibility posture, validation evidence, benchmark references, and non-goals, then move current-baseline references in README.md and docs/production-adoption-checklist.md to v0.24.0.
- Use src/DCoding.Data.DVault/DataVaultSaveService.cs, src/DCoding.Data.DVault/DataVaultSaveServiceAsyncExtensions.cs, src/DCoding.Data.DVault/DataVaultSaveServiceTypedExtensions.cs, and tests/DCoding.Data.DVault.Tests/Unit/Snapshots/PublicApi/DCoding.Data.DVault.approved.txt as the naming baseline for the async overload and helper methods; do not guess helper names.
- Reuse docs/architecture/dvault-v1-streaming-explicit-save-contract.md and docs/performance-profiles.md for the landed async-source wording: sequential one-pass async chunk enumeration, bounded chunking, no background continuation, and no provider-native async claim.
- Reuse README.md section 'Isolate EF model cache entries' for model-cache-key customization guidance and docs/architecture/dvault-ef-compiled-compatibility.md for UseModel(...) and AddDbContextPool<TContext>(...) fixed-model guardrails.
- Use src/DCoding.Data.DVault.Analyzers/README.md, src/DCoding.Data.DVault.Analyzers/EfCoreMisuseDiagnosticCatalog.cs, and tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs as the authoritative baseline for any analyzer-ID references; the EF misuse slice lands DMV1910 and DMV1911 only.
- Keep benchmark citations tied to the root benchmark-summary triplet and the shared performance-evidence contract; the current branch already contains v0.24 async-source wording in docs/performance-profiles.md, so the remaining documentation surfaces should be aligned to that settled wording rather than redefining it.
- Update analyzer package version examples and coordinated release references from 0.23.0 to 0.24.0 where appropriate while preserving the seven-package family boundary and PrivateAssets=all guidance.

## Open Questions
- none

## Follow-Up Questions
- If the product later wants analyzer-enforced model-cache or pooling safety, should that be a separate analyzer implementation ticket with catalog and test coverage rather than a documentation add-on?
- After the v0.24.0 doc rollup lands, do we want provider-specific tenant or schema examples for IModelCacheKeyFactory customization, or should public guidance stay provider-neutral?
- Once more benchmark evidence accumulates, should a later docs pass add chunk-size heuristics or profile-based tuning ranges beyond the initial provider-neutral selection guidance?

## Risks
- If any v0.24.0 doc surface still asks for model-cache or pooling diagnostic IDs, the developer can only invent diagnostics that do not exist in the landed analyzer catalog.
- If README.md, docs/production-adoption-checklist.md, src/DCoding.Data.DVault.Analyzers/README.md, docs/performance-profiles.md, and docs/releases/v0.24.0.md are not updated together, the repository will continue to expose conflicting v0.23.0 versus v0.24.0 baseline guidance.
- If EF safety wording blurs the line between DVault-owned registry isolation and caller-owned model discriminators, readers may over-assume compiled-model or pooled-context safety for variable model shapes.
- If async streaming prose drops the benchmark run-context caveats or the provider-neutral boundary, the docs can overstate throughput or imply provider-native async behavior that is not part of the landed surface.
- docs/performance-profiles.md already contains v0.24 async-source guidance while other public baseline documents still point to v0.23.0, so partial rollout drift is already visible in the branch snapshot.

## Split Recommendations
- No split is required for this ticket; keep it as the bounded v0.24.0 documentation and release-note rollup over already-landed async streaming, benchmark evidence, and EF safety guidance.
- If stakeholders later want concrete model-cache or pooling diagnostics, handle that as separate analyzer implementation work rather than expanding this documentation-only ticket.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Document the v0.24 release surface and adoption guidance.

# Scope In
- Update README, production checklist, analyzer README, performance guidance, and release notes.
- Explain when to use async streaming, materialized bulk, or synchronous chunked saves.
- Include safe IModelCacheKeyFactory/DbContext pooling examples where applicable.

# Acceptance Criteria
- Docs preserve the EF Core library boundary and exclude platform ingestion responsibilities.
- Release notes include API additions, analyzer IDs, compatibility posture, validation, and benchmark evidence.