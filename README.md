# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Installation

Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is version-aligned:

```sh
dotnet add package DCoding.Data.DVault --version 0.6.0
dotnet add package DCoding.Data.DVault.Sqlite --version 0.6.0
dotnet add package DCoding.Data.DVault.Postgres --version 0.6.0
dotnet add package DCoding.Data.DVault.MySql --version 0.6.0
dotnet add package DCoding.Data.DVault.Oracle --version 0.6.0
dotnet add package DCoding.Data.DVault.SqlServer --version 0.6.0
```

Applications still need their normal Entity Framework Core database provider package, such as `Microsoft.EntityFrameworkCore.Sqlite` for SQLite, `Npgsql.EntityFrameworkCore.PostgreSQL` for PostgreSQL, `Pomelo.EntityFrameworkCore.MySql` for the DVault MySQL v1 path, or the relevant provider for SQL Server or Oracle.

Runnable SQLite and PostgreSQL quickstart projects are available under `examples/`; see `examples/README.md` for exact build and run commands.

## Quickstart

Use this flow in a .NET 10 project that references `DCoding.Data.DVault` and has an Entity Framework Core provider configured. The recommended v0.6.0 path is convention-first: register DVault, declare the Data Vault model through EF Code-First metadata, save explicitly through `IDataVaultSaveService`, and read common satellite views through typed latest/as-of projector helpers.

### Register DVault services

```csharp
using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddDVault();

using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
```

Provider packages can register provider-specific startup extensions alongside the provider-neutral services:

```csharp
services.AddDVaultSqlite();
services.AddDVaultPostgres();
services.AddDVaultSqlServer();
services.AddDVaultOracle();
services.AddDVaultMySql();
```

### Declare the EF model with Code-First metadata

Declare hubs, satellites, and links in `OnModelCreating` with `ApplyDataVaultMetadata(vault => ...)`. Business keys, driving keys, payload fields, and link participants use direct scalar member selectors. Composite keys use repeated calls in their canonical order.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

public sealed class SalesVaultContext(DbContextOptions<SalesVaultContext> options) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(vault => {
      vault.Hub<Customer>(hub => {
        hub.BusinessKey(customer => customer.CustomerId);
        hub.Satellite("Profile", satellite => {
          satellite.Payload(customer => customer.CustomerName);
          satellite.Payload(customer => customer.CustomerStatus);
        });
        hub.Satellite("ContactByType", satellite => {
          satellite.DrivingKey(customer => customer.ContactType);
          satellite.Payload(customer => customer.EmailAddress);
        });
      });

      vault.Hub<Order>(hub => hub.BusinessKey(order => order.OrderId));

      vault.Link("CustomerOrder", link => {
        link.Participant<Customer>();
        link.Participant<Order>();
      });
    });
  }
}

public sealed class Customer {
  public string CustomerId { get; set; } = string.Empty;
  public string CustomerName { get; set; } = string.Empty;
  public string CustomerStatus { get; set; } = string.Empty;
  public string ContactType { get; set; } = string.Empty;
  public string EmailAddress { get; set; } = string.Empty;
}

public sealed class Order {
  public string OrderId { get; set; } = string.Empty;
}
```

Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a public Code-First-to-registry bridge.

### Save explicitly

Persistence remains an explicit service boundary. `DataVaultSaveRequest` carries the load timestamp and record source, and callers choose when to write vault rows through `IDataVaultSaveService`. DVault does not intercept `SaveChanges` or hide persistence behind ordinary EF entity tracking.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

public static class SalesVaultWriter {
  public static async Task SaveCustomerOrderAsync(
      SalesVaultContext context,
      IServiceProvider serviceProvider,
      CancellationToken cancellationToken = default) {
    var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
    var order = new DataVaultHubMetadata("Order", ["OrderId"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["CustomerName", "CustomerStatus"]);
    var customerOrder = new DataVaultLinkMetadata(
        "CustomerOrder",
        [customer.ToReference(), order.ToReference()]);

    var loadTimestamp = new DateTimeOffset(2026, 5, 11, 10, 15, 0, TimeSpan.Zero);
    var saveService = serviceProvider.GetRequiredService<IDataVaultSaveService>();

    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [
                new(customer, [new("CustomerId", "C-100")]),
                new(order, [new("OrderId", "O-200")]),
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
            ],
            [
                new(
                    profile,
                    customerHashKey,
                    [new("CustomerName", "Alice Adams"), new("CustomerStatus", "Active")],
                    "customer-profile-active"),
            ]),
        cancellationToken);
  }
}
```

For loaders that already have multiple source batches prepared, `DataVaultBulkSaveRequest` processes ordered save requests through the same service and keeps satellite HashDiff state in memory across the batch.

### Read typed latest and as-of satellite projections

`IDataVaultReadService` provides provider-neutral latest and as-of satellite reads. The common path maps selected rows through a caller-owned projector delegate so application code can return typed read models without binding DTOs through reflection. Passing an `asOf` timestamp to `DataVaultLatestSatelliteReadRequest` selects the latest row visible at that point in time.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.Extensions.DependencyInjection;

var readService = serviceProvider.GetRequiredService<IDataVaultReadService>();
var customer = new DataVaultHubMetadata("Customer", ["CustomerId"]);
var profile = new DataVaultSatelliteMetadata(
    "Profile",
    customer.ToReference(),
    ["CustomerName", "CustomerStatus"]);

var latestProfiles = await readService.ReadLatestSatelliteAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey]),
    row => new CustomerProfileRead(
        row.RequiredString("ParentHashKey"),
        row.RequiredString("CustomerName"),
        row.RequiredString("CustomerStatus"),
        row.RequiredDateTimeOffset("LoadTimestamp")),
    cancellationToken);

var asOfProfiles = await readService.ReadLatestSatelliteAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], asOfTimestamp),
    row => new CustomerProfileRead(
        row.RequiredString("ParentHashKey"),
        row.RequiredString("CustomerName"),
        row.RequiredString("CustomerStatus"),
        row.RequiredDateTimeOffset("LoadTimestamp")),
    cancellationToken);

public sealed record CustomerProfileRead(
    string ParentHashKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset LoadTimestamp);
```

The lower-level `ReadLatestSatelliteRowsAsync(...)` API remains available as the advanced escape hatch. It returns `DataVaultSatelliteReadRecord` values containing the parent hash key, driving-key values, hash diff, load timestamp, record source, and payload values for callers that need row-level dictionaries or custom projections. PIT-backed read models, bridge traversal helpers, and provider-specific read strategies remain future extension points.

### Register metadata once and opt in a DbContext

The metadata-first `DataVaultMetadataModel` path remains supported and compatible for v0.5 users, shared metadata, examples, and advanced scenarios. Applications that want one authoritative metadata source can register a model or prebuilt registry during service setup and opt selected contexts into registry-backed projection through `DbContextOptionsBuilder`.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var customerProfile = new DataVaultSatelliteMetadata(
    "CustomerProfile",
    customer.ToReference(),
    ["Profile Name", "Customer Status"]);
var salesVaultMetadata = new DataVaultMetadataModel(
    [customer],
    [],
    [customerProfile]);

var services = new ServiceCollection();
services.AddDVault(options => options.UseMetadataModel(salesVaultMetadata));
services.AddDbContext<SalesVaultContext>(options => {
  options.UseSqlite(connectionString);
  options.UseDataVaultMetadata();
});

public sealed class SalesVaultContext(DbContextOptions<SalesVaultContext> options) : DbContext(options) {
}
```

`UseDataVaultMetadata()` consumes the app-level registry registered by `AddDVault(...)`. A context can override that default by passing an explicit `DataVaultMetadataModel` or `DataVaultMetadataRegistry` to `UseDataVaultMetadata(...)`. If the same EF model receives two different DVault metadata sources, model building fails with a DVault source-conflict diagnostic instead of merging or duplicating projection. The SQLite and PostgreSQL quickstarts intentionally use this registry-backed path; see `examples/README.md` for exact commands and provider setup.

### Diagnostics and explain output

`IDataVaultDiagnosticsService` can analyze metadata models, registries, Code-First declarations, and configured DbContexts. Validation and explain output can run without a save request. Provider-specific save-strategy dispatch diagnostics are request-bound, so strategy status remains not evaluated until a single save request or ordered bulk save request is supplied.

### Multi-active satellite opt-in

Ordinary satellites remain the default. A satellite becomes multi-active only when it declares one or more driving keys, and those names define the canonical identity tuple for each active satellite row. Driving-key values stay separate from payload values, and `hashDiff` continues to represent payload state for change detection rather than driving-key identity.

```csharp
modelBuilder.ApplyDataVaultMetadata(vault => {
  vault.Hub<Customer>(hub => {
    hub.BusinessKey(customer => customer.CustomerId);
    hub.Satellite("ContactByType", satellite => {
      satellite.DrivingKey(customer => customer.ContactType);
      satellite.DrivingKey(customer => customer.RegionCode);
      satellite.Payload(customer => customer.EmailAddress);
    });
  });
});
```

The save operation supplies driving-key values by logical name. Caller enumeration order does not matter; the values below are matched by name and persisted in the declared canonical order `ContactType`, then `RegionCode`:

```csharp
var contact = new DataVaultSatelliteMetadata(
    "ContactByType",
    customer.ToReference(),
    ["EmailAddress"],
    ["ContactType", "RegionCode"]);

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
                customerHashKey,
                [new("RegionCode", "DE"), new("ContactType", "billing")],
                [new("EmailAddress", "billing-de@example.test")],
                "contact-payload-hash"),
        ]),
    cancellationToken);
```

The provider-neutral projection stores driving-key columns immediately after the parent hash-key column and before `HashDiff`, `LoadTimestamp`, `RecordSource`, and payload columns. The satellite primary key and parent/latest-state index expand to include the driving-key tuple, so different driving-key tuples for the same parent can coexist. Current multi-active support is intentionally narrow and opt-in; future work includes PIT over multi-active satellites, bridge interactions involving multi-active state, link-based PIT support, and provider-specific optimized multi-active save behavior. Provider-specific save strategies may decline multi-active batches so the provider-neutral writer handles the documented baseline.

### Provider Packages

`DCoding.Data.DVault` contains the provider-neutral API, metadata model, naming conventions, stable hashing, read helpers, diagnostics, and EF fallback writer. Provider packages extend that base registration without changing the explicit save or read APIs.

`DCoding.Data.DVault.Sqlite` registers the optimized SQLite set-based save strategy and registers the SQLite EF provider name for the existing `ApplyDataVaultMetadata(...)` capability-profile selection path. `DCoding.Data.DVault.Postgres` registers an optimized Npgsql/PostgreSQL strategy for clean contexts that use set-based `INSERT ... ON CONFLICT DO NOTHING` hub and link writes plus latest-state satellite checks. `DCoding.Data.DVault.SqlServer` registers an optimized SQL Server strategy for clean contexts with set-based unique-row inserts and latest-state satellite checks. `DCoding.Data.DVault.Oracle` registers an Oracle-gated insert-only strategy for clean `Oracle.EntityFrameworkCore` hub/link batches and declines unsupported shapes, including dirty tracked contexts and request batches that contain satellite operations, so the provider-neutral fallback writer handles them. `DCoding.Data.DVault.MySql` targets `Pomelo.EntityFrameworkCore.MySql`, registers that Pomelo provider name for the existing `ApplyDataVaultMetadata(...)` capability-profile selection path, and registers an optimized MySQL strategy for clean Pomelo contexts.

Provider-specific save-strategy registration and provider-name capability-profile auto-registration are separate startup surfaces. The current startup extensions visibly auto-register provider names for SQLite and MySQL only. PostgreSQL, SQL Server, and Oracle still provide provider-specific save strategies and keep the provider-neutral fallback as the caller-visible safety net, but their startup extensions do not currently auto-register provider-name capability profiles.

### Migration from v0.5

v0.5 metadata-first `DataVaultMetadataModel` usage remains valid in v0.6.0. Existing applications can keep constructing metadata models, registering them with `AddDVault(options => options.UseMetadataModel(...))`, and opting DbContexts into `UseDataVaultMetadata()`.

New application code can prefer Code-First declarations when the Data Vault model fits the implemented hub-parent satellite and ordered hub-link surface. Keep metadata-first declarations for shared metadata registries, example-local quickstarts, link-parent satellite shapes, bridge/PIT metadata baselines, or naming requirements outside the bounded Code-First API.

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

The shared-type table names and columns in this quickstart follow DVault's default naming conventions, for example `HubCustomer`, `HubOrder`, `LinkCustomerOrder`, `CustomerHashKey`, `OrderHashKey`, `LoadTimestamp`, and `RecordSource`. Direct EF queries remain available for table-specific projections, custom joins, diagnostics, and cases outside the typed read helper baseline.

## v0.6.0 Release Notes

The v0.6.0 release updates the recommended usability flow for the coordinated six-package DVault family. See `docs/releases/v0.6.0.md` for the full release-note record, package scope, compatibility notes, known limitations, and validation evidence.

Notable user-facing changes:

- Code-First EF model declarations are now the README happy path for hubs, hub-parent satellites, multi-active driving keys, and ordered hub links.
- Metadata-first `DataVaultMetadataModel` and registry-backed `UseDataVaultMetadata()` remain supported and are still used by the runnable SQLite and PostgreSQL quickstarts.
- Typed latest/as-of satellite projector helpers provide the common read experience, while raw row reads remain available for advanced callers.
- Diagnostics and explain output cover metadata, registry, Code-First, and DbContext analysis, with save-strategy dispatch evaluated only when a save request is supplied.

## Deferred Capabilities

Model-first import/export specs, PIT-backed read APIs, bridge traversal helpers, PIT/bridge row maintenance, provider-specific read optimizations, and a public Code-First-to-registry bridge remain future work. Existing PIT and bridge metadata projection baselines remain advanced metadata surfaces rather than the recommended v0.6.0 README happy path.

## Layout

- `DVault.slnx`: Canonical root solution file for build and test automation.
- `src/DCoding.Data/`: Non-packable build anchor for the `DCoding.Data` source-root namespace family.
- `src/DCoding.Data.DVault/`: Main library project. The NuGet package id and root namespace are `DCoding.Data.DVault`.
- `src/DCoding.Data.DVault.*`: Provider extension packages for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- `tests/DCoding.Data.DVault.Tests/`: Unit, integration, and shared test projects for DVault.
- `examples/`: Runnable SQLite and PostgreSQL quickstart projects.
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
