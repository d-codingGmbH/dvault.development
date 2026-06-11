using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Bounded provider threshold fact derived from known provider save-strategy gates.
/// </summary>
public sealed record DataVaultProviderThresholdFact(
    DataVaultProviderThresholdFactKind Kind,
    DataVaultSaveStrategyFallbackCauseKind GateKind,
    string ProviderName,
    string Message) {
  /// <summary>
  /// Gets the minimum total operation count when the threshold is a minimum-operation gate.
  /// </summary>
  public int? MinimumTotalOperationCount { get; init; }

  /// <summary>
  /// Gets the maximum satellite operation count when the threshold is a maximum-satellite gate.
  /// </summary>
  public int? MaximumSatelliteOperationCount { get; init; }
}
