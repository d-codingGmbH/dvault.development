using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for one provider-specific read-strategy candidate.
/// </summary>
public sealed record DataVaultReadStrategyCandidateDiagnostics(
    int Ordinal,
    string StrategyName,
    int Priority,
    bool CanRead,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> FallbackCauses) {
  /// <summary>
  /// Gets the provider names this candidate declares as eligible, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<string> SupportedProviderNames { get; init; } = Array.Empty<string>();

  /// <summary>
  /// Gets the bounded eligibility gates this candidate declares, when the strategy is known to DVault diagnostics.
  /// </summary>
  public IReadOnlyList<DataVaultReadStrategyGateRequirement> GateRequirements { get; init; } =
      Array.Empty<DataVaultReadStrategyGateRequirement>();
}
