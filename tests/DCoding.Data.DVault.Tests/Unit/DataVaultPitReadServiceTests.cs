using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPitReadServiceTests {
  [Fact]
  public void PitReadRequestNormalizesAsOfAndDeduplicatesParentHashKeysOrdinally() {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
    var asOf = new DateTimeOffset(2026, 5, 11, 14, 0, 0, TimeSpan.FromHours(2));

    var request = new DataVaultPitAsOfReadRequest(
        pit,
        ["customer-hash", "CUSTOMER-HASH", "customer-hash"],
        asOf);

    Assert.Same(pit, request.Pit);
    Assert.Equal(["customer-hash", "CUSTOMER-HASH"], request.ParentHashKeys);
    Assert.Equal(new DateTimeOffset(2026, 5, 11, 12, 0, 0, TimeSpan.Zero), request.AsOf);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public void PitReadRequestRejectsNullEmptyOrWhitespaceParentHashKeys(string? parentHashKey) {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    Assert.ThrowsAny<ArgumentException>(() => new DataVaultPitAsOfReadRequest(
        pit,
        [parentHashKey!],
        DateTimeOffset.UtcNow));
  }

  [Fact]
  public async Task PitReadFailsBeforeQueryWhenGeneratedPitEntityIsMissing() {
    var readService = new DefaultDataVaultReadService();
    await using var context = new EmptyPitModelContext(
        new DbContextOptionsBuilder<EmptyPitModelContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadPitRowsAsync(
            context,
            new DataVaultPitAsOfReadRequest(CreateCustomerProfilePit(), ["customer-hash"], DateTimeOffset.UtcNow)));

    Assert.Contains("PIT metadata 'CustomerProfile'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("generated PIT table/entity 'PitCustomerProfile'", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitReadRejectsUnsupportedShapesBeforeQuery() {
    var readService = new DefaultDataVaultReadService();
    await using var context = new EmptyPitModelContext(new DbContextOptionsBuilder<EmptyPitModelContext>().Options);
    var linkParentPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);
    var multiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Profile", isMultiActive: true)]);

    var linkParentException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadPitRowsAsync(
            context,
            new DataVaultPitAsOfReadRequest(linkParentPit, ["link-hash"], DateTimeOffset.UtcNow)));
    var multiActiveException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadPitRowsAsync(
            context,
            new DataVaultPitAsOfReadRequest(multiActivePit, ["customer-hash"], DateTimeOffset.UtcNow)));

    Assert.Contains("PIT metadata 'CustomerOrderState'", linkParentException.Message, StringComparison.Ordinal);
    Assert.Contains("link-based PIT tables", linkParentException.Message, StringComparison.Ordinal);
    Assert.Contains("PIT metadata 'CustomerProfile'", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("multi-active satellite 'Profile'", multiActiveException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitReadFailsBeforeQueryWhenGeneratedSnapshotReferencePropertyIsMissing() {
    var readService = new DefaultDataVaultReadService();
    await using var context = new MissingPitSnapshotPropertyContext(
        new DbContextOptionsBuilder<MissingPitSnapshotPropertyContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadPitRowsAsync(
            context,
            new DataVaultPitAsOfReadRequest(CreateCustomerProfilePit(), ["customer-hash"], DateTimeOffset.UtcNow)));

    Assert.Contains("PIT metadata 'CustomerProfile'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("satellite snapshot reference property", exception.Message, StringComparison.Ordinal);
    Assert.Contains("metadata name 'Profile'", exception.Message, StringComparison.Ordinal);
  }

  private static DataVaultPitMetadata CreateCustomerProfilePit() {
    return new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);
  }

  private sealed class EmptyPitModelContext(DbContextOptions<EmptyPitModelContext> options) : DbContext(options) {
  }

  private sealed class MissingPitSnapshotPropertyContext(DbContextOptions<MissingPitSnapshotPropertyContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("PitCustomerProfile", entityBuilder => {
        entityBuilder.ToTable("PitCustomerProfile");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Pit);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "CustomerProfile");
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
}
