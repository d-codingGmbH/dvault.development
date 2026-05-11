using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DCoding.Data.DVault.Tests.Unit;

public sealed class DataVaultBridgeReadServiceTests {
  [Fact]
  public void BridgeReadRequestRejectsUnsupportedEndpointAndDepthShapes() {
    var manyToManyBridge = CreateManyToManyBridge();
    var hierarchyBridge = CreateHierarchyBridge();

    var manyToManyEndpointException = Assert.Throws<ArgumentException>(() =>
        new DataVaultBridgeReadRequest(
            manyToManyBridge,
            DataVaultBridgeTraversalEndpoint.Ancestor,
            ["customer-hash"]));
    var manyToManyDepthException = Assert.Throws<ArgumentException>(() =>
        new DataVaultBridgeReadRequest(
            manyToManyBridge,
            DataVaultBridgeTraversalEndpoint.From,
            ["customer-hash"],
            maximumDepth: 1));
    var hierarchyDepthException = Assert.Throws<ArgumentException>(() =>
        new DataVaultBridgeReadRequest(
            hierarchyBridge,
            DataVaultBridgeTraversalEndpoint.Ancestor,
            ["region-hash"]));
    var hierarchyNegativeDepthException = Assert.Throws<ArgumentOutOfRangeException>(() =>
        new DataVaultBridgeReadRequest(
            hierarchyBridge,
            DataVaultBridgeTraversalEndpoint.Ancestor,
            ["region-hash"],
            maximumDepth: -1));

    Assert.Contains("many-to-many bridge 'CustomerOrder'", manyToManyEndpointException.Message, StringComparison.Ordinal);
    Assert.Contains("endpoint From or To", manyToManyEndpointException.Message, StringComparison.Ordinal);
    Assert.Contains("many-to-many bridge 'CustomerOrder'", manyToManyDepthException.Message, StringComparison.Ordinal);
    Assert.Contains("does not support hierarchy depth", manyToManyDepthException.Message, StringComparison.Ordinal);
    Assert.Contains("hierarchy bridge 'SalesRegionHierarchy'", hierarchyDepthException.Message, StringComparison.Ordinal);
    Assert.Contains("bounded maximum depth", hierarchyDepthException.Message, StringComparison.Ordinal);
    Assert.Equal("maximumDepth", hierarchyNegativeDepthException.ParamName);
  }

  [Fact]
  public async Task BridgeReadFailsBeforeQueryWhenGeneratedEntityIsMissing() {
    var readService = new DefaultDataVaultReadService();
    await using var context = new EmptyBridgeModelContext(
        new DbContextOptionsBuilder<EmptyBridgeModelContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadBridgeRowsAsync(
            context,
            new DataVaultBridgeReadRequest(
                CreateManyToManyBridge(),
                DataVaultBridgeTraversalEndpoint.From,
                ["customer-hash"])));

    Assert.Contains("bridge metadata 'CustomerOrder'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("generated bridge table/entity 'BridgeCustomerOrder'", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task BridgeReadFailsBeforeQueryWhenGeneratedPropertyIsMissing() {
    var readService = new DefaultDataVaultReadService();
    await using var context = new MissingBridgePropertyContext(
        new DbContextOptionsBuilder<MissingBridgePropertyContext>()
            .UseSqlite("Data Source=:memory:")
            .Options);

    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        readService.ReadBridgeRowsAsync(
            context,
            new DataVaultBridgeReadRequest(
                CreateManyToManyBridge(),
                DataVaultBridgeTraversalEndpoint.From,
                ["customer-hash"])));

    Assert.Contains("bridge metadata 'CustomerOrder'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("generated bridge property 'OrderHashKey'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("BridgeCustomerOrder", exception.Message, StringComparison.Ordinal);
  }

  [Fact]
  public async Task BridgeReadRejectsUnsupportedProjectionFeaturesBeforeQuery() {
    var readService = new DefaultDataVaultReadService();
    var bridge = new DataVaultBridgeMetadata(
        "CustomerOrder",
        DataVaultBridgeKind.ManyToMany,
        DataVaultMetadataReference.Link("CustomerOrder"),
        [
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.From,
                DataVaultMetadataReference.Hub("Customer"),
                "Customer"),
            new DataVaultBridgeEndpointMetadata(
                DataVaultBridgeEndpointRole.To,
                DataVaultMetadataReference.Hub("Order"),
                "Order"),
        ],
        DataVaultBridgeProjectionFeatures.PathPayload);
    await using var context = new EmptyBridgeModelContext(new DbContextOptionsBuilder<EmptyBridgeModelContext>().Options);

    var exception = await Assert.ThrowsAsync<NotSupportedException>(() =>
        readService.ReadBridgeRowsAsync(
            context,
            new DataVaultBridgeReadRequest(
                bridge,
                DataVaultBridgeTraversalEndpoint.From,
                ["customer-hash"])));

    Assert.Contains("bridge metadata 'CustomerOrder'", exception.Message, StringComparison.Ordinal);
    Assert.Contains("PathPayload", exception.Message, StringComparison.Ordinal);
    Assert.Contains("TraversalDepth", exception.Message, StringComparison.Ordinal);
  }

  private static DataVaultBridgeMetadata CreateManyToManyBridge() {
    return DataVaultBridgeMetadata.ManyToMany(
        "CustomerOrder",
        DataVaultMetadataReference.Hub("Customer"),
        DataVaultMetadataReference.Link("CustomerOrder"),
        DataVaultMetadataReference.Hub("Order"));
  }

  private static DataVaultBridgeMetadata CreateHierarchyBridge() {
    return DataVaultBridgeMetadata.Hierarchy(
        "SalesRegionHierarchy",
        DataVaultMetadataReference.Hub("SalesRegion"),
        DataVaultMetadataReference.Link("SalesRegionParentChild"),
        DataVaultMetadataReference.Hub("SalesRegion"),
        ancestorParticipantOrdinal: 0,
        descendantParticipantOrdinal: 1);
  }

  private sealed class EmptyBridgeModelContext(DbContextOptions<EmptyBridgeModelContext> options) : DbContext(options) {
  }

  private sealed class MissingBridgePropertyContext(DbContextOptions<MissingBridgePropertyContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.SharedTypeEntity<Dictionary<string, object>>("BridgeCustomerOrder", entityBuilder => {
        entityBuilder.ToTable("BridgeCustomerOrder");
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Bridge);
        entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "CustomerOrder");
        var property = entityBuilder.IndexerProperty<string>("CustomerHashKey");
        property.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.ParticipantReference);
        entityBuilder.HasKey("CustomerHashKey");
      });
    }
  }
}
