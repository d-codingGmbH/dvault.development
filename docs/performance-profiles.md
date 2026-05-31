# Performance Profiles

Status: v0.23.0 adopter guidance

This guide is the detailed performance-profile reference for DVault adopters. It translates the checked-in benchmark evidence into starting profiles, stop conditions, and rerun triggers. It does not create absolute performance guarantees, provider service-level objectives, dashboards, hosted observability, database provisioning, scheduler templates, or credential-management guidance.

## Evidence Baseline

Use the root benchmark artifact triplet as the source for the row names and timing values in this guide:

- [benchmark-summary.md](../benchmark-summary.md)
- [benchmark-summary.csv](../benchmark-summary.csv)
- [benchmark-summary.json](../benchmark-summary.json)

The benchmark runner and artifact rules are documented in [DVault Benchmarks](../benchmarks/DCoding.Data.DVault.Benchmarks/README.md) and [Performance Evidence And Benchmark Artifact Contract](plans/performance-evidence-benchmark-artifact-contract.md). Keep those linked artifacts with any copied result so the timing numbers stay attached to the run context.

The current checked-in root run used:

- 3 iterations and 1 warmup iteration.
- Load timestamp storage `ProviderDefault`.
- Provider filter `all`.
- Debian GNU/Linux 13 (trixie), X64 OS and process architecture, 32 processors.
- .NET 10.0.8.
- Required provider `SQLite local temporary files`.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle rows emitted as `executionStatus=skipped` because `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, and `DVAULT_TEST_ORACLE_CONNECTION_STRING` were unset.

Treat all millisecond values below as observations from that run only. Rerun the benchmarks when provider, hardware, runtime, load-timestamp storage, iteration count, warmup count, dataset size, request shape, or provider configuration changes.

## Profile Selection

| Profile | Start here when | Main starting point | Primary stop condition |
| --- | --- | --- | --- |
| Small app-local vault | The application writes ordinary hub, link, and satellite rows and needs a local SQLite or app-local proof first. | Register `AddDVault()` first, then add `AddDVaultSqlite()` only for SQLite deployments that want the provider package path. | Save/read diagnostics show provider fallback, a non-SQLite provider is selected, or the workload grows beyond the root customer-profile rows. |
| Medium chunked ingestion | The loader has an ordered source stream and must bound memory without changing load timestamps, record sources, or request order. | Keep `DataVaultBulkSaveRequest` for materialized batches; use `DataVaultChunkedSaveRequest` only for bounded ordered loaders, starting around chunk size 10. | Materializing the batch is acceptable, chunk overhead dominates, or chunk count/retained-state telemetry no longer matches the local workload. |
| Staged provider ingestion | The application has clean provider-specific contexts and larger eligible ordered bulk batches for PostgreSQL, SQL Server, MySQL, or Oracle. | Register `AddDVault()` plus the matching provider extension and verify save-strategy diagnostics before claiming provider-native behavior. | Optional-provider benchmark rows are skipped, the context is dirty, native gates decline, or the provider-local run has not been collected. |
| Read-model heavy | The application repeatedly reads latest satellites, maintained PIT rows, or maintained bridge rows. | Use `IDataVaultReadService`; add `AddDVaultSqlite()` for the repository-proven optimized SQLite read path. | PIT or bridge maintenance is stale, the provider is not SQLite, or read-shape diagnostics report fallback or unsupported shape. |

## Small App-Local Vault

### Workload Shape

Use this profile for small application-local vaults, early local proofs, and services that first need ordinary explicit saves to be correct and observable. The checked-in SQLite evidence covers:

- `customer-profile-history`: 1 customer with 2 profile states.
- `customer-profile-bulk-insert-only`: 100 customers with 1 profile state each.
- `customer-profile-bulk-history`: 100 customers with 10 profile states each.

### Registration Guidance

Start with `AddDVault()` as the safe baseline. It registers the explicit save/read services, stable hashing, provider-neutral fallback paths, and no telemetry listener by default. This is the right first registration when the provider path is not yet selected, when the app is still proving metadata and save ordering, or when provider-specific strategy diagnostics have not been reviewed.

For SQLite applications that install the SQLite provider package, add `AddDVaultSqlite()` beside the provider-neutral registration. In the checked-in run, the SQLite rows that use `AddDVaultSqlite()` selected `SqliteDataVaultSaveStrategy` for the write-history rows and `SqliteDataVaultReadStrategy` for supported read rows. Do not treat `AddDVaultSqlite()` as a universal performance requirement for non-SQLite providers.

### Starting Point

Keep the first production proof on the explicit `IDataVaultSaveService` boundary with caller-supplied load timestamp, record source, hub/link/satellite intent, and caller-owned transaction behavior. Move from provider-neutral `AddDVault()` to `AddDVaultSqlite()` only after the configured context really uses SQLite and diagnostics confirm the SQLite strategy is selected.

### Diagnostics And Telemetry

Inspect request-bound save-strategy diagnostics through `IDataVaultDiagnosticsService` before changing registration. The useful fields are strategy status, selected strategy name, candidate count, and finite fallback causes. For read-heavy app-local paths, inspect `IDataVaultReadDiagnosticsService` read strategy and read-shape diagnostics. Register `AddDVaultTelemetry()` only when the application wants built-in `System.Diagnostics.Metrics` for explicit save/read attempts; metric listeners, exporters, dashboards, and alerting stay application-owned.

### Supporting Rows

All values in this section are from the evidence baseline above:

| Scenario | Baseline | Mean ms | Evidence posture |
| --- | --- | ---: | --- |
| `customer-profile-history` | `dvault-adddvault-fallback` | 3.548 | Provider-neutral explicit save through `AddDVault()`. |
| `customer-profile-history` | `dvault-adddvaultsqlite-optimized` | 2.947 | SQLite optimized write path selected `SqliteDataVaultSaveStrategy`. |
| `customer-profile-bulk-insert-only` | `dvault-adddvault-fallback` | 14.387 | Provider-neutral fallback for 100 satellite operations. |
| `customer-profile-bulk-insert-only` | `dvault-adddvaultsqlite-optimized` | 5.129 | SQLite optimized write path selected for the same logical profile rows. |
| `customer-profile-bulk-history` | `dvault-adddvault-fallback` | 86.279 | Provider-neutral fallback for 1000 satellite operations across 10 requests. |
| `customer-profile-bulk-history` | `dvault-adddvaultsqlite-optimized` | 35.639 | SQLite optimized write path selected for the same logical profile history shape. |

### Stop Conditions And Rerun Triggers

Stop treating the root SQLite rows as enough evidence when the application uses a non-SQLite database, provider diagnostics report fallback, the request shape includes unsupported multi-active or dirty-context behavior, the dataset size is materially larger, or runtime/hardware differs enough to change the tuning question. Rerun the benchmark triplet with the same provider filter and changed inputs documented before copying new timing values into adopter materials.

## Medium Chunked Ingestion

### Workload Shape

Use this profile when a loader receives an ordered source sequence and cannot or should not materialize the complete request set before saving. The checked-in `customer-profile-streaming-save` rows use 20 customers, 60 ordered explicit profile-save requests, 3 profile events per customer, and one unchanged replay. The rows compare a materialized `DataVaultBulkSaveRequest` with bounded `DataVaultChunkedSaveRequest` runs.

### Registration Guidance

Use the same explicit `IDataVaultSaveService` boundary as ordinary saves. `DataVaultChunkedSaveRequest` is an input shape for bounded provider-neutral chunking; it is not a provider-native chunk execution claim. Register `AddDVault()` first. Provider package registrations can still optimize eligible ordinary ordered batches, but the current chunked evidence is scoped to the provider-neutral chunked path.

### Starting Point

Keep `DataVaultBulkSaveRequest` when the loader already has the complete ordered request set materialized. Choose `DataVaultChunkedSaveRequest` only when the loader needs bounded chunks while preserving explicit load timestamps, record sources, request order, chunk order, and caller-owned transactions.

The checked-in run supports chunk size 10 as the first bounded chunk-size candidate for this shape. Chunk size 5 is useful as a lower-memory comparison, but it increased elapsed time in the checked-in run. Retune with local data when the source event count, payload size, transaction policy, or provider changes.

### Diagnostics And Telemetry

Use save-strategy diagnostics to confirm whether the provider-neutral writer or a provider strategy handled each chunk. With `AddDVaultTelemetry()`, inspect `DataVaultSaveTelemetrySummary` for operation kind, request count, chunk count, processed chunk count, retained-state high-water count, fallback causes, unsupported-shape classifications, duration, and transaction guidance. Do not expect DVault to create scheduler, file-ingestion, CDC, database, or hosting infrastructure around chunked saves.

### Supporting Rows

All values in this section are from the evidence baseline above:

| Scenario | Baseline | Mean ms | Chunk detail |
| --- | --- | ---: | --- |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/materialized-explicit-bulk` | 6.828 | 60 ordered requests in one materialized bulk request. |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/chunked-save-bounded-10` | 13.330 | 6 chunks of 10, retained-state high-water 20. |
| `customer-profile-streaming-save` | `dvault-adddvault-fallback/chunked-save-bounded-5` | 19.313 | 12 chunks of 5, retained-state high-water 20. |

### Stop Conditions And Rerun Triggers

Prefer the materialized bulk request when memory use is acceptable and the application can naturally build the ordered batch. Stop using chunk size 10 as the default when telemetry shows excessive chunk count, retained-state fallback, unsupported shapes, cancellation pressure, or transaction boundaries that do not match the loader. Rerun local benchmarks for the chunk sizes the application can actually use before documenting a new operational starting point.

## Staged Provider Ingestion

### Workload Shape

Use this profile for provider-eligible ordered bulk ingestion on PostgreSQL, SQL Server, MySQL, or Oracle. The root `provider-native-bulk-ingestion` rows describe 20 order-product pairs, 20 order-product links, and 3 ordered fulfillment satellite operations, including one unchanged replay, in a clean-context provider-eligible batch. PostgreSQL and MySQL also keep smaller retained-path rows below the staged threshold.

### Registration Guidance

Register `AddDVault()` and the matching provider extension:

- `AddDVaultPostgres()` for clean `Npgsql.EntityFrameworkCore.PostgreSQL` contexts.
- `AddDVaultSqlServer()` for clean SQL Server contexts.
- `AddDVaultMySql()` for clean Pomelo or official MySQL EF Core contexts.
- `AddDVaultOracle()` for clean `Oracle.EntityFrameworkCore` contexts.

Provider-native dispatch is diagnostics-gated behind the same public save service. Dirty tracked contexts, provider-name mismatches, unsupported multi-active satellite batches, or batches outside the provider gate decline to a smaller provider-native path or the provider-neutral writer. The detailed save boundary is documented in [DVault V1 Explicit Save Service](architecture/dvault-v1-explicit-save-service.md).

### Starting Point

Use these provider boundaries as starting gates, not timing claims from the checked-in run:

| Provider | Starting gate | Evidence posture |
| --- | --- | --- |
| PostgreSQL | Retain direct or UNNEST below 60 operations; use staged COPY at 60-plus operations. | Rows are present but skipped because `DVAULT_TEST_POSTGRES_CONNECTION_STRING` was unset. |
| SQL Server | Native bulk starts at 50-plus total operations and no more than 500 satellite operations. | Rows are present but skipped because `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` was unset. |
| MySQL | Native gate starts at 50-plus operations; retained multi-row path is visible below the 60-operation staged boundary; staged bulk starts at 60-plus operations. | Rows are present but skipped because `DVAULT_TEST_MYSQL_CONNECTION_STRING` was unset. |
| Oracle | Direct optimized batching starts at 50-plus total operations and no more than 10000 satellite operations. | Rows are present but skipped because `DVAULT_TEST_ORACLE_CONNECTION_STRING` was unset; `stagedOracleBulk=not-selected-no-measured-win`. |

### Diagnostics And Telemetry

Before claiming provider-native behavior, run request-bound `IDataVaultDiagnosticsService` analysis for the exact batch and verify strategy status, selected strategy name, candidate diagnostics, operation counts, and fallback causes. Use `AddDVaultTelemetry()` for bounded save summaries after the application opts into metrics. Rerun the benchmark with the relevant provider environment variable set when the claim needs measured external-provider timings.

### Supporting Rows

The checked-in provider-native bulk rows are evidence for visibility and boundaries, not measured wins. `benchmark-summary.csv` and `benchmark-summary.json` keep the skipped rows visible with `iterations=0`, the skip reason, planned execution detail, selected strategy names, staged/direct boundary text, and `persistedOutcome=not executed`.

Rows to cite:

- PostgreSQL: `dvault-adddvaultpostgres-direct-or-unnest` for the below-60 retained direct/UNNEST boundary and `dvault-adddvaultpostgres-optimized` for the 60-plus staged COPY boundary.
- SQL Server: `dvault-adddvaultsqlserver-optimized` for the native bulk boundary.
- MySQL: `dvault-adddvaultmysql-multi-row` for the retained multi-row boundary and `dvault-adddvaultmysql-optimized` for the 60-plus staged bulk boundary.
- Oracle: `dvault-adddvaultoracle-optimized` for the retained direct optimized batching boundary and the current no-measured-win staged posture.

### Stop Conditions And Rerun Triggers

Stop before making a measured provider-specific performance claim when optional provider rows are skipped, connection strings are unset, provider packages are not restored for the benchmark run, the context has pending tracked changes, the operation count is below the provider gate, the satellite count exceeds SQL Server or Oracle limits, or diagnostics do not select the expected strategy. Rerun the benchmark triplet with the relevant provider configured and preserve skipped or failed rows exactly as the artifact contract requires.

## Read-Model Heavy

### Workload Shape

Use this profile when read throughput or allocation is the main concern for:

- Latest satellite reads over seeded profile history.
- PIT as-of reads over explicitly maintained PIT rows.
- Bridge traversal reads over explicitly maintained bridge rows.

The checked-in rows focus on `IDataVaultReadService` latest satellite reads, `DataVaultPitAsOfReadRequest`/`DataVaultPitReadRecord`, and `DataVaultBridgeReadRequest`/`DataVaultBridgeReadRecord`. SQLite is the only repository-proven optimized latest-satellite, PIT, or bridge read provider path in the root benchmark triplet.

### Registration Guidance

Start with `AddDVault()` and `IDataVaultReadService` for provider-neutral latest/current/as-of satellite, PIT, and bridge reads. Add `AddDVaultSqlite()` when the application uses SQLite and wants the optimized provider read strategy covered by the checked-in evidence. Non-SQLite providers and unsupported request shapes fall back through provider-neutral read pipelines unless a future artifact set proves another provider path.

PIT and bridge rows are caller-owned read models. Use `IDataVaultPitMaintenanceService` after satellite ingestion and `IDataVaultBridgeMaintenanceService` after source-link ingestion when those materialized tables should be refreshed. Reads do not run PIT or bridge maintenance implicitly, schedule maintenance, delete stale hierarchy rows, or create provider-specific physical tuning. Keep the detailed boundary in [DVault V1 PIT And Bridge Boundary](architecture/dvault-v1-pit-bridge-boundary.md).

### Starting Point

Use the runtime read service for dynamic and request-built shapes. For SQLite, add the provider package after confirming the read shape is supported and diagnostics select `SqliteDataVaultReadStrategy`. For PIT and bridge, place explicit maintenance in the ingestion workflow before read traffic depends on those rows.

### Diagnostics And Telemetry

Use `IDataVaultReadDiagnosticsService` for request-bound read strategy and read-shape diagnostics. The useful fields are strategy status, selected strategy name, fallback causes, read-shape kind, provider status, translated table identity, filter columns, and deterministic row-selection rules. Register `AddDVaultTelemetry()` when read attempt, returned-row, requested-key, duration, and finite fallback-cause metrics are needed. Do not expect raw SQL, provider query plans, automatic index creation, or provider-specific physical-design promises from these surfaces.

### Supporting Rows

All values in this section are from the evidence baseline above:

| Scenario | Baseline | Mean ms | Evidence posture |
| --- | --- | ---: | --- |
| `latest-satellite-read` | `dvault-adddvault-fallback` | 12.377 | Provider-neutral latest read over 100 customers and 1000 seeded profile states. |
| `latest-satellite-read` | `dvault-adddvaultsqlite-optimized` | 4.913 | SQLite optimized read path selected `SqliteDataVaultReadStrategy`. |
| `pit-as-of-read` | `dvault-adddvault-fallback` | 21.573 | Provider-neutral PIT as-of read over 100 PIT rows and 2 satellite segments. |
| `pit-as-of-read` | `dvault-adddvaultsqlite-optimized` | 20.947 | SQLite optimized PIT read path selected `SqliteDataVaultReadStrategy`. |
| `bridge-traversal-read` | `dvault-adddvault-fallback` | 2.091 | Provider-neutral bridge traversal over 1 ancestor and 100 descendant bridge rows. |
| `bridge-traversal-read` | `dvault-adddvaultsqlite-optimized` | 1.618 | SQLite optimized bridge read path selected `SqliteDataVaultReadStrategy`. |

### Stop Conditions And Rerun Triggers

Stop using the root read rows as sufficient evidence when the application is not using SQLite, read-shape diagnostics report fallback or unsupported shape, PIT or bridge maintenance is not run before reads, bridge hierarchy deletions require full rebuild behavior, or the data shape differs materially from the seeded benchmark. Rerun read benchmarks and keep read-shape diagnostics with the result when changing provider, indexes, maintenance cadence, shape, dataset size, or runtime.
