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

## Preflight Validation

Run DVault validation explicitly before deciding whether to apply a generated migration. The validation step constructs the configured `DbContext` through the same factory and analyzes the in-memory EF design-time model. It does not require opening a live database connection.

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

## Migration Guardrail Preflight

The migration guardrail step runs after scaffolding and before applying the migration:

```sh
dotnet ef migrations add AddCustomerProfile
dotnet run -- dvault-preflight AddCustomerProfile
dotnet ef database update
```

The preflight command is consumer-owned. It resolves the generated migration, passes the migration `UpOperations` to DVault, prints the deterministic guardrail summary, and fails the local command when guardrail findings exist.

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

## Workflow Order

1. Keep the `DbContext`, DVault metadata registration, factory, and preflight entrypoint in the same project.
2. Build the factory-backed context and run `IDataVaultDiagnosticsService.Analyze(DbContext)`.
3. Print `DataVaultDiagnosticsResult.ToDisplayString()` and stop when validation is invalid.
4. Scaffold the migration normally with `dotnet ef migrations add`.
5. Run the consumer-owned migration guardrail preflight against the proposed migration `MigrationOperation` set.
6. Print `DataVaultMigrationGuardrailReport.ToDisplayString()` and stop when guardrail findings exist.
7. Run `dotnet ef database update` only after the explicit preflight steps pass.

## Unsupported In V1

- DVault-owned `IDesignTimeServices` registration.
- DVault-owned `dotnet ef` command shims or EF CLI interception.
- Automatic migration guardrail output during `migrations add` or `database update`.
- Repo-owned `Microsoft.EntityFrameworkCore.Design` dependencies in DVault packages.
- Startup-project and target-project split layouts.
- Live-database validation, model snapshot drift comparison, or schema drift reporting.
- Provider-specific online migration runners.

The no-live-database design-time proof remains the existing diagnostics and model-first drift path. Downstream model snapshot and live schema drift work stays outside this v1 workflow.
