# DVault Quickstart Examples

These examples run the same bounded customer-profile history flow through the public registry-backed metadata path:

- `DCoding.Data.DVault.SqliteQuickstart` uses SQLite through `AddDVaultSqlite()` and needs no external infrastructure.
- `DCoding.Data.DVault.PostgresQuickstart` uses PostgreSQL through `AddDVaultPostgres()` and a developer-managed connection string.

Both projects register one shared `DataVaultMetadataModel` with `AddDVault(options => options.UseMetadataModel(...))`, opt the DbContext into that registry with `UseDataVaultMetadata()`, create the sample schema with EF Core for the quickstart run, write through `IDataVaultSaveService`, and read typed latest/as-of satellite projections through `IDataVaultReadService`.

The checked-in examples use project references so they can build against the current repository checkout. Published consumer applications should install the same coordinated NuGet package family described in the root [README installation guidance](../README.md#installation).

## Package And Provider Setup

Consumer applications install the provider-neutral package and exactly one provider package for the database they use. Keep every DVault package on one aligned version:

```sh
dotnet add package DCoding.Data.DVault --version 0.10.0
dotnet add package DCoding.Data.DVault.Sqlite --version 0.10.0
dotnet add package DCoding.Data.DVault.Postgres --version 0.10.0
dotnet add package DCoding.Data.DVault.MySql --version 0.10.0
dotnet add package DCoding.Data.DVault.Oracle --version 0.10.0
dotnet add package DCoding.Data.DVault.SqlServer --version 0.10.0
dotnet add package DCoding.Data.DVault.Analyzers --version 0.10.0
```

Applications also need the normal Entity Framework Core provider package for their database, such as `Microsoft.EntityFrameworkCore.Sqlite`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, or a MySQL EF Core provider.

The analyzer package is optional and should usually be referenced with `PrivateAssets="all"` in consumer projects that own DVault Code-First declarations.

Provider startup is explicit. Register `AddDVault()` for the provider-neutral services, then register the matching provider extension when a provider package is installed:

```csharp
services.AddDVault(options => options.UseMetadataModel(QuickstartHistoryFlow.MetadataModel));
services.AddDVaultSqlite();
services.AddDbContext<QuickstartVaultContext>(
    options => options
        .UseSqlite(connectionString)
        .UseDataVaultMetadata());
```

The PostgreSQL quickstart uses the same shape with `AddDVaultPostgres()` and `UseNpgsql(connectionString)`. Other provider packages expose the matching `AddDVaultSqlServer()`, `AddDVaultOracle()`, and `AddDVaultMySql()` startup extensions, but these examples only provide runnable SQLite and PostgreSQL projects.

## Build

From the repository root:

```sh
dotnet build DVault.slnx --nologo
```

## Run SQLite

```sh
dotnet run --project examples/DCoding.Data.DVault.SqliteQuickstart/DCoding.Data.DVault.SqliteQuickstart.csproj
```

The SQLite quickstart creates a temporary SQLite database file, creates the DVault schema, writes one customer profile twice with distinct load timestamps, then prints the latest profile and the as-of profile from the first timestamp.

## Run PostgreSQL

Set `DVAULT_TEST_POSTGRES_CONNECTION_STRING` to a developer-managed PostgreSQL connection string, then run:

```sh
dotnet run --project examples/DCoding.Data.DVault.PostgresQuickstart/DCoding.Data.DVault.PostgresQuickstart.csproj
```

The PostgreSQL quickstart uses `AddDVaultPostgres()` plus the same `UseDataVaultMetadata()` registry-backed DbContext path as SQLite. It creates the DVault schema in the database named by the connection string and runs the same typed save/read flow.

For a local Podman or Docker fixture that can supply this connection string, see `examples/DCoding.Data.DVault.PostgresQuickstart/README.md`. The fixture remains opt-in; default `dotnet test` execution does not require PostgreSQL, Docker, or Podman.

If `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is missing or empty, the PostgreSQL quickstart exits successfully before opening a database connection and prints:

```text
Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.
```

## Model Declaration Path

The quickstarts intentionally use the metadata-first path so one `DataVaultMetadataModel` drives schema projection, explicit saves, and typed reads. The same model declares a `Customer` hub with a `Customer Id` business key and a `CustomerProfile` satellite with `Profile Name` and `Customer Status` payload fields.

Code-First and model-first adoption remain compatible alternatives:

- Use Code-First metadata when a model is local to one EF model and fits the fluent hub, satellite, multi-active driving-key, and ordered link surface in the root [README quickstart](../README.md#quickstart).
- Use model-first governance when a reviewed `dvault.model.v1` JSON artifact should be imported, projected into EF metadata, exported canonically, and compared against generated metadata. Follow [Model-First Governance Workflow](../docs/model-first-governance.md).

Choose one authoritative declaration path for each model boundary. Do not mix multiple metadata authorities for the same EF model.

## Save And Read Flow

The shared quickstart flow writes through `IDataVaultSaveService` with registry-backed requests:

- the first request saves the `Customer` hub with an explicit UTC load timestamp and `quickstart` record source;
- the second and third requests save two `CustomerProfile` satellite versions for the same customer hash key;
- the read step uses `IDataVaultReadService.ReadLatestSatelliteAsync(...)` for both latest and as-of typed projections.

This keeps the write boundary explicit. The examples do not rely on ordinary EF entity tracking to create DVault rows, and they do not hide Data Vault persistence behind `SaveChanges`.

`UseDataVaultSaveChangesMetadataInterceptor(...)` is supported as an optional metadata-only path for applications that already track generated DVault rows themselves. It fills missing `LoadTimestamp` and `RecordSource` values on added generated hub, link, or satellite rows; it does not create rows, compute hash keys, compute hash diffs, or replace `IDataVaultSaveService`. The quickstarts avoid the interceptor so the default explicit save boundary stays visible.

## Migration Guardrails And Drift Checks

These quickstarts create disposable example schemas with EF Core so the projects remain small and directly runnable. Production applications should own migrations in the consumer project that owns the configured `DbContext`, design-time factory, and preflight entrypoint. DVault does not ship a `dotnet ef` shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs.

Use the v1 design-time workflow for production migration guardrails:

1. Build the same configured `DbContext` that EF design-time commands use.
2. Run DVault diagnostics against the configured model before applying migrations.
3. Scaffold migrations through normal EF Core commands owned by the consumer project.
4. Run `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)` against generated migration operations before applying the migration.

For model-first or metadata-first review evidence, compare the reviewed artifact or metadata model against generated EF metadata with `DataVaultModelDriftReporter.Compare(...)`.

Live-schema drift evidence is intentionally bounded. SQLite is the supported v1 live-schema reader through `DataVaultLiveSchemaReader.ReadAsync(context)` and `DataVaultLiveSchemaDriftReporter.Compare(...)`. PostgreSQL, SQL Server, Oracle, and MySQL live-schema readers are not first-class supported readers in this slice; use external opt-in evidence for those providers when a team requires live database checks.

See [DVault Dotnet EF Design-Time Workflow](../docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [Model-First Governance Workflow](../docs/model-first-governance.md), and the [Production Adoption Checklist](../docs/production-adoption-checklist.md) before promoting a quickstart shape into a production application.
