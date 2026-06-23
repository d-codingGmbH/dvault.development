using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class MySqlProviderCapabilityTests {
  [Fact]
  public void AddDVaultMySqlRegistersDualProviderStagedAndMultiRowStrategiesAndProviderProfileSelection() {
    try {
      var services = new ServiceCollection();

      services.AddDVaultMySql();

      using var provider = services.BuildServiceProvider(validateScopes: true);
      var strategies = provider.GetServices<IDataVaultProviderSaveStrategy>().ToArray();

      Assert.Contains(strategies, strategy => strategy is MySqlStagedDataVaultSaveStrategy and IDataVaultProviderStagedBulkSaveDiagnostics);
      Assert.Contains(strategies, strategy => strategy is MySqlDataVaultSaveStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderReadStrategy>(), strategy => strategy is MySqlDataVaultReadStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderPitReadStrategy>(), strategy => strategy is MySqlDataVaultReadStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderBridgeReadStrategy>(), strategy => strategy is MySqlDataVaultReadStrategy);
      Assert.Same(
          DataVaultProviderCapabilityProfiles.MySql,
          DataVaultProviderCapabilityProfileSelection.Select(MySqlDataVaultSaveStrategy.PomeloProviderName));
      Assert.Same(
          DataVaultProviderCapabilityProfiles.MySql,
          DataVaultProviderCapabilityProfileSelection.Select("MySql.EntityFrameworkCore"));
      Assert.Same(
          DataVaultProviderCapabilityProfiles.Sqlite,
          DataVaultProviderCapabilityProfileSelection.Select((string?)null));
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  [Fact]
  public void MySqlStrategyAcceptsPomeloAndOfficialOracleProviderNames() {
    Assert.True(MySqlDataVaultSaveStrategy.IsSupportedProviderName("Pomelo.EntityFrameworkCore.MySql"));
    Assert.True(MySqlDataVaultSaveStrategy.IsSupportedProviderName("MySql.EntityFrameworkCore"));
    Assert.False(MySqlDataVaultSaveStrategy.IsSupportedProviderName("Microsoft.EntityFrameworkCore.Sqlite"));
    Assert.False(MySqlDataVaultSaveStrategy.IsSupportedProviderName(null));
  }

  [Fact]
  public void MySqlGateKeepsMultiRowBoundaryBelowStagedBulkBoundary() {
    var midSizedBatch = CreateHubRequest(totalOperationCount: 50);
    var stagedBatch = CreateHubRequest(totalOperationCount: MySqlDataVaultSaveStrategy.MinimumStagedBulkOperationCount);
    var largeHubBatch = CreateHubRequest(totalOperationCount: MySqlDataVaultSaveStrategy.MaximumStagedBulkMixedOperationCount + 1);

    var stagedDecline = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        midSizedBatch);
    var multiRowAccept = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        midSizedBatch);
    var stagedAccept = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        stagedBatch);
    var largeStagedDecline = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        largeHubBatch);
    var largeMultiRowDecline = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        largeHubBatch);

    Assert.False(stagedDecline.CanSave);
    Assert.Contains(
        stagedDecline.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold &&
            cause.Message.Contains("MySQL staged bulk", StringComparison.Ordinal));
    Assert.True(multiRowAccept.CanSave);
    Assert.True(stagedAccept.CanSave);
    Assert.False(largeStagedDecline.CanSave);
    Assert.Contains(
        largeStagedDecline.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape &&
            cause.Message.Contains(
                MySqlDataVaultSaveStrategy.MaximumStagedBulkMixedOperationCount.ToString(CultureInfo.InvariantCulture),
                StringComparison.Ordinal));
    Assert.False(largeMultiRowDecline.CanSave);
    Assert.Contains(
        largeMultiRowDecline.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlLargeMixedProviderNeutralFallback &&
            cause.Message.Contains(
                MySqlDataVaultSaveStrategy.MaximumStagedBulkMixedOperationCount.ToString(CultureInfo.InvariantCulture),
                StringComparison.Ordinal));
    Assert.False(MySqlDataVaultSaveStrategy.IsOptimizedBatchShape(largeHubBatch));
  }

  [Fact]
  public void MySqlGateDeliberatelyFallsBackForTinySatelliteHistoryBatches() {
    var singleRequestTinyBatch = CreateSatelliteHistoryRequests(requestCount: 1, satelliteOperationsPerRequest: 10);
    var tinyHistoryBatch = CreateSatelliteHistoryRequests(requestCount: 10, satelliteOperationsPerRequest: 10);
    var retainedMultiRowBatch = CreateSatelliteHistoryRequests(requestCount: 1, satelliteOperationsPerRequest: 50);

    var singleRequestTinyGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        singleRequestTinyBatch);
    var singleRequestStagedGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        singleRequestTinyBatch);
    var multiRowGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        tinyHistoryBatch);
    var stagedGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        tinyHistoryBatch);
    var stagedDiagnostics = MySqlStagedDataVaultSaveStrategy.CreateStagedProviderBulkDiagnostics(
        hasPendingTrackedChanges: false,
        tinyHistoryBatch);
    var retainedMultiRowGate = DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySql(
        KnownProviderNames.MySqlOracle,
        hasPendingTrackedChanges: false,
        retainedMultiRowBatch);

    Assert.False(singleRequestTinyGate.CanSave);
    Assert.Contains(
        singleRequestTinyGate.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold);
    Assert.Contains(
        singleRequestTinyGate.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback);
    Assert.False(singleRequestStagedGate.CanSave);
    Assert.Contains(
        singleRequestStagedGate.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback);
    Assert.False(multiRowGate.CanSave);
    Assert.Contains(
        multiRowGate.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback);
    Assert.False(stagedGate.CanSave);
    Assert.Contains(
        stagedGate.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback);
    Assert.False(MySqlDataVaultSaveStrategy.IsOptimizedBatchShape(singleRequestTinyBatch));
    Assert.False(MySqlDataVaultSaveStrategy.IsOptimizedBatchShape(tinyHistoryBatch));
    Assert.True(retainedMultiRowGate.CanSave);
    Assert.True(MySqlDataVaultSaveStrategy.IsOptimizedBatchShape(retainedMultiRowBatch));

    Assert.Equal(DataVaultStagedProviderBulkLifecyclePhase.Declined, stagedDiagnostics.LifecyclePhase);
    Assert.Equal(DataVaultStagedProviderBulkProviderCaveatKind.ProviderLimitation, stagedDiagnostics.ProviderCaveatKind);
    Assert.Equal(100, stagedDiagnostics.OperationCount);
    Assert.Contains(
        stagedDiagnostics.FallbackCauseKinds,
        cause => cause == DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkProviderLimitation);
  }

  [Fact]
  public void MySqlStagedDiagnosticsDistinguishStagedSelectionFromRetainedMultiRowBoundary() {
    var midSizedBatch = CreateHubRequest(totalOperationCount: 50);
    var stagedBatch = CreateHubRequest(totalOperationCount: MySqlDataVaultSaveStrategy.MinimumStagedBulkOperationCount);
    var largeHubBatch = CreateHubRequest(totalOperationCount: MySqlDataVaultSaveStrategy.MaximumStagedBulkMixedOperationCount + 1);

    var midSizedDiagnostics = MySqlStagedDataVaultSaveStrategy.CreateStagedProviderBulkDiagnostics(
        hasPendingTrackedChanges: false,
        midSizedBatch);
    var stagedDiagnostics = MySqlStagedDataVaultSaveStrategy.CreateStagedProviderBulkDiagnostics(
        hasPendingTrackedChanges: false,
        stagedBatch);
    var largeDiagnostics = MySqlStagedDataVaultSaveStrategy.CreateStagedProviderBulkDiagnostics(
        hasPendingTrackedChanges: false,
        largeHubBatch);

    Assert.Equal(DataVaultStagedProviderBulkLifecyclePhase.Declined, midSizedDiagnostics.LifecyclePhase);
    Assert.Equal(DataVaultStagedProviderBulkProviderCaveatKind.UnsupportedShape, midSizedDiagnostics.ProviderCaveatKind);
    Assert.Equal(50, midSizedDiagnostics.OperationCount);
    Assert.Contains(
        midSizedDiagnostics.FallbackCauseKinds,
        cause => cause == DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape);

    Assert.Equal(DataVaultStagedProviderBulkLifecyclePhase.NativeBulkApplication, stagedDiagnostics.LifecyclePhase);
    Assert.Equal(DataVaultStagedProviderBulkProviderCaveatKind.None, stagedDiagnostics.ProviderCaveatKind);
    Assert.Equal(MySqlDataVaultSaveStrategy.MinimumStagedBulkOperationCount, stagedDiagnostics.OperationCount);
    Assert.Empty(stagedDiagnostics.FallbackCauseKinds);

    Assert.Equal(DataVaultStagedProviderBulkLifecyclePhase.Declined, largeDiagnostics.LifecyclePhase);
    Assert.Equal(DataVaultStagedProviderBulkProviderCaveatKind.UnsupportedShape, largeDiagnostics.ProviderCaveatKind);
    Assert.Equal(MySqlDataVaultSaveStrategy.MaximumStagedBulkMixedOperationCount + 1, largeDiagnostics.OperationCount);
    Assert.Contains(
        largeDiagnostics.FallbackCauseKinds,
        cause => cause == DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape);
  }

  [Fact]
  public void OfficialMySqlProviderNameResolvesBuiltInMySqlProfileWithoutStartupRegistration() {
    DataVaultProviderCapabilityProfileSelection.Reset();

    Assert.Same(
        DataVaultProviderCapabilityProfiles.MySql,
        DataVaultProviderCapabilityProfileSelection.Select("MySql.EntityFrameworkCore"));
  }

  [Fact]
  public void MySqlStrategyBuildsParameterizedMySqlInsertSqlInsideProviderPackage() {
    var ignoreCommandText = MySqlDataVaultSaveStrategy.CreateMySqlInsertCommandText(
        "Hub`Customer",
        ["CustomerHashKey", "LoadTimestamp"],
        rowCount: 2,
        MySqlInsertConflictBehavior.Ignore);
    var failCommandText = MySqlDataVaultSaveStrategy.CreateMySqlInsertCommandText(
        "SatCustomerProfile",
        ["CustomerHashKey", "HashDiff"],
        rowCount: 1,
        MySqlInsertConflictBehavior.Fail);

    Assert.Equal(
        "INSERT IGNORE INTO `Hub``Customer` (`CustomerHashKey`, `LoadTimestamp`) VALUES (@p0, @p1), (@p2, @p3)",
        ignoreCommandText);
    Assert.Equal(
        "INSERT INTO `SatCustomerProfile` (`CustomerHashKey`, `HashDiff`) VALUES (@p0, @p1)",
        failCommandText);
  }

  [Fact]
  public void MySqlStrategyBuildsStagedInsertSqlInsideProviderPackage() {
    var createCommandText = MySqlDataVaultSaveStrategy.CreateMySqlCreateStagingTableCommandText(
        "__dvault_stage`1",
        "Hub`Customer",
        ["CustomerHashKey", "LoadTimestamp"]);
    var insertCommandText = MySqlDataVaultSaveStrategy.CreateMySqlInsertFromStagingCommandText(
        "Hub`Customer",
        "__dvault_stage`1",
        ["CustomerHashKey", "LoadTimestamp"],
        MySqlInsertConflictBehavior.Ignore);
    var dropCommandText = MySqlDataVaultSaveStrategy.CreateMySqlDropTemporaryTableCommandText("__dvault_stage`1");

    Assert.Equal(
        "CREATE TEMPORARY TABLE `__dvault_stage``1` AS SELECT `CustomerHashKey`, `LoadTimestamp` " +
        "FROM `Hub``Customer` WHERE 1 = 0",
        createCommandText);
    Assert.Equal(
        "INSERT IGNORE INTO `Hub``Customer` (`CustomerHashKey`, `LoadTimestamp`) " +
        "SELECT `CustomerHashKey`, `LoadTimestamp` FROM `__dvault_stage``1`",
        insertCommandText);
    Assert.Equal(
        "DROP TEMPORARY TABLE IF EXISTS `__dvault_stage``1`",
        dropCommandText);
  }

  [Fact]
  public void MySqlStrategyUsesWindowFunctionForLatestSatelliteHashDiffLookup() {
    var commandText = MySqlDataVaultSaveStrategy.CreateLatestSatelliteHashDiffsCommandText(
        "Sat`CustomerProfile",
        "CustomerHashKey",
        "HashDiff",
        "LoadTimestamp",
        ["@p0", "@p1"]);

    Assert.Equal(
        "SELECT `CustomerHashKey`, `HashDiff`, `LoadTimestamp` FROM " +
        "(SELECT `CustomerHashKey`, `HashDiff`, `LoadTimestamp`, " +
        "ROW_NUMBER() OVER (PARTITION BY `CustomerHashKey` ORDER BY `LoadTimestamp` DESC) AS `__dvault_row_number` " +
        "FROM `Sat``CustomerProfile` WHERE `CustomerHashKey` IN (@p0, @p1)) AS `__dvault_latest` " +
        "WHERE `__dvault_row_number` = 1",
        commandText);
  }

  [Fact]
  public void MySqlReadStrategyBuildsWindowFunctionForLatestSatelliteRows() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Name"]);
    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(profile);
    var strategy = new MySqlDataVaultReadStrategy();
    using var context = new DbContext(new DbContextOptionsBuilder().Options);

    var commandText = strategy.CreateLatestRowsCommandText(
        context,
        projection,
        ["CustomerHashKey", "HashDiff", "LoadTimestamp", "RecordSource", "Name"],
        parentHashKeyCount: 2,
        hasAsOf: true);

    Assert.Equal(
        "SELECT `CustomerHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, `Name` FROM " +
        "(SELECT `CustomerHashKey`, `HashDiff`, `LoadTimestamp`, `RecordSource`, `Name`, " +
        "ROW_NUMBER() OVER (PARTITION BY `CustomerHashKey` ORDER BY `LoadTimestamp` DESC) AS `__dvault_row_number` " +
        "FROM `SatCustomerProfile` WHERE `CustomerHashKey` IN (@p0, @p1) AND `LoadTimestamp` <= @p2) " +
        "AS `__dvault_latest` WHERE `__dvault_row_number` = 1 ORDER BY `CustomerHashKey`",
        commandText);
  }

  [Fact]
  public void AddDVaultMySqlDoesNotSwitchBareModelBuildersWithoutPomeloProviderEvidence() {
    try {
      var services = new ServiceCollection();
      services.AddDVaultMySql();

      var modelBuilder = new ModelBuilder(new ConventionSet());
      modelBuilder.ApplyDataVaultMetadata(CreateMetadataModel());

      var properties = modelBuilder.Model
          .GetEntityTypes()
          .SelectMany(entity => entity.GetProperties())
          .ToArray();

      Assert.NotEmpty(properties);
      Assert.All(properties, property =>
          Assert.Equal("sqlite-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile)));
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  [Fact]
  public void PomeloProviderSelectionMakesTranslatorEmitMySqlProfileAnnotations() {
    try {
      var services = new ServiceCollection();
      services.AddDVaultMySql();

      var modelBuilder = new ModelBuilder(new ConventionSet());
      var providerCapabilities = DataVaultProviderCapabilityProfileSelection.Select(MySqlDataVaultSaveStrategy.PomeloProviderName);

      DataVaultEfMetadataTranslator.Apply(modelBuilder, CreateMetadataModel(), providerCapabilities);

      var properties = modelBuilder.Model
          .GetEntityTypes()
          .SelectMany(entity => entity.GetProperties())
          .ToArray();

      Assert.NotEmpty(properties);
      Assert.All(properties, property =>
          Assert.Equal("mysql-pomelo-v1", AnnotationValue<string>(property, DataVaultAnnotationNames.ProviderProfile)));
      Assert.Equal(
          "varchar(33)",
          AnnotationValue<string>(FindProperty(modelBuilder.Model, "HubCustomer", "LoadTimestamp"), DataVaultAnnotationNames.ProviderStorageType));
      Assert.Equal(
          "longtext",
          AnnotationValue<string>(FindProperty(modelBuilder.Model, "SatCustomerContact", "EmailAddress"), DataVaultAnnotationNames.ProviderStorageType));
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  private static DataVaultMetadataModel CreateMetadataModel() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return new DataVaultMetadataModel(
        [customer],
        [],
        [
            new DataVaultSatelliteMetadata(
                "Contact",
                customer.ToReference(),
                ["Email Address"]),
        ]);
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateHubRequest(int totalOperationCount) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);

    return
    [
        new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 26, 0, 0, 0, TimeSpan.Zero),
            "mysql-gate-test",
            Enumerable.Range(0, totalOperationCount)
                .Select(index => new DataVaultHubSaveOperation(
                    customer,
                    [new("Customer Id", "C-" + index.ToString("000", CultureInfo.InvariantCulture))]))
                .ToArray(),
            []),
    ];
  }

  private static IReadOnlyList<DataVaultSaveRequest> CreateSatelliteHistoryRequests(
      int requestCount,
      int satelliteOperationsPerRequest) {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Profile Status"]);

    return Enumerable.Range(0, requestCount)
        .Select(requestIndex => new DataVaultSaveRequest(
            new DateTimeOffset(2026, 5, 26, 0, requestIndex, 0, TimeSpan.Zero),
            "mysql-history-gate-test",
            [],
            [],
            Enumerable.Range(0, satelliteOperationsPerRequest)
                .Select(satelliteIndex => new DataVaultSatelliteSaveOperation(
                    profile,
                    "customer-hash-" + satelliteIndex.ToString("000", CultureInfo.InvariantCulture),
                    [new("Profile Status", "active")],
                    "profile-hash-" + requestIndex.ToString("000", CultureInfo.InvariantCulture) + "-" +
                    satelliteIndex.ToString("000", CultureInfo.InvariantCulture)))
                .ToArray()))
        .ToArray();
  }

  private static IMutableProperty FindProperty(IMutableModel model, string entityName, string propertyName) {
    var entityType = model.GetEntityTypes().Single(entity =>
        string.Equals(
            entity.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string,
            entityName,
            StringComparison.Ordinal));
    var property = entityType.FindProperty(propertyName);

    Assert.NotNull(property);

    return property!;
  }

  private static T AnnotationValue<T>(IMutableProperty property, string name) {
    var annotation = property.FindAnnotation(name);

    Assert.NotNull(annotation);

    return Assert.IsType<T>(annotation!.Value);
  }
}
