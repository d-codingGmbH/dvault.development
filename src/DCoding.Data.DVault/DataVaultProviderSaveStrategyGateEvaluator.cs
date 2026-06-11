using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

internal static class DataVaultProviderSaveStrategyGateEvaluator {
  private const int MinimumSqlServerOptimizedBatchOperationCount = 50;
  private const int MaximumSqlServerOptimizedSatelliteOperationCount = 500;
  private const int MinimumMySqlOptimizedBatchOperationCount = 50;
  private const int MinimumMySqlStagedBatchOperationCount = 60;
  private const int MySqlTinySatelliteHistoryProviderNeutralFallbackSingleRequestMaximumOperationCount = 10;
  private const int MySqlTinySatelliteHistoryProviderNeutralFallbackMaximumOperationCount = 100;
  private const int MinimumOracleOptimizedBatchOperationCount = 50;
  private const int MaximumOracleOptimizedSatelliteOperationCount = 10000;

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Sqlite,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Sqlite],
        minimumOperationCount: null,
        maximumSatelliteOperationCount: null);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Postgres,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Postgres],
        minimumOperationCount: null,
        maximumSatelliteOperationCount: null);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.SqlServer,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.SqlServer],
        minimumOperationCount: MinimumSqlServerOptimizedBatchOperationCount,
        maximumSatelliteOperationCount: MaximumSqlServerOptimizedSatelliteOperationCount);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return DeclineMySqlTinySatelliteHistoryProviderNeutralFallbackBatch(
        Evaluate(
            DataVaultKnownProviderSaveStrategy.MySql,
            providerName,
            hasPendingTrackedChanges,
            requests,
            supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
            minimumOperationCount: MinimumMySqlOptimizedBatchOperationCount,
            maximumSatelliteOperationCount: null),
        requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySqlStaged(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySqlStaged(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateMySqlStaged(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return DeclineMySqlTinySatelliteHistoryProviderNeutralFallbackBatch(
        Evaluate(
            DataVaultKnownProviderSaveStrategy.MySqlStaged,
            providerName,
            hasPendingTrackedChanges,
            requests,
            supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
            minimumOperationCount: MinimumMySqlStagedBatchOperationCount,
            maximumSatelliteOperationCount: null),
        requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Oracle,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Oracle],
        minimumOperationCount: MinimumOracleOptimizedBatchOperationCount,
        maximumSatelliteOperationCount: MaximumOracleOptimizedSatelliteOperationCount);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateDb2(
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateDb2(dbContext.Database.ProviderName, HasPendingTrackedChanges(dbContext), requests);
  }

  public static DataVaultProviderSaveStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    return Evaluate(
        DataVaultKnownProviderSaveStrategy.Db2,
        providerName,
        hasPendingTrackedChanges,
        requests,
        supportedProviderNames: [KnownProviderNames.Db2],
        minimumOperationCount: null,
        maximumSatelliteOperationCount: null);
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderSaveStrategy strategy,
      DbContext dbContext,
      IReadOnlyList<DataVaultSaveRequest> requests,
      out DataVaultProviderSaveStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultSaveStrategy" => EvaluateSqlite(dbContext, requests),
      "PostgresDataVaultSaveStrategy" => EvaluatePostgres(dbContext, requests),
      "SqlServerDataVaultSaveStrategy" => EvaluateSqlServer(dbContext, requests),
      "MySqlStagedDataVaultSaveStrategy" => EvaluateMySqlStaged(dbContext, requests),
      "MySqlDataVaultSaveStrategy" => EvaluateMySql(dbContext, requests),
      "OracleDataVaultSaveStrategy" => EvaluateOracle(dbContext, requests),
      "Db2DataVaultSaveStrategy" => EvaluateDb2(dbContext, requests),
      _ => new DataVaultProviderSaveStrategyGateEvaluation(false, Array.Empty<DataVaultSaveStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanSave;
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderSaveStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultSaveStrategy" => [KnownProviderNames.Sqlite],
      "PostgresDataVaultSaveStrategy" => [KnownProviderNames.Postgres],
      "SqlServerDataVaultSaveStrategy" => [KnownProviderNames.SqlServer],
      "MySqlStagedDataVaultSaveStrategy" => [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
      "MySqlDataVaultSaveStrategy" => [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
      "OracleDataVaultSaveStrategy" => [KnownProviderNames.Oracle],
      "Db2DataVaultSaveStrategy" => [KnownProviderNames.Db2],
      _ => Array.Empty<string>(),
    };
  }

  public static IReadOnlyList<DataVaultSaveStrategyGateRequirement> GetKnownStrategyGateRequirements(
      IDataVaultProviderSaveStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    var commonRequirements = new[]
    {
        new DataVaultSaveStrategyGateRequirement(DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch),
        new DataVaultSaveStrategyGateRequirement(DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext),
        new DataVaultSaveStrategyGateRequirement(DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations),
    };

    return strategy.GetType().Name switch {
      "SqliteDataVaultSaveStrategy" => commonRequirements,
      "PostgresDataVaultSaveStrategy" => commonRequirements,
      "Db2DataVaultSaveStrategy" => commonRequirements,
      "SqlServerDataVaultSaveStrategy" => commonRequirements
          .Concat([
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
                  MinimumTotalOperationCount: MinimumSqlServerOptimizedBatchOperationCount),
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
                  MaximumSatelliteOperationCount: MaximumSqlServerOptimizedSatelliteOperationCount),
          ])
          .ToArray(),
      "MySqlDataVaultSaveStrategy" => commonRequirements
          .Concat([
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
                  MinimumTotalOperationCount: MinimumMySqlOptimizedBatchOperationCount),
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback),
          ])
          .ToArray(),
      "MySqlStagedDataVaultSaveStrategy" => commonRequirements
          .Concat([
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
                  MinimumTotalOperationCount: MinimumMySqlStagedBatchOperationCount),
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback),
          ])
          .ToArray(),
      "OracleDataVaultSaveStrategy" => commonRequirements
          .Concat([
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold,
                  MinimumTotalOperationCount: MinimumOracleOptimizedBatchOperationCount),
              new DataVaultSaveStrategyGateRequirement(
                  DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold,
                  MaximumSatelliteOperationCount: MaximumOracleOptimizedSatelliteOperationCount),
          ])
          .ToArray(),
      _ => Array.Empty<DataVaultSaveStrategyGateRequirement>(),
    };
  }

  public static bool HasPendingTrackedChanges(DbContext dbContext) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return dbContext.ChangeTracker
        .Entries()
        .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
  }

  public static bool ContainsMultiActiveSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    return requests.Any(request => request.SatelliteOperations.Any(operation => operation.Metadata.DrivingKeyNames.Count > 0));
  }

  public static int CountOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var operationCount = 0;
    foreach (var request in requests) {
      operationCount += request.HubOperations.Count + request.LinkOperations.Count + request.SatelliteOperations.Count;
    }

    return operationCount;
  }

  public static int CountSatelliteOperations(IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(requests);

    var operationCount = 0;
    foreach (var request in requests) {
      operationCount += request.SatelliteOperations.Count;
    }

    return operationCount;
  }

  private static DataVaultProviderSaveStrategyGateEvaluation Evaluate(
      DataVaultKnownProviderSaveStrategy strategy,
      string? providerName,
      bool hasPendingTrackedChanges,
      IReadOnlyList<DataVaultSaveRequest> requests,
      IReadOnlyList<string> supportedProviderNames,
      int? minimumOperationCount,
      int? maximumSatelliteOperationCount) {
    ArgumentNullException.ThrowIfNull(requests);

    var causes = new List<DataVaultSaveStrategyFallbackCause>();
    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + FormatStrategyName(strategy) + "."));
    }

    if (hasPendingTrackedChanges) {
      causes.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.DirtyDbContext,
          "The DbContext change tracker contains pending added, modified, or deleted state."));
    }

    if (ContainsMultiActiveSatelliteOperations(requests)) {
      causes.Add(new DataVaultSaveStrategyFallbackCause(
          DataVaultSaveStrategyFallbackCauseKind.MultiActiveSatelliteOperations,
          "The save batch contains one or more multi-active satellite operations."));
    }

    if (minimumOperationCount.HasValue) {
      var operationCount = CountOperations(requests);
      if (operationCount < minimumOperationCount.Value) {
        causes.Add(new DataVaultSaveStrategyFallbackCause(
            GetMinimumThresholdCauseKind(strategy),
            FormatStrategyName(strategy) +
            " optimized dispatch requires at least " +
            minimumOperationCount.Value.ToString(CultureInfo.InvariantCulture) +
            " total operations; the request batch contains " +
            operationCount.ToString(CultureInfo.InvariantCulture) +
            "."));
      }
    }

    if (maximumSatelliteOperationCount.HasValue) {
      var satelliteOperationCount = CountSatelliteOperations(requests);
      if (satelliteOperationCount > maximumSatelliteOperationCount.Value) {
        causes.Add(new DataVaultSaveStrategyFallbackCause(
            GetMaximumSatelliteThresholdCauseKind(strategy),
            FormatStrategyName(strategy) +
            " optimized dispatch accepts at most " +
            maximumSatelliteOperationCount.Value.ToString(CultureInfo.InvariantCulture) +
            " satellite operations; the request batch contains " +
            satelliteOperationCount.ToString(CultureInfo.InvariantCulture) +
            "."));
      }
    }

    return new DataVaultProviderSaveStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static DataVaultProviderSaveStrategyGateEvaluation DeclineMySqlTinySatelliteHistoryProviderNeutralFallbackBatch(
      DataVaultProviderSaveStrategyGateEvaluation evaluation,
      IReadOnlyList<DataVaultSaveRequest> requests) {
    if (!IsMySqlTinySatelliteHistoryProviderNeutralFallbackBatch(requests)) {
      return evaluation;
    }

    var requestCount = requests.Count;
    var satelliteOperationCount = CountSatelliteOperations(requests);
    var fallbackCauses = evaluation.FallbackCauses.ToList();
    if (fallbackCauses.Any(cause => cause.Kind == DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback)) {
      return new DataVaultProviderSaveStrategyGateEvaluation(false, fallbackCauses);
    }

    fallbackCauses.Add(
        new DataVaultSaveStrategyFallbackCause(
            DataVaultSaveStrategyFallbackCauseKind.MySqlTinySatelliteHistoryProviderNeutralFallback,
            "MySQL provider-native dispatch deliberately uses provider-neutral fallback for satellite-only history batches at or below " +
            MySqlTinySatelliteHistoryProviderNeutralFallbackMaximumOperationCount.ToString(CultureInfo.InvariantCulture) +
            " total operations across multiple explicit requests or at or below " +
            MySqlTinySatelliteHistoryProviderNeutralFallbackSingleRequestMaximumOperationCount.ToString(CultureInfo.InvariantCulture) +
            " operations in one explicit request; the request batch contains " +
            satelliteOperationCount.ToString(CultureInfo.InvariantCulture) +
            " satellite operations across " +
            requestCount.ToString(CultureInfo.InvariantCulture) +
            " requests."));

    return new DataVaultProviderSaveStrategyGateEvaluation(
        false,
        fallbackCauses);
  }

  private static bool IsMySqlTinySatelliteHistoryProviderNeutralFallbackBatch(
      IReadOnlyList<DataVaultSaveRequest> requests) {
    var satelliteOperationCount = 0;
    foreach (var request in requests) {
      if (request.HubOperations.Count > 0 || request.LinkOperations.Count > 0) {
        return false;
      }

      satelliteOperationCount += request.SatelliteOperations.Count;
    }

    return satelliteOperationCount > 0 &&
        satelliteOperationCount <= MySqlTinySatelliteHistoryProviderNeutralFallbackMaximumOperationCount &&
        (requests.Count > 1 ||
            satelliteOperationCount <= MySqlTinySatelliteHistoryProviderNeutralFallbackSingleRequestMaximumOperationCount);
  }

  private static DataVaultSaveStrategyFallbackCauseKind GetMinimumThresholdCauseKind(
      DataVaultKnownProviderSaveStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderSaveStrategy.SqlServer => DataVaultSaveStrategyFallbackCauseKind.SqlServerMinimumOperationThreshold,
      DataVaultKnownProviderSaveStrategy.MySql => DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
      DataVaultKnownProviderSaveStrategy.MySqlStaged => DataVaultSaveStrategyFallbackCauseKind.MySqlMinimumOperationThreshold,
      DataVaultKnownProviderSaveStrategy.Oracle => DataVaultSaveStrategyFallbackCauseKind.OracleMinimumOperationThreshold,
      _ => DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
    };
  }

  private static DataVaultSaveStrategyFallbackCauseKind GetMaximumSatelliteThresholdCauseKind(
      DataVaultKnownProviderSaveStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderSaveStrategy.SqlServer => DataVaultSaveStrategyFallbackCauseKind.SqlServerMaximumSatelliteOperationThreshold,
      DataVaultKnownProviderSaveStrategy.Oracle => DataVaultSaveStrategyFallbackCauseKind.OracleMaximumSatelliteOperationThreshold,
      _ => DataVaultSaveStrategyFallbackCauseKind.StrategyDeclined,
    };
  }

  private static string FormatStrategyName(DataVaultKnownProviderSaveStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderSaveStrategy.Sqlite => "SQLite",
      DataVaultKnownProviderSaveStrategy.Postgres => "PostgreSQL",
      DataVaultKnownProviderSaveStrategy.SqlServer => "SQL Server",
      DataVaultKnownProviderSaveStrategy.MySql => "MySQL",
      DataVaultKnownProviderSaveStrategy.MySqlStaged => "MySQL staged bulk",
      DataVaultKnownProviderSaveStrategy.Oracle => "Oracle",
      DataVaultKnownProviderSaveStrategy.Db2 => "DB2",
      _ => strategy.ToString(),
    };
  }
}
