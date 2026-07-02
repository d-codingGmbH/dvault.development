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
public sealed class DataVaultPitReadServiceSqliteTests {
  private const string StableHashAlgorithmId = "sha256-128-v1";
  private const int StableHashDigestByteLength = 16;

  [Theory]
  [InlineData(DataVaultLoadTimestampStorage.ProviderDefault)]
  [InlineData(DataVaultLoadTimestampStorage.Iso8601UtcText)]
  [InlineData(DataVaultLoadTimestampStorage.UtcTicks)]
  public async Task PitReadMaterializesLatestVisibleRowsAndMissingSnapshotsAcrossStorageModesThroughSqlite(
      DataVaultLoadTimestampStorage loadTimestampStorage) {
    var metadata = CreateMetadata();
    var importTimestamp = new DateTimeOffset(2026, 5, 11, 8, 0, 0, TimeSpan.Zero);
    var statusTimestamp = new DateTimeOffset(2026, 5, 11, 9, 59, 0, TimeSpan.Zero);
    var profileTimestamp = new DateTimeOffset(2026, 5, 11, 10, 58, 0, TimeSpan.Zero);
    var olderPitTimestamp = new DateTimeOffset(2026, 5, 11, 10, 0, 0, TimeSpan.Zero);
    var selectedPitTimestamp = new DateTimeOffset(2026, 5, 11, 11, 0, 0, TimeSpan.Zero);
    var asOf = new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, PitReadModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    var readDiagnostics = provider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    var fallbackServices = new ServiceCollection();
    fallbackServices.AddDVault();
    using var fallbackProvider = fallbackServices.BuildServiceProvider(validateScopes: true);
    var fallbackReadService = fallbackProvider.GetRequiredService<IDataVaultReadService>();
    var fallbackReadDiagnostics = fallbackProvider.GetRequiredService<IDataVaultReadDiagnosticsService>();
    string customerHashKey;

    await using (var context = new PitReadContext(options, loadTimestampStorage)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              importTimestamp,
              "crm-import",
              [new(metadata.Customer, [new("Customer Id", "C-100")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              statusTimestamp,
              "crm-import",
              [],
              [],
              [
                  new(
                      metadata.Profile,
                      customerHashKey,
                      [new("Customer Name", "Alice Adams"), new("Customer Tier", "Gold")],
                      "profile-hash-1"),
                  new(
                      metadata.Status,
                      customerHashKey,
                      [new("Status Code", "Active")],
                      "status-hash-1"),
              ]));
      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              profileTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(
                      metadata.Profile,
                      customerHashKey,
                      [new("Customer Name", "Alice Baker"), new("Customer Tier", "Platinum")],
                      "profile-hash-2"),
              ]));

      var pitRows = context.Set<Dictionary<string, object>>("PitCustomerProfileStatus");
      pitRows.Add(CreatePitRow(
          loadTimestampStorage,
          customerHashKey,
          olderPitTimestamp,
          statusTimestamp,
          statusTimestamp));
      pitRows.Add(CreatePitRow(
          loadTimestampStorage,
          customerHashKey,
          selectedPitTimestamp,
          profileTimestamp,
          statusSnapshotTimestamp: null));
      await context.SaveChangesAsync();
    }

    await using (var context = new PitReadContext(options, loadTimestampStorage)) {
      var request = new DataVaultPitAsOfReadRequest(
          metadata.Pit,
          [customerHashKey, "customer-hash-missing"],
          asOf);
      var diagnostics = readDiagnostics.Analyze(context, request);
      var records = await readService.ReadPitRowsAsync(context, request);
      var projectedRows = await readService.ReadPitAsync(
          context,
          request,
          row => {
            var profile = row.RequiredSatellite("Profile");
            var status = row.OptionalSatellite("Status");

            return new CustomerSnapshotRead(
                row.RequiredString("ParentHashKey"),
                row.RequiredDateTimeOffset("LoadTimestamp"),
                profile.RequiredString("Customer Name"),
                profile.RequiredString("Customer Tier"),
                status?.NullableString("Status Code"));
          });
      var emptyRows = await readService.ReadPitRowsAsync(
          context,
          new DataVaultPitAsOfReadRequest(
              metadata.Pit,
              [customerHashKey],
              olderPitTimestamp.AddMinutes(-1)));
      var fallbackDiagnostics = fallbackReadDiagnostics.Analyze(context, request);
      var fallbackRecords = await fallbackReadService.ReadPitRowsAsync(context, request);

      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, diagnostics.ReadStrategy.Status);
      Assert.Equal("SqliteDataVaultReadStrategy", diagnostics.ReadStrategy.SelectedStrategyName);
      var selectedCandidate = Assert.Single(diagnostics.ReadStrategy.Candidates);
      Assert.Equal([KnownProviderNames.Sqlite], selectedCandidate.SupportedProviderNames);
      Assert.Contains(
          selectedCandidate.GateRequirements,
          requirement => requirement.Kind == DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape);
      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, fallbackDiagnostics.ReadStrategy.Status);
      Assert.Contains(
          fallbackDiagnostics.ReadStrategy.FallbackCauses,
          cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered);
      Assert.NotNull(diagnostics.ReadShape);
      var pitShape = diagnostics.ReadShape!;
      Assert.Equal(DataVaultReadShapeKind.PitAsOf, pitShape.Kind);
      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected, pitShape.Provider.ReadStrategyStatus);
      Assert.NotNull(pitShape.Pit);
      var pitReadShape = pitShape.Pit!;
      Assert.Equal("PitCustomerProfileStatus", pitReadShape.Pit.TableName);
      Assert.Equal(["CustomerHashKey"], pitReadShape.FilterColumns[0].ColumnNames);
      Assert.Equal(["LoadTimestamp"], pitReadShape.FilterColumns[1].ColumnNames);
      Assert.Equal(["Profile", "Status"], pitReadShape.ReferencedSatellites.Select(satellite => satellite.MetadataName).ToArray());
      Assert.Contains(
          pitReadShape.ExpectedIndexBaseline,
          index => index.Kind == "primary-key" && index.ColumnNames.SequenceEqual(["CustomerHashKey", "LoadTimestamp"]));
      Assert.Equal(DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback, fallbackDiagnostics.ReadShape!.Provider.ReadStrategyStatus);

      var record = Assert.Single(records);
      var fallbackRecord = Assert.Single(fallbackRecords);
      Assert.Equal(customerHashKey, record.ParentHashKey);
      Assert.Equal(record.ParentHashKey, fallbackRecord.ParentHashKey);
      Assert.Equal(selectedPitTimestamp, record.LoadTimestamp);
      Assert.Equal(record.LoadTimestamp, fallbackRecord.LoadTimestamp);
      Assert.True(record.SatelliteSnapshotsByName.ContainsKey("Profile"));
      Assert.True(record.SatelliteSnapshotsByName.ContainsKey("Status"));
      Assert.Collection(
          record.SatelliteSnapshots,
          profile => {
            Assert.Equal("Profile", profile.SatelliteName);
            Assert.Equal(0, profile.Ordinal);
            Assert.True(profile.IsPresent);
            Assert.Equal(profileTimestamp, profile.SnapshotLoadTimestamp);
            Assert.Equal("profile-hash-2", profile.HashDiff);
            Assert.Equal("crm-change", profile.RecordSource);
            Assert.Equal("Alice Baker", profile.PayloadValues["Customer Name"]);
            Assert.Equal("Platinum", profile.PayloadValues["Customer Tier"]);
          },
          status => {
            Assert.Equal("Status", status.SatelliteName);
            Assert.Equal(1, status.Ordinal);
            Assert.False(status.IsPresent);
            Assert.Null(status.SnapshotLoadTimestamp);
            Assert.Null(status.HashDiff);
            Assert.Null(status.RecordSource);
            Assert.Empty(status.PayloadValues);
          });

      var projectedRow = Assert.Single(projectedRows);
      Assert.Equal(customerHashKey, projectedRow.ParentHashKey);
      Assert.Equal(selectedPitTimestamp, projectedRow.LoadTimestamp);
      Assert.Equal("Alice Baker", projectedRow.CustomerName);
      Assert.Equal("Platinum", projectedRow.CustomerTier);
      Assert.Null(projectedRow.StatusCode);
      Assert.Empty(emptyRows);
    }
  }

  [Fact]
  public async Task PitReadRoundTripsBinaryHashKeyStorageThroughSqlite() {
    var metadata = CreateMetadata();
    var importTimestamp = new DateTimeOffset(2026, 6, 1, 8, 0, 0, TimeSpan.Zero);
    var profileTimestamp = new DateTimeOffset(2026, 6, 1, 9, 0, 0, TimeSpan.Zero);
    var pitLoadTimestamp = new DateTimeOffset(2026, 6, 1, 10, 0, 0, TimeSpan.Zero);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitReadContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .ReplaceService<IModelCacheKeyFactory, PitReadModelCacheKeyFactory>()
        .Options;
    var services = new ServiceCollection();
    services.AddDVault(configure => configure.UseStableHashAlgorithm(StableHashAlgorithmId));
    services.AddDVaultSqlite();

    using var provider = services.BuildServiceProvider(validateScopes: true);
    var saveService = provider.GetRequiredService<IDataVaultSaveService>();
    var readService = provider.GetRequiredService<IDataVaultReadService>();
    string customerHashKey;

    await using (var context = new PitReadContext(
        options,
        DataVaultLoadTimestampStorage.ProviderDefault,
        DataVaultHashKeyStorageProfile.Binary)) {
      await context.Database.EnsureCreatedAsync();

      var hubResult = await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              importTimestamp,
              "crm-import",
              [new(metadata.Customer, [new("Customer Id", "C-BIN-PIT")])],
              []));
      customerHashKey = GetHashKey(hubResult, DataVaultTableKind.Hub, "Customer");

      await saveService.SaveAsync(
          context,
          new DataVaultSaveRequest(
              profileTimestamp,
              "crm-change",
              [],
              [],
              [
                  new(
                      metadata.Profile,
                      customerHashKey,
                      [new("Customer Name", "Binary Customer"), new("Customer Tier", "Gold")],
                      "profile-hash-2"),
              ]));

      context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(
          CreatePitRow(
              DataVaultLoadTimestampStorage.ProviderDefault,
              customerHashKey,
              pitLoadTimestamp,
              profileTimestamp,
              statusSnapshotTimestamp: null));
      await context.SaveChangesAsync();
    }

    using (var connection = database.CreateOpenConnection()) {
      AssertSqliteHashStorage(connection, "HubCustomer", "CustomerHashKey", customerHashKey, "blob", StableHashDigestByteLength);
      AssertSqliteHashStorage(connection, "PitCustomerProfileStatus", "CustomerHashKey", customerHashKey, "blob", StableHashDigestByteLength);
    }

    await using (var context = new PitReadContext(
        options,
        DataVaultLoadTimestampStorage.ProviderDefault,
        DataVaultHashKeyStorageProfile.Binary)) {
      var request = new DataVaultPitAsOfReadRequest(metadata.Pit, [customerHashKey], pitLoadTimestamp.AddMinutes(1));
      var records = await readService.ReadPitRowsAsync(context, request);
      var projectedRows = await readService.ReadPitAsync(
          context,
          request,
          row => row.RequiredString("ParentHashKey"));
      var record = Assert.Single(records);
      var profile = record.SatelliteSnapshots.Single(snapshot => snapshot.SatelliteName == "Profile");
      var status = record.SatelliteSnapshots.Single(snapshot => snapshot.SatelliteName == "Status");

      Assert.Equal(customerHashKey, record.ParentHashKey);
      Assert.Equal(pitLoadTimestamp, record.LoadTimestamp);
      Assert.True(profile.IsPresent);
      Assert.Equal(profileTimestamp, profile.SnapshotLoadTimestamp);
      Assert.Equal("Binary Customer", profile.PayloadValues["Customer Name"]);
      Assert.Equal("Gold", profile.PayloadValues["Customer Tier"]);
      Assert.False(status.IsPresent);
      Assert.Equal([customerHashKey], projectedRows);
    }
  }

  private static Dictionary<string, object> CreatePitRow(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      string parentHashKey,
      DateTimeOffset pitLoadTimestamp,
      DateTimeOffset profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    return new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["LoadTimestamp"] = ToStoredTimestamp(loadTimestampStorage, pitLoadTimestamp),
      ["ProfileLoadTimestamp"] = ToStoredTimestamp(loadTimestampStorage, profileSnapshotTimestamp),
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

  private static DataVaultProviderCapabilityProfile CreateSqliteProfile(DataVaultHashKeyStorageProfile storageProfile) {
    return DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
        storageProfile,
        StableHashAlgorithmId,
        StableHashDigestByteLength);
  }

  private static void AssertSqliteHashStorage(
      SqliteTestConnection connection,
      string tableName,
      string columnName,
      string expectedHashKey,
      string expectedStorageClass,
      int expectedLength) {
    Assert.Equal(
        expectedStorageClass,
        connection.ExecuteScalarString(
            "SELECT typeof(" + QuoteSqliteIdentifier(columnName) + ") FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));
    Assert.Equal(
        expectedLength.ToString(CultureInfo.InvariantCulture),
        connection.ExecuteScalarString(
            "SELECT length(" + QuoteSqliteIdentifier(columnName) + ") FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));
    Assert.Equal(
        expectedHashKey.ToUpperInvariant(),
        connection.ExecuteScalarString(
            "SELECT hex(" + QuoteSqliteIdentifier(columnName) + ") FROM " + QuoteSqliteIdentifier(tableName) + " ORDER BY rowid LIMIT 1;"));
  }

  private static string QuoteSqliteIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static PitReadMetadata CreateMetadata() {
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

    return new PitReadMetadata(customer, profile, status, pit, model);
  }

  private static string GetHashKey(
      DataVaultSaveResult result,
      DataVaultTableKind kind,
      string metadataName) {
    return result.SavedRecords
        .Single(record => record.Kind == kind && record.MetadataName == metadataName)
        .HashKey;
  }

  private sealed class PitReadContext(
      DbContextOptions<PitReadContext> options,
      DataVaultLoadTimestampStorage loadTimestampStorage,
      DataVaultHashKeyStorageProfile storageProfile = DataVaultHashKeyStorageProfile.HexString) : DbContext(options) {
    public DataVaultLoadTimestampStorage LoadTimestampStorage { get; } = loadTimestampStorage;

    public DataVaultHashKeyStorageProfile StorageProfile { get; } = storageProfile;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateMetadata().Model,
          CreateSqliteProfile(StorageProfile),
          LoadTimestampStorage);
    }
  }

  private sealed class PitReadModelCacheKeyFactory : IModelCacheKeyFactory {
    public object Create(DbContext context, bool designTime) {
      return context is PitReadContext pitReadContext
          ? (
              context.GetType(),
              pitReadContext.LoadTimestampStorage,
              pitReadContext.StorageProfile,
              StableHashAlgorithmId,
              StableHashDigestByteLength,
              designTime)
          : (object)(context.GetType(), designTime);
    }
  }

  private sealed record PitReadMetadata(
      DataVaultHubMetadata Customer,
      DataVaultSatelliteMetadata Profile,
      DataVaultSatelliteMetadata Status,
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record CustomerSnapshotRead(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      string CustomerName,
      string CustomerTier,
      string? StatusCode);
}
