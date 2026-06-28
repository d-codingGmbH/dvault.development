# DVault

DVault is a focused .NET library family for Data Vault 2.x-oriented persistence on Entity Framework Core. It keeps the public surface close to EF Core: model metadata, generated DVault tables, explicit save/read services, provider-specific optimizations, analyzers, and source generators where they make EF usage safer or easier.

## Contents

- [Installation](#installation)
- [Quickstart](#quickstart)
- [Package Compatibility](#package-compatibility)
- [Documentation Map](#documentation-map)
- [Current v0.49.0 Limitations](#current-v0490-limitations)
- [Layout](#layout)
- [Local Validation](#local-validation)
- [License](#license)

## Installation

Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The blocks below list the full coordinated package family so each needed package can be copied from one aligned line. Use exactly one package line for a consumer project: `8.50.0` for `net8.0` and EF Core 8, or `10.50.0` for `net10.0` and EF Core 10. Do not mix package lines, and do not use a consumer-facing `0.49.0` package version from the v0.49.0 release label. This documentation baseline does not by itself confirm package publication.

For `net8.0` projects on EF Core 8, use the `8.50.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 8.50.0
dotnet add package DCoding.Data.DVault.Db2 --version 8.50.0
dotnet add package DCoding.Data.DVault.Sqlite --version 8.50.0
dotnet add package DCoding.Data.DVault.Postgres --version 8.50.0
dotnet add package DCoding.Data.DVault.MySql --version 8.50.0
dotnet add package DCoding.Data.DVault.Oracle --version 8.50.0
dotnet add package DCoding.Data.DVault.SqlServer --version 8.50.0
dotnet add package DCoding.Data.DVault.Privacy --version 8.50.0
```

For `net10.0` projects on EF Core 10, use the `10.50.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 10.50.0
dotnet add package DCoding.Data.DVault.Db2 --version 10.50.0
dotnet add package DCoding.Data.DVault.Sqlite --version 10.50.0
dotnet add package DCoding.Data.DVault.Postgres --version 10.50.0
dotnet add package DCoding.Data.DVault.MySql --version 10.50.0
dotnet add package DCoding.Data.DVault.Oracle --version 10.50.0
dotnet add package DCoding.Data.DVault.SqlServer --version 10.50.0
dotnet add package DCoding.Data.DVault.Privacy --version 10.50.0
```

Install `DCoding.Data.DVault.Privacy` only when the application explicitly opts into the privacy extension seam. The package is a provider-neutral proof for registration, options, and alias-driven encrypted payload conversion over ordinary EF Core mapped payload properties; it does not make an application compliant, enable automatic encryption or redaction, provide database-at-rest encryption, or use provider-native encrypted column/cell/row features.

Privacy provider caveats stay inside the finite repository-backed provider baseline: SQLite, PostgreSQL, SQL Server, MySQL, Oracle, and DB2. MySQL means the repository MySQL profile for `MySql.EntityFrameworkCore` and Pomelo, not a separate MariaDB capability profile. Provider-native features such as SQL Server TDE or Always Encrypted, PostgreSQL deployment encryption or `pgcrypto`, Oracle TDE or `DBMS_CRYPTO`, MySQL SQL crypto or file or tablespace encryption, SQLite encrypted-file builds, and DB2 native database encryption remain guidance-only. DVault does not emit provider-native encrypted DDL, call provider SQL crypto functions, probe provider encryption capabilities, or route runtime behavior based on native encryption availability; future native encryption support needs a separate provider-specific ticket or contract.

Add the analyzer package only to projects that own DVault declarations, compile-time generated row mappings, or generated typed read helpers, and keep it local with `PrivateAssets="all"`. Build projects that reference `DCoding.Data.DVault.Analyzers` with a `.NET 10 SDK` host, including `net8.0` projects using the `8.50.0` package line. The current analyzer package carries one `net10.0` analyzer asset for both coordinated package lines; this repository does not validate pure `.NET 8 SDK` analyzer consumption.

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="8.50.0" PrivateAssets="all" />
</ItemGroup>
```

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="10.50.0" PrivateAssets="all" />
</ItemGroup>
```

Applications still need their normal Entity Framework Core provider package, such as `IBM.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Sqlite`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, or a MySQL EF Core provider.

## Quickstart

The shortest new-project path is SQLite-first and binary-first. Use `AddDVault(options => options.UseBinaryFirstProfile())` plus `AddDVaultSqlite()` alongside the application's ordinary `UseSqlite(...)` `DbContext` configuration. For direct Code-First model projection, call `ApplyDataVaultMetadataWithBinaryFirstProfile(...)` when declaring the fluent model for a new binary-first schema. The existing `UseDataVaultBinaryFirstProfile()` plus `ApplyDataVaultMetadata(...)` setup remains supported. DVault persistence stays explicit: generated hub, link, and satellite rows are written through `IDataVaultSaveService`; ordinary EF entity tracking remains under the application's control.

The binary-first profile is the recommended physical storage profile for new projects. Existing databases and configurations are not migrated automatically; `HexString`-compatible setups remain valid until the application owner intentionally plans and executes a separate reviewed migration, reset, or data-move change. Use the [Hash-Key Storage Migration Guide](docs/hash-key-storage-migration.md) and its `dvault.hash-key-storage-migration.v1` dry-run manifest before changing persisted hash-key storage. Logical and public hash-key values remain lowercase hexadecimal strings even when new projects choose binary physical storage.

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDVault(options => options.UseBinaryFirstProfile());
services.AddDVaultSqlite();

services.AddDbContext<SalesVaultContext>(options =>
    options.UseSqlite("Data Source=sales-vault.db"));
```

For PostgreSQL, install `DCoding.Data.DVault.Postgres` with the matching DVault package line and the normal EF Core provider package `Npgsql.EntityFrameworkCore.PostgreSQL`, then use the same binary-first posture with `AddDVaultPostgres()` and `UseNpgsql(connectionString)`:

```csharp
services.AddDVault(options => options.UseBinaryFirstProfile());
services.AddDVaultPostgres();

services.AddDbContext<SalesVaultContext>(options =>
    options.UseNpgsql(connectionString));
```

The runnable PostgreSQL quickstart and live PostgreSQL integration tests are opt-in behind `DVAULT_TEST_POSTGRES_CONNECTION_STRING`. See [examples/README.md](examples/README.md#run-postgresql), [examples/DCoding.Data.DVault.PostgresQuickstart/README.md](examples/DCoding.Data.DVault.PostgresQuickstart/README.md), and [Local Validation](docs/local-validation.md#postgresql) for the existing local fixture and validation flow. DVault does not provision PostgreSQL containers, databases, users, credentials, or deployment infrastructure.

Declare Data Vault metadata in `OnModelCreating` with Code-First metadata, or provide a reviewed `DataVaultMetadataModel` / `DataVaultMetadataRegistry` when metadata should be shared across schema projection, saves, reads, diagnostics, and generated helper surfaces.

```csharp
public sealed class SalesVaultContext(DbContextOptions<SalesVaultContext> options) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadataWithBinaryFirstProfile(vault => {
      vault.Hub<Customer>(hub => {
        hub.BusinessKey(customer => customer.CustomerId);
        hub.Satellite("Profile", satellite => {
          satellite.Payload(customer => customer.Name);
          satellite.Payload(customer => customer.Status);
        });
      });
    });
  }
}
```

Create or migrate the schema through the application's normal EF Core path. For a minimal disposable quickstart database, `EnsureCreatedAsync(...)` is enough to make the generated DVault tables visible before the first save.

Callers own load timestamps, record sources, ordering, transactions, deterministic satellite hash diffs, and the moment a DVault write happens.

```csharp
using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
using var scope = serviceProvider.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<SalesVaultContext>();
var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
var readService = scope.ServiceProvider.GetRequiredService<IDataVaultReadService>();
var loadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
var customerHub = new DataVaultHubMetadata("Customer", ["CustomerId"]);
var customerProfile = new DataVaultSatelliteMetadata(
    "Profile",
    customerHub.ToReference(),
    ["Name", "Status"]);

await context.Database.EnsureCreatedAsync(cancellationToken);

var hubResult = await saveService.SaveAsync(
    context,
    new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [
            new DataVaultHubSaveOperation(
                customerHub,
                [new("CustomerId", "C-100")]),
        ],
        []),
    cancellationToken);
var customerHashKey = hubResult.SavedRecords.Single(record =>
    record.Kind == DataVaultTableKind.Hub &&
    record.MetadataName == "Customer").HashKey;

await saveService.SaveAsync(
    context,
    new DataVaultSaveRequest(
        loadTimestamp,
        "crm-import",
        [],
        [],
        [
            new DataVaultSatelliteSaveOperation(
                customerProfile,
                customerHashKey,
                [
                    new("Name", "Alice Adams"),
                    new("Status", "prospect"),
                ],
                "profile-hash-001"),
        ]),
    cancellationToken);

var latestProfiles = await readService.ReadLatestSatelliteAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(customerProfile, [customerHashKey]),
    row => new {
      Name = row.RequiredString("Name"),
      Status = row.RequiredString("Status"),
      LoadTimestamp = row.RequiredDateTimeOffset("LoadTimestamp"),
      RecordSource = row.RequiredString("RecordSource"),
    },
    cancellationToken);
var latestProfile = latestProfiles.Single();
```

Provider packages can add optimized strategies behind the same public service contract. The shared surface also includes chunked/async saves, latest/as-of satellite reads, PIT and bridge maintenance/read services, diagnostics and explain metadata, support-bundle export, model-first governance, Roslyn analyzers, and opt-in typed read-model generation.

For runnable examples, the optional privacy proof, and fuller workflows, see [Getting Started](docs/getting-started.md), [examples/README.md](examples/README.md), and the current [DVault v0.49.0 Release Notes](docs/releases/v0.49.0.md).

## Package Compatibility

The current coordinated package baseline is documented in [Package Compatibility](docs/package-compatibility.md) and [DVault v0.49.0 Release Notes](docs/releases/v0.49.0.md). DVault has nine packable packages, two visible consumer package lines, target-specific dependency pins, a local analyzer package boundary, and an optional privacy proof package.

In short:

- `8.50.0` targets `net8.0` and the EF Core 8 dependency line.
- `10.50.0` targets `net10.0` and the EF Core 10 dependency line.
- `v0.49.0` is a repository release tag and release-note label, not a NuGet package version.
- `DCoding.Data.DVault.Analyzers` remains a local `PrivateAssets="all"` analyzer reference and currently requires a `.NET 10 SDK` build host for both package lines.
- `DCoding.Data.DVault.Privacy` remains optional and opt-in; it provides registration and alias-driven encrypted payload conversion seams over ordinary EF Core mapped payload properties only, not compliance, automatic privacy execution, database-at-rest encryption, provider-native encrypted column/cell/row features, provider SQL crypto calls, encrypted DDL, capability probing, or runtime routing based on native encryption availability.
- Hash-key storage guidance now routes new projects to binary-first setup and existing persisted `HexString` setups to the migration guide and dry-run manifest path.
- Generated link mappers support repeated same-hub links only when every binding uses a distinct explicit produced participant name; ambiguous same-hub mappings and dependent child key modeling stay outside the current public surface.

## Documentation Map

| Need | Start here |
| --- | --- |
| Release history | [CHANGELOG.md](CHANGELOG.md) and [docs/releases/](docs/releases/) |
| Package lines and dependency matrix | [Package Compatibility](docs/package-compatibility.md) |
| First implementation pass | [Getting Started](docs/getting-started.md) and [examples/README.md](examples/README.md) |
| Production readiness | [Production Adoption Checklist](docs/production-adoption-checklist.md) |
| Local build, tests, packages, provider test gates | [Local Validation](docs/local-validation.md) |
| Manual package publication | [Manual NuGet Publication Checklist](docs/manual-nuget-publication.md) |
| Performance evidence and tuning boundaries | [Performance Profiles](docs/performance-profiles.md) and [benchmarks/](benchmarks/) |
| Model-first workflow | [Model-First Governance](docs/model-first-governance.md) |
| PIT and bridge boundary | [DVault V1 PIT And Bridge Boundary](docs/architecture/dvault-v1-pit-bridge-boundary.md) |
| Typed PIT/bridge helper boundary | [DVault V1 Typed PIT And Bridge Helper Contract](docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md) |
| Read-plan diagnostics and redaction | [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md) |
| Analyzer and generator scope | [Analyzer README](src/DCoding.Data.DVault.Analyzers/README.md) |
| Optional privacy proof and boundary | [Getting Started privacy proof](docs/getting-started.md#optional-privacy-proof), [examples privacy notes](examples/README.md#optional-privacy-proof), and [DVault V1 Optional Privacy Extension Boundary](docs/architecture/dvault-v1-optional-privacy-extension-boundary.md) |
| Hashing and hash-key storage contracts | [Stable Hashing Contract](docs/plans/stable-hashing-contract.md), [Hash-Key Storage Profile Contract](docs/plans/hash-key-storage-profile-contract.md), and [Hash-Key Storage Migration Guide](docs/hash-key-storage-migration.md) |

## Current v0.49.0 Limitations

- DVault is an EF Core library family, not a platform, scheduler, ingestion service, CLI, or provider provisioning tool.
- Package publication remains a manual release operation; this repository records package creation and verification, not NuGet publication.
- The analyzer package is validated against the `.NET 10 SDK` build-host baseline for both coordinated package lines; pure `.NET 8 SDK` analyzer consumption is not a current compatibility claim.
- Stored procedures and provider-specific SQL artifacts are not default write paths. Any artifact lane is explicit, design-time, review-owned, and outside normal persistence.
- Binary hash-key storage is opt-in physical storage for new schemas. Existing persisted `HexString` storage changes require a reviewed migration, reset, or data-move plan; public hash-key values remain lowercase hexadecimal strings.
- Generated link mappers support repeated same-hub links through explicit produced participant names only. DVault does not infer ambiguous same-hub roles, add model-first same-hub mapper generation, or add dependent child key modeling in this release.
- DB2 live-schema reading is available as external opt-in evidence through `IBM.EntityFrameworkCore`; DB2 databases, credentials, lifecycle cleanup, and CI isolation remain consumer-owned.
- Live PostgreSQL, SQL Server, Oracle, MySQL, and DB2 integration tests are opt-in behind local `DVAULT_TEST_*` connection strings. Default validation does not require external databases or containers.

## Layout

- `DVault.slnx`: root build and test solution.
- `src/DCoding.Data.DVault/`: provider-neutral runtime package.
- `src/DCoding.Data.DVault.*`: provider extension packages for DB2, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, plus the optional privacy proof and analyzer/source-generator packages.
- `tests/DCoding.Data.DVault.Tests/`: unit, integration, public API, package-verifier, and provider test projects.
- `examples/`: runnable SQLite and PostgreSQL quickstarts.
- `benchmarks/`: local benchmark projects.
- `docs/`: release notes, architecture, planning, quality, validation, and adoption documentation.

## Local Validation

Run the repository validation lane from a .NET 10 SDK checkout:

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
bash tools/pack-release-packages.sh
bash tools/verify-packages.sh
bash tools/check-format.sh
```

`bash tools/pack-release-packages.sh` creates the two coordinated package lines under `artifacts/packages/`: nine `8.50.0` packages with `net8.0` assets and EF Core 8 dependency groups, and nine `10.50.0` packages with `net10.0` assets and EF Core 10 dependency groups. `bash tools/verify-packages.sh` inspects those artifacts, expects exactly eighteen DVault `.nupkg` files plus sixteen matching symbol packages for the runtime, provider, and privacy packages, checks README, XML documentation, analyzer assets, declared NuGet metadata, and confirms each provider and privacy package depends on the packed `DCoding.Data.DVault` version from the same package line.

For provider-specific filters, environment variables, benchmark commands, and package-verification details, see [Local Validation](docs/local-validation.md).

## License

DVault uses the Apache License 2.0. See [LICENSE](LICENSE).
