# DVault Benchmarks

Run the local scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The executable always uses SQLite temporary files as the required local baseline. SQLite rows exercise classic EF rows, the provider-neutral DVault fallback registered through `AddDVault()`, and the SQLite optimized DVault path registered through `AddDVaultSqlite()`. External providers are optional and are enabled by non-empty process environment variables:

- `DVAULT_TEST_POSTGRES_CONNECTION_STRING`
- `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`
- `DVAULT_TEST_MYSQL_CONNECTION_STRING`
- `DVAULT_TEST_ORACLE_CONNECTION_STRING`

When a provider is configured, the default report includes DVault fallback plus provider-specific optimized rows for the provider-native bulk-ingestion scenario. The optimized row first checks DVault diagnostics and fails the row if the named provider save strategy is not selected. SQLite read optimized rows likewise check read-strategy diagnostics before timing and fail if the SQLite provider read strategy is not selected. When a provider is not configured, its dependency is unavailable, or the connection cannot be opened, the command still emits provider rows with `executionStatus=skipped` and a normalized `skipReason`.

The required SQLite matrix includes read baselines for latest satellite, PIT as-of, and bridge traversal scenarios. Fixture creation, seeding, and strategy-diagnostic checks run before the timed operation so the measured read rows focus on the `IDataVaultReadService` latest satellite path, the `DataVaultPitAsOfReadRequest`/`DataVaultPitReadRecord` path, and the `DataVaultBridgeReadRequest`/`DataVaultBridgeReadRecord` path. SQLite latest-satellite, PIT, and bridge read rows compare the provider-neutral `AddDVault()` fallback with the `AddDVaultSqlite()` optimized provider read strategy. Non-SQLite provider rows are not emitted as provider-specific PIT or bridge read evidence in the default matrix.

The required SQLite matrix also includes bounded EF Core compiled and pooled-context evidence:

- compiled-model startup compares ordinary DVault model building with a DVault-projected design model initialized into an EF runtime model and supplied through `UseModel(runtimeModel)`
- compiled-query hub read compares `EF.CompileQuery(...)` with an equivalent ordinary direct EF query over the generated `HubOrder` shared-type table and deterministic projection
- DbContext pooling compares `AddDbContext<TContext>` with `AddDbContextPool<TContext>` for the same options-only context, fixed metadata source, SQLite provider, and generated order hub save/read operation

These rows are SQLite evidence only. They do not claim provider-specific compiled-model generation, dynamic `IDataVaultReadService` request compilation, provider-specific SQL shape changes, or pooled contexts whose model shape depends on per-request constructor state.

Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

Use `--provider` to restrict a run to one provider while tuning a specific backend:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite
```

Valid provider filters are `all`, `sqlite`, `postgres`, `sqlserver`, `mysql`, and `oracle`.

Use `--load-timestamp-storage` to compare the physical representation of Data Vault load timestamps:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --load-timestamp-storage utc-ticks --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite-utc-ticks
```

Valid timestamp storage values are `provider-default`, `iso8601-utc-text`, and `utc-ticks`.

Use `--latest-indexes` to isolate the latest-satellite hash-diff lookup that protects insert-only satellites from repeated unchanged writes:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --latest-indexes --load-timestamp-storage utc-ticks --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite-latest-indexes
```

This mode seeds 100 customers with 20 existing profile satellite states each, then compares unchanged replay and changed replay saves across the current model index and explicit index variants. It is intended for tuning the satellite parent/load-timestamp/hash-diff index shape independently from the broader scenario benchmarks.

When collecting external-provider comparison rows, set the relevant environment variable before restore/build/run so the benchmark project's conditional provider package references are present. PostgreSQL example:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING="Host=localhost;Database=dvault_benchmarks;Username=postgres;Password=postgres" dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchmarks
```

To emit documentation-ready artifacts from the same benchmark execution, pass an output directory:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchmarks
```

The command creates the output directory when needed and writes deterministic filenames:

- `benchmark-summary.md`
- `benchmark-summary.csv`
- `benchmark-summary.json`

The repository-facing evidence contract is defined in `docs/plans/performance-evidence-benchmark-artifact-contract.md`. Before/after evidence must keep two comparable copies of the artifact trio under one explicit scenario, ticket, or release label. The markdown, CSV, and JSON artifacts describe the same comparison rows. Each row includes scenario, provider, baseline, strategy family, dataset-size metadata, change-ratio metadata, execution status, skip reason, iteration count, mean/min/max milliseconds, mean/min/max allocated bytes, and persisted outcome. Skipped and failed rows use `iterations=0`, blank CSV/markdown metric cells, and JSON `null` timing and allocation values. The markdown and JSON artifacts also include the benchmark options, OS description, OS and process architecture, processor count, .NET runtime details, provider filter, load-timestamp storage, and optional provider discovery status. Downstream docs that cite benchmark results must preserve that hardware and provider context with the copied table or linked artifact so machine-specific timings are not separated from the run environment.

When a performance claim depends on emitted query shape, index usage, batching behavior, or materialization behavior, store representative SQL beside the same before/after artifact set. Save-path claims that are limited to change-tracker or allocation behavior do not need duplicate SQL capture unless emitted SQL is part of the claim.

The benchmark command executes the required comparisons:

- customer profile history: one `C-100` customer with two profile states
- customer profile bulk insert-only: 100 customers with one initial profile state each and no repeat-change history
- customer profile bulk history: 100 customers with 10 profile states each
- order-product fulfillment history: one `O-1000`/`SKU-COFFEE` order-product relationship with two fulfillment states and an unchanged replay proof outside the timing window
- latest satellite read: 100 seeded customers with 10 profile states each, measured through `ReadLatestSatelliteRowsAsync(...)`
- PIT as-of read: 100 seeded customers with profile and status snapshots plus one PIT row per customer, measured through `ReadPitRowsAsync(...)`
- bridge traversal read: one seeded hierarchy ancestor with 100 descendant bridge rows and a bounded maximum depth, measured through `ReadBridgeRowsAsync(...)`
- compiled-model startup: one seeded generated order hub row, measured once through ordinary DVault model building and once through precomputed `UseModel(runtimeModel)`
- compiled-query hub read: one seeded generated order hub row, measured once through ordinary direct EF projection and once through `EF.CompileQuery(...)`
- DbContext pooling DVault operation: one generated order hub save/read operation, measured once through `AddDbContext<TContext>` and once through `AddDbContextPool<TContext>`
- optional provider-native bulk ingestion: 20 order-product pairs, 20 order-product links, and three ordered fulfillment satellite operations including one unchanged replay in a single provider-eligible bulk request

For write-history scenarios, SQLite emits one row for each strategy family:

- `classic-ef`
- `provider-neutral-dvault-fallback`
- `sqlite-optimized-dvault`

Read baselines emit the current provider-neutral read-service path through the DVault fallback registration and the selected provider package registration for SQLite only. For SQLite reads, the provider package row uses the SQLite optimized read strategy for supported hub-parent, non-multi-active latest/as-of satellite reads, supported maintained PIT reads, and supported many-to-many or hierarchy bridge reads. When PostgreSQL, SQL Server, MySQL, or Oracle is configured and reachable in the default matrix, the provider-native bulk-ingestion scenario emits:

Compiled and pooled SQLite evidence emits these strategy families:

- `ef-model-build`
- `ef-usemodel-runtime-model`
- `direct-ef-query`
- `compiled-ef-query`
- `non-pooled-dvault-context`
- `pooled-dvault-context`

The compiled-model row precomputes runtime-model initialization outside the timed operation so the measured `UseModel(...)` row is not charged for design-model creation. The pooling rows use an options-only context with one fixed metadata model; caller-owned tenant, schema, naming, provider, or profile discriminators remain outside the supported pooled baseline unless the caller owns the corresponding EF model-cache-key behavior.

When PostgreSQL, SQL Server, MySQL, or Oracle is configured and reachable in the default matrix, the provider-native bulk-ingestion scenario emits:

- `provider-neutral-dvault-fallback`
- the provider-specific optimized DVault strategy family

The provider-native optimized row uses a clean `DbContext`, no multi-active satellites, 63 total operations, and three satellite operations, so it satisfies the SQL Server, MySQL, and Oracle native-save gates before timing. When an optional provider is not configured or unavailable, those same provider-native bulk rows are present as skipped rows so archived artifacts do not silently omit the optional provider boundary.
