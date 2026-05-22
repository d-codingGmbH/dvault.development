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
- persisted outcome

Completed rows must carry timing and allocation values. Skipped and failed rows must keep the row visible, set `iterations=0`, preserve the skip or failure reason, use blank markdown/CSV metric cells and JSON `null` metric values, and keep `persistedOutcome` as `not executed`.

## Minimum Scenario Baseline

The required local baseline is SQLite temporary files. A standard local evidence set must include:

- customer profile history
- customer profile bulk insert-only
- customer profile bulk history
- order-product fulfillment history
- latest satellite read
- PIT as-of read
- bridge traversal read

When the claim depends on scale behavior, include the scale matrix mode. When the claim depends on latest-satellite lookup/index behavior, include the latest-index matrix mode.

The optional external-provider matrix is limited to PostgreSQL, SQL Server, MySQL, and Oracle. Those providers emit provider-native bulk-ingestion comparison rows only when the provider is configured and reachable. If an optional provider is not configured, its rows must remain present as `executionStatus=skipped` with the normalized skip reason.

## Allocation Evidence

Allocation metrics are required for completed rows because many DVault performance claims depend on batching, materialization, and change-tracker behavior rather than wall-clock time alone.

The default gate for targeted allocation work is that the targeted allocation metric must improve or hold. Required SQLite non-target allocation regressions above 5% fail by default. Configured optional-provider allocation regressions above 10% must be called out and justified with the same before/after artifact set.

## SQL Capture Evidence

SQL capture is required when a claim depends on emitted query shape, index usage, batching behavior, or materialization behavior. Store representative SQL beside the before/after artifact sets under the same label, grouped by scenario, provider, and baseline. A recommended layout is:

- `artifacts/benchmarks/<label>/before/sql/<scenario>-<provider>-<baseline>.sql`
- `artifacts/benchmarks/<label>/after/sql/<scenario>-<provider>-<baseline>.sql`

Save-path scenarios that only claim change-tracker or allocation wins do not need duplicate SQL capture unless emitted SQL is part of the claim.

## Regression Budget

The targeted metric must improve or hold. For required SQLite rows, non-target mean-time and allocation regressions above 5% fail by default. For configured optional-provider rows, regressions above 10% must be explicitly called out and justified. Skipped optional providers are acceptable only when the artifact records the skip reason instead of omitting the row.

Failed rows do not satisfy a completed performance claim for that provider and scenario. They may be retained as failure evidence, but a downstream ticket must either fix the failed row or explicitly narrow and justify the claim.
