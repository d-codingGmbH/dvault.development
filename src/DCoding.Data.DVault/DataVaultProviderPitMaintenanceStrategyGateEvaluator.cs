using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultProviderPitMaintenanceStrategyGateEvaluator {
  public static DataVaultProviderPitMaintenanceStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      DataVaultPitRebuildRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return EvaluatePostgres(
        dbContext.Database.ProviderName,
        request,
        hasCompleteMaintenanceShapeEvidence: HasCompleteMaintenanceShapeEvidence(dbContext, request),
        hasPendingTrackedChanges: HasPendingTrackedChanges(dbContext),
        hasCurrentTransaction: HasCurrentTransaction(dbContext));
  }

  public static DataVaultProviderPitMaintenanceStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      DataVaultPitRebuildRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return EvaluateMySql(
        dbContext.Database.ProviderName,
        request,
        hasCompleteMaintenanceShapeEvidence: HasCompleteMaintenanceShapeEvidence(dbContext, request),
        hasPendingTrackedChanges: HasPendingTrackedChanges(dbContext),
        hasCurrentTransactionWithoutSavepoints: HasCurrentTransactionWithoutSavepoints(dbContext));
  }

  public static DataVaultProviderPitMaintenanceStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitRebuildRequest request,
      bool hasCompleteMaintenanceShapeEvidence = true,
      bool hasPendingTrackedChanges = false,
      bool hasCurrentTransaction = false) {
    ArgumentNullException.ThrowIfNull(request);

    return EvaluatePitRebuild(
        "PostgreSQL",
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Postgres],
        hasCompleteMaintenanceShapeEvidence: hasCompleteMaintenanceShapeEvidence,
        hasPendingTrackedChanges: hasPendingTrackedChanges,
        hasCurrentTransaction: hasCurrentTransaction);
  }

  public static DataVaultProviderPitMaintenanceStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitRebuildRequest request,
      bool hasCompleteMaintenanceShapeEvidence = true,
      bool hasPendingTrackedChanges = false,
      bool hasCurrentTransactionWithoutSavepoints = false) {
    ArgumentNullException.ThrowIfNull(request);

    return EvaluatePitRebuild(
        "MySQL",
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.MySqlOracle],
        hasCompleteMaintenanceShapeEvidence: hasCompleteMaintenanceShapeEvidence,
        hasPendingTrackedChanges: hasPendingTrackedChanges,
        hasCurrentTransactionWithoutSavepoints: hasCurrentTransactionWithoutSavepoints,
        supportsLinkParent: false,
        supportsMultiActiveHubParent: false);
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderPitMaintenanceStrategy strategy,
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      out DataVaultProviderPitMaintenanceStrategyGateEvaluation evaluation) {
    ArgumentNullException.ThrowIfNull(strategy);
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    evaluation = strategy.GetType().Name switch {
      "PostgresDataVaultPitMaintenanceStrategy" => EvaluatePostgres(dbContext, request),
      "MySqlDataVaultPitMaintenanceStrategy" => EvaluateMySql(dbContext, request),
      _ => new DataVaultProviderPitMaintenanceStrategyGateEvaluation(
          false,
          Array.Empty<DataVaultPitMaintenanceStrategyFallbackCause>()),
    };

    return evaluation.CanRebuild || evaluation.FallbackCauses.Count > 0;
  }

  private static DataVaultProviderPitMaintenanceStrategyGateEvaluation EvaluatePitRebuild(
      string strategyName,
      string? providerName,
      DataVaultPitRebuildRequest request,
      IReadOnlyList<string> supportedProviderNames,
      bool hasCompleteMaintenanceShapeEvidence,
      bool hasPendingTrackedChanges,
      bool hasCurrentTransaction = false,
      bool hasCurrentTransactionWithoutSavepoints = false,
      bool supportsLinkParent = true,
      bool supportsMultiActiveHubParent = true) {
    var causes = new List<DataVaultPitMaintenanceStrategyFallbackCause>();

    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + strategyName + "."));
    }

    if (CapabilityProfileDefaulted(providerName)) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.UnknownOrUnregisteredProviderName,
          "Provider name '" + providerName + "' is unknown or unregistered for Data Vault provider capability selection."));
    }

    if (hasPendingTrackedChanges) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.DirtyDbContext,
          strategyName + " optimized PIT rebuild requires a clean DbContext because pending tracked changes can diverge from persisted satellite history."));
    }

    if (hasCurrentTransaction) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.CurrentTransactionSavepointUnavailable,
          strategyName + " optimized PIT rebuild falls back when a caller transaction is already active because this provider path does not implement strategy-owned savepoint rollback for delete-plus-insert maintenance."));
    }

    if (!hasCompleteMaintenanceShapeEvidence) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.IncompleteMaintenanceShapeEvidence,
          strategyName + " optimized PIT rebuild requires complete generated PIT and referenced satellite projection evidence in the DbContext model."));
    }

    if (hasCurrentTransactionWithoutSavepoints) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.RollbackSavepointBoundaryUnavailable,
          strategyName + " optimized PIT rebuild requires the current DbContext transaction to report savepoint support so delete-plus-insert work can roll back cleanly."));
    }

    AddUnsupportedPitShapeCauses(
        strategyName,
        request,
        causes,
        supportsLinkParent,
        supportsMultiActiveHubParent);

    return new DataVaultProviderPitMaintenanceStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static void AddUnsupportedPitShapeCauses(
      string strategyName,
      DataVaultPitRebuildRequest request,
      ICollection<DataVaultPitMaintenanceStrategyFallbackCause> causes,
      bool supportsLinkParent,
      bool supportsMultiActiveHubParent) {
    var pit = request.Pit;
    if (pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub &&
        pit.Parent.Kind != DataVaultMetadataReferenceKind.Link) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape,
          strategyName + " optimized PIT rebuild supports hub- or link-parent PIT declarations only."));
    }

    if (pit.Parent.Kind == DataVaultMetadataReferenceKind.Link && !supportsLinkParent) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape,
          strategyName + " optimized PIT rebuild supports ordinary hub-parent PIT declarations only."));
    }

    if (pit.Satellites.Count == 0) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape,
          strategyName + " optimized PIT rebuild requires at least one satellite snapshot reference."));
    }

    if ((!supportsMultiActiveHubParent || pit.Parent.Kind == DataVaultMetadataReferenceKind.Link) &&
        pit.Satellites.Any(satellite => satellite.IsMultiActive)) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape,
          strategyName + " optimized PIT rebuild requires non-multi-active satellite references for this provider lane."));
    }

    var duplicateSatelliteName = pit.Satellites
        .GroupBy(satellite => satellite.SatelliteName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();
    if (duplicateSatelliteName is not null) {
      causes.Add(new DataVaultPitMaintenanceStrategyFallbackCause(
          DataVaultPitMaintenanceStrategyFallbackCauseKind.UnsupportedPitShape,
          strategyName + " optimized PIT rebuild requires distinct satellite snapshot references."));
    }
  }

  private static bool HasCompleteMaintenanceShapeEvidence(
      DbContext dbContext,
      DataVaultPitRebuildRequest request) {
    try {
      _ = DefaultDataVaultPitMaintenanceService.CreatePitProjection(dbContext, request.Pit);
      return true;
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return false;
    }
  }

  private static bool HasPendingTrackedChanges(DbContext dbContext) {
    try {
      return dbContext.ChangeTracker.HasChanges();
    }
    catch (Exception exception) when (exception is InvalidOperationException or NotSupportedException) {
      return true;
    }
  }

  private static bool HasCurrentTransactionWithoutSavepoints(DbContext dbContext) {
    var currentTransaction = dbContext.Database.CurrentTransaction;

    return currentTransaction is not null && !currentTransaction.SupportsSavepoints;
  }

  private static bool CapabilityProfileDefaulted(string? providerName) {
    return !string.IsNullOrWhiteSpace(providerName) &&
        !DataVaultProviderCapabilityProfileSelection.TrySelectRegistered(providerName, out _);
  }

  private static bool HasCurrentTransaction(DbContext dbContext) {
    try {
      return dbContext.Database.CurrentTransaction is not null;
    }
    catch (Exception exception) when (exception is InvalidOperationException or NotSupportedException) {
      return true;
    }
  }
}
