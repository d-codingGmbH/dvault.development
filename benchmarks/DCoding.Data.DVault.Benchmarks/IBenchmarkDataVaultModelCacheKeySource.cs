using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Benchmarks;

internal interface IBenchmarkDataVaultModelCacheKeySource {
  DataVaultProviderCapabilityProfile ProviderCapabilities { get; }
}
