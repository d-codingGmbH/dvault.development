[gicket-bot] PO refinement contract

Summary
- Refined the story to implement one additive async IAsyncEnumerable<DataVaultSaveChunk> save overload on the existing save service, reusing the ratified chunked-save contract and leaving typed helpers and benchmark evidence to the already-linked follow-up tickets.

PO handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

Clarifications
- Done contract story 06F7Y0CN1804HZW03J4XQ8XEJR already ratifies the API shape: add one IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken = default) overload, and do not reopen that decision here.
- Current source in src/DCoding.Data.DVault/DataVaultSaveService.cs exposes only the single-request, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest save overloads, so the async source overload is new public API work.
- The new overload reuses the existing DataVaultSaveChunk and DataVaultSaveRequest payload model plus the existing chunked telemetry and tracing family; no second public chunk wrapper or new telemetry mode is needed.
- Typed async mapper helpers and async benchmark or allocation evidence already have separate follow-up tickets, 06F7Y0DZ3AJSG99YN00CAVX3JR and 06F7Y0EVNY2M0113A6VWBNDCPR, so they should not be pulled into this story.

Scope In
- Add the additive IDataVaultSaveService async chunk-source overload and implement it in the default save service.
- Enumerate IAsyncEnumerable<DataVaultSaveChunk> exactly once, sequentially, in caller order, without pre-buffering the complete source.
- Preserve existing explicit request metadata rules, caller-owned transaction participation, cancellation propagation, and deterministic completion and failure boundaries.
- Reuse existing chunked retained-state handling, provider fallback behavior, telemetry summaries, Activity tracing, and redaction rules for async streaming saves.
- Update public API approval snapshots and save-service tests for the new public surface and behavior.

Scope Out
- Typed IAsyncEnumerable<TSource> mapper or helper APIs; keep that work in story 06F7Y0DZ3AJSG99YN00CAVX3JR.
- Async streaming benchmark or allocation evidence and documentation expansion; keep that work in task 06F7Y0EVNY2M0113A6VWBNDCPR.
- File ingestion, CDC, background workers, schedulers, queues, or implicit SaveChanges-based ingestion.
- Provider-native async chunk execution guarantees, new provider-specific public APIs, or changes to existing provider strategy eligibility.
- Automatic retry, checkpointing, resumable background continuation, or durable stream-state persistence.
- Behavior changes to the existing DataVaultSaveRequest, DataVaultBulkSaveRequest, or DataVaultChunkedSaveRequest overloads.

Open questions
- none

Follow-up questions
- After the core overload lands, should docs/performance-profiles.md add async-stream-specific chunk-size heuristics backed by task 06F7Y0EVNY2M0113A6VWBNDCPR?
- After the core overload lands, should public docs add a short migration example explaining when to keep DataVaultBulkSaveRequest, when to keep DataVaultChunkedSaveRequest, and when to switch to IAsyncEnumerable<DataVaultSaveChunk>?
- When should story 06F7Y0DZ3AJSG99YN00CAVX3JR be scheduled to add typed async mapper helpers on top of this core overload?

Risks
- Poorly behaved async sources can defer faults or ignore cancellation until MoveNextAsync advances, so tests must prove later chunks are not requested after failure or cancellation.
- Large or high-cardinality satellite streams can hit the existing retained-state limit and fall back to persisted latest-state lookup, preserving correctness but potentially changing performance characteristics.
- The public interface change will break existing in-repo IDataVaultSaveService test doubles until they implement the new overload.
- If implementation introduces a new telemetry mode or provider-specific fast path, it would violate the ratified contract that async streaming is only a source-shape addition over the existing provider-neutral chunked boundary.

Split recommendations
- No additional split is needed; keep typed async helper work in 06F7Y0DZ3AJSG99YN00CAVX3JR and benchmark or allocation evidence in 06F7Y0EVNY2M0113A6VWBNDCPR.
- Keep this story focused on the core overload, API snapshot change, and behavior tests only.

Persisted contract coverage
- acceptance-criteria items: 8
- definition-of-done items: 5
- implementation-notes items: 6

Planned ticket updates
- Refresh the durable refinement contract block in the ticket description.
- Update labels (added [critic-needed]; removed [needs-po]).
- Keep assignees unchanged.
- Keep status unchanged.

Run mode
- apply: planned updates are applied after this comment