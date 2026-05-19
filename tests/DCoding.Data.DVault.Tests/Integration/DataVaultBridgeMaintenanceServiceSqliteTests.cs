using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultBridgeMaintenanceServiceSqliteTests {
  [Fact]
  public async Task ManyToManyBridgeRebuildAndIncrementalMaintenanceUsePersistedSourceLinksThroughSqlite() {
    var bridge = ManyToManyMetadataModel.Bridges.Single();
    var link = ManyToManyMetadataModel.Links.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ManyToManyBridgeMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultBridgeMaintenanceService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    await using var context = new ManyToManyBridgeMaintenanceContext(options);
    await context.Database.EnsureCreatedAsync();

    await SaveCustomerOrderLinkAsync(context, saveService, link, "customer-1", "order-2");
    await SaveCustomerOrderLinkAsync(context, saveService, link, "customer-1", "order-1");
    await SaveCustomerOrderLinkAsync(context, saveService, link, "customer-2", "order-3");

    var rebuildResult = await maintenanceService.RebuildBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var unchangedResult = await maintenanceService.MaintainBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));

    await SaveCustomerOrderLinkAsync(context, saveService, link, "customer-1", "order-3");
    var incrementalResult = await maintenanceService.MaintainBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var rows = await ReadManyToManyRowsAsync(readService, context, bridge);

    Assert.Equal("CustomerOrder", rebuildResult.MetadataName);
    Assert.Equal("BridgeCustomerOrder", rebuildResult.TableName);
    Assert.Equal(3, rebuildResult.RowsInserted);
    Assert.Equal(0, rebuildResult.RowsUpdated);
    Assert.Equal(0, rebuildResult.RowsDeleted);
    Assert.Equal(0, rebuildResult.RowsUnchanged);
    Assert.Equal(0, unchangedResult.RowsInserted);
    Assert.Equal(0, unchangedResult.RowsUpdated);
    Assert.Equal(3, unchangedResult.RowsUnchanged);
    Assert.Equal(1, incrementalResult.RowsInserted);
    Assert.Equal(0, incrementalResult.RowsUpdated);
    Assert.Equal(3, incrementalResult.RowsUnchanged);
    Assert.Equal(
        [
            "customer-1->order-1",
            "customer-1->order-2",
            "customer-1->order-3",
            "customer-2->order-3",
        ],
        rows);
  }

  [Fact]
  public async Task HierarchyBridgeMaintenanceKeepsShortestPositiveDepthThroughSqlite() {
    var bridge = HierarchyMetadataModel.Bridges.Single();
    var link = HierarchyMetadataModel.Links.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<HierarchyBridgeMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultBridgeMaintenanceService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    await using var context = new HierarchyBridgeMaintenanceContext(options);
    await context.Database.EnsureCreatedAsync();

    await SaveHierarchyLinkAsync(context, saveService, link, "region-a", "region-b");
    await SaveHierarchyLinkAsync(context, saveService, link, "region-b", "region-c");
    var rebuildResult = await maintenanceService.RebuildBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var initialRows = await ReadHierarchyRowsAsync(readService, context, bridge);

    await SaveHierarchyLinkAsync(context, saveService, link, "region-a", "region-d");
    await SaveHierarchyLinkAsync(context, saveService, link, "region-d", "region-c");
    var equalDepthResult = await maintenanceService.MaintainBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var equalDepthRows = await ReadHierarchyRowsAsync(readService, context, bridge);

    await SaveHierarchyLinkAsync(context, saveService, link, "region-a", "region-c");
    var shorterPathResult = await maintenanceService.MaintainBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var incrementalRows = await ReadHierarchyRowsAsync(readService, context, bridge);
    var convergenceResult = await maintenanceService.RebuildBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var rebuiltRows = await ReadHierarchyRowsAsync(readService, context, bridge);

    Assert.Equal(3, rebuildResult.RowsInserted);
    Assert.Equal(
        [
            "region-a->region-b:1",
            "region-a->region-c:2",
            "region-b->region-c:1",
        ],
        initialRows);
    Assert.Equal(2, equalDepthResult.RowsInserted);
    Assert.Equal(0, equalDepthResult.RowsUpdated);
    Assert.Contains("region-a->region-c:2", equalDepthRows);
    Assert.Equal(0, shorterPathResult.RowsInserted);
    Assert.Equal(1, shorterPathResult.RowsUpdated);
    Assert.Equal(
        [
            "region-a->region-b:1",
            "region-a->region-c:1",
            "region-a->region-d:1",
            "region-b->region-c:1",
            "region-d->region-c:1",
        ],
        incrementalRows);
    Assert.Equal(5, convergenceResult.RowsInserted);
    Assert.Equal(incrementalRows, rebuiltRows);
  }

  [Fact]
  public async Task HierarchyBridgeMaintenanceDoesNotMaterializeSelfRowsForCyclesThroughSqlite() {
    var bridge = HierarchyMetadataModel.Bridges.Single();
    var link = HierarchyMetadataModel.Links.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<HierarchyBridgeMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateProvider();
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultBridgeMaintenanceService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    await using var context = new HierarchyBridgeMaintenanceContext(options);
    await context.Database.EnsureCreatedAsync();

    await SaveHierarchyLinkAsync(context, saveService, link, "region-a", "region-b");
    await SaveHierarchyLinkAsync(context, saveService, link, "region-b", "region-a");
    await SaveHierarchyLinkAsync(context, saveService, link, "region-b", "region-c");

    var result = await maintenanceService.RebuildBridgeAsync(
        context,
        new DataVaultBridgeMaintenanceRequest(bridge));
    var rows = await ReadHierarchyRowsAsync(readService, context, bridge);

    Assert.Equal(4, result.RowsInserted);
    Assert.Equal(
        [
            "region-a->region-b:1",
            "region-a->region-c:2",
            "region-b->region-a:1",
            "region-b->region-c:1",
        ],
        rows);
  }

  [Fact]
  public async Task RegistryBackedBridgeMaintenanceResolvesBridgeNameThroughSqlite() {
    var bridge = ManyToManyMetadataModel.Bridges.Single();
    var link = ManyToManyMetadataModel.Links.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(ManyToManyMetadataModel));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryBridgeMaintenanceContext>(options => options
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryBridgeMaintenanceContext>();
    var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = scope.ServiceProvider.GetRequiredService<IDataVaultBridgeMaintenanceService>();
    var readService = scope.ServiceProvider.GetRequiredService<IDataVaultReadService>();
    await context.Database.EnsureCreatedAsync();

    await SaveCustomerOrderLinkAsync(context, saveService, link, "customer-9", "order-9");

    var result = await maintenanceService.MaintainBridgeAsync(
        context,
        new DataVaultRegistryBridgeMaintenanceRequest("CustomerOrder"));
    var rows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-9"]));

    Assert.Equal(1, result.RowsInserted);
    Assert.Collection(rows, row => AssertManyToManyRow(row, "customer-9", "order-9"));
  }

  [Fact]
  public async Task RegistryBackedBridgeMaintenanceFailsWhenBridgeMetadataIsMissing() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var metadataModel = new DataVaultMetadataModel([customer], [], []);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(metadataModel));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryBridgeMaintenanceContext>(options => options
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryBridgeMaintenanceContext>();
    var maintenanceService = scope.ServiceProvider.GetRequiredService<IDataVaultBridgeMaintenanceService>();
    await context.Database.EnsureCreatedAsync();

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        maintenanceService.MaintainBridgeAsync(
            context,
            new DataVaultRegistryBridgeMaintenanceRequest("MissingBridge")));

    Assert.Contains("bridge metadata 'MissingBridge'", exception.Message, StringComparison.Ordinal);
  }

  private static ServiceProvider CreateProvider() {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DataVaultMetadataModel ManyToManyMetadataModel { get; } = CreateManyToManyMetadataModel();

  private static DataVaultMetadataModel HierarchyMetadataModel { get; } = CreateHierarchyMetadataModel();

  private static Task<DataVaultSaveResult> SaveCustomerOrderLinkAsync(
      DbContext context,
      IDataVaultSaveService saveService,
      DataVaultLinkMetadata link,
      string customerHashKey,
      string orderHashKey) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 18, 12, 0, 0, TimeSpan.Zero),
            "bridge-maintenance-test",
            [],
            [new DataVaultLinkSaveOperation(
                link,
                [
                    new KeyValuePair<string, string>("Customer", customerHashKey),
                    new KeyValuePair<string, string>("Order", orderHashKey),
                ])]));
  }

  private static Task<DataVaultSaveResult> SaveHierarchyLinkAsync(
      DbContext context,
      IDataVaultSaveService saveService,
      DataVaultLinkMetadata link,
      string ancestorHashKey,
      string descendantHashKey) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 18, 12, 0, 0, TimeSpan.Zero),
            "bridge-maintenance-test",
            [],
            [new DataVaultLinkSaveOperation(
                link,
                [
                    new KeyValuePair<string, string>("ParentRegion", ancestorHashKey),
                    new KeyValuePair<string, string>("ChildRegion", descendantHashKey),
                ])]));
  }

  private static async Task<IReadOnlyList<string>> ReadManyToManyRowsAsync(
      IDataVaultReadService readService,
      DbContext context,
      DataVaultBridgeMetadata bridge) {
    var rows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-1", "customer-2"]));

    return rows.Select(row => row.EndpointHashKeys[0].HashKey + "->" + row.EndpointHashKeys[1].HashKey).ToArray();
  }

  private static async Task<IReadOnlyList<string>> ReadHierarchyRowsAsync(
      IDataVaultReadService readService,
      DbContext context,
      DataVaultBridgeMetadata bridge) {
    var rows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.Ancestor,
            ["region-a", "region-b", "region-c", "region-d"],
            maximumDepth: 10));

    return rows
        .Select(row => row.EndpointHashKeys[0].HashKey + "->" + row.EndpointHashKeys[1].HashKey + ":" + row.TraversalDepth)
        .ToArray();
  }

  private static DataVaultMetadataModel CreateManyToManyMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var bridge = DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        customer.ToReference(),
        customerOrder.ToReference(),
        order.ToReference());

    return new DataVaultMetadataModel([customer, order], [customerOrder], [], [bridge]);
  }

  private static DataVaultMetadataModel CreateHierarchyMetadataModel() {
    var salesRegion = new DataVaultHubMetadata("SalesRegion", ["Region Id"]);
    var parentChild = new DataVaultLinkMetadata(
        "SalesRegionParentChild",
        [
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ParentRegion"),
            new DataVaultLinkParticipantMetadata(salesRegion.ToReference(), "ChildRegion"),
        ]);
    var bridge = new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        parentChild.ToReference(),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                salesRegion.ToReference(),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Descendant,
                salesRegion.ToReference(),
                "ChildRegion"),
        ]);

    return new DataVaultMetadataModel([salesRegion], [parentChild], [], [bridge]);
  }

  private static void AssertManyToManyRow(
      DataVaultBridgeReadRecord row,
      string customerHashKey,
      string orderHashKey) {
    Assert.Equal("CustomerOrder", row.MetadataName);
    Assert.Equal("BridgeCustomerOrder", row.TableName);
    Assert.Null(row.TraversalDepth);
    Assert.Equal(customerHashKey, row.EndpointHashKeys[0].HashKey);
    Assert.Equal(orderHashKey, row.EndpointHashKeys[1].HashKey);
  }

  private sealed class ManyToManyBridgeMaintenanceContext(DbContextOptions<ManyToManyBridgeMaintenanceContext> options)
      : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ManyToManyMetadataModel);
    }
  }

  private sealed class HierarchyBridgeMaintenanceContext(DbContextOptions<HierarchyBridgeMaintenanceContext> options)
      : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(HierarchyMetadataModel);
    }
  }

  private sealed class RegistryBridgeMaintenanceContext(DbContextOptions<RegistryBridgeMaintenanceContext> options)
      : DbContext(options) {
  }
}
