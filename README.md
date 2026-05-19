# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Installation

Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family is version-aligned:

```sh
dotnet add package DCoding.Data.DVault --version 0.15.0
dotnet add package DCoding.Data.DVault.Sqlite --version 0.15.0
dotnet add package DCoding.Data.DVault.Postgres --version 0.15.0
dotnet add package DCoding.Data.DVault.MySql --version 0.15.0
dotnet add package DCoding.Data.DVault.Oracle --version 0.15.0
dotnet add package DCoding.Data.DVault.SqlServer --version 0.15.0
dotnet add package DCoding.Data.DVault.Analyzers --version 0.15.0
```

Applications still need their normal Entity Framework Core database provider package, such as `Microsoft.EntityFrameworkCore.Sqlite` for SQLite, `Npgsql.EntityFrameworkCore.PostgreSQL` for PostgreSQL, `Microsoft.EntityFrameworkCore.SqlServer` for SQL Server, `Oracle.EntityFrameworkCore` for Oracle, or `Pomelo.EntityFrameworkCore.MySql` / `MySql.EntityFrameworkCore` for MySQL.

`DCoding.Data.DVault.Analyzers` is optional developer tooling. Prefer `PrivateAssets="all"` for that package so analyzer and source-generator assets stay local to the project that declares DVault Code-First metadata or compile-time mapping declarations. See `src/DCoding.Data.DVault.Analyzers/README.md` for the package-local diagnostic, code-fix, source-generator, suppression, and configuration guidance.

Runnable SQLite and PostgreSQL quickstart projects are available under `examples/`; see `examples/README.md` for exact build and run commands.

For a short adopter readiness pass before production use, see the [Production Adoption Checklist](docs/production-adoption-checklist.md).

## Quickstart

Use this flow in a .NET 10 project that references `DCoding.Data.DVault` and has an Entity Framework Core provider configured. DVault supports three additive declaration paths:

- Code-First declarations for app-local EF models that fit the fluent hub, hub-parent satellite, link-parent satellite, multi-active driving-key, explicit or derived hub-link, and explicitly named repeated same-hub link surface.
- Metadata-first declarations through a shared `DataVaultMetadataModel` or `DataVaultMetadataRegistry` when one public metadata object should drive schema projection, explicit saves, typed latest/as-of reads, diagnostics, examples, or provider setup.
- Model-first governance for reviewed `dvault.model.v1` JSON artifacts that should be imported, projected into EF metadata, exported canonically, and compared against generated metadata for drift evidence.

Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-first-governance.md) for the current `dvault.model.v1` JSON artifact contract and [DVault EF Design-Time Workflow](docs/architecture/dvault-dotnet-ef-design-time-workflow.md) for the current consumer-owned design-time command workflow around reviewed artifacts, EF metadata, migrations, and live schema drift.

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
        link.Satellite<CustomerOrderState>("State", satellite => {
          satellite.DrivingKey(state => state.StateSource);
          satellite.Payload(state => state.StatusCode);
          satellite.Payload(state => state.StateChangedAt);
        });
      });

      vault.Link("CustomerIdentityMatch", link => {
        link.Participant<Customer>("SourceCustomer");
        link.Participant<Customer>("MatchedCustomer");
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

public sealed class CustomerOrderState {
  public string StateSource { get; set; } = string.Empty;
  public string StatusCode { get; set; } = string.Empty;
  public DateTimeOffset StateChangedAt { get; set; }
}
```

Code-First metadata is additive. It does not ask callers to put DVault hash-key, load-timestamp, or record-source technical fields on domain entities, and it does not create a public Code-First-to-registry bridge. Callers that want the same public metadata object reused by schema, save, and read paths should use the registry-backed metadata path shown below.

Code-First link names can be explicit or, for non-repeated hub participants, derived from participant order. When the same hub type appears more than once in one link, use an explicit relationship name and distinct `Participant<TEntity>(string role)` roles; those roles become participant names and generated hash-key columns such as `SourceCustomerHashKey` and `MatchedCustomerHashKey`. Link-parent satellites are declared inside `Link(..., link => ...)` with `link.Satellite<TSatellite>(...)`, and they use the same `Payload(...)` and optional `DrivingKey(...)` selector rules as hub-parent satellites. Effectivity is modeled as caller-owned link-parent satellite state on a link; DVault does not add an effectivity-specific builder, metadata kind, or entity family.

### Save explicitly

Persistence remains an explicit service boundary. `DataVaultSaveRequest` carries the load timestamp and record source, and callers choose when to write vault rows through `IDataVaultSaveService`. The default DVault path does not intercept `SaveChanges` or hide persistence behind ordinary EF entity tracking.

DVault also provides an explicit opt-in `SaveChanges` metadata interceptor for applications that already add generated DVault rows through EF tracking. The interceptor only fills missing `LoadTimestamp` and `RecordSource` values on added hub, link, or satellite rows annotated by DVault metadata. It does not replace `IDataVaultSaveService`, compute hash keys, compute hash diffs, create rows, or change manually supplied metadata values.

```csharp
services.AddDbContext<SalesVaultContext>(options => {
  options.UseSqlite(connectionString);
  options.UseDataVaultMetadata(salesVaultMetadata);
  options.UseDataVaultSaveChangesMetadataInterceptor(interceptor => interceptor
      .UseLoadTimestamp(() => DateTimeOffset.UtcNow)
      .UseRecordSource("crm-import"));
});
```

The raw request example below uses explicit metadata objects. Applications that want to avoid repeating those metadata declarations in loaders can opt the `DbContext` into a `DataVaultMetadataRegistry` with `UseDataVaultMetadata()` and then use registry-backed requests or the typed mapper helpers such as `SaveHubAsync(...)`, `SaveLinkAsync(...)`, and `SaveOrdinaryHubSatelliteAsync(...)`.

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

For loaders that already have multiple source batches prepared, `DataVaultBulkSaveRequest` processes ordered save requests through the same explicit service. Each contained request keeps its caller-supplied load timestamp, record source, hub operations, link operations, and satellite operations. The provider-neutral writer keeps satellite HashDiff state in memory across the ordered batch, and provider packages can select native bulk strategies when diagnostics gates accept the current clean context. Registry-backed callers can use `DataVaultRegistryBulkSaveRequest` to resolve logical metadata names once and delegate to the same bulk pipeline.

### Read typed latest and as-of satellite projections

`IDataVaultReadService` provides provider-neutral current and as-of satellite reads over the latest-satellite baseline. The common path maps selected rows through a caller-owned projector delegate so application code can return typed read models without binding DTOs through reflection. `ReadCurrentSatelliteAsync(...)` selects the latest visible row, and `ReadAsOfSatelliteAsync(...)` selects the latest row visible at the supplied cutoff. These convenience names delegate to the existing `DataVaultLatestSatelliteReadRequest` pipeline; `ReadLatestSatelliteAsync(...)` and `DataVaultLatestSatelliteReadRequest` remain supported for compatibility.

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
Func<DataVaultSatelliteProjectionRow, CustomerProfileRead> projectProfile = row =>
    new CustomerProfileRead(
        row.RequiredString("ParentHashKey"),
        row.RequiredString("CustomerName"),
        row.RequiredString("CustomerStatus"),
        row.RequiredDateTimeOffset("LoadTimestamp"));

var currentProfiles = await readService.ReadCurrentSatelliteAsync(
    context,
    profile,
    [customerHashKey],
    projectProfile,
    cancellationToken);

var asOfProfiles = await readService.ReadLatestSatelliteAsync(
    context,
    new DataVaultLatestSatelliteReadRequest(profile, [customerHashKey], asOfTimestamp),
    projectProfile,
    cancellationToken);

var convenienceAsOfProfiles = await readService.ReadAsOfSatelliteAsync(
    context,
    profile,
    [customerHashKey],
    asOfTimestamp,
    projectProfile,
    cancellationToken);

var registryCurrentProfiles = await readService.ReadCurrentSatelliteAsync(
    context,
    DataVaultMetadataReference.Hub("Customer"),
    "Profile",
    [customerHashKey],
    projectProfile,
    cancellationToken);

var registryAsOfProfiles = await readService.ReadAsOfSatelliteAsync(
    context,
    DataVaultMetadataReference.Hub("Customer"),
    "Profile",
    [customerHashKey],
    asOfTimestamp,
    projectProfile,
    cancellationToken);

public sealed record CustomerProfileRead(
    string ParentHashKey,
    string CustomerName,
    string CustomerStatus,
    DateTimeOffset LoadTimestamp);
```

The lower-level `ReadCurrentSatelliteRowsAsync(...)`, `ReadAsOfSatelliteRowsAsync(...)`, and `ReadLatestSatelliteRowsAsync(...)` APIs remain available as advanced escape hatches. They return `DataVaultSatelliteReadRecord` values containing the parent hash key, driving-key values, hash diff, load timestamp, record source, and payload values for callers that need row-level dictionaries or custom projections.

### Read PIT and bridge projections

PIT-backed as-of reads and bridge reads are read-service helpers over materialized read-model tables with provider-neutral fallback behavior. PIT-backed reads consume explicitly maintained PIT rows populated through the caller-invoked `IDataVaultPitMaintenanceService`; bridge reads consume explicitly maintained bridge rows populated through the caller-invoked `IDataVaultBridgeMaintenanceService`. `AddDVaultSqlite()` selects SQLite optimized read dispatch for supported PIT and bridge shapes; unsupported providers or declined shapes keep the provider-neutral pipelines. The read surface does not add automatic PIT or bridge maintenance, scheduling, implicit read-time maintenance, registry-backed PIT maintenance, full graph traversal APIs, or non-SQLite PIT/bridge optimization.

PIT-backed reads target one `DataVaultPitMetadata` declaration, explicit parent hash keys, and an `asOf` timestamp. `ReadPitRowsAsync(...)` returns raw `DataVaultPitReadRecord` rows; `ReadPitAsync(...)` maps selected rows through a caller-owned projection delegate with exact-name access to `ParentHashKey`, `LoadTimestamp`, and declared satellite segments.

```csharp
var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
var snapshots = await readService.ReadPitAsync(
    context,
    new DataVaultPitAsOfReadRequest(pit, [customerHashKey], asOfTimestamp),
    row => {
      var profile = row.RequiredSatellite("Profile");
      var status = row.OptionalSatellite("Status");

      return new CustomerSnapshotRead(
          row.RequiredString("ParentHashKey"),
          row.RequiredDateTimeOffset("LoadTimestamp"),
          profile.RequiredString("CustomerName"),
          profile.RequiredString("CustomerStatus"),
          status?.NullableString("StatusCode"));
    },
    cancellationToken);
```

PIT maintenance targets one `DataVaultPitMetadata` declaration at a time. `RebuildAsync(...)` recomputes the complete generated PIT table from persisted hub-parent satellite history. `MaintainParentsAsync(...)` recomputes complete PIT history for explicit parent hash keys, replacing the targeted parents' PIT rows so late-arriving satellite history can correct earlier snapshots. Empty parent-hash-key requests are no-ops. PIT maintenance is explicit caller work after ingestion; reads, saves, EF `SaveChanges`, provider startup, and background scheduling do not refresh PIT rows implicitly.

```csharp
var pitMaintenanceService = serviceProvider.GetRequiredService<IDataVaultPitMaintenanceService>();

await pitMaintenanceService.RebuildAsync(
    context,
    new DataVaultPitRebuildRequest(pit),
    cancellationToken);

await pitMaintenanceService.MaintainParentsAsync(
    context,
    new DataVaultPitParentMaintenanceRequest(pit, [customerHashKey]),
    cancellationToken);
```

Bridge maintenance targets one `DataVaultBridgeMetadata` declaration at a time. `RebuildBridgeAsync(...)` recomputes the generated bridge table from persisted source-link rows. `MaintainBridgeAsync(...)` inserts missing rows from the current source-link state without deleting obsolete rows; for hierarchy bridges it lowers an existing `TraversalDepth` when a newly materialized shorter path is available and leaves equal or longer alternate paths unchanged. Many-to-many bridges maintain one row per distinct endpoint pair. Hierarchy bridges maintain one row per distinct ancestor/descendant pair, store the minimum positive hop count, treat direct edges as depth `1`, and do not add implicit self rows. Registry-backed callers can use `DataVaultRegistryBridgeMaintenanceRequest` to resolve the bridge by logical name from `UseDataVaultMetadata()`.

Bridge reads target one `DataVaultBridgeMetadata` declaration and filter by endpoint hash keys. Many-to-many bridges support `DataVaultBridgeTraversalEndpoint.From` and `DataVaultBridgeTraversalEndpoint.To`. Hierarchy bridges support `DataVaultBridgeTraversalEndpoint.Ancestor` and `DataVaultBridgeTraversalEndpoint.Descendant`, require a bounded `maximumDepth`, and expose `TraversalDepth` on hierarchy rows.

```csharp
var bridgeMaintenanceService = serviceProvider.GetRequiredService<IDataVaultBridgeMaintenanceService>();
var customerOrder = new DataVaultLinkMetadata(
    "CustomerOrder",
    [customer.ToReference(), order.ToReference()]);
var customerOrderBridge = DataVaultBridgeMetadata.ManyToMany(
    "CustomerOrder",
    customer.ToReference(),
    customerOrder.ToReference(),
    order.ToReference());

await bridgeMaintenanceService.MaintainBridgeAsync(
    context,
    new DataVaultBridgeMaintenanceRequest(customerOrderBridge),
    cancellationToken);

var orderHashKeys = await readService.ReadBridgeAsync(
    context,
    new DataVaultBridgeReadRequest(
        customerOrderBridge,
        DataVaultBridgeTraversalEndpoint.From,
        [customerHashKey]),
    row => row.RequiredString("OrderHashKey"),
    cancellationToken);

var regionHierarchy = DataVaultBridgeMetadata.Hierarchy(
    "SalesRegionHierarchy",
    DataVaultMetadataReference.Hub("SalesRegion"),
    DataVaultMetadataReference.Link("SalesRegionParentChild"),
    DataVaultMetadataReference.Hub("SalesRegion"),
    ancestorParticipantOrdinal: 0,
    descendantParticipantOrdinal: 1);

var descendantRegions = await readService.ReadBridgeAsync(
    context,
    new DataVaultBridgeReadRequest(
        regionHierarchy,
        DataVaultBridgeTraversalEndpoint.Ancestor,
        ["region-a"],
        maximumDepth: 2),
    row => new SalesRegionPathRead(
        row.RequiredString("DescendantSalesRegionHashKey"),
        row.RequiredInt32("TraversalDepth")),
    cancellationToken);
```

`ReadBridgeRowsAsync(...)` returns `DataVaultBridgeReadRecord` values with endpoint hash keys in generated column order. Each endpoint value carries the public endpoint role, endpoint name, generated column name, and hash key. Typed bridge projectors use exact generated column names such as `OrderHashKey`, `AncestorSalesRegionHashKey`, `DescendantSalesRegionHashKey`, and `TraversalDepth`.

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

### Model-first governed artifacts

Use model-first governance when a source-controlled `dvault.model.v1` JSON artifact should be the reviewed authority for a Data Vault model. This path is separate from app-local Code-First declarations and from registry-backed metadata-first setup: JSON artifacts are imported with `DataVaultModelArtifactImporter.ImportJson`, projected through `UseDataVaultMetadata(DataVaultModelImportResult)`, exported from fluent Code-First declaration callbacks or already-materialized metadata with `DataVaultModelArtifactExporter.ExportJson`, and compared against generated EF metadata with `DataVaultModelDriftReporter.Compare`.

```csharp
using DCoding.Data.DVault;

var json = await File.ReadAllTextAsync("models/sales-vault.json", cancellationToken);
var importResult = DataVaultModelArtifactImporter.ImportJson(json, "models/sales-vault.json");

if (!importResult.IsValid) {
  throw new InvalidOperationException(
      string.Join(Environment.NewLine, importResult.Diagnostics.Select(diagnostic => diagnostic.Message)));
}

services.AddDVault(options => options.UseMetadataModel(importResult));
services.AddDbContext<SalesVaultContext>(options => {
  options.UseSqlite(connectionString);
  options.UseDataVaultMetadata(importResult);
});

if (importResult.MetadataRegistry is not null) {
  var canonicalJson = DataVaultModelArtifactExporter.ExportJson(importResult.MetadataRegistry);
}

var codeFirstJson = DataVaultModelArtifactExporter.ExportJson(vault => {
  vault.Hub<Customer>(hub => hub.BusinessKey(customer => customer.CustomerId));
});

using var context = new SalesVaultContext(options);
var driftReport = DataVaultModelDriftReporter.Compare(importResult, context);
```

Treat the JSON artifact and any drift report as review evidence. `dvault.model.v1` artifacts use exact `schemaVersion` handling, canonical JSON declaration ordering, the `default` `naming.policy`, strict unknown-field rejection, `loadTimestampStorage` tokens `provider-default`, `iso-8601-utc-text`, and `utc-ticks`, and stable `hubs`, `links`, `satellites`, `pits`, and `bridges` declaration categories. See [Model-First Governance Workflow](docs/model-first-governance.md) for the full workflow, versioning rules, YAML boundary, and current limitations.

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

`DCoding.Data.DVault.Sqlite` registers the optimized SQLite set-based save strategy plus optimized latest-satellite, PIT, and bridge read dispatch. `DCoding.Data.DVault.Postgres` registers an optimized Npgsql/PostgreSQL strategy for clean contexts that use set-based `INSERT ... ON CONFLICT DO NOTHING` hub and link writes plus latest-state satellite checks. `DCoding.Data.DVault.SqlServer` registers an optimized SQL Server strategy for clean contexts with set-based unique-row inserts and latest-state satellite checks. `DCoding.Data.DVault.Oracle` registers an Oracle-gated insert strategy for clean `Oracle.EntityFrameworkCore` contexts that meet the native bulk gate, including ordinary hub, link, and satellite batches. `DCoding.Data.DVault.MySql` supports `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`, and registers an optimized MySQL strategy for clean supported MySQL contexts.

Provider-native bulk dispatch is diagnostics-gated. Dirty tracked contexts, multi-active satellite batches, and provider-name mismatches decline to the provider-neutral writer. SQL Server native dispatch also requires at least `50` total operations and at most `500` satellite operations. MySQL native dispatch requires at least `50` total operations and accepts both Pomelo and official MySQL EF Core provider names. Oracle native dispatch requires at least `50` total operations.

Provider-specific save-strategy registration and provider capability-profile selection are separate surfaces. The core package includes built-in capability profiles for the known SQLite, PostgreSQL, SQL Server, Oracle, Pomelo MySQL, and official MySQL EF provider names. Direct `ApplyDataVaultMetadata(...)` calls can pass an explicit `DataVaultProviderCapabilityProfile` when an application wants deterministic provider-specific schema projection at model-building time. Registry-backed `UseDataVaultMetadata(...)` remains the easiest path when one metadata source should drive schema, save, and read usage.

### Migration from v0.5

v0.5 metadata-first `DataVaultMetadataModel` usage remains valid in v0.6.0. Existing applications can keep constructing metadata models, registering them with `AddDVault(options => options.UseMetadataModel(...))`, and opting DbContexts into `UseDataVaultMetadata()`.

New application code can prefer Code-First declarations when the Data Vault model fits the implemented hub-parent satellite, link-parent satellite, multi-active driving-key, explicit or derived link, and repeated same-hub role-bearing link surface. Keep metadata-first declarations for shared metadata registries, example-local quickstarts, bridge/PIT metadata baselines, dependent child key modeling, custom naming requirements, or any scenario that needs one public metadata object reused by schema, save, and read paths.

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

### Live schema drift checks

DVault has a bounded live-schema drift path for comparing expected Data Vault metadata with the physical schema in a reachable database. The live snapshot surface is intentionally limited to DVault-owned tables, ordered columns, named primary-key constraints, and secondary indexes. It does not compare foreign keys, arbitrary non-DVault database objects, destructive migration plans, or repair operations.

The built-in live-schema reader covers SQLite, PostgreSQL, SQL Server, Oracle, and MySQL. It is available through `DataVaultLiveSchemaReader.ReadAsync(context)` and returns a classified `DataVaultLiveSchemaReadResult` rather than silently skipping unsupported or unavailable environments. Compare the result with expected metadata using `DataVaultLiveSchemaDriftReporter.Compare`:

```csharp
using DCoding.Data.DVault;

using var context = new SalesVaultContext(options);
var liveSchema = await DataVaultLiveSchemaReader.ReadAsync(context);
var report = DataVaultLiveSchemaDriftReporter.Compare(metadataModel, liveSchema);

if (report.HasBlockingDifferences) {
  throw new InvalidOperationException(report.ToDisplayString());
}
```

Built-in provider dispatch recognizes `Microsoft.EntityFrameworkCore.Sqlite`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, `MySql.EntityFrameworkCore`, and `Pomelo.EntityFrameworkCore.MySql`. Both MySQL EF Core provider names map to the MySQL reader.

Providers without a built-in live-schema reader return `DataVaultLiveSchemaReadStatus.UnsupportedProvider`. A supported provider whose database cannot be reached returns `DataVaultLiveSchemaReadStatus.Unavailable`. Both outcomes become stable blocking drift differences when passed to `DataVaultLiveSchemaDriftReporter.Compare`.

SQLite remains the default local live-schema proof because it does not require external infrastructure. PostgreSQL, SQL Server, Oracle, and MySQL live-schema checks require consumer-managed reachable databases, connection strings, credentials, lifecycle cleanup, and CI isolation. Keep those external provider checks opt-in behind the documented connection-string environment variables: `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_MYSQL_CONNECTION_STRING`. Default local test execution does not require those external databases.

## v0.15.0 Release Notes

The v0.15.0 release records explicit bridge maintenance, explicit PIT maintenance, current/as-of satellite read convenience overloads, and SQLite optimized PIT/bridge read dispatch while keeping read-model population caller-invoked. See `docs/releases/v0.15.0.md` for the release-note record, package scope, compatibility notes, validation evidence, and package verification posture.

Notable user-facing changes:

- `IDataVaultBridgeMaintenanceService` is registered by `AddDVault()` beside the explicit save and read services.
- `IDataVaultPitMaintenanceService` is registered by `AddDVault()` beside the explicit save, read, and bridge maintenance services.
- `DataVaultPitRebuildRequest` rebuilds one generated PIT table from persisted hub-parent satellite history.
- `DataVaultPitParentMaintenanceRequest` recomputes complete PIT history for explicit parent hash keys and supports late-arriving history correction for those parents.
- `RebuildBridgeAsync(...)` recomputes one bridge table from persisted source-link rows and converges with repeated execution over the same source state.
- `MaintainBridgeAsync(...)` incrementally inserts newly reachable bridge rows without deleting obsolete rows; hierarchy maintenance also lowers an existing `TraversalDepth` when a newly persisted path is shorter.
- Many-to-many bridge maintenance stores one row per distinct endpoint pair required by the bridge metadata.
- Hierarchy bridge maintenance stores one row per distinct ancestor/descendant pair, uses the minimum positive hop count as `TraversalDepth`, treats direct edges as depth `1`, and does not add implicit self rows.
- Registry-backed callers can resolve bridge metadata by logical name through `DataVaultRegistryBridgeMaintenanceRequest` when `UseDataVaultMetadata()` is the authoritative model source.
- `ReadCurrentSatelliteAsync(...)` and `ReadAsOfSatelliteAsync(...)` remain additive convenience wrappers over the existing `DataVaultLatestSatelliteReadRequest` baseline for explicit and registry-backed callers.
- `AddDVaultSqlite()` is the repository-proven optimized PIT/bridge read path; unsupported providers or unsupported request shapes fall back to the provider-neutral read pipelines without implicit maintenance side effects.
- Provider-native bulk ingestion, Code-First same-hub roles, link-parent satellites, model-first artifact governance, and analyzer guidance from earlier releases remain part of the current public baseline.

## Current v0.15.0 Limitations

Lifecycle guardrails remain explicit library APIs hosted by the consumer application. DVault does not ship a standalone CLI, does not ship a first-party `dotnet ef` command shim, does not intercept EF migration commands, does not automatically execute migrations, and does not apply schema repairs. Startup-project and target-project splits for design-time discovery remain outside the v0.15.0 boundary. Live-schema reading is built in for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, but non-SQLite checks still require consumer-managed databases and should remain opt-in operational evidence rather than default local validation.

Provider-native bulk dispatch is an optimization, not a separate persistence contract. Dirty tracked contexts, multi-active satellite batches, provider-name mismatches, below-threshold SQL Server, MySQL, or Oracle batches, and SQL Server batches with more than `500` satellite operations fall back to the provider-neutral writer. DVault does not provision Docker containers, databases, users, schemas, credentials, or checked-in benchmark result snapshots for optional external-provider proof.

Model-first APIs continue to operate on JSON artifacts, fluent Code-First declaration callbacks, and already-materialized metadata through `DataVaultModelArtifactImporter.ImportJson`, `DataVaultModelArtifactExporter.ExportJson`, `UseDataVaultMetadata(DataVaultModelImportResult)`, and `DataVaultModelDriftReporter.Compare`. PIT and bridge maintenance are explicit caller-invoked service boundaries through `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`; they are not automatic, scheduler-driven, trigger-driven, read-time maintenance, or provider-specific maintenance. PIT maintenance does not add registry-backed PIT resolution, link-parent PITs, multi-active PITs, or provider-specific PIT maintenance strategies. Bridge maintenance is not delete-aware and does not add broader graph traversal APIs, effectivity windows, path payload columns, or closure-state columns. Use full bridge rebuild when hierarchy deletions or topology shrinkage would require row removal or increased `TraversalDepth`. SQLite remains the only repository-proven optimized PIT/bridge read provider path; non-SQLite providers and unsupported shapes fall back to provider-neutral read pipelines. The metadata interceptor is opt-in and metadata-only: callers still own generated row creation, hash-key and hash-diff values, save ordering, and explicit service-based persistence when they use `IDataVaultSaveService`.

Dependent child key modeling is not part of the current public claim set. Repeated same-hub runtime and metadata support does not imply typed link-mapper or source-generator parity for repeated same-hub mappings; generated and manual typed link mappers continue to use the existing unique-participant mapping boundary. Effectivity remains a generic link-parent satellite pattern rather than a first-class effectivity entity family, fluent builder, metadata kind, or technical column set. The analyzer package is not a complete model validator; it covers the documented Code-First selector diagnostics, duplicate-member diagnostics, bounded code fixes, and compile-time mapping declaration diagnostics only.

## Layout

- `DVault.slnx`: Canonical root solution file for build and test automation.
- `src/DCoding.Data/`: Non-packable build anchor for the `DCoding.Data` source-root namespace family.
- `src/DCoding.Data.DVault/`: Main library project. The NuGet package id and root namespace are `DCoding.Data.DVault`.
- `src/DCoding.Data.DVault.*`: Provider extension packages for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- `src/DCoding.Data.DVault.Analyzers/`: Roslyn analyzer and source-generator package for DVault Code-First diagnostics, bounded code fixes, and compile-time mapping declarations.
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

`bash tools/verify-packages.sh` inspects the artifacts created under `artifacts/packages/` by the solution-level pack command. It expects exactly the seven DVault packages plus six matching symbol packages for the runtime/provider packages, checks README, XML documentation, analyzer assets, declared NuGet metadata, and confirms each provider package depends on the packed `DCoding.Data.DVault` version. The verifier intentionally fails when stale, unexpected, or non-packable package artifacts remain in `artifacts/packages/`.

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

The benchmark executable compares conventional EF and DVault flows for the shared customer profile history contract, a larger customer profile bulk-history contract, the reduced order-product fulfillment history contract, and the provider-native bulk-ingestion contract. It uses SQLite temporary files by default and does not require Postgres, SQL Server, Oracle, MySQL, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, or `DVAULT_TEST_MYSQL_CONNECTION_STRING`. Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

Pass `--output <directory>` to emit documentation-ready benchmark artifacts named `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`. These artifacts keep scenario, provider, baseline, strategy family, dataset size, change ratio, execution status, skip reason, iteration count, mean/min/max milliseconds, persisted outcome, benchmark options, optional provider status, and machine/runtime context together. Optional PostgreSQL, SQL Server, MySQL, and Oracle provider-native bulk rows stay visible as skipped rows with `executionStatus` and `skipReason` when the provider is not configured or unavailable.

## Optional Local Postgres Integration Tests

Postgres integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require Postgres, Docker, or checked-in machine-specific configuration.

To run the Postgres-backed integration tests, provide a developer-managed PostgreSQL database connection string in `DVAULT_TEST_POSTGRES_CONNECTION_STRING`:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret' dotnet test DVault.slnx --nologo -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured
```

To select only the live Postgres integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_POSTGRES_CONNECTION_STRING='Host=localhost;Port=5432;Database=dvault_tests;Username=dvault;Password=local-secret' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Postgres" -p:DVAULT_TEST_POSTGRES_CONNECTION_STRING=Configured
```

DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop temporary schemas. The live Postgres lane includes schema drift checks and an ordered bulk hub, link, and satellite save through the provider strategy. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## Optional Local SQL Server Integration Tests

SQL Server integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require SQL Server, Docker, or checked-in machine-specific configuration.

To run the SQL Server integration lane, provide a developer-managed SQL Server database connection string in `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` and run the representative repo-root command:

```sh
DVAULT_TEST_SQLSERVER_CONNECTION_STRING='Server=localhost;Database=dvault_tests;User Id=dvault;Password=local-secret;TrustServerCertificate=True' dotnet test DVault.slnx --filter FullyQualifiedName~SqlServer -p:DVAULT_TEST_SQLSERVER_CONNECTION_STRING=Configured
```

The configured SQL Server principal must be able to create and drop temporary `dvault_test_*` schemas and tables in the target database. The tests create isolated schemas, validate representative single saves, and exercise an eligible ordered bulk hub, link, and satellite batch through the optimized SQL Server provider strategy before dropping the generated schema. Missing `DVAULT_TEST_SQLSERVER_CONNECTION_STRING` produces a deterministic skip message instead of loading the conditional SQL Server provider package.

DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop temporary schemas. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## Optional Local Oracle Integration Tests

Oracle integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require Oracle, Docker, or checked-in machine-specific configuration.

To run the Oracle-backed integration tests, provide a developer-managed Oracle database connection string in `DVAULT_TEST_ORACLE_CONNECTION_STRING`:

```sh
DVAULT_TEST_ORACLE_CONNECTION_STRING='User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1' dotnet test DVault.slnx --nologo -p:DVAULT_TEST_ORACLE_CONNECTION_STRING=Configured
```

To select only the live Oracle integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_ORACLE_CONNECTION_STRING='User Id=dvault;Password=local-secret;Data Source=localhost:1521/FREEPDB1' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=Oracle" -p:DVAULT_TEST_ORACLE_CONNECTION_STRING=Configured
```

DVault does not provision Docker containers, Oracle databases, or Oracle users for these tests. The configured database and user must already exist, and the configured user must be allowed to create and drop temporary tables. The live Oracle lane includes the existing smoke coverage plus an eligible ordered bulk hub, link, and satellite batch through the provider strategy. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## Optional Local MySQL Integration Tests

MySQL integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require MySQL, Docker, or checked-in machine-specific configuration.

To run the MySQL-backed integration tests, provide a developer-managed MySQL database connection string in `DVAULT_TEST_MYSQL_CONNECTION_STRING`:

```sh
DVAULT_TEST_MYSQL_CONNECTION_STRING='Server=localhost;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret;AllowPublicKeyRetrieval=True;SslMode=Disabled' dotnet test DVault.slnx --nologo -p:DVAULT_TEST_MYSQL_CONNECTION_STRING=Configured
```

To select only the live MySQL integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_MYSQL_CONNECTION_STRING='Server=localhost;Port=3306;Database=dvault_tests;User=dvault;Password=local-secret;AllowPublicKeyRetrieval=True;SslMode=Disabled' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=MySQL" -p:DVAULT_TEST_MYSQL_CONNECTION_STRING=Configured
```

The integration project conditionally restores `MySql.EntityFrameworkCore` only when the MySQL opt-in property is non-empty. When running the live MySQL path, keep the environment variable set for test execution and pass the non-secret MSBuild marker property shown above so the conditional provider package is available during restore and build. The live MySQL lane includes the existing smoke coverage plus an eligible ordered bulk hub, link, and satellite batch through the provider strategy. DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop the temporary smoke and bulk-test tables. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## License

DVault uses the Apache License 2.0. See `LICENSE`.
