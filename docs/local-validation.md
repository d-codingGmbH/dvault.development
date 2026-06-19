# Local Validation

Run validation from the repository root with a .NET 10 SDK checkout. Helper projects may stay on `net10.0`; the packaging lane proves the consumer `net8.0` and `net10.0` package outputs.

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
bash tools/pack-release-packages.sh
bash tools/verify-packages.sh
bash tools/check-format.sh
```

## Packages

`bash tools/pack-release-packages.sh` clears stale package artifacts and creates the coordinated release package lines under `artifacts/packages/`:

- eight `8.41.0` `.nupkg` files with `net8.0` assets and EF Core 8 dependency groups
- eight `10.41.0` `.nupkg` files with `net10.0` assets and EF Core 10 dependency groups
- matching `.snupkg` files for the runtime and provider packages

`bash tools/verify-packages.sh` checks package counts, ids, versions, filenames, metadata, README install guidance, XML documentation, analyzer assets, provider dependencies, DB2 dependency alignment, EF Core dependency lines, symbol packages, and stale package artifacts.

## Test Categories

Provider integration tests use stable xUnit trait categories:

- `Category=ProviderIntegration.RequiredLocal`: required SQLite-backed integration coverage that does not need external services.
- `Category=ProviderSmoke.Default`: provider package registration and configuration-contract smoke coverage that runs in the default local path.
- `Category=ProviderIntegration.ExternalOptIn`: live external database integration coverage for PostgreSQL, SQL Server, Oracle, MySQL, and DB2.

To make the default local provider boundary explicit:

```sh
dotnet test DVault.slnx --nologo --filter "Category!=ProviderIntegration.ExternalOptIn"
```

## Optional Provider Tests

External provider tests are skipped unless the matching connection string is configured locally. Keep credentials in local environment variables or another untracked secret store.

### PostgreSQL

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Postgres" -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured
```

### SQL Server

```sh
DVAULT_TEST_SQLSERVER_CONNECTION_STRING='Server=localhost;Database=dvault_tests;User Id=dvault;Password=local-secret;TrustServerCertificate=True' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=SqlServer" -p:DVAULT_TEST_SQLSERVER_CONNECTION_STRING=Configured
```

### Oracle

```sh
DVAULT_TEST_ORACLE_CONNECTION_STRING='User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Oracle" -p:DVAULT_TEST_ORACLE_CONNECTION_STRING=Configured
```

### MySQL

```sh
DVAULT_TEST_MYSQL_CONNECTION_STRING='Server=localhost;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret;AllowPublicKeyRetrieval=True;SslMode=Disabled' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=MySQL" -p:DVAULT_TEST_MYSQL_CONNECTION_STRING=Configured
```

### DB2

```sh
DVAULT_TEST_DB2_CONNECTION_STRING='Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=DB2" -p:DVAULT_TEST_DB2_CONNECTION_STRING=Configured
```

DVault does not provision external databases, users, schemas, credentials, Docker containers, or Podman containers for these tests. The configured principal must be allowed to create and drop the temporary objects used by the integration lane.

## Benchmarks

Run local SQLite scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

DB2 benchmark rows use the same optional-provider contract as the other external providers. Set the DB2 connection string before restore/build/run, keep provisioning outside the repository, and select `--provider db2` when isolating the lane:

```sh
DVAULT_TEST_DB2_CONNECTION_STRING='Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret' dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --provider db2 --iterations 1 --warmup 0 --output artifacts/benchmarks/db2
```

When DB2 is unset, unavailable, or unreachable, the root benchmark artifact triplet still preserves DB2 rows in the optional-provider matrix with `executionStatus=skipped`, `iterations=0`, blank/null metrics, normalized skip reasons, and planned execution details for the DB2 clean-context save and PIT/bridge read boundaries.

Pass `--output <directory>` to emit `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`. Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

Use `--hash-key-storage-matrix` to emit the bounded `sha256-v1` hex, `sha256-v1` binary, `sha256-128-v1` hex, and `sha256-128-v1` binary comparison rows. With the default provider filter, SQLite always runs locally and configured PostgreSQL, SQL Server, MySQL, and Oracle lanes run or stay visible as skipped placeholders; DB2 is not part of this benchmark lane baseline.
