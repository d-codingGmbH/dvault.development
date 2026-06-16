using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class OrderProductDataVaultBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;

  public OrderProductDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public OrderProductDataVaultBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _hashKeyVariant = hashKeyVariant;
  }

  public string ScenarioName => "order-product-fulfillment-history";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize => "1 order-product relationship, 2 fulfillment states";

  public string ChangeRatio => "50% repeat-change history";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<OrderProductDataVaultContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy, _hashKeyVariant);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    try {
      await using (var context = new OrderProductDataVaultContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      }

      string orderProductHashKey = string.Empty;
      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        var relationship = ScenarioContracts.OrderRelationship;
        string orderHashKey;
        string productHashKey;

        await using (var context = new OrderProductDataVaultContext(options, providerCapabilities)) {
          var hubResult = await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  relationship.CreatedAtUtc,
                  relationship.RecordSource,
                  [
                      new(ScenarioContracts.OrderHub, [new("Order Id", relationship.OrderBusinessKey)]),
                      new(ScenarioContracts.ProductHub, [new("Sku", relationship.ProductBusinessKey)]),
                  ],
                  []),
              cancellationToken).ConfigureAwait(false);
          orderHashKey = DataVaultBenchmarkHelpers.GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");
          productHashKey = DataVaultBenchmarkHelpers.GetHashKey(hubResult, DataVaultTableKind.Hub, "Product");

          var linkResult = await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  relationship.CreatedAtUtc,
                  relationship.RecordSource,
                  [],
                  [
                      new(
                          ScenarioContracts.OrderProductLink,
                          [new("Order", orderHashKey), new("Product", productHashKey)]),
                  ]),
              cancellationToken).ConfigureAwait(false);
          orderProductHashKey = DataVaultBenchmarkHelpers.GetHashKey(linkResult, DataVaultTableKind.Link, "OrderProduct");
        }

        foreach (var fulfillmentEvent in ScenarioContracts.MeasuredOrderFulfillmentEvents) {
          await using var context = new OrderProductDataVaultContext(options, providerCapabilities);
          await saveService.SaveAsync(
              context,
              new DataVaultSaveRequest(
                  fulfillmentEvent.ChangedAtUtc,
                  fulfillmentEvent.RecordSource,
                  [],
                  [],
                  [CreateSatelliteSaveOperation(fulfillmentEvent, orderProductHashKey)]),
              cancellationToken).ConfigureAwait(false);
        }
      }).ConfigureAwait(false);

      await using (var context = new OrderProductDataVaultContext(options, providerCapabilities)) {
        var replayResult = await saveService.SaveAsync(
            context,
            new DataVaultSaveRequest(
                ScenarioContracts.UnchangedOrderFulfillmentReplay.ChangedAtUtc,
                ScenarioContracts.UnchangedOrderFulfillmentReplay.RecordSource,
                [],
                [],
                [CreateSatelliteSaveOperation(ScenarioContracts.UnchangedOrderFulfillmentReplay, orderProductHashKey)]),
            cancellationToken).ConfigureAwait(false);
        BenchmarkAssert.Equal(0, replayResult.RowsWritten, "The DVault replay operation must not persist a third satellite row.");
      }

      await VerifyOutcomeAsync(options, providerCapabilities, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          "1 order hub, 1 product hub, 1 link, and 2 fulfillment satellite rows for O-1000/SKU-COFFEE");
    }
    finally {
      await using var cleanupContext = new OrderProductDataVaultContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static DataVaultSatelliteSaveOperation CreateSatelliteSaveOperation(
      OrderFulfillmentEvent fulfillmentEvent,
      string orderProductHashKey) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.OrderFulfillmentSatellite,
        orderProductHashKey,
        [
            new("Allocation Status", fulfillmentEvent.AllocationStatus),
            new("Warehouse Code", fulfillmentEvent.WarehouseCode),
        ],
        fulfillmentEvent.HashDiff);
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<OrderProductDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new OrderProductDataVaultContext(options, providerCapabilities);
    var orderRows = await context.Set<Dictionary<string, object>>("HubOrder")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var productRows = await context.Set<Dictionary<string, object>>("HubProduct")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var linkRows = await context.Set<Dictionary<string, object>>("LinkOrderProduct")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
    var fulfillmentRows = (await context.Set<Dictionary<string, object>>("SatOrderProductFulfillment")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false))
        .OrderBy(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .ToArray();
    var orderRow = BenchmarkAssert.Single(orderRows, "The DVault order benchmark must persist one order hub row.");
    var productRow = BenchmarkAssert.Single(productRows, "The DVault order benchmark must persist one product hub row.");
    var linkRow = BenchmarkAssert.Single(linkRows, "The DVault order benchmark must persist one order-product link row.");
    var orderHashKey = (string)orderRow["OrderHashKey"];
    var productHashKey = (string)productRow["ProductHashKey"];
    var orderProductHashKey = (string)linkRow["OrderProductHashKey"];

    BenchmarkAssert.Equal(ScenarioContracts.OrderBusinessKey, (string)orderRow["OrderId"], "Order hub business key drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.ProductBusinessKey, (string)productRow["Sku"], "Product hub business key drifted.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        orderHashKey,
        providerCapabilities,
        "Order hub hash key must use the active stable-hash shape.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        productHashKey,
        providerCapabilities,
        "Product hub hash key must use the active stable-hash shape.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        orderProductHashKey,
        providerCapabilities,
        "Order-product link hash key must use the active stable-hash shape.");
    DataVaultBenchmarkHelpers.AssertHashKeyStorageMapping(
        context,
        "HubOrder",
        "OrderHashKey",
        providerCapabilities,
        "Order hub hash key must project the active storage profile.");
    DataVaultBenchmarkHelpers.AssertHashKeyStorageMapping(
        context,
        "LinkOrderProduct",
        "OrderProductHashKey",
        providerCapabilities,
        "Order-product link hash key must project the active storage profile.");
    BenchmarkAssert.Equal(orderHashKey, (string)linkRow["OrderHashKey"], "Order-product link order hash key drifted.");
    BenchmarkAssert.Equal(productHashKey, (string)linkRow["ProductHashKey"], "Order-product link product hash key drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.OrderRelationship.CreatedAtUtc, DataVaultBenchmarkHelpers.ReadLoadTimestamp(linkRow), "Order-product link load timestamp drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.OrderRelationship.RecordSource, (string)linkRow["RecordSource"], "Order-product link record source drifted.");
    BenchmarkAssert.Equal(ScenarioContracts.MeasuredOrderFulfillmentEvents.Length, fulfillmentRows.Length, "The DVault order benchmark must persist exactly two fulfillment satellite rows.");

    for (var index = 0; index < ScenarioContracts.MeasuredOrderFulfillmentEvents.Length; index++) {
      AssertFulfillmentSatelliteRow(
          fulfillmentRows[index],
          orderProductHashKey,
          ScenarioContracts.MeasuredOrderFulfillmentEvents[index]);
    }
  }

  private static void AssertFulfillmentSatelliteRow(
      Dictionary<string, object> row,
      string orderProductHashKey,
      OrderFulfillmentEvent expected) {
    BenchmarkAssert.Equal(orderProductHashKey, (string)row["OrderProductHashKey"], "Fulfillment satellite parent hash key drifted.");
    BenchmarkAssert.Equal(expected.AllocationStatus, (string)row["AllocationStatus"], "Fulfillment satellite allocation status drifted.");
    BenchmarkAssert.Equal(expected.WarehouseCode, (string)row["WarehouseCode"], "Fulfillment satellite warehouse code drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, (string)row["HashDiff"], "Fulfillment satellite hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, DataVaultBenchmarkHelpers.ReadLoadTimestamp(row), "Fulfillment satellite load timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, (string)row["RecordSource"], "Fulfillment satellite record source drifted.");
  }

  private sealed class OrderProductDataVaultContext(
      DbContextOptions<OrderProductDataVaultContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateOrderProductDataVaultModel(), ProviderCapabilities);
    }
  }
}
