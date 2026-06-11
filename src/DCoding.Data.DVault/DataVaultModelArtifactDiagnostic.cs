using System.Text.Json;
using DCoding.Data.DVault.Modeling;

namespace DCoding.Data.DVault;

internal sealed record DataVaultModelArtifactDiagnostic(
    DataVaultDiagnosticDefinition Definition,
    string Message,
    string Path) {
  public string Severity => Definition.Severity;

  public string Category => Definition.Category;

  public string Code => Definition.Code;
}
