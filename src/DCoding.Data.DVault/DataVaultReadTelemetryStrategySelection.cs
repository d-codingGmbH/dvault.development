using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed record DataVaultReadTelemetryStrategySelection(
    object? Strategy,
    DataVaultReadStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    IReadOnlyList<DataVaultReadStrategyFallbackCauseKind> FallbackCauseKinds) {
  public static DataVaultReadTelemetryStrategySelection NotEvaluated(string? providerName) {
    return new DataVaultReadTelemetryStrategySelection(
        Strategy: null,
        DataVaultReadStrategyDiagnosticsStatus.NotEvaluated,
        providerName,
        SelectedStrategyName: null,
        FallbackCauseKinds: Array.Empty<DataVaultReadStrategyFallbackCauseKind>());
  }
}
