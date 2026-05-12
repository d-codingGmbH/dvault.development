## Developer implementation note

Implemented the combined core hook plus SQLite optimized read slice for ticket `06F0MEJE5WC51MFQ3CWDRATCWC`.

### Repository changes
- Added public core read-strategy contracts: `IDataVaultProviderReadStrategy` and `DataVaultProviderReadStrategyContext`.
- Routed `DefaultDataVaultReadService.ReadLatestSatelliteRowsAsync` and the internal typed satellite projection path through provider read-strategy dispatch before provider-neutral fallback.
- Added read-strategy diagnostics alongside existing save-strategy diagnostics: status, candidates, selected strategy name/priority, provider-neutral fallback, and material fallback causes.
- Added SQLite optimized latest/as-of satellite reads for the supported hub-parent, non-multi-active shape using parameterized SQLite SQL and database-side latest row selection.
- Registered `SqliteDataVaultReadStrategy` from `AddDVaultSqlite`; `AddDVault()` still registers no provider read strategy.
- Added unit/integration coverage for dispatch ordering, registration, diagnostics, unsupported-shape fallback, and typed projection routing.
- Updated the core public API approval snapshot and benchmark README.

### Verification performed
- `bash tools/check-format.sh` passed. The command reported the existing solution workspace format warning but ended with `Formatting check passed`.

### Verification blocked by environment
- `dotnet build DVault.slnx --nologo` could not complete because NuGet restore is blocked in this sandbox: `NU1301 Unable to load the service index for source https://api.nuget.org/v3/index.json` and `Permission denied (api.nuget.org:443)`.
- `dotnet test DVault.slnx --nologo --no-restore` also stopped on the same NuGet source access error for the test projects.
- SQLite benchmark smoke command attempted: `dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release --no-restore -- --provider sqlite --iterations 1 --warmup 0 --load-timestamp-storage provider-default --output /tmp/dvault-read-optimization-benchmark`. It could not produce measured rows because the benchmark project hit the same `NU1301` NuGet source access error.

### Benchmark rerun command
When package restore is available, rerun from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider sqlite --iterations 1 --warmup 0 --load-timestamp-storage provider-default --output artifacts/benchmarks/sqlite-read-strategy
```

Expected rows to compare in the emitted summary are the `latest-satellite-read` rows for `provider-neutral-dvault-fallback` and `sqlite-optimized-dvault`, preserving the command line, provider filter, iteration/warmup values, load timestamp storage, run context, and measured row counts/timings from that run.