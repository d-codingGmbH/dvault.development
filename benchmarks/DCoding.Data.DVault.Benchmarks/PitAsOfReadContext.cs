using System.Globalization;
using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class PitAsOfReadContext(
    DbContextOptions<PitAsOfReadContext> options,
    DataVaultProviderCapabilityProfile providerCapabilities)
    : CustomerProfileReadContext(options, providerCapabilities) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(
        PitReadScenario.Metadata.Model,
        ProviderCapabilities);
  }
}
