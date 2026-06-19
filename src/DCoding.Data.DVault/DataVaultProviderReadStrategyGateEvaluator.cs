using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DCoding.Data.DVault;

internal static class DataVaultProviderReadStrategyGateEvaluator {
  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlite(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluatePostgres(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateSqlServer(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateMySql(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateOracle(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateDb2(dbContext.Database.ProviderName, request);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateDb2(
        dbContext.Database.ProviderName,
        request,
        HasCompletePitReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return EvaluateDb2(
        dbContext.Database.ProviderName,
        request,
        HasCompleteBridgeReadShapeEvidence(dbContext, request),
        HasStaleReadModelMaintenanceSignal(dbContext));
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.Sqlite,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Sqlite]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateSqlite(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlite(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Sqlite,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Sqlite],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateSqlite(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlite(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlite(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Sqlite,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Sqlite],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.Postgres,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Postgres]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluatePostgres(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluatePostgres(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Postgres,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Postgres],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluatePostgres(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluatePostgres(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluatePostgres(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Postgres,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Postgres],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.SqlServer,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.SqlServer]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateSqlServer(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlServer(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.SqlServer,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.SqlServer],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateSqlServer(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateSqlServer(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateSqlServer(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.SqlServer,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.SqlServer],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.MySql,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateMySql(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateMySql(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.MySql,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateMySql(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateMySql(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateMySql(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.MySql,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.Oracle,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Oracle]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateOracle(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateOracle(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Oracle,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Oracle],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateOracle(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateOracle(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateOracle(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Oracle,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Oracle],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultLatestSatelliteReadRequest request) {
    return EvaluateLatestSatellite(
        DataVaultKnownProviderReadStrategy.Db2,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Db2]);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultPitAsOfReadRequest request) {
    return EvaluateDb2(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateDb2(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluatePit(
        DataVaultKnownProviderReadStrategy.Db2,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Db2],
        supportsLinkParent: true,
        supportsMultiActive: true,
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultBridgeReadRequest request) {
    return EvaluateDb2(providerName, request, hasCompleteReadShapeEvidence: true);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence) {
    return EvaluateDb2(
        providerName,
        request,
        hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: false);
  }

  public static DataVaultProviderReadStrategyGateEvaluation EvaluateDb2(
      string? providerName,
      DataVaultBridgeReadRequest request,
      bool hasCompleteReadShapeEvidence,
      bool hasStaleReadModelMaintenanceSignal) {
    return EvaluateBridge(
        DataVaultKnownProviderReadStrategy.Db2,
        providerName,
        request,
        supportedProviderNames: [KnownProviderNames.Db2],
        hasCompleteReadShapeEvidence: hasCompleteReadShapeEvidence,
        hasStaleReadModelMaintenanceSignal: hasStaleReadModelMaintenanceSignal);
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderReadStrategy strategy,
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      out DataVaultProviderReadStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => EvaluateSqlite(dbContext, request),
      "PostgresDataVaultReadStrategy" => EvaluatePostgres(dbContext, request),
      "SqlServerDataVaultReadStrategy" => EvaluateSqlServer(dbContext, request),
      "MySqlDataVaultReadStrategy" => EvaluateMySql(dbContext, request),
      "OracleDataVaultReadStrategy" => EvaluateOracle(dbContext, request),
      "Db2DataVaultReadStrategy" => EvaluateDb2(dbContext, request),
      _ => new DataVaultProviderReadStrategyGateEvaluation(false, Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanRead;
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderPitReadStrategy strategy,
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      out DataVaultProviderReadStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => EvaluateSqlite(dbContext, request),
      "PostgresDataVaultReadStrategy" => EvaluatePostgres(dbContext, request),
      "SqlServerDataVaultReadStrategy" => EvaluateSqlServer(dbContext, request),
      "MySqlDataVaultReadStrategy" => EvaluateMySql(dbContext, request),
      "OracleDataVaultReadStrategy" => EvaluateOracle(dbContext, request),
      "Db2DataVaultReadStrategy" => EvaluateDb2(dbContext, request),
      _ => new DataVaultProviderReadStrategyGateEvaluation(false, Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanRead;
  }

  public static bool TryEvaluateKnownStrategy(
      IDataVaultProviderBridgeReadStrategy strategy,
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      out DataVaultProviderReadStrategyGateEvaluation evaluation) {
    evaluation = strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => EvaluateSqlite(dbContext, request),
      "PostgresDataVaultReadStrategy" => EvaluatePostgres(dbContext, request),
      "SqlServerDataVaultReadStrategy" => EvaluateSqlServer(dbContext, request),
      "MySqlDataVaultReadStrategy" => EvaluateMySql(dbContext, request),
      "OracleDataVaultReadStrategy" => EvaluateOracle(dbContext, request),
      "Db2DataVaultReadStrategy" => EvaluateDb2(dbContext, request),
      _ => new DataVaultProviderReadStrategyGateEvaluation(false, Array.Empty<DataVaultReadStrategyFallbackCause>()),
    };

    return evaluation.FallbackCauses.Count > 0 || evaluation.CanRead;
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return GetKnownStrategySupportedProviderNames(strategy.GetType().Name);
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderPitReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return GetKnownStrategySupportedProviderNames(strategy.GetType().Name);
  }

  public static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(IDataVaultProviderBridgeReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return GetKnownStrategySupportedProviderNames(strategy.GetType().Name);
  }

  public static IReadOnlyList<DataVaultReadStrategyGateRequirement> GetKnownLatestSatelliteGateRequirements(
      IDataVaultProviderReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      "PostgresDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      "SqlServerDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      "MySqlDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      "OracleDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      "Db2DataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported),
      ],
      _ => Array.Empty<DataVaultReadStrategyGateRequirement>(),
    };
  }

  public static IReadOnlyList<DataVaultReadStrategyGateRequirement> GetKnownPitGateRequirements(
      IDataVaultProviderPitReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "PostgresDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "SqlServerDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "MySqlDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "OracleDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "Db2DataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      _ => Array.Empty<DataVaultReadStrategyGateRequirement>(),
    };
  }

  public static IReadOnlyList<DataVaultReadStrategyGateRequirement> GetKnownBridgeGateRequirements(
      IDataVaultProviderBridgeReadStrategy strategy) {
    ArgumentNullException.ThrowIfNull(strategy);

    return strategy.GetType().Name switch {
      "SqliteDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "PostgresDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "SqlServerDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "MySqlDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "OracleDataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      "Db2DataVaultReadStrategy" => [
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence),
          new DataVaultReadStrategyGateRequirement(DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance),
      ],
      _ => Array.Empty<DataVaultReadStrategyGateRequirement>(),
    };
  }

  private static DataVaultProviderReadStrategyGateEvaluation EvaluateLatestSatellite(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      DataVaultLatestSatelliteReadRequest request,
      IReadOnlyList<string> supportedProviderNames) {
    ArgumentNullException.ThrowIfNull(request);

    var causes = new List<DataVaultReadStrategyFallbackCause>();
    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + FormatStrategyName(strategy) + "."));
    }

    if (request.Satellite.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedSatelliteParent,
          FormatStrategyName(strategy) + " optimized latest/as-of satellite reads support hub-parent satellites only."));
    }

    if (request.Satellite.DrivingKeyNames.Count > 0) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.MultiActiveSatelliteUnsupported,
          FormatStrategyName(strategy) + " optimized latest/as-of satellite reads do not support multi-active driving keys."));
    }

    return new DataVaultProviderReadStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static DataVaultProviderReadStrategyGateEvaluation EvaluatePit(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      DataVaultPitAsOfReadRequest request,
      IReadOnlyList<string> supportedProviderNames,
      bool supportsLinkParent = false,
      bool supportsMultiActive = false,
      bool hasCompleteReadShapeEvidence = true,
      bool hasStaleReadModelMaintenanceSignal = false) {
    ArgumentNullException.ThrowIfNull(request);

    var causes = CreateProviderMismatchCauses(strategy, providerName, supportedProviderNames);
    AddStaleReadModelMaintenanceCause(causes, strategy, hasStaleReadModelMaintenanceSignal, "PIT");

    if (!hasCompleteReadShapeEvidence) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence,
          FormatStrategyName(strategy) + " optimized PIT reads require a complete generated PIT table/entity projection and referenced satellite projection evidence in the DbContext model."));
    }

    if (request.Pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub &&
        (!supportsLinkParent || request.Pit.Parent.Kind != DataVaultMetadataReferenceKind.Link)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          supportsLinkParent
              ? FormatStrategyName(strategy) + " optimized PIT reads support hub- or link-parent PIT declarations only."
              : FormatStrategyName(strategy) + " optimized PIT reads support hub-parent PIT declarations only."));
    }

    if (request.Pit.Satellites.Count == 0) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized PIT reads require at least one satellite snapshot reference."));
    }

    if (!supportsMultiActive && request.Pit.Satellites.Any(satellite => satellite.IsMultiActive)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized PIT reads do not support multi-active satellite references."));
    }

    if (supportsLinkParent &&
        request.Pit.Parent.Kind == DataVaultMetadataReferenceKind.Link &&
        request.Pit.Satellites.Any(satellite => satellite.IsMultiActive)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized link-parent PIT reads require non-multi-active satellite references."));
    }

    var duplicateSatelliteName = request.Pit.Satellites
        .GroupBy(satellite => satellite.SatelliteName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();
    if (duplicateSatelliteName is not null) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedPitShape,
          FormatStrategyName(strategy) + " optimized PIT reads require distinct satellite snapshot references."));
    }

    return new DataVaultProviderReadStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static DataVaultProviderReadStrategyGateEvaluation EvaluateBridge(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      DataVaultBridgeReadRequest request,
      IReadOnlyList<string> supportedProviderNames,
      bool hasCompleteReadShapeEvidence = true,
      bool hasStaleReadModelMaintenanceSignal = false) {
    ArgumentNullException.ThrowIfNull(request);

    var causes = CreateProviderMismatchCauses(strategy, providerName, supportedProviderNames);
    AddStaleReadModelMaintenanceCause(causes, strategy, hasStaleReadModelMaintenanceSignal, "bridge");

    if (!hasCompleteReadShapeEvidence) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.IncompleteReadShapeEvidence,
          FormatStrategyName(strategy) + " optimized bridge reads require a complete generated bridge table/entity projection in the DbContext model."));
    }

    if (request.Bridge.ProjectionFeatures != DataVaultBridgeProjectionFeatures.None) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape,
          FormatStrategyName(strategy) + " optimized bridge reads support endpoint hash keys and TraversalDepth only."));
    }

    if (request.Bridge.Kind is not DataVaultBridgeKind.ManyToMany and not DataVaultBridgeKind.Hierarchy) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.UnsupportedBridgeShape,
          FormatStrategyName(strategy) + " optimized bridge reads support many-to-many and hierarchy bridges only."));
    }

    return new DataVaultProviderReadStrategyGateEvaluation(causes.Count == 0, causes);
  }

  private static bool HasCompletePitReadShapeEvidence(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    try {
      _ = DataVaultPitReadPipeline.CreatePitProjection(dbContext, request);
      return true;
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return false;
    }
  }

  private static bool HasCompleteBridgeReadShapeEvidence(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    try {
      _ = DataVaultBridgeReadPipeline.CreateBridgeProjection(dbContext, request);
      return true;
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or NotSupportedException) {
      return false;
    }
  }

  private static bool HasStaleReadModelMaintenanceSignal(DbContext dbContext) {
    try {
      return dbContext.ChangeTracker.HasChanges();
    }
    catch (Exception exception) when (exception is InvalidOperationException or NotSupportedException) {
      return true;
    }
  }

  private static void AddStaleReadModelMaintenanceCause(
      ICollection<DataVaultReadStrategyFallbackCause> causes,
      DataVaultKnownProviderReadStrategy strategy,
      bool hasStaleReadModelMaintenanceSignal,
      string readModelKind) {
    if (!hasStaleReadModelMaintenanceSignal) {
      return;
    }

    causes.Add(new DataVaultReadStrategyFallbackCause(
        DataVaultReadStrategyFallbackCauseKind.StaleReadModelMaintenance,
        FormatStrategyName(strategy) + " optimized " + readModelKind + " reads require clean context evidence because pending tracked changes can make caller-maintained read-model rows stale."));
  }

  private static List<DataVaultReadStrategyFallbackCause> CreateProviderMismatchCauses(
      DataVaultKnownProviderReadStrategy strategy,
      string? providerName,
      IReadOnlyList<string> supportedProviderNames) {
    var causes = new List<DataVaultReadStrategyFallbackCause>();
    if (!supportedProviderNames.Contains(providerName, StringComparer.Ordinal)) {
      causes.Add(new DataVaultReadStrategyFallbackCause(
          DataVaultReadStrategyFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match " + FormatStrategyName(strategy) + "."));
    }

    return causes;
  }

  private static IReadOnlyList<string> GetKnownStrategySupportedProviderNames(string strategyName) {
    return strategyName switch {
      "SqliteDataVaultReadStrategy" => [KnownProviderNames.Sqlite],
      "PostgresDataVaultReadStrategy" => [KnownProviderNames.Postgres],
      "SqlServerDataVaultReadStrategy" => [KnownProviderNames.SqlServer],
      "MySqlDataVaultReadStrategy" => [KnownProviderNames.MySqlPomelo, KnownProviderNames.MySqlOracle],
      "OracleDataVaultReadStrategy" => [KnownProviderNames.Oracle],
      "Db2DataVaultReadStrategy" => [KnownProviderNames.Db2],
      _ => Array.Empty<string>(),
    };
  }

  private static string FormatStrategyName(DataVaultKnownProviderReadStrategy strategy) {
    return strategy switch {
      DataVaultKnownProviderReadStrategy.Sqlite => "SQLite",
      DataVaultKnownProviderReadStrategy.Postgres => "PostgreSQL",
      DataVaultKnownProviderReadStrategy.SqlServer => "SQL Server",
      DataVaultKnownProviderReadStrategy.MySql => "MySQL",
      DataVaultKnownProviderReadStrategy.Oracle => "Oracle",
      DataVaultKnownProviderReadStrategy.Db2 => "DB2",
      _ => strategy.ToString(),
    };
  }
}
