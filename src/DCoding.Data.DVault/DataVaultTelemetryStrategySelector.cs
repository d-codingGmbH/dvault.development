using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

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
    var stagedProviderBulkDiagnostics = new List<DataVaultStagedProviderBulkDiagnostics>();

    foreach (var strategy in providerSaveStrategies) {
      var stagedProviderBulk = DataVaultStagedProviderBulkDiagnosticsSupport.TryEvaluate(strategy, dbContext, requests);
      if (stagedProviderBulk is not null) {
        stagedProviderBulkDiagnostics.Add(stagedProviderBulk);
      }

      if (strategy.CanSave(dbContext, requests)) {
        return new DataVaultSaveTelemetryStrategySelection(
            strategy,
            DataVaultSaveStrategyDiagnosticsStatus.ProviderStrategySelected,
            providerName,
            strategy.GetType().Name,
            Array.Empty<DataVaultSaveStrategyFallbackCauseKind>(),
            stagedProviderBulkDiagnostics.FirstOrDefault());
      }

      if (DataVaultProviderSaveStrategyGateEvaluator.TryEvaluateKnownStrategy(
              strategy,
              dbContext,
              requests,
              out var evaluation)) {
        fallbackCauses.AddRange(evaluation.FallbackCauses);
      }
      else {
        var stagedFallbackCauses = DataVaultStagedProviderBulkDiagnosticsSupport.CreateFallbackCauses(stagedProviderBulk);
        if (stagedFallbackCauses.Count > 0) {
          fallbackCauses.AddRange(stagedFallbackCauses);
        }
        else {
          fallbackCauses.Add(new DataVaultSaveStrategyFallbackCause(
              DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
              "Provider save strategy '" + strategy.GetType().Name + "' declined the request batch."));
        }
      }
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
        FallbackCauseKinds: fallbackCauses.Select(cause => cause.Kind).Distinct().ToArray(),
        StagedProviderBulk: stagedProviderBulkDiagnostics.FirstOrDefault());
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
