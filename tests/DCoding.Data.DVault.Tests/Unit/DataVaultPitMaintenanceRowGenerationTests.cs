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

  [Fact]
  public async Task RebuildCreatesTupleAwarePitRowsAfterMultiActiveTupleFirstAppears() {
    var service = new DefaultDataVaultPitMaintenanceService();
    var metadata = CreateTuplePitMetadata();
    using var database = SqliteTestDatabase.CreateTemporaryFile();
    var options = new DbContextOptionsBuilder<TuplePitGenerationModelContext>()
        .UseSqlite("Data Source=" + Assert.IsType<string>(database.DatabasePath) + ";Pooling=False")
        .Options;
    var profileBeforeTuple = Utc(2026, 5, 11, 8, 0);
    var billingContact = Utc(2026, 5, 11, 9, 0);
    var shippingContact = Utc(2026, 5, 11, 10, 0);
    var profileAfterTuple = Utc(2026, 5, 11, 11, 0);

    await using (var context = new TuplePitGenerationModelContext(options)) {
      await context.Database.EnsureCreatedAsync();
      AddTupleProfileRow(context, "customer-a", profileBeforeTuple, "Alice");
      AddTupleContactRow(context, "customer-a", "billing", billingContact, "billing@example.test");
      AddTupleContactRow(context, "customer-a", "shipping", shippingContact, "shipping@example.test");
      AddTupleProfileRow(context, "customer-a", profileAfterTuple, "Alice Updated");
      await context.SaveChangesAsync();

      var result = await service.RebuildAsync(context, new DataVaultPitRebuildRequest(metadata.Pit));

      Assert.Equal("PitCustomerContactProfile", result.TableName);
      Assert.Equal(1, result.ParentHashKeyCount);
      Assert.Equal(0, result.RowsDeleted);
      Assert.Equal(4, result.RowsWritten);
    }

    await using (var context = new TuplePitGenerationModelContext(options)) {
      var pitRows = await ReadTuplePitRowsAsync(context);

      Assert.Collection(
          pitRows,
          row => AssertTuplePitRow(row, "customer-a", "billing", billingContact, billingContact, profileBeforeTuple),
          row => AssertTuplePitRow(row, "customer-a", "billing", profileAfterTuple, billingContact, profileAfterTuple),
          row => AssertTuplePitRow(row, "customer-a", "shipping", shippingContact, shippingContact, profileBeforeTuple),
          row => AssertTuplePitRow(row, "customer-a", "shipping", profileAfterTuple, shippingContact, profileAfterTuple));
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

  private static void AddTupleContactRow(
      DbContext context,
      string parentHashKey,
      string contactType,
      DateTimeOffset loadTimestamp,
      string emailAddress) {
    context.Set<Dictionary<string, object>>("SatCustomerContact").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["ContactType"] = contactType,
      ["HashDiff"] = "contact-" + contactType + "-" + loadTimestamp.ToUnixTimeSeconds(),
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "test",
      ["EmailAddress"] = emailAddress,
    });
  }

  private static void AddTupleProfileRow(
      DbContext context,
      string parentHashKey,
      DateTimeOffset loadTimestamp,
      string customerName) {
    context.Set<Dictionary<string, object>>("SatCustomerProfile").Add(new Dictionary<string, object>(StringComparer.Ordinal) {
      ["CustomerHashKey"] = parentHashKey,
      ["HashDiff"] = "profile-" + loadTimestamp.ToUnixTimeSeconds(),
      ["LoadTimestamp"] = loadTimestamp,
      ["RecordSource"] = "test",
      ["CustomerName"] = customerName,
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

  private static async Task<IReadOnlyList<TuplePitRow>> ReadTuplePitRowsAsync(TuplePitGenerationModelContext context) {
    var rows = await context
        .Set<Dictionary<string, object>>("PitCustomerContactProfile")
        .AsNoTracking()
        .ToListAsync();

    return rows
        .Select(row => new TuplePitRow(
            Assert.IsType<string>(row["CustomerHashKey"]),
            Assert.IsType<string>(row["ContactType"]),
            DataVaultLoadTimestampValueConverter.ReadProviderValue(row["LoadTimestamp"]),
            ReadOptionalTimestamp(row, "ContactLoadTimestamp"),
            ReadOptionalTimestamp(row, "ProfileLoadTimestamp")))
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.ContactType, StringComparer.Ordinal)
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

  private static void AssertTuplePitRow(
      TuplePitRow row,
      string parentHashKey,
      string contactType,
      DateTimeOffset loadTimestamp,
      DateTimeOffset? contactSnapshotTimestamp,
      DateTimeOffset? profileSnapshotTimestamp) {
    Assert.Equal(parentHashKey, row.ParentHashKey);
    Assert.Equal(contactType, row.ContactType);
    Assert.Equal(loadTimestamp, row.LoadTimestamp);
    Assert.Equal(contactSnapshotTimestamp, row.ContactSnapshotTimestamp);
    Assert.Equal(profileSnapshotTimestamp, row.ProfileSnapshotTimestamp);
  }

  private static TuplePitGenerationMetadata CreateTuplePitMetadata() {
    var customer = new DataVaultHubMetadata("Customer", ["Customer Id"]);
    var contact = new DataVaultSatelliteMetadata(
        "Contact",
        customer.ToReference(),
        ["Email Address"],
        ["Contact Type"]);
    var profile = new DataVaultSatelliteMetadata(
        "Profile",
        customer.ToReference(),
        ["Customer Name"]);
    var pit = new DataVaultPitMetadata(
        customer.ToReference(),
        [
            new DataVaultPitSatelliteReferenceMetadata("Contact", isMultiActive: true),
            new DataVaultPitSatelliteReferenceMetadata("Profile"),
        ]);
    var model = new DataVaultMetadataModel([customer], [], [contact, profile], [pit]);

    return new TuplePitGenerationMetadata(pit, model);
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

  private sealed class TuplePitGenerationModelContext(DbContextOptions<TuplePitGenerationModelContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.ApplyDataVaultMetadata(
          CreateTuplePitMetadata().Model,
          DataVaultProviderCapabilityProfiles.Sqlite.WithHashKeyStorageProfile(
              DataVaultHashKeyStorageProfile.HexString,
              "sha256-v1",
              32));
    }
  }

  private sealed record TuplePitGenerationMetadata(
      DataVaultPitMetadata Pit,
      DataVaultMetadataModel Model);

  private sealed record PitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp,
      DateTimeOffset? StatusSnapshotTimestamp);

  private sealed record TuplePitRow(
      string ParentHashKey,
      string ContactType,
      DateTimeOffset LoadTimestamp,
      DateTimeOffset? ContactSnapshotTimestamp,
      DateTimeOffset? ProfileSnapshotTimestamp);
}
