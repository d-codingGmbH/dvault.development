using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultPitMaintenanceServiceTests {
  [Fact]
  public void PitParentMaintenanceRequestDeduplicatesParentHashKeysOrdinally() {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    var request = new DataVaultPitParentMaintenanceRequest(
        pit,
        ["customer-hash", "CUSTOMER-HASH", "customer-hash"]);

    Assert.Same(pit, request.Pit);
    Assert.Equal(["customer-hash", "CUSTOMER-HASH"], request.ParentHashKeys);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public void PitParentMaintenanceRequestRejectsNullEmptyOrWhitespaceParentHashKeys(string? parentHashKey) {
    var pit = new DataVaultPitMetadata(DataVaultMetadataReference.Hub("Customer"), ["Profile"]);

    Assert.ThrowsAny<ArgumentException>(() => new DataVaultPitParentMaintenanceRequest(
        pit,
        [parentHashKey!]));
  }

  [Fact]
  public void AddDVaultRegistersPitMaintenanceServiceBesideSaveAndReadServices() {
    using var provider = new ServiceCollection()
        .AddDVault()
        .BuildServiceProvider(validateScopes: true);

    Assert.IsType<DefaultDataVaultSaveService>(provider.GetRequiredService<IDataVaultSaveService>());
    Assert.IsType<DefaultDataVaultReadService>(provider.GetRequiredService<IDataVaultReadService>());
    Assert.IsType<DefaultDataVaultPitMaintenanceService>(provider.GetRequiredService<IDataVaultPitMaintenanceService>());
  }

  [Fact]
  public async Task EmptyParentMaintenanceRequestIsNoOpWithoutModelValidation() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new EmptyPitModelContext(new DbContextOptionsBuilder<EmptyPitModelContext>().Options);

    var result = await service.MaintainParentsAsync(
        context,
        new DataVaultPitParentMaintenanceRequest(CreateCustomerProfilePit(), []));

    Assert.Equal("CustomerProfile", result.Pit.Name);
    Assert.Equal("PitCustomerProfile", result.TableName);
    Assert.Equal(0, result.ParentHashKeyCount);
    Assert.Equal(0, result.RowsDeleted);
    Assert.Equal(0, result.RowsWritten);
    Assert.True(result.IsNoOp);
  }

  [Fact]
  public async Task PitMaintenanceRejectsUnsupportedShapesBeforeQuery() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new EmptyPitModelContext(new DbContextOptionsBuilder<EmptyPitModelContext>().Options);
    var linkParentPit = new DataVaultPitMetadata(DataVaultMetadataReference.Link("CustomerOrder"), ["State"]);
    var multiActivePit = new DataVaultPitMetadata(
        DataVaultMetadataReference.Hub("Customer"),
        [new DataVaultPitSatelliteReferenceMetadata("Profile", isMultiActive: true)]);

    var linkParentException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(linkParentPit)));
    var multiActiveException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(multiActivePit)));

    Assert.Contains("PIT metadata 'CustomerOrderState'", linkParentException.Message, StringComparison.Ordinal);
    Assert.Contains("link-based PIT tables", linkParentException.Message, StringComparison.Ordinal);
    Assert.Contains("PIT metadata 'CustomerProfile'", multiActiveException.Message, StringComparison.Ordinal);
    Assert.Contains("multi-active satellite 'Profile'", multiActiveException.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task PitMaintenanceFailsBeforeWriteWhenGeneratedSnapshotReferencePropertyIsMissing() {
    var service = new DefaultDataVaultPitMaintenanceService();
    await using var context = new MissingPitSnapshotPropertyContext(
        new DbContextOptionsBuilder<MissingPitSnapshotPropertyContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.RebuildAsync(
            context,
            new DataVaultPitRebuildRequest(CreateCustomerProfilePit())));

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
