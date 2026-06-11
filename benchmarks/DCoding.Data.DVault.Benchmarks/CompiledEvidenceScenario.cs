using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal static class CompiledEvidenceScenario {
  public static readonly DataVaultHubMetadata OrderHub = new("Order", ["Order Id"]);

  private static readonly DataVaultSatelliteMetadata OrderFulfillmentSatellite = new(
      "Fulfillment",
      OrderHub.ToReference(),
      ["Status Code"]);
  private static readonly DataVaultMetadataModel OrderMetadataModel = new(
      [OrderHub],
      [],
      [OrderFulfillmentSatellite]);

  private static readonly Func<CompiledEvidenceContext, string, CompiledHubOrderRead> ReadHubOrderByHashKey =
      EF.CompileQuery((CompiledEvidenceContext context, string orderHashKey) =>
          context.Set<Dictionary<string, object>>("HubOrder")
              .AsNoTracking()
              .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
              .Select(row => new CompiledHubOrderRead(
                  EF.Property<string>(row, "OrderHashKey"),
                  EF.Property<string>(row, "OrderId"),
                  EF.Property<string>(row, "RecordSource")))
              .Single());

  public static DataVaultMetadataModel CreateOrderMetadataModel() {
    return OrderMetadataModel;
  }

  public static DbContextOptions<CompiledEvidenceContext> CreateOptions(string connectionString) {
    return new DbContextOptionsBuilder<CompiledEvidenceContext>()
        .UseSqlite(connectionString)
        .ReplaceService<IModelCacheKeyFactory, CompiledEvidenceModelCacheKeyFactory>()
        .Options;
  }

  public static DbContextOptions<CompiledEvidenceContext> CreateRuntimeModelOptions(
      string connectionString,
      IModel runtimeModel) {
    return new DbContextOptionsBuilder<CompiledEvidenceContext>()
        .UseSqlite(connectionString)
        .UseModel(runtimeModel)
        .Options;
  }

  public static IModel CreateRuntimeModel(DbContext context) {
    var designModel = context.GetService<IDesignTimeModel>().Model;

    return context.GetService<IModelRuntimeInitializer>()
        .Initialize(designModel, designTime: false, validationLogger: null);
  }

  public static async Task<string> SeedOrderHubAsync(
      CompiledEvidenceContext context,
      string orderBusinessKey,
      string recordSource,
      CancellationToken cancellationToken) {
    var services = new ServiceCollection();
    services.AddDVaultSqlite();
    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var saveResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 13, 12, 0, 0, TimeSpan.Zero),
            recordSource,
            [new(OrderHub, [new("Order Id", orderBusinessKey)])],
            []),
        cancellationToken).ConfigureAwait(false);

    return saveResult.SavedRecords
        .Single(record => record.Kind == DataVaultTableKind.Hub && record.MetadataName == "Order")
        .HashKey;
  }

  public static CompiledHubOrderRead ReadHubOrderCompiled(
      CompiledEvidenceContext context,
      string orderHashKey) {
    return ReadHubOrderByHashKey(context, orderHashKey);
  }

  public static CompiledHubOrderRead ReadHubOrder(
      DbContext context,
      string orderHashKey) {
    return context.Set<Dictionary<string, object>>("HubOrder")
        .AsNoTracking()
        .Where(row => EF.Property<string>(row, "OrderHashKey") == orderHashKey)
        .Select(row => new CompiledHubOrderRead(
            EF.Property<string>(row, "OrderHashKey"),
            EF.Property<string>(row, "OrderId"),
            EF.Property<string>(row, "RecordSource")))
        .Single();
  }

  public static void AssertHubOrder(
      CompiledHubOrderRead? row,
      string orderHashKey,
      string orderBusinessKey,
      string recordSource) {
    ArgumentNullException.ThrowIfNull(row);

    BenchmarkAssert.Equal(orderHashKey, row.OrderHashKey, "The compiled evidence order hash key drifted.");
    BenchmarkAssert.Equal(orderBusinessKey, row.OrderId, "The compiled evidence order business key drifted.");
    BenchmarkAssert.Equal(recordSource, row.RecordSource, "The compiled evidence record source drifted.");
  }
}
