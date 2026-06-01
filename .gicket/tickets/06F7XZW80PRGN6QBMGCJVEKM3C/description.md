<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Reclassified the epic to closure-only over already-landed v0.24.0 async streaming and EF safety guidance, and explicitly neutralized the stale legacy draft that still implied new diagnostics.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- This epic is closure-only and no-new-dev-work over the already-landed v0.24.0 repository and documentation surface.
- The only authoritative scope is the delivery contract; any preserved legacy draft is archival background only and is non-binding.
- Async streaming in scope means the additive `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` overload and the async mapping helpers on the existing explicit save boundary.
- EF safety in scope is guidance-only: registry-backed `UseDataVaultMetadata(...)` isolates DVault-owned metadata sources, while caller-owned model-shape discriminators remain application `IModelCacheKeyFactory` responsibilities.
- `UseModel(...)` and `AddDbContextPool<TContext>(...)` remain fixed-model guidance only; they are not generalized safe defaults for variable realized model shapes.
- Implemented analyzer scope remains `DMV1910` and `DMV1911`; this epic does not promise new model-cache, compiled-model, or pooling diagnostics.

### Scope In
- Ratify the already-landed async chunk-source explicit-save boundary on `IDataVaultSaveService`, including the `IAsyncEnumerable<DataVaultSaveChunk>` overload and async helper methods.
- Ratify save-path guidance across `DataVaultBulkSaveRequest`, `DataVaultChunkedSaveRequest`, and async chunk-source input without changing caller-owned timestamps, ordering, cancellation, or transaction behavior.
- Ratify the settled EF safety documentation boundary: registry-backed `UseDataVaultMetadata(...)` cache-key isolation, caller-owned `IModelCacheKeyFactory` discriminators, and fixed-model-only guidance for `UseModel(...)` and `AddDbContextPool<TContext>(...)`.
- Keep README, release-note wording, analyzer README guidance, public API evidence, tests, and benchmark artifacts aligned to one v0.24.0 story.
- Archive or explicitly neutralize the stale legacy draft so it no longer reads as live scope.

### Scope Out
- New analyzer IDs or runtime guards for caller-owned model-cache, compiled-model, or pooled-context safety.
- Provider-native async writes, provider-native chunk execution, or provider-specific async performance claims.
- Any new ingestion platform, scheduler, background worker, dashboard, or automatic deployment work.
- Replacing the explicit `IDataVaultSaveService` boundary with implicit `SaveChanges` streaming or another default persistence path.
- Reopening new developer work from this epic without a separate concrete follow-on ticket.

## Acceptance Criteria
- The epic is explicitly described as closure-only and no-new-dev-work over the already-landed v0.24.0 repository surface.
- The active contract defines only two bounded slices: additive async source explicit saves and guidance-only EF safety.
- The ticket text states that `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` preserves caller chunk order, request order, explicit metadata, cancellation, and caller-owned transaction responsibility, without pre-buffering the full source or continuing in the background.
- The ticket text states that `UseDataVaultMetadata(...)` isolates DVault-owned metadata registries in the EF model cache while caller-owned shape discriminators remain application `IModelCacheKeyFactory` responsibilities.
- Compiled-model and pooling guidance is explicitly limited to one fixed realized model shape, consumer-owned `UseModel(...)` runtime-model usage, and options-only pooled contexts, with evidence bounded to the documented SQLite compatibility and benchmark baseline.
- Implemented EF misuse diagnostics are explicitly limited to `DMV1910` and `DMV1911`; the epic does not promise new model-cache, compiled-model, or pooling diagnostics.
- The legacy draft is archived or explicitly marked as superseded and non-authoritative so its former Scope In and Acceptance Criteria no longer read as live scope.
- Release and performance evidence are bounded to the v0.24.0 documentation set and the checked-in benchmark-summary triplet, including `customer-profile-streaming-save`, `compiled-model-startup`, and `dbcontext-pooling-dvault-operation` evidence rows.

## Definition of Done
- The epic text no longer implies outstanding developer work or future delivery of model-cache, compiled-model, or pooling diagnostics.
- Any preserved legacy draft is explicitly labeled archival-only and stripped of conflicting live-scope meaning.
- Ticket wording, README, analyzer README guidance, release notes, and benchmark evidence tell one consistent v0.24.0 story.
- Future expansion ideas are moved to follow-up work rather than left as blocking ambiguity on this epic.

## Implementation Notes
- Ticket text should remove the conflicting live meaning of `.gicket/tickets/06F7XZW80PRGN6QBMGCJVEKM3C/description.md:74-92`, ideally by replacing it with a one-line archival note or by deleting that legacy block from the live description.
- `docs/releases/v0.24.0.md:26-81` and `README.md:786-800` already settle the async-save and guidance-only EF-safety boundary for this epic.
- `src/DCoding.Data.DVault/DataVaultSaveService.cs`, `src/DCoding.Data.DVault/DataVaultSaveServiceAsyncExtensions.cs`, and `tests/DCoding.Data.DVault.Tests/Unit/DataVaultAsyncSaveHelperTests.cs` cover the additive async save surface and helper behavior.
- `src/DCoding.Data.DVault.Analyzers/README.md:14` and `tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs:13-29` bound implemented EF misuse diagnostics to `DMV1910` and `DMV1911`.
- `benchmark-summary.md:44`, `benchmark-summary.md:56`, and `benchmark-summary.md:60` provide the checked-in async streaming, compiled-model, and pooled-context evidence rows referenced by this epic.
- No new code, analyzer catalog entry, or runtime guard needs to be reopened from this ticket unless a separate future ticket defines that new work explicitly.

## Open Questions
- none

## Follow-Up Questions
- Should a future follow-on ticket add analyzer or runtime guardrails for variable-model compiled-model or pooled-context misuse beyond the current documentation-only boundary?
- Should a future provider-evidence ticket collect non-SQLite async/provider benchmark runs once external provider connection strings are configured?
- Should a future release add distinct public diagnostics or telemetry language for async chunk-source saves beyond the shared `ChunkedRequest` family?

## Risks
- The epic title can still be misread as promising new EF safety diagnostics unless the closure-only framing and archived legacy-draft note stay explicit.
- Optional external-provider benchmark rows remain skipped when `DVAULT_TEST_*` connection strings are unset, so performance wording must stay bounded to the checked-in SQLite/provider-neutral evidence.
- Consumers can still misuse compiled models or pooled contexts for variable realized model shapes; the current mitigation is documentation plus caller-owned cache-key design, not enforcement.

## Split Recommendations
- Keep any future provider-native async write or provider-specific async execution claims in a separate follow-on ticket.
- Keep any future model-cache, compiled-model, or pooling analyzer/runtime guardrails in a separate follow-on ticket.
- If future development work is later desired, reopen only a new concrete ticket for that deliverable instead of reopening this closure-only epic.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Plan and deliver focused EF Core library improvements for async explicit saves and model-cache safety without turning DVault into an ingestion platform.

# Scope In
- Async explicit save inputs over the existing chunked save semantics.
- Analyzer/runtime diagnostics for EF Core model-cache, compiled-model, and DbContext pooling risks caused by caller-owned dynamic DVault model shape.
- Benchmark, allocation, telemetry, docs, and release evidence.

# Scope Out
- File ingestion, CDC, schedulers, background jobs, dashboards, hosted workers, or automatic database deployment.
- Replacing the existing explicit IDataVaultSaveService boundary.

# Acceptance Criteria
- Child tickets define and implement the bounded async streaming and EF safety work.
- Documentation states how the feature relates to existing DataVaultChunkedSaveRequest and consumer-owned EF model cache responsibilities. TicketSpec