# DVault EF Compiled Compatibility

Status: v1 implementation note
Ticket: 06F1XPYA9MD0T9C4651ND8KX0W

## Decision

DVault v1 supports Entity Framework Core compiled-model usage when the application supplies an EF runtime model through `UseModel(...)` and that runtime model was initialized from a design model that already has DVault metadata projected into it. DVault does not add a compiled-model generator, design-time service, custom `dotnet ef` command, or provider-specific compiled-model tooling for this boundary.

DVault also supports EF compiled queries for stable direct EF query shapes over generated Data Vault shared-type tables. The supported shape is a normal EF query expression with scalar parameters, generated table names, `EF.Property<T>(...)` access to generated columns, and a deterministic projection. The flexible `IDataVaultReadService` request APIs remain the default path for dynamic read requests.

SQLite is the required local compatibility baseline for this proof. Other providers keep the same provider-neutral metadata and query-expression boundary, but this note does not claim a provider matrix for compiled models or compiled queries.

## Compiled Model Pattern

The supported pattern is:

1. Build a normal `DbContext` model with `ApplyDataVaultMetadata(...)`, `UseDataVaultMetadata(...)`, or another documented DVault projection path.
2. Resolve EF's design model from the configured context.
3. Initialize a runtime model with EF's runtime model initializer.
4. Build runtime options with the same provider and `UseModel(runtimeModel)`.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

static IModel CreateRuntimeModel(DbContext designContext) {
  var designModel = designContext.GetService<IDesignTimeModel>().Model;

  return designContext.GetService<IModelRuntimeInitializer>()
      .Initialize(designModel, designTime: false, validationLogger: null);
}

var runtimeModel = CreateRuntimeModel(designContext);
var runtimeOptions = new DbContextOptionsBuilder<SalesVaultContext>()
    .UseSqlite(connectionString)
    .UseModel(runtimeModel)
    .Options;
```

DVault metadata annotations are expected to remain available after EF runtime-model initialization for the shared metadata projection path. The compatibility proof covers model-level metadata source annotations, entity annotations for kind, metadata name, and produced name, property-role annotations for business keys and technical columns, and technical column role annotations such as `RecordSource`.

Applications that use EF's generated compiled-model code own that generated artifact in their application project. DVault does not generate or customize that artifact in v1.

## Compiled Query Pattern

Compiled queries should use stable generated table and column names. For generated shared-type Data Vault tables, use `context.Set<Dictionary<string, object>>(producedName)` plus `EF.Property<T>(...)` inside the compiled expression:

```csharp
using Microsoft.EntityFrameworkCore;

private static readonly Func<SalesVaultContext, string, OrderRead> ReadHubOrderByHashKey =
    EF.CompileQuery((SalesVaultContext context, string orderHashKey) =>
        context.Set<Dictionary<string, object>>("HubOrder")
            .AsNoTracking()
            .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
            .Select(row => new OrderRead(
                EF.Property<string>(row, "OrderHashKey"),
                EF.Property<string>(row, "OrderId"),
                EF.Property<string>(row, "RecordSource")))
            .Single());
```

This pattern is appropriate when the table name, column names, filter shape, ordering, and projection are known at compile time except for scalar query parameters. It can be used with data seeded through `IDataVaultSaveService` as long as the compiled query reads the generated EF table shape directly.

Direct typed read projections may also be compiled when they are ordinary EF-translatable expressions with stable shape. Keep the compiled query boundary at the EF query expression; do not expect DVault to compile arbitrary caller-owned request objects or runtime projector delegates.

## Unsupported Shapes And Diagnostics

EF compiled queries are not a replacement for the dynamic DVault read APIs. These shapes are outside the v1 compatibility claim:

- request-built `IDataVaultReadService` calls whose filters, satellite selection, point-in-time shape, bridge traversal, or projector delegate are assembled dynamically;
- arbitrary caller-owned delegates that EF cannot convert into a single translatable query expression;
- compiled queries that depend on provider-specific SQL behavior not already represented by normal EF translation;
- provider-specific compiled-model generation or design-time integration owned by DVault.

DVault does not add a separate diagnostic code for those unsupported compiled-query shapes. If an unsupported query is attempted, the expected diagnostic is the normal EF Core compile-time or translation exception for the expression. Use the flexible DVault read services for those dynamic cases.

## Validation And Benchmark Boundary

Repository compatibility coverage is carried by `tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs`. That test initializes an EF runtime model through `IModelRuntimeInitializer`, supplies it through `UseModel(...)`, verifies DVault annotations survive runtime-model initialization, seeds deterministic SQLite data through `IDataVaultSaveService`, and reads the generated `HubOrder` shared-type table through `EF.CompileQuery`.

Existing focused SQLite integration tests continue to cover non-compiled EF save and read usage through the explicit save service, normal EF generated-table reads, typed satellite reads, PIT reads, and bridge reads.

No compiled-model or compiled-query performance claim is made by this note. The repository benchmark harness does not currently include stable, attributable compiled-model or compiled-query benchmark rows with provider and environment context. Treat the current evidence as compatibility proof only.
