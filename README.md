# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Installation

Install the provider-neutral DVault package from NuGet:

```sh
dotnet add package DCoding.Data.DVault --version 0.5.0
```

For provider-specific startup extensions, add the matching provider package as well. For example, SQLite users should install:

```sh
dotnet add package DCoding.Data.DVault.Sqlite --version 0.5.0
```

The provider package family is version-aligned:

```sh
dotnet add package DCoding.Data.DVault.MySql --version 0.5.0
dotnet add package DCoding.Data.DVault.Oracle --version 0.5.0
dotnet add package DCoding.Data.DVault.Postgres --version 0.5.0
dotnet add package DCoding.Data.DVault.SqlServer --version 0.5.0
```

Applications still need their normal Entity Framework Core database provider package, such as `Microsoft.EntityFrameworkCore.Sqlite` for SQLite, `Pomelo.EntityFrameworkCore.MySql` for the DVault MySQL v1 path, or the relevant provider for PostgreSQL, SQL Server, or Oracle.

## Quickstart

Use this flow in a .NET 10 project that references `DCoding.Data.DVault` and has an Entity Framework Core provider configured. The v1 path is convention-first: register DVault without options, declare Data Vault metadata on the EF model, save explicitly through `IDataVaultSaveService`, and read generated vault rows either through `IDataVaultReadService` for common latest/as-of satellite access or through ordinary EF shared-type table queries.

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

### Multi-active satellite opt-in

Ordinary satellites remain the default. A satellite becomes multi-active only when it declares one or more driving keys, and those names define the canonical identity tuple for each active satellite row. Driving-key values stay separate from payload values, and `hashDiff` continues to represent payload state for change detection rather than driving-key identity.

The minimal shape below declares a customer contact satellite with payload `Email Address` and driving keys `Contact Type` then `Region Code`:

```csharp
var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var contact = new DataVaultSatelliteMetadata(
    "Contact",
    customer.ToReference(),
    ["Email Address"],
    ["Contact Type", "Region Code"]);

modelBuilder.ApplyDataVaultMetadata(
    new DataVaultMetadataModel(
        [customer],
        [],
        [contact]));
```

The save operation supplies driving-key values by logical name. Caller enumeration order does not matter; the values below are matched by name and persisted in the declared canonical order `Contact Type`, then `Region Code`:

```csharp
await saveService.SaveAsync(
    context,
    new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [],
        [],
        [
            new(
                contact,
                "customer-hash",
                [new("Region Code", "DE"), new("Contact Type", "billing")],
                [new("Email Address", "billing-de@example.test")],
                "contact-payload-hash"),
        ]),
    cancellationToken);
```

The provider-neutral projection stores driving-key columns immediately after the parent hash-key column and before `HashDiff`, `LoadTimestamp`, `RecordSource`, and payload columns. For the example above, the satellite row shape is `CustomerHashKey`, `ContactType`, `RegionCode`, `HashDiff`, `LoadTimestamp`, `RecordSource`, then `EmailAddress`. The satellite primary key and parent/latest-state index expand to `(CustomerHashKey, ContactType, RegionCode, LoadTimestamp)`, so different driving-key tuples for the same parent can coexist. Re-saving the same parent plus driving-key tuple with the same `hashDiff` reuses the latest state; changing the `hashDiff` writes a new history row for that tuple.

Current multi-active support is intentionally narrow and opt-in. Future work includes PIT over multi-active satellites, bridge interactions involving multi-active state, link-based PIT support, and provider-specific optimized multi-active save behavior. Provider-specific save strategies may decline multi-active batches so the provider-neutral writer handles the documented baseline.

### Provider Packages

`DCoding.Data.DVault` contains the provider-neutral API, metadata model, naming conventions, stable hashing, and EF fallback writer. Provider packages extend that base registration without changing the write API:

```csharp
services.AddDVaultSqlite();
services.AddDVaultPostgres();
services.AddDVaultSqlServer();
services.AddDVaultOracle();
services.AddDVaultMySql();
```

`DCoding.Data.DVault.Sqlite` registers the optimized SQLite set-based save strategy and registers the SQLite EF provider name for the existing `ApplyDataVaultMetadata(...)` capability-profile selection path. `DCoding.Data.DVault.Postgres` registers an optimized Npgsql/PostgreSQL strategy for clean contexts that use set-based `INSERT ... ON CONFLICT DO NOTHING` hub and link writes plus latest-state satellite checks. `DCoding.Data.DVault.SqlServer` registers an optimized SQL Server strategy for clean contexts with set-based unique-row inserts and latest-state satellite checks. `DCoding.Data.DVault.Oracle` registers an Oracle-gated insert-only strategy for clean `Oracle.EntityFrameworkCore` hub/link batches and declines unsupported shapes, including dirty tracked contexts and request batches that contain satellite operations, so the provider-neutral fallback writer handles them. `DCoding.Data.DVault.MySql` targets `Pomelo.EntityFrameworkCore.MySql`, registers that Pomelo provider name for the existing `ApplyDataVaultMetadata(...)` capability-profile selection path, and registers an optimized MySQL strategy for clean Pomelo contexts.

Provider-specific save-strategy registration and provider-name capability-profile auto-registration are separate startup surfaces. The current startup extensions visibly auto-register provider names for SQLite and MySQL only. PostgreSQL, SQL Server, and Oracle still provide provider-specific save strategies and keep the provider-neutral fallback as the caller-visible safety net, but their startup extensions do not currently auto-register provider-name capability profiles.

### Read latest satellite rows

`IDataVaultReadService` provides a provider-neutral helper for common latest and as-of satellite reads. It keeps the same explicit metadata boundary as the save service and uses parent-hash-key filtering so generated satellite parent indexes can be used by the database:

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

var readService = serviceProvider.GetRequiredService<IDataVaultReadService>();
var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var profile = new DataVaultSatelliteMetadata(
    "Profile",
    customer.ToReference(),
    ["customer_name", "customer_status"]);

var latestRows = await readService.ReadLatestSatelliteRowsAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey]),
    cancellationToken);

var asOfRows = await readService.ReadLatestSatelliteRowsAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], asOfTimestamp),
    cancellationToken);
```

The read service returns `DataVaultSatelliteReadRecord` values containing the parent hash key, driving-key values, hash diff, load timestamp, record source, and payload values. It is intentionally narrow in v0.5.0: PIT-backed read models, bridge traversal helpers, and provider-specific read strategies remain future extension points.

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

Direct EF queries remain available for table-specific projections, custom joins, diagnostics, and cases outside the v0.5 read-service baseline.

## v0.5.0 Release Notes

The v0.5.0 release expands DVault from the SQLite-first v0.4.x baseline into a coordinated six-package release covering the provider-neutral package plus SQLite, PostgreSQL, SQL Server, Oracle, and MySQL provider packages. The release focuses on provider-specific write optimizations, deferred Data Vault capability baselines, read access for latest/as-of satellite rows, and package-publication readiness.

Notable user-facing changes:

- Provider packages for MySQL, Oracle, PostgreSQL, SQLite, and SQL Server are version-aligned with the core package.
- Provider-specific save strategies optimize clean-context write paths for the supported provider shapes, with provider-neutral fallback when a strategy declines.
- Load-timestamp storage can be projected as provider default, ISO 8601 UTC text, or UTC ticks.
- Satellite latest-state indexes now respect provider capabilities for included columns, avoiding MySQL hash-diff key expansion where measurements showed it to be counterproductive.
- `IDataVaultReadService` adds latest/as-of satellite reads over generated Data Vault tables.
- The provider-neutral fallback writer now filters persisted satellite latest-state checks by parent hash key instead of loading whole satellite tables.
- Deferred capability baselines cover multi-active satellite driving keys, PIT metadata projection, bridge metadata projection, provider capability profiles, and explicit API snapshots.

## Deferred Capabilities

PIT tables, bridge tables, and multi-active satellites remain opt-in v0.5 capabilities rather than part of ordinary hub, link, and satellite setup. Multi-active satellite support is limited to the driving-key modeling, projection, and explicit-save baseline described above. The current bridge documentation baseline is in `docs/plans/deferred-data-vault-capabilities.md`; it reflects the implemented `DataVaultBridgeMetadata`/`DataVaultMetadataModel.Bridges` metadata surface and the bounded `ApplyDataVaultMetadata()` projection for many-to-many endpoint hash-key bridges and hierarchy bridges with `TraversalDepth`. Bridge row population, traversal maintenance, generated EF relationships or navigations, provider-specific DDL beyond the generated EF model, PIT interactions, and multi-active interactions remain future scope.

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
- `Category=ProviderIntegration.ExternalOptIn`: live external database integration coverage, currently Postgres, SQL Server, Oracle, and MySQL.

To make the default local provider boundary explicit in a focused run, exclude opt-in external database tests:

```sh
dotnet test DVault.slnx --nologo --filter "Category!=ProviderIntegration.ExternalOptIn"
```

## Benchmarks

Run the local SQLite scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The benchmark executable compares conventional EF and DVault flows for the shared customer profile history contract, a larger customer profile bulk-history contract, and the reduced order-product fulfillment history contract. It uses SQLite temporary files by default and does not require Postgres, SQL Server, Oracle, MySQL, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, or `DVAULT_TEST_MYSQL_CONNECTION_STRING`.
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

## Optional Local SQL Server Integration Tests

SQL Server integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require SQL Server, Docker, or checked-in machine-specific configuration.

To run the SQL Server smoke lane, provide a developer-managed SQL Server database connection string in `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` and run the representative repo-root command:

```sh
DVAULT_TEST_SQLSERVER_CONNECTION_STRING='Server=localhost;Database=dvault_tests;User Id=dvault;Password=local-secret;TrustServerCertificate=True' dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer
```

The configured SQL Server principal must be able to create and drop temporary `dvault_test_*` schemas and tables in the target database. The tests create isolated schemas, validate one hub save, one link save, and one satellite save through the optimized SQL Server provider strategy, then drop the generated schema. Missing `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` produces a deterministic skip message instead of loading the conditional SQL Server provider package.

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
