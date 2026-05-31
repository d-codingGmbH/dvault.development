# DVault V1 Streaming Explicit Save Contract

Status: v1 contract
Ticket: 06F5Q8X261DQHG7N1445NGXB5W
Async source contract update: 06F7Y0CN1804HZW03J4XQ8XEJR
Current public baseline: [DVault v0.21.0 Release Notes](../releases/v0.21.0.md)

## Decision

DVault v1 defines streaming or chunked explicit saves as additive `IDataVaultSaveService` boundaries. The existing `SaveAsync(DbContext, DataVaultSaveRequest, ...)`, `SaveAsync(DbContext, DataVaultBulkSaveRequest, ...)`, and `SaveAsync(DbContext, DataVaultChunkedSaveRequest, ...)` overloads remain valid, backward compatible, and semantically authoritative for ordinary single-request, ordered-bulk, and materialized chunked callers.

The landed materialized chunked contract is:

```csharp
Task<DataVaultSaveResult> SaveAsync(
    DbContext dbContext,
    DataVaultChunkedSaveRequest request,
    CancellationToken cancellationToken = default);
```

The v0.24 async source contract adds one additional overload:

```csharp
Task<DataVaultSaveResult> SaveAsync(
    DbContext dbContext,
    IAsyncEnumerable<DataVaultSaveChunk> chunks,
    CancellationToken cancellationToken = default);
```

`DataVaultChunkedSaveRequest` carries a materialized ordered sequence of bounded `DataVaultSaveChunk` values. The async overload consumes a caller-ordered async sequence of the same `DataVaultSaveChunk` values without introducing a second public chunk wrapper. Each chunk carries an ordered, finite collection of ordinary `DataVaultSaveRequest` values. Those contained requests keep the same validation, metadata, resolver, hash-key, hash-diff, hub, link, and satellite rules as existing explicit save requests.

This contract defines the public API and behavior expectations for the additive v1 boundaries. The v0.19.0 public baseline documented the landed provider-neutral chunk execution and bounded retained-state diagnostics. The current v0.21.0 baseline keeps chunked saves provider-neutral while carrying forward the v0.20.0 staged PostgreSQL/MySQL optimized paths only for eligible materialized ordered bulk batches. The materialized `DataVaultChunkedSaveRequest` path is the landed chunked-save API; the async source overload is a source-shape addition over the same provider-neutral chunked boundary, not a provider-native async chunk execution claim.

## Input Shape

The caller supplies chunks in the order they should be processed. For `DataVaultChunkedSaveRequest`, that ordered chunk sequence is already materialized when the call starts. For `IAsyncEnumerable<DataVaultSaveChunk>`, DVault enumerates the async source exactly once and requests each next chunk only after the previous chunk has finished or been skipped as an empty chunk. Chunk enumeration is part of the explicit save call; DVault must not continue work in the background after the returned task completes, faults, or is canceled.

Each chunk is bounded. A chunk can contain zero or more `DataVaultSaveRequest` values. An empty materialized chunk sequence, completed empty async source, or empty chunk is a no-op and returns a valid `DataVaultSaveResult` with `RowsWritten` equal to `0` when no later chunk writes rows.

The service processes:

1. chunks in caller-supplied sequence order,
2. requests inside each chunk in caller-supplied order, and
3. operations inside each `DataVaultSaveRequest` according to the existing hub, link, then satellite save ordering.

DVault must not reorder chunks or requests by load timestamp, record source, table name, provider strategy, or hash key. Timestamp-aware satellite latest-state comparisons can still use load timestamps to decide whether a hash-diff state should replace the retained latest state for a parent series. The async source overload must not pre-buffer the full source before writing; it may retain only the bounded continuity state and diagnostics described below.

## Metadata And Resolver Rules

Load timestamp and record source remain explicit caller-visible request metadata. Chunked and async streaming execution use the same `DataVaultSaveRequest.LoadTimestamp`, `DataVaultSaveRequest.RecordSource`, `IDataVaultLoadTimestampResolver`, and `IDataVaultRecordSourceResolver` hooks already used by the existing save pipeline.

Chunked and async streaming execution must not introduce hidden metadata lanes, implicit batch timestamps, implicit record sources, file or stream metadata, scheduler metadata, or provider-specific metadata overrides. Resolver failures are ordinary save failures and stop continuation to later chunks.

## Cancellation And Transaction Ownership

The caller owns the `DbContext`, current or ambient transaction, async chunk source, and cancellation token. Chunked and async streaming execution participate in the caller's current transaction and must not create, commit, roll back, or suppress transactions on the caller's behalf.

The cancellation token is the single cancellation boundary for chunked execution. The service must observe cancellation before continuing to a later chunk and must propagate cancellation instead of silently completing later chunks. For the async source overload, the same token is also the token DVault uses while advancing the async enumerator. If the caller needs all-or-nothing behavior across chunks, the caller should open the transaction before invoking the service and roll it back if the operation is canceled or fails.

## Failure And Completion

Async source enumeration failures, resolver failures, validation failures, provider failures, and processing failures are ordinary save failures. DVault must stop requesting later chunks after the first failure, fault the returned task with the observed exception, and release retained chunked state in the same deterministic completion path used for materialized chunked saves.

Cancellation before async enumeration, during async enumeration, between chunks, or during request processing must stop later chunks and cancel the returned task. A completed, faulted, or canceled returned task is the completion boundary; DVault must not keep enumerating, writing, retrying, checkpointing, or resuming the source in the background.

## Compatibility Rules

Existing single-request, ordered-bulk, and materialized chunked save semantics remain the compatibility baseline:

- `DataVaultBulkSaveRequest.Requests` order remains caller order.
- `DataVaultChunkedSaveRequest.Chunks` order remains caller order.
- `IAsyncEnumerable<DataVaultSaveChunk>` is the only async source overload and is consumed sequentially in yielded order.
- `DataVaultProviderSaveStrategyContext.Requests` and `ResolvedRequests` remain ordered batches.
- provider strategies must use bound parameters, participate in the current transaction, propagate cancellation, and decline unsupported tracked-change shapes.
- hub and link saves preserve idempotent reuse semantics by generated hash key.
- satellite saves preserve parent-scoped and driving-key-scoped hash-diff replay semantics.
- `DataVaultSaveResult.SavedRecords` remains deterministic relative to caller-supplied chunk, request, and operation order.

The default v1 implementation should process each bounded chunk through the existing ordered request pipeline and append each processed chunk's `DataVaultSaveResult.SavedRecords` in chunk order. Provider-specific optimized strategies can accept or decline chunked work according to their documented gates, but a declined provider strategy must not change the public caller contract. The async source overload uses the same per-chunk execution boundary after each chunk is yielded; it does not add provider-specific public APIs or provider-native async chunk guarantees.

## Hash-State Continuity

Chunked execution must carry enough hash-key and hash-diff continuity across chunk boundaries to match the existing ordered-bulk behavior for equivalent ordered inputs. For satellites, continuity is tracked by satellite table, parent hash key, and canonical multi-active driving-key values.

The contract does not require materializing the complete logical source load before writing. Implementations must keep retained state bounded and deterministic. A shape that would require unbounded retained state can be rejected deterministically or routed through a documented bounded fallback, but DVault must not silently consume unbounded memory to preserve streaming semantics.

The v1 retained-state implementation keeps satellite continuity state for one explicit chunked-save attempt. The state key is the translated satellite table shape, parent hash key, and canonical driving-key values for multi-active satellites. The implementation clears that state in a deterministic completion path for successful, failed, and canceled attempts, so retained continuity does not leak into a later service call or the caller-owned `DbContext` lifetime.

The default retained-state limit is `10000` satellite series per chunked-save attempt. When an attempt would exceed that in-memory retained series count, DVault records the finite fallback cause `RetainedSatelliteSeriesLimitReached`, clears retained state, and falls back to the bounded per-chunk persisted latest-state lookup used by the ordinary ordered request pipeline. The unsupported or memory-sensitive shape classification is `RetainedSatelliteSeriesLimitExceeded`. This fallback preserves public save semantics without retaining raw hash keys, payload values, or unbounded per-parent listings in diagnostics.

`DataVaultSaveTelemetrySummary` is the bounded diagnostics surface for this v1 slice. Materialized chunked and async streaming attempts use the existing chunked telemetry family, including the `dvault.save.chunked_request` Activity operation. They report `ChunkCount`, `ProcessedChunkCount`, retained-state current and high-water counts, finite retained-state fallback cause kinds, and finite unsupported-shape kinds. The meter-backed observer projects those values as low-cardinality counters and histograms; it does not emit raw hash keys, payload values, or per-parent state entries.

The summary also exposes bounded explanation/remediation records for the finite provider save-fallback causes, retained-state fallback causes, and unsupported-shape classifications. These records preserve the existing enum vocabulary while giving callers actionable guidance for provider wiring, dirty tracked `DbContext` state, provider threshold chunk sizing, unsupported multi-active or memory-sensitive shapes, and retained-state fallback. Chunked summaries include explicit transaction guidance: execution participates in the caller's current transaction, and callers that need all-or-nothing behavior across chunks should open that transaction before invoking the save service. Async streaming does not create a new telemetry mode; it is reported as the same chunked request family with the same redaction rules.

## Compatibility Test Baseline

This story adds focused executable contract coverage for the additive chunked boundary using a test-local harness over the existing ordered bulk-save API. These tests prove the current explicit-save semantics that the implementation story must preserve without adding production chunk-execution mechanics in this contract story:

- `ChunkedSaveMatchesEquivalentBulkOrderingForHubAndLinkRequests`
- `ChunkedSaveObservesCancellationBeforeLaterChunks`
- `ChunkedSaveParticipatesInCallerTransactionAcrossChunks`
- `ChunkedSaveReusesRepeatedHubAndLinkRowsAcrossChunks`
- `ChunkedSaveCarriesSatelliteHashDiffContinuityAcrossChunks`

The existing API behavior that chunked saves must preserve is already covered by focused tests:

- `DefaultSaveServiceCarriesSatelliteHashDiffsAcrossBulkRequests`
- `DefaultSaveServiceKeepsBulkSatelliteLatestHashDiffChronological`
- `DefaultSaveServiceReusesExistingHubAndLinkRowsAcrossSqliteContexts`
- `ProviderSqlExecutionContract.ParticipatesInCurrentTransactionAsync`
- `ProviderSqlExecutionContract.PropagatesCancellationTokenAsync`

The retained-state implementation and diagnostics baseline extends this coverage with public chunked-save execution, bounded retained-state metrics, and deterministic release evidence. Provider-specific chunk optimization remains a later extension point and must not weaken the public caller contract.

## Non-Goals

This contract does not require provider-specific chunk execution, background ingestion, schedulers, queues, file ingestion, CDC ingestion, automatic runtime orchestration, or implicit `SaveChanges` interception.

`IDataVaultSaveService` remains the default explicit write boundary. SaveChanges interceptors remain outside the default v1 persistence path and do not become the streaming write path.
