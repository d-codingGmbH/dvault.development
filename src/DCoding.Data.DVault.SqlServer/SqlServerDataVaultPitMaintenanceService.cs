using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class SqlServerDataVaultPitMaintenanceService : IDataVaultPitMaintenanceService {
  internal const string StrategyName = nameof(SqlServerDataVaultPitMaintenanceService);
  private static readonly AsyncLocal<Func<CancellationToken, Task>?> BeforeCommitHookForTesting = new();

  internal static Func<CancellationToken, Task>? BeforeCommitHookForTestingAsync {
    get => BeforeCommitHookForTesting.Value;
    set => BeforeCommitHookForTesting.Value = value;
  }

  public async Task<DataVaultPitMaintenanceResult> RebuildAsync(
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    using var activity = DataVaultActivityTracing.StartMaintenanceActivity(
        dbContext,
        DataVaultActivityTracing.PitRebuildOperation,
        DataVaultActivityTracing.PitRebuildMaintenanceKind,
        DataVaultActivityTracing.PitReadModelKind,
        DataVaultActivityTracing.FullRebuildScope);

    var evaluation = EvaluateRebuildCandidate(
        dbContext.Database.ProviderName,
        DataVaultProviderSaveStrategyGateEvaluator.HasPendingTrackedChanges(dbContext),
        request.Pit,
        HasCurrentTransactionWithoutSavepoints(dbContext));
    if (!evaluation.CanRebuild) {
      activity.RecordStrategyFallback(StrategyName, evaluation.FallbackCauses.Select(cause => cause.Kind.ToString()));
      return await DefaultDataVaultPitMaintenanceService
          .RebuildProviderNeutralCoreAsync(dbContext, request, activity, cancellationToken)
          .ConfigureAwait(false);
    }

    activity.RecordStrategySelected(StrategyName);
    try {
      return await ExecuteSqlServerRebuildAsync(dbContext, request, activity, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception exception) {
      activity.RecordFailure(exception);
      throw;
    }
  }

  public async Task<DataVaultPitMaintenanceResult> MaintainParentsAsync(
      DbContext dbContext,
      DataVaultPitParentMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    using var activity = DataVaultActivityTracing.StartMaintenanceActivity(
        dbContext,
        DataVaultActivityTracing.PitMaintainParentsOperation,
        DataVaultActivityTracing.PitMaintainParentsMaintenanceKind,
        DataVaultActivityTracing.PitReadModelKind,
        DataVaultActivityTracing.ParentsRebuildScope);

    activity.RecordStrategyFallback(
        StrategyName,
        [SqlServerPitMaintenanceFallbackCauseKind.MaintainParentsUnsupported.ToString()]);
    return await DefaultDataVaultPitMaintenanceService
        .MaintainParentsProviderNeutralCoreAsync(dbContext, request, activity, cancellationToken)
        .ConfigureAwait(false);
  }

  internal static SqlServerPitMaintenanceGateEvaluation EvaluateRebuildCandidate(
      string? providerName,
      bool hasPendingTrackedChanges,
      DataVaultPitMetadata pit,
      bool hasCurrentTransactionWithoutSavepoints = false) {
    ArgumentNullException.ThrowIfNull(pit);

    var causes = new List<SqlServerPitMaintenanceFallbackCause>();
    if (!string.Equals(providerName, KnownProviderNames.SqlServer, StringComparison.Ordinal)) {
      causes.Add(new SqlServerPitMaintenanceFallbackCause(
          SqlServerPitMaintenanceFallbackCauseKind.ProviderNameMismatch,
          "Provider name '" + (providerName ?? "<null>") + "' does not match SQL Server."));
    }

    if (hasPendingTrackedChanges) {
      causes.Add(new SqlServerPitMaintenanceFallbackCause(
          SqlServerPitMaintenanceFallbackCauseKind.DirtyDbContext,
          "The DbContext change tracker contains pending added, modified, or deleted state."));
    }

    if (hasCurrentTransactionWithoutSavepoints) {
      causes.Add(new SqlServerPitMaintenanceFallbackCause(
          SqlServerPitMaintenanceFallbackCauseKind.CurrentTransactionSavepointUnavailable,
          "The current DbContext transaction does not support savepoints for rollback-clean candidate execution."));
    }

    if (pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
      causes.Add(new SqlServerPitMaintenanceFallbackCause(
          SqlServerPitMaintenanceFallbackCauseKind.UnsupportedPitParent,
          "The SQL Server PIT rebuild candidate supports hub-parent PIT metadata only."));
    }

    if (pit.Satellites.Any(satellite => satellite.IsMultiActive)) {
      causes.Add(new SqlServerPitMaintenanceFallbackCause(
          SqlServerPitMaintenanceFallbackCauseKind.MultiActivePitUnsupported,
          "The SQL Server PIT rebuild candidate supports ordinary non-multi-active PIT metadata only."));
    }

    return new SqlServerPitMaintenanceGateEvaluation(causes.Count == 0, causes);
  }

  internal static string CreateSqlServerPitRebuildInsertCommandText(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);

    return CreateSqlServerPitRebuildInsertCommandText(CreateSqlServerProjection(dbContext, projection));
  }

  internal static string CreateSqlServerPitParentCountCommandText(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);

    return CreateSqlServerPitParentCountCommandText(CreateSqlServerProjection(dbContext, projection));
  }

  private static async Task<DataVaultPitMaintenanceResult> ExecuteSqlServerRebuildAsync(
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      DataVaultMaintenanceActivity activity,
      CancellationToken cancellationToken) {
    var projection = DefaultDataVaultPitMaintenanceService.CreatePitProjection(dbContext, request.Pit);
    var sqlProjection = CreateSqlServerProjection(dbContext, projection);
    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    IDbContextTransaction? currentTransaction = dbContext.Database.CurrentTransaction;
    DbTransaction? localTransaction = null;
    var transaction = currentTransaction?.GetDbTransaction();
    if (transaction is null) {
      localTransaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
      transaction = localTransaction;
    }

    var savepointName = currentTransaction is not null && currentTransaction.SupportsSavepoints
        ? "__dvault_pit_rebuild_" + Guid.NewGuid().ToString("N")
        : null;

    try {
      if (savepointName is not null) {
        await currentTransaction!.CreateSavepointAsync(savepointName, cancellationToken).ConfigureAwait(false);
      }

      var parentHashKeyCount = await ExecuteScalarInt32Async(
          connection,
          transaction,
          CreateSqlServerPitParentCountCommandText(sqlProjection),
          cancellationToken).ConfigureAwait(false);
      var rowsDeleted = await ExecuteNonQueryAsync(
          connection,
          transaction,
          CreateSqlServerDeletePitRowsCommandText(sqlProjection),
          cancellationToken).ConfigureAwait(false);
      var rowsWritten = await ExecuteNonQueryAsync(
          connection,
          transaction,
          CreateSqlServerPitRebuildInsertCommandText(sqlProjection),
          cancellationToken).ConfigureAwait(false);

      await InvokeBeforeCommitHookForTestingAsync(cancellationToken).ConfigureAwait(false);
      cancellationToken.ThrowIfCancellationRequested();
      DefaultDataVaultPitMaintenanceService.DetachTrackedPitRows(dbContext, projection);

      if (localTransaction is not null) {
        await localTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
      }

      var result = new DataVaultPitMaintenanceResult(
          request.Pit,
          projection.TableName,
          parentHashKeyCount,
          rowsDeleted,
          rowsWritten);
      activity.RecordSuccess(
          result.RowsDeleted + result.RowsWritten,
          parentKeyCount: null,
          isNoOp: false);

      return result;
    }
    catch {
      if (localTransaction is not null) {
        await RollbackLocalTransactionAsync(localTransaction).ConfigureAwait(false);
      }
      else if (savepointName is not null) {
        await RollbackSavepointAsync(currentTransaction!, savepointName).ConfigureAwait(false);
      }

      throw;
    }
    finally {
      if (localTransaction is not null) {
        await localTransaction.DisposeAsync().ConfigureAwait(false);
      }

      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static bool HasCurrentTransactionWithoutSavepoints(DbContext dbContext) {
    var currentTransaction = dbContext.Database.CurrentTransaction;

    return currentTransaction is not null && !currentTransaction.SupportsSavepoints;
  }

  private static Task InvokeBeforeCommitHookForTestingAsync(CancellationToken cancellationToken) {
    var hook = BeforeCommitHookForTestingAsync;

    return hook is null ? Task.CompletedTask : hook(cancellationToken);
  }

  private static async Task<int> ExecuteScalarInt32Async(
      DbConnection connection,
      DbTransaction transaction,
      string commandText,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = commandText;

    var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
    return Convert.ToInt32(value, CultureInfo.InvariantCulture);
  }

  private static async Task<int> ExecuteNonQueryAsync(
      DbConnection connection,
      DbTransaction transaction,
      string commandText,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = commandText;

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static async Task RollbackLocalTransactionAsync(DbTransaction transaction) {
    try {
      await transaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
    }
    catch (DbException) {
    }
    catch (InvalidOperationException) {
    }
  }

  private static async Task RollbackSavepointAsync(
      IDbContextTransaction transaction,
      string savepointName) {
    try {
      await transaction.RollbackToSavepointAsync(savepointName, CancellationToken.None).ConfigureAwait(false);
    }
    catch (DbException) {
    }
    catch (InvalidOperationException) {
    }
  }

  private static string CreateSqlServerPitParentCountCommandText(SqlServerPitMaintenanceProjection projection) {
    var builder = new StringBuilder();
    builder.Append("SELECT COUNT(1) FROM (");
    AppendSatelliteParentSelects(builder, projection);
    builder.Append(") AS ")
        .Append(QuoteSqlServerIdentifier("__dvault_pit_parent_keys"));

    return builder.ToString();
  }

  private static string CreateSqlServerDeletePitRowsCommandText(SqlServerPitMaintenanceProjection projection) {
    return "DELETE FROM " + QuoteSqlServerTable(projection.Table);
  }

  private static string CreateSqlServerPitRebuildInsertCommandText(SqlServerPitMaintenanceProjection projection) {
    var timestampsAlias = "__dvault_pit_timestamps";
    var rowAlias = "pit_source";
    var builder = new StringBuilder();
    builder.Append("WITH ")
        .Append(QuoteSqlServerIdentifier(timestampsAlias))
        .Append(" AS (");
    AppendSatelliteTimestampSelects(builder, projection);
    builder.Append(") INSERT INTO ")
        .Append(QuoteSqlServerTable(projection.Table))
        .Append(" (");
    AppendIdentifierList(builder, projection.InsertColumnNames);
    builder.Append(") SELECT ")
        .Append(QuoteSqlServerIdentifier(rowAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(projection.ParentHashKeyColumnName))
        .Append(", ")
        .Append(QuoteSqlServerIdentifier(rowAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(projection.LoadTimestampColumnName));

    for (var index = 0; index < projection.Satellites.Count; index++) {
      builder.Append(", ");
      AppendSnapshotTimestampSelect(builder, projection, projection.Satellites[index], index, rowAlias);
    }

    builder.Append(" FROM ")
        .Append(QuoteSqlServerIdentifier(timestampsAlias))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier(rowAlias));

    return builder.ToString();
  }

  private static void AppendSatelliteParentSelects(
      StringBuilder builder,
      SqlServerPitMaintenanceProjection projection) {
    for (var index = 0; index < projection.Satellites.Count; index++) {
      if (index > 0) {
        builder.Append(" UNION ");
      }

      var satellite = projection.Satellites[index];
      var alias = "sat" + index.ToString(CultureInfo.InvariantCulture);
      builder.Append("SELECT ")
          .Append(QuoteSqlServerIdentifier(alias))
          .Append('.')
          .Append(QuoteSqlServerIdentifier(satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuoteSqlServerIdentifier(projection.ParentHashKeyColumnName))
          .Append(" FROM ")
          .Append(QuoteSqlServerTable(satellite.Table))
          .Append(" AS ")
          .Append(QuoteSqlServerIdentifier(alias));
    }
  }

  private static void AppendSatelliteTimestampSelects(
      StringBuilder builder,
      SqlServerPitMaintenanceProjection projection) {
    for (var index = 0; index < projection.Satellites.Count; index++) {
      if (index > 0) {
        builder.Append(" UNION ");
      }

      var satellite = projection.Satellites[index];
      var alias = "sat" + index.ToString(CultureInfo.InvariantCulture);
      builder.Append("SELECT ")
          .Append(QuoteSqlServerIdentifier(alias))
          .Append('.')
          .Append(QuoteSqlServerIdentifier(satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuoteSqlServerIdentifier(projection.ParentHashKeyColumnName))
          .Append(", ")
          .Append(QuoteSqlServerIdentifier(alias))
          .Append('.')
          .Append(QuoteSqlServerIdentifier(satellite.LoadTimestampColumnName))
          .Append(" AS ")
          .Append(QuoteSqlServerIdentifier(projection.LoadTimestampColumnName))
          .Append(" FROM ")
          .Append(QuoteSqlServerTable(satellite.Table))
          .Append(" AS ")
          .Append(QuoteSqlServerIdentifier(alias));
    }
  }

  private static void AppendSnapshotTimestampSelect(
      StringBuilder builder,
      SqlServerPitMaintenanceProjection projection,
      SqlServerPitSatelliteMaintenanceProjection satellite,
      int index,
      string rowAlias) {
    var snapshotAlias = "snapshot" + index.ToString(CultureInfo.InvariantCulture);
    builder.Append("(SELECT TOP(1) ")
        .Append(QuoteSqlServerIdentifier(snapshotAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(satellite.LoadTimestampColumnName))
        .Append(" FROM ")
        .Append(QuoteSqlServerTable(satellite.Table))
        .Append(" AS ")
        .Append(QuoteSqlServerIdentifier(snapshotAlias))
        .Append(" WHERE ")
        .Append(QuoteSqlServerIdentifier(snapshotAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(satellite.ParentHashKeyColumnName))
        .Append(" = ")
        .Append(QuoteSqlServerIdentifier(rowAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(projection.ParentHashKeyColumnName))
        .Append(" AND ")
        .Append(QuoteSqlServerIdentifier(snapshotAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(satellite.LoadTimestampColumnName))
        .Append(" <= ")
        .Append(QuoteSqlServerIdentifier(rowAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(projection.LoadTimestampColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteSqlServerIdentifier(snapshotAlias))
        .Append('.')
        .Append(QuoteSqlServerIdentifier(satellite.LoadTimestampColumnName))
        .Append(" DESC)");
  }

  private static SqlServerPitMaintenanceProjection CreateSqlServerProjection(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    var pitEntity = GetRequiredEntityType(dbContext, projection.TableName);
    var satellites = projection.Satellites
        .Select(satellite => CreateSqlServerSatelliteProjection(dbContext, satellite))
        .ToArray();

    return new SqlServerPitMaintenanceProjection(
        ResolveTable(pitEntity, projection.TableName),
        GetColumnName(pitEntity, projection.ParentHashKeyColumnName),
        GetColumnName(pitEntity, projection.LoadTimestampColumnName),
        projection.Satellites.Select(satellite => GetColumnName(pitEntity, satellite.SnapshotReferenceColumnName)).ToArray(),
        satellites);
  }

  private static SqlServerPitSatelliteMaintenanceProjection CreateSqlServerSatelliteProjection(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitSatelliteMaintenanceProjection projection) {
    var entityType = GetRequiredEntityType(dbContext, projection.Satellite.TableName);

    return new SqlServerPitSatelliteMaintenanceProjection(
        ResolveTable(entityType, projection.Satellite.TableName),
        GetColumnName(entityType, projection.Satellite.ParentHashKeyColumnName),
        GetColumnName(entityType, projection.Satellite.LoadTimestampColumnName));
  }

  private static IEntityType GetRequiredEntityType(DbContext dbContext, string producedName) {
    var entityType = dbContext.Model
        .GetEntityTypes()
        .SingleOrDefault(entity =>
            string.Equals(entity.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string, producedName, StringComparison.Ordinal) ||
            string.Equals(entity.GetTableName(), producedName, StringComparison.Ordinal) ||
            string.Equals(entity.Name, producedName, StringComparison.Ordinal));

    return entityType ??
        throw new InvalidOperationException("SQL Server PIT maintenance could not resolve generated table/entity '" + producedName + "'.");
  }

  private static string GetColumnName(IEntityType entityType, string propertyName) {
    var property = entityType.FindProperty(propertyName) ??
        throw new InvalidOperationException(
            "SQL Server PIT maintenance could not resolve generated property '" +
            propertyName +
            "' on table/entity '" +
            entityType.Name +
            "'.");
    var table = StoreObjectIdentifier.Table(entityType.GetTableName() ?? entityType.Name, entityType.GetSchema());

    return property.GetColumnName(table) ?? property.GetColumnName() ?? property.Name;
  }

  private static SqlServerTableIdentifier ResolveTable(IEntityType entityType, string producedName) {
    return new SqlServerTableIdentifier(entityType.GetTableName() ?? producedName, entityType.GetSchema());
  }

  private static string QuoteSqlServerTable(SqlServerTableIdentifier table) {
    if (table.SchemaName is null) {
      return QuoteSqlServerIdentifier(table.TableName);
    }

    return QuoteSqlServerIdentifier(table.SchemaName) + "." + QuoteSqlServerIdentifier(table.TableName);
  }

  private static void AppendIdentifierList(
      StringBuilder builder,
      IReadOnlyList<string> identifiers) {
    for (var index = 0; index < identifiers.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteSqlServerIdentifier(identifiers[index]));
    }
  }

  private static string QuoteSqlServerIdentifier(string identifier) {
    return "[" + identifier.Replace("]", "]]", StringComparison.Ordinal) + "]";
  }

  private sealed record SqlServerPitMaintenanceProjection(
      SqlServerTableIdentifier Table,
      string ParentHashKeyColumnName,
      string LoadTimestampColumnName,
      IReadOnlyList<string> SnapshotReferenceColumnNames,
      IReadOnlyList<SqlServerPitSatelliteMaintenanceProjection> Satellites) {
    public IReadOnlyList<string> InsertColumnNames { get; } =
    [
        ParentHashKeyColumnName,
        LoadTimestampColumnName,
        .. SnapshotReferenceColumnNames,
    ];
  }

  private sealed record SqlServerPitSatelliteMaintenanceProjection(
      SqlServerTableIdentifier Table,
      string ParentHashKeyColumnName,
      string LoadTimestampColumnName);

  private sealed record SqlServerTableIdentifier(string TableName, string? SchemaName);
}
