using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class ProviderNativeBulkIngestionBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private static readonly ProviderNativeBulkIngestionWorkload StagedEligibleWorkload = new(
      PairCount: 20,
      DatasetSize: "20 order-product pairs, 3 fulfillment satellite operations",
      ChangeRatio: "provider-eligible mixed hub/link/satellite bulk batch with one unchanged replay");

  private static readonly ProviderNativeBulkIngestionWorkload StagedIneligibleRetainedPathWorkload = new(
      PairCount: 18,
      DatasetSize: "18 order-product pairs, 3 fulfillment satellite operations",
      ChangeRatio: "staged-ineligible provider-native batch below the 60-operation staged boundary");

  private static readonly DateTimeOffset HubLoadTimestamp = new(2026, 5, 18, 10, 0, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset LinkLoadTimestamp = new(2026, 5, 18, 10, 5, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset FirstSatelliteLoadTimestamp = new(2026, 5, 18, 10, 10, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset UnchangedSatelliteLoadTimestamp = new(2026, 5, 18, 10, 15, 0, TimeSpan.Zero);
  private static readonly DateTimeOffset ChangedSatelliteLoadTimestamp = new(2026, 5, 18, 10, 20, 0, TimeSpan.Zero);

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;
  private readonly ProviderNativeBulkIngestionWorkload _workload;
  private readonly string? _baselineNameOverride;
  private readonly string? _executionPathOverride;
  private readonly string? _expectedProviderSaveStrategyName;

  public ProviderNativeBulkIngestionBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public ProviderNativeBulkIngestionBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant)
      : this(
          provider,
          strategy,
          loadTimestampStorage,
          hashKeyVariant,
          StagedEligibleWorkload,
          baselineNameOverride: null,
          executionPathOverride: null,
          expectedProviderSaveStrategyName: null) {
  }

  private ProviderNativeBulkIngestionBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant,
      ProviderNativeBulkIngestionWorkload workload,
      string? baselineNameOverride,
      string? executionPathOverride,
      string? expectedProviderSaveStrategyName) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);
    ArgumentNullException.ThrowIfNull(workload);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _hashKeyVariant = hashKeyVariant;
    _workload = workload;
    _baselineNameOverride = baselineNameOverride;
    _executionPathOverride = executionPathOverride;
    _expectedProviderSaveStrategyName = expectedProviderSaveStrategyName;
  }

  public string ScenarioName => "provider-native-bulk-ingestion";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => _baselineNameOverride is null
      ? DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant)
      : AppendHashKeyVariant(_baselineNameOverride, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize => _workload.DatasetSize;

  public string ChangeRatio => _workload.ChangeRatio;

  internal string ExecutionPathDetail => _executionPathOverride ?? GetDefaultExecutionPathDetail();

  public static ProviderNativeBulkIngestionBenchmark CreatePostgresRetainedDirectOrUnnest(
      BenchmarkDatabaseProvider provider,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    return CreatePostgresRetainedDirectOrUnnest(provider, loadTimestampStorage, BenchmarkHashKeyVariant.Default);
  }

  public static ProviderNativeBulkIngestionBenchmark CreatePostgresRetainedDirectOrUnnest(
      BenchmarkDatabaseProvider provider,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    return new ProviderNativeBulkIngestionBenchmark(
        provider,
        DataVaultBenchmarkStrategy.PostgresOptimized,
        loadTimestampStorage,
        hashKeyVariant,
        StagedIneligibleRetainedPathWorkload,
        "dvault-adddvaultpostgres-direct-or-unnest",
        "DVault PostgreSQL retained direct or UNNEST save path; transfer=direct-or-UNNEST; " +
        "selectedStrategy=PostgresDataVaultSaveStrategy; stagedBulkBoundary=below-60-operations; " +
        "cleanupBoundary=no-staging-table",
        "PostgresDataVaultSaveStrategy");
  }

  public static ProviderNativeBulkIngestionBenchmark CreateMySqlRetainedMultiRow(
      BenchmarkDatabaseProvider provider,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    return CreateMySqlRetainedMultiRow(provider, loadTimestampStorage, BenchmarkHashKeyVariant.Default);
  }

  public static ProviderNativeBulkIngestionBenchmark CreateMySqlRetainedMultiRow(
      BenchmarkDatabaseProvider provider,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    return new ProviderNativeBulkIngestionBenchmark(
        provider,
        DataVaultBenchmarkStrategy.MySqlOptimized,
        loadTimestampStorage,
        hashKeyVariant,
        StagedIneligibleRetainedPathWorkload,
        "dvault-adddvaultmysql-multi-row",
        "DVault MySQL retained multi-row save path; selectedStrategy=MySqlDataVaultSaveStrategy; " +
        "nativeBulkBoundary=50-plus-operations; stagedBulkBoundary=below-60-operations; cleanupBoundary=no-staging-table",
        "MySqlDataVaultSaveStrategy");
  }

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<ProviderNativeBulkIngestionContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy, _hashKeyVariant);

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var diagnostics = provider.GetRequiredService<IDataVaultDiagnosticsService>();
    var scenario = CreateScenario(provider);

    try {
      await using (var context = new ProviderNativeBulkIngestionContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var executionDetail = BenchmarkExecutionDetails.CreatePlanned(this);
      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new ProviderNativeBulkIngestionContext(options, providerCapabilities);
        var strategyDiagnostics = diagnostics.Analyze(context, scenario.Request);
        AssertStrategySelection(strategyDiagnostics);
        executionDetail = BenchmarkExecutionDetails.CreateSaveStrategyDetail(
            this,
            strategyDiagnostics,
            _workload.RequestCount,
            _workload.HubOperationCount,
            _workload.LinkOperationCount,
            _workload.SatelliteOperationCount);

        var result = await saveService.SaveAsync(context, scenario.Request, cancellationToken).ConfigureAwait(false);

        BenchmarkAssert.Equal(_workload.ExpectedRowsWritten, result.RowsWritten, "The provider-native bulk benchmark row count drifted.");
        BenchmarkAssert.Equal(_workload.ExpectedSavedRecordCount, result.SavedRecords.Count, "The provider-native bulk benchmark saved-record count drifted.");
        if (DataVaultBenchmarkHelpers.GetProviderSaveStrategyName(_strategy) is not null) {
          BenchmarkAssert.Equal(0, context.ChangeTracker.Entries().Count(), "The provider-native bulk benchmark must leave a clean change tracker.");
        }
      }).ConfigureAwait(false);

      await VerifyOutcomeAsync(options, providerCapabilities, scenario, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          _workload.PairCount.ToString(CultureInfo.InvariantCulture) +
          " order hubs, " +
          _workload.PairCount.ToString(CultureInfo.InvariantCulture) +
          " product hubs, " +
          _workload.PairCount.ToString(CultureInfo.InvariantCulture) +
          " order-product links, and 2 fulfillment satellite rows",
          executionDetail);
    }
    finally {
      await using var cleanupContext = new ProviderNativeBulkIngestionContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private void AssertStrategySelection(DataVaultDiagnosticsResult diagnostics) {
    var expectedStrategyName = _expectedProviderSaveStrategyName ?? DataVaultBenchmarkHelpers.GetProviderSaveStrategyName(_strategy);
    if (expectedStrategyName is not null) {
      DataVaultBenchmarkHelpers.AssertProviderSaveStrategySelected(diagnostics, expectedStrategyName);
    }
  }

  private ProviderNativeBulkIngestionScenario CreateScenario(IServiceProvider provider) {
    return CreateScenario(provider, _workload);
  }

  private static ProviderNativeBulkIngestionScenario CreateScenario(
      IServiceProvider provider,
      ProviderNativeBulkIngestionWorkload workload) {
    var orderIds = Enumerable.Range(0, workload.PairCount)
        .Select(index => "O-NATIVE-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var productIds = Enumerable.Range(0, workload.PairCount)
        .Select(index => "SKU-NATIVE-" + index.ToString("000", CultureInfo.InvariantCulture))
        .ToArray();
    var orderHashKeys = orderIds
        .Select(orderId => ComputeHash(provider, [new("Order Id", orderId)]))
        .ToArray();
    var productHashKeys = productIds
        .Select(productId => ComputeHash(provider, [new("Sku", productId)]))
        .ToArray();
    var linkHashKeys = Enumerable.Range(0, workload.PairCount)
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
        Enumerable.Range(0, workload.PairCount)
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

    BenchmarkAssert.Equal(scenario.OrderIds.Count, orderRows.Count, "The provider-native bulk benchmark must persist every order hub row.");
    BenchmarkAssert.Equal(scenario.ProductIds.Count, productRows.Count, "The provider-native bulk benchmark must persist every product hub row.");
    BenchmarkAssert.Equal(scenario.LinkHashKeys.Count, linkRows.Count, "The provider-native bulk benchmark must persist every order-product link row.");
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
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        scenario.OrderHashKeys[0],
        providerCapabilities,
        "Provider-native order hash key must use the active stable-hash shape.");
    DataVaultBenchmarkHelpers.AssertHashKeyStorageMapping(
        context,
        "HubOrder",
        "OrderHashKey",
        providerCapabilities,
        "Provider-native order hub hash key must project the active storage profile.");

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

  private string GetDefaultExecutionPathDetail() {
    return _strategy switch {
      DataVaultBenchmarkStrategy.ProviderNeutralFallback =>
          "DVault provider-neutral fallback path; selectedStrategy=<none>; comparisonBoundary=staged-eligible-63-operations",
      DataVaultBenchmarkStrategy.SqliteOptimized =>
          "DVault SQLite optimized path; selectedStrategy=SqliteDataVaultSaveStrategy",
      DataVaultBenchmarkStrategy.PostgresOptimized =>
          "DVault PostgreSQL staged bulk save path; transfer=COPY; selectedStrategy=PostgresDataVaultSaveStrategy; " +
          "stagedBulkBoundary=60-plus-operations; smallBatchBoundary=direct-or-UNNEST; cleanupBoundary=temporary-staging-table",
      DataVaultBenchmarkStrategy.SqlServerOptimized =>
          "DVault SQL Server staged native bulk save path; transfer=SqlBulkCopy; selectedStrategy=SqlServerDataVaultSaveStrategy; " +
          "nativeBulkBoundary=50-plus-operations; cleanupBoundary=temporary-staging-table",
      DataVaultBenchmarkStrategy.MySqlOptimized =>
          "DVault MySQL staged bulk save path; selectedStrategy=MySqlStagedDataVaultSaveStrategy; " +
          "nativeBulkBoundary=50-plus-operations; stagedBulkBoundary=60-plus-operations; cleanupBoundary=temporary-staging-tables",
      DataVaultBenchmarkStrategy.OracleOptimized =>
          "DVault Oracle direct optimized save path; selectedStrategy=OracleDataVaultSaveStrategy; " +
          "oracleBulkBoundary=direct-oracle-batching; stagedOracleBulk=not-selected-no-measured-win; cleanupBoundary=direct-provider-transaction",
      DataVaultBenchmarkStrategy.Db2Optimized =>
          "DVault DB2 optimized save path; selectedStrategy=Db2DataVaultSaveStrategy; " +
          "db2SaveBoundary=clean-context-set-based; stagedBulkBoundary=not-supported; cleanupBoundary=direct-provider-transaction",
      _ => throw new ArgumentOutOfRangeException(nameof(_strategy), _strategy, "Unsupported benchmark strategy."),
    };
  }

  private static string AppendHashKeyVariant(
      string baselineName,
      BenchmarkHashKeyVariant hashKeyVariant) {
    return hashKeyVariant == BenchmarkHashKeyVariant.Default
        ? baselineName
        : baselineName + "/" + hashKeyVariant.Label;
  }

  private sealed class ProviderNativeBulkIngestionContext(
      DbContextOptions<ProviderNativeBulkIngestionContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateOrderProductDataVaultModel(), ProviderCapabilities);
    }
  }

  private sealed record ProviderNativeBulkIngestionScenario(
      DataVaultBulkSaveRequest Request,
      IReadOnlyList<string> OrderIds,
      IReadOnlyList<string> ProductIds,
      IReadOnlyList<string> OrderHashKeys,
      IReadOnlyList<string> ProductHashKeys,
      IReadOnlyList<string> LinkHashKeys);

  private sealed record ProviderNativeBulkIngestionWorkload(
      int PairCount,
      string DatasetSize,
      string ChangeRatio) {
    public int RequestCount => 5;

    public int HubOperationCount => PairCount * 2;

    public int LinkOperationCount => PairCount;

    public int SatelliteOperationCount => 3;

    public int ExpectedRowsWritten => PairCount * 3 + 2;

    public int ExpectedSavedRecordCount => PairCount * 3 + 3;
  }
}
