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

When a provider is configured, the report includes DVault fallback plus provider-specific optimized rows for the same Data Vault scenarios. When a provider is not configured, its dependency is unavailable, or the connection cannot be opened, the command still emits provider rows with `executionStatus=skipped` and a normalized `skipReason`.

The default matrix includes read baselines for latest satellite, PIT as-of, and bridge traversal scenarios. Fixture creation and seeding run before the timed operation so the measured read rows focus on the `IDataVaultReadService` latest satellite path, the `DataVaultPitAsOfReadRequest`/`DataVaultPitReadRecord` path, and the provider-neutral `DataVaultBridgeReadRequest`/`DataVaultBridgeReadRecord` path. These rows intentionally do not add provider-specific read optimizations; provider-specific package rows are labeled so later optimized read implementations can be compared against the current provider-neutral baseline.

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

The markdown, CSV, and JSON artifacts describe the same comparison rows. Each row includes scenario, provider, baseline, strategy family, dataset-size metadata, change-ratio metadata, execution status, skip reason, iteration count, mean/min/max milliseconds, and persisted outcome. Skipped rows use `iterations=0`, blank CSV/markdown timing cells, and JSON `null` timing values. The markdown and JSON artifacts also include the benchmark options, OS description, OS and process architecture, processor count, .NET runtime details, and optional provider discovery status. Downstream docs that cite benchmark results must preserve that hardware and provider context with the copied table or linked artifact so machine-specific timings are not separated from the run environment.

The benchmark command executes the required comparisons:

- customer profile history: one `C-100` customer with two profile states
- customer profile bulk insert-only: 100 customers with one initial profile state each and no repeat-change history
- customer profile bulk history: 100 customers with 10 profile states each
- order-product fulfillment history: one `O-1000`/`SKU-COFFEE` order-product relationship with two fulfillment states and an unchanged replay proof outside the timing window
- latest satellite read: 100 seeded customers with 10 profile states each, measured through `ReadLatestSatelliteRowsAsync(...)`
- PIT as-of read: 100 seeded customers with profile and status snapshots plus one PIT row per customer, measured through `ReadPitRowsAsync(...)`
- bridge traversal read: one seeded hierarchy ancestor with 100 descendant bridge rows and a bounded maximum depth, measured through `ReadBridgeRowsAsync(...)`

For write-history scenarios, SQLite emits one row for each strategy family:

- `classic-ef`
- `provider-neutral-dvault-fallback`
- `sqlite-optimized-dvault`

Read baselines emit the current provider-neutral read-service path through the DVault fallback registration and the selected provider package registration. When PostgreSQL, SQL Server, MySQL, or Oracle is configured and reachable, each Data Vault scenario emits:

- `provider-neutral-dvault-fallback`
- the provider-specific optimized DVault strategy family

When an optional provider is not configured or unavailable, those same provider Data Vault rows are present as skipped rows so archived artifacts do not silently omit the optional provider boundary.
