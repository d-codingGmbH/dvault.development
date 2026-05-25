using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed record DataVaultSaveTelemetryStrategySelection(
    IDataVaultProviderSaveStrategy? Strategy,
    DataVaultSaveStrategyDiagnosticsStatus Status,
    string? ProviderName,
    string? SelectedStrategyName,
    IReadOnlyList<DataVaultSaveStrategyFallbackCauseKind> FallbackCauseKinds) {
  public static DataVaultSaveTelemetryStrategySelection NotEvaluated(string? providerName) {
    return new DataVaultSaveTelemetryStrategySelection(
        Strategy: null,
        DataVaultSaveStrategyDiagnosticsStatus.NotEvaluated,
        providerName,
        SelectedStrategyName: null,
        FallbackCauseKinds: Array.Empty<DataVaultSaveStrategyFallbackCauseKind>());
  }
}

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

internal static class DataVaultTelemetryDispatcher {
  public static IReadOnlyList<IDataVaultTelemetryObserver> CreateObservers(IEnumerable<IDataVaultTelemetryObserver>? observers) {
    return observers?.ToArray() ?? Array.Empty<IDataVaultTelemetryObserver>();
  }

  public static void RecordSave(
      IReadOnlyList<IDataVaultTelemetryObserver> observers,
      DataVaultSaveTelemetrySummary summary) {
    foreach (var observer in observers) {
      try {
        observer.RecordSave(summary);
      }
      catch (Exception) {
      }
    }
  }

  public static void RecordRead(
      IReadOnlyList<IDataVaultTelemetryObserver> observers,
      DataVaultReadTelemetrySummary summary) {
    foreach (var observer in observers) {
      try {
        observer.RecordRead(summary);
      }
      catch (Exception) {
      }
    }
  }
}

internal static class DataVaultTelemetryStrategySelector {
  public static string? GetProviderName(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    try {
      return dbContext.Database.ProviderName;
    }
    catch (InvalidOperationException) {
      return null;
    }
    catch (NotSupportedException) {
      return null;
    }
  }

  public static DataVaultSaveTelemetryStrategySelection SelectSaveStrategy(
      DbContext dbContext,
      IReadOnlyList<IDataVaultProviderSaveStrategy> providerSaveStrategies,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    var providerName = GetProviderName(dbContext);
    var fallbackCauses = new List<DataVaultSaveStrategyFallbackCause>();

    foreach (var strategy in providerSaveStrategies) {
      if (strategy.CanSave(dbContext, requests)) {
        return new DataVaultSaveTelemetryStrategySelection(
            strategy,
            DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            strategy.GetType().Name,
            Array.Empty<DataVaultSaveStrategyFallbackCauseKind>());
      }

      fallbackCauses.AddRange(
          DataVaultProviderSaveStrategyGateEvaluator.TryEvaluateKnownStrategy(
              strategy,
              dbContext,
              requests,
              out var evaluation)
              ? evaluation.FallbackCauses
              : [new DataVaultSaveStrategyFallbackCause(
                  DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
                  "Provider save strategy '" + strategy.GetType().Name + "' declined the request batch.")]);
    }

    if (providerSaveStrategies.Count == 0) {
      fallbackCauses.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          "No provider-specific Data Vault save strategy is registered."));
    }

    if (CapabilityProfileDefaulted(providerName) &&
        !fallbackCauses.Any(cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauses.Insert(0, new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauses.Count == 0) {
      fallbackCauses.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
          "Every registered provider-specific Data Vault save strategy declined the request batch."));
    }

    return new DataVaultSaveTelemetryStrategySelection(
        Strategy: null,
        DataVaultSaveStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        FallbackCauseKinds: fallbackCauses.Select(cause => cause.Kind).Distinct().ToArray());
  }

  public static DataVaultReadTelemetryStrategySelection SelectLatestSatelliteReadStrategy(
      DbContext dbContext,
      IReadOnlyList<IDataVaultProviderReadStrategy> providerReadStrategies,
      DataVaultLatestSatelliteReadRequest request) {
    var providerName = GetProviderName(dbContext);
    var fallbackCauses = new List<DataVaultReadStrategyFallbackCause>();

    foreach (var strategy in providerReadStrategies) {
      if (strategy.CanReadLatestSatelliteRows(dbContext, request)) {
        return CreateSelectedReadStrategy(strategy, providerName);
      }

      fallbackCauses.AddRange(
          DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
              strategy,
              dbContext,
              request,
              out var evaluation)
              ? evaluation.FallbackCauses
              : [new DataVaultReadStrategyFallbackCause(
                  DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                  "Provider read strategy '" + strategy.GetType().Name + "' declined the latest/as-of satellite read request.")]);
    }

    return CreateReadFallbackStrategy(
        providerName,
        providerReadStrategies.Count,
        fallbackCauses,
        "No provider-specific Data Vault read strategy is registered.",
        "Every registered provider-specific Data Vault read strategy declined the latest/as-of satellite read request.");
  }

  public static DataVaultReadTelemetryStrategySelection SelectPitReadStrategy(
      DbContext dbContext,
      IReadOnlyList<IDataVaultProviderPitReadStrategy> providerPitReadStrategies,
      DataVaultPitAsOfReadRequest request) {
    var providerName = GetProviderName(dbContext);
    var fallbackCauses = new List<DataVaultReadStrategyFallbackCause>();

    foreach (var strategy in providerPitReadStrategies) {
      if (strategy.CanReadPitRows(dbContext, request)) {
        return CreateSelectedReadStrategy(strategy, providerName);
      }

      fallbackCauses.AddRange(
          DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
              strategy,
              dbContext,
              request,
              out var evaluation)
              ? evaluation.FallbackCauses
              : [new DataVaultReadStrategyFallbackCause(
                  DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                  "Provider read strategy '" + strategy.GetType().Name + "' declined the PIT read request.")]);
    }

    return CreateReadFallbackStrategy(
        providerName,
        providerPitReadStrategies.Count,
        fallbackCauses,
        "No provider-specific Data Vault PIT read strategy is registered.",
        "Every registered provider-specific Data Vault PIT read strategy declined the request.");
  }

  public static DataVaultReadTelemetryStrategySelection SelectBridgeReadStrategy(
      DbContext dbContext,
      IReadOnlyList<IDataVaultProviderBridgeReadStrategy> providerBridgeReadStrategies,
      DataVaultBridgeReadRequest request) {
    var providerName = GetProviderName(dbContext);
    var fallbackCauses = new List<DataVaultReadStrategyFallbackCause>();

    foreach (var strategy in providerBridgeReadStrategies) {
      if (strategy.CanReadBridgeRows(dbContext, request)) {
        return CreateSelectedReadStrategy(strategy, providerName);
      }

      fallbackCauses.AddRange(
          DataVaultProviderReadStrategyGateEvaluator.TryEvaluateKnownStrategy(
              strategy,
              dbContext,
              request,
              out var evaluation)
              ? evaluation.FallbackCauses
              : [new DataVaultReadStrategyFallbackCause(
                  DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
                  "Provider read strategy '" + strategy.GetType().Name + "' declined the bridge read request.")]);
    }

    return CreateReadFallbackStrategy(
        providerName,
        providerBridgeReadStrategies.Count,
        fallbackCauses,
        "No provider-specific Data Vault bridge read strategy is registered.",
        "Every registered provider-specific Data Vault bridge read strategy declined the request.");
  }

  private static DataVaultReadTelemetryStrategySelection CreateSelectedReadStrategy(
      object strategy,
      string? providerName) {
    return new DataVaultReadTelemetryStrategySelection(
        strategy,
        DataVaultReadStrategyDiagnosticsStatus.ProviderStrategySelected,
        providerName,
        strategy.GetType().Name,
        Array.Empty<DataVaultReadStrategyFallbackCauseKind>());
  }

  private static DataVaultReadTelemetryStrategySelection CreateReadFallbackStrategy(
      string? providerName,
      int strategyCount,
      List<DataVaultReadStrategyFallbackCause> fallbackCauses,
      string noStrategyMessage,
      string allDeclinedMessage) {
    if (strategyCount == 0) {
      fallbackCauses.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered,
          noStrategyMessage));
    }

    if (CapabilityProfileDefaulted(providerName) &&
        !fallbackCauses.Any(cause => cause.Kind == DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName)) {
      fallbackCauses.Insert(0, new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + (providerName ?? "<null>") + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (fallbackCauses.Count == 0) {
      fallbackCauses.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.StrategyDeclined,
          allDeclinedMessage));
    }

    return new DataVaultReadTelemetryStrategySelection(
        Strategy: null,
        DataVaultReadStrategyDiagnosticsStatus.ProviderNeutralFallback,
        providerName,
        SelectedStrategyName: null,
        FallbackCauseKinds: fallbackCauses.Select(cause => cause.Kind).Distinct().ToArray());
  }

  private static bool CapabilityProfileDefaulted(string? providerName) {
    return !string.IsNullOrWhiteSpace(providerName) &&
        !DataVaultProviderCapabilityProfileSelection.TrySelectRegistered(providerName, out _);
  }
}

internal readonly record struct DataVaultSaveTelemetryCounts(
    int RequestCount,
    int HubOperationCount,
    int LinkOperationCount,
    int SatelliteOperationCount);

internal sealed record DataVaultChunkedSaveTelemetryState(
    int ChunkCount,
    int ProcessedChunkCount,
    int RetainedStateCurrentCount,
    int RetainedStateHighWaterCount,
    IReadOnlyList<DataVaultChunkedSaveStateFallbackCauseKind> StateFallbackCauseKinds,
    IReadOnlyList<DataVaultChunkedSaveUnsupportedShapeKind> UnsupportedShapeKinds) {
  public static DataVaultChunkedSaveTelemetryState Empty { get; } = new(
      ChunkCount: 0,
      ProcessedChunkCount: 0,
      RetainedStateCurrentCount: 0,
      RetainedStateHighWaterCount: 0,
      StateFallbackCauseKinds: Array.Empty<DataVaultChunkedSaveStateFallbackCauseKind>(),
      UnsupportedShapeKinds: Array.Empty<DataVaultChunkedSaveUnsupportedShapeKind>());
}

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
        chunkedState.UnsupportedShapeKinds);
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
