using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class ProviderNativeBulkIngestionBenchmark : IScenarioBenchmark {
  private const int PairCount = 20;
  private const int ExpectedRowsWritten = PairCount * 3 + 2;
  private const int ExpectedSavedRecordCount = PairCount * 3 + 3;

  private static readonly DateTimeOffset HubLoadTimestamp = new(2026, 5, 18, 10, 0, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset LinkLoadTimestamp = new(2026, 5, 18, 10, 5, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset FirstSatelliteLoadTimestamp = new(2026, 5, 18, 10, 10, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset UnchangedSatelliteLoadTimestamp = new(2026, 5, 18, 10, 15, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset ChangedSatelliteLoadTimestamp = new(2026, 5, 18, 10, 20, 0, TimeSpan.Zero);

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;

  public ProviderNativeBulkIngestionBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(provider);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
  }

  public string ScenarioName => "provider-native-bulk-ingestion";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => "20 order-product pairs, 3 fulfillment satellite operations";

  public string ChangeRatio => "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<ProviderNativeBulkIngestionContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var scenario = CreateScenario(provider);

    try {
      await using (var context = new ProviderNativeBulkIngestionContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new ProviderNativeBulkIngestionContext(options, providerCapabilities);
        AssertStrategySelection(diagnostics.Analyze(context, scenario.Request));

        var result = await saveService.SaveAsync(context, scenario.Request, cancellationToken).ConfigureAwait(false);

        BenchmarkAssert.Equal(ExpectedRowsWritten, result.RowsWritten, "The provider-native bulk benchmark row count drifted.");
        BenchmarkAssert.Equal(ExpectedSavedRecordCount, result.SavedRecords.Count, "The provider-native bulk benchmark saved-record count drifted.");
        BenchmarkAssert.Equal(0, context.ChangeTracker.Entries().Count(), "The provider-native bulk benchmark must leave a clean change tracker.");
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          PairCount.ToString(CultureInfo.InvariantCulture) +
          " order hubs, " +
          PairCount.ToString(CultureInfo.InvariantCulture) +
          " product hubs, " +
          PairCount.ToString(CultureInfo.InvariantCulture) +
          " order-product links, and 2 fulfillment satellite rows");
    }
    finally {
      await using var cleanupContext = new ProviderNativeBulkIngestionContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private void AssertStrategySelection(DataVaultDiagnosticsResult diagnostics) {
    var expectedStrategyName = DataVaultBenchmarkHelpers.GetProviderSaveStrategyName(_strategy);
    if (expectedStrategyName is not null) {
      DataVaultBenchmarkHelpers.AssertProviderSaveStrategySelected(diagnostics, expectedStrategyName);
    }
  }

  private static ProviderNativeBulkIngestionScenario CreateScenario(IServiceProvider provider) {
    var orderIds = Enumerable.Range(0, PairCount)
        .Select(index => "O-NATIVE-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var productIds = Enumerable.Range(0, PairCount)
        .Select(index => "SKU-NATIVE-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var orderHashKeys = orderIds
        .Select(orderId => ComputeHash(provider, [new("Order Id", orderId)]))
        .ToArray();
    var productHashKeys = productIds
        .Select(productId => ComputeHash(provider, [new("Sku", productId)]))
        .ToArray();
    var linkHashKeys = Enumerable.Range(0, PairCount)
        .Select(index => ComputeHash(
            provider,
            [
                new("Order", orderHashKeys[index]),
                new("Product", productHashKeys[index]),
            ]))
        .ToArray();
    var hubRequest = new DataVaultSaveRequest(
        HubLoadTimestamp,
        "native-bulk-hubs",
        orderIds
            .Select(orderId => new DataVaultHubSaveOperation(ScenarioContracts.OrderHub, [new("Order Id", orderId)]))
            .Concat(productIds.Select(productId => new DataVaultHubSaveOperation(ScenarioContracts.ProductHub, [new("Sku", productId)])))
            .ToArray(),
        []);
    var linkRequest = new DataVaultSaveRequest(
        LinkLoadTimestamp,
        "native-bulk-links",
        [],
        Enumerable.Range(0, PairCount)
            .Select(index => new DataVaultLinkSaveOperation(
                ScenarioContracts.OrderProductLink,
                [
                    new("Order", orderHashKeys[index]),
                    new("Product", productHashKeys[index]),
                ]))
            .ToArray());
    var firstSatelliteRequest = new DataVaultSaveRequest(
        FirstSatelliteLoadTimestamp,
        "native-bulk-satellite-first",
        [],
        [],
        [
            CreateFulfillmentOperation(
                linkHashKeys[0],
                allocationStatus: "Backordered",
                warehouseCode: "NORTH-1",
                hashDiff: "fulfillment-hash-1"),
        ]);
    var unchangedSatelliteRequest = new DataVaultSaveRequest(
        UnchangedSatelliteLoadTimestamp,
        "native-bulk-satellite-replay",
        [],
        [],
        [
            CreateFulfillmentOperation(
                linkHashKeys[0],
                allocationStatus: "Backordered",
                warehouseCode: "NORTH-1",
                hashDiff: "fulfillment-hash-1"),
        ]);
    var changedSatelliteRequest = new DataVaultSaveRequest(
        ChangedSatelliteLoadTimestamp,
        "native-bulk-satellite-change",
        [],
        [],
        [
            CreateFulfillmentOperation(
                linkHashKeys[0],
                allocationStatus: "Allocated",
                warehouseCode: "NORTH-1",
                hashDiff: "fulfillment-hash-2"),
        ]);

    return new ProviderNativeBulkIngestionScenario(
        new DataVaultBulkSaveRequest(
            [
                hubRequest,
                linkRequest,
                firstSatelliteRequest,
                unchangedSatelliteRequest,
                changedSatelliteRequest,
            ]),
        orderIds,
        productIds,
        orderHashKeys,
        productHashKeys,
        linkHashKeys);
  }

  private static DataVaultSatelliteSaveOperation CreateFulfillmentOperation(
      string orderProductHashKey,
      string allocationStatus,
      string warehouseCode,
      string hashDiff) {
    return new DataVaultSatelliteSaveOperation(
        ScenarioContracts.OrderFulfillmentSatellite,
        orderProductHashKey,
        [
            new("Allocation Status", allocationStatus),
            new("Warehouse Code", warehouseCode),
        ],
        hashDiff);
  }

  private static async Task VerifyOutcomeAsync(
      DbContextOptions<ProviderNativeBulkIngestionContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      ProviderNativeBulkIngestionScenario scenario,
      CancellationToken cancellationToken) {
    await using var context = new ProviderNativeBulkIngestionContext(options, providerCapabilities);
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

    BenchmarkAssert.Equal(PairCount, orderRows.Count, "The provider-native bulk benchmark must persist every order hub row.");
    BenchmarkAssert.Equal(PairCount, productRows.Count, "The provider-native bulk benchmark must persist every product hub row.");
    BenchmarkAssert.Equal(PairCount, linkRows.Count, "The provider-native bulk benchmark must persist every order-product link row.");
    BenchmarkAssert.Equal(2, fulfillmentRows.Length, "The provider-native bulk benchmark must persist the changed fulfillment history only once.");

    var firstOrderRow = BenchmarkAssert.Single(
        orderRows.Where(row => string.Equals(ReadString(row, "OrderId"), scenario.OrderIds[0], StringComparison.Ordinal)),
        "The provider-native bulk benchmark must persist the first order hub row.");
    var firstProductRow = BenchmarkAssert.Single(
        productRows.Where(row => string.Equals(ReadString(row, "Sku"), scenario.ProductIds[0], StringComparison.Ordinal)),
        "The provider-native bulk benchmark must persist the first product hub row.");
    var firstLinkRow = BenchmarkAssert.Single(
        linkRows.Where(row => string.Equals(ReadString(row, "OrderProductHashKey"), scenario.LinkHashKeys[0], StringComparison.Ordinal)),
        "The provider-native bulk benchmark must persist the first order-product link row.");

    BenchmarkAssert.Equal(scenario.OrderHashKeys[0], ReadString(firstOrderRow, "OrderHashKey"), "Order hub hash key drifted.");
    BenchmarkAssert.Equal(scenario.ProductHashKeys[0], ReadString(firstProductRow, "ProductHashKey"), "Product hub hash key drifted.");
    BenchmarkAssert.Equal(scenario.OrderHashKeys[0], ReadString(firstLinkRow, "OrderHashKey"), "Order-product link order hash key drifted.");
    BenchmarkAssert.Equal(scenario.ProductHashKeys[0], ReadString(firstLinkRow, "ProductHashKey"), "Order-product link product hash key drifted.");

    AssertFulfillmentRow(
        fulfillmentRows[0],
        scenario.LinkHashKeys[0],
        allocationStatus: "Backordered",
        warehouseCode: "NORTH-1",
        hashDiff: "fulfillment-hash-1",
        loadTimestamp: FirstSatelliteLoadTimestamp,
        recordSource: "native-bulk-satellite-first");
    AssertFulfillmentRow(
        fulfillmentRows[1],
        scenario.LinkHashKeys[0],
        allocationStatus: "Allocated",
        warehouseCode: "NORTH-1",
        hashDiff: "fulfillment-hash-2",
        loadTimestamp: ChangedSatelliteLoadTimestamp,
        recordSource: "native-bulk-satellite-change");
  }

  private static void AssertFulfillmentRow(
      Dictionary<string, object> row,
      string orderProductHashKey,
      string allocationStatus,
      string warehouseCode,
      string hashDiff,
      DateTimeOffset loadTimestamp,
      string recordSource) {
    BenchmarkAssert.Equal(orderProductHashKey, ReadString(row, "OrderProductHashKey"), "Fulfillment satellite parent hash key drifted.");
    BenchmarkAssert.Equal(allocationStatus, ReadString(row, "AllocationStatus"), "Fulfillment satellite allocation status drifted.");
    BenchmarkAssert.Equal(warehouseCode, ReadString(row, "WarehouseCode"), "Fulfillment satellite warehouse code drifted.");
    BenchmarkAssert.Equal(hashDiff, ReadString(row, "HashDiff"), "Fulfillment satellite hash diff drifted.");
    BenchmarkAssert.Equal(loadTimestamp, DataVaultBenchmarkHelpers.ReadLoadTimestamp(row), "Fulfillment satellite load timestamp drifted.");
    BenchmarkAssert.Equal(recordSource, ReadString(row, "RecordSource"), "Fulfillment satellite record source drifted.");
  }

  private static string ReadString(Dictionary<string, object> row, string columnName) {
    return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Expected column '" + columnName + "' to contain a non-null value.");
  }

  private static string ComputeHash(
      IServiceProvider provider,
      IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizer = provider.GetRequiredService<IStableHashNormalizer>();
    var hashService = provider.GetRequiredService<IStableHashService>();
    var normalized = normalizer.NormalizeFields(fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return hashService.ComputeHash(normalized).Value;
  }

  private sealed class ProviderNativeBulkIngestionContext(
      DbContextOptions<ProviderNativeBulkIngestionContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateOrderProductDataVaultModel(), providerCapabilities);
    }
  }

  private sealed record ProviderNativeBulkIngestionScenario(
      DataVaultBulkSaveRequest Request,
      IReadOnlyList<string> OrderIds,
      IReadOnlyList<string> ProductIds,
      IReadOnlyList<string> OrderHashKeys,
      IReadOnlyList<string> ProductHashKeys,
      IReadOnlyList<string> LinkHashKeys);
}
