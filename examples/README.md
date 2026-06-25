# DVault Quickstart Examples

The root [README quickstart](../README.md#quickstart) and [Getting Started](../docs/getting-started.md) page are the shortest SQLite-first path for a new binary-first project. These runnable companion examples run the same bounded customer-profile history flow through the public registry-backed metadata path:

- `DCoding.Data.DVault.SqliteQuickstart` uses SQLite through `AddDVaultSqlite()` and needs no external infrastructure.
- `DCoding.Data.DVault.PostgresQuickstart` uses PostgreSQL through `AddDVaultPostgres()` and a developer-managed connection string.

Both projects register one shared `DataVaultMetadataModel` with `AddDVault(options => options.UseBinaryFirstProfile().UseMetadataModel(...))`, opt the DbContext into that registry with `UseDataVaultMetadata()`, create the sample schema with EF Core for the quickstart run, write through `IDataVaultSaveService`, read typed latest/as-of satellite projections through `IDataVaultReadService`, and print bounded request-level diagnostics from the current DVault diagnostics services.

The runnable quickstarts show the recommended binary-first physical storage profile for new projects. Existing databases and configurations are not migrated automatically; `HexString`-compatible setups remain valid until the application owner intentionally plans and executes a separate reviewed migration, reset, or data-move change. Logical and public hash-key values remain lowercase hexadecimal strings even when the new generated schema stores hash-key columns in binary form.

The checked-in examples use project references so they can build against the current repository checkout. Published consumer applications should install the same coordinated NuGet package family described in the root [README installation guidance](../README.md#installation).

## Customer Profile Scenario

Both runnable projects execute the same compact EF Core plus DVault scenario:

- one `Customer` hub identified by a synthetic customer business key;
- one `CustomerProfile` satellite with `Profile Name` and `Customer Status` payload fields;
- an initial CRM import at `2026-04-29T10:15:00Z` with record source `crm-import`;
- a later CRM change at `2026-04-29T11:30:00Z` with record source `crm-change`;
- explicit hub and satellite writes through `IDataVaultSaveService`; and
- typed latest and as-of reads through `IDataVaultReadService`.

The console output keeps diagnostics sanitized. Diagnostic lines report request-level strategy status, selected strategy name when one is selected, fallback presence, and latest/as-of read-shape category. They do not print raw SQL, connection strings, business keys, hash keys, payload values, provider message text, exception text, support-bundle content, exporter endpoints, or deployment instructions.

## Package And Provider Setup

Consumer applications install the provider-neutral package and exactly one provider package for the database they use. For PostgreSQL, install `DCoding.Data.DVault.Postgres` plus the normal EF Core provider package `Npgsql.EntityFrameworkCore.PostgreSQL`. Install `DCoding.Data.DVault.Privacy` only for the explicit privacy proof or a later opt-in privacy flow. Keep every DVault package on one aligned version. Use `8.47.0` for `net8.0` and EF Core 8 projects, or `10.47.0` for `net10.0` and EF Core 10 projects; do not use a consumer-facing `0.47.0` package version from the v0.47.0 release label. The blocks below list the full coordinated package family so each needed package can be copied from one aligned line.

For `net8.0` projects on EF Core 8, use the `8.47.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 8.47.0
dotnet add package DCoding.Data.DVault.Db2 --version 8.47.0
dotnet add package DCoding.Data.DVault.Sqlite --version 8.47.0
dotnet add package DCoding.Data.DVault.Postgres --version 8.47.0
dotnet add package DCoding.Data.DVault.MySql --version 8.47.0
dotnet add package DCoding.Data.DVault.Oracle --version 8.47.0
dotnet add package DCoding.Data.DVault.SqlServer --version 8.47.0
dotnet add package DCoding.Data.DVault.Privacy --version 8.47.0
```

For `net10.0` projects on EF Core 10, use the `10.47.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 10.47.0
dotnet add package DCoding.Data.DVault.Db2 --version 10.47.0
dotnet add package DCoding.Data.DVault.Sqlite --version 10.47.0
dotnet add package DCoding.Data.DVault.Postgres --version 10.47.0
dotnet add package DCoding.Data.DVault.MySql --version 10.47.0
dotnet add package DCoding.Data.DVault.Oracle --version 10.47.0
dotnet add package DCoding.Data.DVault.SqlServer --version 10.47.0
dotnet add package DCoding.Data.DVault.Privacy --version 10.47.0
```

Applications also need the normal Entity Framework Core provider package for their database, such as `IBM.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Sqlite`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, or a MySQL EF Core provider.

The analyzer package is optional and should usually be referenced with `PrivateAssets="all"` in consumer projects that own DVault Code-First declarations or compile-time generated row mapping declarations. Use `8.47.0` or `10.47.0` to match the runtime and provider package line.

Provider startup is explicit. Register `AddDVault(...)` with the binary-first profile and the shared metadata model for the provider-neutral services, then register the matching provider extension when a provider package is installed. The runnable projects keep the metadata model in shared code so SQLite and PostgreSQL use the same scenario; the model shape is:

```csharp
var customerHub = new DataVaultHubMetadata("Customer", ["Customer Id"]);
var customerProfile = new DataVaultSatelliteMetadata(
    "CustomerProfile",
    customerHub.ToReference(),
    ["Profile Name", "Customer Status"]);
var metadataModel = new DataVaultMetadataModel(
    [customerHub],
    [],
    [customerProfile]);
```

SQLite registration then stays binary-first and provider-explicit:

```csharp
services.AddDVault(options => options
    .UseBinaryFirstProfile()
    .UseMetadataModel(metadataModel));
services.AddDVaultSqlite();
services.AddDbContext<QuickstartVaultContext>(
    options => options
        .UseSqlite(connectionString)
        .UseDataVaultMetadata());
```

The PostgreSQL quickstart uses the same shape with `AddDVaultPostgres()` and `UseNpgsql(connectionString)`. Other provider packages expose the matching `AddDVaultDb2()`, `AddDVaultSqlServer()`, `AddDVaultOracle()`, and `AddDVaultMySql()` startup extensions, but these examples only provide runnable SQLite and PostgreSQL projects.

## Optional Privacy Proof

The optional privacy proof is documented in [Getting Started](../docs/getting-started.md#optional-privacy-proof). It shows the provider-neutral `DCoding.Data.DVault.Privacy` package using `AddDVaultPrivacy(...)`, `RegisterEncryptedPayloadAlias(...)`, a caller-owned provider passed through `UseCallerOwnedKeyProvider(...)`, and `DataVaultEncryptedPayloadValueConverter` on a payload property.

Keep the type boundary explicit when adapting that example: `UseCallerOwnedKeyProvider(...)` accepts `IDataVaultPrivacyKeyProvider`, while encrypted payload conversion requires the configured provider to also implement `IDataVaultEncryptedPayloadKeyProvider`. The alias registered at runtime should match the model-first `personalData[].encryptedPayloadAlias` value for the logical payload. Missing alias registration, missing provider wiring, providers that do not satisfy the encrypted-payload interface, and declined conversions fail closed rather than storing plaintext or treating ciphertext as decrypted payload data.

The quickstart privacy proof is provider-neutral and SQLite-friendly because it uses ordinary EF Core value conversion over a mapped payload property. It is not a GDPR/DSGVO compliance guarantee, automatic encryption or redaction feature, provider-native encryption feature, encrypted-column DDL contract, deletion workflow, PIT or bridge cleanup workflow, backup purge, retention completion, legal-erasure completion, or DVault-owned key lifecycle.

## Observability Examples

`AddDVault(...)` is telemetry-free by default. It does not add counters, `ActivityListener` instances, exporters, dashboards, collectors, hosting, or OpenTelemetry package requirements. Applications opt into each observability surface they want to own.

Built-in save/read metrics use the `System.Diagnostics.Metrics` observer path. Register the provider-neutral services first, then add `AddDVaultTelemetry()`:

```csharp
using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;

services.AddDVault(options => options
    .UseBinaryFirstProfile()
    .UseMetadataModel(QuickstartHistoryFlow.MetadataModel));
services.AddDVaultTelemetry();
```

The built-in meter name is `DCoding.Data.DVault`. Applications that need custom bounded summaries can also register `IDataVaultTelemetryObserver` implementations; those observers are a sibling opt-in surface, not a tracing prerequisite.

Activity tracing is listener-driven and does not require `AddDVaultTelemetry()`. Register an `ActivityListener`, OpenTelemetry tracing provider, or equivalent application-owned listener for the `DCoding.Data.DVault` ActivitySource:

```csharp
using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

using var listener = new ActivityListener {
  ShouldListenTo = source => source.Name == "DCoding.Data.DVault",
  Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
      ActivitySamplingResult.AllDataAndRecorded,
  ActivityStopped = activity => Console.WriteLine(activity.DisplayName),
};

ActivitySource.AddActivityListener(listener);

services.AddDVault(options => options
    .UseBinaryFirstProfile()
    .UseMetadataModel(QuickstartHistoryFlow.MetadataModel));
```

For OpenTelemetry-style application wiring, keep DVault to the source and meter names while the application chooses packages, exporters, sampling, hosting, and backends:

```csharp
// Pseudo-code only: use the observability package owned by the application.
applicationObservability.Configure(options => {
  options.TraceSources.Add("DCoding.Data.DVault");
  options.Meters.Add("DCoding.Data.DVault");
});
```

The authoritative ActivitySource, span, event, tag, sampling, omission, and redaction rules live in [DVault V1 Activity Tracing Contract](../docs/architecture/dvault-v1-activity-tracing-contract.md). For v0.31 performance and observability posture, keep the quickstarts aligned with [Performance Profiles](../docs/performance-profiles.md): they show request-level DVault diagnostics and opt-in telemetry/tracing hooks, not a hosted observability stack, dashboard, exporter, collector, database-provisioning flow, automatic PIT or bridge maintenance job, orchestration sample, provider-specific SQL artifact, physical-plan contract, or new runtime routing promise. Example output and telemetry sinks must stay sanitized: no raw business keys or hash keys, payload values, SQL text, connection strings, provider messages, exception text, stack traces, support-bundle content, exporter endpoints, or deployment instructions.

## Build

From the repository root:

```sh
dotnet build DVault.slnx --nologo
```

## Run SQLite

```sh
dotnet run --project examples/DCoding.Data.DVault.SqliteQuickstart/DCoding.Data.DVault.SqliteQuickstart.csproj
```

The SQLite quickstart creates a temporary SQLite database file, creates the DVault schema, writes one customer profile twice with distinct load timestamps and record sources, then prints sanitized typed latest/as-of read summaries and bounded DVault diagnostics. This is the default proof path and requires no external database, container runtime, hosted worker, scheduler, dashboard, exporter, collector, or telemetry backend.

## Run PostgreSQL

Set `DVAULT_TEST_POSTGRES_CONNECTION_STRING` to a developer-managed PostgreSQL connection string, then run:

```sh
dotnet run --project examples/DCoding.Data.DVault.PostgresQuickstart/DCoding.Data.DVault.PostgresQuickstart.csproj
```

The PostgreSQL quickstart uses `UseBinaryFirstProfile()`, `AddDVaultPostgres()`, and the same `UseDataVaultMetadata()` registry-backed DbContext path as SQLite. It creates the DVault schema in the database named by the connection string and runs the same explicit save, typed read, and bounded diagnostics flow.

For a local Podman or Docker fixture that can supply this connection string, see `examples/DCoding.Data.DVault.PostgresQuickstart/README.md`. The fixture remains opt-in; default `dotnet test` execution does not require PostgreSQL, Docker, or Podman.

If `DVAULT_TEST_POSTGRES_CONNECTION_STRING` is missing or empty, the PostgreSQL quickstart exits successfully before opening a database connection and prints:

```text
Skipping PostgreSQL quickstart. Set DVAULT_TEST_POSTGRES_CONNECTION_STRING to a developer-managed PostgreSQL connection string and rerun this example.
```

## Model Declaration Path

The quickstarts intentionally use the metadata-first path so one `DataVaultMetadataModel` drives schema projection, explicit saves, and typed reads. The same model declares a `Customer` hub with a `Customer Id` business key and a `CustomerProfile` satellite with `Profile Name` and `Customer Status` payload fields.

Code-First and model-first adoption remain compatible alternatives:

- Use Code-First metadata when a model is local to one EF model and fits the fluent hub, hub-parent satellite, link-parent satellite, multi-active driving-key, explicit or derived link, and repeated same-hub role-bearing link surface in the root [README quickstart](../README.md#quickstart).
- Use model-first governance when a reviewed `dvault.model.v1` JSON artifact should be imported, projected into EF metadata, exported canonically, and compared against generated metadata. Follow [Model-First Governance Workflow](../docs/model-first-governance.md).

Choose one authoritative declaration path for each model boundary. Do not mix multiple metadata authorities for the same EF model. The runnable quickstarts stay metadata-first; the README and current release notes document Code-First same-hub roles, link-parent satellites, and explicit bulk ingestion without adding separate runnable Code-First or bulk-ingestion quickstart projects.

## Save And Read Flow

The shared quickstart flow writes through `IDataVaultSaveService` with registry-backed requests:

- the first request saves the `Customer` hub with the CRM import UTC load timestamp and `crm-import` record source;
- the second request saves the imported `CustomerProfile` satellite version for the same customer hub;
- the third request saves the changed `CustomerProfile` satellite version with the later UTC load timestamp and `crm-change` record source;
- the read step uses `IDataVaultReadService.ReadLatestSatelliteAsync(...)` for both latest and as-of typed projections.

The minimal SQLite proof keeps the schema creation, explicit save, and latest read visible at the service boundary:

```csharp
var initialLoadTimestamp = new DateTimeOffset(2026, 4, 29, 10, 15, 0, TimeSpan.Zero);
var customerParent = DataVaultMetadataReference.Hub("Customer");

await context.Database.EnsureCreatedAsync(cancellationToken);

var hubResult = await saveService.SaveAsync(
    context,
    new DataVaultRegistrySaveRequest(
        initialLoadTimestamp,
        "crm-import",
        [
            new DataVaultRegistryHubSaveOperation(
                "Customer",
                [new("Customer Id", "C-100")]),
        ],
        []),
    cancellationToken);
var customerHashKey = hubResult.SavedRecords.Single(record =>
    record.Kind == DataVaultTableKind.Hub &&
    record.MetadataName == "Customer").HashKey;

await saveService.SaveAsync(
    context,
    new DataVaultRegistrySaveRequest(
        initialLoadTimestamp,
        "crm-import",
        [],
        [],
        [
            new DataVaultRegistrySatelliteSaveOperation(
                customerParent,
                "CustomerProfile",
                customerHashKey,
                [
                    new("Profile Name", "Alice Adams"),
                    new("Customer Status", "prospect"),
                ],
                "customer-profile-import"),
        ]),
    cancellationToken);

var latestRows = await readService.ReadLatestSatelliteAsync(
    context,
    new DataVaultRegistryLatestSatelliteReadRequest(
        customerParent,
        "CustomerProfile",
        [customerHashKey]),
    row => new {
      ProfileName = row.RequiredString("Profile Name"),
      CustomerStatus = row.RequiredString("Customer Status"),
      LoadTimestamp = row.RequiredDateTimeOffset("LoadTimestamp"),
      RecordSource = row.RequiredString("RecordSource"),
    },
    cancellationToken);
var latestProfile = latestRows.Single();
```

The checked-in flow repeats the satellite save with the later `2026-04-29T11:30:00Z` load timestamp and `crm-change` record source, then performs an as-of read by passing that cutoff timestamp to `DataVaultRegistryLatestSatelliteReadRequest`.

This keeps the write boundary explicit. The examples do not rely on ordinary EF entity tracking to create DVault rows, and they do not hide Data Vault persistence behind `SaveChanges`.

`UseDataVaultSaveChangesMetadataInterceptor(...)` is supported as an optional metadata-only path for applications that already track generated DVault rows themselves. It fills missing `LoadTimestamp` and `RecordSource` values on added generated hub, link, or satellite rows; it does not create rows, compute hash keys, compute hash diffs, or replace `IDataVaultSaveService`. The quickstarts avoid the interceptor so the default explicit save boundary stays visible.

## Migration Guardrails And Drift Checks

These quickstarts create disposable example schemas with EF Core so the projects remain small and directly runnable. Production applications should own migrations in the consumer project that owns the configured `DbContext`, design-time factory, and preflight entrypoint. DVault does not ship a `dotnet ef` shim, intercept EF CLI commands, auto-run migrations, or apply schema repairs.

Use the v1 design-time workflow for production migration guardrails. It includes the GitHub Actions baseline for pre-integration checks, and the reusable command host is invoked from the consumer project:

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- validate
dotnet run --project src/SalesVault/SalesVault.csproj -- drift --artifact src/SalesVault/dvault.model.v1
dotnet run --project src/SalesVault/SalesVault.csproj -- guardrail --migration AddCustomerProfile
```

The drift command uses a committed reviewed artifact when one exists. `export` is for artifact maintenance or reviewed refresh workflows, not the default blocking CI gate.

1. Build the same configured `DbContext` that EF design-time commands use.
2. Run DVault diagnostics against the configured model before applying migrations.
3. Scaffold migrations through normal EF Core commands owned by the consumer project.
4. Run `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)` against generated migration operations before applying the migration.

For model-first or metadata-first review evidence, compare the reviewed artifact or metadata model against generated EF metadata with `DataVaultModelDriftReporter.Compare(...)`.

Live-schema drift evidence is intentionally bounded. `DataVaultLiveSchemaReader.ReadAsync(context)` and `DataVaultLiveSchemaDriftReporter.Compare(...)` provide built-in successful catalog-reader coverage for SQLite, PostgreSQL, SQL Server, Oracle, DB2, and MySQL. `IBM.EntityFrameworkCore` maps to the DB2 reader; both `MySql.EntityFrameworkCore` and `Pomelo.EntityFrameworkCore.MySql` map to the MySQL reader. Keep PostgreSQL, SQL Server, Oracle, DB2, and MySQL live checks as external opt-in evidence because the consumer application still owns reachable databases, connection strings, credentials, lifecycle cleanup, and CI isolation.

See [DVault Dotnet EF Design-Time Workflow](../docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [Model-First Governance Workflow](../docs/model-first-governance.md), and the [Production Adoption Checklist](../docs/production-adoption-checklist.md) before promoting a quickstart shape into a production application.
