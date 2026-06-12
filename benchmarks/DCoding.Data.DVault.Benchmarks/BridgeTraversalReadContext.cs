using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class BridgeTraversalReadContext(
    DbContextOptions<BridgeTraversalReadContext> options,
    DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
  public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

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
    var mapping = ProviderCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.ParticipantReference);
    var propertyBuilder = entityBuilder.IndexerProperty<string>(propertyName);

    propertyBuilder.HasColumnName(propertyName);
    propertyBuilder.HasColumnType(mapping.NativeStoreType);
    if (mapping.ValueFormat == DataVaultProviderValueFormat.LowercaseHexBinary) {
      var digestByteLength = mapping.DigestByteLength ??
          throw new InvalidOperationException("Binary bridge hash-key storage requires a stable-hash digest byte length.");
      propertyBuilder.HasConversion(new ValueConverter<string, byte[]>(
          value => ConvertCanonicalHexToBytes(value, digestByteLength),
          value => ConvertBytesToCanonicalHex(value, digestByteLength)));
    }

    propertyBuilder.HasColumnOrder(ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, propertyName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.ParticipantReference);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.TechnicalColumnRole, TechnicalMetadataColumnRole.HashKey);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, metadataName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, ordinal);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, ProviderCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, DataVaultLogicalPropertyKind.ParticipantReference);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, mapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, mapping.ValueFormat);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.HashKeyStorageProfile, mapping.HashKeyStorageProfile);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.StableHashAlgorithmId, mapping.StableHashAlgorithmId);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.StableHashDigestByteLength, mapping.DigestByteLength);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.StableHashDigestEncoding, mapping.DigestEncoding);
  }

  private void ConfigureBridgeDepthProperty(EntityTypeBuilder<Dictionary<string, object>> entityBuilder) {
    var mapping = ProviderCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.BridgeDepth);
    var propertyBuilder = entityBuilder.IndexerProperty<int>("TraversalDepth");

    propertyBuilder.HasColumnName("TraversalDepth");
    propertyBuilder.HasColumnType(mapping.NativeStoreType);
    propertyBuilder.HasColumnOrder(2);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProducedName, "TraversalDepth");
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.PropertyRole, DataVaultPropertyRole.BridgeDepth);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.MetadataName, "TraversalDepth");
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.Ordinal, 2);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderProfile, ProviderCapabilities.ProfileName);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderLogicalPropertyKind, DataVaultLogicalPropertyKind.BridgeDepth);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderStorageType, mapping.NativeStoreType);
    propertyBuilder.Metadata.SetAnnotation(DataVaultAnnotationNames.ProviderValueFormat, mapping.ValueFormat);
  }

  private static byte[] ConvertCanonicalHexToBytes(string value, int digestByteLength) {
    ArgumentNullException.ThrowIfNull(value);

    if (!DataVaultBenchmarkHelpers.IsLowercaseHexDigest(value, digestByteLength)) {
      throw new FormatException(
          "Bridge benchmark binary hash-key storage requires canonical lowercase hexadecimal values for the active stable-hash digest.");
    }

    return Convert.FromHexString(value);
  }

  private static string ConvertBytesToCanonicalHex(byte[] value, int digestByteLength) {
    ArgumentNullException.ThrowIfNull(value);

    if (value.Length != digestByteLength) {
      throw new FormatException(
          "Bridge benchmark binary hash-key storage read an unexpected provider digest byte length.");
    }

    return Convert.ToHexString(value).ToLowerInvariant();
  }
}
