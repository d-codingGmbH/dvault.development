using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.ExternalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.MySqlProvider)]
public sealed class MySqlPitMaintenanceServiceTests {
  private const string MySqlProviderName = "MySql.EntityFrameworkCore";
  private const string ParentHashKeyColumnName = "MySqlPitCustomerHashKey";
  private const string PitTableName = "PitMySqlPitCustomerProfileStatus";
  private const string ProfileTableName = "SatMySqlPitCustomerProfile";
  private const string StatusTableName = "SatMySqlPitCustomerStatus";
  private const string HubTableName = "HubMySqlPitCustomer";
  private const string MySqlStrategyRegistrationDiagnostic =
      "MySQL PIT maintenance expected AddDVaultMySql to register a compatible provider strategy for a clean official-provider ordinary hub-parent full rebuild request.";
  private const string MySqlOptimizedPathDiagnostic =
      "MySQL PIT maintenance expected the provider delete-plus-insert path to rebuild without fallback-tracked PIT rows.";

  [Fact]
  public async Task AddDVaultMySqlPitRebuildsOfficialOrdinaryHubFullRebuildWhenConfigured() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    using var provider = CreateServiceProvider();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    var options = CreateMySqlOptions(configuration.ConnectionString!);
    var metadata = CreateMetadata();
    var parentHashKey = "mysql-pit-customer-100";
    var statusTimestamp = Utc(2026, 6, 24, 9, 0);
    var profileTimestamp = Utc(2026, 6, 24, 10, 0);
    var secondStatusTimestamp = Utc(2026, 6, 24, 11, 0);
    var staleTimestamp = Utc(2026, 6, 24, 8, 30);
    var request = new DataVaultPitRebuildRequest(metadata.Pit);

    await using var context = new MySqlPitMaintenanceContext(options);
    await ResetSchemaAsync(context);

    try {
      Assert.Equal(MySqlProviderName, context.Database.ProviderName);
      AssertCompatibleMySqlStrategy(provider, context, request);

      SeedProfileRow(context, parentHashKey, profileTimestamp, "Ada Adams", "Gold", "profile-1");
      SeedStatusRow(context, parentHashKey, statusTimestamp, "Active", "status-1");
      SeedStatusRow(context, parentHashKey, secondStatusTimestamp, "Preferred", "status-2");
      SeedPitRow(context, parentHashKey, staleTimestamp, profileTimestamp: null, statusTimestamp: null);
      await context.SaveChangesAsync();
      context.ChangeTracker.Clear();

      var result = await maintenanceService.RebuildAsync(context, request);

      AssertProviderPathObserved(context);
      Assert.Equal(PitTableName, result.TableName);
      Assert.Equal(1, result.ParentHashKeyCount);
      Assert.Equal(1, result.RowsDeleted);
      Assert.Equal(3, result.RowsWritten);
      Assert.Collection(
          await ReadPitRowsAsync(context),
          row => AssertPitRow(row, parentHashKey, statusTimestamp, null, statusTimestamp),
          row => AssertPitRow(row, parentHashKey, profileTimestamp, profileTimestamp, statusTimestamp),
          row => AssertPitRow(row, parentHashKey, secondStatusTimestamp, profileTimestamp, secondStatusTimestamp));
    }
    finally {
      await DropTablesIfExistsAsync(context);
    }
  }

  [Fact]
  public async Task AddDVaultMySqlPitRebuildRollsBackLocalTransactionWhenProviderPathFaultsWhenConfigured() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    using var provider = CreateServiceProvider();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    var options = CreateMySqlOptions(configuration.ConnectionString!);
    var metadata = CreateMetadata();
    var parentHashKey = "mysql-pit-customer-fault";
    var staleTimestamp = Utc(2026, 6, 24, 12, 30);

    await using var context = new MySqlPitMaintenanceContext(options);
    await ResetSchemaAsync(context);

    try {
      SeedProfileRow(context, parentHashKey, Utc(2026, 6, 24, 13, 0), "Fran Fault", "Silver", "profile-fault");
      SeedStatusRow(context, parentHashKey, Utc(2026, 6, 24, 13, 30), "Active", "status-fault");
      SeedPitRow(context, parentHashKey, staleTimestamp, profileTimestamp: null, statusTimestamp: null);
      await context.SaveChangesAsync();
      context.ChangeTracker.Clear();

      MySqlDataVaultPitMaintenanceStrategy.BeforeCommitHookForTestingAsync = _ =>
          throw new InvalidOperationException("Injected MySQL PIT maintenance rollback fault.");

      await Assert.ThrowsAsync<InvalidOperationException>(() =>
          maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit)));

      Assert.Equal(
          [new PitRowSnapshot(parentHashKey, staleTimestamp, ProfileLoadTimestamp: null, StatusLoadTimestamp: null)],
          await ReadPitRowsAsync(context));
    }
    finally {
      MySqlDataVaultPitMaintenanceStrategy.BeforeCommitHookForTestingAsync = null;
      await DropTablesIfExistsAsync(context);
    }
  }

  [Fact]
  public async Task AddDVaultMySqlPitRebuildRollsBackLocalTransactionWhenCancellationIsObservedBeforeCommitWhenConfigured() {
    var configuration = MySqlIntegrationTestConfiguration.FromEnvironment();
    if (!configuration.IsConfigured) {
      Assert.Skip(MySqlIntegrationTestConfiguration.MissingConfigurationSkipMessage);
    }

    using var provider = CreateServiceProvider();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    var options = CreateMySqlOptions(configuration.ConnectionString!);
    var metadata = CreateMetadata();
    var parentHashKey = "mysql-pit-customer-cancel";
    var staleTimestamp = Utc(2026, 6, 24, 14, 30);
    using var cancellation = new CancellationTokenSource();

    await using var context = new MySqlPitMaintenanceContext(options);
    await ResetSchemaAsync(context);

    try {
      SeedProfileRow(context, parentHashKey, Utc(2026, 6, 24, 15, 0), "Casey Cancel", "Bronze", "profile-cancel");
      SeedStatusRow(context, parentHashKey, Utc(2026, 6, 24, 15, 30), "Active", "status-cancel");
      SeedPitRow(context, parentHashKey, staleTimestamp, profileTimestamp: null, statusTimestamp: null);
      await context.SaveChangesAsync();
      context.ChangeTracker.Clear();

      MySqlDataVaultPitMaintenanceStrategy.BeforeCommitHookForTestingAsync = _ => {
        cancellation.Cancel();

        return Task.CompletedTask;
      };

      await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
          maintenanceService.RebuildAsync(
              context,
              new DataVaultPitRebuildRequest(metadata.Pit),
              cancellation.Token));

      Assert.Equal(
          [new PitRowSnapshot(parentHashKey, staleTimestamp, ProfileLoadTimestamp: null, StatusLoadTimestamp: null)],
          await ReadPitRowsAsync(context));
    }
    finally {
      MySqlDataVaultPitMaintenanceStrategy.BeforeCommitHookForTestingAsync = null;
      await DropTablesIfExistsAsync(context);
    }
  }

  private static ServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();
    services.AddDVaultMySql();

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static DbContextOptions<MySqlPitMaintenanceContext> CreateMySqlOptions(string connectionString) {
    var optionsBuilder = new DbContextOptionsBuilder<MySqlPitMaintenanceContext>();

    MySqlProviderReflection.UseMySql(optionsBuilder, connectionString);

    return optionsBuilder.Options;
  }

  private static async Task ResetSchemaAsync(DbContext context) {
    await DropTablesIfExistsAsync(context);
    await context.Database.ExecuteSqlRawAsync(context.Database.GenerateCreateScript());
  }

  private static async Task DropTablesIfExistsAsync(DbContext context) {
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(PitTableName) + ";");
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(ProfileTableName) + ";");
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(StatusTableName) + ";");
    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS " + QuoteIdentifier(HubTableName) + ";");
  }

  private static void AssertCompatibleMySqlStrategy(
      IServiceProvider provider,
      MySqlPitMaintenanceContext context,
      DataVaultPitRebuildRequest request) {
    Assert.True(
        provider.GetServices<IDataVaultProviderPitMaintenanceStrategy>().Any(strategy => strategy.CanRebuild(context, request)),
        MySqlStrategyRegistrationDiagnostic);
  }

  private static void AssertProviderPathObserved(MySqlPitMaintenanceContext context) {
    var trackedEntries = context.ChangeTracker.Entries().ToArray();

    Assert.True(
        trackedEntries.Length == 0,
        MySqlOptimizedPathDiagnostic + " Actual tracked entries: " + trackedEntries.Length);
  }

  private static void SeedProfileRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    context.Set<Dictionary<string, object>>(ProfileTableName).Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      [ParentHashKeyColumnName] = parentHashKey,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "mysql-profile",
      ["CustomerName"] = customerName,
      ["CustomerTier"] = customerTier,
    });
  }

  private static void SeedStatusRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      string statusCode,
      string hashDiff) {
    context.Set<Dictionary<string, object>>(StatusTableName).Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      [ParentHashKeyColumnName] = parentHashKey,
      ["HashDiff"] = hashDiff,
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "mysql-status",
      ["StatusCode"] = statusCode,
    });
  }

  private static void SeedPitRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? profileTimestamp,
      DateTimeOffset? statusTimestamp) {
    context.Set<Dictionary<string, object>>(PitTableName).Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      [ParentHashKeyColumnName] = parentHashKey,
      ["LoadTimestamp"] = loadTimestamp,
      ["ProfileLoadTimestamp"] = profileTimestamp is { } currentProfileTimestamp
          ? currentProfileTimestamp
          : null!,
      ["StatusLoadTimestamp"] = statusTimestamp is { } currentStatusTimestamp
          ? currentStatusTimestamp
          : null!,
    });
  }

  private static async Task<IReadOnlyList<PitRowSnapshot>> ReadPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>(PitTableName)
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new PitRowSnapshot(
            Assert.IsType<string>(row[ParentHashKeyColumnName]),
            ReadRequiredTimestamp(row, "LoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp"),
            ReadOptionalTimestamp(row, "StatusLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static DateTimeOffset ReadRequiredTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    Assert.True(DataVaultLoadTimestampValueConverter.TryReadProviderValue(row[columnName], out var timestamp));

    return timestamp;
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    return row.TryGetValue(columnName, out var value) && value is not null
        ? DataVaultLoadTimestampValueConverter.ReadProviderValue(value)
        : null;
  }

  private static void AssertPitRow(
      PitRowSnapshot row,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? profileTimestamp,
      DateTimeOffset? statusTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(profileTimestamp, row.ProfileLoadTimestamp);
    Assert.Equal(statusTimestamp, row.StatusLoadTimestamp);
  }

  private static PitMaintenanceMetadata CreateMetadata() {
    var customer = new DataVaultHubMetadata("MySqlPitCustomer", ["Customer Id"]);
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

    return new PitMaintenanceMetadata(pit, model);
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private static string QuoteIdentifier(string value) {
    return "`" + value.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  private sealed class MySqlPitMaintenanceContext(
      DbContextOptions<MySqlPitMaintenanceContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadata().Model,
          DataVaultProviderCapabilityProfiles.MySql);
    }
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record PitRowSnapshot(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ProfileLoadTimestamp,
      DateTimeOffset? StatusLoadTimestamp);
}
