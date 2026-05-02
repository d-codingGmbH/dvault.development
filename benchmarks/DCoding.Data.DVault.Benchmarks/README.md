# DVault Benchmarks

Run the local scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The executable uses SQLite temporary files only. It does not require Postgres, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, or checked-in machine-specific secrets.
Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

The benchmark command executes both required comparisons:

- customer profile history: conventional EF rows compared with the DVault hub-plus-satellite flow using the shared `C-100` profile contract
- order-product fulfillment history: conventional EF rows compared with the DVault hub-link-satellite flow using the reduced `O-1000` and `SKU-COFFEE` contract
