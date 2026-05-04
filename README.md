# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Installation

Install the provider-neutral DVault package from NuGet:

```sh
dotnet add package DCoding.Data.DVault --version 0.4.1
```

For provider-specific startup extensions, add the matching provider package as well. For example, SQLite users should install:

```sh
dotnet add package DCoding.Data.DVault.Sqlite --version 0.4.1
```

The provider package family is version-aligned:

```sh
dotnet add package DCoding.Data.DVault.MySql --version 0.4.1
dotnet add package DCoding.Data.DVault.Oracle --version 0.4.1
dotnet add package DCoding.Data.DVault.Postgres --version 0.4.1
dotnet add package DCoding.Data.DVault.SqlServer --version 0.4.1
```

Applications still need their normal Entity Framework Core database provider package, such as `Microsoft.EntityFrameworkCore.Sqlite` for SQLite or the relevant provider for PostgreSQL, SQL Server, Oracle, or MySQL.

## Quickstart

Use this flow in a .NET 10 project that references `DCoding.Data.DVault` and has an Entity Framework Core provider configured. The v1 path is convention-first: register DVault without options, declare Data Vault metadata on the EF model, save explicitly through `IDataVaultSaveService`, and read the generated shared-type tables through EF.

### Register DVault services

```csharp
using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDVault();

using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
```

### Configure the EF model

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

public sealed class SalesVaultContext(DbContextOptions<SalesVaultContext> options) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);

    modelBuilder.ApplyDataVaultMetadata(
        new DataVaultMetadataModel(
            [customer, order],
            [customerOrder],
            []));
  }
}
```

### Save explicitly

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

public static class SalesVaultWriter {
  public static async Task SaveCustomerOrderAsync(
      SalesVaultContext context,
      IServiceProvider serviceProvider,
      CancellationToken cancellationToken = default) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);
    var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
    var saveService = serviceProvider.GetRequiredService<IDataVaultSaveService>();

    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [
                new(customer, [new("Customer Id", "C-100")]),
                new(order, [new("Order Id", "O-200")]),
            ],
            []),
        cancellationToken);

    var customerHashKey = hubResult.SavedRecords.Single(record =>
        record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Customer").HashKey;
    var orderHashKey = hubResult.SavedRecords.Single(record =>
        record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Order").HashKey;

    await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [],
            [
                new(customerOrder, [new("Customer", customerHashKey), new("Order", orderHashKey)]),
            ]),
        cancellationToken);
  }
}
```

`DataVaultSaveRequest` keeps the load timestamp and record source explicit. DVault does not intercept `SaveChanges`; callers choose when to write vault rows. For loaders that already have multiple source batches prepared, `DataVaultBulkSaveRequest` processes ordered save requests through the same service and keeps satellite HashDiff state in memory across the batch.

### Provider Packages

`DCoding.Data.DVault` contains the provider-neutral API, metadata model, naming conventions, stable hashing, and EF fallback writer. Provider packages extend that base registration without changing the write API:

```csharp
services.AddDVaultSqlite();
services.AddDVaultPostgres();
services.AddDVaultSqlServer();
services.AddDVaultOracle();
services.AddDVaultMySql();
```

`DCoding.Data.DVault.Sqlite` registers the optimized SQLite set-based save strategy. `DCoding.Data.DVault.Postgres` registers an optimized Npgsql/PostgreSQL strategy for clean contexts that use set-based `INSERT ... ON CONFLICT DO NOTHING` hub and link writes plus latest-state satellite checks. `DCoding.Data.DVault.SqlServer` registers an optimized SQL Server strategy for clean contexts with set-based unique-row inserts and latest-state satellite checks. `DCoding.Data.DVault.Oracle` registers an Oracle-gated insert-only strategy for clean hub/link batches and declines unsupported shapes so the provider-neutral fallback writer handles them. `DCoding.Data.DVault.MySql` currently provides the stable package and startup boundary and uses the provider-neutral fallback. Provider strategies decline incompatible contexts so the provider-neutral fallback remains the caller-visible safety net.

### Query generated tables

```csharp
using Microsoft.EntityFrameworkCore;

public static class SalesVaultReader {
  public static async Task<IReadOnlyList<Dictionary<string, object>>> ReadCustomerOrdersAsync(
      SalesVaultContext context,
      CancellationToken cancellationToken = default) {
    return await context
        .Set<Dictionary<string, object>>("LinkCustomerOrder")
        .AsNoTracking()
        .ToListAsync(cancellationToken);
  }
}
```

The shared-type table names and columns in this quickstart follow DVault's default naming conventions, for example `HubCustomer`, `HubOrder`, `LinkCustomerOrder`, `CustomerHashKey`, `OrderHashKey`, `LoadTimestamp`, and `RecordSource`. This Customer/Order/CustomerOrder flow is mirrored by the SQLite explicit-save integration tests in `tests/DCoding.Data.DVault.Tests`.

## Layout

- `DVault.slnx`: Canonical root solution file for build and test automation.
- `src/DCoding.Data/`: Non-packable build anchor for the `DCoding.Data` source-root namespace family.
- `src/DCoding.Data.DVault/`: Main library project. The NuGet package id and root namespace are `DCoding.Data.DVault`.
- `src/DCoding.Data.DVault.*`: Provider extension packages for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- `tests/DCoding.Data.DVault.Tests/`: Unit, integration, and shared test projects for DVault.
- `examples/`: Future runnable examples for DVault APIs.
- `benchmarks/`: Local performance benchmark projects.
- `docs/`: Documentation and design notes.

All current .NET projects are included in `DVault.slnx`. Empty future-use folders contain `.gitkeep` files so the layout is present in clean checkouts.

## Local Validation

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
dotnet pack DVault.slnx --configuration Release --nologo
bash tools/verify-packages.sh
bash tools/check-format.sh
```

The normal test run includes package-specific public API snapshot checks for `DCoding.Data.DVault` and the five provider packages. See `docs/quality/api-surface-snapshots.md` for the approved baseline location and the explicit update workflow for intentional API changes.

`bash tools/verify-packages.sh` inspects the artifacts created under `artifacts/packages/` by the solution-level pack command. It expects exactly the six DVault library packages and matching symbol packages, checks README and XML documentation entries, validates declared NuGet metadata, and confirms each provider package depends on the packed `DCoding.Data.DVault` version. The verifier intentionally fails when stale, unexpected, or non-packable package artifacts remain in `artifacts/packages/`.

Provider integration tests use stable xUnit trait categories so required local coverage and opt-in external database coverage can be selected explicitly:

- `Category=ProviderIntegration.RequiredLocal`: required SQLite-backed integration coverage that does not need external services.
- `Category=ProviderSmoke.Default`: provider package registration and configuration-contract smoke coverage that runs in the default local path.
- `Category=ProviderIntegration.ExternalOptIn`: live external database integration coverage, currently Postgres, Oracle, and MySQL.

To make the default local provider boundary explicit in a focused run, exclude opt-in external database tests:

```sh
dotnet test DVault.slnx --nologo --filter "Category!=ProviderIntegration.ExternalOptIn"
```

## Benchmarks

Run the local SQLite scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The benchmark executable compares conventional EF and DVault flows for the shared customer profile history contract, a larger customer profile bulk-history contract, and the reduced order-product fulfillment history contract. It uses SQLite temporary files by default and does not require Postgres, Oracle, MySQL, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, or `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

## Optional Local Postgres Integration Tests

Postgres integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require Postgres, Docker, or checked-in machine-specific configuration.

To run the Postgres-backed integration tests, provide a developer-managed PostgreSQL database connection string in `DVAULT_TEST_POSTGRES_CONNECTION_STRING`:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret' dotnet test DVault.slnx --nologo
```

To select only the live Postgres integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Postgres"
```

DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop temporary schemas. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## Optional Local Oracle Integration Tests

Oracle integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require Oracle, Docker, or checked-in machine-specific configuration.

To run the Oracle-backed smoke test, provide a developer-managed Oracle database connection string in `DVAULT_TEST_ORACLE_CONNECTION_STRING`:

```sh
DVAULT_TEST_ORACLE_CONNECTION_STRING='User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1' dotnet test DVault.slnx --nologo
```

To select only the live Oracle integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_ORACLE_CONNECTION_STRING='User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Oracle"
```

DVault does not provision Docker containers, Oracle databases, or Oracle users for these tests. The configured database and user must already exist, and the configured user must be allowed to create and drop temporary tables. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## Optional Local MySQL Integration Tests

MySQL integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require MySQL, Docker, or checked-in machine-specific configuration.

To run the MySQL-backed integration test, provide a developer-managed MySQL database connection string in `DVAULT_TEST_MYSQL_CONNECTION_STRING`:

```sh
DVAULT_TEST_MYSQL_CONNECTION_STRING='Server=localhost;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret' dotnet test DVault.slnx --nologo
```

To select only the live MySQL integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_MYSQL_CONNECTION_STRING='Server=localhost;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=MySQL"
```

The integration project conditionally restores `Pomelo.EntityFrameworkCore.MySql` only when `DVAULT_TEST_MYSQL_CONNECTION_STRING` is non-empty. When running the live MySQL path, keep the variable set for restore, build, and test so the conditional provider package is available. DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop the smoke-test table. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## License

DVault uses the Apache License 2.0. See `LICENSE`.
