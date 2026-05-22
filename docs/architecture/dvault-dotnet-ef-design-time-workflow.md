# DVault Dotnet EF Design-Time Workflow

Status: v1 implementation note
Ticket: 06F1XPVPKVGYKCV04PY98TSS78

## Decision

DVault v1 supports one `dotnet ef` composition boundary: the application that owns the configured `DbContext` also owns an Entity Framework Core `IDesignTimeDbContextFactory<TContext>` and a small preflight entrypoint. The factory builds the same configured `DbContext` that normal EF design-time commands use. The preflight entrypoint constructs that context through the factory, runs DVault diagnostics against the configured model, and optionally analyzes the scaffolded migration operations before the migration is applied.

The DVault package does not provide `IDesignTimeServices`, does not provide a custom `dotnet ef` shim, does not intercept EF CLI commands, and does not reference `Microsoft.EntityFrameworkCore.Design`. Any EF design package reference belongs in the consumer project that owns the factory and invokes `dotnet ef`.

## Supported Layout

The supported v1 layout is a single project:

```text
src/SalesVault/SalesVault.csproj
src/SalesVault/SalesVaultContext.cs
src/SalesVault/SalesVaultDesignTimeFactory.cs
src/SalesVault/SalesVaultDvaultPreflight.cs
src/SalesVault/Migrations/
```

That project owns:

- the concrete `DbContext`;
- DVault metadata registration through either `ApplyDataVaultMetadata(...)` or `UseDataVaultMetadata(...)`;
- the consumer-owned `IDesignTimeDbContextFactory<TContext>`;
- the consumer-owned preflight entrypoint;
- the `dotnet ef migrations add` and `dotnet ef database update` invocation point.

Startup-project and target-project splits, host discovery from a separate executable, and other multi-project design-time layouts are unsupported in v1. A later ticket may add a broader layout, but this workflow deliberately keeps the design-time boundary local to the project that owns the configured context.

## Package Boundary

A consumer project that runs `dotnet ef` can reference EF Core design tooling as an application dependency:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.7">
  <PrivateAssets>all</PrivateAssets>
</PackageReference>
```

`src/DCoding.Data.DVault/DCoding.Data.DVault.csproj` remains design-package-free. Provider packages can continue to supply provider-specific DVault startup extensions, but DVault itself does not ship an EF CLI integration surface for this story.

## Consumer-Owned Factory

The factory should construct the same context shape used by migrations. The example below uses Code-First metadata, but registry-backed and model-first metadata are also supported when the produced `DbContext` already has DVault metadata projected into its EF model.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public sealed class SalesVaultContext(DbContextOptions<SalesVaultContext> options) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(vault => {
      vault.Hub<Customer>(hub => {
        hub.BusinessKey(customer => customer.CustomerId);
        hub.Satellite("Profile", satellite => {
          satellite.Payload(customer => customer.CustomerName);
          satellite.Payload(customer => customer.CustomerStatus);
        });
      });
    });
  }
}

public sealed class SalesVaultDesignTimeFactory : IDesignTimeDbContextFactory<SalesVaultContext> {
  public SalesVaultContext CreateDbContext(string[] args) {
    var options = new DbContextOptionsBuilder<SalesVaultContext>()
        .UseSqlite("Data Source=sales-vault-design-time.db")
        .Options;

    return new SalesVaultContext(options);
  }
}
```

For registry-backed metadata, keep the same single-project ownership and opt the factory-created context into the registry:

```csharp
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public sealed class SalesVaultDesignTimeFactory : IDesignTimeDbContextFactory<SalesVaultContext> {
  public SalesVaultContext CreateDbContext(string[] args) {
    var metadataModel = SalesVaultMetadata.CreateModel();
    var optionsBuilder = new DbContextOptionsBuilder<SalesVaultContext>()
        .UseSqlite("Data Source=sales-vault-design-time.db");

    optionsBuilder.UseDataVaultMetadata(DataVaultMetadataRegistry.Create(metadataModel));

    return new SalesVaultContext(optionsBuilder.Options);
  }
}
```

## Consumer-Owned Command Host

DVault exposes `DataVaultDesignTimeCommand` and `DataVaultDesignTimeCommandHost` so consumers can keep one small executable entrypoint in the project that owns the configured `DbContext`, design-time factory, migrations, and metadata source. The package still does not ship a standalone `dvault` CLI or intercept `dotnet ef`; the host below is application code that wires the reusable DVault command runner to application-owned dependencies.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;

using var services = new ServiceCollection()
    .AddDVault()
    .BuildServiceProvider(validateScopes: true);

var diagnostics = services.GetRequiredService<IDataVaultDiagnosticsService>();
var host = new DataVaultDesignTimeCommandHost(
    diagnostics,
    () => new SalesVaultDesignTimeFactory().CreateDbContext(args),
    DataVaultDesignTimeExportSource.FromMetadataModel(SalesVaultMetadata.CreateModel()),
    ResolveMigrationOperations);

return DataVaultDesignTimeCommand.Run(args, Console.Out, Console.Error, host);

static IEnumerable<MigrationOperation> ResolveMigrationOperations(string migrationName) {
  var migrationType = typeof(SalesVaultContext).Assembly
      .GetTypes()
      .SingleOrDefault(type =>
          typeof(Migration).IsAssignableFrom(type) &&
          string.Equals(type.Name, migrationName, StringComparison.Ordinal));

  if (migrationType is null) {
    throw new InvalidOperationException("Migration '" + migrationName + "' was not found.");
  }

  var migration = (Migration)Activator.CreateInstance(migrationType)!;
  return migration.UpOperations;
}
```

`DataVaultDesignTimeExportSource` should point at the same Code-First declarations, metadata model, or metadata registry that the configured context uses. The `export` verb is for artifact maintenance and reviewed refresh workflows:

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- export --output src/SalesVault/dvault.model.v1
```

Do not make `export` the default blocking CI gate. A blocking pre-integration gate should validate the configured design-time model and compare it against an already reviewed artifact when that artifact exists.

## Support Bundle Export

The `support-bundle` verb emits one deterministic redacted JSON document for configuration and provider-behavior
troubleshooting:

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- support-bundle --output src/SalesVault/dvault-support-bundle.json
```

The default bundle constructs the configured design-time `DbContext`, runs `IDataVaultDiagnosticsService.Analyze(DbContext)`,
and serializes the resulting `DataVaultDiagnosticsResult` under the `dvault.support-bundle.v1` contract. That default path
does not open a live database connection and includes validation status, metadata source kind and fingerprint, provider name,
capability profile, provider-behavior profile, load-timestamp storage details, translated Data Vault entities and tables, and
any already-populated save/read strategy diagnostics. Request-bound read diagnostics may also carry the additive `readShape`
section, which records translated latest-satellite, PIT, or bridge query-shape facts without raw request values or SQL text.

When an application already has a representative save or read request, keep that request in application code and supply the
request-bound diagnostics through the host instead of having the generic command runner invent one:

```csharp
var host = new DataVaultDesignTimeCommandHost(
    diagnostics,
    () => new SalesVaultDesignTimeFactory().CreateDbContext(args),
    DataVaultDesignTimeExportSource.FromMetadataModel(SalesVaultMetadata.CreateModel()),
    ResolveMigrationOperations) {
  CreateSupportBundleDiagnostics = context => diagnostics.Analyze(
      context,
      SalesVaultRepresentativeRequests.CreateCustomerProfileSave()),
};
```

Opt-in sections stay explicit. `--artifact <path>` adds a `DataVaultModelDriftReport` from the reviewed artifact and current
design-time model. `--live-schema` adds a `DataVaultLiveSchemaReadResult`; with `--artifact`, the drift section is based on the
classified live-schema read result. Provider exception text and connection-string fragments are redacted from the exported JSON
while provider names, profile names, diagnostic codes, and metadata identifiers remain available for troubleshooting.

## Preflight Validation

Run DVault validation explicitly before deciding whether to apply a generated migration. The validation step constructs the configured `DbContext` through the same factory and analyzes the in-memory EF design-time model. It does not require opening a live database connection.

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- validate
```

The reusable command runner prints `DataVaultDiagnosticsResult.ToDisplayString()` and exits with a non-zero status when validation is invalid. The equivalent low-level shape is `IDataVaultDiagnosticsService.Analyze(DbContext)`.

```csharp
using DCoding.Data.DVault;
using Microsoft.Extensions.DependencyInjection;

public static class SalesVaultDvaultPreflight {
  public static int RunModelValidation(string[] args) {
    using var services = new ServiceCollection()
        .AddDVault()
        .BuildServiceProvider(validateScopes: true);

    var diagnostics = services.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new SalesVaultDesignTimeFactory().CreateDbContext(args);

    var result = diagnostics.Analyze(context);
    Console.WriteLine(result.ToDisplayString());

    return result.Validation.IsValid ? 0 : 1;
  }
}
```

Stable diagnostic identifiers come from the existing DVault diagnostics surfaces. Model validation uses the `DMV####` family and migration guardrails use the `DVM2xxx` family. Do not add new design-time-only diagnostic identifiers for this workflow.

## Artifact Drift Preflight

When the consumer project has a reviewed `dvault.model.v1` artifact committed to source control, compare that artifact against the configured design-time model as the default drift lane:

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- drift --artifact src/SalesVault/dvault.model.v1
```

This is an artifact-versus-design-time-model comparison. It fails when the reviewed artifact cannot be imported or when the current configured model has blocking differences from the artifact. If the project has not adopted a reviewed artifact yet, keep this lane disabled or skipped until the artifact exists; do not generate a fresh artifact in CI and compare against that newly generated output.

Live-schema drift is optional and non-default:

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- drift --artifact src/SalesVault/dvault.model.v1 --live-schema
```

Use the live-schema lane only inside the documented boundary. SQLite is the first-class local live-schema reader. PostgreSQL, SQL Server, Oracle, and MySQL have built-in reader dispatch, but their checks require external opt-in evidence with consumer-managed databases, credentials, lifecycle cleanup, and CI isolation rather than a default DVault-provided CI environment.

## Snapshot-Model Drift Preflight

When a consumer project owns an EF model snapshot, the consumer can materialize that snapshot as an `IReadOnlyModel` and pass it
to DVault without adding EF design tooling to the DVault package. `DataVaultModelDriftPreflightReporter.Compare(...)` compares
the authoritative DVault metadata, the configured `DbContext.Model` runtime surface, and the explicit snapshot model in one
structured report:

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore.Metadata;

using var context = new SalesVaultDesignTimeFactory().CreateDbContext(args);
IReadOnlyModel snapshotModel = SalesVaultSnapshotMaterializer.CreateSnapshotModel();

var report = DataVaultModelDriftPreflightReporter.Compare(
    SalesVaultMetadata.CreateModel(),
    context,
    snapshotModel);

Console.WriteLine(report.ToDisplayString());
return report.HasBlockingDifferences ? 1 : 0;
```

The report has separate `MetadataVersusRuntime`, `MetadataVersusSnapshotModel`, and `RuntimeVersusSnapshotModel` sections plus
an overall blocking status. The runtime lane deliberately uses `DbContext.Model`; the existing
`DataVaultModelDriftReporter.Compare(..., DbContext)` overloads remain the design-time model comparison path over EF's
`IDesignTimeModel`. Snapshot acquisition is consumer-owned: DVault accepts the materialized `IReadOnlyModel`, does not expose
EF `ModelSnapshot` as a public contract, and does not discover migrations or snapshot files.

## Migration Guardrail Preflight

The migration guardrail step runs after scaffolding and before applying the migration:

```sh
dotnet ef migrations add AddCustomerProfile --project src/SalesVault/SalesVault.csproj
dotnet run --project src/SalesVault/SalesVault.csproj -- guardrail --migration AddCustomerProfile
dotnet ef database update --project src/SalesVault/SalesVault.csproj
```

The preflight command is consumer-owned. It resolves the generated migration through the configured `DataVaultDesignTimeCommandHost`, passes the migration `UpOperations` to DVault, prints the deterministic guardrail summary, and fails the local command when guardrail findings exist.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

public static class SalesVaultDvaultPreflight {
  public static int RunMigrationGuardrails(string[] args) {
    if (args.Length != 1) {
      Console.Error.WriteLine("Pass the generated migration type name.");
      return 2;
    }

    using var services = new ServiceCollection()
        .AddDVault()
        .BuildServiceProvider(validateScopes: true);

    var diagnostics = services.GetRequiredService<IDataVaultDiagnosticsService>();
    using var context = new SalesVaultDesignTimeFactory().CreateDbContext(args);

    var migrationType = Type.GetType(args[0], throwOnError: true)!;
    var migration = (Migration)Activator.CreateInstance(migrationType)!;
    var report = DataVaultMigrationOperationDiagnostics.AnalyzeReport(
        diagnostics,
        context,
        migration.UpOperations);

    Console.WriteLine(report.ToDisplayString());

    return report.HasFindings || !report.IsValid ? 1 : 0;
  }
}
```

This step does not promise guardrail output inside `dotnet ef migrations add` or `dotnet ef database update`; those commands remain ordinary EF Core commands. The consumer decides whether to continue to `database update` after reading the preflight summary.

`DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)` classifies each inspected operation as `Safe`, `Risky`, or `Incompatible`. A safe operation has no DVM findings. A risky operation has warning-severity DVM findings that should be reviewed before integration. An incompatible operation has one or more error-severity DVM findings and should block apply until the generated migration is corrected. The report keeps deterministic operation paths such as `migration/CreateTable/HubCustomer`, `migration/RenameColumn/HubCustomer/LoadTimestamp`, and `migration/DropTable/BridgeCustomerOrder` so a consumer-owned script can fail on incompatible output without parsing raw EF operation objects.

## Aggregate Preflight Facade

Consumers that want one application-owned entrypoint can aggregate the explicit lanes through `DataVaultPreflight.Run(...)`. The facade validates the configured model through `IDataVaultDiagnosticsService.Analyze(DbContext)` and evaluates only the optional inputs supplied on `DataVaultPreflightRequest`: reviewed artifact import, consumer-materialized snapshot model, migration operations, precomputed representative diagnostics, and representative diagnostics factories.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

public static class SalesVaultDvaultPreflight {
  public static int RunAggregatePreflight(string[] args) {
    using var services = new ServiceCollection()
        .AddDVault()
        .BuildServiceProvider(validateScopes: true);

    var diagnostics = services.GetRequiredService<IDataVaultDiagnosticsService>();
    var readDiagnostics = services.GetRequiredService<IDataVaultReadDiagnosticsService>();
    using var context = new SalesVaultDesignTimeFactory().CreateDbContext(args);

    Migration migration = SalesVaultMigrationResolver.Resolve("AddCustomerProfile");
    IReadOnlyModel snapshotModel = SalesVaultSnapshotMaterializer.CreateSnapshotModel();

    var report = DataVaultPreflight.Run(
        diagnostics,
        new DataVaultPreflightRequest(context, SalesVaultMetadata.CreateModel()) {
          ReviewedArtifactImport = SalesVaultArtifacts.ImportReviewedModel(),
          SnapshotModel = snapshotModel,
          MigrationOperations = migration.UpOperations,
          RepresentativeDiagnosticsRequests = [
            new DataVaultPreflightRepresentativeDiagnosticsRequest(
                "latest-profile",
                dbContext => readDiagnostics.Analyze(
                    dbContext,
                    SalesVaultRepresentativeRequests.CreateLatestProfileRead())),
          ],
        });

    Console.WriteLine(report.ToDisplayString());
    return report.IsBlocked ? 1 : 0;
  }
}
```

This facade does not change the ownership boundary. The consumer still owns the `DbContext`, factory, reviewed artifact path, snapshot materialization, migration resolution, representative request selection, command hosting, and CI failure policy. If an optional lane is not supplied, the aggregate report marks that lane as skipped; DVault does not scan the repository, discover EF snapshot files, discover migrations, invent representative save/read requests, open a live database, execute migrations, or repair schema.

## GitHub Actions Example

The following adopter workflow keeps the design-time checks in the consumer repository. It assumes `src/SalesVault/SalesVault.csproj` contains the configured `DbContext`, the `IDesignTimeDbContextFactory<TContext>`, the command host entrypoint shown above, and the EF migrations.

```yaml
name: DVault design-time checks

on:
  pull_request:
  workflow_dispatch:
    inputs:
      migration_name:
        description: "Optional migration name to scaffold and guard before apply."
        required: false
        type: string
        default: ""

permissions:
  contents: read

jobs:
  dvault-design-time:
    name: Validate model and reviewed artifact
    runs-on: ubuntu-latest
    env:
      DOTNET_NOLOGO: "true"
      DOTNET_CLI_TELEMETRY_OPTOUT: "true"
      CONSUMER_PROJECT: src/SalesVault/SalesVault.csproj
      REVIEWED_MODEL_ARTIFACT: src/SalesVault/dvault.model.v1
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x

      - name: Restore dependencies
        run: dotnet restore "$CONSUMER_PROJECT" --nologo

      - name: Build consumer project
        run: dotnet build "$CONSUMER_PROJECT" --no-restore --nologo

      - name: Validate configured DVault design-time model
        run: dotnet run --no-build --project "$CONSUMER_PROJECT" -- validate

      - name: Check drift against reviewed model artifact
        run: |
          if [ ! -f "$REVIEWED_MODEL_ARTIFACT" ]; then
            echo "No reviewed dvault.model.v1 artifact found at $REVIEWED_MODEL_ARTIFACT; skipping artifact drift gate."
            exit 0
          fi

          dotnet run --no-build --project "$CONSUMER_PROJECT" -- drift --artifact "$REVIEWED_MODEL_ARTIFACT"

  dvault-migration-guardrail:
    name: Scaffold migration and run guardrails
    if: ${{ github.event_name == 'workflow_dispatch' && github.event.inputs.migration_name != '' }}
    runs-on: ubuntu-latest
    env:
      DOTNET_NOLOGO: "true"
      DOTNET_CLI_TELEMETRY_OPTOUT: "true"
      CONSUMER_PROJECT: src/SalesVault/SalesVault.csproj
      MIGRATION_NAME: ${{ github.event.inputs.migration_name }}
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET SDK
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x

      - name: Restore dependencies
        run: dotnet restore "$CONSUMER_PROJECT" --nologo

      - name: Install EF CLI
        run: dotnet tool install --global dotnet-ef --version 10.0.7

      - name: Scaffold proposed migration
        run: dotnet ef migrations add "$MIGRATION_NAME" --project "$CONSUMER_PROJECT"

      - name: Run DVault migration guardrails
        run: dotnet run --project "$CONSUMER_PROJECT" -- guardrail --migration "$MIGRATION_NAME"
```

The validation step is the default blocking check. The drift step becomes blocking when a committed reviewed artifact is present, but it skips cleanly before a project adopts model-first artifact review. The guardrail job is separate because it runs after migration scaffolding and before any `dotnet ef database update` or integration step. It does not imply that DVault intercepts EF commands.

If an adopter adds a live-schema drift lane, keep it separate from the default job and mark it as SQLite-only or external-opt-in:

```yaml
- name: Optional SQLite live-schema drift check
  run: dotnet run --no-build --project "$CONSUMER_PROJECT" -- drift --artifact "$REVIEWED_MODEL_ARTIFACT" --live-schema
```

## Workflow Order

1. Keep the `DbContext`, DVault metadata registration, factory, and preflight entrypoint in the same project.
2. Build the factory-backed context and run `dotnet run --project <consumer-project> -- validate`; the reusable command host delegates to `IDataVaultDiagnosticsService.Analyze(DbContext)`.
3. Print `DataVaultDiagnosticsResult.ToDisplayString()` and stop when validation is invalid.
4. When a reviewed `dvault.model.v1` artifact exists, run `dotnet run --project <consumer-project> -- drift --artifact <path-to-reviewed-artifact>`.
5. Scaffold the migration normally with `dotnet ef migrations add`.
6. Run `dotnet run --project <consumer-project> -- guardrail --migration <migration-name>` against the proposed migration `MigrationOperation` set.
7. Print `DataVaultMigrationGuardrailReport.ToDisplayString()` and stop when guardrail findings exist.
8. Optionally run a consumer-owned aggregate entrypoint backed by `DataVaultPreflight.Run(...)` when the project wants one report that preserves validation, drift, migration, and representative diagnostics sections together.
9. Run `dotnet ef database update` only after the explicit preflight steps pass.

## Unsupported In V1

- DVault-owned `IDesignTimeServices` registration.
- DVault-owned `dotnet ef` command shims or EF CLI interception.
- Automatic migration guardrail output during `migrations add` or `database update`.
- Repo-owned `Microsoft.EntityFrameworkCore.Design` dependencies in DVault packages.
- Startup-project and target-project split layouts.
- Live-database validation as a default CI gate, automatic snapshot discovery, or provider-wide live schema drift as a first-class boundary beyond SQLite.
- Automatic reviewed-artifact discovery, migration discovery, or representative request generation for aggregate preflight.
- Provider-specific online migration runners.

The default no-live-database design-time proof remains the existing diagnostics and artifact-versus-design-time-model drift path. Broader command orchestration, broad snapshot-model documentation, and broader provider live schema drift work stay outside this v1 workflow.
