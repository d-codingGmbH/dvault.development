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

    Assert.False(stagedDecline.CanSave);
    Assert.Contains(
        stagedDecline.FallbackCauses,
        cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold &&
            cause.Message.Contains("MySQL staged bulk", StringComparison.Ordinal));
    Assert.True(multiRowAccept.CanSave);
    Assert.True(stagedAccept.CanSave);
  }

  [Fact]
  public void MySqlStagedDiagnosticsDistinguishStagedSelectionFromRetainedMultiRowBoundary() {
    var midSizedBatch = CreateHubRequest(totalOperationCount: 50);
    var stagedBatch = CreateHubRequest(totalOperationCount: MySqlDataVaultSaveStrategy.MinimumStagedBulkOperationCount);

    var midSizedDiagnostics = MySqlStagedDataVaultSaveStrategy.CreateStagedProviderBulkDiagnostics(
        hasPendingTrackedChanges: false,
        midSizedBatch);
    var stagedDiagnostics = MySqlStagedDataVaultSaveStrategy.CreateStagedProviderBulkDiagnostics(
        hasPendingTrackedChanges: false,
        stagedBatch);

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
