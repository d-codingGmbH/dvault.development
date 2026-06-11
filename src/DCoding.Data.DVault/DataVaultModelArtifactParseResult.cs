using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelArtifactParseResult(
    DataVaultModelArtifact? Artifact,
    DataVaultMetadataModel? MetadataModel,
    DataVaultMetadataRegistry? MetadataRegistry,
    IReadOnlyList<DataVaultModelArtifactDiagnostic> Diagnostics) {
  public bool IsValid => Diagnostics.All(
      diagnostic => !string.Equals(diagnostic.Severity, "error", StringComparison.Ordinal));

  public DataVaultLoadTimestampStorage LoadTimestampStorage =>
      Artifact?.LoadTimestampStorage ?? DataVaultLoadTimestampStorage.ProviderDefault;
}
