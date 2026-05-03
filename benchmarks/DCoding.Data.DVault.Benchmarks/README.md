# DVault Benchmarks

Run the local scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The executable uses SQLite temporary files and registers `DCoding.Data.DVault.Sqlite` so DVault timings include the SQLite provider save strategy. It does not require Postgres, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, or checked-in machine-specific secrets.
Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

To emit documentation-ready artifacts from the same benchmark execution, pass an output directory:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0 --output artifacts/benchmarks
```

The command creates the output directory when needed and writes deterministic filenames:

- `benchmark-summary.md`
- `benchmark-summary.csv`
- `benchmark-summary.json`

The markdown and JSON artifacts include the SQLite provider statement, benchmark options, OS description, OS and process architecture, processor count, and .NET runtime details. Downstream docs that cite benchmark results must preserve that hardware and provider context with the copied table or linked artifact so machine-specific timings are not separated from the run environment.

The benchmark command executes the required comparisons:

- customer profile history: conventional EF rows compared with the DVault hub-plus-satellite flow using the shared `C-100` profile contract
- customer profile bulk history: conventional EF bulk rows compared with the DVault bulk-save hub-plus-satellite flow using 100 customers and 10 profile states each
- order-product fulfillment history: conventional EF rows compared with the DVault hub-link-satellite flow using the reduced `O-1000` and `SKU-COFFEE` contract
