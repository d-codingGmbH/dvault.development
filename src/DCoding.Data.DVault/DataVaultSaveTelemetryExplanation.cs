namespace DCoding.Data.DVault;

internal static class DataVaultSaveTelemetryExplanationCatalog {
  private static readonly DataVaultChunkedSaveTransactionExplanation ChunkedTransaction =
      new(
          "Chunked execution participates in the caller-owned DbContext current transaction and does not create, commit, roll back, or suppress transactions for the caller.",
          "For all-or-nothing behavior across chunks, open the transaction before invoking the save service and roll it back if the save fails or is canceled.");

  public static IReadOnlyList<DataVaultSaveStrategyFallbackExplanation> ExplainSaveStrategyFallbacks(
      IEnumerable<DataVaultSaveStrategyFallbackCauseKind> kinds) {
    ArgumentNullException.ThrowIfNull(kinds);

    return kinds
        .Distinct()
        .Select(ExplainSaveStrategyFallback)
        .ToArray();
  }

  public static IReadOnlyList<DataVaultChunkedSaveStateFallbackExplanation> ExplainChunkedStateFallbacks(
      IEnumerable<DataVaultChunkedSaveStateFallbackCauseKind> kinds) {
    ArgumentNullException.ThrowIfNull(kinds);

    return kinds
        .Distinct()
        .Select(ExplainChunkedStateFallback)
        .ToArray();
  }

  public static IReadOnlyList<DataVaultChunkedSaveUnsupportedShapeExplanation> ExplainUnsupportedShapes(
      IEnumerable<DataVaultChunkedSaveUnsupportedShapeKind> kinds) {
    ArgumentNullException.ThrowIfNull(kinds);

    return kinds
        .Distinct()
        .Select(ExplainUnsupportedShape)
        .ToArray();
  }

  public static DataVaultChunkedSaveTransactionExplanation? ExplainChunkedTransaction(
      DataVaultSaveTelemetryOperationKind operationKind) {
    return operationKind == DataVaultSaveTelemetryOperationKind.ChunkedRequest
        ? ChunkedTransaction
        : null;
  }

  private static DataVaultSaveStrategyFallbackExplanation ExplainSaveStrategyFallback(
      DataVaultSaveStrategyFallbackCauseKind kind) {
    return kind switch {
      DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch => new(
          kind,
          "The registered provider-specific save strategy does not match the Entity Framework provider selected by the DbContext.",
          "Register the DVault provider package and Entity Framework provider that match the DbContext, or allow the provider-neutral writer to handle the request."),
      DataVaultSaveStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName => new(
          kind,
          "The DbContext provider name is absent, unknown, or not registered with DVault provider capability metadata.",
          "Register the matching DVault provider startup extension or a compatible provider capability mapping before requesting provider-native save dispatch."),
      DataVaultSaveStrategyFallbackCauseKind.NoProviderSpecificStrategyRegistered => new(
          kind,
          "No provider-specific save strategy was available to evaluate the request batch.",
          "Register the matching DVault provider package or a custom IDataVaultProviderSaveStrategy when provider-native dispatch is required; otherwise provider-neutral fallback is expected."),
      DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext => new(
          kind,
          "The DbContext change tracker already contains pending added, modified, or deleted entries, so provider-native save dispatch cannot isolate the explicit DVault batch.",
          "Use a clean DbContext for the explicit save, or save, detach, or discard tracked application changes before invoking the DVault save service."),
      DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations => new(
          kind,
          "The request batch contains multi-active satellite operations that the provider-native save strategy does not support.",
          "Route this shape through provider-neutral fallback, or split unsupported multi-active satellite work from provider-native eligible hub, link, and ordinary satellite batches."),
      DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold => new(
          kind,
          "SQL Server provider-native save dispatch is only selected for batches with at least 50 total hub, link, and satellite operations.",
          "Increase the request or chunk size to meet the 50-operation SQL Server threshold when provider-native dispatch is desired, or accept provider-neutral fallback for small batches."),
      DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold => new(
          kind,
          "SQL Server provider-native save dispatch accepts at most 500 satellite operations in one request batch.",
          "Reduce satellite operations per request or chunk to 500 or fewer when SQL Server provider-native dispatch is desired."),
      DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold => new(
          kind,
          "MySQL provider-native save dispatch is only selected for batches with at least 50 total hub, link, and satellite operations.",
          "Increase the request or chunk size to meet the 50-operation MySQL threshold when provider-native dispatch is desired, or accept provider-neutral fallback for small batches."),
      DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold => new(
          kind,
          "Oracle provider-native save dispatch is only selected for batches with at least 50 total hub, link, and satellite operations.",
          "Increase the request or chunk size to meet the 50-operation Oracle threshold when provider-native dispatch is desired, or accept provider-neutral fallback for small batches."),
      DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined => new(
          kind,
          "A registered provider-specific or custom save strategy declined the request batch without a more specific DVault gate classification.",
          "Review the strategy's bounded gate documentation and adjust provider registration, request shape, or chunk sizing; otherwise use provider-neutral fallback."),
      DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold => new(
          kind,
          "Oracle provider-native save dispatch accepts at most 10000 satellite operations in one request batch.",
          "Reduce satellite operations per request or chunk to 10000 or fewer when Oracle provider-native dispatch is desired."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkDirtyDbContext => new(
          kind,
          "Staged-provider bulk execution declined because pending tracked DbContext changes would prevent the staged batch from being isolated.",
          "Use a clean DbContext for staged-provider bulk execution, or save, detach, or discard tracked application changes before invoking the DVault save service."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape => new(
          kind,
          "Staged-provider bulk execution declined because the request batch contains a shape outside the provider's staged bulk contract.",
          "Route unsupported shapes through provider-neutral fallback, or split the batch so staged-provider eligible hub, link, and ordinary satellite operations are evaluated separately."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkTransactionParticipationUnsupported => new(
          kind,
          "Staged-provider bulk execution declined because the staged provider path could not participate in the caller-owned transaction contract.",
          "Open and manage the transaction on the DbContext before calling DVault, or use provider-neutral fallback when the staged provider cannot join that transaction boundary."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkCleanupFailed => new(
          kind,
          "Staged-provider bulk execution fell back because transient staging cleanup did not complete safely.",
          "Inspect provider operational logs for cleanup failures, ensure the caller has permission to create and drop transient staging objects, then retry or use provider-neutral fallback."),
      DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkProviderLimitation => new(
          kind,
          "Staged-provider bulk execution declined because a bounded provider limitation prevented the staged path.",
          "Review the provider's staged bulk limits and adjust batch size, schema shape, or provider configuration; otherwise rely on provider-neutral fallback."),
      DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback => new(
          kind,
          "MySQL provider-native save dispatch deliberately uses provider-neutral fallback for a tiny satellite-only batch or small satellite history batch.",
          "Keep this shape on the provider-neutral lane, or increase the batch size enough to justify remeasuring MySQL provider-native dispatch."),
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown save strategy fallback cause kind."),
    };
  }

  private static DataVaultChunkedSaveStateFallbackExplanation ExplainChunkedStateFallback(
      DataVaultChunkedSaveStateFallbackCauseKind kind) {
    return kind switch {
      DataVaultChunkedSaveStateFallbackCauseKind.RetainedSatelliteSeriesLimitReached => new(
          kind,
          "The chunked save reached the retained satellite continuity-state series limit, so retained in-memory state was cleared and the save continued with bounded persisted latest-state lookup fallback.",
          "Reduce chunk size or the number of distinct satellite parent and driving-key series in one attempt; the default v1 retained-state limit is 10000 series."),
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown chunked state fallback cause kind."),
    };
  }

  private static DataVaultChunkedSaveUnsupportedShapeExplanation ExplainUnsupportedShape(
      DataVaultChunkedSaveUnsupportedShapeKind kind) {
    return kind switch {
      DataVaultChunkedSaveUnsupportedShapeKind.RetainedSatelliteSeriesLimitExceeded => new(
          kind,
          "The chunked save touched more satellite continuity series than the bounded retained-state budget can keep in memory for one attempt.",
          "Keep each chunked attempt within the retained-state budget, split the source into smaller attempts, or rely on the bounded persisted latest-state lookup fallback."),
      _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown chunked unsupported shape kind."),
    };
  }
}
