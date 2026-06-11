using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

/// <summary>
/// Machine-readable diagnostics for request-bound provider-specific save-strategy dispatch.
/// </summary>
public sealed record DataVaultSaveStrategyDiagnostics(
    DataVaultSaveStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    int? SelectedStrategyPriority,
    IReadOnlyList<DataVaultSaveStrategyCandidateDiagnostics> Candidates,
    IReadOnlyList<DataVaultSaveStrategyFallbackCause> FallbackCauses) {
  /// <summary>
  /// Gets representative bounded staged-provider bulk diagnostics, when staged evaluation participated in strategy dispatch.
  /// </summary>
  public DataVaultStagedProviderBulkDiagnostics? StagedProviderBulk { get; init; }
}
