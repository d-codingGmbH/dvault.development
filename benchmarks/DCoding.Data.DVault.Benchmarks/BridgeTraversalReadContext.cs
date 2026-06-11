using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class BridgeTraversalReadContext(
    DbContextOptions<BridgeTraversalReadContext> options,
    DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.SharedTypeEntity<Dictionary<string, object>>("BridgeSalesRegionHierarchy", entityBuilder => {
      entityBuilder.ToTable("BridgeSalesRegionHierarchy");
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "BridgeSalesRegionHierarchy");
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.EntityKind, DataVaultTableKind.Bridge);
      entityBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "SalesRegionHierarchy");

      ConfigureBridgeEndpointProperty(entityBuilder, "AncestorSalesRegionHashKey", "SalesRegion", 0);
      ConfigureBridgeEndpointProperty(entityBuilder, "DescendantSalesRegionHashKey", "SalesRegion", 1);
      ConfigureBridgeDepthProperty(entityBuilder);

      entityBuilder.HasKey("AncestorSalesRegionHashKey", "DescendantSalesRegionHashKey")
          .HasName("PkBridgeSalesRegionHierarchy");
      entityBuilder.HasIndex("AncestorSalesRegionHashKey", "TraversalDepth")
          .HasDatabaseName("IxBridgeRegionAncestorDepth");
      entityBuilder.HasIndex("DescendantSalesRegionHashKey", "AncestorSalesRegionHashKey")
          .HasDatabaseName("IxBridgeRegionDescAncestor");
    });
  }

  private void ConfigureBridgeEndpointProperty(
      EntityTypeBuilder<Dictionary<string, object>> entityBuilder,
      string propertyName,
      string metadataName,
      int ordinal) {
    var mapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.ParticipantReference);
    var propertyBuilder = entityBuilder.IndexerProperty<string>(propertyName);

    propertyBuilder.HasColumnName(propertyName);
    propertyBuilder.HasColumnType(mapping.NativeStoreType);
    propertyBuilder.HasColumnOrder(ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, propertyName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.ParticipantReference);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, metadataName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, providerCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, DataVaultLogicalPropertyKind.ParticipantReference);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, mapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, mapping.ValueFormat);
  }

  private void ConfigureBridgeDepthProperty(EntityTypeBuilder<Dictionary<string, object>> entityBuilder) {
    var mapping = providerCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.BridgeDepth);
    var propertyBuilder = entityBuilder.IndexerProperty<int>("TraversalDepth");

    propertyBuilder.HasColumnName("TraversalDepth");
    propertyBuilder.HasColumnType(mapping.NativeStoreType);
    propertyBuilder.HasColumnOrder(2);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "TraversalDepth");
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.BridgeDepth);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "TraversalDepth");
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, 2);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, providerCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, DataVaultLogicalPropertyKind.BridgeDepth);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, mapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, mapping.ValueFormat);
  }
}
