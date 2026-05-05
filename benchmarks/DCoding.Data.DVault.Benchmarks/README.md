# DVault Benchmarks

Run the local scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The executable always uses SQLite temporary files as the required local baseline. SQLite rows exercise classic EF rows, the provider-neutral DVault fallback registered through `AddDVault()`, and the SQLite optimized DVault path registered through `AddDVaultSqlite()`. PostgreSQL is the only optional external provider in the v1 report. It is enabled by a non-empty `DVAULT_TEST_POSTGRES_CONNECTION_STRING` process environment variable and contributes DVault fallback plus `AddDVaultPostgres()` optimized rows for the same Data Vault scenarios when the provider dependency is available and the connection can be opened.

If `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is missing, the PostgreSQL EF provider dependency is unavailable, or the connection cannot be opened, the command still emits PostgreSQL report rows with `executionStatus=skipped` and a normalized `skipReason`. SQL Server, Oracle, and MySQL are intentionally not represented in this v1 comparison artifact. That benchmark scope is not a release-posture claim: those packages still expose provider-specific `AddDVaultSqlServer()`, `AddDVaultOracle()`, and `AddDVaultMySql()` save-strategy entry points with provider-neutral fallback. Oracle optimization is limited to clean `Oracle.EntityFrameworkCore` hub/link batches; dirty tracked contexts or request batches containing satellite operations fall back through the provider-neutral writer.

Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

When collecting PostgreSQL comparison rows, set the environment variable before restore/build/run so the benchmark project's conditional `Npgsql.EntityFrameworkCore.PostgreSQL` package reference is present:

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

The markdown, CSV, and JSON artifacts describe the same comparison rows. Each row includes provider, baseline, strategy family, dataset-size metadata, change-ratio metadata, execution status, skip reason, iteration count, mean/min/max milliseconds, and persisted outcome. Skipped rows use `iterations=0`, blank CSV/markdown timing cells, and JSON `null` timing values. The markdown and JSON artifacts also include the benchmark options, OS description, OS and process architecture, processor count, .NET runtime details, and PostgreSQL discovery status. Downstream docs that cite benchmark results must preserve that hardware and provider context with the copied table or linked artifact so machine-specific timings are not separated from the run environment.

The benchmark command executes the required comparisons:

- customer profile history: one `C-100` customer with two profile states
- customer profile bulk insert-only: 100 customers with one initial profile state each and no repeat-change history
- customer profile bulk history: 100 customers with 10 profile states each
- order-product fulfillment history: one `O-1000`/`SKU-COFFEE` order-product relationship with two fulfillment states and an unchanged replay proof outside the timing window

Every represented scenario emits one row for each strategy family:

- `classic-ef`
- `provider-neutral-dvault-fallback`
- `sqlite-optimized-dvault`

When PostgreSQL is configured and reachable, each Data Vault scenario also emits:

- `provider-neutral-dvault-fallback`
- `postgres-optimized-dvault`

When PostgreSQL is not configured or unavailable, those same PostgreSQL Data Vault rows are present as skipped rows so archived artifacts do not silently omit the optional provider boundary.
