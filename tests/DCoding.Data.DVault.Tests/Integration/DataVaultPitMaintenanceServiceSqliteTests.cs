using System.Globalization;
using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Integration;

[Trait(ProviderTestCategories.CategoryTraitName, ProviderTestCategories.RequiredLocalProviderIntegration)]
[Trait(ProviderTestCategories.ProviderTraitName, ProviderTestCategories.SqliteProvider)]
public sealed class DataVaultPitMaintenanceServiceSqliteTests {
  [Theory]
  [InlineData(DataVaultLoadTimestampStorage.ProviderDefault)]
  [InlineData(DataVaultLoadTimestampStorage.Iso8601UtcText)]
  [InlineData(DataVaultLoadTimestampStorage.UtcTicks)]
  public async Task PitMaintenanceRebuildsDeterministicRowsAndMissingSnapshotsThroughSqlite(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var secondStatusTimestamp = Utc(2026, 5, 11, 11, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database.DatabasePath, loadTimestampStorage);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string customerHashKey;
    DataVaultPitMaintenanceResult maintenanceResult;

    await using (var context = new PitMaintenanceContext(options, loadTimestampStorage)) {
      await context.Database.EnsureCreatedAsync();
      customerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-100", importTimestamp);
      await SaveStatusAsync(saveService, context, metadata, customerHashKey, statusTimestamp, "Active", "status-1");
      await SaveProfileAsync(saveService, context, metadata, customerHashKey, profileTimestamp, "Alice Adams", "Gold", "profile-1");
      await SaveStatusAsync(saveService, context, metadata, customerHashKey, secondStatusTimestamp, "Preferred", "status-2");

      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(CreatePitRow(
          loadTimestampStorage,
          customerHashKey,
          Utc(2026, 5, 11, 8, 30),
          profileSnapshotTimestamp: null,
          statusSnapshotTimestamp: null));
      await context.SaveChangesAsync();

      maintenanceResult = await maintenanceService.RebuildAsync(
          context,
          new DataVaultPitRebuildRequest(metadata.Pit));
    }

    await using (var context = new PitMaintenanceContext(options, loadTimestampStorage)) {
      var pitRows = await ReadPitRowsAsync(context);
      var readRecords = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(metadata.Pit, [customerHashKey], Utc(2026, 5, 11, 10, 30)));

      Assert.Equal("PitCustomerProfileStatus", maintenanceResult.TableName);
      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(1, maintenanceResult.RowsDeleted);
      Assert.Equal(3, maintenanceResult.RowsWritten);
      Assert.False(maintenanceResult.IsNoOp);
      Assert.Collection(
          pitRows,
          row => AssertPitRow(row, customerHashKey, statusTimestamp, null, statusTimestamp),
          row => AssertPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusTimestamp),
          row => AssertPitRow(row, customerHashKey, secondStatusTimestamp, profileTimestamp, secondStatusTimestamp));

      var record = Assert.Single(readRecords);
      Assert.Equal(profileTimestamp, record.LoadTimestamp);
      Assert.Equal(profileTimestamp, RequiredSnapshot(record, "Profile").SnapshotLoadTimestamp);
      Assert.Equal(statusTimestamp, RequiredSnapshot(record, "Status").SnapshotLoadTimestamp);
    }
  }

  [Fact]
  public async Task PitMaintenanceMaintainsOnlyRequestedParentsAndCorrectsLateArrivingSatelliteHistoryThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var lateProfileTimestamp = Utc(2026, 5, 11, 8, 30);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = CreateOptions(database.DatabasePath, DataVaultLoadTimestampStorage.ProviderDefault);
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var maintenanceService = provider.GetRequiredService<IDataVaultPitMaintenanceService>();
    string firstCustomerHashKey;
    string secondCustomerHashKey;

    await using (var context = new PitMaintenanceContext(options, DataVaultLoadTimestampStorage.ProviderDefault)) {
      await context.Database.EnsureCreatedAsync();
      firstCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-100", importTimestamp);
      secondCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-200", importTimestamp);
      await SaveStatusAsync(saveService, context, metadata, firstCustomerHashKey, statusTimestamp, "Active", "status-c100");
      await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, profileTimestamp, "Alice Adams", "Gold", "profile-c100");
      await SaveStatusAsync(saveService, context, metadata, secondCustomerHashKey, statusTimestamp, "Prospect", "status-c200");
      await SaveProfileAsync(saveService, context, metadata, secondCustomerHashKey, profileTimestamp, "Bob Brown", "Silver", "profile-c200");
      await maintenanceService.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));
    }

    DataVaultPitMaintenanceResult maintenanceResult;
    await using (var context = new PitMaintenanceContext(options, DataVaultLoadTimestampStorage.ProviderDefault)) {
      await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, lateProfileTimestamp, "Alice A.", "Bronze", "profile-c100-late");
      maintenanceResult = await maintenanceService.MaintainParentsAsync(
          context,
          new DataVaultPitParentMaintenanceRequest(metadata.Pit, [firstCustomerHashKey]));
    }

    await using (var context = new PitMaintenanceContext(options, DataVaultLoadTimestampStorage.ProviderDefault)) {
      var pitRows = await ReadPitRowsAsync(context);
      var firstRows = pitRows.Where(row => row.ParentHashKey == firstCustomerHashKey).ToArray();
      var secondRows = pitRows.Where(row => row.ParentHashKey == secondCustomerHashKey).ToArray();
      var readRecords = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(metadata.Pit, [firstCustomerHashKey], Utc(2026, 5, 11, 9, 15)));

      Assert.Equal(1, maintenanceResult.ParentHashKeyCount);
      Assert.Equal(2, maintenanceResult.RowsDeleted);
      Assert.Equal(3, maintenanceResult.RowsWritten);
      Assert.Collection(
          firstRows,
          row => AssertPitRow(row, firstCustomerHashKey, lateProfileTimestamp, lateProfileTimestamp, null),
          row => AssertPitRow(row, firstCustomerHashKey, statusTimestamp, lateProfileTimestamp, statusTimestamp),
          row => AssertPitRow(row, firstCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
      Assert.Collection(
          secondRows,
          row => AssertPitRow(row, secondCustomerHashKey, statusTimestamp, null, statusTimestamp),
          row => AssertPitRow(row, secondCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));

      var record = Assert.Single(readRecords);
      Assert.Equal(statusTimestamp, record.LoadTimestamp);
      Assert.Equal(lateProfileTimestamp, RequiredSnapshot(record, "Profile").SnapshotLoadTimestamp);
      Assert.Equal(statusTimestamp, RequiredSnapshot(record, "Status").SnapshotLoadTimestamp);
    }
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceRebuildsByNameThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    using var provider = CreateRegistryProvider(database.DatabasePath);
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryPitMaintenanceContext>();
    var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = scope.ServiceProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    await context.Database.EnsureCreatedAsync();

    var customerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-300", importTimestamp);
    await SaveStatusAsync(saveService, context, metadata, customerHashKey, statusTimestamp, "Active", "status-c300");
    await SaveProfileAsync(saveService, context, metadata, customerHashKey, profileTimestamp, "Carol Clark", "Gold", "profile-c300");
    context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(CreatePitRow(
        DataVaultLoadTimestampStorage.ProviderDefault,
        customerHashKey,
        Utc(2026, 5, 11, 8, 30),
        profileSnapshotTimestamp: null,
        statusSnapshotTimestamp: null));
    await context.SaveChangesAsync();

    var result = await maintenanceService.RebuildAsync(
        context,
        new DataVaultRegistryPitRebuildRequest(metadata.Pit.Name));
    var pitRows = await ReadPitRowsAsync(context);

    Assert.Equal("PitCustomerProfileStatus", result.TableName);
    Assert.Equal(1, result.RowsDeleted);
    Assert.Equal(2, result.RowsWritten);
    Assert.Collection(
        pitRows,
        row => AssertPitRow(row, customerHashKey, statusTimestamp, null, statusTimestamp),
        row => AssertPitRow(row, customerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
  }

  [Fact]
  public async Task RegistryBackedPitMaintenanceMaintainsParentsByClrMappingThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = Utc(2026, 5, 11, 8, 0);
    var statusTimestamp = Utc(2026, 5, 11, 9, 0);
    var profileTimestamp = Utc(2026, 5, 11, 10, 0);
    var lateProfileTimestamp = Utc(2026, 5, 11, 8, 30);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    using var provider = CreateRegistryProvider(database.DatabasePath);
    using var scope = provider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<RegistryPitMaintenanceContext>();
    var saveService = scope.ServiceProvider.GetRequiredService<IDataVaultSaveService>();
    var maintenanceService = scope.ServiceProvider.GetRequiredService<IDataVaultPitMaintenanceService>();
    await context.Database.EnsureCreatedAsync();

    var firstCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-400", importTimestamp);
    var secondCustomerHashKey = await SaveCustomerAsync(saveService, context, metadata, "C-500", importTimestamp);
    await SaveStatusAsync(saveService, context, metadata, firstCustomerHashKey, statusTimestamp, "Active", "status-c400");
    await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, profileTimestamp, "Drew Davis", "Gold", "profile-c400");
    await SaveStatusAsync(saveService, context, metadata, secondCustomerHashKey, statusTimestamp, "Prospect", "status-c500");
    await SaveProfileAsync(saveService, context, metadata, secondCustomerHashKey, profileTimestamp, "Evan Evans", "Silver", "profile-c500");
    await maintenanceService.RebuildAsync(context, new DataVaultRegistryPitRebuildRequest(metadata.Pit.Name));
    await SaveProfileAsync(saveService, context, metadata, firstCustomerHashKey, lateProfileTimestamp, "Drew D.", "Bronze", "profile-c400-late");

    var result = await maintenanceService.MaintainParentsAsync(
        context,
        new DataVaultRegistryPitParentMaintenanceRequest(
            typeof(CustomerProfileStatusPitMapping),
            [firstCustomerHashKey]));
    var pitRows = await ReadPitRowsAsync(context);
    var firstRows = pitRows.Where(row => row.ParentHashKey == firstCustomerHashKey).ToArray();
    var secondRows = pitRows.Where(row => row.ParentHashKey == secondCustomerHashKey).ToArray();

    Assert.Equal(1, result.ParentHashKeyCount);
    Assert.Equal(2, result.RowsDeleted);
    Assert.Equal(3, result.RowsWritten);
    Assert.Collection(
        firstRows,
        row => AssertPitRow(row, firstCustomerHashKey, lateProfileTimestamp, lateProfileTimestamp, null),
        row => AssertPitRow(row, firstCustomerHashKey, statusTimestamp, lateProfileTimestamp, statusTimestamp),
        row => AssertPitRow(row, firstCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
    Assert.Collection(
        secondRows,
        row => AssertPitRow(row, secondCustomerHashKey, statusTimestamp, null, statusTimestamp),
        row => AssertPitRow(row, secondCustomerHashKey, profileTimestamp, profileTimestamp, statusTimestamp));
  }

  private static DbContextOptions<PitMaintenanceContext> CreateOptions(
      object? databasePath,
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    return new DbContextOptionsBuilder<PitMaintenanceContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(databasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, PitMaintenanceModelCacheKeyFactory>()
        .Options;
  }

  private static async Task<string> SaveCustomerAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitMaintenanceMetadata metadata,
      string customerId,
      DateTimeOffset loadTimestamp) {
    var result = await saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-import",
            [new(metadata.Customer, [new("Customer Id", customerId)])],
            []));

    return GetHashKey(result, DataVaultTableKind.Hub, "Customer");
  }

  private static Task SaveProfileAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitMaintenanceMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string customerName,
      string customerTier,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-profile",
            [],
            [],
            [
                new(
                    metadata.Profile,
                    customerHashKey,
                    [new("Customer Name", customerName), new("Customer Tier", customerTier)],
                    hashDiff),
            ]));
  }

  private static Task SaveStatusAsync(
      IDataVaultSaveService saveService,
      DbContext context,
      PitMaintenanceMetadata metadata,
      string customerHashKey,
      DateTimeOffset loadTimestamp,
      string statusCode,
      string hashDiff) {
    return saveService.SaveAsync(
        context,
        new DataVaultSaveRequest(
            loadTimestamp,
            "crm-status",
            [],
            [],
            [
                new(
                    metadata.Status,
                    customerHashKey,
                    [new("Status Code", statusCode)],
                    hashDiff),
            ]));
  }

  private static Dictionary<string, object> CreatePitRow(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      string parentHashKey,
      DateTimeOffset pitLoadTimestamp,
      DateTimeOffset? profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    return new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["LoadTimestamp"] = ToStoredTimestamp(loadTimestampStorage, pitLoadTimestamp),
      ["ProfileLoadTimestamp"] = profileSnapshotTimestamp.HasValue
          ? ToStoredTimestamp(loadTimestampStorage, profileSnapshotTimestamp.Value)
          : null!,
      ["StatusLoadTimestamp"] = statusSnapshotTimestamp.HasValue
          ? ToStoredTimestamp(loadTimestampStorage, statusSnapshotTimestamp.Value)
          : null!,
    };
  }

  private static object ToStoredTimestamp(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      DateTimeOffset timestamp) {
    var utcTimestamp = timestamp.ToUniversalTime();
    return loadTimestampStorage switch {
      DataVaultLoadTimestampStorage.Iso8601UtcText => utcTimestamp.ToString("O", CultureInfo.InvariantCulture),
      DataVaultLoadTimestampStorage.UtcTicks => utcTimestamp.UtcDateTime.Ticks,
      _ => utcTimestamp,
    };
  }

  private static async Task<IReadOnlyList<PitRow>> ReadPitRowsAsync(DbContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerProfileStatus")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new PitRow(
            Assert.IsType<string>(row["CustomerHashKey"]),
            DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp"),
            ReadOptionalTimestamp(row, "StatusLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      IReadOnlyDictionary<string, object> row,
      string columnName) {
    return row.TryGetValue(columnName, out var value) && value is not null
        ? DataVaultLoadTimestampValueConverter.ReadProviderValue(value)
        : null;
  }

  private static DataVaultPitSatelliteSnapshot RequiredSnapshot(
      DataVaultPitReadRecord record,
      string satelliteName) {
    Assert.True(record.SatelliteSnapshotsByName.TryGetValue(satelliteName, out var snapshot));
    return snapshot!;
  }

  private static void AssertPitRow(
      PitRow row,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(profileSnapshotTimestamp, row.ProfileSnapshotTimestamp);
    Assert.Equal(statusSnapshotTimestamp, row.StatusSnapshotTimestamp);
  }

  private static PitMaintenanceMetadata CreateMetadata() {
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

    return new PitMaintenanceMetadata(customer, profile, status, pit, model);
  }

  private static ServiceProvider CreateRegistryProvider(object? databasePath) {
    var registry = DataVaultMetadataRegistry.Create(
        CreateMetadata().Model,
        [],
        [DataVaultMetadataClrMapping.Pit<CustomerProfileStatusPitMapping>("CustomerProfileStatus")]);
    var services = new ServiceCollection();
    services.AddDVault(options => options.UseMetadataRegistry(registry));
    services.AddDVaultSqlite();
    services.AddDbContext<RegistryPitMaintenanceContext>(options => options
        .UseSqlite("Data Source=" + Assert.IsType<string>(databasePath) + ";Pooling=False")
        .UseDataVaultMetadata());

    return services.BuildServiceProvider(validateScopes: true);
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private sealed class PitMaintenanceContext(
      DbContextOptions<PitMaintenanceContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadata().Model,
          DataVaultProviderCapabilityProfiles.Sqlite,
          LoadTimestampStorage);
    }
  }

  private sealed class PitMaintenanceModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is PitMaintenanceContext pitMaintenanceContext
          ? (context.GetType(), pitMaintenanceContext.LoadTimestampStorage, designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed class RegistryPitMaintenanceContext(DbContextOptions<RegistryPitMaintenanceContext> options) : DbContext(options) {
  }

  private sealed class CustomerProfileStatusPitMapping {
  }

  private sealed record PitMaintenanceMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Status,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record PitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp,
      DateTimeOffset? StatusSnapshotTimestamp);
}
