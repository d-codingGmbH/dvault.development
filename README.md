# DVault

DVault is the repository for the `DCoding.Data.DVault` .NET library.

## Installation

Install the provider-neutral DVault package from NuGet and add the provider package that matches the database used by the application. The coordinated DVault package family keeps the same package ids across both supported package-version lines. Use exactly one line for a consumer project: `8.34.0` for `net8.0` and EF Core 8, or `10.34.0` for `net10.0` and EF Core 10. Do not mix package versions from both lines in one project, and do not use a consumer-facing `0.34.0` package version. This documentation baseline does not by itself confirm package publication.

For `net8.0` projects on EF Core 8, use the `8.34.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 8.34.0
dotnet add package DCoding.Data.DVault.Db2 --version 8.34.0
dotnet add package DCoding.Data.DVault.Sqlite --version 8.34.0
dotnet add package DCoding.Data.DVault.Postgres --version 8.34.0
dotnet add package DCoding.Data.DVault.MySql --version 8.34.0
dotnet add package DCoding.Data.DVault.Oracle --version 8.34.0
dotnet add package DCoding.Data.DVault.SqlServer --version 8.34.0
```

For `net10.0` projects on EF Core 10, use the `10.34.0` package line:

```sh
dotnet add package DCoding.Data.DVault --version 10.34.0
dotnet add package DCoding.Data.DVault.Db2 --version 10.34.0
dotnet add package DCoding.Data.DVault.Sqlite --version 10.34.0
dotnet add package DCoding.Data.DVault.Postgres --version 10.34.0
dotnet add package DCoding.Data.DVault.MySql --version 10.34.0
dotnet add package DCoding.Data.DVault.Oracle --version 10.34.0
dotnet add package DCoding.Data.DVault.SqlServer --version 10.34.0
```

Add the analyzer package only to projects that own DVault declarations or generated read helpers, and keep it local with `PrivateAssets="all"`. Use the analyzer version that matches the selected package line:

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="8.34.0" PrivateAssets="all" />
</ItemGroup>
```

```xml
<ItemGroup>
  <PackageReference Include="DCoding.Data.DVault.Analyzers" Version="10.34.0" PrivateAssets="all" />
</ItemGroup>
```

Applications still need their normal Entity Framework Core database provider package, such as `IBM.EntityFrameworkCore` for DB2, `Microsoft.EntityFrameworkCore.Sqlite` for SQLite, `Npgsql.EntityFrameworkCore.PostgreSQL` for PostgreSQL, `Microsoft.EntityFrameworkCore.SqlServer` for SQL Server, `Oracle.EntityFrameworkCore` for Oracle, or `Pomelo.EntityFrameworkCore.MySql` / `MySql.EntityFrameworkCore` for MySQL.

`DCoding.Data.DVault.Analyzers` is optional developer tooling. Prefer `PrivateAssets="all"` for that package so analyzer and source-generator assets stay local to the project that declares DVault Code-First metadata, compile-time mapping declarations, or generated typed satellite, PIT, and bridge read helpers. The current analyzer surface includes `DMV1910` and `DMV1911` for high-confidence EF Core misuse patterns around generated shared-type tables, `DMV1912` through `DMV1914` for source-visible DVault EF lifecycle misuse around caller-owned model-cache discriminators, compiled-model selection, and `DbContext` pooling, plus opt-in support-bundle-driven typed read-model generation (`DVaultGenerateTypedReadModels=true`) with `DMV1960` through `DMV1969` metadata and shape outcomes. See `src/DCoding.Data.DVault.Analyzers/README.md` for the package-local diagnostic, code-fix, source-generator, suppression, and configuration guidance.

Runnable SQLite and PostgreSQL quickstart projects are available under `examples/`; see `examples/README.md` for exact build and run commands.

The latest persisted release-note baseline is [DVault v0.34.0 Release Notes](docs/releases/v0.34.0.md). Earlier release notes remain historical feature-introduction records. For detailed performance guidance, see [Performance Profiles](docs/performance-profiles.md). For a short adopter readiness pass before production use, see the [Production Adoption Checklist](docs/production-adoption-checklist.md).

## Quickstart

Use this flow in a .NET 10 project that references `DCoding.Data.DVault` and has an Entity Framework Core provider configured. DVault supports three additive declaration paths:

- Code-First declarations for app-local EF models that fit the fluent hub, hub-parent satellite, link-parent satellite, multi-active driving-key, explicit or derived hub-link, and explicitly named repeated same-hub link surface.
- Metadata-first declarations through a shared `DataVaultMetadataModel` or `DataVaultMetadataRegistry` when one public metadata object should drive schema projection, explicit saves, typed latest/as-of reads, diagnostics, support-bundle export, examples, or provider setup.
- Model-first governance for reviewed `dvault.model.v1` JSON artifacts that should be imported, projected into EF metadata, exported canonically, compared against generated metadata for drift evidence, and optionally routed through a consumer-owned support-bundle export for generated typed read helpers.

Choose one authoritative path for a model boundary and keep the others as compatible alternatives for different ownership needs. See [Model-First Governance Workflow](docs/model-first-governance.md) for the current `dvault.model.v1` JSON artifact contract and [DVault EF Design-Time Workflow](docs/architecture/dvault-dotnet-ef-design-time-workflow.md) for the current consumer-owned design-time command workflow around reviewed artifacts, EF metadata, migrations, live schema drift, and redacted support-bundle export.

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
services.AddDVaultDb2();
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

Applications that want an early runtime check for unsafe generated-row EF tracking can opt into the separate SaveChanges guard interceptor. `AddDVault()` does not enable this guard, and the guard does not replace `IDataVaultSaveService` as the default write boundary. It can run beside `UseDataVaultSaveChangesMetadataInterceptor(...)`: the metadata interceptor fills caller-owned metadata on already tracked rows, while the guard reports or blocks direct generated-table writes that should normally flow through the explicit save service.

```csharp
services.AddDbContext<SalesVaultContext>(options => {
  options.UseSqlite(connectionString);
  options.UseDataVaultMetadata(salesVaultMetadata);
  options.UseDataVaultSaveChangesMetadataInterceptor(interceptor => interceptor
      .UseLoadTimestamp(() => DateTimeOffset.UtcNow)
      .UseRecordSource("crm-import"));
  options.UseDataVaultSaveChangesGuardInterceptor(guard => guard.UseBlockingMode());
});
```

Use `UseWarningMode(...)` when an application needs a deterministic `DataVaultSaveChangesGuardReport` while it is migrating callers away from direct generated-row writes. Use `UseBlockingMode()` when those writes should fail before `SaveChanges` persists them.

The raw request example below uses explicit metadata objects. Applications that want to avoid repeating those metadata declarations in loaders can opt the `DbContext` into a `DataVaultMetadataRegistry` with `UseDataVaultMetadata()` and then use registry-backed requests or the typed mapper helpers such as `SaveHubAsync(...)`, `SaveLinkAsync(...)`, and `SaveOrdinaryHubSatelliteAsync(...)`. For async sources, the same boundary has `SaveAsync<TSource>(...)`, `SaveHubsAsync(...)`, `SaveLinksAsync(...)`, and `SaveOrdinaryHubSatellitesAsync(...)` helpers that map caller-ordered source rows into bounded explicit chunks.

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

For bounded loaders that should not materialize the complete ordered request set before saving, `DataVaultChunkedSaveRequest` and `DataVaultSaveChunk` are additive explicit-save inputs. Each chunk contains ordinary `DataVaultSaveRequest` values and preserves each request's load timestamp, record source, hub operations, link operations, satellite operations, and caller order. The service processes chunks in caller order, observes cancellation before continuing to later chunks, and participates in the caller's current transaction. Empty chunk sequences and empty chunks are no-ops.

```csharp
IEnumerable<DataVaultSaveRequest> orderedSourceRequests =
    BuildOrderedRequests(loadTimestamp, "crm-import");

var chunkedRequest = new DataVaultChunkedSaveRequest(
    orderedSourceRequests
        .Chunk(10)
        .Select(requests => new DataVaultSaveChunk(requests)));

await saveService.SaveAsync(context, chunkedRequest, cancellationToken);
```

The async chunk-source overload accepts the same `DataVaultSaveChunk` payload model without requiring a materialized `DataVaultChunkedSaveRequest` first. Use it only when the producer naturally yields bounded chunks asynchronously and should be enumerated once in source order. The convenience `SaveAsync<TSource>(...)` helper maps an `IAsyncEnumerable<TSource>` to ordered `DataVaultSaveRequest` values and buffers at most the caller-supplied `chunkSize` before delegating to `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)`.

```csharp
IAsyncEnumerable<DataVaultSaveChunk> asyncChunks =
    StreamRequestChunksAsync(loadTimestamp, "crm-import", cancellationToken);

await saveService.SaveAsync(context, asyncChunks, cancellationToken);

await saveService.SaveAsync(
    context,
    StreamCustomersAsync(cancellationToken),
    customer => BuildCustomerProfileRequest(customer, loadTimestamp, "crm-import"),
    chunkSize: 10,
    cancellationToken);
```

Keep `DataVaultBulkSaveRequest` when the loader already has the full ordered request set materialized. Switch to `DataVaultChunkedSaveRequest` when the caller has already formed bounded chunks and wants materialized chunk input without changing explicit timestamps, record sources, request ordering, or caller-owned transaction behavior. Use `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` or the async source helpers when bounded chunks or source rows are already asynchronous. Provider-specific save strategies remain optimizations around the same public save contract; PostgreSQL and MySQL can stage larger eligible materialized bulk batches behind that boundary, while DVault does not claim provider-native async writes, provider-native chunk execution, background ingestion, or scheduler behavior.

The current v0.34.0 documentation baseline keeps that same write hierarchy and carries forward support-bundle-backed satellite, PIT, and bounded bridge typed read helpers. `IDataVaultSaveService` remains the public write entry point, `DataVaultBulkSaveRequest` remains the compatibility baseline for already-materialized ordered saves, `DataVaultChunkedSaveRequest` remains provider-neutral bounded chunking guidance, and `IAsyncEnumerable<DataVaultSaveChunk>` is the async source shape over those same bounded chunks. Provider-specific optimized write paths stay evidence-bound behind the same service contract: PostgreSQL staged COPY and MySQL staged bulk are the preferred optimized paths only for their documented staged-provider lanes, SQL Server keeps its current native-bulk wording, Oracle keeps the retained direct optimized path until benchmark evidence selects a staged Oracle path, and DB2 uses provider-neutral fallback. Stored procedures and provider-specific SQL artifacts are not DVault default write paths: treat them only as explicit opt-in, design-time artifact escape hatches after provider evidence, migration synchronization, consumer-owned deployment, rollback, invocation, and cleanup rules are documented.

### Govern stable hashes

`AddDVault()` registers the default `IStableHashService` and `IStableHashNormalizer` unless the application has already registered replacements. The default algorithm identifier is `sha256-v1`; it hashes UTF-8 bytes without a byte order mark and emits lowercase 64-character SHA-256 digest text. The normalizer owns canonical text rules before hashing, including explicit null, string, boolean, integer, decimal, timestamp, and GUID encodings plus ordinal field ordering for structured values.

Hub and link hash-key generation flows through the registered stable hash normalizer and service. Provider packages may optimize batching, staging, existence checks, and insert shapes, but they do not silently replace the shared `sha256-v1` compatibility contract with provider SQL hash functions. Callers that intentionally replace the hash service must expose a stable algorithm id; compatible replacements keep `sha256-v1` and the same digests, while incompatible replacements use a distinct id.

The compatibility contract and published vectors live in `docs/plans/stable-hashing-contract.md`, with repository coverage in `tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs`. Treat those as the evidence baseline when reviewing model, provider, or migration changes that depend on persisted hash values.

### Observe explicit save and read attempts

Save/read telemetry is opt-in. The default `AddDVault()` registration does not add counters or listeners. Applications can enable the built-in `System.Diagnostics.Metrics` observer with `AddDVaultTelemetry()` and can register additional `IDataVaultTelemetryObserver` implementations for bounded per-attempt summaries.

```csharp
services.AddDVault();
services.AddDVaultTelemetry();
```

The built-in meter name is `DCoding.Data.DVault`. It records save attempt, row, saved-record, request-count, operation-count, chunk-count, processed-chunk-count, retained-state high-water, duration, and fallback-cause instruments, plus read attempt, returned-row, requested-key, duration, and fallback-cause instruments. Metric tags stay low-cardinality: operation kind, read family, success/failure outcome, provider name, selected strategy type name, strategy status, and finite fallback-cause enum names.

`IDataVaultTelemetryObserver` receives one `DataVaultSaveTelemetrySummary` for each explicit single, bulk, or chunked save attempt and one `DataVaultReadTelemetrySummary` for each latest/current/as-of satellite, PIT, or bridge read attempt handled by the DVault read path. Chunked summaries include bounded chunk counts, processed chunk counts, retained-state high-water counts, finite fallback causes, unsupported-shape classifications, and transaction guidance without raw hash keys, payload values, or per-parent state entries. Failure summaries include duration and strategy classification without raw exception messages, hash keys, record sources, metadata names, table names, or full diagnostic text.

### Trace explicit save, read, and maintenance operations

Activity tracing is a sibling observability surface, not a prerequisite for metrics or telemetry summaries. `AddDVault()` remains telemetry-free by default and does not require `AddDVaultTelemetry()`. Applications opt into traces by registering an `ActivityListener`, OpenTelemetry tracing provider, or equivalent listener for the `DCoding.Data.DVault` ActivitySource. Exporters, collectors, dashboards, alerts, hosting, and sampling policy stay application-owned.

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

var services = new ServiceCollection();
services.AddDVault();
```

The tracing contract is closed and versioned in [DVault V1 Activity Tracing Contract](docs/architecture/dvault-v1-activity-tracing-contract.md). DVault Activity names, tags, events, status descriptions, and exception metadata must not include raw business keys or hash keys, payload values, record sources, SQL text, credentials, connection strings, provider messages, exception messages, stack traces, or other high-cardinality diagnostic text.

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

### Generate typed read-model helpers

The analyzer package can generate typed satellite, PIT, and bounded bridge read-model records plus extension methods over the same `IDataVaultReadService` APIs shown above. Enable the generator explicitly in the consumer project and provide exactly one authoritative `dvault.support-bundle.v1` additional file that was exported after the chosen Code-First, metadata-first, or model-first declarations were projected into EF/DVault metadata:

```xml
<PropertyGroup>
  <DVaultGenerateTypedReadModels>true</DVaultGenerateTypedReadModels>
  <DVaultTypedReadModelMetadataSourceFingerprint>metadata-source-fingerprint-from-reviewed-bundle</DVaultTypedReadModelMetadataSourceFingerprint>
</PropertyGroup>

<ItemGroup>
  <AdditionalFiles Include="dvault-support-bundle.json" />
</ItemGroup>
```

The fingerprint property is optional, but pinning it turns metadata-source drift into a build failure. The generator reads the support bundle's projected explain metadata, not raw `dvault.model.v1` JSON, Code-First source callbacks, or literal `DataVaultMetadataModel` declarations. Keep the reviewed model artifact, support-bundle export command, artifact storage, and bundle transport in the consuming repository or release workflow; DVault does not route or publish support bundles.

After any Code-First metadata, metadata registry, reviewed `dvault.model.v1` artifact, provider profile, load-timestamp storage, or representative read request changes, regenerate the authoritative support bundle from the consumer-owned design-time command and replace the single `AdditionalFiles` item used by the analyzer. If the project pins `DVaultTypedReadModelMetadataSourceFingerprint`, update it to the refreshed bundle's `metadataSourceFingerprint` or remove the property until the new bundle is reviewed. `DMV1960` means the generator could not resolve exactly one authoritative `dvault.support-bundle.v1` input, including missing, invalid, incompatible-version, ambiguous, non-authoritative, or raw `dvault.model.v1` files. `DMV1961` means the resolved support bundle is fresh enough to read but does not match the pinned fingerprint. Treat either outcome as stale input recovery work rather than as a runtime read failure.

Supported hub-parent, link-parent, and deterministic multi-active satellites emit `{SatelliteProducedName}ReadModel` plus `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` helpers under `{RootNamespace}.DVault.GeneratedReadModels` when `RootNamespace` is available. Supported PIT read shapes emit `{PitProducedName}ReadModel` plus `Read...AsOfAsync` helpers. Supported bridge read shapes emit `{BridgeProducedName}ReadModel` plus endpoint-specific helpers: `Read...FromAsync` and `Read...ToAsync` for many-to-many bridges, and `Read...AncestorAsync` and `Read...DescendantAsync` for hierarchy bridges with a required `maximumDepth`.

The generated PIT surface is bounded to hub-parent ordinary PITs, hub-parent multi-active PITs when all referenced multi-active satellites share one canonical driving-key family, and link-parent PITs with unique non-multi-active satellites on one declared link parent. PIT helpers project maintained PIT-table columns only: `ParentHashKey`, `LoadTimestamp`, canonical driving keys when present, and nullable snapshot-reference timestamps. They do not join satellite payload, rebuild PIT rows, fall back to latest satellites, or perform provider-specific query work.

The generated bridge surface is bounded to maintained many-to-many bridge traversal through `From` and `To` endpoints and maintained hierarchy bridge traversal through `Ancestor` and `Descendant` endpoints. Hierarchy helpers require an explicit `maximumDepth`; there is no generated unbounded traversal overload. Unsupported residual shapes report `DMV1963`, `DMV1964`, `DMV1967`, or `DMV1969` as appropriate for incomplete read-shape evidence, unsupported PIT or bridge facts, dynamic-query requirements, unbounded traversal, or skipped helper cases. Other supported helpers in the same support bundle can still generate.

```csharp
using ConsumerApp.DVault.GeneratedReadModels;

var customerProfiles = await readService.ReadSatCustomerProfileCurrentAsync(
    context,
    customerHashKeys,
    cancellationToken);

var customerSnapshots = await readService.ReadPitCustomerProfileAsOfAsync(
    context,
    customerHashKeys,
    asOfTimestamp,
    cancellationToken);

var customerOrders = await readService.ReadBridgeCustomerOrderFromAsync(
    context,
    customerHashKeys,
    cancellationToken);

var regionDescendants = await readService.ReadBridgeSalesRegionHierarchyDescendantAsync(
    context,
    regionHashKeys,
    maximumDepth: 3,
    cancellationToken);
```

Use generated helpers when a reviewed support bundle proves stable, shared, compile-time-friendly read shapes. Use dynamic `IDataVaultReadService` requests when requests are runtime-built, when callers select projectors dynamically, or when a shape remains outside the generated helper contract. Use consumer-owned EF compiled queries for stable direct EF shared-type-table expressions as documented in `docs/architecture/dvault-ef-compiled-compatibility.md`; DVault does not generate provider-specific SQL or compile arbitrary dynamic read requests.

### Read PIT and bridge projections

PIT-backed as-of reads and bridge reads are read-service helpers over materialized read-model tables with provider-neutral fallback behavior. PIT-backed reads consume explicitly maintained PIT rows populated through the caller-invoked `IDataVaultPitMaintenanceService`; bridge reads consume explicitly maintained bridge rows populated through the caller-invoked `IDataVaultBridgeMaintenanceService`. `AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, and `AddDVaultOracle()` select diagnostics-gated optimized read dispatch candidates for supported PIT and bridge shapes. `AddDVaultDb2()` registers DB2 capability and behavior, while DB2 PIT and bridge reads stay on provider-neutral fallback in this baseline. SQLite also remains the only optimized latest-satellite read provider path. Unsupported providers, non-SQLite latest-satellite requests, declined shapes, incomplete generated read-model projection evidence, or stale PIT/bridge maintenance evidence keep the provider-neutral pipelines. The read surface does not add automatic PIT or bridge maintenance, scheduling, implicit read-time maintenance, or full graph traversal APIs.

The v0.28.0 provider read optimization baseline remains documented in [DVault v0.28.0 Release Notes](docs/releases/v0.28.0.md). The PIT/bridge feature-introduction baseline remains [DVault v0.21.0 Release Notes](docs/releases/v0.21.0.md), with the detailed current boundary centralized in [DVault V1 PIT And Bridge Boundary](docs/architecture/dvault-v1-pit-bridge-boundary.md).

PIT-backed reads target one `DataVaultPitMetadata` declaration, explicit parent hash keys, and an `asOf` timestamp. The runtime metadata path supports hub-parent PITs, including the bounded multi-active hub-parent baseline, and bounded link-parent PITs when every referenced satellite is unique, non-multi-active, and attached to the same declared link parent; for link-parent PITs, `ParentHashKey` carries the link hash key. For ordinary PITs, one selected PIT row is returned per requested parent. For the bounded multi-active hub-parent baseline, all referenced multi-active satellites must share the same canonical driving-key names and order; reads keep the parent-hash-key request surface and return one visible row per parent and driving-key tuple. `ReadPitRowsAsync(...)` returns raw `DataVaultPitReadRecord` rows with the PIT driving-key values when present; `ReadPitAsync(...)` maps selected rows through a caller-owned projection delegate with exact-name access to `ParentHashKey`, the canonical driving-key names when present, `LoadTimestamp`, and declared satellite segments. The public `dvault.model.v1` PIT artifact shape remains hub-parent-only.

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

PIT maintenance targets one `DataVaultPitMetadata` declaration at a time. `RebuildAsync(...)` recomputes the complete generated PIT table from persisted hub- or link-parent satellite history. `MaintainParentsAsync(...)` recomputes complete PIT history for explicit parent hash keys, replacing the targeted parents' PIT rows so late-arriving satellite history can correct earlier snapshots. Empty parent-hash-key requests are no-ops. For supported multi-active hub-parent PITs, maintenance computes the existing distinct-timestamp and carry-forward rule independently per `(parentHashKey, drivingKeyTuple)`; a tuple starts producing PIT rows only after at least one referenced multi-active satellite row makes that tuple visible, while ordinary satellites remain parent-wide snapshots for the same parent. Link-parent PITs support ordinary non-multi-active satellites only. Registry-backed callers can use `DataVaultRegistryPitRebuildRequest` and `DataVaultRegistryPitParentMaintenanceRequest` to resolve the PIT by exact logical name or exact `DataVaultMetadataClrMapping.Pit(...)` CLR mapping from `UseDataVaultMetadata()`. PIT maintenance is explicit caller work after ingestion; reads, saves, EF `SaveChanges`, provider startup, and background scheduling do not refresh PIT rows implicitly.

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

await pitMaintenanceService.MaintainParentsAsync(
    context,
    new DataVaultRegistryPitParentMaintenanceRequest("CustomerProfileStatus", [customerHashKey]),
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

### Isolate EF model cache entries

Entity Framework Core caches realized models for a `DbContext` CLR type. DVault replaces the EF model-cache key for contexts configured with `UseDataVaultMetadata(...)` so the key carries the DVault metadata source kind and the deterministic metadata fingerprint. This built-in isolation covers the app-default registry selected by `UseDataVaultMetadata()`, explicit `DataVaultMetadataModel` and `DataVaultMetadataRegistry` values, and `UseDataVaultMetadata(DataVaultModelImportResult)` because a successful import resolves to a registry before projection. Distinct authoritative registries or reviewed artifacts for the same context type therefore receive distinct EF models, and the realized model records `MetadataSourceKind` and `MetadataSourceFingerprint` annotations for the active source.

That guarantee is intentionally limited to DVault-owned registry-backed metadata selection. DVault does not auto-discover arbitrary caller state used by `OnModelCreating`, constructor fields, ambient tenant values, naming overrides, schema selection, load-timestamp storage profiles, or provider-specific identifier tweaks. When those values can change the EF model shape, the application must replace `IModelCacheKeyFactory` and include every caller-owned discriminator that affects the model.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var options = new DbContextOptionsBuilder<SalesVaultContext>()
    .UseSqlite(connectionString)
    .ReplaceService<IModelCacheKeyFactory, SalesVaultModelCacheKeyFactory>()
    .Options;

using var context = new SalesVaultContext(
    options,
    tenantSchema: "tenant_blue",
    tablePrefix: "Blue_",
    loadTimestampProfile: "utc-ticks");

public sealed class SalesVaultContext(
    DbContextOptions<SalesVaultContext> options,
    string tenantSchema,
    string tablePrefix,
    string loadTimestampProfile) : DbContext(options) {
  public string TenantSchema { get; } = tenantSchema;
  public string TablePrefix { get; } = tablePrefix;
  public string LoadTimestampProfile { get; } = loadTimestampProfile;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.HasDefaultSchema(TenantSchema);
    modelBuilder.ApplyDataVaultMetadata(CreateSalesVaultMetadata());

    modelBuilder.SharedTypeEntity<Dictionary<string, object>>("HubCustomer", entity => {
      entity.ToTable(TablePrefix + "HubCustomer", TenantSchema);
    });
  }
}

public sealed class SalesVaultModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context, bool designTime) {
    return context is SalesVaultContext salesVaultContext
        ? (
            context.GetType(),
            salesVaultContext.TenantSchema,
            salesVaultContext.TablePrefix,
            salesVaultContext.LoadTimestampProfile,
            designTime)
        : (object)(context.GetType(), designTime);
  }
}
```

Direct `ApplyDataVaultMetadata(...)` usage is safe with EF's default cache key only when the model shape is stable for the context type and design-time flag. Once callers add tenant-, schema-, naming-, provider-, or profile-dependent branches around that projection, those inputs belong in the consumer-owned cache key. `DCoding.Data.DVault.Analyzers` reports `DMV1912` only when the caller-owned model-shape variation and the missing cache-key discriminator are directly visible in source. Registry-backed `UseDataVaultMetadata()`, `UseDataVaultMetadata(DataVaultMetadataModel)`, `UseDataVaultMetadata(DataVaultMetadataRegistry)`, and `UseDataVaultMetadata(DataVaultModelImportResult)` remain non-diagnostic because DVault contributes the metadata-source kind and fingerprint to EF's model cache key.

The same source-visible lifecycle boundary applies to EF compiled-model and pooling usage. `DMV1913` reports direct `UseModel(...)` selection for a visibly variable DVault model shape, while fixed-shape models and the documented design-model-to-runtime-model flow remain non-diagnostic. Stable direct EF compiled queries over generated shared-type tables remain non-diagnostic because they compile a fixed query expression rather than select a compiled EF model for a context. `DMV1914` reports direct `AddDbContextPool<TContext>(...)` registration for visibly variable DVault model shapes, while options-only contexts with one fixed metadata/model shape remain in the supported pooling baseline. The lifecycle analyzer slice does not add a runtime guard, runtime behavior change, compiled-model generator, provider-specific lifecycle guarantee, cross-assembly inference, or whole-application inference.

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

Explain output includes the selected provider capability profile, the selected provider-behavior profile, validation status, generated Data Vault entity facts, and any request-bound save/read strategy diagnostics supplied by the caller. Provider explainability is deterministic and bounded: it records provider names, profile names, strategy status, selected strategy names, finite fallback causes, and generated schema identifiers. It does not validate provider-specific SQL text, include raw SQL, expose provider query plans, or serialize credentials.

`IDataVaultReadDiagnosticsService` analyzes representative latest/current/as-of satellite, PIT as-of, and bridge read requests. Request-bound read diagnostics keep provider strategy selection in `ReadStrategy` and add a separate `ReadShape` payload that describes translated table identity, filter columns, deterministic row-selection and ordering rules, expected key/index access paths, and provider fallback caveats. Registry-backed latest-satellite and bridge diagnostics resolve metadata first, then emit the same read-shape payload as the equivalent explicit request. The [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md) is the bounded public contract for this surface. The read-shape payload includes metadata and generated schema identifiers but not raw hash-key values, as-of values, request keys, SQL text, or provider query plans.

A redacted support bundle can carry the same request-bound read-plan facts when the application supplies representative diagnostics. The JSON keeps enum/status values, generated table and column names, and bounded fallback causes while omitting request values:

```json
{
  "diagnostics": {
    "readStrategy": {
      "status": "ProviderNeutralFallback",
      "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
      "fallbackCauses": [
        {
          "kind": "UnsupportedBridgeShape"
        }
      ]
    },
    "readShape": {
      "kind": "Bridge",
      "provider": {
        "providerName": "Microsoft.EntityFrameworkCore.Sqlite",
        "capabilityProfileName": "sqlite",
        "capabilityProfileDefaulted": false,
        "providerBehaviorProfileName": "sqlite",
        "providerBehaviorDefaulted": false,
        "readStrategyStatus": "ProviderNeutralFallback",
        "readStrategyFallbackCauses": [
          {
            "kind": "UnsupportedBridgeShape"
          }
        ]
      },
      "bridge": {
        "bridgeKind": "Hierarchy",
        "bridge": {
          "metadataName": "SalesRegionHierarchy",
          "tableKind": "Bridge",
          "tableName": "BridgeSalesRegionHierarchy"
        },
        "filterEndpoint": "Ancestor",
        "endpointFilter": {
          "role": "endpointHashKeyFilter",
          "columnNames": [
            "AncestorSalesRegionHashKey"
          ]
        },
        "projectedColumns": [
          {
            "role": "endpointProjection",
            "columnNames": [
              "AncestorSalesRegionHashKey",
              "DescendantSalesRegionHashKey"
            ]
          },
          {
            "role": "depthProjection",
            "columnNames": [
              "TraversalDepth"
            ]
          }
        ]
      }
    }
  }
}
```

### Export redacted support bundles

DVault exposes `DataVaultDesignTimeCommand` and `DataVaultDesignTimeCommandHost` so applications can host design-time verbs from the project that owns the configured `DbContext`, EF design-time factory, migrations, and metadata source. The `support-bundle` verb emits one deterministic redacted JSON document under the `dvault.support-bundle.v1` contract:

```sh
dotnet run --project src/SalesVault/SalesVault.csproj -- support-bundle --output src/SalesVault/dvault-support-bundle.json
```

The default support bundle constructs the configured design-time context, runs `IDataVaultDiagnosticsService.Analyze(DbContext)`, and serializes validation, explain, save-strategy, and read-strategy diagnostics without opening a live database connection. When application code supplies representative request-bound read diagnostics, the same deterministic redacted JSON also includes the additive `readShape` section. Use `--artifact <path>` to add reviewed `dvault.model.v1` drift evidence and `--live-schema` only when the consumer application owns the reachable database, credentials, lifecycle cleanup, and CI isolation for that check.

Applications that want representative request-bound save or read strategy evidence should supply that diagnostics result from application code through `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics`. Applications that generate typed read helpers should review the same support bundle as the generator input, provide it as the single authoritative additional file, and optionally pin `DVaultTypedReadModelMetadataSourceFingerprint` to the exported metadata-source fingerprint. PIT and bridge helper generation depends on representative request-bound `ReadShape` diagnostics being supplied to that support-bundle diagnostics factory; the generic command runner does not invent representative requests. The reusable command host does not publish support bundles, attach them to tickets, intercept `dotnet ef`, or ship a standalone `dvault` CLI.

For stale typed-helper input, refresh the bundle and the analyzer configuration together:

1. Re-run the consumer-owned metadata projection and `support-bundle` command after the metadata or representative request change.
2. Replace the analyzer `AdditionalFiles` input so exactly one reviewed `dvault.support-bundle.v1` file is visible to the project.
3. Update `DVaultTypedReadModelMetadataSourceFingerprint` to the refreshed `metadataSourceFingerprint`, or remove the property while the new source is under review.
4. When `DMV1963`, `DMV1964`, `DMV1967`, or `DMV1969` reports a stale or missing PIT/bridge helper shape, update `CreateSupportBundleDiagnostics` to analyze the representative `DataVaultPitAsOfReadRequest` or `DataVaultBridgeReadRequest` that proves the helper's `ReadShape`, then export the support bundle again.
5. Rebuild and confirm `DMV1960` and `DMV1961` are gone before treating generated helper output as current.

### Run aggregate preflight checks

`DataVaultPreflight.Run(...)` aggregates the library-local validation lanes that a consumer-owned design-time entrypoint may already run separately: configured-model diagnostics, reviewed-artifact drift, explicit snapshot-model drift, idempotency live-schema preflight, migration-operation guardrails, and representative request-bound diagnostics. Each optional lane is caller-supplied; omitted lanes are reported as skipped rather than auto-discovered.

```csharp
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

using var services = new ServiceCollection()
    .AddDVaultSqlite()
    .BuildServiceProvider(validateScopes: true);

var diagnostics = services.GetRequiredService<IDataVaultDiagnosticsService>();
var readDiagnostics = services.GetRequiredService<IDataVaultReadDiagnosticsService>();
using var context = new SalesVaultDesignTimeFactory().CreateDbContext(args);

Migration migration = SalesVaultMigrationResolver.Resolve("AddCustomerProfile");

var report = DataVaultPreflight.Run(
    diagnostics,
    new DataVaultPreflightRequest(context, SalesVaultMetadata.CreateModel()) {
      ReviewedArtifactImport = SalesVaultArtifacts.ImportReviewedModel(),
      SnapshotModel = SalesVaultSnapshotMaterializer.CreateSnapshotModel(),
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
```

The aggregate facade does not couple DVault to EF `ModelSnapshot`, scan the repository for migrations or artifacts, generate representative requests, open a live database by default, execute migrations, or repair schema. Snapshot-model input is an explicit consumer-materialized `IReadOnlyModel`; idempotency input is an explicit `DataVaultLiveSchemaReadResult` from consumer-owned live-schema access; migration input is an explicit reviewed `MigrationOperation` list; representative save/read diagnostics are either precomputed by application code or produced by caller-owned factories.

### Review provider schema guardrails

The v0.29.0 provider schema guardrail baseline remains documented in [DVault v0.29.0 Release Notes](docs/releases/v0.29.0.md), is carried forward by the current v0.34.0 documentation baseline, and is anchored by [Provider Identifier And DDL Guardrail Contract](docs/plans/provider-identifier-ddl-guardrail-contract.md). Logical DVault names remain provider-neutral and traceable through `DataVaultAnnotationNames.ProducedName`; supported provider profiles may derive safe physical names only when generated DVault-owned tables, columns, keys, indexes, or constraints would otherwise be unsafe for the selected provider.

The finite provider-specific safety baseline is SQLite (`sqlite-v1`), Oracle (`oracle-v1`), PostgreSQL (`postgres-v1`), SQL Server (`sqlserver-v1`), DB2 (`db2-v1`), and MySQL (`mysql-pomelo-v1`). Unrecognized providers must not inherit provider-specific DDL safety guarantees from those profiles. Treat them as unsupported for provider-specific identifier, index, load-timestamp, and migration DDL safety claims until a future contract adds an explicit profile.

Review provider caveats before applying generated DDL. MySQL uses the `mysql-pomelo-v1` 64-character identifier cap and ignores unsupported included-index columns. Oracle appends unsupported included-index columns to the effective key and does not treat secondary indexes covered by a primary key as safe generated metadata. PostgreSQL and SQL Server preserve native included-index columns. SQLite appends unsupported included-index columns to the key when needed.

For example, a long logical satellite index name can stay visible as the produced name, while the MySQL physical identifier must fit the 64-character profile limit through a deterministic safe projection or fail before unsafe DDL is emitted. For Oracle, a hand-edited migration that recreates an index covered by the generated primary key should be treated as incompatible with the reviewed provider profile until the migration is corrected. The adopter response is to rename the source declaration or role, adjust the reviewed `loadTimestampStorage` token when that is the intentional change, keep EF migrations aligned with generated DVault metadata, or choose a supported provider/profile that can represent the shape.

Run this lane after migration scaffolding and before apply:

```sh
dotnet run --project <consumer-project> -- validate
dotnet run --project <consumer-project> -- drift --artifact <path-to-reviewed-artifact>
dotnet run --project <consumer-project> -- guardrail --migration <migration-name>
```

`DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)` and `DataVaultMigrationGuardrailReport.ToDisplayString()` classify inspected operations as `Safe`, `Risky`, or `Incompatible` with deterministic DVM findings. The guardrail lane does not intercept `dotnet ef`, execute migrations, rewrite raw SQL, or repair schema automatically.

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

The provider-neutral projection stores driving-key columns immediately after the parent hash-key column and before `HashDiff`, `LoadTimestamp`, `RecordSource`, and payload columns. The satellite primary key and parent/latest-state index expand to include the driving-key tuple, so different driving-key tuples for the same parent can coexist. Current multi-active support is intentionally narrow and opt-in. Hub-parent PITs may include one shared canonical driving-key family across referenced multi-active satellites; bridge interactions involving multi-active state, model-first link-parent PIT artifacts, incompatible driving-key-family PITs, cross-product tuple semantics, and provider-specific optimized multi-active save behavior remain future work. Provider-specific save strategies may decline multi-active batches so the provider-neutral writer handles the documented baseline.

### Provider Packages

`DCoding.Data.DVault` contains the provider-neutral API, metadata model, naming conventions, stable hashing, read helpers, diagnostics, and EF fallback writer. Provider packages extend that base registration without changing the explicit save or read APIs.

`DCoding.Data.DVault.Sqlite` registers the optimized SQLite set-based save strategy plus optimized latest-satellite, PIT, and bridge read dispatch. `DCoding.Data.DVault.Db2` registers `IBM.EntityFrameworkCore` provider behavior and the `db2-v1` capability profile for DB2 schema projection, diagnostics, and provider selection. DB2 currently uses the provider-neutral save and read pipelines: representative external smoke coverage exercises hub, link, and satellite writes plus latest/as-of satellite, PIT, and bridge reads through provider-neutral fallback. `DCoding.Data.DVault.Postgres` registers an optimized Npgsql/PostgreSQL strategy for clean contexts that use set-based `INSERT ... ON CONFLICT DO NOTHING` hub and link writes plus latest-state satellite checks. `DCoding.Data.DVault.SqlServer` registers an optimized SQL Server strategy for clean contexts with set-based unique-row inserts and latest-state satellite checks. `DCoding.Data.DVault.Oracle` registers an Oracle-gated insert strategy for clean `Oracle.EntityFrameworkCore` contexts that meet the native bulk gate, including ordinary hub, link, and satellite batches. `DCoding.Data.DVault.MySql` supports `Pomelo.EntityFrameworkCore.MySql` and `MySql.EntityFrameworkCore`, and registers an optimized MySQL strategy for clean supported MySQL contexts. PostgreSQL, SQL Server, MySQL, and Oracle provider packages also register diagnostics-gated PIT and bridge read strategy candidates; they do not register optimized latest-satellite read strategies. DB2 does not register a provider-native save strategy, provider-native read strategy, or built-in live-schema reader in this release.

Provider-native bulk dispatch is diagnostics-gated. Dirty tracked contexts, multi-active satellite batches, and provider-name mismatches decline to the provider-neutral writer. SQL Server native dispatch also requires at least `50` total operations and at most `500` satellite operations. MySQL native dispatch requires at least `50` total operations and accepts both Pomelo and official MySQL EF Core provider names. Oracle native dispatch requires at least `50` total operations and accepts at most `10000` satellite operations.

The provider-optimized wording is intentionally provider-specific. PostgreSQL larger eligible ordered batches use staged COPY and smaller batches keep the direct or UNNEST path. MySQL larger eligible ordered batches use staged bulk and smaller eligible native batches keep the multi-row path. SQL Server remains described as the current native-bulk strategy rather than a direct-versus-staged split. Oracle remains described as the retained direct optimized batching strategy with staged Oracle bulk not selected until measured evidence shows a net win over the direct path.

Provider-specific save-strategy registration and provider capability-profile selection are separate surfaces. The core package includes built-in capability profiles for the known SQLite, DB2, PostgreSQL, SQL Server, Oracle, Pomelo MySQL, and official MySQL EF provider names. Direct `ApplyDataVaultMetadata(...)` calls can pass an explicit `DataVaultProviderCapabilityProfile` when an application wants deterministic provider-specific schema projection at model-building time. Registry-backed `UseDataVaultMetadata(...)` remains the easiest path when one metadata source should drive schema, save, and read usage.

Provider-specific schema guardrail claims are limited to those same built-in profiles: `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, `db2-v1`, and `mysql-pomelo-v1`. Logical DVault names remain provider-neutral, and generated physical names, included-index handling, duplicate-index caveats, and load-timestamp storage mappings should be reviewed against the selected profile before provider-specific DDL is applied. The DB2 profile uses a 128-character identifier limit, ISO 8601 UTC text timestamp storage by default, `CLOB` payload text, and appended-key handling for included-index columns that DB2 cannot represent as included columns. Unrecognized provider names do not inherit provider-specific DDL safety guarantees from the supported profiles.

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

The built-in live-schema reader has successful catalog readers for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL. It is available through `DataVaultLiveSchemaReader.ReadAsync(context)` and returns a classified `DataVaultLiveSchemaReadResult` rather than silently skipping unsupported or unavailable environments. Compare the result with expected metadata using `DataVaultLiveSchemaDriftReporter.Compare`:

```csharp
using DCoding.Data.DVault;

using var context = new SalesVaultContext(options);
var liveSchema = await DataVaultLiveSchemaReader.ReadAsync(context);
var report = DataVaultLiveSchemaDriftReporter.Compare(metadataModel, liveSchema);

if (report.HasBlockingDifferences) {
  throw new InvalidOperationException(report.ToDisplayString());
}
```

Built-in provider dispatch recognizes `Microsoft.EntityFrameworkCore.Sqlite`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.SqlServer`, `Oracle.EntityFrameworkCore`, `IBM.EntityFrameworkCore`, `MySql.EntityFrameworkCore`, and `Pomelo.EntityFrameworkCore.MySql`. Both MySQL EF Core provider names map to the MySQL reader. `IBM.EntityFrameworkCore` is recognized as the DB2 provider name, but DB2 live-schema reading is explicitly unsupported until a DB2 catalog reader is implemented.

Providers without a built-in live-schema reader return `DataVaultLiveSchemaReadStatus.UnsupportedProvider`. A supported provider whose database cannot be reached returns `DataVaultLiveSchemaReadStatus.Unavailable`. Both outcomes become stable blocking drift differences when passed to `DataVaultLiveSchemaDriftReporter.Compare`.

SQLite remains the default local live-schema proof because it does not require external infrastructure. PostgreSQL, SQL Server, Oracle, and MySQL live-schema checks require consumer-managed reachable databases, connection strings, credentials, lifecycle cleanup, and CI isolation. Keep those external provider checks opt-in behind the documented connection-string environment variables: `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, and `DVAULT_TEST_MYSQL_CONNECTION_STRING`. DB2 has opt-in live smoke tests, but not a live-schema reader. Default local test execution does not require those external databases.

## Current v0.34.0 DB2 Documentation Baseline

The v0.34.0 release record is the current coordinated eight-package documentation baseline for the dual consumer package-version lines. See `docs/releases/v0.34.0.md` for package scope, the `8.34.0` / `net8.0` / EF Core 8 line, the `10.34.0` / `net10.0` / EF Core 10 line, the DB2 provider baseline, manual publication separation, validation evidence, compatibility caveats, and non-goals.

Notable DB2-facing documentation points:

- `DCoding.Data.DVault.Db2` is the DB2 provider package id for both supported package lines.
- The DB2 provider package targets `net8.0` and `net10.0`, depends on `IBM.EntityFrameworkCore` `8.0.0.400` for `net8.0`, and depends on `IBM.EntityFrameworkCore` `10.0.0.100` for `net10.0`.
- `AddDVaultDb2()` registers DVault defaults, DB2 provider behavior, and the `db2-v1` capability profile.
- DB2 support is currently provider-neutral at execution time: no DB2-specific optimized save strategy, optimized read strategy, live-schema reader, container fixture, or mandatory CI database dependency is introduced by this baseline.
- DB2 external tests are opt-in behind `DVAULT_TEST_DB2_CONNECTION_STRING`; a developer-managed DB2 database or container can be used, but DVault does not provision it.

## Previous v0.33.0 Release Notes

The v0.33.0 release record is the previous coordinated seven-package documentation baseline for the dual consumer package-version lines. See `docs/releases/v0.33.0.md` for package scope, the `8.33.0` / `net8.0` / EF Core 8 line, the `10.33.0` / `net10.0` / EF Core 10 line, the finite provider/version matrix, manual publication separation, validation evidence, compatibility caveats, and non-goals.

It carries forward the v0.32.0 benchmark-driven provider threshold evidence and review-only provider-specific SQL artifact manifest lane, v0.31.0 performance decision-tree and observability baseline, v0.30.0 typed helper support-bundle freshness baseline, v0.29.0 provider schema guardrail baseline, v0.28.0 provider read optimization evidence boundary, v0.27.0 EF lifecycle analyzer guardrails, v0.26.0 provider-tuning diagnostics and benchmark verifier evidence, v0.25.0 ReadShape and typed helper boundary, v0.24.0 async streaming and EF safety boundary, and v0.21.0 PIT/bridge maintenance boundary from earlier releases.

Notable user-facing documentation changes:

- The coordinated seven-package family kept the same package ids across both compatibility lines.
- At that boundary, consumer projects chose exactly one package-version line: `8.33.0` for `net8.0` and EF Core 8, or `10.33.0` for `net10.0` and EF Core 10.
- `v0.33.0` is the release-note and planning label, not a consumer-facing `0.33.0` package version.
- Runtime, provider, and analyzer packages must stay aligned to the selected line; analyzer references remain local with `PrivateAssets="all"`.
- The finite provider/version matrix is grounded in shared standards and `EfCoreProviderVersionMatrixTests.cs`, including `MySql.EntityFrameworkCore` `10.0.7` as the documented evidence exception for both targets.
- Package-tested evidence, default local SQLite-backed and provider-smoke evidence, and live PostgreSQL, SQL Server, Oracle, and MySQL opt-in lanes behind `DVAULT_TEST_*` connection-string gates remain distinct.
- No new runtime behavior, provider provisioning, standalone DVault CLI, release automation, package publication, or mandatory live external-provider test requirement is introduced by the v0.33.0 documentation baseline.

Primary v0.33.0 documentation anchors are [DVault v0.33.0 Release Notes](docs/releases/v0.33.0.md), [Manual NuGet Publication Checklist](docs/manual-nuget-publication.md), [Shared Implementation Standards](docs/plans/shared-implementation-standards.md#v033-compatibility-contract), [DCoding.Data.DVault.Analyzers README](src/DCoding.Data.DVault.Analyzers/README.md), [Production Adoption Checklist](docs/production-adoption-checklist.md), and [EfCoreProviderVersionMatrixTests.cs](tests/DCoding.Data.DVault.Tests/Unit/EfCoreProviderVersionMatrixTests.cs).

## v0.32.0 Historical Release Notes

The v0.32.0 release record moved the coordinated seven-package baseline forward for benchmark-driven provider threshold evidence and the review-only provider-specific SQL artifact manifest lane. See `docs/releases/v0.32.0.md` for package scope, boundary shift from v0.31.0, benchmark evidence anchors, the design-time `sql-artifact` command surface, validation surfaces, and non-goals.

It carried forward the v0.31.0 performance decision-tree and observability baseline, v0.30.0 typed helper support-bundle freshness baseline, v0.29.0 provider schema guardrail baseline, v0.28.0 provider read optimization evidence boundary, v0.27.0 EF lifecycle analyzer guardrails, v0.26.0 provider-tuning diagnostics and benchmark verifier evidence, v0.25.0 ReadShape and typed helper boundary, v0.24.0 async streaming and EF safety boundary, and v0.21.0 PIT/bridge maintenance boundary from earlier releases.

Notable historical user-facing changes:

- Provider save threshold guidance has checked-in v0.32 benchmark evidence bundles for local Podman PostgreSQL, SQL Server, MySQL, and Oracle scale runs under the shared artifact-triplet contract.
- MySQL provider registration deliberately routes tiny satellite-history rows through provider-neutral fallback while preserving staged bulk for larger eligible rows.
- SQL Server native-bulk gate documentation remains 50 minimum operations and 500 maximum satellite operations, with fallback rows worded according to request-bound diagnostics.
- Oracle remains on the direct optimized path; staged Oracle bulk is recorded as `not-selected-no-measured-win`.
- The `sql-artifact` design-time verb emits a `dvault.sql-artifact.v1` SQL Server dry-run manifest for `provider-native-bulk-ingestion` review only; it does not emit deployable SQL payload files, auto-deploy artifacts, or create runtime dispatch.
- No package publication, automatic stored-procedure generation, runtime artifact invocation, migration synchronization, provider support expansion, DBA workflow automation, credential handling, or release automation was introduced by the v0.32.0 release baseline.

Primary v0.32.0 documentation anchors are [DVault v0.32.0 Release Notes](docs/releases/v0.32.0.md), [Performance Profiles](docs/performance-profiles.md), [Provider-Specific SQL Artifact Contract](docs/plans/provider-specific-sql-artifact-contract.md), [DVault EF Design-Time Workflow](docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [DVault V1 Explicit Save Service](docs/architecture/dvault-v1-explicit-save-service.md), and [Production Adoption Checklist](docs/production-adoption-checklist.md).

## v0.31.0 Historical Release Notes

The v0.31.0 release record moved the coordinated seven-package baseline forward for performance decision-tree guidance, application-owned observability examples, realistic EF Core quickstart evidence, and carried-forward package scope. See `docs/releases/v0.31.0.md` for package scope, boundary shift from v0.30.0, evidence anchors, validation surfaces, and non-goals.

It carries forward the v0.30.0 typed helper support-bundle freshness baseline, v0.29.0 provider schema guardrail baseline, v0.28.0 provider read optimization evidence boundary, v0.27.0 EF lifecycle analyzer guardrails, v0.26.0 provider-tuning diagnostics and benchmark verifier evidence, v0.25.0 ReadShape and typed helper boundary, v0.24.0 async streaming and EF safety boundary, and v0.21.0 PIT/bridge maintenance boundary from earlier releases.

Notable user-facing changes:

- Performance guidance now has an explicit adopter decision tree for small app-local vaults, medium chunked ingestion, staged provider ingestion, and read-model-heavy workloads.
- Observability examples keep `AddDVault()` telemetry-free by default, make `AddDVaultTelemetry()` opt-in for built-in metrics, and keep Activity tracing listener-driven through the `DCoding.Data.DVault` ActivitySource.
- The quickstarts now document a realistic customer-profile flow with fixed CRM import/change timestamps, explicit record sources, explicit save-service writes, typed latest/as-of reads, and bounded diagnostics.
- Evidence links point to [Performance Profiles](docs/performance-profiles.md), [DVault Quickstart Examples](examples/README.md), [DVault V1 Activity Tracing Contract](docs/architecture/dvault-v1-activity-tracing-contract.md), and the root benchmark summary triplet instead of duplicating decision trees, tracing rules, or console output.
- No new runtime behavior, dashboard, exporter, collector, hosting setup, benchmark rerun, package publication, automatic PIT or bridge maintenance, ingestion orchestration, provider-specific SQL artifact workflow, or release automation is introduced by the v0.31.0 release baseline.

Primary v0.31.0 documentation anchors are [DVault v0.31.0 Release Notes](docs/releases/v0.31.0.md), [Performance Profiles](docs/performance-profiles.md), [DVault Quickstart Examples](examples/README.md), [Production Adoption Checklist](docs/production-adoption-checklist.md), and [DVault V1 Activity Tracing Contract](docs/architecture/dvault-v1-activity-tracing-contract.md).

## v0.30.0 Historical Release Notes

The v0.30.0 release record moved the coordinated seven-package baseline forward for support-bundle freshness, stale `DVaultTypedReadModelMetadataSourceFingerprint` recovery, generator source-boundary diagnostics, and request-bound `ReadShape` troubleshooting for typed satellite, PIT, and bridge helper generation. See `docs/releases/v0.30.0.md` for package scope, boundary shift from v0.29.0, the authoritative support-bundle refresh path, stale input recovery checklist, validation surfaces, and non-goals.

It carries forward the v0.29.0 provider schema guardrail baseline, v0.28.0 provider read optimization evidence boundary, v0.27.0 EF lifecycle analyzer guardrails, v0.26.0 provider-tuning diagnostics and benchmark verifier evidence, v0.25.0 ReadShape and typed helper boundary, v0.24.0 async streaming and EF safety boundary, and v0.21.0 PIT/bridge maintenance boundary from earlier releases.

Notable user-facing changes:

- Typed helper inputs must be refreshed by re-exporting exactly one authoritative `dvault.support-bundle.v1` file after metadata, reviewed artifact, provider profile, load-timestamp storage, or representative read request changes.
- Projects that pin `DVaultTypedReadModelMetadataSourceFingerprint` must update it to the refreshed bundle fingerprint or remove the pin while the bundle is under review; `DMV1960` identifies unresolved or non-authoritative support-bundle input, and `DMV1961` identifies stale fingerprint pins.
- PIT and bridge helper troubleshooting now explicitly routes through representative `CreateSupportBundleDiagnostics` read requests so the exported support bundle carries the request-bound `readShape.pit` or `readShape.bridge` facts the generator consumes.
- Helper-shape diagnostics remain bounded to the affected helper; unsupported PIT, bridge, dynamic-query, or skipped-helper outcomes do not suppress unrelated supported helpers in the same valid support bundle.
- No new runtime behavior, source-generator inference path, provider-specific SQL generation, support-bundle publishing flow, benchmark rerun, package publication, or release automation is introduced by the v0.30.0 release baseline.

Primary v0.30.0 documentation anchors are [DVault v0.30.0 Release Notes](docs/releases/v0.30.0.md), [DVault EF Design-Time Workflow](docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [DVault V1 Typed PIT And Bridge Helper Contract](docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md), and the package-local [DCoding.Data.DVault.Analyzers README](src/DCoding.Data.DVault.Analyzers/README.md).

## v0.29.0 Historical Release Notes

The v0.29.0 release record moved the coordinated seven-package schema guardrail baseline forward for provider naming and DDL review. See `docs/releases/v0.29.0.md` for package scope, boundary shift from v0.28.0, finite supported-provider baseline, logical-to-physical name guidance, DDL caveats, adopter workflow, examples, validation surfaces, and non-goals.

It carries forward the v0.28.0 provider read optimization evidence boundary, v0.27.0 EF lifecycle analyzer guardrails, v0.26.0 provider-tuning diagnostics and benchmark verifier evidence, v0.25.0 ReadShape and typed helper boundary, v0.24.0 async streaming and EF safety boundary, and v0.21.0 PIT/bridge maintenance boundary from earlier releases.

Notable user-facing changes:

- Provider-specific DDL safety claims are bounded to SQLite, Oracle, PostgreSQL, SQL Server, and MySQL through the existing `sqlite-v1`, `oracle-v1`, `postgres-v1`, `sqlserver-v1`, and `mysql-pomelo-v1` capability profiles.
- Logical DVault names remain provider-neutral and traceable through `DataVaultAnnotationNames.ProducedName`; provider profiles may derive safe physical names only for DVault-owned generated metadata when identifier length, reserved-word, escaping, included-index, duplicate-index, or collision rules require it.
- Included-index and duplicate-index caveats are documented for generated DDL review: PostgreSQL and SQL Server preserve native include columns, SQLite and Oracle append unsupported include columns to the key, MySQL ignores unsupported include columns, and Oracle does not treat primary-key-covered secondary indexes as safe generated metadata.
- The adopter workflow routes through consumer-owned `validate`, reviewed-artifact `drift --artifact`, and `guardrail --migration` design-time lanes before schema changes are applied.
- Unrecognized providers do not inherit provider-specific DDL safety guarantees from supported profiles, and the guardrail lane does not apply migrations, repair schema, rewrite raw SQL, or intercept EF commands.

Primary v0.29.0 documentation anchors are [Provider Identifier And DDL Guardrail Contract](docs/plans/provider-identifier-ddl-guardrail-contract.md), [DataVaultProviderCapabilities.cs](src/DCoding.Data.DVault/DataVaultProviderCapabilities.cs), [DataVaultAnnotationNames.cs](src/DCoding.Data.DVault/DataVaultAnnotationNames.cs), [DataVaultMigrationOperationDiagnostics.cs](src/DCoding.Data.DVault/DataVaultMigrationOperationDiagnostics.cs), [DataVaultMigrationGuardrailReport.cs](src/DCoding.Data.DVault/DataVaultMigrationGuardrailReport.cs), and [DVault EF Design-Time Workflow](docs/architecture/dvault-dotnet-ef-design-time-workflow.md).

## v0.28.0 Historical Release Notes

The v0.28.0 release record moved the coordinated seven-package evidence baseline forward for provider read optimization guidance. See `docs/releases/v0.28.0.md` for package scope, boundary shift from v0.27.0, provider read optimization boundaries, evidence posture, validation surfaces, and non-goals.

It carries forward the v0.27.0 EF lifecycle analyzer guardrails, v0.26.0 provider-tuning diagnostics and benchmark verifier evidence, v0.25.0 ReadShape and typed helper boundary, v0.24.0 async streaming and EF safety boundary, and v0.21.0 PIT/bridge maintenance boundary from earlier releases.

Notable user-facing changes:

- SQLite remains the only repository-proven optimized latest-satellite read provider path.
- `AddDVaultSqlite()`, `AddDVaultPostgres()`, `AddDVaultSqlServer()`, `AddDVaultMySql()`, and `AddDVaultOracle()` are documented as diagnostics-gated PIT/bridge read strategy candidate registrations for supported maintained PIT and bridge shapes.
- The checked-in benchmark evidence distinguishes completed SQLite latest-satellite, PIT, and bridge timing rows from optional PostgreSQL, SQL Server, MySQL, and Oracle read guidance rows that remain skipped when their connection-string environment variables are unset.
- `IDataVaultReadDiagnosticsService` remains the bounded explain surface for `ReadStrategy`, selected strategy name, finite fallback causes, and `ReadShape` facts without raw SQL, provider query plans, automatic index advice, or provider-specific physical-design promises.
- Outside the explicitly registered diagnostics-gated PIT/bridge provider strategy candidates and the read diagnostics fallback-cause expansion, no implicit PIT/bridge maintenance, scheduling, raw SQL disclosure, query-plan disclosure, automatic physical tuning, new external-provider latest-satellite strategy, benchmark rerun, package publication, or release automation is introduced by the v0.28.0 release baseline.

Primary v0.28.0 validation surfaces are [BenchmarkScenarioExecutionTests.cs](tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs), [DataVaultProviderReadStrategyTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultProviderReadStrategyTests.cs), [DataVaultRelationalPitBridgeReadStrategyParityTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultRelationalPitBridgeReadStrategyParityTests.cs), [ExplicitDataVaultSaveServiceTests.cs](tests/DCoding.Data.DVault.Tests/Unit/ExplicitDataVaultSaveServiceTests.cs), [DataVaultDiagnostics.cs](src/DCoding.Data.DVault/DataVaultDiagnostics.cs), the provider package service-collection extensions under `src/DCoding.Data.DVault.*`, and the root [benchmark-summary.md](benchmark-summary.md), [benchmark-summary.csv](benchmark-summary.csv), and [benchmark-summary.json](benchmark-summary.json) triplet.

## v0.27.0 Historical Release Notes

The v0.27.0 release record moved the coordinated seven-package documentation baseline forward for EF Core lifecycle analyzer guardrails. See `docs/releases/v0.27.0.md` for compatibility posture, validation evidence, bounded examples, and non-goals.

It carries forward the v0.26.0 provider-tuning diagnostics, benchmark verifier evidence, migration and idempotency guardrails, stored-procedure artifact boundary, request-bound ReadShape diagnostics, support-bundle-driven typed helper boundary, and manual package publication separation from earlier releases.

Notable user-facing changes:

- The analyzer catalog now describes `DMV1910` and `DMV1911` for generated shared-type-table misuse plus `DMV1912` through `DMV1914` for source-visible EF lifecycle misuse around caller-owned model-cache discriminators, direct `UseModel(...)` compiled-model selection, and direct `AddDbContextPool<TContext>(...)` usage.
- Registry-backed `UseDataVaultMetadata(...)`, fixed-shape `UseModel(runtimeModel)`, stable direct EF compiled queries over generated shared-type tables, and options-only pooling for one fixed metadata/model shape remain supported and non-diagnostic.
- The lifecycle analyzer slice remains high-confidence and source-visible only. It does not infer across assemblies, expand arbitrary helpers, inspect generated compiled-model artifacts, prove provider-specific SQL behavior, or diagnose pooled factory patterns.
- No runtime guard, runtime behavior change, compiled-model generator, provider-specific lifecycle guarantee, benchmark rerun, package publication, or release automation is introduced by the v0.27.0 release baseline.

Primary v0.27.0 validation surfaces are [DataVaultEfCoreMisuseAnalyzerTests.cs](tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultEfCoreMisuseAnalyzerTests.cs), [DataVaultCompiledCompatibilitySqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultCompiledCompatibilitySqliteTests.cs), [DVault EF Compiled Compatibility](docs/architecture/dvault-ef-compiled-compatibility.md), the root [dvault-ef-compiled-compatibility.md](dvault-ef-compiled-compatibility.md) entrypoint, and the package-local [DCoding.Data.DVault.Analyzers README](src/DCoding.Data.DVault.Analyzers/README.md).

## v0.26.0 Historical Release Notes

The v0.26.0 release record moved the coordinated seven-package baseline forward for provider-tuning diagnostics, benchmark verifier evidence, migration and idempotency guardrails, stored-procedure artifact boundaries, request-bound ReadShape diagnostics, and support-bundle-driven typed read helpers. It preserved the v0.25.0 typed helper boundary, the v0.24.0 async streaming and EF safety boundary, the v0.23.0 Activity tracing and performance-profile boundary, the v0.21.0 PIT/bridge maintenance and read boundary, and the manual package publication separation from earlier releases. See `docs/releases/v0.26.0.md` for compatibility posture, validation evidence, bounded examples, and non-goals.

Notable user-facing changes:

- Performance guidance is explicitly organized around the four checked-in categories: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model heavy.
- Provider eligibility diagnostics are documented as bounded `SaveStrategy` and `ReadStrategy` facts: status, provider name, selected strategy name when present, finite fallback causes, candidate facts, and gate requirements.
- Benchmark verifier evidence points to the root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet plus row-consistency and regression-budget checks instead of duplicating raw tables.
- Migration guardrails and idempotency preflight remain consumer-owned operations: callers supply migration operations, reviewed artifacts, snapshot models, live-schema read results, and representative diagnostics explicitly.
- Stored-procedure and provider-specific SQL artifact discussion is limited to opt-in design-time artifacts with consumer-owned deployment, versioning, invocation, rollback, cleanup, credentials, transactions, and lifecycle.
- At the v0.26.0 release boundary, SQLite remained the only repository-proven optimized latest-satellite, PIT, and bridge read provider path. The current v0.34.0 baseline preserves SQLite-only latest-satellite optimization, documents later PIT/bridge candidate evidence for PostgreSQL, SQL Server, MySQL, and Oracle, and keeps DB2 on provider-neutral read fallback.
- No default save/read runtime dispatch changes, new benchmark artifact schema, package publication, provider-specific SQL generation, automatic migrations, automatic schema repair, stored-procedure runtime dispatch, or deployment automation are introduced by the v0.26.0 release baseline.

Primary v0.26.0 validation surfaces are [BenchmarkScenarioExecutionTests.cs](tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs), [DataVaultDiagnosticsIntegrationTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs), [DataVaultPreflightTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultPreflightTests.cs), [DataVaultIdempotencyPreflightTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultIdempotencyPreflightTests.cs), [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md), [DVault Dotnet EF Design-Time Workflow](docs/architecture/dvault-dotnet-ef-design-time-workflow.md), [Performance Profiles](docs/performance-profiles.md), and the package-local [DCoding.Data.DVault.Analyzers README](src/DCoding.Data.DVault.Analyzers/README.md).

## v0.25.0 Historical Release Notes

The v0.25.0 release record moved the coordinated seven-package documentation baseline forward for request-bound ReadShape diagnostics and support-bundle-driven typed read helpers. It preserves the v0.24.0 async streaming and EF safety boundary, the v0.23.0 Activity tracing and performance-profile boundary, the v0.22.0 support-bundle generator and stable hash governance boundary, the v0.21.0 PIT/bridge maintenance and read boundary, and the manual package publication separation from earlier releases. See `docs/releases/v0.25.0.md` for compatibility posture, validation evidence, the `DMV1960` through `DMV1969` typed read-model diagnostic range, and non-goals.

Notable user-facing changes:

- `IDataVaultReadDiagnosticsService.Analyze(...)` request-bound output is documented as the current read-plan surface: `DataVaultDiagnosticsResult.ReadStrategy` plus additive `DataVaultDiagnosticsResult.ReadShape`.
- Redacted `dvault.support-bundle.v1` JSON can include `readShape` when application code supplies representative read diagnostics through `DataVaultDesignTimeCommandHost.CreateSupportBundleDiagnostics`.
- Generated typed read helpers are support-bundle-driven over `IDataVaultReadService` for satellite latest/current/as-of, PIT as-of, and bounded bridge traversal shapes.
- PIT helpers cover hub-parent ordinary PITs, hub-parent multi-active PITs with one canonical driving-key family, and bounded link-parent PITs with unique non-multi-active satellites.
- Bridge helpers cover many-to-many `Read...FromAsync`/`Read...ToAsync` methods and hierarchy `Read...AncestorAsync`/`Read...DescendantAsync` methods with required `maximumDepth`.
- Dynamic `IDataVaultReadService` requests remain the default for runtime-built shapes and caller-selected projectors; consumer-owned EF compiled queries remain the direct-query option for stable shared-type-table expressions.
- Read-plan diagnostics and generated helpers do not add provider-specific SQL generation, LINQ-provider behavior, automatic index advice, PIT or bridge maintenance orchestration, raw request-value export, or unbounded traversal.

Primary v0.25.0 validation surfaces are [DataVaultDiagnosticsTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultDiagnosticsTests.cs), [DataVaultTypedReadModelSourceGeneratorTests.cs](tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs), [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md), [DVault V1 Typed PIT And Bridge Helper Contract](docs/architecture/dvault-v1-typed-pit-bridge-helper-contract.md), and the package-local [DCoding.Data.DVault.Analyzers README](src/DCoding.Data.DVault.Analyzers/README.md).

## v0.24.0 Historical Release Notes

The v0.24.0 release record moved the coordinated seven-package baseline forward for async chunk-source saves, async helper convenience, guidance-only EF safety, listener-driven Activity tracing, and benchmark-backed performance profiles. It preserved the v0.23.0 Activity tracing and performance-profile boundary, the v0.22.0 typed satellite helper and stable hash governance boundary, the v0.21.0 PIT/bridge maintenance and read boundary, the v0.20.0 provider-specific optimized write guidance, and the manual package publication separation from earlier releases. See `docs/releases/v0.24.0.md` for package scope, compatibility posture, validation evidence, benchmark evidence references, and non-goals.

Notable user-facing changes:

- `IDataVaultSaveService.SaveAsync(DbContext, IAsyncEnumerable<DataVaultSaveChunk>, ...)` adds an async chunk-source shape over the existing explicit save boundary.
- `SaveAsync<TSource>(...)`, `SaveHubsAsync(...)`, `SaveLinksAsync(...)`, and `SaveOrdinaryHubSatellitesAsync(...)` are convenience helpers that map caller-ordered async source rows into bounded chunks and require an explicit positive `chunkSize`.
- `DataVaultBulkSaveRequest` remains the best fit for already-materialized ordered request sets, `DataVaultChunkedSaveRequest` remains the materialized bounded-chunk shape, and the async overload is for already-asynchronous bounded chunk producers.
- Async streaming preserves caller ordering, cancellation, and caller-owned transaction behavior. It does not introduce provider-native async writes, background ingestion, schedulers, file or CDC ingestion, or implicit `SaveChanges` streaming.
- EF model-cache safety guidance stays documentation-only: registry-backed `UseDataVaultMetadata(...)` isolates DVault metadata sources, caller-owned model-shape discriminators belong in an application `IModelCacheKeyFactory`, and `UseModel(...)` plus `AddDbContextPool<TContext>(...)` are documented only for one fixed realized model shape.
- `DCoding.Data.DVault.Analyzers` still reports `DMV1910` and `DMV1911` for generated shared-type-table exposure and direct generated-table writes. Those IDs are not model-cache or pooling diagnostics, and v0.24.0 does not add such diagnostics.
- Activity tracing remains listener-driven through the `DCoding.Data.DVault` ActivitySource and keeps the v0.23.0 redaction boundary.
- Performance guidance is consolidated in [Performance Profiles](docs/performance-profiles.md) with four adopter starting profiles: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model heavy.
- Timing claims remain tied to the root [benchmark-summary.md](benchmark-summary.md), [benchmark-summary.csv](benchmark-summary.csv), and [benchmark-summary.json](benchmark-summary.json) triplet and its run context.
- The `customer-profile-streaming-save` evidence now includes the provider-neutral `dvault-adddvault-fallback/async-source-bounded-10` row beside the materialized bulk and bounded chunked rows.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle benchmark rows remain visible as skipped rows when their connection-string environment variables are unset; the current checked-in evidence does not claim measured external-provider wins.

Primary v0.24.0 validation surfaces are [ExplicitDataVaultSaveServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/ExplicitDataVaultSaveServiceSqliteTests.cs), [DataVaultAsyncSaveHelperTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultAsyncSaveHelperTests.cs), [DataVaultTypedMapperSaveServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultTypedMapperSaveServiceSqliteTests.cs), [BenchmarkScenarioExecutionTests.cs](tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs), [Public API snapshots](docs/quality/api-surface-snapshots.md), [Performance Profiles](docs/performance-profiles.md), [DVault V1 Streaming Explicit Save Contract](docs/architecture/dvault-v1-streaming-explicit-save-contract.md), and [DVault EF Compiled Compatibility](docs/architecture/dvault-ef-compiled-compatibility.md).

## v0.23.0 Historical Release Notes

The v0.23.0 release record moved the coordinated seven-package baseline forward for listener-driven Activity tracing and benchmark-backed performance profiles. It preserved the v0.22.0 typed satellite helper and stable hash governance boundary, the v0.21.0 PIT/bridge maintenance and read boundary, the v0.20.0 provider-specific optimized write guidance, and the manual package publication separation from earlier releases. See `docs/releases/v0.23.0.md` for package scope, compatibility posture, validation evidence, benchmark evidence references, and non-goals.

Notable user-facing changes:

- Activity tracing is available and documented as listener-driven through the `DCoding.Data.DVault` ActivitySource.
- `AddDVault()` remains telemetry-free by default; `AddDVaultTelemetry()`, built-in `System.Diagnostics.Metrics`, and `IDataVaultTelemetryObserver` remain sibling opt-in observability surfaces rather than prerequisites for tracing.
- The Activity tracing redaction boundary is explicit: no raw business keys or hash keys, payload values, record sources, SQL text, credentials, connection strings, provider messages, exception messages, stack traces, or other high-cardinality diagnostic text in Activity names, tags, events, status descriptions, or exception metadata.
- Performance guidance is consolidated in [Performance Profiles](docs/performance-profiles.md) with four adopter starting profiles: small app-local vault, medium chunked ingestion, staged provider ingestion, and read-model heavy.
- Timing claims remain tied to the root [benchmark-summary.md](benchmark-summary.md), [benchmark-summary.csv](benchmark-summary.csv), and [benchmark-summary.json](benchmark-summary.json) triplet and its run context.
- Optional PostgreSQL, SQL Server, MySQL, and Oracle benchmark rows remain visible as skipped rows when their connection-string environment variables are unset; the checked-in v0.23.0 evidence does not claim measured external-provider wins.

Primary v0.23.0 validation surfaces are [DataVaultActivityTracingTests.cs](tests/DCoding.Data.DVault.Tests/Unit/DataVaultActivityTracingTests.cs), [DataVaultPitMaintenanceServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs), [DataVaultBridgeMaintenanceServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs), [BenchmarkScenarioExecutionTests.cs](tests/DCoding.Data.DVault.Tests/Integration/BenchmarkScenarioExecutionTests.cs), [Performance Profiles](docs/performance-profiles.md), and [Performance Evidence And Benchmark Artifact Contract](docs/plans/performance-evidence-benchmark-artifact-contract.md).

## v0.22.0 Historical Release Notes

The v0.22.0 release recorded the typed satellite read-model and stable-hash governance documentation rollout as the prior coordinated seven-package baseline while preserving the explicit save/read service boundaries, PIT/bridge maintenance limits, support-bundle transport ownership, public API snapshot posture, compiled-query guidance, and package publication separation from earlier releases. See `docs/releases/v0.22.0.md` for the release-note record, source-backed typed-helper evidence, public API snapshot evidence, stable `sha256-v1` compatibility vectors, compiled EF query alternative, validation commands, and package verification posture.

Notable user-facing changes:

- Typed satellite read-model generation is documented as opt-in with `DVaultGenerateTypedReadModels=true`, exactly one authoritative `dvault.support-bundle.v1` additional file, and optional `DVaultTypedReadModelMetadataSourceFingerprint` drift enforcement.
- Generated typed helpers are limited to hub-parent, link-parent, and deterministic multi-active satellites with `Read...CurrentAsync`, `Read...LatestAsync`, and `Read...AsOfAsync` methods over `IDataVaultReadService`.
- Raw `dvault.model.v1` artifacts are documented as reviewed workflow inputs that must be imported and projected before a consumer-owned `support-bundle` export; the generator does not parse raw model artifacts directly.
- PIT and bridge shapes remain on runtime read-service and diagnostics surfaces and do not emit typed helpers in this release.
- Dynamic `IDataVaultReadService` requests and consumer-owned EF compiled queries remain the documented alternatives outside the generated satellite-helper boundary.
- Stable hash governance is linked to the `sha256-v1` contract and published vector tests; provider packages must preserve those values instead of substituting provider SQL hashing behind the public save contract.
- Public API references remain limited to the committed core and provider snapshot baselines for `DCoding.Data.DVault`, `Sqlite`, `Postgres`, `SqlServer`, `Oracle`, and `MySql`.

Primary v0.22.0 validation surfaces are [DataVaultTypedReadModelSourceGeneratorTests.cs](tests/DCoding.Data.DVault.Tests/Analyzers/DataVaultTypedReadModelSourceGeneratorTests.cs), [StableHashServiceTests.cs](tests/DCoding.Data.DVault.Tests/Unit/StableHashServiceTests.cs), [ApiSurfaceSnapshotTests.cs](tests/DCoding.Data.DVault.Tests/Unit/ApiSurfaceSnapshotTests.cs), [API Surface Snapshots](docs/quality/api-surface-snapshots.md), [Stable Hashing Contract](docs/plans/stable-hashing-contract.md), and [DVault EF Compiled Compatibility](docs/architecture/dvault-ef-compiled-compatibility.md).

## v0.21.0 Historical Release Notes

The v0.21.0 release recorded the PIT and bridge completeness documentation rollout as a historical coordinated seven-package baseline while preserving the EF safety, aggregate preflight, explicit service, opt-in telemetry, redacted support-bundle, consumer-owned design-time boundaries, provider-optimized write guidance, and performance-evidence posture from earlier releases. See `docs/releases/v0.21.0.md` for the release-note record, package scope, PIT/bridge maintenance boundary, original SQLite-only optimized PIT/bridge read dispatch, diagnostics and read-shape evidence, benchmark artifact links, validation evidence, and package verification posture. See `docs/architecture/dvault-v1-pit-bridge-boundary.md` for the current centralized architecture boundary.

Notable user-facing changes:

- `IDataVaultPitMaintenanceService` remains the explicit maintenance boundary for PIT tables. Full rebuilds and bounded parent maintenance are caller-invoked after ingestion.
- PIT reads consume maintained PIT rows through `DataVaultPitAsOfReadRequest`, `ReadPitRowsAsync(...)`, and `ReadPitAsync(...)`; reads do not refresh PIT rows implicitly.
- Hub-parent ordinary PITs, hub-parent shared-driving-key multi-active PITs, and runtime link-parent non-multi-active PITs are documented as the bounded supported shapes.
- Registry-backed PIT coverage is limited to `DataVaultRegistryPitRebuildRequest` and `DataVaultRegistryPitParentMaintenanceRequest`; there is no documented registry-backed PIT as-of read request.
- `IDataVaultBridgeMaintenanceService` remains the explicit maintenance boundary for bridge tables. `MaintainBridgeAsync(...)` inserts missing rows and can lower hierarchy depths, while `RebuildBridgeAsync(...)` is the shrink-safe path for row removal or increased `TraversalDepth`.
- At the v0.21.0 release boundary, `AddDVaultSqlite()` was the repository-proven optimized PIT/bridge read provider path. The current v0.34.0 baseline documents diagnostics-gated PIT/bridge read strategy candidates for SQLite, PostgreSQL, SQL Server, MySQL, and Oracle, while DB2, unsupported providers, and unsupported request shapes fall back to provider-neutral read pipelines.
- `IDataVaultReadDiagnosticsService` provides read strategy and read-shape evidence for latest/current/as-of satellite, PIT as-of, and bridge read requests without exposing raw hash keys, request values, SQL text, or query plans; the current bounded payload is formalized by the [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md).
- The benchmark evidence keeps fallback and SQLite-optimized `pit-as-of-read` and `bridge-traversal-read` rows visible in the root [benchmark-summary.md](benchmark-summary.md), [benchmark-summary.csv](benchmark-summary.csv), and [benchmark-summary.json](benchmark-summary.json) triplet plus the [PIT/bridge diagnostics benchmark bundle](artifacts/benchmarks/06F5Q91DR1555RSBQT7KDST684-pit-bridge-diagnostics/benchmark-summary.md).
- `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, staged PostgreSQL/MySQL ordered-bulk guidance, SQL Server native-bulk wording, Oracle direct optimization, EF compiled-model/query/pooling guidance, `DMV1910`/`DMV1911`, `UseDataVaultSaveChangesGuardInterceptor(...)`, aggregate preflight, provider explainability, telemetry, support-bundle export, model-first governance, and typed current/as-of reads from earlier releases remain carried forward into the current public baseline.

Primary v0.21.0 validation surfaces are [DataVaultPitReadServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitReadServiceSqliteTests.cs), [DataVaultPitMaintenanceServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultPitMaintenanceServiceSqliteTests.cs), [DataVaultBridgeReadServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeReadServiceSqliteTests.cs), [DataVaultBridgeMaintenanceServiceSqliteTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultBridgeMaintenanceServiceSqliteTests.cs), and [DataVaultDiagnosticsIntegrationTests.cs](tests/DCoding.Data.DVault.Tests/Integration/DataVaultDiagnosticsIntegrationTests.cs).

## v0.20.0 Historical Release Notes

The v0.20.0 release recorded the staged provider bulk ingestion documentation rollout as the previous coordinated seven-package baseline while preserving the EF safety, aggregate preflight, explicit service, opt-in telemetry, redacted support-bundle, consumer-owned design-time boundaries, and performance-evidence posture from earlier releases. See `docs/releases/v0.20.0.md` for the historical release-note record, package scope, provider-specific optimized write-path boundary, staged PostgreSQL/MySQL guidance, SQL Server and Oracle limits, stored-procedure boundary, validation evidence, benchmark artifact links, and package verification posture.

Notable user-facing changes:

- `IDataVaultSaveService` remains the public write boundary for single, ordered bulk, and bounded chunked saves.
- PostgreSQL staged COPY is the preferred optimized path for larger eligible materialized ordered batches, while the retained direct or UNNEST path remains documented below the staged threshold.
- MySQL staged bulk is the preferred optimized path for larger eligible materialized ordered batches, while the retained multi-row path remains documented above the native gate and below the staged threshold.
- SQL Server stays documented as its current native-bulk optimized boundary, and Oracle stays on retained direct optimized batching with `stagedOracleBulk=not-selected-no-measured-win` until measured evidence selects a staged Oracle path.
- Staged-provider diagnostics report bounded lifecycle, caveat, operation-count, and fallback-cause evidence without exposing raw SQL, hash keys, payload values, credentials, or transient stage-row contents.
- Stored procedures remain non-default escape-hatch guidance only. DVault does not auto-generate, auto-manage, or select stored procedures as a standard runtime path.
- The benchmark evidence keeps provider, strategy, execution-detail, skip, and run-context information together through the root benchmark summary triplet and the shared benchmark artifact contract.
- `DataVaultChunkedSaveRequest`, `DataVaultSaveChunk`, EF compiled-model/query/pooling guidance, `DMV1910`/`DMV1911`, `UseDataVaultSaveChangesGuardInterceptor(...)`, aggregate preflight, provider explainability, telemetry, support-bundle export, model-first governance, PIT/bridge maintenance, and typed current/as-of reads from earlier releases remain part of the current public baseline.

## v0.19.0 Historical Release Notes

The v0.19.0 release recorded the streaming explicit-save documentation rollout as the previous coordinated seven-package baseline while preserving the EF safety, aggregate preflight, explicit service, opt-in telemetry, redacted support-bundle, consumer-owned design-time boundaries, and performance-evidence posture from earlier releases. See `docs/releases/v0.19.0.md` for the historical release-note record, package scope, streaming save contract links, compatibility notes, validation evidence, benchmark artifact links, and package verification posture.

Notable user-facing changes:

- `DataVaultChunkedSaveRequest` and `DataVaultSaveChunk` are documented as additive explicit save inputs for bounded ordered chunks. `DataVaultBulkSaveRequest` remains the compatibility baseline when callers already have a fully materialized ordered batch.
- Chunked saves use the same explicit load timestamp, record source, metadata resolution, hash-key, hash-diff, caller ordering, cancellation, and caller-owned transaction rules as the existing single-request and bulk save paths.
- Bounded chunked telemetry reports chunk count, processed chunk count, retained-state current and high-water counts, finite retained-state fallback causes, unsupported-shape classifications, and transaction guidance without high-cardinality raw values.
- Benchmark evidence now includes `customer-profile-streaming-save` rows in the root benchmark summary triplet comparing the materialized explicit bulk baseline with chunked-save bounded-10 and bounded-5 runs.
- EF compiled-model usage is documented as consumer-owned `UseModel(runtimeModel)` usage after DVault metadata has been projected into the design model; DVault does not ship a compiled-model generator or provider-specific compiled guarantee.
- EF compiled queries are documented for stable direct EF shared-type-table expressions with generated names, scalar parameters, `EF.Property<T>(...)`, and deterministic projection. Dynamic `IDataVaultReadService` requests remain the default path for runtime-built read shapes.
- `AddDbContextPool<TContext>` guidance is bounded to options-only contexts with one fixed metadata/model shape; tenant, schema, naming, provider, or profile discriminators that affect the model remain caller-owned EF model-cache-key responsibilities.
- Benchmark evidence now rolls up `compiled-model-startup`, `compiled-query-hub-read`, `dbcontext-pooling-dvault-operation`, provider-neutral read allocation rows, explicit-save change-tracker rows, and provider-optimization regression rows through the root benchmark summary triplet and checked-in benchmark bundles.
- Query-shape tuning guidance remains based on request-bound `IDataVaultReadDiagnosticsService` read-shape output and bounded benchmark evidence under the [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md). DVault does not promise raw SQL output, automatic index creation, or provider-specific physical-plan advice.
- `DCoding.Data.DVault.Analyzers` reports `DMV1910` for exposed generated shared-type `DbSet<Dictionary<string, object>>` members and `DMV1911` for direct mutating calls against generated shared-type sets.
- `UseDataVaultSaveChangesGuardInterceptor(...)` is an explicit runtime guard opt-in with blocking and warning modes. It is separate from `AddDVault()`, can coexist with `UseDataVaultSaveChangesMetadataInterceptor(...)`, and does not replace `IDataVaultSaveService`.
- `DataVaultModelDriftPreflightReporter.Compare(...)` compares authoritative DVault metadata, the configured runtime model, and a consumer-materialized snapshot model without treating EF `ModelSnapshot` as a DVault public contract.
- `DataVaultMigrationOperationDiagnostics.AnalyzeReport(...)` preserves per-operation `Safe`, `Risky`, and `Incompatible` migration guardrail outcomes with deterministic DVM findings and remediation text.
- `DataVaultPreflight.Run(...)` aggregates validation/provider explain, artifact drift, snapshot drift, migration guardrail, and representative request-diagnostics lanes when the consumer supplies those inputs.
- Provider explainability covers capability profiles, provider-behavior profiles, save strategy diagnostics, read strategy diagnostics, and request-bound read-shape facts as deterministic redacted explain output rather than raw SQL or provider-magic claims. The read-shape payload follows the [DVault V2 Redacted Read-Plan Explain Contract](docs/architecture/dvault-v2-redacted-read-plan-explain-contract.md).
- `AddDVaultTelemetry()`, `IDataVaultTelemetryObserver`, `support-bundle`, explicit bridge maintenance, explicit PIT maintenance, current/as-of satellite reads, provider-native bulk ingestion, Code-First same-hub roles, link-parent satellites, and model-first artifact governance from earlier releases remain part of the current public baseline.

## v0.20.0 Historical Provider-Optimized Write Boundary

The [v0.20.0 notes](docs/releases/v0.20.0.md) document a provider-specific optimized write-path boundary without changing the public write API. v0.19.0 remains the historical baseline for provider-neutral chunked explicit saves and kept staged provider bulk ingestion outside that release's claim set. v0.20.0 moved the write documentation boundary forward only where repository evidence already exposed a supported or measured provider path; v0.21.0 carried that write guidance forward as the PIT/bridge read-model documentation boundary preserved by the current v0.34.0 baseline.

Use this carried-forward hierarchy when planning provider-optimized write adoption:

- `IDataVaultSaveService` remains the public write boundary for single, bulk, and chunked explicit saves.
- `DataVaultBulkSaveRequest` remains the compatibility baseline when the loader already has a complete ordered request set materialized.
- `DataVaultChunkedSaveRequest` remains the provider-neutral materialized bounded streaming path. `IAsyncEnumerable<DataVaultSaveChunk>` is the async source shape over the same bounded chunks. Neither shape is a blanket provider-native chunk execution promise.
- PostgreSQL staged COPY is the preferred optimized path for larger eligible staged-provider batches, with the retained direct or UNNEST path below the 60-operation staged boundary.
- MySQL staged bulk is the preferred optimized path for larger eligible staged-provider batches, with the retained multi-row path above the 50-operation native gate and below the 60-operation staged boundary.
- SQL Server stays on current native-bulk wording for its provider boundary instead of inventing an unsupported staged/direct split.
- Oracle stays on retained direct optimized batching with `stagedOracleBulk=not-selected-no-measured-win` until benchmark evidence proves a staged Oracle win.
- Stored procedures remain non-default escape-hatch guidance only. DVault does not auto-generate, auto-manage, or select stored procedures as a standard runtime path.

The benchmark-facing evidence continues to use the root `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json` triplet plus the shared [Performance Evidence And Benchmark Artifact Contract](docs/plans/performance-evidence-benchmark-artifact-contract.md). Provider-specific timing claims must preserve provider, strategy, execution-detail, skip, and run-context information instead of introducing new ad hoc evidence files.

## Current v0.34.0 Limitations

Lifecycle guardrails remain explicit library APIs hosted by the consumer application. The `DMV1912` through `DMV1914` lifecycle diagnostics are analyzer-only source-visible guardrails; they do not add a runtime guard, runtime behavior change, compiled-model generator, provider-specific lifecycle guarantee, cross-assembly inference, or whole-application inference. DVault does not ship a standalone CLI, does not ship a first-party `dotnet ef` command shim, does not intercept EF migration commands, does not automatically execute migrations, and does not apply schema repairs. Startup-project and target-project splits for design-time discovery remain outside the v0.34.0 boundary. Live-schema reading is built in for SQLite, PostgreSQL, SQL Server, Oracle, and MySQL, but non-SQLite checks still require consumer-managed databases and should remain opt-in operational evidence rather than default local validation. DB2 live-schema reading is explicitly unsupported for `IBM.EntityFrameworkCore` until a DB2 catalog reader exists.

Provider schema guardrails are review and diagnostics guidance for DVault-owned generated metadata. They do not add provider support beyond SQLite, Oracle, PostgreSQL, SQL Server, DB2, and MySQL; they do not give unrecognized providers provider-specific DDL safety guarantees; and they do not rewrite arbitrary consumer migrations, raw SQL, or third-party DDL.

Compiled-model, compiled-query, and `DbContext` pooling evidence is SQLite timing and allocation evidence for bounded EF shapes. It does not assert provider-specific SQL shape, index usage, generated compiled-model code ownership, dynamic request-built read compilation, provider-specific lifecycle behavior, or pooling for caller-owned variable model shapes.

The SaveChanges guard is opt-in and generated-row focused. `AddDVault()` does not enable it automatically, warning mode is caller-observed, and blocking mode protects only the unsafe generated-table changes it can identify at `SaveChanges` time. It does not compute hash keys, compute hash diffs, create generated rows, replace `IDataVaultSaveService`, or make ordinary EF entity tracking the default DVault persistence path.

Aggregate preflight is an explicit facade over caller-owned inputs. DVault does not discover EF snapshots, scan repositories for migrations or reviewed artifacts, generate representative save or read requests, open live databases by default, publish dashboards, or route support-bundle artifacts. Consumers must supply snapshot models, reviewed imports, idempotency live-schema read results, migration operations, and representative diagnostics when they want those lanes evaluated.

Telemetry remains explicit opt-in application wiring. DVault does not configure metric listeners, exporters, dashboards, alert rules, or backend-specific observability pipelines, and it does not emit high-cardinality raw values such as exception messages, hash keys, record sources, metadata names, table names, generated SQL, or full diagnostics text as metric tags.

Activity tracing remains listener-driven application wiring. DVault does not configure OpenTelemetry packages, exporters, collectors, dashboards, alert rules, hosting, sampling policy, baggage, custom trace identifiers, or custom correlation storage. Activity data is bounded operational shape and outcome evidence only; it is not a transport for raw keys, payload values, record sources, SQL text, credentials, connection strings, provider messages, exception messages, stack traces, support bundles, or diagnostic text.

Support bundles are consumer-invoked diagnostic artifacts. DVault does not upload, attach, archive, retain, or route support-bundle JSON, and it does not open a live database connection unless the consumer explicitly invokes the live-schema option in an environment they manage.

Typed read-model generation is opt-in and support-bundle-driven for bounded satellite, PIT, and bridge helper shapes. The generator does not parse raw `dvault.model.v1` additional files, infer helpers from source-visible declarations, generate provider-specific SQL, compile dynamic `IDataVaultReadService` requests, discover support bundles automatically, emit unbounded traversal helpers, maintain PIT or bridge rows, or publish dedicated generator approval snapshots.

Chunked and async source explicit saves are provider-neutral public behavior over the same `IDataVaultSaveService` boundary. Provider-native async writes, provider-native chunk execution, file ingestion, background workers, schedulers, CDC ingestion, and implicit `SaveChanges` streaming remain outside the public claim set.

Provider-native bulk dispatch is an optimization, not a separate persistence contract. Dirty tracked contexts, multi-active satellite batches, provider-name mismatches, below-threshold PostgreSQL or MySQL staged-bulk batches, SQL Server or Oracle batches outside their native gates, MySQL batches below the 50-operation native gate, SQL Server batches with more than `500` satellite operations, and Oracle batches with more than `10000` satellite operations fall back to a smaller provider-native path or the provider-neutral writer as appropriate. PostgreSQL uses the existing set-based direct or UNNEST path below the staged threshold and uses transient staging plus PostgreSQL COPY for larger eligible ordered batches. MySQL deliberately routes tiny satellite-history batches through provider-neutral fallback while keeping staged bulk for larger eligible ordered batches. DB2 currently has no provider-native bulk dispatch and uses provider-neutral fallback. DVault does not provision Docker containers, databases, users, schemas, credentials, or external-provider CI infrastructure; the v0.32 checked-in benchmark bundles are local Podman evidence with preserved run context, not a promise that consumer environments match those timings. The v0.34.0 compatibility baseline does not add provider provisioning or make live external-provider tests mandatory in the default local path.

The provider-specific SQL artifact lane is review-only design-time output. The `sql-artifact` verb can emit a SQL Server `dvault.sql-artifact.v1` dry-run manifest for the documented `provider-native-bulk-ingestion` workload, but it does not emit deployable SQL payload files, auto-generate stored procedures for every provider, auto-discover artifacts at runtime, auto-deploy, auto-invoke, auto-clean up, or synchronize artifacts with EF migrations, live schema, metadata changes, model-first artifacts, support bundles, or benchmark outputs.

Oracle provider-native bulk dispatch currently retains the direct Oracle path for eligible ordered batches. That path uses Oracle array binding when available and bounded direct insert batching otherwise. The staged Oracle branch is not selected without evidence that it beats the direct Oracle path and preserves deterministic stage cleanup within the caller-owned transaction boundary.

SQL capture is required only when a claim depends on emitted SQL shape, index usage, batching behavior, or materialization behavior. Timing and allocation claims for the compiled-model, compiled-query, and pooled-context rows do not require companion SQL captures.

Model-first APIs continue to operate on JSON artifacts, fluent Code-First declaration callbacks, and already-materialized metadata through `DataVaultModelArtifactImporter.ImportJson`, `DataVaultModelArtifactExporter.ExportJson`, `UseDataVaultMetadata(DataVaultModelImportResult)`, and `DataVaultModelDriftReporter.Compare`. PIT and bridge maintenance are explicit caller-invoked service boundaries through `IDataVaultPitMaintenanceService` and `IDataVaultBridgeMaintenanceService`; they are not automatic, scheduler-driven, trigger-driven, read-time maintenance, or provider-specific maintenance. PIT maintenance supports explicit `DataVaultPitMetadata` requests and registry-backed exact logical-name or exact CLR-mapping resolution for hub-parent ordinary PITs, hub-parent multi-active PITs with one shared canonical driving-key family, and link-parent non-multi-active runtime PITs; it does not add model-first link-parent PIT artifacts, incompatible driving-key-family PITs, cross-product tuple semantics, tuple-filter request parameters, registry-backed PIT as-of reads, or provider-specific PIT maintenance strategies. Bridge maintenance is not delete-aware and does not add broader graph traversal APIs, effectivity windows, path payload columns, or closure-state columns. Use full bridge rebuild when hierarchy deletions or topology shrinkage would require row removal or increased `TraversalDepth`. SQLite, PostgreSQL, SQL Server, MySQL, and Oracle are the repository-proven diagnostics-gated PIT/bridge read provider candidate paths. DB2 PIT and bridge reads use the provider-neutral fallback path. SQLite is the only optimized latest-satellite read provider path. Unsupported providers, non-SQLite latest-satellite requests, unsupported shapes, incomplete generated read-model projection evidence, and stale PIT/bridge maintenance evidence fall back to provider-neutral read pipelines. Keep the detailed PIT/bridge boundary in [DVault V1 PIT And Bridge Boundary](docs/architecture/dvault-v1-pit-bridge-boundary.md). The metadata interceptor is opt-in and metadata-only: callers still own generated row creation, hash-key and hash-diff values, save ordering, and explicit service-based persistence when they use `IDataVaultSaveService`.

Dependent child key modeling is not part of the current public claim set. Repeated same-hub runtime and metadata support does not imply typed link-mapper or source-generator parity for repeated same-hub mappings; generated and manual typed link mappers continue to use the existing unique-participant mapping boundary. Effectivity remains a generic link-parent satellite pattern rather than a first-class effectivity entity family, fluent builder, metadata kind, or technical column set. The analyzer package is not a complete model validator; it covers the documented Code-First selector diagnostics, duplicate-member diagnostics, bounded code fixes, compile-time mapping declaration diagnostics, and the high-confidence EF Core misuse diagnostics for generated shared-type tables only.

## Layout

- `DVault.slnx`: Canonical root solution file for build and test automation.
- `src/DCoding.Data/`: Non-packable build anchor for the `DCoding.Data` source-root namespace family.
- `src/DCoding.Data.DVault/`: Main library project. The NuGet package id and root namespace are `DCoding.Data.DVault`.
- `src/DCoding.Data.DVault.*`: Provider extension packages for DB2, SQLite, PostgreSQL, SQL Server, Oracle, and MySQL.
- `src/DCoding.Data.DVault.Analyzers/`: Roslyn analyzer and source-generator package for DVault Code-First diagnostics, bounded code fixes, and compile-time mapping declarations.
- `tests/DCoding.Data.DVault.Tests/`: Unit, integration, and shared test projects for DVault.
- `examples/`: Runnable SQLite and PostgreSQL quickstart projects.
- `benchmarks/`: Local performance benchmark projects.
- `docs/`: Documentation and design notes.

All current .NET projects are included in `DVault.slnx`. Empty future-use folders contain `.gitkeep` files so the layout is present in clean checkouts.

## Local Validation

Run the repository validation lane from a .NET 10 SDK checkout. The helper projects stay `net10.0`, while the pack and package-verification steps prove the `net8.0` and `net10.0` package dependency groups.

```sh
dotnet build DVault.slnx --nologo
dotnet test DVault.slnx --nologo
bash tools/pack-release-packages.sh
bash tools/verify-packages.sh
bash tools/check-format.sh
```

The normal test run includes package-specific public API snapshot checks for `DCoding.Data.DVault` and the six provider packages. See `docs/quality/api-surface-snapshots.md` for the approved baseline location and the explicit update workflow for intentional API changes.

`bash tools/pack-release-packages.sh` creates the two coordinated package lines under `artifacts/packages/`: eight `8.34.0` packages with `net8.0` assets and EF Core 8 dependency groups, and eight `10.34.0` packages with `net10.0` assets and EF Core 10 dependency groups. `bash tools/verify-packages.sh` inspects those artifacts, expects exactly sixteen DVault `.nupkg` files plus fourteen matching symbol packages for the runtime/provider packages, checks README, XML documentation, analyzer assets, declared NuGet metadata, and confirms each provider package depends on the packed `DCoding.Data.DVault` version from the same package line. It also verifies line-specific nuspec dependency groups, expected EF Core, IBM Entity Framework Core, and `Microsoft.Extensions.DependencyInjection.Abstractions` lines, dual `8.34.0` / `10.34.0` README install guidance, and analyzer `PrivateAssets="all"` guidance. The verifier intentionally fails when stale, unexpected, mixed-line, multi-target-combined, or non-packable package artifacts remain in `artifacts/packages/`.

Provider integration tests use stable xUnit trait categories so required local coverage and opt-in external database coverage can be selected explicitly:

- `Category=ProviderIntegration.RequiredLocal`: required SQLite-backed integration coverage that does not need external services.
- `Category=ProviderSmoke.Default`: provider package registration and configuration-contract smoke coverage that runs in the default local path.
- `Category=ProviderIntegration.ExternalOptIn`: live external database integration coverage, currently Postgres, SQL Server, Oracle, MySQL, and DB2.

To make the default local provider boundary explicit in a focused run, exclude opt-in external database tests:

```sh
dotnet test DVault.slnx --nologo --filter "Category!=ProviderIntegration.ExternalOptIn"
```

## Benchmarks

Run the local SQLite scenario comparison benchmarks from the repository root:

```sh
dotnet run --project benchmarks/DCoding.Data.DVault.Benchmarks/DCoding.Data.DVault.Benchmarks.csproj --configuration Release -- --iterations 1 --warmup 0
```

The benchmark executable compares conventional EF and DVault flows for the shared customer profile history contract, a larger customer profile bulk-history contract, the reduced order-product fulfillment history contract, and the provider-native bulk-ingestion contract. It uses SQLite temporary files by default and does not require Postgres, SQL Server, Oracle, MySQL, DB2, Docker, `DVAULT_TEST_POSTGRES_CONNECTION_STRING`, `DVAULT_TEST_SQLSERVER_CONNECTION_STRING`, `DVAULT_TEST_ORACLE_CONNECTION_STRING`, `DVAULT_TEST_MYSQL_CONNECTION_STRING`, or `DVAULT_TEST_DB2_CONNECTION_STRING`. Increase `--iterations` and `--warmup` locally when collecting steadier timing numbers.

Pass `--output <directory>` to emit documentation-ready benchmark artifacts named `benchmark-summary.md`, `benchmark-summary.csv`, and `benchmark-summary.json`. These artifacts keep scenario, provider, baseline, strategy family, dataset size, change ratio, execution status, skip reason, iteration count, mean/min/max milliseconds, mean/min/max allocated bytes, persisted outcome, benchmark options, optional provider status, and machine/runtime context together. The repository-facing evidence contract in `docs/plans/performance-evidence-benchmark-artifact-contract.md` defines the before/after storage rule, SQL-capture rule, provider matrix, and regression budgets. Optional PostgreSQL, SQL Server, MySQL, and Oracle provider-native bulk rows stay visible as skipped rows with `executionStatus` and `skipReason` when the provider is not configured or unavailable.

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

DVault does not provision Docker containers or databases for these tests. The configured database must already exist, and the configured user must be allowed to create and drop temporary schemas and temporary staging tables. The live Postgres lane includes schema drift checks, representative single saves, and an eligible ordered bulk hub, link, and satellite save through the staged COPY-backed provider strategy. Keep credentials in local environment variables or another untracked secret store, not in repository files.

## Optional Local DB2 Integration Tests

DB2 integration tests are opt-in and are skipped by default. Normal `dotnet test` execution does not require DB2, Docker, Podman, or checked-in machine-specific configuration.

To run the DB2-backed integration tests, provide a developer-managed DB2 database connection string in `DVAULT_TEST_DB2_CONNECTION_STRING`:

```sh
DVAULT_TEST_DB2_CONNECTION_STRING='Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret' dotnet test DVault.slnx --nologo -p:DVAULT_TEST_DB2_CONNECTION_STRING=Configured
```

To select only the live DB2 integration category, use the same configured connection string with the provider category filter:

```sh
DVAULT_TEST_DB2_CONNECTION_STRING='Server=localhost:50000;Database=dvault;UID=dvault;PWD=local-secret' dotnet test DVault.slnx --nologo --filter "Category=ProviderIntegration.ExternalOptIn&Provider=DB2" -p:DVAULT_TEST_DB2_CONNECTION_STRING=Configured
```

The integration project conditionally restores `IBM.EntityFrameworkCore` and the `DCoding.Data.DVault.Db2` project reference only when the DB2 opt-in property is non-empty. When running the live DB2 path, keep the environment variable set for test execution and pass the non-secret MSBuild marker property shown above so the conditional provider package and project reference are available during restore and build.

DVault does not provision DB2 containers, databases, users, schemas, credentials, or Podman/Docker fixtures for these tests. If a local container is used, start and configure that container outside the repository and provide only the resulting connection string through the environment variable. The configured DB2 database must already exist, and the configured user must be allowed to create and drop the temporary smoke-test tables. The live DB2 lane includes representative hub, link, and satellite writes plus latest/as-of satellite, PIT, and bridge reads through provider-neutral fallback. Keep credentials in local environment variables or another untracked secret store, not in repository files.

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

DVault does not provision Docker containers, Oracle databases, or Oracle users for these tests. The configured database and user must already exist, and the configured user must be allowed to create and drop temporary tables if future staged Oracle proof is enabled. The current live Oracle lane includes the existing smoke coverage plus an eligible ordered bulk hub, link, and satellite batch through the retained direct Oracle provider strategy. Keep credentials in local environment variables or another untracked secret store, not in repository files.

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
