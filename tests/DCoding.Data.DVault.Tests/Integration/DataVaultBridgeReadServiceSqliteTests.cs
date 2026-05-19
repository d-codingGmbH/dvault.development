using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultBridgeReadServiceSqliteTests {
  [Fact]
  public async Task ManyToManyBridgeReadUsesEndpointFilterAndDeterministicOrderingThroughSqlite() {
    var bridge = ManyToManyMetadataModel.Bridges.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ManyToManyBridgeReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateProvider();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    using var fallbackProvider = CreateFallbackProvider();
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var fallbackReadDiagnostics = fallbackProvider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    await using var context = new ManyToManyBridgeReadContext(options);
    await context.Database.EnsureCreatedAsync();

    await SeedManyToManyBridgeRowAsync(context, "customer-1", "order-2");
    await SeedManyToManyBridgeRowAsync(context, "customer-2", "order-3");
    await SeedManyToManyBridgeRowAsync(context, "customer-1", "order-1");

    var request = new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.From,
        ["customer-1"]);
    var diagnostics = readDiagnostics.Analyze(context, request);
    var readRows = await readService.ReadBridgeRowsAsync(
        context,
        request);
    var projectedOrderKeys = await readService.ReadBridgeAsync(
        context,
        request,
        row => row.RequiredString("OrderHashKey"));
    var reverseRows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.To,
            ["order-3"]));
    var missingRows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["missing-customer"]));
    var fallbackDiagnostics = fallbackReadDiagnostics.Analyze(context, request);
    var fallbackRows = await fallbackReadService.ReadBridgeRowsAsync(context, request);

    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.ReadStrategy.Status);
    Assert.Equal("SqliteDataVaultReadStrategy", diagnostics.ReadStrategy.SelectedStrategyName);
    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, fallbackDiagnostics.ReadStrategy.Status);
    Assert.Contains(
        fallbackDiagnostics.ReadStrategy.FallbackCauses,
        cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered);

    Assert.Collection(
        readRows,
        row => AssertManyToManyRow(row, "customer-1", "order-1"),
        row => AssertManyToManyRow(row, "customer-1", "order-2"));
    Assert.Equal(readRows.Select(row => row.EndpointHashKeys[1].HashKey), fallbackRows.Select(row => row.EndpointHashKeys[1].HashKey));
    Assert.Equal(["order-1", "order-2"], projectedOrderKeys);
    Assert.Collection(reverseRows, row => AssertManyToManyRow(row, "customer-2", "order-3"));
    Assert.Empty(missingRows);
  }

  [Fact]
  public async Task BridgeReadReturnsEmptyRowsForEmptyBridgeTablesThroughSqlite() {
    var bridge = ManyToManyMetadataModel.Bridges.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<ManyToManyBridgeReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateProvider();
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    await using var context = new ManyToManyBridgeReadContext(options);
    await context.Database.EnsureCreatedAsync();

    var rows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-1"]));

    Assert.Empty(rows);
  }

  [Fact]
  public async Task HierarchyBridgeReadHonorsBoundedDepthAndDirectionThroughSqlite() {
    var bridge = HierarchyMetadataModel.Bridges.Single();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<HierarchyBridgeReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    using var provider = CreateProvider();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    await using var context = new HierarchyBridgeReadContext(options);
    await context.Database.EnsureCreatedAsync();

    await SeedHierarchyBridgeRowAsync(context, "region-a", "region-d", 3);
    await SeedHierarchyBridgeRowAsync(context, "region-a", "region-c", 2);
    await SeedHierarchyBridgeRowAsync(context, "region-b", "region-c", 1);
    await SeedHierarchyBridgeRowAsync(context, "region-a", "region-b", 1);

    var ancestorRequest = new DataVaultBridgeReadRequest(
        bridge,
        DataVaultBridgeTraversalEndpoint.Ancestor,
        ["region-a"],
        maximumDepth: 2);
    var diagnostics = readDiagnostics.Analyze(context, ancestorRequest);
    var ancestorRows = await readService.ReadBridgeRowsAsync(
        context,
        ancestorRequest);
    var descendantRows = await readService.ReadBridgeRowsAsync(
        context,
        new DataVaultBridgeReadRequest(
            bridge,
            DataVaultBridgeTraversalEndpoint.Descendant,
            ["region-c"],
            maximumDepth: 1));
    var projectedDepths = await readService.ReadBridgeAsync(
        context,
        ancestorRequest,
        row => row.RequiredInt32("TraversalDepth"));

    Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.ReadStrategy.Status);
    Assert.Equal("SqliteDataVaultReadStrategy", diagnostics.ReadStrategy.SelectedStrategyName);

    Assert.Collection(
        ancestorRows,
        row => AssertHierarchyRow(row, "region-a", "region-b", 1),
        row => AssertHierarchyRow(row, "region-a", "region-c", 2));
    Assert.Collection(descendantRows, row => AssertHierarchyRow(row, "region-b", "region-c", 1));
    Assert.Equal([1, 2], projectedDepths);
  }

  [Fact]
  public async Task RegistryBackedBridgeReadFailsWhenBridgeMetadataIsMissing() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var metadataModel = new DataVaultMetadataModel([customer], [], []);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataModel(metadataModel));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryBridgeReadContext>(
        options => options
            .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
            .UseDataVaultMetadata());

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var readService = provider.GetRequiredService<IDataVaultReadService>();

    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryBridgeReadContext>();
    await context.Database.EnsureCreatedAsync();

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadBridgeRowsAsync(
            context,
            new DataVaultRegistryBridgeReadRequest(
                "MissingBridge",
                DataVaultBridgeTraversalEndpoint.From,
                ["hash-key"])));

    Assert.Contains("bridge metadata 'MissingBridge'", exception.Message, StringComparison.Ordinal);
  }

  private static ServiceProvider CreateProvider() {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static ServiceProvider CreateFallbackProvider() {
    var services = new ServiceCollection();
    services.AddDVault();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DataVaultMetadataModel ManyToManyMetadataModel { get; } = CreateManyToManyMetadataModel();

  private static DataVaultMetadataModel HierarchyMetadataModel { get; } = CreateHierarchyMetadataModel();

  private static Task SeedManyToManyBridgeRowAsync(
      DbContext context,
      string customerHashKey,
      string orderHashKey) {
    return context.Database.ExecuteSqlRawAsync(
        "INSERT INTO \"BridgeCustomerOrder\" (\"CustomerHashKey\", \"OrderHashKey\") VALUES ({0}, {1});",
        customerHashKey,
        orderHashKey);
  }

  private static Task SeedHierarchyBridgeRowAsync(
      DbContext context,
      string ancestorHashKey,
      string descendantHashKey,
      int traversalDepth) {
    return context.Database.ExecuteSqlRawAsync(
        "INSERT INTO \"BridgeSalesRegionHierarchy\" " +
        "(\"AncestorSalesRegionHashKey\", \"DescendantSalesRegionHashKey\", \"TraversalDepth\") " +
        "VALUES ({0}, {1}, {2});",
        ancestorHashKey,
        descendantHashKey,
        traversalDepth);
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
    var bridge = new DataVaultBridgeMetadata(
        "SalesRegionHierarchy",
        DataVaultBridgeKind.Hierarchy,
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Ancestor,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ParentRegion"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.Descendant,
                DataVaultMetadataReference.Hub("SalesRegion"),
                "ChildRegion"),
        ]);

    return new DataVaultMetadataModel([], [], [], [bridge]);
  }

  private static void AssertManyToManyRow(
      DataVaultBridgeReadRecord row,
      string customerHashKey,
      string orderHashKey) {
    Assert.Equal("CustomerOrder", row.MetadataName);
    Assert.Equal("BridgeCustomerOrder", row.TableName);
    Assert.Null(row.TraversalDepth);
    Assert.Collection(
        row.EndpointHashKeys,
        endpoint => {
          Assert.Equal(DataVaultBridgeTraversalEndpoint.From, endpoint.Endpoint);
          Assert.Equal("Customer", endpoint.EndpointName);
          Assert.Equal("CustomerHashKey", endpoint.ColumnName);
          Assert.Equal(customerHashKey, endpoint.HashKey);
        },
        endpoint => {
          Assert.Equal(DataVaultBridgeTraversalEndpoint.To, endpoint.Endpoint);
          Assert.Equal("Order", endpoint.EndpointName);
          Assert.Equal("OrderHashKey", endpoint.ColumnName);
          Assert.Equal(orderHashKey, endpoint.HashKey);
        });
  }

  private static void AssertHierarchyRow(
      DataVaultBridgeReadRecord row,
      string ancestorHashKey,
      string descendantHashKey,
      int traversalDepth) {
    Assert.Equal("SalesRegionHierarchy", row.MetadataName);
    Assert.Equal("BridgeSalesRegionHierarchy", row.TableName);
    Assert.Equal(traversalDepth, row.TraversalDepth);
    Assert.Collection(
        row.EndpointHashKeys,
        endpoint => {
          Assert.Equal(DataVaultBridgeTraversalEndpoint.Ancestor, endpoint.Endpoint);
          Assert.Equal("ParentRegion", endpoint.EndpointName);
          Assert.Equal("AncestorSalesRegionHashKey", endpoint.ColumnName);
          Assert.Equal(ancestorHashKey, endpoint.HashKey);
        },
        endpoint => {
          Assert.Equal(DataVaultBridgeTraversalEndpoint.Descendant, endpoint.Endpoint);
          Assert.Equal("ChildRegion", endpoint.EndpointName);
          Assert.Equal("DescendantSalesRegionHashKey", endpoint.ColumnName);
          Assert.Equal(descendantHashKey, endpoint.HashKey);
        });
  }

  private sealed class ManyToManyBridgeReadContext(DbContextOptions<ManyToManyBridgeReadContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ManyToManyMetadataModel);
    }
  }

  private sealed class HierarchyBridgeReadContext(DbContextOptions<HierarchyBridgeReadContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(HierarchyMetadataModel);
    }
  }

  private sealed class RegistryBridgeReadContext(DbContextOptions<RegistryBridgeReadContext> options) : DbContext(options) {
  }
}
