# DVault

DVault is a focused .NET library family for Data Vault 2.x-oriented persistence on Entity Framework Core. It keeps the public surface close to EF Core: model metadata, generated DVault tables, explicit save/read services, provider-specific optimizations, analyzers, and source generators where they make EF usage safer or easier.

## Contents

- [Installation](#installation)
- [Quickstart](#quickstart)
- [Package Compatibility](#package-compatibility)
- [Documentation Map](#documentation-map)
- [Current v0.42.0 Limitations](#current-v0420-limitations)
- [Layout](#layout)
- [Local Validation](#local-validation)
- [License](#license)

## Installation

Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. Use exactly one package line for a consumer project: `8.42.0` for `net8.0` and EF Core 8, or `10.42.0` for `net10.0` and EF Core 10. Do not mix package lines, and do not use a consumer-facing `0.42.0` package version from the v0.42.0 release label. This documentation baseline does not by itself confirm package publication.

For `net8.0` projects on EF Core 8, use the `8.42.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 8.42.0
dotnet add package DCoding.Data.DVault.Db2 --version 8.42.0
dotnet add package DCoding.Data.DVault.Sqlite --version 8.42.0
dotnet add package DCoding.Data.DVault.Postgres --version 8.42.0
dotnet add package DCoding.Data.DVault.MySql --version 8.42.0
dotnet add package DCoding.Data.DVault.Oracle --version 8.42.0
dotnet add package DCoding.Data.DVault.SqlServer --version 8.42.0
```

For `net10.0` projects on EF Core 10, use the `10.42.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 10.42.0
dotnet add package DCoding.Data.DVault.Db2 --version 10.42.0
dotnet add package DCoding.Data.DVault.Sqlite --version 10.42.0
dotnet add package DCoding.Data.DVault.Postgres --version 10.42.0
dotnet add package DCoding.Data.DVault.MySql --version 10.42.0
dotnet add package DCoding.Data.DVault.Oracle --version 10.42.0
dotnet add package DCoding.Data.DVault.SqlServer --version 10.42.0
```

Add the analyzer package only to projects that own DVault declarations, compile-time generated row mappings, or generated typed read helpers, and keep it local with `PrivateAssets="all"`. Build projects that reference `DCoding.Data.DVault.Analyzers` with a `.NET 10 SDK` host, including `net8.0` projects using the `8.42.0` package line. The current analyzer package carries one `net10.0` analyzer asset for both coordinated package lines; this repository does not validate pure `.NET 8 SDK` analyzer consumption.

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="8.42.0" PrivateAssets="all" />
</ItemGroup>
```

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="10.42.0" PrivateAssets="all" />
</ItemGroup>
```

Applications still need their normal Entity Framework Core provider package, such as `IBM.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Sqlite`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, or a MySQL EF Core provider.

## Quickstart

Use `AddDVault(options => options.UseBinaryFirstProfile())` plus the provider extension package that matches the configured EF Core provider. For direct Code-First model projection, call `UseDataVaultBinaryFirstProfile()` before applying DVault metadata. DVault persistence stays explicit: generated hub, link, and satellite rows are written through `IDataVaultSaveService`; ordinary EF entity tracking remains under the application's control.

The binary-first profile is the recommended physical storage profile for new projects. Existing databases and configurations are not migrated automatically; `HexString`-compatible setups remain valid until the application owner intentionally plans and executes a separate reviewed migration, reset, or data-move change. Logical and public hash-key values remain lowercase hexadecimal strings even when new projects choose binary physical storage.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDVault(options => options.UseBinaryFirstProfile());
services.AddDVaultSqlite();

services.AddDbContext<SalesVaultContext>(options =>
    options.UseSqlite("Data Source=sales-vault.db"));
```

Declare Data Vault metadata in `OnModelCreating` with Code-First metadata, or provide a reviewed `DataVaultMetadataModel` / `DataVaultMetadataRegistry` when metadata should be shared across schema projection, saves, reads, diagnostics, and generated helper surfaces.

```csharp
public sealed class SalesVaultContext(DbContextOptions<SalesVaultContext> options) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.UseDataVaultBinaryFirstProfile();
    modelBuilder.ApplyDataVaultMetadata(vault => {
      vault.Hub<Customer>(hub => {
        hub.BusinessKey(customer => customer.CustomerId);
        hub.Satellite("Profile", satellite => {
          satellite.Payload(customer => customer.Name);
          satellite.Payload(customer => customer.Status);
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
```

Callers own load timestamps, record sources, ordering, transactions, and the moment a DVault write happens.

```csharp
var saveService = serviceProvider.GetRequiredService<IDataVaultSaveService>();

await saveService.SaveAsync(
    context,
    new DataVaultSaveRequest(
        DateTimeOffset.UtcNow,
        "crm-import",
        [
            new(new DataVaultHubMetadata("Customer", ["CustomerId"]), [new("CustomerId", "C-100")]),
        ],
        []),
    cancellationToken);
```

Provider packages can add optimized strategies behind the same public service contract. The shared surface also includes chunked/async saves, latest/as-of satellite reads, PIT and bridge maintenance/read services, diagnostics and explain metadata, support-bundle export, model-first governance, Roslyn analyzers, and opt-in typed read-model generation.

For runnable examples and fuller workflows, see [Getting Started](docs/getting-started.md), [examples/README.md](examples/README.md), and the current [DVault v0.42.0 Release Notes](docs/releases/v0.42.0.md).

## Package Compatibility

The current coordinated package baseline is documented in [Package Compatibility](docs/package-compatibility.md) and [DVault v0.42.0 Release Notes](docs/releases/v0.42.0.md). DVault has eight packable packages, two visible consumer package lines, target-specific dependency pins, and a local analyzer package boundary.

In short:

- `8.42.0` targets `net8.0` and the EF Core 8 dependency line.
- `10.42.0` targets `net10.0` and the EF Core 10 dependency line.
- `v0.42.0` is a repository release tag and release-note label, not a NuGet package version.
- `DCoding.Data.DVault.Analyzers` remains a local `PrivateAssets="all"` analyzer reference and currently requires a `.NET 10 SDK` build host for both package lines.
- Hash-key storage guidance from v0.36.0 carries forward: logical public hash keys remain lowercase hexadecimal strings, while binary physical storage remains explicit opt-in.

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
| Hashing and hash-key storage contracts | [Stable Hashing Contract](docs/plans/stable-hashing-contract.md) and [Hash-Key Storage Profile Contract](docs/plans/hash-key-storage-profile-contract.md) |

## Current v0.42.0 Limitations

- DVault is an EF Core library family, not a platform, scheduler, ingestion service, CLI, or provider provisioning tool.
- Package publication remains a manual release operation; this repository records package creation and verification, not NuGet publication.
- The analyzer package is validated against the `.NET 10 SDK` build-host baseline for both coordinated package lines; pure `.NET 8 SDK` analyzer consumption is not a current compatibility claim.
- Stored procedures and provider-specific SQL artifacts are not default write paths. Any artifact lane is explicit, design-time, review-owned, and outside normal persistence.
- Binary hash-key storage is opt-in physical storage. Public hash-key values remain lowercase hexadecimal strings.
- DB2 live-schema reading is available as external opt-in evidence through `IBM.EntityFrameworkCore`; DB2 databases, credentials, lifecycle cleanup, and CI isolation remain consumer-owned.
- Live PostgreSQL, SQL Server, Oracle, MySQL, and DB2 integration tests are opt-in behind local `DVAULT_TEST_*` connection strings. Default validation does not require external databases or containers.

## Layout

- `DVault.slnx`: root build and test solution.
- `src/DCoding.Data.DVault/`: provider-neutral runtime package.
- `src/DCoding.Data.DVault.*`: provider extension packages for DB2, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, plus the analyzer/source-generator package.
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

`bash tools/pack-release-packages.sh` creates the two coordinated package lines under `artifacts/packages/`: eight `8.42.0` packages with `net8.0` assets and EF Core 8 dependency groups, and eight `10.42.0` packages with `net10.0` assets and EF Core 10 dependency groups. `bash tools/verify-packages.sh` inspects those artifacts, expects exactly sixteen DVault `.nupkg` files plus fourteen matching symbol packages for the runtime/provider packages, checks README, XML documentation, analyzer assets, declared NuGet metadata, and confirms each provider package depends on the packed `DCoding.Data.DVault` version from the same package line.

For provider-specific filters, environment variables, benchmark commands, and package-verification details, see [Local Validation](docs/local-validation.md).

## License

DVault uses the Apache License 2.0. See [LICENSE](LICENSE).
