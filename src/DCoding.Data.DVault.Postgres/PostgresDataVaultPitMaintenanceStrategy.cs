using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class PostgresDataVaultPitMaintenanceStrategy : IDataVaultProviderPitMaintenanceStrategy {
  public int Priority => 100;

  public bool CanRebuild(
      DbContext dbContext,
      DataVaultPitRebuildRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderPitMaintenanceStrategyGateEvaluator.EvaluatePostgres(dbContext, request).CanRebuild;
  }

  public async Task<DataVaultPitMaintenanceResult> RebuildAsync(
      DataVaultProviderPitMaintenanceStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);
    cancellationToken.ThrowIfCancellationRequested();

    var evaluation = DataVaultProviderPitMaintenanceStrategyGateEvaluator.EvaluatePostgres(
        context.DbContext,
        context.Request);
    if (!evaluation.CanRebuild) {
      throw new InvalidOperationException(
          "PostgreSQL PIT maintenance strategy cannot rebuild this request. Fallback causes: " +
          string.Join(", ", evaluation.FallbackCauses.Select(cause => cause.Kind.ToString())) +
          ".");
    }

    var projection = DefaultDataVaultPitMaintenanceService.CreatePitProjection(
        context.DbContext,
        context.Request.Pit);
    var commandPlan = CreatePostgresPitRebuildCommandPlan(context.DbContext, projection);

    return await ExecutePostgresRebuildAsync(
        context.DbContext,
        context.Request,
        projection,
        commandPlan,
        cancellationToken).ConfigureAwait(false);
  }

  internal static string CreatePostgresPitRebuildInsertCommandText(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);

    return CreatePostgresPitRebuildCommandPlan(dbContext, projection).InsertCommandText;
  }

  private static async Task<DataVaultPitMaintenanceResult> ExecutePostgresRebuildAsync(
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection,
      PostgresPitRebuildCommandPlan commandPlan,
      CancellationToken cancellationToken) {
    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    DbTransaction? localTransaction = null;
    var transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();
    if (transaction is null) {
      localTransaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
      transaction = localTransaction;
    }

    try {
      var parentHashKeyCount = await ExecutePostgresScalarIntAsync(
          connection,
          transaction,
          commandPlan.ParentHashKeyCountCommandText,
          cancellationToken).ConfigureAwait(false);
      var rowsDeleted = await ExecutePostgresNonQueryAsync(
          connection,
          transaction,
          commandPlan.DeleteCommandText,
          cancellationToken).ConfigureAwait(false);

      DefaultDataVaultPitMaintenanceService.DetachTrackedPitRows(
          dbContext,
          projection,
          parentHashKeys: null);

      var rowsWritten = await ExecutePostgresNonQueryAsync(
          connection,
          transaction,
          commandPlan.InsertCommandText,
          cancellationToken).ConfigureAwait(false);

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
        await localTransaction.RollbackAsync(CancellationToken.None).ConfigureAwait(false);
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

  private static PostgresPitRebuildCommandPlan CreatePostgresPitRebuildCommandPlan(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    var sqlProjection = CreateSqlProjection(dbContext, projection);

    return new PostgresPitRebuildCommandPlan(
        CreatePostgresParentHashKeyCountCommandText(sqlProjection),
        "DELETE FROM " + sqlProjection.Pit.TableSql,
        CreatePostgresInsertCommandText(sqlProjection));
  }

  private static string CreatePostgresParentHashKeyCountCommandText(PostgresPitRebuildProjection projection) {
    var builder = new StringBuilder();
    builder.Append("SELECT COUNT(DISTINCT ")
        .Append(Qualify("parents", "parent_hash_key"))
        .Append(") FROM (");
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      if (satelliteIndex > 0) {
        builder.Append(" UNION ");
      }

      var satellite = projection.Satellites[satelliteIndex];
      var alias = CreateSatelliteAlias(satelliteIndex);
      builder.Append("SELECT ")
          .Append(Qualify(alias, satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("parent_hash_key"))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuotePostgresIdentifier(alias));
    }

    builder.Append(") AS ")
        .Append(QuotePostgresIdentifier("parents"));

    return builder.ToString();
  }

  private static string CreatePostgresInsertCommandText(PostgresPitRebuildProjection projection) {
    return projection.DrivingKeyColumnNames.Count == 0
        ? CreatePostgresOrdinaryInsertCommandText(projection)
        : CreatePostgresTupleAwareInsertCommandText(projection);
  }

  private static string CreatePostgresOrdinaryInsertCommandText(PostgresPitRebuildProjection projection) {
    var builder = new StringBuilder();
    builder.Append("WITH ")
        .Append(QuotePostgresIdentifier("pit_source"))
        .Append(" AS (");
    AppendOrdinaryPitSourceSelects(builder, projection);
    builder.Append(") INSERT INTO ")
        .Append(projection.Pit.TableSql)
        .Append(" (");
    AppendQuotedColumnList(builder, CreatePitInsertColumns(projection));
    builder.Append(") SELECT ")
        .Append(Qualify("source", "parent_hash_key"))
        .Append(", ")
        .Append(Qualify("source", "load_timestamp"));
    AppendSnapshotSelections(builder, projection);
    builder.Append(" FROM ")
        .Append(QuotePostgresIdentifier("pit_source"))
        .Append(" AS ")
        .Append(QuotePostgresIdentifier("source"));
    AppendSnapshotJoins(builder, projection);
    builder.Append(" ORDER BY ")
        .Append(Qualify("source", "parent_hash_key"))
        .Append(", ")
        .Append(Qualify("source", "load_timestamp"));

    return builder.ToString();
  }

  private static string CreatePostgresTupleAwareInsertCommandText(PostgresPitRebuildProjection projection) {
    var builder = new StringBuilder();
    builder.Append("WITH ")
        .Append(QuotePostgresIdentifier("tuple_source"))
        .Append(" AS (");
    AppendTupleSourceSelects(builder, projection);
    builder.Append("), ")
        .Append(QuotePostgresIdentifier("tuple_identity"))
        .Append(" AS (SELECT ")
        .Append(Qualify("tuple_source", "parent_hash_key"));
    for (var drivingKeyIndex = 0; drivingKeyIndex < projection.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
      builder.Append(", ")
          .Append(Qualify("tuple_source", CreateDrivingKeyAlias(drivingKeyIndex)));
    }

    builder.Append(", MIN(")
        .Append(Qualify("tuple_source", "load_timestamp"))
        .Append(") AS ")
        .Append(QuotePostgresIdentifier("first_tuple_load_timestamp"))
        .Append(" FROM ")
        .Append(QuotePostgresIdentifier("tuple_source"))
        .Append(" GROUP BY ")
        .Append(Qualify("tuple_source", "parent_hash_key"));
    for (var drivingKeyIndex = 0; drivingKeyIndex < projection.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
      builder.Append(", ")
          .Append(Qualify("tuple_source", CreateDrivingKeyAlias(drivingKeyIndex)));
    }

    builder.Append("), ")
        .Append(QuotePostgresIdentifier("pit_source"))
        .Append(" AS (");
    AppendTuplePitSourceSelects(builder, projection);
    builder.Append(") INSERT INTO ")
        .Append(projection.Pit.TableSql)
        .Append(" (");
    AppendQuotedColumnList(builder, CreatePitInsertColumns(projection));
    builder.Append(") SELECT ")
        .Append(Qualify("source", "parent_hash_key"));
    for (var drivingKeyIndex = 0; drivingKeyIndex < projection.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
      builder.Append(", ")
          .Append(Qualify("source", CreateDrivingKeyAlias(drivingKeyIndex)));
    }

    builder.Append(", ")
        .Append(Qualify("source", "load_timestamp"));
    AppendSnapshotSelections(builder, projection);
    builder.Append(" FROM ")
        .Append(QuotePostgresIdentifier("pit_source"))
        .Append(" AS ")
        .Append(QuotePostgresIdentifier("source"));
    AppendSnapshotJoins(builder, projection);
    builder.Append(" ORDER BY ")
        .Append(Qualify("source", "parent_hash_key"));
    for (var drivingKeyIndex = 0; drivingKeyIndex < projection.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
      builder.Append(", ")
          .Append(Qualify("source", CreateDrivingKeyAlias(drivingKeyIndex)));
    }

    builder.Append(", ")
        .Append(Qualify("source", "load_timestamp"));

    return builder.ToString();
  }

  private static void AppendOrdinaryPitSourceSelects(
      StringBuilder builder,
      PostgresPitRebuildProjection projection) {
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      if (satelliteIndex > 0) {
        builder.Append(" UNION ");
      }

      var satellite = projection.Satellites[satelliteIndex];
      var alias = CreateSatelliteAlias(satelliteIndex);
      builder.Append("SELECT ")
          .Append(Qualify(alias, satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("parent_hash_key"))
          .Append(", ")
          .Append(Qualify(alias, satellite.LoadTimestampColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("load_timestamp"))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuotePostgresIdentifier(alias));
    }
  }

  private static void AppendTupleSourceSelects(
      StringBuilder builder,
      PostgresPitRebuildProjection projection) {
    var tupleSatellites = projection.Satellites
        .Select((satellite, index) => new { Satellite = satellite, Index = index })
        .Where(current => current.Satellite.DrivingKeyColumnNames.Count > 0)
        .ToArray();

    for (var tupleIndex = 0; tupleIndex < tupleSatellites.Length; tupleIndex++) {
      if (tupleIndex > 0) {
        builder.Append(" UNION ALL ");
      }

      var satellite = tupleSatellites[tupleIndex].Satellite;
      var alias = CreateSatelliteAlias(tupleSatellites[tupleIndex].Index);
      builder.Append("SELECT ")
          .Append(Qualify(alias, satellite.ParentHashKeyColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("parent_hash_key"));
      for (var drivingKeyIndex = 0; drivingKeyIndex < satellite.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
        builder.Append(", ")
            .Append(Qualify(alias, satellite.DrivingKeyColumnNames[drivingKeyIndex]))
            .Append(" AS ")
            .Append(QuotePostgresIdentifier(CreateDrivingKeyAlias(drivingKeyIndex)));
      }

      builder.Append(", ")
          .Append(Qualify(alias, satellite.LoadTimestampColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("load_timestamp"))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuotePostgresIdentifier(alias));
    }
  }

  private static void AppendTuplePitSourceSelects(
      StringBuilder builder,
      PostgresPitRebuildProjection projection) {
    AppendTuplePitSourceProjection(builder, projection, "tuple_source");
    builder.Append(" FROM ")
        .Append(QuotePostgresIdentifier("tuple_source"));

    foreach (var ordinarySatellite in projection.Satellites
        .Select((satellite, index) => new { Satellite = satellite, Index = index })
        .Where(current => current.Satellite.DrivingKeyColumnNames.Count == 0)) {
      builder.Append(" UNION SELECT ")
          .Append(Qualify("identity", "parent_hash_key"));
      for (var drivingKeyIndex = 0; drivingKeyIndex < projection.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
        builder.Append(", ")
            .Append(Qualify("identity", CreateDrivingKeyAlias(drivingKeyIndex)));
      }

      var alias = CreateSatelliteAlias(ordinarySatellite.Index);
      builder.Append(", ")
          .Append(Qualify(alias, ordinarySatellite.Satellite.LoadTimestampColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("load_timestamp"))
          .Append(" FROM ")
          .Append(QuotePostgresIdentifier("tuple_identity"))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("identity"))
          .Append(" INNER JOIN ")
          .Append(ordinarySatellite.Satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuotePostgresIdentifier(alias))
          .Append(" ON ")
          .Append(Qualify(alias, ordinarySatellite.Satellite.ParentHashKeyColumnName))
          .Append(" = ")
          .Append(Qualify("identity", "parent_hash_key"))
          .Append(" AND ")
          .Append(Qualify(alias, ordinarySatellite.Satellite.LoadTimestampColumnName))
          .Append(" >= ")
          .Append(Qualify("identity", "first_tuple_load_timestamp"));
    }
  }

  private static void AppendTuplePitSourceProjection(
      StringBuilder builder,
      PostgresPitRebuildProjection projection,
      string alias) {
    builder.Append("SELECT DISTINCT ")
        .Append(Qualify(alias, "parent_hash_key"));
    for (var drivingKeyIndex = 0; drivingKeyIndex < projection.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
      builder.Append(", ")
          .Append(Qualify(alias, CreateDrivingKeyAlias(drivingKeyIndex)));
    }

    builder.Append(", ")
        .Append(Qualify(alias, "load_timestamp"));
  }

  private static void AppendSnapshotSelections(
      StringBuilder builder,
      PostgresPitRebuildProjection projection) {
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      builder.Append(", ")
          .Append(Qualify(CreateSnapshotAlias(satelliteIndex), "snapshot_load_timestamp"));
    }
  }

  private static void AppendSnapshotJoins(
      StringBuilder builder,
      PostgresPitRebuildProjection projection) {
    for (var satelliteIndex = 0; satelliteIndex < projection.Satellites.Count; satelliteIndex++) {
      var satellite = projection.Satellites[satelliteIndex];
      var satelliteAlias = CreateSnapshotSatelliteAlias(satelliteIndex);
      var snapshotAlias = CreateSnapshotAlias(satelliteIndex);
      builder.Append(" LEFT JOIN LATERAL (SELECT ")
          .Append(Qualify(satelliteAlias, satellite.LoadTimestampColumnName))
          .Append(" AS ")
          .Append(QuotePostgresIdentifier("snapshot_load_timestamp"))
          .Append(" FROM ")
          .Append(satellite.Table.TableSql)
          .Append(" AS ")
          .Append(QuotePostgresIdentifier(satelliteAlias))
          .Append(" WHERE ")
          .Append(Qualify(satelliteAlias, satellite.ParentHashKeyColumnName))
          .Append(" = ")
          .Append(Qualify("source", "parent_hash_key"));

      for (var drivingKeyIndex = 0; drivingKeyIndex < satellite.DrivingKeyColumnNames.Count; drivingKeyIndex++) {
        builder.Append(" AND ")
            .Append(Qualify(satelliteAlias, satellite.DrivingKeyColumnNames[drivingKeyIndex]))
            .Append(" = ")
            .Append(Qualify("source", CreateDrivingKeyAlias(drivingKeyIndex)));
      }

      builder.Append(" AND ")
          .Append(Qualify(satelliteAlias, satellite.LoadTimestampColumnName))
          .Append(" <= ")
          .Append(Qualify("source", "load_timestamp"))
          .Append(" ORDER BY ")
          .Append(Qualify(satelliteAlias, satellite.LoadTimestampColumnName))
          .Append(" DESC LIMIT 1) AS ")
          .Append(QuotePostgresIdentifier(snapshotAlias))
          .Append(" ON TRUE");
    }
  }

  private static IReadOnlyList<string> CreatePitInsertColumns(PostgresPitRebuildProjection projection) {
    return new[] { projection.ParentHashKeyColumnName }
        .Concat(projection.DrivingKeyColumnNames)
        .Concat([projection.LoadTimestampColumnName])
        .Concat(projection.Satellites.Select(satellite => satellite.SnapshotReferenceColumnName))
        .ToArray();
  }

  private static PostgresPitRebuildProjection CreateSqlProjection(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitMaintenanceProjection projection) {
    var pitTable = CreateTableProjection(dbContext, projection.TableName);
    var satellites = projection.Satellites
        .Select(satellite => CreateSqlSatelliteProjection(dbContext, satellite))
        .ToArray();

    return new PostgresPitRebuildProjection(
        pitTable,
        GetColumnName(pitTable, projection.ParentHashKeyColumnName),
        projection.DrivingKeyColumnNames.Select(columnName => GetColumnName(pitTable, columnName)).ToArray(),
        GetColumnName(pitTable, projection.LoadTimestampProperty),
        projection.Satellites
            .Zip(satellites)
            .Select(pair => pair.Second with {
              SnapshotReferenceColumnName = GetColumnName(pitTable, pair.First.SnapshotReferenceProperty),
            })
            .ToArray());
  }

  private static PostgresPitRebuildSatelliteProjection CreateSqlSatelliteProjection(
      DbContext dbContext,
      DefaultDataVaultPitMaintenanceService.PitSatelliteMaintenanceProjection projection) {
    var table = CreateTableProjection(dbContext, projection.Satellite.TableName);

    return new PostgresPitRebuildSatelliteProjection(
        table,
        GetColumnName(table, projection.Satellite.ParentHashKeyColumnName),
        projection.Satellite.DrivingKeyColumnNames.Select(columnName => GetColumnName(table, columnName)).ToArray(),
        GetColumnName(table, projection.Satellite.LoadTimestampColumnName),
        SnapshotReferenceColumnName: string.Empty);
  }

  private static PostgresTableProjection CreateTableProjection(DbContext dbContext, string producedTableName) {
    var entityType = FindEntityType(dbContext, producedTableName) ??
        throw new InvalidOperationException(
            "PostgreSQL optimized PIT rebuild could not find generated table/entity '" +
            producedTableName +
            "'.");
    var tableName = entityType.GetTableName() ?? producedTableName;
    var schemaName = entityType.GetSchema();

    return new PostgresTableProjection(
        producedTableName,
        tableName,
        schemaName,
        QuotePostgresTableIdentifier(tableName, schemaName),
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

  private static string GetColumnName(PostgresTableProjection table, string propertyName) {
    var property = table.EntityType.FindProperty(propertyName) ??
        throw new InvalidOperationException(
            "PostgreSQL optimized PIT rebuild could not find generated property '" +
            propertyName +
            "' on table/entity '" +
            table.ProducedTableName +
            "'.");

    return GetColumnName(table, property);
  }

  private static string GetColumnName(PostgresTableProjection table, IProperty property) {
    var storeObject = StoreObjectIdentifier.Table(table.TableName, table.SchemaName);

    return property.GetColumnName(storeObject) ?? property.GetColumnName() ?? property.Name;
  }

  private static async Task<int> ExecutePostgresScalarIntAsync(
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

  private static async Task<int> ExecutePostgresNonQueryAsync(
      DbConnection connection,
      DbTransaction transaction,
      string commandText,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = commandText;

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string Qualify(string tableAlias, string columnName) {
    return QuotePostgresIdentifier(tableAlias) + "." + QuotePostgresIdentifier(columnName);
  }

  private static void AppendQuotedColumnList(StringBuilder builder, IReadOnlyList<string> columns) {
    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuotePostgresIdentifier(columns[columnIndex]));
    }
  }

  private static string QuotePostgresTableIdentifier(string tableName, string? schemaName) {
    return string.IsNullOrWhiteSpace(schemaName)
        ? QuotePostgresIdentifier(tableName)
        : QuotePostgresIdentifier(schemaName) + "." + QuotePostgresIdentifier(tableName);
  }

  private static string QuotePostgresIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string CreateSatelliteAlias(int index) {
    return "satellite_" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateSnapshotAlias(int index) {
    return "snapshot_" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateSnapshotSatelliteAlias(int index) {
    return "snapshot_satellite_" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateDrivingKeyAlias(int index) {
    return "driving_key_" + index.ToString(CultureInfo.InvariantCulture);
  }

  private sealed record PostgresPitRebuildCommandPlan(
      string ParentHashKeyCountCommandText,
      string DeleteCommandText,
      string InsertCommandText);

  private sealed record PostgresPitRebuildProjection(
      PostgresTableProjection Pit,
      string ParentHashKeyColumnName,
      IReadOnlyList<string> DrivingKeyColumnNames,
      string LoadTimestampColumnName,
      IReadOnlyList<PostgresPitRebuildSatelliteProjection> Satellites);

  private sealed record PostgresPitRebuildSatelliteProjection(
      PostgresTableProjection Table,
      string ParentHashKeyColumnName,
      IReadOnlyList<string> DrivingKeyColumnNames,
      string LoadTimestampColumnName,
      string SnapshotReferenceColumnName);

  private sealed record PostgresTableProjection(
      string ProducedTableName,
      string TableName,
      string? SchemaName,
      string TableSql,
      IEntityType EntityType);
}
