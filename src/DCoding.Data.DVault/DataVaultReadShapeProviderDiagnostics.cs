using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Provider caveat and fallback facts attached to request-bound read-shape diagnostics.
/// </summary>
public sealed record DataVaultReadShapeProviderDiagnostics(
    string? ProviderName,
    string CapabilityProfileName,
    bool CapabilityProfileDefaulted,
    string ProviderBehaviorProfileName,
    bool ProviderBehaviorDefaulted,
    DataVaultReadStrategyDiagnosticsStatus ReadStrategyStatus,
    IReadOnlyList<DataVaultReadStrategyFallbackCause> ReadStrategyFallbackCauses) {
  /// <summary>
  /// Gets the selected provider-specific read strategy name when a provider strategy accepted the request.
  /// </summary>
  public string? SelectedStrategyName { get; init; }

  /// <summary>
  /// Gets bounded performance-profile recommendation context for this provider/read-shape evaluation.
  /// </summary>
  public DataVaultProviderTuningRecommendation? Recommendation { get; init; }
}
