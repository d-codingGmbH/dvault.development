using DCoding.Data.DVault;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace DCoding.Data.DVault.Benchmarks;

internal sealed class CompiledEvidenceContext(
    DbContextOptions<CompiledEvidenceContext> options,
    DataVaultMetadataModel metadataModel,
    object modelCacheKey) : DbContext(options) {
  public DataVaultMetadataModel MetadataModel { get; } = metadataModel;

  public object ModelCacheKey { get; } = modelCacheKey;

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyDataVaultMetadata(MetadataModel);
  }
}
