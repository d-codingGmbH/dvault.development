using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class BenchmarkDataVaultModelCacheKeyFactory : IModelCacheKeyFactory {
  public object Create(DbContext context) {
    return Create(context, designTime: false);
  }

  public object Create(DbContext context, bool designTime) {
    ArgumentNullException.ThrowIfNull(context);

    if (context is not IBenchmarkDataVaultModelCacheKeySource source) {
      return (context.GetType(), designTime);
    }

    var hashKeyMapping = source.ProviderCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.HashKey);
    var participantMapping = source.ProviderCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.ParticipantReference);
    var loadTimestampMapping = source.ProviderCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.LoadTimestamp);
    var satelliteSnapshotMapping = source.ProviderCapabilities.GetRequiredTypeMapping(DataVaultLogicalPropertyKind.SatelliteSnapshotReference);

    return new BenchmarkDataVaultModelCacheKey(
        context.GetType(),
        designTime,
        source.ProviderCapabilities.ProfileName,
        hashKeyMapping.NativeStoreType,
        hashKeyMapping.ValueFormat,
        hashKeyMapping.HashKeyStorageProfile,
        hashKeyMapping.StableHashAlgorithmId,
        hashKeyMapping.DigestByteLength,
        participantMapping.NativeStoreType,
        participantMapping.ValueFormat,
        loadTimestampMapping.NativeStoreType,
        loadTimestampMapping.ValueFormat,
        satelliteSnapshotMapping.NativeStoreType,
        satelliteSnapshotMapping.ValueFormat);
  }

  private readonly record struct BenchmarkDataVaultModelCacheKey(
      Type ContextType,
      bool DesignTime,
      string ProviderProfileName,
      string HashKeyStoreType,
      DataVaultProviderValueFormat HashKeyValueFormat,
      DataVaultHashKeyStorageProfile? HashKeyStorageProfile,
      string? StableHashAlgorithmId,
      int? StableHashDigestByteLength,
      string ParticipantReferenceStoreType,
      DataVaultProviderValueFormat ParticipantReferenceValueFormat,
      string LoadTimestampStoreType,
      DataVaultProviderValueFormat LoadTimestampValueFormat,
      string SatelliteSnapshotStoreType,
      DataVaultProviderValueFormat SatelliteSnapshotValueFormat);
}
