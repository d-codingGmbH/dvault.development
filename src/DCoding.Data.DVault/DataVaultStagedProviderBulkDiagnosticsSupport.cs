using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultStagedProviderBulkDiagnosticsSupport {
  public static DataVaultStagedProviderBulkDiagnostics? TryEvaluate(
      IDataVaultProviderSaveStrategy strategy,
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(strategy);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    if (strategy is not IDataVaultProviderStagedBulkSaveDiagnostics stagedDiagnostics) {
      return null;
    }

    try {
      return stagedDiagnostics.EvaluateStagedProviderBulkSave(dbContext, requests);
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      var counts = DataVaultTelemetrySummaryFactory.CountSaveRequests(requests);
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.EligibilityEvaluation,
          DataVaultStagedProviderBulkProviderCaveatKind.ProviderLimitation,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkProviderLimitation]);
    }
  }

  public static bool IsStagedProviderBulkFallbackCause(DataVaultSaveStrategyFallbackCauseKind kind) {
    return kind is
        DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkDirtyDbContext or
        DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape or
        DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkTransactionParticipationUnsupported or
        DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkCleanupFailed or
        DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkProviderLimitation;
  }

  public static IReadOnlyList<DataVaultSaveStrategyFallbackCause> CreateFallbackCauses(
      DataVaultStagedProviderBulkDiagnostics? diagnostics) {
    return diagnostics is null
        ? Array.Empty<DataVaultSaveStrategyFallbackCause>()
        : diagnostics.FallbackCauseKinds.Select(CreateFallbackCause).ToArray();
  }

  public static DataVaultStagedProviderBulkDiagnostics? SelectRepresentative(
      IEnumerable<DataVaultSaveStrategyCandidateDiagnostics> candidates) {
    ArgumentNullException.ThrowIfNull(candidates);

    return candidates
        .Select(candidate => candidate.StagedProviderBulk)
        .FirstOrDefault(diagnostics => diagnostics is not null);
  }

  public static DataVaultSaveStrategyFallbackCause CreateFallbackCause(DataVaultSaveStrategyFallbackCauseKind kind) {
    return kind switch {
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkDirtyDbContext => new(
          kind,
          "Staged-provider bulk execution declined because the DbContext change tracker contains pending added, modified, or deleted state."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape => new(
          kind,
          "Staged-provider bulk execution declined because the request batch contains a shape outside the staged provider contract."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkTransactionParticipationUnsupported => new(
          kind,
          "Staged-provider bulk execution declined because the staged path could not participate in the caller-owned transaction contract."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkCleanupFailed => new(
          kind,
          "Staged-provider bulk execution fell back because transient staging cleanup did not complete safely."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkProviderLimitation => new(
          kind,
          "Staged-provider bulk execution declined because a bounded provider limitation prevented the staged path."),
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown staged-provider bulk fallback cause kind."),
    };
  }
}
