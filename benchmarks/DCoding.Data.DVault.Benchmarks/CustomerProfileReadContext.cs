using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal class CustomerProfileReadContext(
    DbContextOptions options,
    DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options), IBenchmarkDataVaultModelCacheKeySource {
  public DataVaultProviderCapabilityProfile ProviderCapabilities { get; } = providerCapabilities;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(
        ScenarioContracts.CreateCustomerProfileDataVaultModel(),
        ProviderCapabilities);
  }
}
