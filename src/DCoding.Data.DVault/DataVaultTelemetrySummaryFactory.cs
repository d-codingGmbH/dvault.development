using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultTelemetrySummaryFactory {
  public static DataVaultSaveTelemetrySummary CreateSaveSummary(
      DataVaultSaveTelemetryOperationKind operationKind,
      DataVaultTelemetryOutcome outcome,
      IReadOnlyList<DataVaultSaveRequest> requests,
      DataVaultSaveResult? result,
      TimeSpan duration,
      DataVaultSaveTelemetryStrategySelection strategySelection) {
    return CreateSaveSummary(
        operationKind,
        outcome,
        CountSaveRequests(requests),
        result,
        duration,
        strategySelection,
        DataVaultChunkedSaveTelemetryState.Empty);
  }

  public static DataVaultSaveTelemetrySummary CreateSaveSummary(
      DataVaultSaveTelemetryOperationKind operationKind,
      DataVaultTelemetryOutcome outcome,
      DataVaultSaveTelemetryCounts counts,
      DataVaultSaveResult? result,
      TimeSpan duration,
      DataVaultSaveTelemetryStrategySelection strategySelection,
      DataVaultChunkedSaveTelemetryState chunkedState) {
    ArgumentNullException.ThrowIfNull(chunkedState);

    return new DataVaultSaveTelemetrySummary(
        operationKind,
        outcome,
        counts.RequestCount,
        counts.HubOperationCount,
        counts.LinkOperationCount,
        counts.SatelliteOperationCount,
        result?.RowsWritten ?? 0,
        result?.SavedRecords.Count ?? 0,
        duration,
        strategySelection.Status,
        strategySelection.ProviderName,
        strategySelection.SelectedStrategyName,
        strategySelection.FallbackCauseKinds,
        chunkedState.ChunkCount,
        chunkedState.ProcessedChunkCount,
        chunkedState.RetainedStateCurrentCount,
        chunkedState.RetainedStateHighWaterCount,
        chunkedState.StateFallbackCauseKinds,
        chunkedState.UnsupportedShapeKinds,
        strategySelection.StagedProviderBulk);
  }

  public static DataVaultReadTelemetrySummary CreateReadSummary(
      DataVaultReadTelemetryFamily family,
      DataVaultTelemetryOutcome outcome,
      int requestedKeyCount,
      int returnedRowCount,
      TimeSpan duration,
      DataVaultReadTelemetryStrategySelection strategySelection) {
    return new DataVaultReadTelemetrySummary(
        family,
        outcome,
        requestedKeyCount,
        returnedRowCount,
        duration,
        strategySelection.Status,
        strategySelection.ProviderName,
        strategySelection.SelectedStrategyName,
        strategySelection.FallbackCauseKinds);
  }

  public static DataVaultSaveTelemetryCounts CountSaveRequests(IReadOnlyList<DataVaultSaveRequest> requests) {
    var hubOperationCount = 0;
    var linkOperationCount = 0;
    var satelliteOperationCount = 0;

    foreach (var request in requests) {
      hubOperationCount += request.HubOperations.Count;
      linkOperationCount += request.LinkOperations.Count;
      satelliteOperationCount += request.SatelliteOperations.Count;
    }

    return new DataVaultSaveTelemetryCounts(
        requests.Count,
        hubOperationCount,
        linkOperationCount,
        satelliteOperationCount);
  }

  public static TimeSpan GetElapsed(Stopwatch stopwatch) {
    stopwatch.Stop();
    return stopwatch.Elapsed;
  }
}
