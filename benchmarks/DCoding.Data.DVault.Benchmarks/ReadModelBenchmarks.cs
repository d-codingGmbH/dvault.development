using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class LatestSatelliteReadBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly CustomerProfileBulkScenarioDefinition _scenario = CustomerProfileBulkScenarios.ChangeHeavy;

  public LatestSatelliteReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(provider);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
  }

  public string ScenarioName => "latest-satellite-read";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => _scenario.DatasetSize;

  public string ChangeRatio => "90% repeat-change history latest read";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<CustomerProfileReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    try {
      await using (var context = new CustomerProfileReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var customerHashKeys = await ReadBenchmarkServices
          .SeedCustomerProfileHistoryAsync(
              options,
              providerCapabilities,
              saveService,
              _scenario,
              cancellationToken)
          .ConfigureAwait(false);
      IReadOnlyList<DataVaultSatelliteReadRecord> readRows = [];
      var request = new DataVaultLatestSatelliteReadRequest(
          ScenarioContracts.CustomerProfileSatellite,
          customerHashKeys);
      DataVaultDiagnosticsResult diagnostics;
      await using (var diagnosticsContext = new CustomerProfileReadContext(options, providerCapabilities)) {
        diagnostics = readDiagnostics.Analyze(diagnosticsContext, request);
        ReadBenchmarkServices.AssertReadStrategySelection(
            _strategy,
            ScenarioName,
            diagnostics);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new CustomerProfileReadContext(options, providerCapabilities);
        readRows = await readService
            .ReadLatestSatelliteRowsAsync(
                context,
                request,
                cancellationToken)
            .ConfigureAwait(false);
      }).ConfigureAwait(false);

      VerifyLatestRows(readRows, customerHashKeys);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " latest profile satellite rows read from " +
          _scenario.TotalChangeCount.ToString(CultureInfo.InvariantCulture) +
          " seeded profile states",
          BenchmarkExecutionDetails.CreateReadStrategyDetail(this, diagnostics));
    }
    finally {
      await using var cleanupContext = new CustomerProfileReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private void VerifyLatestRows(
      IReadOnlyList<DataVaultSatelliteReadRecord> readRows,
      IReadOnlyList<string> customerHashKeys) {
    BenchmarkAssert.Equal(
        _scenario.CustomerCount,
        readRows.Count,
        "The latest satellite read benchmark must return one row per seeded customer.");

    var sampleHashKey = customerHashKeys[_scenario.SampleCustomerIndex];
    var sampleRow = BenchmarkAssert.Single(
        readRows.Where(row => string.Equals(row.ParentHashKey, sampleHashKey, StringComparison.Ordinal)),
        "The latest satellite read benchmark must return the sample customer row.");
    var expected = _scenario.CreateEvent(_scenario.SampleCustomerIndex, _scenario.ChangeCount - 1);

    BenchmarkAssert.Equal("Profile", sampleRow.MetadataName, "The latest satellite read metadata name drifted.");
    BenchmarkAssert.Equal("SatCustomerProfile", sampleRow.TableName, "The latest satellite read table name drifted.");
    BenchmarkAssert.Equal(expected.HashDiff, sampleRow.HashDiff, "The latest satellite read hash diff drifted.");
    BenchmarkAssert.Equal(expected.ChangedAtUtc, sampleRow.LoadTimestamp, "The latest satellite read timestamp drifted.");
    BenchmarkAssert.Equal(expected.RecordSource, sampleRow.RecordSource, "The latest satellite read record source drifted.");
    BenchmarkAssert.Equal(expected.CustomerName, sampleRow.PayloadValues["customer_name"], "The latest satellite read name drifted.");
    BenchmarkAssert.Equal(expected.CustomerStatus, sampleRow.PayloadValues["customer_status"], "The latest satellite read status drifted.");
    BenchmarkAssert.True(
        readRows.All(row => customerHashKeys.Contains(row.ParentHashKey, StringComparer.Ordinal)),
        "The latest satellite read benchmark returned an unseeded parent hash key.");
  }
}

internal sealed class PitAsOfReadBenchmark : IScenarioBenchmark {
  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly CustomerProfileBulkScenarioDefinition _scenario = CustomerProfileBulkScenarios.ChangeHeavy;

  public PitAsOfReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(provider);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
  }

  public string ScenarioName => "pit-as-of-read";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => "100 customers, 100 PIT rows, 2 satellite segments";

  public string ChangeRatio => "as-of read after latest profile/status snapshots";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<PitAsOfReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    try {
      await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
      }

      var customerHashKeys = await ReadBenchmarkServices
          .SeedCustomerProfileHistoryAsync(
              options,
              providerCapabilities,
              saveService,
              _scenario,
              cancellationToken)
          .ConfigureAwait(false);
      await SeedStatusAndPitRowsAsync(
          options,
          providerCapabilities,
          saveService,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);

      IReadOnlyList<DataVaultPitReadRecord> readRows = [];
      var request = new DataVaultPitAsOfReadRequest(
          PitReadScenario.Metadata.Pit,
          customerHashKeys,
          PitReadScenario.AsOf);
      DataVaultDiagnosticsResult diagnostics;
      await using (var diagnosticsContext = new PitAsOfReadContext(options, providerCapabilities)) {
        diagnostics = readDiagnostics.Analyze(diagnosticsContext, request);
        ReadBenchmarkServices.AssertReadStrategySelection(
            _strategy,
            ScenarioName,
            diagnostics);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new PitAsOfReadContext(options, providerCapabilities);
        readRows = await readService
            .ReadPitRowsAsync(
                context,
                request,
                cancellationToken)
            .ConfigureAwait(false);
      }).ConfigureAwait(false);

      VerifyPitRows(readRows, customerHashKeys);

      return new ScenarioBenchmarkResult(
          elapsed,
          _scenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " PIT as-of rows read across profile and status satellite snapshots",
          BenchmarkExecutionDetails.CreateReadStrategyDetail(this, diagnostics));
    }
    finally {
      await using var cleanupContext = new PitAsOfReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private async Task SeedStatusAndPitRowsAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    var statusTimestamp = _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount);
    await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
      var statusRequests = Enumerable.Range(0, _scenario.CustomerCount)
          .Select(customerIndex => new DataVaultSaveRequest(
              statusTimestamp,
              _scenario.RecordSource,
              [],
              [],
              [
                  new DataVaultSatelliteSaveOperation(
                      PitReadScenario.Metadata.Status,
                      customerHashKeys[customerIndex],
                      [new("status_code", customerIndex % 2 == 0 ? "Active" : "Review")],
                      "status-" + customerIndex.ToString("0000", CultureInfo.InvariantCulture)),
              ]))
          .ToArray();

      await saveService
          .SaveAsync(context, new DataVaultBulkSaveRequest(statusRequests), cancellationToken)
          .ConfigureAwait(false);
    }

    await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
      var pitRows = context.Set<Dictionary<string, object>>("PitCustomerProfileStatus");
      var profileSnapshotTimestamp = _scenario.BaseTimestamp.AddMinutes(_scenario.ChangeCount - 1);
      var storedPitTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
          providerCapabilities,
          DataVaultLogicalPropertyKind.LoadTimestamp,
          PitReadScenario.PitTimestamp);
      var storedProfileTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
          providerCapabilities,
          DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
          profileSnapshotTimestamp);
      var storedStatusTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
          providerCapabilities,
          DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
          statusTimestamp);

      foreach (var customerHashKey in customerHashKeys) {
        pitRows.Add(new Dictionary<string, object>(StringComparer.Ordinal) {
          ["CustomerHashKey"] = customerHashKey,
          ["LoadTimestamp"] = storedPitTimestamp,
          ["ProfileLoadTimestamp"] = storedProfileTimestamp,
          ["StatusLoadTimestamp"] = storedStatusTimestamp,
        });
      }

      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
  }

  private void VerifyPitRows(
      IReadOnlyList<DataVaultPitReadRecord> readRows,
      IReadOnlyList<string> customerHashKeys) {
    BenchmarkAssert.Equal(
        _scenario.CustomerCount,
        readRows.Count,
        "The PIT read benchmark must return one row per seeded customer.");

    var sampleHashKey = customerHashKeys[_scenario.SampleCustomerIndex];
    var sampleRow = BenchmarkAssert.Single(
        readRows.Where(row => string.Equals(row.ParentHashKey, sampleHashKey, StringComparison.Ordinal)),
        "The PIT read benchmark must return the sample customer row.");
    var profile = sampleRow.SatelliteSnapshotsByName["Profile"];
    var status = sampleRow.SatelliteSnapshotsByName["Status"];
    var expectedProfile = _scenario.CreateEvent(_scenario.SampleCustomerIndex, _scenario.ChangeCount - 1);

    BenchmarkAssert.Equal(PitReadScenario.PitTimestamp, sampleRow.LoadTimestamp, "The PIT read load timestamp drifted.");
    BenchmarkAssert.True(profile.IsPresent, "The PIT read benchmark must materialize a profile snapshot.");
    BenchmarkAssert.True(status.IsPresent, "The PIT read benchmark must materialize a status snapshot.");
    BenchmarkAssert.Equal(expectedProfile.HashDiff, profile.HashDiff, "The PIT read profile hash diff drifted.");
    BenchmarkAssert.Equal(expectedProfile.CustomerName, profile.PayloadValues["customer_name"], "The PIT read profile name drifted.");
    BenchmarkAssert.Equal(expectedProfile.CustomerStatus, profile.PayloadValues["customer_status"], "The PIT read profile status drifted.");
    BenchmarkAssert.Equal("status-0042", status.HashDiff, "The PIT read status hash diff drifted.");
    BenchmarkAssert.Equal("Active", status.PayloadValues["status_code"], "The PIT read status code drifted.");
  }
}

internal sealed class BridgeTraversalReadBenchmark : IScenarioBenchmark {
  private const int DescendantCount = 100;
  private const int MaximumDepth = 3;
  private const int DepthCycle = 5;
  private const string AncestorHashKey = "region-root";

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;

  public BridgeTraversalReadBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    ArgumentNullException.ThrowIfNull(provider);

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
  }

  public string ScenarioName => "bridge-traversal-read";

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public string DatasetSize => "1 hierarchy ancestor with 100 descendant bridge rows";

  public string ChangeRatio => "maximum depth 3 of 5";

  public async Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<BridgeTraversalReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy);
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();

    try {
      await using (var context = new BridgeTraversalReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
        await SeedBridgeRowsAsync(context, cancellationToken).ConfigureAwait(false);
      }

      IReadOnlyList<DataVaultBridgeReadRecord> readRows = [];
      var request = new DataVaultBridgeReadRequest(
          BridgeReadScenario.Metadata.Bridge,
          DataVaultBridgeTraversalEndpoint.Ancestor,
          [AncestorHashKey],
          MaximumDepth);
      DataVaultDiagnosticsResult diagnostics;
      await using (var diagnosticsContext = new BridgeTraversalReadContext(options, providerCapabilities)) {
        diagnostics = readDiagnostics.Analyze(diagnosticsContext, request);
        ReadBenchmarkServices.AssertReadStrategySelection(
            _strategy,
            ScenarioName,
            diagnostics);
      }

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new BridgeTraversalReadContext(options, providerCapabilities);
        readRows = await readService
            .ReadBridgeRowsAsync(
                context,
                request,
                cancellationToken)
            .ConfigureAwait(false);
      }).ConfigureAwait(false);

      VerifyBridgeRows(readRows);

      return new ScenarioBenchmarkResult(
          elapsed,
          ExpectedDepthBoundedRowCount().ToString(CultureInfo.InvariantCulture) +
          " bridge traversal rows read from " +
          DescendantCount.ToString(CultureInfo.InvariantCulture) +
          " seeded hierarchy rows",
          BenchmarkExecutionDetails.CreateReadStrategyDetail(this, diagnostics));
    }
    finally {
      await using var cleanupContext = new BridgeTraversalReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private static async Task SeedBridgeRowsAsync(
      BridgeTraversalReadContext context,
      CancellationToken cancellationToken) {
    var rows = context.Set<Dictionary<string, object>>("BridgeSalesRegionHierarchy");

    for (var index = 1; index <= DescendantCount; index++) {
      rows.Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["AncestorSalesRegionHashKey"] = AncestorHashKey,
        ["DescendantSalesRegionHashKey"] = "region-" + index.ToString("000", CultureInfo.InvariantCulture),
        ["TraversalDepth"] = ((index - 1) % DepthCycle) + 1,
      });
    }

    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  private static void VerifyBridgeRows(IReadOnlyList<DataVaultBridgeReadRecord> readRows) {
    BenchmarkAssert.Equal(
        ExpectedDepthBoundedRowCount(),
        readRows.Count,
        "The bridge traversal benchmark must return only rows within the requested maximum depth.");
    BenchmarkAssert.True(
        readRows.All(row => row.TraversalDepth is > 0 and <= MaximumDepth),
        "The bridge traversal benchmark returned a row outside the requested depth bound.");

    var firstRow = readRows[0];
    BenchmarkAssert.Equal("SalesRegionHierarchy", firstRow.MetadataName, "The bridge traversal metadata name drifted.");
    BenchmarkAssert.Equal("BridgeSalesRegionHierarchy", firstRow.TableName, "The bridge traversal table name drifted.");
    BenchmarkAssert.Equal(AncestorHashKey, firstRow.EndpointHashKeys[0].HashKey, "The bridge traversal ancestor hash key drifted.");
  }

  private static int ExpectedDepthBoundedRowCount() {
    return Enumerable.Range(1, DescendantCount)
        .Count(index => ((index - 1) % DepthCycle) + 1 <= MaximumDepth);
  }
}

internal static class ReadBenchmarkServices {
  public static ServiceProvider CreateProvider(DataVaultBenchmarkStrategy strategy) {
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, strategy);

    return services.BuildServiceProvider(validateScopes: true);
  }

  public static void AssertReadStrategySelection(
      DataVaultBenchmarkStrategy strategy,
      string scenarioName,
      DataVaultDiagnosticsResult diagnostics) {
    var expectedStrategyName = DataVaultBenchmarkHelpers.GetProviderReadStrategyName(strategy, scenarioName);
    if (expectedStrategyName is not null) {
      DataVaultBenchmarkHelpers.AssertProviderReadStrategySelected(diagnostics, expectedStrategyName);
    }
  }

  public static async Task<IReadOnlyList<string>> SeedCustomerProfileHistoryAsync<TContext>(
      DbContextOptions<TContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      CustomerProfileBulkScenarioDefinition scenario,
      CancellationToken cancellationToken)
      where TContext : CustomerProfileReadContext {
    await using var context = (TContext)Activator.CreateInstance(
        typeof(TContext),
        options,
        providerCapabilities)!;
    var hubResult = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            scenario.BaseTimestamp,
            scenario.RecordSource,
            Enumerable.Range(0, scenario.CustomerCount)
                .Select(customerIndex => new DataVaultHubSaveOperation(
                    ScenarioContracts.CustomerHub,
                    [new("Customer Id", scenario.CreateBusinessKey(customerIndex))]))
                .ToArray(),
            []),
        cancellationToken).ConfigureAwait(false);
    var customerHashKeys = hubResult.SavedRecords
        .Select((record, customerIndex) => new {
          CustomerIndex = customerIndex,
          record.HashKey,
        })
        .OrderBy(value => value.CustomerIndex)
        .Select(value => value.HashKey)
        .ToArray();
    var satelliteRequests = Enumerable.Range(0, scenario.ChangeCount)
        .Select(changeIndex => new DataVaultSaveRequest(
            scenario.BaseTimestamp.AddMinutes(changeIndex),
            scenario.RecordSource,
            [],
            [],
            Enumerable.Range(0, scenario.CustomerCount)
                .Select(customerIndex => {
                  var customerProfileEvent = scenario.CreateEvent(customerIndex, changeIndex);
                  return new DataVaultSatelliteSaveOperation(
                      ScenarioContracts.CustomerProfileSatellite,
                      customerHashKeys[customerIndex],
                      [
                          new("customer_name", customerProfileEvent.CustomerName),
                          new("customer_status", customerProfileEvent.CustomerStatus),
                      ],
                      customerProfileEvent.HashDiff);
                })
                .ToArray()))
        .ToArray();

    await saveService
        .SaveAsync(context, new DataVaultBulkSaveRequest(satelliteRequests), cancellationToken)
        .ConfigureAwait(false);

    return customerHashKeys;
  }
}

internal class CustomerProfileReadContext(
    DbContextOptions options,
    DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
  protected DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(
        ScenarioContracts.CreateCustomerProfileDataVaultModel(),
        ProviderCapabilities);
  }
}

internal sealed class PitAsOfReadContext(
    DbContextOptions<PitAsOfReadContext> options,
    DataVaultProviderCapabilityProfile providerCapabilities)
    : CustomerProfileReadContext(options, providerCapabilities) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(
        PitReadScenario.Metadata.Model,
        ProviderCapabilities);
  }
}

internal sealed class BridgeTraversalReadContext(
    DbContextOptions<BridgeTraversalReadContext> options,
    DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.SharedTypeEntity<Dictionary<string, object>>("BridgeSalesRegionHierarchy", entityBuilder => {
      entityBuilder.ToTable("BridgeSalesRegionHierarchy");
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "BridgeSalesRegionHierarchy");
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Bridge);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "SalesRegionHierarchy");

      ConfigureBridgeEndpointProperty(entityBuilder, "AncestorSalesRegionHashKey", "SalesRegion", 0);
      ConfigureBridgeEndpointProperty(entityBuilder, "DescendantSalesRegionHashKey", "SalesRegion", 1);
      ConfigureBridgeDepthProperty(entityBuilder);

      entityBuilder.HasKey("AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey")
          .HasName("PkBridgeSalesRegionHierarchy");
      entityBuilder.HasIndex("AncestorSalesRegionHashKey", "TraversalDepth")
          .HasDatabaseName("IxBridgeRegionAncestorDepth");
      entityBuilder.HasIndex("DescendantSalesRegionHashKey", "AncestorSalesRegionHashKey")
          .HasDatabaseName("IxBridgeRegionDescAncestor");
    });
  }

  private void ConfigureBridgeEndpointProperty(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      string propertyName,
      string metadataName,
      int ordinal) {
    var mapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.ParticipantReference);
    var propertyBuilder = entityBuilder.IndexerProperty<string>(propertyName);

    propertyBuilder.HasColumnName(propertyName);
    propertyBuilder.HasColumnType(mapping.NativeStoreType);
    propertyBuilder.HasColumnOrder(ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, propertyName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.ParticipantReference);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, metadataName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, providerCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, DataVaultLogicalPropertyKind.ParticipantReference);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, mapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, mapping.ValueFormat);
  }

  private void ConfigureBridgeDepthProperty(EntityTypeBuilder<Dictionary<string, object>> entityBuilder) {
    var mapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.BridgeDepth);
    var propertyBuilder = entityBuilder.IndexerProperty<int>("TraversalDepth");

    propertyBuilder.HasColumnName("TraversalDepth");
    propertyBuilder.HasColumnType(mapping.NativeStoreType);
    propertyBuilder.HasColumnOrder(2);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "TraversalDepth");
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.BridgeDepth);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "TraversalDepth");
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, 2);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, providerCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, DataVaultLogicalPropertyKind.BridgeDepth);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, mapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, mapping.ValueFormat);
  }
}

internal static class PitReadScenario {
  public static readonly DateTimeOffset PitTimestamp = new(2026, 4, 29, 10, 11, 0, TimeSpan.Zero);
  public static readonly DateTimeOffset AsOf = new(2026, 4, 29, 10, 12, 0, TimeSpan.Zero);

  public static PitReadMetadata Metadata { get; } = CreateMetadata();

  private static PitReadMetadata CreateMetadata() {
    var status = new DataVaultSatelliteMetadata(
        "Status",
        ScenarioContracts.CustomerHub.ToReference(),
        ["status_code"]);
    var pit = new DataVaultPitMetadata(
        ScenarioContracts.CustomerHub.ToReference(),
        [ScenarioContracts.CustomerProfileSatellite.Name, status.Name]);
    var model = new DataVaultMetadataModel(
        [ScenarioContracts.CustomerHub],
        [],
        [ScenarioContracts.CustomerProfileSatellite, status],
        [pit]);

    return new PitReadMetadata(status, pit, model);
  }
}

internal sealed record PitReadMetadata(
    DataVaultSatelliteMetadata Status,
    DataVaultPitMetadata Pit,
    DataVaultMetadataModel Model);

internal static class BridgeReadScenario {
  public static BridgeReadMetadata Metadata { get; } = CreateMetadata();

  private static BridgeReadMetadata CreateMetadata() {
    var bridge = DataVaultBridgeMetadata.Hierarchy(
        "SalesRegionHierarchy",
        DataVaultMetadataReference.Hub("SalesRegion"),
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        DataVaultMetadataReference.Hub("SalesRegion"),
        ancestorParticipantOrdinal: 0,
        descendantParticipantOrdinal: 1);

    return new BridgeReadMetadata(bridge);
  }
}

internal sealed record BridgeReadMetadata(
    DataVaultBridgeMetadata Bridge);
