using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class MySqlDataVaultPitMaintenanceStrategy : IDataVaultProviderPitMaintenanceStrategy {
  private static readonly AsyncLocal<Func<CancellationToken, Task>?> BeforeCommitHookForTesting = new();

  internal static Func<CancellationToken, Task>? BeforeCommitHookForTestingAsync {
    get => BeforeCommitHookForTesting.Value;
    set => BeforeCommitHookForTesting.Value = value;
  }

  public int Priority => 100;

  public bool CanRebuild(
      DbContext dbContext,
      DataVaultPitRebuildRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderPitMaintenanceStrategyGateEvaluator.EvaluateMySql(dbContext, request).CanRebuild;
  }

  public async Task<DataVaultPitMaintenanceResult> RebuildAsync(
      DataVaultProviderPitMaintenanceStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);
    cancellationToken.ThrowIfCancellationRequested();

    var projection = DefaultDataVaultPitMaintenanceService.CreatePitProjection(
        context.DbContext,
        context.Request.Pit);
    var commandPlan = CreateMySqlPitRebuildCommandPlan(context.DbContext, projection);

    return await ExecuteMySqlRebuildAsync(
        context.DbContext,
        context.Request,
        projection,
        commandPlan,
        cancellationToken).ConfigureAwait(false);
  }

  internal static string CreateMySqlPitRebuildInsertCommandText(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);

    return CreateMySqlPitRebuildCommandPlan(dbContext, projection).InsertCommandText;
  }

  internal static string CreateMySqlPitParentCountCommandText(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);

    return CreateMySqlPitRebuildCommandPlan(dbContext, projection).ParentHashKeyCountCommandText;
  }

  private static async Task<DataVaultPitMaintenanceResult> ExecuteMySqlRebuildAsync(
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection,
      MySqlPitRebuildCommandPlan commandPlan,
      CancellationToken cancellationToken) {
    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    var currentTransaction = dbContext.Database.CurrentTransaction;
    DbTransaction? localTransaction = null;
    var transaction = currentTransaction?.GetDbTransaction();
    if (transaction is null) {
      localTransaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
      transaction = localTransaction;
    }

    var savepointName = currentTransaction is not null && currentTransaction.SupportsSavepoints
        ? "__dvault_pit_rebuild_" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)
        : null;

    try {
      if (savepointName is not null) {
        await currentTransaction!.CreateSavepointAsync(savepointName, cancellationToken).ConfigureAwait(false);
      }

      var parentHashKeyCount = await ExecuteMySqlScalarInt32Async(
          connection,
          transaction,
          commandPlan.ParentHashKeyCountCommandText,
          cancellationToken).ConfigureAwait(false);
      var rowsDeleted = await ExecuteMySqlNonQueryAsync(
          connection,
          transaction,
          commandPlan.DeleteCommandText,
          cancellationToken).ConfigureAwait(false);
      var rowsWritten = await ExecuteMySqlNonQueryAsync(
          connection,
          transaction,
          commandPlan.InsertCommandText,
          cancellationToken).ConfigureAwait(false);

      await InvokeBeforeCommitHookForTestingAsync(cancellationToken).ConfigureAwait(false);
      cancellationToken.ThrowIfCancellationRequested();
      DefaultDataVaultPitMaintenanceService.DetachTrackedPitRows(dbContext, projection);

      if (localTransaction is not null) {
        await localTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
      }

      return new DataVaultPitMaintenanceResult(
          request.Pit,
          projection.TableName,
          parentHashKeyCount,
          rowsDeleted,
          rowsWritten);
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

  private static MySqlPitRebuildCommandPlan CreateMySqlPitRebuildCommandPlan(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    if (projection.DrivingKeyColumnNames.Count > 0) {
      throw new InvalidOperationException(
          "MySQL optimized PIT rebuild supports ordinary non-multi-active PIT declarations only.");
    }

    var sqlProjection = CreateSqlProjection(dbContext, projection);

    return new MySqlPitRebuildCommandPlan(
        CreateMySqlParentHashKeyCountCommandText(sqlProjection),
        "DELETE FROM " + sqlProjection.Pit.TableSql,
        CreateMySqlInsertCommandText(sqlProjection));
  }

  private static string CreateMySqlParentHashKeyCountCommandText(MySqlPitRebuildProjection projection) {
    var builder = new StringBuilder();
    builder.Append("SELECT COUNT(DISTINCT ")
        .Append(Qualify("parents", "parent_hash_key"))
        .Append(") FROM (");
    AppendSatelliteParentSelects(builder, projection);
    builder.Append(") AS ")
        .Append(QuoteMySqlIdentifier("parents"));

    return builder.ToString();
  }

  private static string CreateMySqlInsertCommandText(MySqlPitRebuildProjection projection) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(projection.Pit.TableSql)
        .Append(" (");
    AppendQuotedColumnList(builder, CreatePitInsertColumns(projection));
    builder.Append(") SELECT ")
        .Append(Qualify("source", "parent_hash_key"))
        .Append(", ")
        .Append(Qualify("source", "load_timestamp"));
    AppendSnapshotSelections(builder, projection);
    builder.Append(" FROM (");
    AppendSatelliteTimestampSelects(builder, projection);
    builder.Append(") AS ")
        .Append(QuoteMySqlIdentifier("source"))
        .Append(" ORDER BY ")
        .Append(Qualify("source", "parent_hash_key"))
        .Append(", ")
        .Append(Qualify("source", "load_timestamp"));

    return builder.ToString();
  }

  private static void AppendSatelliteParentSelects(
      StringBuilder builder,
      MySqlPitRebuildProjection projection) {
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      if (satelliteIndex > 0) {
        builder.Append(" UNION ");
      }

      var satellite = projection.Satellites[satelliteIndex];
      var alias = CreateSatelliteAlias(satelliteIndex);
      builder.Append("SELECT ")
          .Append(Qualify(alias, satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuoteMySqlIdentifier("parent_hash_key"))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuoteMySqlIdentifier(alias));
    }
  }

  private static void AppendSatelliteTimestampSelects(
      StringBuilder builder,
      MySqlPitRebuildProjection projection) {
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      if (satelliteIndex > 0) {
        builder.Append(" UNION ");
      }

      var satellite = projection.Satellites[satelliteIndex];
      var alias = CreateSatelliteAlias(satelliteIndex);
      builder.Append("SELECT ")
          .Append(Qualify(alias, satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuoteMySqlIdentifier("parent_hash_key"))
          .Append(", ")
          .Append(Qualify(alias, satellite.LoadTimestampColumnName))
          .Append(" AS ")
          .Append(QuoteMySqlIdentifier("load_timestamp"))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuoteMySqlIdentifier(alias));
    }
  }

  private static void AppendSnapshotSelections(
      StringBuilder builder,
      MySqlPitRebuildProjection projection) {
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      var satellite = projection.Satellites[satelliteIndex];
      var alias = CreateSnapshotAlias(satelliteIndex);
      builder.Append(", (SELECT ")
          .Append(Qualify(alias, satellite.LoadTimestampColumnName))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuoteMySqlIdentifier(alias))
          .Append(" WHERE ")
          .Append(Qualify(alias, satellite.ParentHashKeyColumnName))
          .Append(" = ")
          .Append(Qualify("source", "parent_hash_key"))
          .Append(" AND ")
          .Append(Qualify(alias, satellite.LoadTimestampColumnName))
          .Append(" <= ")
          .Append(Qualify("source", "load_timestamp"))
          .Append(" ORDER BY ")
          .Append(Qualify(alias, satellite.LoadTimestampColumnName))
          .Append(" DESC LIMIT 1)");
    }
  }

  private static IReadOnlyList<string> CreatePitInsertColumns(MySqlPitRebuildProjection projection) {
    return new[] { projection.ParentHashKeyColumnName }
        .Concat([projection.LoadTimestampColumnName])
        .Concat(projection.Satellites.Select(satellite => satellite.SnapshotReferenceColumnName))
        .ToArray();
  }

  private static MySqlPitRebuildProjection CreateSqlProjection(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    var pitTable = CreateTableProjection(dbContext, projection.TableName);
    var satellites = projection.Satellites
        .Select(satellite => CreateSqlSatelliteProjection(dbContext, satellite))
        .ToArray();

    return new MySqlPitRebuildProjection(
        pitTable,
        GetColumnName(pitTable, projection.ParentHashKeyColumnName),
        GetColumnName(pitTable, projection.LoadTimestampProperty),
        projection.Satellites
            .Zip(satellites)
            .Select(pair => pair.Second with {
              SnapshotReferenceColumnName = GetColumnName(pitTable, pair.First.SnapshotReferenceProperty),
            })
            .ToArray());
  }

  private static MySqlPitRebuildSatelliteProjection CreateSqlSatelliteProjection(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitSatelliteMaintenanceProjection projection) {
    var table = CreateTableProjection(dbContext, projection.Satellite.TableName);

    return new MySqlPitRebuildSatelliteProjection(
        table,
        GetColumnName(table, projection.Satellite.ParentHashKeyColumnName),
        GetColumnName(table, projection.Satellite.LoadTimestampColumnName),
        SnapshotReferenceColumnName: string.Empty);
  }

  private static MySqlTableProjection CreateTableProjection(DbContext dbContext, string producedTableName) {
    var entityType = FindEntityType(dbContext, producedTableName) ??
        throw new InvalidOperationException(
            "MySQL optimized PIT rebuild could not find generated table/entity '" +
            producedTableName +
            "'.");
    var tableName = entityType.GetTableName() ?? producedTableName;
    var schemaName = entityType.GetSchema();

    return new MySqlTableProjection(
        producedTableName,
        tableName,
        schemaName,
        QuoteMySqlTableIdentifier(tableName, schemaName),
        entityType);
  }

  private static IEntityType? FindEntityType(DbContext dbContext, string producedTableName) {
    foreach (var entityType in dbContext.Model.GetEntityTypes()) {
      var producedName = entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string;
      if (string.Equals(producedName, producedTableName, StringComparison.Ordinal)) {
        return entityType;
      }
    }

    foreach (var entityType in dbContext.Model.GetEntityTypes()) {
      if (string.Equals(entityType.GetTableName(), producedTableName, StringComparison.Ordinal) ||
          string.Equals(entityType.Name, producedTableName, StringComparison.Ordinal)) {
        return entityType;
      }
    }

    return null;
  }

  private static string GetColumnName(MySqlTableProjection table, string propertyName) {
    var property = table.EntityType.FindProperty(propertyName) ??
        throw new InvalidOperationException(
            "MySQL optimized PIT rebuild could not find generated property '" +
            propertyName +
            "' on table/entity '" +
            table.ProducedTableName +
            "'.");

    return GetColumnName(table, property);
  }

  private static string GetColumnName(MySqlTableProjection table, IProperty property) {
    var storeObject = StoreObjectIdentifier.Table(table.TableName, table.SchemaName);

    return property.GetColumnName(storeObject) ?? property.GetColumnName() ?? property.Name;
  }

  private static Task InvokeBeforeCommitHookForTestingAsync(CancellationToken cancellationToken) {
    var hook = BeforeCommitHookForTestingAsync;

    return hook is null ? Task.CompletedTask : hook(cancellationToken);
  }

  private static async Task<int> ExecuteMySqlScalarInt32Async(
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

  private static async Task<int> ExecuteMySqlNonQueryAsync(
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

  private static string Qualify(string tableAlias, string columnName) {
    return QuoteMySqlIdentifier(tableAlias) + "." + QuoteMySqlIdentifier(columnName);
  }

  private static void AppendQuotedColumnList(StringBuilder builder, IReadOnlyList<string> columns) {
    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteMySqlIdentifier(columns[columnIndex]));
    }
  }

  private static string QuoteMySqlTableIdentifier(string tableName, string? schemaName) {
    return string.IsNullOrWhiteSpace(schemaName)
        ? QuoteMySqlIdentifier(tableName)
        : QuoteMySqlIdentifier(schemaName) + "." + QuoteMySqlIdentifier(tableName);
  }

  private static string QuoteMySqlIdentifier(string identifier) {
    return "`" + identifier.Replace("`", "``", StringComparison.Ordinal) + "`";
  }

  private static string CreateSatelliteAlias(int index) {
    return "satellite_" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateSnapshotAlias(int index) {
    return "snapshot_" + index.ToString(CultureInfo.InvariantCulture);
  }

  private sealed record MySqlPitRebuildCommandPlan(
      string ParentHashKeyCountCommandText,
      string DeleteCommandText,
      string InsertCommandText);

  private sealed record MySqlPitRebuildProjection(
      MySqlTableProjection Pit,
      string ParentHashKeyColumnName,
      string LoadTimestampColumnName,
      IReadOnlyList<MySqlPitRebuildSatelliteProjection> Satellites);

  private sealed record MySqlPitRebuildSatelliteProjection(
      MySqlTableProjection Table,
      string ParentHashKeyColumnName,
      string LoadTimestampColumnName,
      string SnapshotReferenceColumnName);

  private sealed record MySqlTableProjection(
      string ProducedTableName,
      string TableName,
      string? SchemaName,
      string TableSql,
      IEntityType EntityType);
}
