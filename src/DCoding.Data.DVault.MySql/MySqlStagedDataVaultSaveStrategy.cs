using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class MySqlStagedDataVaultSaveStrategy : IDataVaultProviderSaveStrategy, IDataVaultProviderStagedBulkSaveDiagnostics {
  public int Priority => 110;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return DataVaultProviderSaveStrategyGateEvaluator.EvaluateMySqlStaged(dbContext, requests).CanSave;
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    return await MySqlDataVaultSaveStrategy.ExecuteStagedSaveAsync(context, cancellationToken).ConfigureAwait(false);
  }

  public DataVaultStagedProviderBulkDiagnostics? EvaluateStagedProviderBulkSave(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    if (!MySqlDataVaultSaveStrategy.IsSupportedProviderName(dbContext.Database.ProviderName)) {
      return null;
    }

    return CreateStagedProviderBulkDiagnostics(
        DataVaultProviderSaveStrategyGateEvaluator.HasPendingTrackedChanges(dbContext),
        requests);
  }

  internal static DataVaultStagedProviderBulkDiagnostics CreateStagedProviderBulkDiagnostics(
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var counts = CountSaveOperations(requests);
    if (counts.OperationCount == 0) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.NotEvaluated,
          DataVaultStagedProviderBulkProviderCaveatKind.None,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          []);
    }

    if (hasPendingTrackedChanges) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.DirtyContext,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkDirtyDbContext]);
    }

    if (DataVaultProviderSaveStrategyGateEvaluator.ContainsMultiActiveSatelliteOperations(requests) ||
        !MySqlDataVaultSaveStrategy.IsStagedBatchShape(requests)) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.UnsupportedShape,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkUnsupportedShape]);
    }

    if (MySqlDataVaultSaveStrategy.IsTinySatelliteHistoryProviderNeutralFallbackBatch(requests)) {
      return new DataVaultStagedProviderBulkDiagnostics(
          DataVaultStagedProviderBulkLifecyclePhase.Declined,
          DataVaultStagedProviderBulkProviderCaveatKind.ProviderLimitation,
          counts.RequestCount,
          counts.HubOperationCount,
          counts.LinkOperationCount,
          counts.SatelliteOperationCount,
          [DataVaultSaveStrategyFallbackCauseKind.StagedProviderBulkProviderLimitation]);
    }

    return new DataVaultStagedProviderBulkDiagnostics(
        DataVaultStagedProviderBulkLifecyclePhase.NativeBulkApplication,
        DataVaultStagedProviderBulkProviderCaveatKind.None,
        counts.RequestCount,
        counts.HubOperationCount,
        counts.LinkOperationCount,
        counts.SatelliteOperationCount,
        []);
  }

  private static MySqlStagedOperationCounts CountSaveOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    var hubOperationCount = 0;
    var linkOperationCount = 0;
    var satelliteOperationCount = 0;
    foreach (var request in requests) {
      hubOperationCount += request.HubOperations.Count;
      linkOperationCount += request.LinkOperations.Count;
      satelliteOperationCount += request.SatelliteOperations.Count;
    }

    return new MySqlStagedOperationCounts(
        requests.Count,
        hubOperationCount,
        linkOperationCount,
        satelliteOperationCount);
  }

  private sealed record MySqlStagedOperationCounts(
      int RequestCount,
      int HubOperationCount,
      int LinkOperationCount,
      int SatelliteOperationCount) {
    public int OperationCount => HubOperationCount + LinkOperationCount + SatelliteOperationCount;
  }
}
