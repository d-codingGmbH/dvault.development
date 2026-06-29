using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

/// <summary>
/// Supplies optional-package encrypted-payload alias facts to core diagnostics without adding a core dependency on privacy concrete types.
/// </summary>
public interface IDataVaultPrivacyAliasCoverageProvider {
  /// <summary>
  /// Evaluates registered encrypted-payload aliases against the optional EF model.
  /// </summary>
  /// <param name="model">The EF model to inspect, or null when diagnostics only have metadata.</param>
  /// <returns>A deterministic redaction-safe alias coverage report.</returns>
  DataVaultPrivacyAliasCoverageReport Analyze(IReadOnlyModel? model);
}
