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
- `DVAULT_TEST_DB2_CONNECTION_STRING`

When a provider is configured, the default report includes DVault fallback plus provider-specific optimized rows for the provider-native bulk-ingestion scenario. PostgreSQL and MySQL also include retained provider-native rows below the staged-bulk boundary so the matrix separates PostgreSQL direct/UNNEST, MySQL multi-row, and staged-provider paths without adding artifact columns. The optimized row first checks DVault diagnostics and fails the row if the named provider save strategy is not selected. The default report also keeps optional-provider read rows visible for latest satellite, PIT as-of, and bridge traversal scenarios. SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2 latest-satellite optimized rows check read-strategy diagnostics before timing and fail if the expected provider read strategy is not selected; non-SQLite PIT and bridge optimized rows identify the provider read strategy candidate in `executionDetail`. SQL Server also emits a PIT full-rebuild maintenance comparator pair under `pit-full-rebuild-maintenance`: `dvault-adddvault-fallback` identifies provider-neutral `RebuildAsync(...)` execution with `selectedStrategy=<none>`, while `dvault-adddvaultsqlserver-optimized` identifies the `SqlServerDataVaultPitMaintenanceService` service-replacement path for clean ordinary hub-parent full rebuilds. When a provider is not configured, its dependency is unavailable, or the connection cannot be opened, the command still emits provider rows with `executionStatus=skipped` and a normalized `skipReason`.

The required SQLite matrix includes read baselines for latest satellite, PIT as-of, and bridge traversal scenarios. Fixture creation, seeding, and strategy-diagnostic checks run before the timed operation so the measured read rows focus on the `IDataVaultReadService` latest satellite path, the `DataVaultPitAsOfReadRequest`/`DataVaultPitReadRecord` path, and the `DataVaultBridgeReadRequest`/`DataVaultBridgeReadRecord` path. SQLite latest-satellite, PIT, and bridge read rows compare the provider-neutral `AddDVault()` fallback with the `AddDVaultSqlite()` optimized provider read strategy, and their `executionDetail` values preserve the read-strategy status, selected strategy, fallback causes, read-shape kind, and read-shape provider status from `IDataVaultReadDiagnosticsService`. Optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 read rows remain visible in the default matrix. When those providers are skipped, the row metrics are blank/null and the planned `executionDetail` identifies the provider package path, including latest-satellite/PIT/bridge strategy candidates for the supported provider packages. Link-parent and multi-active PIT shapes remain diagnostics and integration-test evidence unless a benchmark row explicitly names those unsupported provider-neutral fallback shapes.

The default SQLite matrix also includes a streaming-save comparison for the existing chunked save boundary. The `customer-profile-streaming-save` rows use the same 60 ordered explicit profile-save requests for a materialized `DataVaultBulkSaveRequest` baseline, bounded `DataVaultChunkedSaveRequest` rows with chunk sizes 10 and 5, and an async-source row named `async-source-bounded-10` over the same chunk size 10 logical sequence. The synchronous chunked rows run through `IDataVaultSaveService.SaveAsync(DbContext, DataVaultChunkedSaveRequest, CancellationToken)`, while the async row runs through `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, CancellationToken)`. The chunked and async-source rows include chunk size, chunk count, processed chunk count, retained-state high-water count, save-path metadata, and the async source shape when applicable in `executionDetail`. These rows measure the current provider-neutral chunked path; they do not claim a provider-specific chunk optimization, provider-native async ingestion, a different ordering contract, or additional artifact columns.

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

Valid provider filters are `all`, `sqlite`, `postgres`, `sqlserver`, `mysql`, `oracle`, and `db2`.

Use `--load-timestamp-storage` to compare the physical representation of Data Vault load timestamps:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --load-timestamp-storage utc-ticks --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite-utc-ticks
```

Valid timestamp storage values are `provider-default`, `iso8601-utc-text`, and `utc-ticks`.

Use `--hash-key-storage-matrix` to run the bounded hash-key comparison baseline across `sha256-v1` hex, `sha256-v1` binary, `sha256-128-v1` hex, and `sha256-128-v1` binary without creating a second benchmark harness:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --hash-key-storage-matrix --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite-hash-key-storage
```

Use the default `--provider all` filter, or one configured provider filter, when collecting provider comparison rows for the same four variants. The external benchmark-lane set is PostgreSQL, SQL Server, MySQL, Oracle, and DB2. Unconfigured selected providers still emit skipped placeholder rows with planned strategy detail and `hashKeyVariant` execution metadata, and configured providers preserve failed rows when a physical storage profile exposes a provider-specific incompatibility, so binary-vs-hex artifact sets remain comparable without silently dropping caveats.

The matrix keeps the public DVault hash-key boundary as lowercase hexadecimal strings while projecting the selected stable-hash algorithm and physical storage profile into the EF model. In matrix mode, DVault save, latest-read, PIT, bridge, streaming-save, and latest-satellite lookup rows preserve `hashKeyVariant` in `executionDetail`; non-default variants also append the variant to the existing baseline name, for example `dvault-adddvaultsqlite-optimized/sha256-128-v1-binary`. Single-variant runs can use `--stable-hash sha256-128-v1` and `--hash-key-storage binary` when isolating one comparison point.

Use `--latest-indexes` to isolate the latest-satellite hash-diff lookup that protects insert-only satellites from repeated unchanged writes:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --latest-indexes --load-timestamp-storage utc-ticks --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite-latest-indexes
```

This mode seeds 100 customers with 20 existing profile satellite states each, then compares unchanged replay and changed replay saves across the current model index and explicit index variants. It is intended for tuning the satellite parent/load-timestamp/hash-diff index shape independently from the broader scenario benchmarks.

Use `--allocation-hotspots` to profile DVault-owned allocation hotspots on the default SQLite `sha256-v1`/`HexString` baseline:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --allocation-hotspots --iterations 3 --warmup 1 --output artifacts/benchmarks/sqlite-allocation-hotspots
```

This mode writes the standard `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet plus additive `allocation-hotspots.md`, `allocation-hotspots.csv`, and `allocation-hotspots.json` sidecars. The hotspot report ranks measured DVault-owned allocation surfaces for stable-hash canonicalization, digest generation, provider-neutral pre-write save preparation, and latest-hash-diff replay filtering. SQLite setup, seeding, verification, cleanup, and caller-owned satellite `HashDiff` generation are outside the profiled save actions.

When collecting external-provider comparison rows, set the relevant environment variable before restore/build/run so the benchmark project's conditional provider package references are present. PostgreSQL example:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING="Host=localhost;Database=dvault_benchmarks;Username=postgres;Password=postgres" dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchmarks
```

DB2 example:

```sh
DVAULT_TEST_DB2_CONNECTION_STRING="Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret" dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider db2 --iterations 1 --warmup 0 --output artifacts/benchmarks/db2
```

To emit documentation-ready artifacts from the same benchmark execution, pass an output directory:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchmarks
```

The command creates the output directory when needed and writes deterministic filenames:

- `benchmark-summary.md`
- `benchmark-summary.csv`
- `benchmark-summary.json`

Hash-key storage matrix runs also write same-directory footprint sidecars:

- `hash-key-footprint.md`
- `hash-key-footprint.csv`
- `hash-key-footprint.json`

These sidecars keep the compared algorithm id, digest byte length, hex character length, physical storage profile, provider store type, value format, and hash-reference payload bytes beside the root timing/allocation triplet. They are supplemental evidence for storage and index-shape interpretation; the required benchmark row schema remains the shared triplet schema.

The repository-facing evidence contract is defined in `docs/plans/performance-evidence-benchmark-artifact-contract.md`. Before/after evidence must keep two comparable copies of the artifact trio under one explicit scenario, ticket, or release label. The markdown, CSV, and JSON artifacts describe the same comparison rows. Each row includes scenario, provider, baseline, strategy family, dataset-size metadata, change-ratio metadata, execution status, skip reason, iteration count, mean/min/max milliseconds, mean/min/max allocated bytes, execution detail, and persisted outcome. Skipped and failed rows use `iterations=0`, blank CSV/markdown metric cells, and JSON `null` timing and allocation values while keeping execution detail visible. The markdown and JSON artifacts also include the benchmark options, OS description, OS and process architecture, processor count, .NET runtime details, provider filter, load-timestamp storage, hash-key variants, and optional provider discovery status. When downstream work needs provider-evidence manifest rows, use the `dvault.provider-evidence.v1` shape in `docs/plans/provider-optimization-evidence-matrix.md` and map only the artifact row fields plus deterministic `executionDetail` tokens into that shape. Downstream docs that cite benchmark results must preserve that hardware and provider context with the copied table or linked artifact so machine-specific timings are not separated from the run environment. The adopter-facing interpretation of the root triplet is [Performance Profiles](../../docs/performance-profiles.md).

The provider-optimized documentation boundary reuses the same root artifact triplet: `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`. Do not introduce separate staged-bulk or stored-procedure evidence filenames for release notes. PostgreSQL staged claims must be backed by the staged COPY row at the 60-operation threshold plus the retained direct or UNNEST row below that threshold. MySQL claims must distinguish the retained multi-row path below 100 operations, the bounded staged bulk window, and the large mixed-batch provider-neutral fallback above the 303-operation mixed window. SQL Server remains a single native-bulk boundary with the documented general and mixed-batch operation gates, and Oracle remains direct optimized batching with `stagedOracleBulk=not-selected-no-measured-win` until an artifact set records a measured staged Oracle win. Stored procedures are outside the benchmark harness default matrix and require separate explicit provider and migration-synchronization evidence before any documentation can treat them as an application-owned escape hatch.

When a performance claim depends on emitted query shape, index usage, batching behavior, or materialization behavior, store representative SQL beside the same before/after artifact set. Save-path claims that are limited to change-tracker or allocation behavior do not need duplicate SQL capture unless emitted SQL is part of the claim.

The benchmark command executes the required comparisons:

- customer profile history: one `C-100` customer with two profile states
- customer profile bulk insert-only: 100 customers with one initial profile state each and no repeat-change history
- customer profile bulk history: 100 customers with 10 profile states each
- customer profile streaming save: 20 customers with 60 ordered explicit save requests, measured once as a materialized bulk request, twice as bounded synchronous chunked saves, and once as a bounded async-source chunked save
- order-product fulfillment history: one `O-1000`/`SKU-COFFEE` order-product relationship with two fulfillment states and an unchanged replay proof outside the timing window
- latest satellite read: 100 seeded customers with 10 profile states each, measured through `ReadLatestSatelliteRowsAsync(...)`
- PIT as-of read: 100 seeded customers with profile and status snapshots plus one PIT row per customer, measured through `ReadPitRowsAsync(...)`
- bridge traversal read: one seeded hierarchy ancestor with 100 descendant bridge rows and a bounded maximum depth, measured through `ReadBridgeRowsAsync(...)`
- optional SQL Server PIT full-rebuild maintenance: 100 seeded customers with profile and status satellite history plus stale PIT rows, measured through `IDataVaultPitMaintenanceService.RebuildAsync(...)`
- compiled-model startup: one seeded generated order hub row, measured once through ordinary DVault model building and once through precomputed `UseModel(runtimeModel)`
- compiled-query hub read: one seeded generated order hub row, measured once through ordinary direct EF projection and once through `EF.CompileQuery(...)`
- DbContext pooling DVault operation: one generated order hub save/read operation, measured once through `AddDbContext<TContext>` and once through `AddDbContextPool<TContext>`
- optional provider-native bulk ingestion: 300 order-product pairs, 300 order-product links, and three ordered fulfillment satellite operations including one unchanged replay in a single provider-eligible bulk request

For conventional write-history scenarios other than `customer-profile-streaming-save`, SQLite emits one row for each strategy family:

- `classic-ef`
- `provider-neutral-dvault-fallback`
- `sqlite-optimized-dvault`

The streaming-save comparison emits provider-neutral DVault rows for the materialized explicit baseline, bounded synchronous chunked-save paths, and the bounded async-source chunked path because the evidence is scoped to the public chunked save boundary rather than provider-specific chunk optimization or provider-native async ingestion.

Read baselines emit the current provider-neutral read-service path through the DVault fallback registration and selected provider package rows. For SQLite reads, the provider package row uses the SQLite optimized read strategy for supported hub-parent, non-multi-active latest/as-of satellite reads, supported maintained PIT reads, and supported many-to-many or hierarchy bridge reads. Optional PostgreSQL, SQL Server, MySQL, Oracle, and DB2 read rows stay in the artifact triplet as skipped rows when their connection strings are absent; latest-satellite rows plus all PIT/bridge rows name their planned provider read strategy for the supported provider packages.

Compiled and pooled SQLite evidence emits these strategy families:

- `ef-model-build`
- `ef-usemodel-runtime-model`
- `direct-ef-query`
- `compiled-ef-query`
- `non-pooled-dvault-context`
- `pooled-dvault-context`

The compiled-model row precomputes runtime-model initialization outside the timed operation so the measured `UseModel(...)` row is not charged for design-model creation. The pooling rows use an options-only context with one fixed metadata model; caller-owned tenant, schema, naming, provider, or profile discriminators remain outside the supported pooled baseline unless the caller owns the corresponding EF model-cache-key behavior.

When PostgreSQL, SQL Server, MySQL, Oracle, or DB2 is configured and reachable in the default matrix, the provider-native bulk-ingestion scenario emits:

- `provider-neutral-dvault-fallback`
- the provider-specific optimized DVault strategy family

The provider-native optimized rows use a clean `DbContext`, no multi-active satellites, 903 total operations, and three satellite operations, so they exercise the large mixed-batch boundary for PostgreSQL staged COPY, SQL Server native bulk, Oracle native-save, and DB2 clean-context save gates before timing. PostgreSQL adds `dvault-adddvaultpostgres-direct-or-unnest` with 57 total operations to preserve the retained direct/UNNEST boundary below the 60-operation staged threshold. MySQL adds `dvault-adddvaultmysql-multi-row` with 57 total operations to preserve the retained multi-row boundary above the 50-operation native gate and below the 100-operation staged threshold; the default MySQL optimized 903-operation row deliberately uses provider-neutral fallback because staged bulk is bounded to satellite-only 100-plus batches or mixed 100-to-303-operation batches and retained multi-row did not beat fallback for the large mixed row. SQL Server remains a single native bulk row with a 100-operation general gate, a 900-operation mixed hub/link gate, and a 500-satellite cap. Oracle remains a direct optimized row with `stagedOracleBulk=not-selected-no-measured-win` until a measured staged Oracle path proves a net win, and DB2 remains a single optimized clean-context row with `stagedBulkBoundary=not-supported`. Completed provider-native rows record diagnostics-backed execution detail with the selected save strategy or deliberate fallback, candidate count, fallback causes, operation counts, staged/direct boundary text, and staged-provider diagnostics when a strategy reports them. When an optional provider is not configured or unavailable, those same provider-native bulk rows are present as skipped rows with planned execution detail so archived artifacts do not silently omit the optional provider boundary.
