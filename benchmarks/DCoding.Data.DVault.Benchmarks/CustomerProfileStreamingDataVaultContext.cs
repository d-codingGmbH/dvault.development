using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CustomerProfileStreamingDataVaultContext(
    DbContextOptions<CustomerProfileStreamingDataVaultContext> options,
    DataVaultProviderCapabilityProfile providerCapabilities) : DbContext(options) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(ScenarioContracts.CreateCustomerProfileDataVaultModel(), providerCapabilities);
  }
}
