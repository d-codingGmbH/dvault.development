using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class PitFullRebuildMaintenanceBenchmark : IScenarioBenchmark, IBenchmarkHashKeyVariantSource {
  private const string Scenario = "pit-full-rebuild-maintenance";
  private const string MaintenanceScope = "FullRebuild";
  private const string SqlServerPitShapeBoundary = "clean-ordinary-hub-parent";
  private const int SqlServerStalePitRowsPerCustomer = 1;
  private const string PostgresPitMaintenanceStrategyName = "PostgresDataVaultPitMaintenanceStrategy";
  private const string PostgresPitShapeBoundary =
      "ordinary-hub-parent|shared-driving-key-multi-active-hub-parent|link-parent-non-multi-active";

  private static readonly OrdinaryPitMaintenanceMetadata OrdinaryMetadata = CreateOrdinaryMetadata();
  private static readonly MultiActivePitMaintenanceMetadata MultiActiveMetadata = CreateMultiActiveMetadata();
  private static readonly LinkParentPitMaintenanceMetadata LinkParentMetadata = CreateLinkParentMetadata();

  private static readonly DateTimeOffset OrdinaryStatusTimestamp = Utc(2026, 5, 21, 9, 0);
  private static readonly DateTimeOffset OrdinaryProfileTimestamp = Utc(2026, 5, 21, 10, 0);
  private static readonly DateTimeOffset OrdinarySecondStatusTimestamp = Utc(2026, 5, 21, 11, 0);
  private static readonly DateTimeOffset MultiActiveProfileBeforeTupleTimestamp = Utc(2026, 5, 22, 8, 0);
  private static readonly DateTimeOffset MultiActiveBillingContactTimestamp = Utc(2026, 5, 22, 9, 0);
  private static readonly DateTimeOffset MultiActiveShippingContactTimestamp = Utc(2026, 5, 22, 10, 0);
  private static readonly DateTimeOffset MultiActiveProfileAfterTupleTimestamp = Utc(2026, 5, 22, 11, 0);
  private static readonly DateTimeOffset LinkStateTimestamp = Utc(2026, 5, 23, 9, 0);
  private static readonly DateTimeOffset LinkFulfillmentTimestamp = Utc(2026, 5, 23, 10, 0);
  private static readonly DateTimeOffset LinkSecondStateTimestamp = Utc(2026, 5, 23, 11, 0);

  private readonly BenchmarkDatabaseProvider _provider;
  private readonly DataVaultBenchmarkStrategy _strategy;
  private readonly DataVaultLoadTimestampStorage _loadTimestampStorage;
  private readonly BenchmarkHashKeyVariant _hashKeyVariant;
  private readonly CustomerProfileBulkScenarioDefinition _sqlServerScenario = CustomerProfileBulkScenarios.ChangeHeavy;

  public PitFullRebuildMaintenanceBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage)
      : this(provider, strategy, loadTimestampStorage, BenchmarkHashKeyVariant.Default) {
  }

  public PitFullRebuildMaintenanceBenchmark(
      BenchmarkDatabaseProvider provider,
      DataVaultBenchmarkStrategy strategy,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(provider);
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    if (IsSqlServerProviderName(provider.ProviderName)) {
      if (strategy is not DataVaultBenchmarkStrategy.ProviderNeutralFallback and not DataVaultBenchmarkStrategy.SqlServerOptimized) {
        throw new ArgumentOutOfRangeException(
            nameof(strategy),
            strategy,
            "PIT full-rebuild maintenance benchmarks are currently bounded to SQL Server and provider-neutral comparison rows.");
      }
    }
    else if (IsPostgresProviderName(provider.ProviderName)) {
      if (strategy is not DataVaultBenchmarkStrategy.ProviderNeutralFallback and not DataVaultBenchmarkStrategy.PostgresOptimized) {
        throw new ArgumentOutOfRangeException(
            nameof(strategy),
            strategy,
            "PIT full-rebuild maintenance benchmarks are currently bounded to PostgreSQL and provider-neutral comparison rows.");
      }
    }
    else {
      throw new ArgumentOutOfRangeException(
          nameof(provider),
          provider.ProviderName,
          "PIT full-rebuild maintenance benchmarks are currently bounded to PostgreSQL and SQL Server external providers.");
    }

    _provider = provider;
    _strategy = strategy;
    _loadTimestampStorage = loadTimestampStorage;
    _hashKeyVariant = hashKeyVariant;
  }

  public string ScenarioName => Scenario;

  public string ProviderName => _provider.ProviderName;

  public string BaselineName => DataVaultBenchmarkHelpers.GetDataVaultBaselineName(_strategy, _hashKeyVariant);

  public string StrategyFamily => DataVaultBenchmarkHelpers.GetDataVaultStrategyFamily(_strategy);

  public BenchmarkHashKeyVariant HashKeyVariant => _hashKeyVariant;

  public string DatasetSize => IsSqlServerProvider
      ? _sqlServerScenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customers, " +
          ExpectedSqlServerRowsWritten.ToString(CultureInfo.InvariantCulture) +
          " rebuilt PIT rows, 2 satellite segments"
      : "3 PIT shapes, 3 parent identities, 10 rebuilt PIT rows";

  public string ChangeRatio => IsSqlServerProvider
      ? "full rebuild after 90% repeat-change history plus status snapshot"
      : "full rebuild across PostgreSQL supported PIT maintenance shapes";

  internal string ExecutionPathDetail => IsSqlServerProvider
      ? _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
          ? "DVault SQL Server PIT full-rebuild maintenance path; maintenanceScope=" + MaintenanceScope +
              "; selectedStrategy=SqlServerDataVaultPitMaintenanceService; pitShapeBoundary=" + SqlServerPitShapeBoundary
          : "DVault provider-neutral PIT full-rebuild maintenance path; maintenanceScope=" + MaintenanceScope +
              "; selectedStrategy=<none>; pitShapeBoundary=" + SqlServerPitShapeBoundary
      : _strategy == DataVaultBenchmarkStrategy.ProviderNeutralFallback
          ? "DVault provider-neutral PIT full rebuild path; maintenanceScope=FullRebuild; selectedStrategy=<none>; " +
              "providerSpecificPitMaintenanceStrategy=fallback; fallbackCauses=" +
              DataVaultPitMaintenanceStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered +
              "; pitShapeBoundary=" +
              PostgresPitShapeBoundary
          : "DVault PostgreSQL optimized PIT full rebuild path; maintenanceScope=FullRebuild; " +
              "selectedStrategy=" +
              PostgresPitMaintenanceStrategyName +
              "; plannedPitMaintenanceStrategy=" +
              PostgresPitMaintenanceStrategyName +
              "; fallbackCauses=none; pitShapeBoundary=" +
              PostgresPitShapeBoundary;

  private bool IsSqlServerProvider => IsSqlServerProviderName(_provider.ProviderName);

  private bool IsPostgresProvider => IsPostgresProviderName(_provider.ProviderName);

  private int ExpectedSqlServerRowsDeleted => _sqlServerScenario.CustomerCount * SqlServerStalePitRowsPerCustomer;

  private int ExpectedSqlServerRowsWritten => _sqlServerScenario.CustomerCount * (_sqlServerScenario.ChangeCount + 1);

  public Task<ScenarioBenchmarkResult> ExecuteAsync(CancellationToken cancellationToken) {
    return IsSqlServerProvider
        ? ExecuteSqlServerAsync(cancellationToken)
        : ExecutePostgresAsync(cancellationToken);
  }

  private async Task<ScenarioBenchmarkResult> ExecuteSqlServerAsync(CancellationToken cancellationToken) {
    using var database = _provider.CreateDatabase();
    var options = database.CreateOptions<PitAsOfReadContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    using var provider = ReadBenchmarkServices.CreateProvider(_strategy, _hashKeyVariant);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();

    try {
      await using (var context = new PitAsOfReadContext(options, providerCapabilities)) {
        await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
        await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
      }

      var customerHashKeys = await ReadBenchmarkServices
          .SeedCustomerProfileHistoryAsync(
              options,
              providerCapabilities,
              saveService,
              _sqlServerScenario,
              cancellationToken)
          .ConfigureAwait(false);
      await SeedSqlServerStatusHistoryAsync(
          options,
          providerCapabilities,
          saveService,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);
      await SeedSqlServerStalePitRowsAsync(
          options,
          providerCapabilities,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);

      DataVaultPitMaintenanceResult? maintenanceResult = null;
      var executionDetail = BenchmarkExecutionDetails.CreatePlanned(this);
      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        await using var context = new PitAsOfReadContext(options, providerCapabilities);
        maintenanceResult = await maintenanceService
            .RebuildAsync(
                context,
                new DataVaultPitRebuildRequest(PitReadScenario.Metadata.Pit),
                cancellationToken)
            .ConfigureAwait(false);
        executionDetail = CreateCompletedExecutionDetail(
            context.Database.ProviderName,
            maintenanceService.GetType().Name,
            maintenanceResult);
      }).ConfigureAwait(false);

      VerifySqlServerMaintenanceResult(maintenanceResult);
      await VerifySqlServerRebuiltPitRowsAsync(
          options,
          providerCapabilities,
          customerHashKeys,
          cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          ExpectedSqlServerRowsDeleted.ToString(CultureInfo.InvariantCulture) +
          " stale PIT rows replaced by " +
          ExpectedSqlServerRowsWritten.ToString(CultureInfo.InvariantCulture) +
          " rebuilt PIT rows for " +
          _sqlServerScenario.CustomerCount.ToString(CultureInfo.InvariantCulture) +
          " customers",
          executionDetail);
    }
    finally {
      await using var cleanupContext = new PitAsOfReadContext(options, providerCapabilities);
      await database.CleanupAsync(cleanupContext, CancellationToken.None).ConfigureAwait(false);
    }
  }

  private async Task SeedSqlServerStatusHistoryAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    var statusTimestamp = _sqlServerScenario.BaseTimestamp.AddMinutes(_sqlServerScenario.ChangeCount);
    await using var context = new PitAsOfReadContext(options, providerCapabilities);
    var statusRequests = Enumerable.Range(0, _sqlServerScenario.CustomerCount)
        .Select(customerIndex => new DataVaultSaveRequest(
            statusTimestamp,
            _sqlServerScenario.RecordSource,
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

  private async Task SeedSqlServerStalePitRowsAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    await using var context = new PitAsOfReadContext(options, providerCapabilities);
    var pitRows = context.Set<Dictionary<string, object>>("PitCustomerProfileStatus");
    var storedPitTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
        providerCapabilities,
        DataVaultLogicalPropertyKind.LoadTimestamp,
        PitReadScenario.PitTimestamp);
    var storedProfileTimestamp = DataVaultBenchmarkHelpers.ToStoredTimestamp(
        providerCapabilities,
        DataVaultLogicalPropertyKind.SatelliteSnapshotReference,
        _sqlServerScenario.BaseTimestamp);

    foreach (var customerHashKey in customerHashKeys) {
      pitRows.Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["CustomerHashKey"] = customerHashKey,
        ["LoadTimestamp"] = storedPitTimestamp,
        ["ProfileLoadTimestamp"] = storedProfileTimestamp,
        ["StatusLoadTimestamp"] = null!,
      });
    }

    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  private void VerifySqlServerMaintenanceResult(DataVaultPitMaintenanceResult? maintenanceResult) {
    if (maintenanceResult is null) {
      throw new InvalidOperationException("The PIT maintenance benchmark did not capture a maintenance result.");
    }

    BenchmarkAssert.Equal(
        _sqlServerScenario.CustomerCount,
        maintenanceResult.ParentHashKeyCount,
        "The PIT full-rebuild benchmark parent count drifted.");
    BenchmarkAssert.Equal(
        ExpectedSqlServerRowsDeleted,
        maintenanceResult.RowsDeleted,
        "The PIT full-rebuild benchmark must delete the seeded stale PIT rows.");
    BenchmarkAssert.Equal(
        ExpectedSqlServerRowsWritten,
        maintenanceResult.RowsWritten,
        "The PIT full-rebuild benchmark rebuilt row count drifted.");
    BenchmarkAssert.Equal(
        "PitCustomerProfileStatus",
        maintenanceResult.TableName,
        "The PIT full-rebuild benchmark table name drifted.");
  }

  private async Task VerifySqlServerRebuiltPitRowsAsync(
      DbContextOptions<PitAsOfReadContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IReadOnlyList<string> customerHashKeys,
      CancellationToken cancellationToken) {
    await using var context = new PitAsOfReadContext(options, providerCapabilities);
    var rows = await context.Set<Dictionary<string, object>>("PitCustomerProfileStatus")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    BenchmarkAssert.Equal(
        ExpectedSqlServerRowsWritten,
        rows.Count,
        "The PIT full-rebuild benchmark must persist the expected rebuilt PIT row count.");
    BenchmarkAssert.True(
        !rows.Any(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row) == PitReadScenario.PitTimestamp),
        "The PIT full-rebuild benchmark must replace the seeded stale PIT timestamp.");

    var sampleHashKey = customerHashKeys[_sqlServerScenario.SampleCustomerIndex];
    var sampleRows = rows
        .Where(row => string.Equals(ReadString(row, "CustomerHashKey"), sampleHashKey, StringComparison.Ordinal))
        .OrderBy(row => DataVaultBenchmarkHelpers.ReadLoadTimestamp(row))
        .ToArray();
    BenchmarkAssert.Equal(
        _sqlServerScenario.ChangeCount + 1,
        sampleRows.Length,
        "The PIT full-rebuild benchmark must rebuild each sample customer timeline point.");

    var finalRow = sampleRows[^1];
    BenchmarkAssert.Equal(
        _sqlServerScenario.BaseTimestamp.AddMinutes(_sqlServerScenario.ChangeCount),
        DataVaultBenchmarkHelpers.ReadLoadTimestamp(finalRow),
        "The PIT full-rebuild benchmark final PIT load timestamp drifted.");
    BenchmarkAssert.Equal(
        _sqlServerScenario.BaseTimestamp.AddMinutes(_sqlServerScenario.ChangeCount - 1),
        DataVaultBenchmarkHelpers.ReadLoadTimestamp(finalRow, "ProfileLoadTimestamp"),
        "The PIT full-rebuild benchmark profile snapshot timestamp drifted.");
    BenchmarkAssert.Equal(
        _sqlServerScenario.BaseTimestamp.AddMinutes(_sqlServerScenario.ChangeCount),
        DataVaultBenchmarkHelpers.ReadLoadTimestamp(finalRow, "StatusLoadTimestamp"),
        "The PIT full-rebuild benchmark status snapshot timestamp drifted.");
    DataVaultBenchmarkHelpers.AssertStableHashKey(
        sampleHashKey,
        providerCapabilities,
        "The PIT full-rebuild benchmark parent hash key must use the active stable-hash shape.");
  }

  private async Task<ScenarioBenchmarkResult> ExecutePostgresAsync(CancellationToken cancellationToken) {
    using var ordinaryDatabase = _provider.CreateDatabase();
    using var multiActiveDatabase = _provider.CreateDatabase();
    using var linkParentDatabase = _provider.CreateDatabase();
    var ordinaryOptions = ordinaryDatabase.CreateOptions<PitFullRebuildOrdinaryContext>();
    var multiActiveOptions = multiActiveDatabase.CreateOptions<PitFullRebuildMultiActiveContext>();
    var linkParentOptions = linkParentDatabase.CreateOptions<PitFullRebuildLinkParentContext>();
    var providerCapabilities = _provider.GetProviderCapabilities(_loadTimestampStorage, _hashKeyVariant);
    var services = new ServiceCollection();
    DataVaultBenchmarkHelpers.AddDataVaultServices(services, _strategy, _hashKeyVariant);

    using var serviceProvider = services.BuildServiceProvider(validateScopes: true);
    var saveService = serviceProvider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = serviceProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    var strategies = serviceProvider.GetServices<IDataVaultProviderPitMaintenanceStrategy>().ToArray();
    var runResults = Array.Empty<PitMaintenanceShapeRunResult>();

    try {
      await SeedOrdinaryAsync(
          ordinaryDatabase,
          ordinaryOptions,
          providerCapabilities,
          saveService,
          cancellationToken).ConfigureAwait(false);
      await SeedMultiActiveAsync(
          multiActiveDatabase,
          multiActiveOptions,
          providerCapabilities,
          saveService,
          cancellationToken).ConfigureAwait(false);
      await SeedLinkParentAsync(
          linkParentDatabase,
          linkParentOptions,
          providerCapabilities,
          saveService,
          cancellationToken).ConfigureAwait(false);

      var elapsed = await BenchmarkClock.MeasureAsync(async () => {
        runResults =
        [
            await RebuildOrdinaryAsync(
                ordinaryOptions,
                providerCapabilities,
                maintenanceService,
                strategies,
                cancellationToken).ConfigureAwait(false),
            await RebuildMultiActiveAsync(
                multiActiveOptions,
                providerCapabilities,
                maintenanceService,
                strategies,
                cancellationToken).ConfigureAwait(false),
            await RebuildLinkParentAsync(
                linkParentOptions,
                providerCapabilities,
                maintenanceService,
                strategies,
                cancellationToken).ConfigureAwait(false),
        ];
      }).ConfigureAwait(false);

      await VerifyOrdinaryAsync(ordinaryOptions, providerCapabilities, cancellationToken).ConfigureAwait(false);
      await VerifyMultiActiveAsync(multiActiveOptions, providerCapabilities, cancellationToken).ConfigureAwait(false);
      await VerifyLinkParentAsync(linkParentOptions, providerCapabilities, cancellationToken).ConfigureAwait(false);

      return new ScenarioBenchmarkResult(
          elapsed,
          CreatePersistedOutcome(runResults),
          CreateCompletedExecutionDetail(runResults));
    }
    finally {
      await CleanupOrdinaryAsync(ordinaryDatabase, ordinaryOptions, providerCapabilities).ConfigureAwait(false);
      await CleanupMultiActiveAsync(multiActiveDatabase, multiActiveOptions, providerCapabilities).ConfigureAwait(false);
      await CleanupLinkParentAsync(linkParentDatabase, linkParentOptions, providerCapabilities).ConfigureAwait(false);
    }
  }

  private async Task SeedOrdinaryAsync(
      IBenchmarkDatabase database,
      DbContextOptions<PitFullRebuildOrdinaryContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      CancellationToken cancellationToken) {
    await using (var context = new PitFullRebuildOrdinaryContext(options, providerCapabilities)) {
      await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
      await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
    }

    string customerHashKey;
    await using (var context = new PitFullRebuildOrdinaryContext(options, providerCapabilities)) {
      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              OrdinaryStatusTimestamp,
              "pit-maintenance-ordinary-hub",
              [new(OrdinaryMetadata.Customer, [new("Customer Id", "C-PIT-ORDINARY")])],
              []),
          cancellationToken).ConfigureAwait(false);
      customerHashKey = DataVaultBenchmarkHelpers.GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(
              [
                  new DataVaultSaveRequest(
                      OrdinaryStatusTimestamp,
                      "pit-maintenance-ordinary-status",
                      [],
                      [],
                      [
                          new DataVaultSatelliteSaveOperation(
                              OrdinaryMetadata.Status,
                              customerHashKey,
                              [new("Status Code", "Active")],
                              "ordinary-status-1"),
                      ]),
                  new DataVaultSaveRequest(
                      OrdinaryProfileTimestamp,
                      "pit-maintenance-ordinary-profile",
                      [],
                      [],
                      [
                          new DataVaultSatelliteSaveOperation(
                              OrdinaryMetadata.Profile,
                              customerHashKey,
                              [new("Customer Name", "Ada Ordinary"), new("Customer Tier", "Gold")],
                              "ordinary-profile-1"),
                      ]),
                  new DataVaultSaveRequest(
                      OrdinarySecondStatusTimestamp,
                      "pit-maintenance-ordinary-status",
                      [],
                      [],
                      [
                          new DataVaultSatelliteSaveOperation(
                              OrdinaryMetadata.Status,
                              customerHashKey,
                              [new("Status Code", "Preferred")],
                              "ordinary-status-2"),
                      ]),
              ]),
          cancellationToken).ConfigureAwait(false);
    }

    await using (var context = new PitFullRebuildOrdinaryContext(options, providerCapabilities)) {
      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["CustomerHashKey"] = customerHashKey,
        ["LoadTimestamp"] = DataVaultBenchmarkHelpers.ToStoredTimestamp(
            providerCapabilities,
            DataVaultLogicalPropertyKind.LoadTimestamp,
            Utc(2026, 5, 21, 8, 30)),
        ["ProfileLoadTimestamp"] = null!,
        ["StatusLoadTimestamp"] = null!,
      });
      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
  }

  private async Task SeedMultiActiveAsync(
      IBenchmarkDatabase database,
      DbContextOptions<PitFullRebuildMultiActiveContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      CancellationToken cancellationToken) {
    await using (var context = new PitFullRebuildMultiActiveContext(options, providerCapabilities)) {
      await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
      await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
    }

    await using var seedContext = new PitFullRebuildMultiActiveContext(options, providerCapabilities);
    var hubResult = await saveService.SaveAsync(
        seedContext,
        new DataVaultSaveRequest(
            MultiActiveProfileBeforeTupleTimestamp,
            "pit-maintenance-multi-active-hub",
            [new(MultiActiveMetadata.Customer, [new("Customer Id", "C-PIT-MULTI-ACTIVE")])],
            []),
        cancellationToken).ConfigureAwait(false);
    var customerHashKey = DataVaultBenchmarkHelpers.GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

    await saveService.SaveAsync(
        seedContext,
        new DataVaultBulkSaveRequest(
            [
                new DataVaultSaveRequest(
                    MultiActiveProfileBeforeTupleTimestamp,
                    "pit-maintenance-multi-active-profile",
                    [],
                    [],
                    [
                        new DataVaultSatelliteSaveOperation(
                            MultiActiveMetadata.Profile,
                            customerHashKey,
                            [new("Customer Name", "Morgan Multi"), new("Customer Tier", "Silver")],
                            "multi-profile-before"),
                    ]),
                new DataVaultSaveRequest(
                    MultiActiveBillingContactTimestamp,
                    "pit-maintenance-multi-active-contact",
                    [],
                    [],
                    [
                        new DataVaultSatelliteSaveOperation(
                            MultiActiveMetadata.Contact,
                            customerHashKey,
                            [new("Contact Type", "billing")],
                            [new("Email Address", "billing@example.test")],
                            "multi-contact-billing"),
                    ]),
                new DataVaultSaveRequest(
                    MultiActiveShippingContactTimestamp,
                    "pit-maintenance-multi-active-contact",
                    [],
                    [],
                    [
                        new DataVaultSatelliteSaveOperation(
                            MultiActiveMetadata.Contact,
                            customerHashKey,
                            [new("Contact Type", "shipping")],
                            [new("Email Address", "shipping@example.test")],
                            "multi-contact-shipping"),
                    ]),
                new DataVaultSaveRequest(
                    MultiActiveProfileAfterTupleTimestamp,
                    "pit-maintenance-multi-active-profile",
                    [],
                    [],
                    [
                        new DataVaultSatelliteSaveOperation(
                            MultiActiveMetadata.Profile,
                            customerHashKey,
                            [new("Customer Name", "Morgan Final"), new("Customer Tier", "Gold")],
                            "multi-profile-after"),
                    ]),
            ]),
        cancellationToken).ConfigureAwait(false);
  }

  private async Task SeedLinkParentAsync(
      IBenchmarkDatabase database,
      DbContextOptions<PitFullRebuildLinkParentContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultSaveService saveService,
      CancellationToken cancellationToken) {
    await using (var context = new PitFullRebuildLinkParentContext(options, providerCapabilities)) {
      await database.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
      await database.EnsureCreatedAsync(context, cancellationToken).ConfigureAwait(false);
    }

    string linkHashKey;
    await using (var context = new PitFullRebuildLinkParentContext(options, providerCapabilities)) {
      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              LinkStateTimestamp,
              "pit-maintenance-link-hubs",
              [
                  new(LinkParentMetadata.Customer, [new("Customer Id", "C-PIT-LINK")]),
                  new(LinkParentMetadata.Order, [new("Order Id", "O-PIT-LINK")]),
              ],
              []),
          cancellationToken).ConfigureAwait(false);
      var customerHashKey = DataVaultBenchmarkHelpers.GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");
      var orderHashKey = DataVaultBenchmarkHelpers.GetHashKey(hubResult, DataVaultTableKind.Hub, "Order");
      var linkResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              LinkStateTimestamp,
              "pit-maintenance-link-parent",
              [],
              [
                  new(
                      LinkParentMetadata.CustomerOrder,
                      [new("Customer", customerHashKey), new("Order", orderHashKey)]),
              ]),
          cancellationToken).ConfigureAwait(false);
      linkHashKey = DataVaultBenchmarkHelpers.GetHashKey(linkResult, DataVaultTableKind.Link, "CustomerOrder");

      await saveService.SaveAsync(
          context,
          new DataVaultBulkSaveRequest(
              [
                  new DataVaultSaveRequest(
                      LinkStateTimestamp,
                      "pit-maintenance-link-state",
                      [],
                      [],
                      [
                          new DataVaultSatelliteSaveOperation(
                              LinkParentMetadata.State,
                              linkHashKey,
                              [new("State Code", "Packed")],
                              "link-state-1"),
                      ]),
                  new DataVaultSaveRequest(
                      LinkFulfillmentTimestamp,
                      "pit-maintenance-link-fulfillment",
                      [],
                      [],
                      [
                          new DataVaultSatelliteSaveOperation(
                              LinkParentMetadata.Fulfillment,
                              linkHashKey,
                              [new("Fulfillment Location", "Dock 12")],
                              "link-fulfillment-1"),
                      ]),
                  new DataVaultSaveRequest(
                      LinkSecondStateTimestamp,
                      "pit-maintenance-link-state",
                      [],
                      [],
                      [
                          new DataVaultSatelliteSaveOperation(
                              LinkParentMetadata.State,
                              linkHashKey,
                              [new("State Code", "Shipped")],
                              "link-state-2"),
                      ]),
              ]),
          cancellationToken).ConfigureAwait(false);
    }

    await using (var context = new PitFullRebuildLinkParentContext(options, providerCapabilities)) {
      context.Set<Dictionary<string, object>>("PitCustomerOrderStateFulfillment").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
        ["CustomerOrderHashKey"] = linkHashKey,
        ["LoadTimestamp"] = DataVaultBenchmarkHelpers.ToStoredTimestamp(
            providerCapabilities,
            DataVaultLogicalPropertyKind.LoadTimestamp,
            Utc(2026, 5, 23, 8, 30)),
        ["StateLoadTimestamp"] = null!,
        ["FulfillmentLoadTimestamp"] = null!,
      });
      await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
  }

  private async Task<PitMaintenanceShapeRunResult> RebuildOrdinaryAsync(
      DbContextOptions<PitFullRebuildOrdinaryContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultPitMaintenanceService maintenanceService,
      IReadOnlyList<IDataVaultProviderPitMaintenanceStrategy> strategies,
      CancellationToken cancellationToken) {
    await using var context = new PitFullRebuildOrdinaryContext(options, providerCapabilities);
    var request = new DataVaultPitRebuildRequest(OrdinaryMetadata.Pit);
    AssertPostgresStrategySelection(context, request, strategies);

    var result = await maintenanceService.RebuildAsync(context, request, cancellationToken).ConfigureAwait(false);

    BenchmarkAssert.Equal(1, result.ParentHashKeyCount, "The ordinary PIT maintenance row must rebuild one parent hash key.");
    BenchmarkAssert.Equal(1, result.RowsDeleted, "The ordinary PIT maintenance row must delete the stale full-rebuild row.");
    BenchmarkAssert.Equal(3, result.RowsWritten, "The ordinary PIT maintenance row count drifted.");

    return new PitMaintenanceShapeRunResult("ordinary-hub-parent", result);
  }

  private async Task<PitMaintenanceShapeRunResult> RebuildMultiActiveAsync(
      DbContextOptions<PitFullRebuildMultiActiveContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultPitMaintenanceService maintenanceService,
      IReadOnlyList<IDataVaultProviderPitMaintenanceStrategy> strategies,
      CancellationToken cancellationToken) {
    await using var context = new PitFullRebuildMultiActiveContext(options, providerCapabilities);
    var request = new DataVaultPitRebuildRequest(MultiActiveMetadata.Pit);
    AssertPostgresStrategySelection(context, request, strategies);

    var result = await maintenanceService.RebuildAsync(context, request, cancellationToken).ConfigureAwait(false);

    BenchmarkAssert.Equal(1, result.ParentHashKeyCount, "The multi-active PIT maintenance row must rebuild one parent hash key.");
    BenchmarkAssert.Equal(0, result.RowsDeleted, "The multi-active PIT maintenance row must start from an empty PIT table.");
    BenchmarkAssert.Equal(4, result.RowsWritten, "The multi-active PIT maintenance row count drifted.");

    return new PitMaintenanceShapeRunResult("shared-driving-key-multi-active-hub-parent", result);
  }

  private async Task<PitMaintenanceShapeRunResult> RebuildLinkParentAsync(
      DbContextOptions<PitFullRebuildLinkParentContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      IDataVaultPitMaintenanceService maintenanceService,
      IReadOnlyList<IDataVaultProviderPitMaintenanceStrategy> strategies,
      CancellationToken cancellationToken) {
    await using var context = new PitFullRebuildLinkParentContext(options, providerCapabilities);
    var request = new DataVaultPitRebuildRequest(LinkParentMetadata.Pit);
    AssertPostgresStrategySelection(context, request, strategies);

    var result = await maintenanceService.RebuildAsync(context, request, cancellationToken).ConfigureAwait(false);

    BenchmarkAssert.Equal(1, result.ParentHashKeyCount, "The link-parent PIT maintenance row must rebuild one link hash key.");
    BenchmarkAssert.Equal(1, result.RowsDeleted, "The link-parent PIT maintenance row must delete the stale full-rebuild row.");
    BenchmarkAssert.Equal(3, result.RowsWritten, "The link-parent PIT maintenance row count drifted.");

    return new PitMaintenanceShapeRunResult("link-parent-non-multi-active", result);
  }

  private void AssertPostgresStrategySelection(
      DbContext context,
      DataVaultPitRebuildRequest request,
      IReadOnlyList<IDataVaultProviderPitMaintenanceStrategy> strategies) {
    if (_strategy == DataVaultBenchmarkStrategy.ProviderNeutralFallback) {
      BenchmarkAssert.Equal(
          0,
          strategies.Count,
          "The provider-neutral PIT maintenance comparator must not register provider-specific maintenance strategies.");
      return;
    }

    var acceptedStrategies = strategies
        .Where(strategy => strategy.CanRebuild(context, request))
        .Select(strategy => strategy.GetType().Name)
        .ToArray();
    var strategyName = BenchmarkAssert.Single(
        acceptedStrategies,
        "The PostgreSQL PIT maintenance benchmark row must select exactly one provider strategy.");
    BenchmarkAssert.Equal(
        PostgresPitMaintenanceStrategyName,
        strategyName,
        "The PostgreSQL PIT maintenance benchmark row selected the wrong provider strategy.");

    var evaluation = DataVaultProviderPitMaintenanceStrategyGateEvaluator.EvaluatePostgres(context, request);
    BenchmarkAssert.True(
        evaluation.CanRebuild,
        "The PostgreSQL PIT maintenance benchmark row must pass the provider maintenance gate.");
    BenchmarkAssert.Equal(
        0,
        evaluation.FallbackCauses.Count,
        "The PostgreSQL PIT maintenance benchmark row must not report fallback causes for supported shapes.");
  }

  private static async Task VerifyOrdinaryAsync(
      DbContextOptions<PitFullRebuildOrdinaryContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new PitFullRebuildOrdinaryContext(options, providerCapabilities);
    var rows = await context.Set<Dictionary<string, object>>("PitCustomerProfileStatus")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    BenchmarkAssert.Equal(3, rows.Count, "The ordinary PIT maintenance benchmark must rebuild three PIT rows.");
  }

  private static async Task VerifyMultiActiveAsync(
      DbContextOptions<PitFullRebuildMultiActiveContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new PitFullRebuildMultiActiveContext(options, providerCapabilities);
    var rows = await context.Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    BenchmarkAssert.Equal(4, rows.Count, "The multi-active PIT maintenance benchmark must rebuild four PIT rows.");
  }

  private static async Task VerifyLinkParentAsync(
      DbContextOptions<PitFullRebuildLinkParentContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities,
      CancellationToken cancellationToken) {
    await using var context = new PitFullRebuildLinkParentContext(options, providerCapabilities);
    var rows = await context.Set<Dictionary<string, object>>("PitCustomerOrderStateFulfillment")
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    BenchmarkAssert.Equal(3, rows.Count, "The link-parent PIT maintenance benchmark must rebuild three PIT rows.");
  }

  private static async Task CleanupOrdinaryAsync(
      IBenchmarkDatabase database,
      DbContextOptions<PitFullRebuildOrdinaryContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    await using var context = new PitFullRebuildOrdinaryContext(options, providerCapabilities);
    await database.CleanupAsync(context, CancellationToken.None).ConfigureAwait(false);
  }

  private static async Task CleanupMultiActiveAsync(
      IBenchmarkDatabase database,
      DbContextOptions<PitFullRebuildMultiActiveContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    await using var context = new PitFullRebuildMultiActiveContext(options, providerCapabilities);
    await database.CleanupAsync(context, CancellationToken.None).ConfigureAwait(false);
  }

  private static async Task CleanupLinkParentAsync(
      IBenchmarkDatabase database,
      DbContextOptions<PitFullRebuildLinkParentContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) {
    await using var context = new PitFullRebuildLinkParentContext(options, providerCapabilities);
    await database.CleanupAsync(context, CancellationToken.None).ConfigureAwait(false);
  }

  private string CreateCompletedExecutionDetail(
      string? providerName,
      string selectedMaintenanceServiceName,
      DataVaultPitMaintenanceResult result) {
    var selectedStrategy = _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
        ? selectedMaintenanceServiceName
        : "<none>";
    var strategyStatus = _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
        ? "ProviderStrategySelected"
        : "ProviderNeutralFallback";
    var fallbackCauses = _strategy == DataVaultBenchmarkStrategy.SqlServerOptimized
        ? "none"
        : "NoProviderSpecificStrategyRegistered";

    return BenchmarkExecutionDetails.CreatePlanned(this) +
        "; maintenanceStrategyStatus=" + strategyStatus +
        "; provider=" + (providerName ?? "<none>") +
        "; maintenanceService=" + selectedMaintenanceServiceName +
        "; selectedStrategy=" + selectedStrategy +
        "; fallbackCauses=" + fallbackCauses +
        "; parentHashKeys=" + result.ParentHashKeyCount.ToString(CultureInfo.InvariantCulture) +
        "; rowsDeleted=" + result.RowsDeleted.ToString(CultureInfo.InvariantCulture) +
        "; rowsWritten=" + result.RowsWritten.ToString(CultureInfo.InvariantCulture);
  }

  private string CreateCompletedExecutionDetail(IReadOnlyList<PitMaintenanceShapeRunResult> runResults) {
    return BenchmarkExecutionDetails.CreatePlanned(this) +
        "; pitMaintenanceStrategyStatus=" +
        (_strategy == DataVaultBenchmarkStrategy.PostgresOptimized ? "ProviderStrategySelected" : "ProviderNeutralFallback") +
        "; provider=" +
        KnownProviderNames.Postgres +
        "; selectedStrategy=" +
        (_strategy == DataVaultBenchmarkStrategy.PostgresOptimized ? PostgresPitMaintenanceStrategyName : "<none>") +
        "; candidates=" +
        (_strategy == DataVaultBenchmarkStrategy.PostgresOptimized ? "1" : "0") +
        "; fallbackCauses=" +
        (_strategy == DataVaultBenchmarkStrategy.PostgresOptimized
            ? "none"
            : DataVaultPitMaintenanceStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered.ToString()) +
        "; shapeCount=" +
        runResults.Count.ToString(CultureInfo.InvariantCulture) +
        "; parentHashKeys=" +
        runResults.Sum(result => result.Result.ParentHashKeyCount).ToString(CultureInfo.InvariantCulture) +
        "; rowsDeleted=" +
        runResults.Sum(result => result.Result.RowsDeleted).ToString(CultureInfo.InvariantCulture) +
        "; rowsWritten=" +
        runResults.Sum(result => result.Result.RowsWritten).ToString(CultureInfo.InvariantCulture);
  }

  private static string CreatePersistedOutcome(IReadOnlyList<PitMaintenanceShapeRunResult> runResults) {
    return string.Join(
        "; ",
        runResults.Select(result =>
            result.ShapeName +
            " wrote " +
            result.Result.RowsWritten.ToString(CultureInfo.InvariantCulture) +
            " PIT rows")) +
        "; " +
        runResults.Sum(result => result.Result.RowsWritten).ToString(CultureInfo.InvariantCulture) +
        " total PIT rows rebuilt";
  }

  private static OrdinaryPitMaintenanceMetadata CreateOrdinaryMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new OrdinaryPitMaintenanceMetadata(customer, profile, status, pit, model);
  }

  private static MultiActivePitMaintenanceMetadata CreateMultiActiveMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name", "Customer Tier"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
        ]);
    var model = new DataVaultMetadataModel([customer], [], [profile, contact], [pit]);

    return new MultiActivePitMaintenanceMetadata(customer, profile, contact, pit, model);
  }

  private static LinkParentPitMaintenanceMetadata CreateLinkParentMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var order = new DataVaultHubMetadata("Order", ["Order Id"]);
    var customerOrder = new DataVaultLinkMetadata("CustomerOrder", [customer.ToReference(), order.ToReference()]);
    var state = new DataVaultSatelliteMetadata(
        "State",
        customerOrder.ToReference(),
        ["State Code"]);
    var fulfillment = new DataVaultSatelliteMetadata(
        "Fulfillment",
        customerOrder.ToReference(),
        ["Fulfillment Location"]);
    var pit = new DataVaultPitMetadata(customerOrder.ToReference(), ["State", "Fulfillment"]);
    var model = new DataVaultMetadataModel([customer, order], [customerOrder], [state, fulfillment], [pit]);

    return new LinkParentPitMaintenanceMetadata(customer, order, customerOrder, state, fulfillment, pit, model);
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private static string ReadString(Dictionary<string, object> row, string columnName) {
    return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ??
        throw new InvalidOperationException("Expected column '" + columnName + "' to contain a non-null value.");
  }

  private static bool IsSqlServerProviderName(string providerName) {
    return string.Equals(providerName, BenchmarkExternalProviderDefinitions.SqlServer.ProviderName, StringComparison.Ordinal);
  }

  private static bool IsPostgresProviderName(string providerName) {
    return string.Equals(providerName, BenchmarkExternalProviderDefinitions.Postgres.ProviderName, StringComparison.Ordinal);
  }

  private sealed class PitFullRebuildOrdinaryContext(
      DbContextOptions<PitFullRebuildOrdinaryContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(OrdinaryMetadata.Model, ProviderCapabilities);
    }
  }

  private sealed class PitFullRebuildMultiActiveContext(
      DbContextOptions<PitFullRebuildMultiActiveContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(MultiActiveMetadata.Model, ProviderCapabilities);
    }
  }

  private sealed class PitFullRebuildLinkParentContext(
      DbContextOptions<PitFullRebuildLinkParentContext> options,
      DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
    public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(LinkParentMetadata.Model, ProviderCapabilities);
    }
  }

  private sealed record OrdinaryPitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Status,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record MultiActivePitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Contact,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record LinkParentPitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultHubMetadata Order,
      DataVaultLinkMetadata CustomerOrder,
      DataVaultSatelliteMetadata State,
      DataVaultSatelliteMetadata Fulfillment,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record PitMaintenanceShapeRunResult(
      string ShapeName,
      DataVaultPitMaintenanceResult Result);
}
