# Performance Evidence And Benchmark Artifact Contract

Status: v1 contract
Ticket: `06F492BZPP5YT9SJSPDHQBGF3R`

## Purpose

This document is the shared DVault performance-evidence contract. Performance tuning, release-note, and documentation work must reuse this contract instead of inventing ticket-specific benchmark formats.

The current benchmark harness remains the v1 baseline. It is extended by contract, documentation, and artifact-field tests rather than replaced.

## Required Artifact Set

Every persisted benchmark evidence set must contain these files from one benchmark execution:

- `benchmark-summary.md`
- `benchmark-summary.csv`
- `benchmark-summary.json`

Before/after evidence must store two comparable artifact sets under one explicit scenario, ticket, or release label. For example:

- `artifacts/benchmarks/<label>/before/benchmark-summary.md`
- `artifacts/benchmarks/<label>/before/benchmark-summary.csv`
- `artifacts/benchmarks/<label>/before/benchmark-summary.json`
- `artifacts/benchmarks/<label>/after/benchmark-summary.md`
- `artifacts/benchmarks/<label>/after/benchmark-summary.csv`
- `artifacts/benchmarks/<label>/after/benchmark-summary.json`

The before and after sets must use the same scenario mode, provider filter, iteration count, warmup count, load-timestamp storage setting, and provider configuration unless the claim explicitly documents why one of those inputs changed.

## Required Run Context

The JSON context and markdown run context must preserve:

- iterations
- warmup iterations
- load-timestamp storage
- provider filter
- hash-key variants when stable-hash algorithm or hash-key storage profile varies
- OS description
- OS architecture
- process architecture
- processor count
- .NET runtime description
- .NET runtime version
- required provider name
- optional provider names
- optional provider connection-string environment variable names
- provider execution status
- provider skip reason when applicable

Legacy PostgreSQL summary fields may remain for compatibility, but the authoritative optional-provider matrix is the `optionalProviders` context collection.

## Required Result Row Fields

Markdown, CSV, and JSON rows describe the same comparison rows. The core row contract is:

- scenario
- provider
- baseline
- strategy family
- dataset size
- change ratio
- execution status
- skip reason
- iterations
- mean milliseconds
- minimum milliseconds
- maximum milliseconds
- mean allocated bytes
- minimum allocated bytes
- maximum allocated bytes
- execution detail
- persisted outcome

Completed rows must carry timing and allocation values. Skipped and failed rows must keep the row visible, set `iterations=0`, preserve the skip or failure reason, use blank markdown/CSV metric cells and JSON `null` metric values, and keep `persistedOutcome` as `not executed`. Every row must keep a deterministic `executionDetail` string that identifies the exercised or planned execution path. Provider-optimized rows must include the selected provider strategy name when the row completes, or the planned provider strategy name when the row is skipped before execution.

## Minimum Scenario Baseline

The required local baseline is SQLite temporary files. A standard local evidence set must include:

- customer profile history
- customer profile bulk insert-only
- customer profile bulk history
- customer profile streaming save, comparing a materialized explicit bulk request with bounded synchronous chunked saves and the provider-neutral async-source chunked path
- order-product fulfillment history
- latest satellite read
- PIT as-of read
- bridge traversal read

Streaming-save evidence must reuse the artifact fields above. The materialized, synchronous chunked, and async-source rows must use the same logical explicit save requests and comparable run inputs. Synchronous chunked rows must make the exercised `DataVaultChunkedSaveRequest` path, chunk size, chunk count, processed chunk count, and retained-state high-water count visible through `executionDetail` or existing metadata fields without adding a new artifact schema. Async-source rows must stay on the same provider-neutral chunked telemetry boundary, identify `IAsyncEnumerable<DataVaultSaveChunk>` as the exercised source shape, preserve the chunk size and processed-chunk counts, and avoid provider-native async ingestion or alternate ordering claims.

When the claim depends on scale behavior, include the scale matrix mode. When the claim depends on latest-satellite lookup/index behavior, include the latest-index matrix mode.

When the claim compares stable-hash algorithm width or physical hash-key storage profile, keep supplemental footprint sidecars beside the same benchmark artifact triplet under the same label. The sidecars may capture provider store types, value formats, digest byte lengths, hex character lengths, and hash-reference payload bytes without adding new columns to the required benchmark-summary row schema.

The optional external-provider matrix is limited to PostgreSQL, SQL Server, MySQL, and Oracle. Those providers emit provider-native bulk-ingestion comparison rows only when the provider is configured and reachable. The provider-native bulk matrix must keep provider-neutral fallback rows, retained provider-native direct or multi-row rows where the repository exposes them, and staged-provider rows where the repository exposes them as distinct row identities. If an optional provider is not configured, its rows must remain present as `executionStatus=skipped` with the normalized skip reason and an `executionDetail` value that preserves the planned provider-native strategy boundary.

PostgreSQL staged evidence must preserve both the staged COPY row at the 60-operation threshold and the retained direct-or-UNNEST boundary below that threshold. MySQL staged evidence must preserve both the staged bulk row at the 60-operation threshold and the retained multi-row boundary above the 50-operation native gate and below the staged threshold. SQL Server v1 remains a single native bulk row for its current provider boundary instead of inventing an unsupported direct-versus-staged split.

Oracle provider-native evidence must distinguish the retained direct Oracle batching path from any future staged Oracle path. Until an Oracle staged path is selected, Oracle optimized rows should identify `OracleDataVaultSaveStrategy`, direct Oracle batching, and the fact that staged Oracle bulk was not selected because no measured win over the direct path is recorded in the artifact set.

## Allocation Evidence

Allocation metrics are required for completed rows because many DVault performance claims depend on batching, materialization, and change-tracker behavior rather than wall-clock time alone.

The default gate for targeted allocation work is that the targeted allocation metric must improve or hold. Required SQLite non-target allocation regressions above 5% fail by default. Configured optional-provider allocation regressions above 10% must be called out and justified with the same before/after artifact set.

## SQL Capture Evidence

SQL capture is required when a claim depends on emitted query shape, index usage, batching behavior, or materialization behavior. Store representative SQL beside the before/after artifact sets under the same label, grouped by scenario, provider, and baseline. A recommended layout is:

- `artifacts/benchmarks/<label>/before/sql/<scenario>-<provider>-<baseline>.sql`
- `artifacts/benchmarks/<label>/after/sql/<scenario>-<provider>-<baseline>.sql`

Save-path scenarios that only claim change-tracker or allocation wins do not need duplicate SQL capture unless emitted SQL is part of the claim. For provider-native bulk-ingestion rows, the artifact row `executionDetail` may serve as the stable execution proof when raw SQL text is too provider- or version-sensitive to persist as the regression boundary.

## Regression Budget

The targeted metric must improve or hold. For required SQLite rows, non-target mean-time and allocation regressions above 5% fail by default. For configured optional-provider rows, regressions above 10% must be explicitly called out and justified. Skipped optional providers are acceptable only when the artifact records the skip reason instead of omitting the row.

Failed rows do not satisfy a completed performance claim for that provider and scenario. They may be retained as failure evidence, but a downstream ticket must either fix the failed row or explicitly narrow and justify the claim.
