using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
public sealed class MySqlExplicitDataVaultSaveServiceTests {
  private const string HubName = "DVaultMySqlSmoke";
  private const string HubTableName = "HubDVaultMySqlSmoke";
  private const string BusinessKeyName = "Smoke Id";
  private const string BusinessKeyColumnName = "SmokeId";
  private const string RecordSource = "mysql-smoke";
  private const string FallbackHubName = "DVaultMySqlFallbackSmoke";
  private const string FallbackHubTableName = "HubDVaultMySqlFallbackSmoke";
  private const string FallbackSatelliteName = "FallbackProfile";
  private const string FallbackSatelliteTableName = "SatDVaultMySqlFallbackSmokeFallbackProfile";
  private const string FallbackPayloadName = "Profile Status";
  private const string FallbackPayloadColumnName = "ProfileStatus";

  private static readonly DateTimeOffset LoadTimestamp =
      new(2026, 5, 4, 0, 0, 0, TimeSpan.Zero);

  [Fact]
  public async Task AddDVaultMySqlPersistsExplicitHubSaveWhenConfigured() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var options = CreateMySqlOptions(configuration.ConnectionString!);
    var services = new ServiceCollection();
    services.AddDVaultMySql();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new MySqlExplicitSaveServiceContext(options);
    await DropSmokeTableIfExistsAsync(context);

    try {
      await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());

      var hub = new DataVaultHubMetadata(HubName, [BusinessKeyName]);
      var result = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              LoadTimestamp,
              RecordSource,
              [new DataVaultHubSaveOperation(hub, [new(BusinessKeyName, "MYSQL-C-100")])],
              []));

      Assert.Equal(1, result.RowsWritten);
      var savedRecord = Assert.Single(result.SavedRecords);
      Assert.Equal(DataVaultTableKind.Hub, savedRecord.Kind);
      Assert.Equal(HubName, savedRecord.MetadataName);
      Assert.Equal(HubTableName, savedRecord.TableName);
      Assert.Matches("^[0-9a-f]{64}$", savedRecord.HashKey);

      var row = Assert.Single(
          await context.Set<Dictionary<string, object>>(HubTableName)
              .AsNoTracking()
              .ToListAsync());

      Assert.Equal("MYSQL-C-100", row[BusinessKeyColumnName]);
      Assert.Equal(RecordSource, row["RecordSource"]);
      Assert.Equal(savedRecord.HashKey, row[HubName + "HashKey"]);
    }
    finally {
      await DropSmokeTableIfExistsAsync(context);
    }
  }

  [Fact]
  public async Task ProviderNeutralFallbackPersistsSatelliteRowsWhenMySqlIsConfigured() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    var options = CreateMySqlFallbackOptions(configuration.ConnectionString!);
    var services = new ServiceCollection();
    services.AddDVault();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();

    await using var context = new MySqlFallbackSaveServiceContext(options);
    await DropFallbackTablesIfExistsAsync(context);

    try {
      await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());

      var hub = new DataVaultHubMetadata(FallbackHubName, [BusinessKeyName]);
      var satellite = new DataVaultSatelliteMetadata(
          FallbackSatelliteName,
          hub.ToReference(),
          [FallbackPayloadName]);
      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              LoadTimestamp,
              RecordSource,
              [new DataVaultHubSaveOperation(hub, [new(BusinessKeyName, "MYSQL-FALLBACK-C-100")])],
              []));
      var parentHashKey = Assert.Single(hubResult.SavedRecords).HashKey;

      var satelliteResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              LoadTimestamp.AddMinutes(1),
              RecordSource,
              [],
              [],
              [
                  new DataVaultSatelliteSaveOperation(
                      satellite,
                      parentHashKey,
                      [new(FallbackPayloadName, "Active")],
                      "fallback-profile-active"),
              ]));

      Assert.Equal(1, satelliteResult.RowsWritten);
      var savedRecord = Assert.Single(satelliteResult.SavedRecords);
      Assert.Equal(DataVaultTableKind.Satellite, savedRecord.Kind);
      Assert.Equal(FallbackSatelliteName, savedRecord.MetadataName);

      var row = Assert.Single(
          await context.Set<Dictionary<string, object>>(FallbackSatelliteTableName)
              .AsNoTracking()
              .ToListAsync());

      Assert.Equal(parentHashKey, row[FallbackHubName + "HashKey"]);
      Assert.Equal("fallback-profile-active", row["HashDiff"]);
      Assert.Equal("Active", row[FallbackPayloadColumnName]);
    }
    finally {
      await DropFallbackTablesIfExistsAsync(context);
    }
  }

  private static DbContextOptions<MySqlExplicitSaveServiceContext> CreateMySqlOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<MySqlExplicitSaveServiceContext>();

    MySqlProviderReflection.UseMySql(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static DbContextOptions<MySqlFallbackSaveServiceContext> CreateMySqlFallbackOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<MySqlFallbackSaveServiceContext>();

    MySqlProviderReflection.UseMySql(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static async Task DropSmokeTableIfExistsAsync(DbContext context) {
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(HubTableName) + ";");
  }

  private static async Task DropFallbackTablesIfExistsAsync(DbContext context) {
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(FallbackSatelliteTableName) + ";");
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(FallbackHubTableName) + ";");
  }

  private static string QuoteIdentifier(string value) {
    return "`" + value.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  private sealed class MySqlExplicitSaveServiceContext(
      DbContextOptions<MySqlExplicitSaveServiceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          new DataVaultMetadataModel(
              [new DataVaultHubMetadata(HubName, [BusinessKeyName])],
              [],
              []),
          DataVaultProviderCapabilityProfiles.MySql);
    }
  }

  private sealed class MySqlFallbackSaveServiceContext(
      DbContextOptions<MySqlFallbackSaveServiceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      var hub = new DataVaultHubMetadata(FallbackHubName, [BusinessKeyName]);
      modelBuilder.ApplyDataVaultMetadata(
          new DataVaultMetadataModel(
              [hub],
              [],
              [new DataVaultSatelliteMetadata(FallbackSatelliteName, hub.ToReference(), [FallbackPayloadName])]),
          DataVaultProviderCapabilityProfiles.MySql);
    }
  }
}
