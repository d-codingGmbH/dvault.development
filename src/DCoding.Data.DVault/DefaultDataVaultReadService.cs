using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultReadService : IDataVaultReadService, IDataVaultSatelliteProjectionReadService {
  private readonly IReadOnlyList<IDataVaultProviderBridgeReadStrategy> _providerBridgeReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderPitReadStrategy> _providerPitReadStrategies;
  private readonly IReadOnlyList<IDataVaultProviderReadStrategy> _providerReadStrategies;
  private readonly IReadOnlyList<IDataVaultTelemetryObserver> _telemetryObservers;

  public DefaultDataVaultReadService()
      : this([], [], [], []) {
  }

  public DefaultDataVaultReadService(IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies) {
    ArgumentNullException.ThrowIfNull(providerReadStrategies);

    _providerReadStrategies = OrderByPriority(providerReadStrategies);
    _providerPitReadStrategies = [];
    _providerBridgeReadStrategies = [];
    _telemetryObservers = [];
  }

  public DefaultDataVaultReadService(
      IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies,
      IEnumerable<IDataVaultProviderPitReadStrategy> providerPitReadStrategies,
      IEnumerable<IDataVaultProviderBridgeReadStrategy> providerBridgeReadStrategies)
      : this(
          providerReadStrategies,
          providerPitReadStrategies,
          providerBridgeReadStrategies,
          []) {
  }

  public DefaultDataVaultReadService(
      IEnumerable<IDataVaultProviderReadStrategy> providerReadStrategies,
      IEnumerable<IDataVaultProviderPitReadStrategy> providerPitReadStrategies,
      IEnumerable<IDataVaultProviderBridgeReadStrategy> providerBridgeReadStrategies,
      IEnumerable<IDataVaultTelemetryObserver> telemetryObservers) {
    ArgumentNullException.ThrowIfNull(providerReadStrategies);
    ArgumentNullException.ThrowIfNull(providerPitReadStrategies);
    ArgumentNullException.ThrowIfNull(providerBridgeReadStrategies);
    ArgumentNullException.ThrowIfNull(telemetryObservers);

    _providerReadStrategies = OrderByPriority(providerReadStrategies);
    _providerPitReadStrategies = OrderByPriority(providerPitReadStrategies);
    _providerBridgeReadStrategies = OrderByPriority(providerBridgeReadStrategies);
    _telemetryObservers = DataVaultTelemetryDispatcher.CreateObservers(telemetryObservers);
  }

  public Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return ReadLatestSatelliteRowsCoreAsync(dbContext, request, cancellationToken);
  }

  private async Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsCoreAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultReadTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      strategySelection = DataVaultTelemetryStrategySelector.SelectLatestSatelliteReadStrategy(dbContext, _providerReadStrategies, request);
      var rows = strategySelection.Strategy is IDataVaultProviderReadStrategy strategy
          ? await strategy.ReadLatestSatelliteRowsAsync(
              new DataVaultProviderReadStrategyContext(dbContext, request),
              cancellationToken).ConfigureAwait(false)
          : await DataVaultSatelliteReadPipeline.ReadLatestReadRecordsAsync(
              dbContext,
              request,
              cancellationToken).ConfigureAwait(false);

      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.LatestSatellite,
          DataVaultTelemetryOutcome.Succeeded,
          request.ParentHashKeys.Count,
          rows.Count,
          stopwatch,
          strategySelection);

      return rows;
    }
    catch {
      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.LatestSatellite,
          DataVaultTelemetryOutcome.Failed,
          request.ParentHashKeys.Count,
          returnedRowCount: 0,
          stopwatch,
          strategySelection);
      throw;
    }
  }

  public Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return ReadPitRowsCoreAsync(dbContext, request, cancellationToken);
  }

  private async Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsCoreAsync(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken) {
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultReadTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      strategySelection = DataVaultTelemetryStrategySelector.SelectPitReadStrategy(dbContext, _providerPitReadStrategies, request);
      var rows = strategySelection.Strategy is IDataVaultProviderPitReadStrategy strategy
          ? await strategy.ReadPitRowsAsync(
              new DataVaultProviderPitReadStrategyContext(dbContext, request),
              cancellationToken).ConfigureAwait(false)
          : await DataVaultPitReadPipeline.ReadPitReadRecordsAsync(
              dbContext,
              request,
              cancellationToken).ConfigureAwait(false);

      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.Pit,
          DataVaultTelemetryOutcome.Succeeded,
          request.ParentHashKeys.Count,
          rows.Count,
          stopwatch,
          strategySelection);

      return rows;
    }
    catch {
      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.Pit,
          DataVaultTelemetryOutcome.Failed,
          request.ParentHashKeys.Count,
          returnedRowCount: 0,
          stopwatch,
          strategySelection);
      throw;
    }
  }

  public Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return ReadBridgeRowsCoreAsync(dbContext, request, cancellationToken);
  }

  private async Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsCoreAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken) {
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultReadTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      strategySelection = DataVaultTelemetryStrategySelector.SelectBridgeReadStrategy(dbContext, _providerBridgeReadStrategies, request);
      var rows = strategySelection.Strategy is IDataVaultProviderBridgeReadStrategy strategy
          ? await strategy.ReadBridgeRowsAsync(
              new DataVaultProviderBridgeReadStrategyContext(dbContext, request),
              cancellationToken).ConfigureAwait(false)
          : await DataVaultBridgeReadPipeline.ReadBridgeReadRecordsAsync(
              dbContext,
              request,
              cancellationToken).ConfigureAwait(false);

      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.Bridge,
          DataVaultTelemetryOutcome.Succeeded,
          request.EndpointHashKeys.Count,
          rows.Count,
          stopwatch,
          strategySelection);

      return rows;
    }
    catch {
      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.Bridge,
          DataVaultTelemetryOutcome.Failed,
          request.EndpointHashKeys.Count,
          returnedRowCount: 0,
          stopwatch,
          strategySelection);
      throw;
    }
  }

  public Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return ReadBridgeProjectionRowsCoreAsync(dbContext, request, cancellationToken);
  }

  private async Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsCoreAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken) {
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultReadTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      strategySelection = DataVaultTelemetryStrategySelector.SelectBridgeReadStrategy(dbContext, _providerBridgeReadStrategies, request);
      var rows = strategySelection.Strategy is IDataVaultProviderBridgeReadStrategy strategy
          ? await strategy.ReadBridgeProjectionRowsAsync(
              new DataVaultProviderBridgeReadStrategyContext(dbContext, request),
              cancellationToken).ConfigureAwait(false)
          : await DataVaultBridgeReadPipeline.ReadBridgeProjectionRowsAsync(
              dbContext,
              request,
              cancellationToken).ConfigureAwait(false);

      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.Bridge,
          DataVaultTelemetryOutcome.Succeeded,
          request.EndpointHashKeys.Count,
          rows.Count,
          stopwatch,
          strategySelection);

      return rows;
    }
    catch {
      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.Bridge,
          DataVaultTelemetryOutcome.Failed,
          request.EndpointHashKeys.Count,
          returnedRowCount: 0,
          stopwatch,
          strategySelection);
      throw;
    }
  }

  Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> IDataVaultSatelliteProjectionReadService.ReadLatestSatelliteProjectionRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return ReadLatestSatelliteProjectionRowsCoreAsync(dbContext, request, cancellationToken);
  }

  private async Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsCoreAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    var stopwatch = Stopwatch.StartNew();
    var strategySelection = DataVaultReadTelemetryStrategySelection.NotEvaluated(
        DataVaultTelemetryStrategySelector.GetProviderName(dbContext));

    try {
      strategySelection = DataVaultTelemetryStrategySelector.SelectLatestSatelliteReadStrategy(dbContext, _providerReadStrategies, request);
      var rows = strategySelection.Strategy is IDataVaultProviderReadStrategy strategy
          ? await strategy.ReadLatestSatelliteProjectionRowsAsync(
              new DataVaultProviderReadStrategyContext(dbContext, request),
              cancellationToken).ConfigureAwait(false)
          : await DataVaultSatelliteReadPipeline.ReadLatestProjectionRowsAsync(
              dbContext,
              request,
              cancellationToken).ConfigureAwait(false);

      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.LatestSatellite,
          DataVaultTelemetryOutcome.Succeeded,
          request.ParentHashKeys.Count,
          rows.Count,
          stopwatch,
          strategySelection);

      return rows;
    }
    catch {
      RecordReadTelemetry(
          DataVaultReadTelemetryFamily.LatestSatellite,
          DataVaultTelemetryOutcome.Failed,
          request.ParentHashKeys.Count,
          returnedRowCount: 0,
          stopwatch,
          strategySelection);
      throw;
    }
  }

  private void RecordReadTelemetry(
      DataVaultReadTelemetryFamily family,
      DataVaultTelemetryOutcome outcome,
      int requestedKeyCount,
      int returnedRowCount,
      Stopwatch stopwatch,
      DataVaultReadTelemetryStrategySelection strategySelection) {
    DataVaultTelemetryDispatcher.RecordRead(
        _telemetryObservers,
        DataVaultTelemetrySummaryFactory.CreateReadSummary(
            family,
            outcome,
            requestedKeyCount,
            returnedRowCount,
            DataVaultTelemetrySummaryFactory.GetElapsed(stopwatch),
            strategySelection));
  }

  private static IReadOnlyList<IDataVaultProviderReadStrategy> OrderByPriority(
      IEnumerable<IDataVaultProviderReadStrategy> strategies) {
    return strategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }

  private static IReadOnlyList<IDataVaultProviderPitReadStrategy> OrderByPriority(
      IEnumerable<IDataVaultProviderPitReadStrategy> strategies) {
    return strategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }

  private static IReadOnlyList<IDataVaultProviderBridgeReadStrategy> OrderByPriority(
      IEnumerable<IDataVaultProviderBridgeReadStrategy> strategies) {
    return strategies
        .OrderByDescending(strategy => strategy.Priority)
        .ToArray();
  }
}
