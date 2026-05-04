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

  private static DbContextOptions<MySqlExplicitSaveServiceContext> CreateMySqlOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<MySqlExplicitSaveServiceContext>();

    MySqlProviderReflection.UseMySql(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static async Task DropSmokeTableIfExistsAsync(DbContext context) {
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(HubTableName) + ";");
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
              []));
    }
  }
}
