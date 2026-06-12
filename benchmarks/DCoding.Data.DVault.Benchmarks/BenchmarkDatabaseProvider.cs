using System.Data.Common;
using System.Reflection;
using DCoding.Data.DVault;
using Microsoft.EntityFrameworkCore;

#pragma warning disable EF1003 // Benchmark cleanup uses fixed produced table names plus provider quoting helpers.

namespace DCoding.Data.DVault.Benchmarks;

internal abstract class BenchmarkDatabaseProvider {
  protected BenchmarkDatabaseProvider(string providerName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(providerName);

    ProviderName = providerName;
  }

  public string ProviderName { get; }

  public virtual DataVaultProviderCapabilityProfile ProviderCapabilities => DataVaultProviderCapabilityProfiles.Sqlite;

  public DataVaultProviderCapabilityProfile GetProviderCapabilities(DataVaultLoadTimestampStorage loadTimestampStorage) {
    return ProviderCapabilities.WithLoadTimestampStorage(loadTimestampStorage);
  }

  public DataVaultProviderCapabilityProfile GetProviderCapabilities(
      DataVaultLoadTimestampStorage loadTimestampStorage,
      BenchmarkHashKeyVariant hashKeyVariant) {
    ArgumentNullException.ThrowIfNull(hashKeyVariant);

    return ProviderCapabilities
        .WithLoadTimestampStorage(loadTimestampStorage)
        .WithHashKeyStorageProfile(
            hashKeyVariant.StorageProfile,
            hashKeyVariant.StableHashAlgorithmId,
            hashKeyVariant.DigestByteLength);
  }

  public abstract IBenchmarkDatabase CreateDatabase();
}
