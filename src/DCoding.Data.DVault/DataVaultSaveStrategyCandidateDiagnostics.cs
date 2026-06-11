using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for one provider-specific save-strategy candidate.
/// </summary>
public sealed record DataVaultSaveStrategyCandidateDiagnostics(
    int Ordinal,
    string StrategyName,
    int Priority,
    bool CanSave,
    IReadOnlyList<DataVaultSaveStrategyFallbackCause> FallbackCauses) {
  /// <summary>
  /// Gets the provider names this candidate declares as eligible, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<string> SupportedProviderNames { get; init; } = Array.Empty<string>();

  /// <summary>
  /// Gets the bounded eligibility gates this candidate declares, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<DataVaultSaveStrategyGateRequirement> GateRequirements { get; init; } =
      Array.Empty<DataVaultSaveStrategyGateRequirement>();

  /// <summary>
  /// Gets bounded staged-provider bulk diagnostics reported by this candidate, when applicable.
  /// </summary>
  public DataVaultStagedProviderBulkDiagnostics? StagedProviderBulk { get; init; }
}
