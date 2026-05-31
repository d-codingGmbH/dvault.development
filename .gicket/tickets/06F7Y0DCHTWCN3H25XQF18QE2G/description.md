<!-- gicket-bot:human-ticket-refinement-contract:v1:start -->
## Delivery Contract

### PO Summary
- Refined the story to implement one additive async IAsyncEnumerable<DataVaultSaveChunk> save overload on the existing save service, reusing the ratified chunked-save contract and leaving typed helpers and benchmark evidence to the already-linked follow-up tickets.

### PO Handoff
- decision: `ready_for_po_critic`
- meaning: ticket can move to PO-critic review

### Clarifications
- Done contract story 06F7Y0CN1804HZW03J4XQ8XEJR already ratifies the API shape: add one IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken = default) overload, and do not reopen that decision here.
- Current source in src/DCoding.Data.DVault/DataVaultSaveService.cs exposes only the single-request, DataVaultBulkSaveRequest, and DataVaultChunkedSaveRequest save overloads, so the async source overload is new public API work.
- The new overload reuses the existing DataVaultSaveChunk and DataVaultSaveRequest payload model plus the existing chunked telemetry and tracing family; no second public chunk wrapper or new telemetry mode is needed.
- Typed async mapper helpers and async benchmark or allocation evidence already have separate follow-up tickets, 06F7Y0DZ3AJSG99YN00CAVX3JR and 06F7Y0EVNY2M0113A6VWBNDCPR, so they should not be pulled into this story.

### Scope In
- Add the additive IDataVaultSaveService async chunk-source overload and implement it in the default save service.
- Enumerate IAsyncEnumerable<DataVaultSaveChunk> exactly once, sequentially, in caller order, without pre-buffering the complete source.
- Preserve existing explicit request metadata rules, caller-owned transaction participation, cancellation propagation, and deterministic completion and failure boundaries.
- Reuse existing chunked retained-state handling, provider fallback behavior, telemetry summaries, Activity tracing, and redaction rules for async streaming saves.
- Update public API approval snapshots and save-service tests for the new public surface and behavior.

### Scope Out
- Typed IAsyncEnumerable<TSource> mapper or helper APIs; keep that work in story 06F7Y0DZ3AJSG99YN00CAVX3JR.
- Async streaming benchmark or allocation evidence and documentation expansion; keep that work in task 06F7Y0EVNY2M0113A6VWBNDCPR.
- File ingestion, CDC, background workers, schedulers, queues, or implicit SaveChanges-based ingestion.
- Provider-native async chunk execution guarantees, new provider-specific public APIs, or changes to existing provider strategy eligibility.
- Automatic retry, checkpointing, resumable background continuation, or durable stream-state persistence.
- Behavior changes to the existing DataVaultSaveRequest, DataVaultBulkSaveRequest, or DataVaultChunkedSaveRequest overloads.

## Acceptance Criteria
- IDataVaultSaveService exposes one additive SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken = default) overload, and the existing single-request, bulk, and DataVaultChunkedSaveRequest overloads remain unchanged.
- The default implementation consumes the async source once and processes yielded chunks sequentially in source order, preserving caller order within each chunk and existing hub, link, then satellite ordering within each request.
- The async overload does not materialize the complete source before writing; completed empty sources and empty chunks are valid no-ops that return a DataVaultSaveResult with RowsWritten equal to 0 when nothing is written.
- The caller remains owner of DbContext, current or ambient transaction, async source, and cancellation token; DVault does not create, commit, roll back, or suppress transactions.
- Cancellation or async enumeration and processing failure stops later chunks, propagates the observed exception or cancellation, and leaves no background continuation after the returned task completes, faults, or is canceled.
- Equivalent ordered async chunk input preserves existing save semantics for RowsWritten, SavedRecords ordering, hub and link reuse, and satellite hash-diff continuity relative to the existing materialized chunked and bulk paths.
- Async streaming attempts reuse the existing chunked telemetry and tracing boundary, including the dvault.save.chunked_request Activity, chunk and processed-chunk counts, retained-state high-water and fallback reporting, and existing redaction rules.
- Automated coverage includes no-op sources, ordered multi-chunk success, cancellation during async enumeration or before later chunks, caller-transaction participation, failure cleanup and retained-state release, and public API snapshot updates.

## Definition of Done
- The new overload is implemented on the public save-service interface and in the default implementation without changing existing overload semantics.
- Repository compile breaks caused by the interface expansion are resolved, including test doubles or other IDataVaultSaveService implementations.
- Public API approval snapshots and any directly impacted snapshot tests are updated and passing.
- Unit and integration tests prove async-source ordering, cancellation, transaction participation, telemetry and tracing continuity, failure cleanup, and compatibility with existing save paths.
- The story lands without pulling typed helper APIs, benchmark evidence work, or provider-native async claims into the implementation.

## Implementation Notes
- Use src/DCoding.Data.DVault/DataVaultSaveService.cs as the implementation baseline: the existing SaveChunkedRequestsAsync path, ChunkedSaveContinuityState, and DataVaultSaveTelemetryOperationKind.ChunkedRequest already encode the chunked behavior this overload should reuse.
- Advance the async enumerator with the caller-supplied cancellation token and request the next chunk only after the prior chunk has completed or been skipped as empty.
- Route each yielded DataVaultSaveChunk through the same per-chunk ordered request pipeline used by DataVaultChunkedSaveRequest so saved-record ordering and satellite continuity stay aligned with existing behavior.
- Keep async streaming under the existing chunked tracing and telemetry family in DataVaultActivityTracing and DataVaultSaveTelemetrySummary; do not add a new Activity name, telemetry enum value, or provider-native mode.
- Update tests around existing chunked coverage in ExplicitDataVaultSaveServiceSqliteTests plus API surface snapshots, activity-tracing assertions, and any interface test doubles such as the replacement save service used in unit tests.
- The behavior source is already ratified in docs/architecture/dvault-v1-streaming-explicit-save-contract.md; implementation should conform to that contract rather than expand scope.

## Open Questions
- none

## Follow-Up Questions
- After the core overload lands, should docs/performance-profiles.md add async-stream-specific chunk-size heuristics backed by task 06F7Y0EVNY2M0113A6VWBNDCPR?
- After the core overload lands, should public docs add a short migration example explaining when to keep DataVaultBulkSaveRequest, when to keep DataVaultChunkedSaveRequest, and when to switch to IAsyncEnumerable<DataVaultSaveChunk>?
- When should story 06F7Y0DZ3AJSG99YN00CAVX3JR be scheduled to add typed async mapper helpers on top of this core overload?

## Risks
- Poorly behaved async sources can defer faults or ignore cancellation until MoveNextAsync advances, so tests must prove later chunks are not requested after failure or cancellation.
- Large or high-cardinality satellite streams can hit the existing retained-state limit and fall back to persisted latest-state lookup, preserving correctness but potentially changing performance characteristics.
- The public interface change will break existing in-repo IDataVaultSaveService test doubles until they implement the new overload.
- If implementation introduces a new telemetry mode or provider-specific fast path, it would violate the ratified contract that async streaming is only a source-shape addition over the existing provider-neutral chunked boundary.

## Split Recommendations
- No additional split is needed; keep typed async helper work in 06F7Y0DZ3AJSG99YN00CAVX3JR and benchmark or allocation evidence in 06F7Y0EVNY2M0113A6VWBNDCPR.
- Keep this story focused on the core overload, API snapshot change, and behavior tests only.

<!-- gicket-bot:human-ticket-refinement-contract:v1:end -->

## Original Ticket Draft (legacy context)

The delivery contract above is authoritative. Use the legacy draft below only as background when it does not conflict with the contract block.

# Goal
Implement the async chunked save entry point selected by the v0.24 contract.

# Scope In
- Process async chunks in caller order without materializing the complete source first.
- Preserve explicit load timestamp, record source, cancellation, transaction, telemetry, Activity tracing, and redaction behavior.
- Update public API snapshots for intentional public surface changes.

# Scope Out
No file ingestion, background worker, automatic retry loop, scheduler, or provider-native chunk execution guarantee.

# Acceptance Criteria
- Async chunk input works for large ordered sources with bounded materialization.
- Tests cover no-op sources, cancellation, transaction participation, telemetry/tracing, failure cleanup, and compatibility with existing save paths.