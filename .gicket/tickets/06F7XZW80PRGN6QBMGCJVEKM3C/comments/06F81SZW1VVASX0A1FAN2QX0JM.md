[gicket-bot] PO refinement contract

Summary
- Refined the epic to match the landed v0.24.0 boundary: async source saves are additive on the existing explicit save service, and EF safety is a bounded guidance/documentation slice rather than new model-cache or pooling diagnostics.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- This epic is a tracking/refinement boundary over the already-landed v0.24.0 repository surface, not a request for a new ingestion subsystem or a replacement for IDataVaultSaveService.
- Async streaming in scope means the additive SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...) overload plus async helper methods over the existing explicit save contract and chunk semantics.
- EF safety in scope is guidance-only for v0.24.0: registry-backed UseDataVaultMetadata(...) isolates DVault-owned metadata sources in the EF model cache, while caller-owned model-shape discriminators remain application IModelCacheKeyFactory responsibilities.
- UseModel(...) and AddDbContextPool<TContext>(...) are only within this epic as fixed-realized-model guidance backed by SQLite compatibility and benchmark evidence; they are not generalized safe defaults for variable model shapes.
- Implemented analyzer scope remains DMV1910 and DMV1911 for generated shared-type-table misuse; this epic does not add new model-cache, compiled-model, or pooling diagnostic IDs.
- Benchmark and release evidence are bounded to the checked-in benchmark-summary triplet and v0.24.0 documentation set; optional external-provider rows may remain skipped when the DVAULT_TEST_* connection-string environment variables are unset.

Scope In
- Coordinate the additive async source explicit-save boundary on IDataVaultSaveService, including the IAsyncEnumerable<DataVaultSaveChunk> overload and async mapping helpers.
- Document save-path selection across DataVaultBulkSaveRequest, DataVaultChunkedSaveRequest, and async chunk-source input without changing explicit timestamps, record sources, ordering, cancellation, or caller-owned transaction behavior.
- Ratify the EF model-cache safety baseline: UseDataVaultMetadata(...) provides DVault-owned cache-key isolation for registry-backed metadata, and caller-owned shape discriminators belong in an application IModelCacheKeyFactory.
- Carry forward fixed-model guidance and evidence for UseModel(...), EF.CompileQuery stable shared-type reads, and AddDbContextPool<TContext>(...) on options-only fixed-shape contexts.
- Align README, architecture notes, analyzer README, performance guidance, release notes, tests, public API evidence, and benchmark artifacts around one v0.24.0 story.

Scope Out
- New analyzer IDs or runtime guards that attempt to prove arbitrary caller-owned model-cache, compiled-model, or pooled-context safety.
- Provider-native async writes, provider-native chunk execution, or a provider-specific async performance matrix.
- File ingestion, CDC ingestion, schedulers, background workers, hosted dashboards, or automatic database deployment.
- Replacing the explicit IDataVaultSaveService boundary with implicit SaveChanges streaming or another default persistence path.

Open questions
- none

Follow-up questions
- Should a future ticket add analyzer or runtime guardrails for variable-model compiled-model or pooled-context misuse beyond the current documentation-only boundary?
- Should a future provider-evidence ticket collect non-SQLite async/provider benchmark runs once external provider connection strings are configured?
- Should a future release add distinct public diagnostics or telemetry language for async chunk-source saves beyond the shared ChunkedRequest family, or is the current bounded surface sufficient?

Risks
- The epic title and older scope wording can be misread as promising new model-cache or pooling diagnostics even though the landed boundary intentionally settles on guidance-only EF safety.
- Optional external-provider benchmark rows are currently skipped when DVAULT_TEST_* connection strings are unset, so performance and provider wording must remain bounded to the checked-in SQLite/provider-neutral evidence.
- Consumers can still misuse compiled models or pooled contexts for variable realized model shapes; the current mitigation is documentation plus caller-owned cache-key design, not enforcement.

Split recommendations
- Keep any future provider-native async write or provider-specific async execution claims in a separate follow-on ticket rather than expanding this epic beyond provider-neutral async source saves.
- Keep any future model-cache, compiled-model, or pooling analyzer/runtime guardrails in a separate follow-on ticket if guidance-only EF safety proves insufficient.

Persisted contract coverage
- acceptance-criteria items: 6
- definition-of-done items: 4
- implementation-notes items: 7

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment