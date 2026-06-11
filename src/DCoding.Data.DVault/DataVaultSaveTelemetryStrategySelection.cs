using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed record DataVaultSaveTelemetryStrategySelection(
    IDataVaultProviderSaveStrategy? Strategy,
    DataVaultSaveStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    IReadOnlyList<DataVaultSaveStrategyFallbackCauseKind> FallbackCauseKinds,
    DataVaultStagedProviderBulkDiagnostics? StagedProviderBulk = null) {
  public static DataVaultSaveTelemetryStrategySelection NotEvaluated(string? providerName) {
    return new DataVaultSaveTelemetryStrategySelection(
        Strategy: null,
        DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
        providerName,
        SelectedStrategyName: null,
        FallbackCauseKinds: Array.Empty<DataVaultSaveStrategyFallbackCauseKind>(),
        StagedProviderBulk: null);
  }
}
