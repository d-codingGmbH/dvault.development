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
      var pitMaintenanceStrategies = provider.GetServices<IDataVaultProviderPitMaintenanceStrategy>().ToArray();

      Assert.Contains(strategies, strategy => strategy is MySqlStagedDataVaultSaveStrategy and IDataVaultProviderStagedBulkSaveDiagnostics);
      Assert.Contains(strategies, strategy => strategy is MySqlDataVaultSaveStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderReadStrategy>(), strategy => strategy is MySqlDataVaultReadStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderPitReadStrategy>(), strategy => strategy is MySqlDataVaultReadStrategy);
      Assert.Contains(provider.GetServices<IDataVaultProviderBridgeReadStrategy>(), strategy => strategy is MySqlDataVaultReadStrategy);
      Assert.Contains(pitMaintenanceStrategies, strategy => strategy is MySqlDataVaultPitMaintenanceStrategy);
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
  public async Task AddDVaultMySqlKeepsDefaultPitMaintenanceServiceAndProviderNeutralParentMaintenance() {
    try {
      using var provider = new ServiceCollection()
          .AddDVaultMySql()
          .BuildServiceProvider(validateScopes: true);
      var maintenanceService = Assert.IsType<DefaultDataVaultPitMaintenanceService>(
          provider.GetRequiredService<IDataVaultPitMaintenanceService>());
      await using var context = new DbContext(new DbContextOptionsBuilder().Options);
      var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

      var result = await maintenanceService.MaintainParentsAsync(
          context,
          new DataVaultPitParentMaintenanceRequest(pit, []));

      Assert.Equal("PitCustomerProfile", result.TableName);
      Assert.True(result.IsNoOp);
    }
    finally {
      DataVaultProviderCapabilityProfileSelection.Reset();
    }
  }

  [Fact]
  public void MySqlPitMaintenanceGateAcceptsOnlyOfficialOrdinaryHubFullRebuildsAndDeclinesFallbackShapes() {
    var ordinaryHubPit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        ["Profile", "Status"]));
    var multiActiveHubPit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
        ]));
    var linkPit = new DataVaultPitRebuildRequest(new DataVaultPitMetadata(
        DataVaultMetadataReference.Link("CustomerOrder"),
        ["State", "Fulfillment"]));

    Assert.True(DataVaultProviderPitMaintenanceStrategyGateEvaluator
        .EvaluateMySql(KnownProviderNames.MySqlOracle, ordinaryHubPit)
        .CanRebuild);

    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlPomelo, ordinaryHubPit)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.ProviderNameMismatch);
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql("Unknown.Provider", ordinaryHubPit)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName);
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlOracle, ordinaryHubPit, hasPendingTrackedChanges: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.DirtyDbContext);
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlOracle, ordinaryHubPit, hasCompleteMaintenanceShapeEvidence: false)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.IncompleteMaintenanceShapeEvidence);
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql(
                KnownProviderNames.MySqlOracle,
                ordinaryHubPit,
                hasCurrentTransactionWithoutSavepoints: true)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.RollbackSavepointBoundaryUnavailable);
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlOracle, multiActiveHubPit)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape);
    Assert.Contains(
        DataVaultProviderPitMaintenanceStrategyGateEvaluator
            .EvaluateMySql(KnownProviderNames.MySqlOracle, linkPit)
            .FallbackCauses,
        cause => cause.Kind == DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape);
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
  public void MySqlPitMaintenanceStrategyBuildsOrdinaryHubInsertSelectSqlInsideProviderPackage() {
    var metadata = CreatePitMaintenanceMetadata();
    var options = new DbContextOptionsBuilder<MySqlPitMaintenanceSqlContext>()
        .UseSqlite("Data Source=:memory:")
        .Options;
    using var context = new MySqlPitMaintenanceSqlContext(options);
    var projection = DefaultDataVaultPitMaintenanceService.CreatePitProjection(context, metadata.Pit);

    var insertCommandText = MySqlDataVaultPitMaintenanceStrategy.CreateMySqlPitRebuildInsertCommandText(
        context,
        projection);
    var parentCountCommandText = MySqlDataVaultPitMaintenanceStrategy.CreateMySqlPitParentCountCommandText(
        context,
        projection);

    Assert.Contains("INSERT INTO `PitCustomerProfileStatus`", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("`ProfileLoadTimestamp`, `StatusLoadTimestamp`", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("SELECT `source`.`parent_hash_key`, `source`.`load_timestamp`", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("FROM `SatCustomerProfile` AS `satellite_", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("ORDER BY `snapshot_0`.`LoadTimestamp` DESC LIMIT 1", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("ORDER BY `snapshot_1`.`LoadTimestamp` DESC LIMIT 1", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("ORDER BY `source`.`parent_hash_key`, `source`.`load_timestamp`", insertCommandText, StringComparison.Ordinal);
    Assert.Contains("SELECT COUNT(DISTINCT `parents`.`parent_hash_key`) FROM", parentCountCommandText, StringComparison.Ordinal);
    Assert.Contains("UNION", parentCountCommandText, StringComparison.Ordinal);
  }

  [Fact]
  public void MySqlReadStrategyBuildsGroupedJoinForLatestSatelliteRows() {
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
        "SELECT `__dvault_source`.`CustomerHashKey`, `__dvault_source`.`HashDiff`, `__dvault_source`.`LoadTimestamp`, `__dvault_source`.`RecordSource`, `__dvault_source`.`Name` " +
        "FROM `SatCustomerProfile` AS `__dvault_source` " +
        "INNER JOIN (SELECT `CustomerHashKey`, MAX(`LoadTimestamp`) AS `__dvault_latest_load_timestamp` " +
        "FROM `SatCustomerProfile` WHERE `CustomerHashKey` IN (@p0, @p1) AND `LoadTimestamp` <= @p2 " +
        "GROUP BY `CustomerHashKey`) AS `__dvault_latest` " +
        "ON `__dvault_source`.`CustomerHashKey` = `__dvault_latest`.`CustomerHashKey` " +
        "AND `__dvault_source`.`LoadTimestamp` = `__dvault_latest`.`__dvault_latest_load_timestamp` " +
        "ORDER BY `__dvault_source`.`CustomerHashKey`",
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

  private static PitMaintenanceMetadata CreatePitMaintenanceMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name"]);
    var status = new DataVaultSatelliteMetadata(
        "Status",
        customer.ToReference(),
        ["Status Code"]);
    var pit = new DataVaultPitMetadata(customer.ToReference(), ["Profile", "Status"]);
    var model = new DataVaultMetadataModel([customer], [], [profile, status], [pit]);

    return new PitMaintenanceMetadata(pit, model);
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

  private sealed class MySqlPitMaintenanceSqlContext(
      DbContextOptions<MySqlPitMaintenanceSqlContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(CreatePitMaintenanceMetadata().Model);
    }
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);
}
