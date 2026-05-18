using DCoding.Data.DVault.Modeling;
using DCoding.Data.DVault.Tests.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPitMaintenanceRowGenerationTests {
  [Fact]
  public async Task RebuildCreatesDeterministicPitRowsFromDistinctSatelliteLoadTimestamps() {
    var service = new DefaultDataVaultPitMaintenanceService();
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile", "Status"]);
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<PitGenerationModelContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var firstStatusTimestamp = Utc(2026, 5, 11, 8, 0);
    var firstProfileTimestamp = Utc(2026, 5, 11, 9, 0);
    var secondProfileTimestamp = Utc(2026, 5, 11, 10, 0);
    var secondStatusTimestamp = Utc(2026, 5, 11, 11, 0);
    var otherStatusTimestamp = Utc(2026, 5, 11, 12, 0);

    await using (var context = new PitGenerationModelContext(options)) {
      await context.Database.EnsureCreatedAsync();
      AddPitRow(context, "customer-a", Utc(2026, 5, 11, 7, 30), profileSnapshotTimestamp: null, statusSnapshotTimestamp: null);
      AddSatelliteRow(context, "SatCustomerProfile", "customer-b", secondProfileTimestamp);
      AddSatelliteRow(context, "SatCustomerStatu", "customer-a", secondStatusTimestamp);
      AddSatelliteRow(context, "SatCustomerStatu", "customer-a", firstStatusTimestamp);
      AddSatelliteRow(context, "SatCustomerProfile", "customer-a", firstProfileTimestamp);
      AddSatelliteRow(context, "SatCustomerStatu", "customer-b", otherStatusTimestamp);
      await context.SaveChangesAsync();

      var result = await service.RebuildAsync(context, new DataVaultPitRebuildRequest(pit));

      Assert.Equal("PitCustomerProfileStatus", result.TableName);
      Assert.Equal(2, result.ParentHashKeyCount);
      Assert.Equal(1, result.RowsDeleted);
      Assert.Equal(5, result.RowsWritten);
    }

    await using (var context = new PitGenerationModelContext(options)) {
      var pitRows = await ReadPitRowsAsync(context);

      Assert.Collection(
          pitRows,
          row => AssertPitRow(row, "customer-a", firstStatusTimestamp, null, firstStatusTimestamp),
          row => AssertPitRow(row, "customer-a", firstProfileTimestamp, firstProfileTimestamp, firstStatusTimestamp),
          row => AssertPitRow(row, "customer-a", secondStatusTimestamp, firstProfileTimestamp, secondStatusTimestamp),
          row => AssertPitRow(row, "customer-b", secondProfileTimestamp, secondProfileTimestamp, null),
          row => AssertPitRow(row, "customer-b", otherStatusTimestamp, secondProfileTimestamp, otherStatusTimestamp));
    }
  }

  private static void AddSatelliteRow(
      DbContext context,
      string tableName,
      string parentHashKey,
      DateTimeOffset loadTimestamp) {
    context.Set<Dictionary<string, object>>(tableName).Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["LoadTimestamp"] = loadTimestamp,
    });
  }

  private static void AddPitRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? profileSnapshotTimestamp,
      DateTimeOffset? statusSnapshotTimestamp) {
    context.Set<Dictionary<string, object>>("PitCustomerProfileStatus").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["LoadTimestamp"] = loadTimestamp,
      ["ProfileLoadTimestamp"] = profileSnapshotTimestamp,
      ["StatusLoadTimestamp"] = statusSnapshotTimestamp,
    });
  }

  private static async Task<IReadOnlyList<PitRow>> ReadPitRowsAsync(PitGenerationModelContext context) {
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

  private static DateTimeOffset Utc(int year, int month, int day, int hour, int minute) {
    return new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.Zero);
  }

  private sealed class PitGenerationModelContext(DbContextOptions<PitGenerationModelContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      ConfigureSatelliteEntity(modelBuilder, "SatCustomerProfile", "Profile");
      ConfigureSatelliteEntity(modelBuilder, "SatCustomerStatu", "Status");

      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("PitCustomerProfileStatus", entityBuilder => {
        entityBuilder.ToTable("PitCustomerProfileStatus");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Pit);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "CustomerProfileStatus");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, DataVaultMetadataReferenceKind.Hub);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, "Customer");

        var parentHashKey = entityBuilder.IndexerProperty<string>("CustomerHashKey");
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Customer");

        var loadTimestamp = entityBuilder.IndexerProperty<DateTimeOffset>("LoadTimestamp");
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "LoadTimestamp");

        var profileSnapshot = entityBuilder.IndexerProperty<DateTimeOffset?>("ProfileLoadTimestamp");
        profileSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.SnapshotReference);
        profileSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);
        profileSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Profile");

        var statusSnapshot = entityBuilder.IndexerProperty<DateTimeOffset?>("StatusLoadTimestamp");
        statusSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.SnapshotReference);
        statusSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);
        statusSnapshot.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Status");

        entityBuilder.HasKey("CustomerHashKey", "LoadTimestamp");
      });
    }

    private static void ConfigureSatelliteEntity(
        ModelBuilder modelBuilder,
        string tableName,
        string satelliteName) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>(tableName, entityBuilder => {
        entityBuilder.ToTable(tableName);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Satellite);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, satelliteName);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceKind, DataVaultMetadataReferenceKind.Hub);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ParentReferenceName, "Customer");

        var parentHashKey = entityBuilder.IndexerProperty<string>("CustomerHashKey");
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
        parentHashKey.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "Customer");

        var loadTimestamp = entityBuilder.IndexerProperty<DateTimeOffset>("LoadTimestamp");
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.Technical);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.LoadTimestamp);
        loadTimestamp.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "LoadTimestamp");

        entityBuilder.HasKey("CustomerHashKey", "LoadTimestamp");
      });
    }
  }

  private sealed record PitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp,
      DateTimeOffset? StatusSnapshotTimestamp);
}
