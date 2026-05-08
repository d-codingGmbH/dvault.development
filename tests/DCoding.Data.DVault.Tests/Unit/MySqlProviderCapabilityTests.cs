using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class MySqlProviderCapabilityTests {
  [Fact]
  public void AddDVaultMySqlRegistersPomeloGatedOptimizedStrategyAndProviderProfileSelection() {
    try {
      var services = new ServiceCollection();

      services.AddDVaultMySql();

      using var provider = services.BuildServiceProvider(validateScopes: true);
      var strategy = Assert.Single(provider.GetServices<IDataVaultProviderSaveStrategy>());

      Assert.IsType<MySqlDataVaultSaveStrategy>(strategy);
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
