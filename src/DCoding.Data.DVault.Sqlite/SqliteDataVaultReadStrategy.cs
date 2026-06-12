using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class SqliteDataVaultReadStrategy :
    IDataVaultProviderReadStrategy,
    IDataVaultProviderPitReadStrategy,
    IDataVaultProviderBridgeReadStrategy {
  private const int SqliteMaxCommandParameterCount = 900;

  public int Priority => 100;

  public bool CanReadLatestSatelliteRows(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(dbContext, request).CanRead;
  }

  public bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(dbContext, request).CanRead;
  }

  public bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateSqlite(dbContext, request).CanRead;
  }

  public async Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DataVaultProviderReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(context.Request.Satellite);
    var rows = await ReadLatestRowsAsync(context, projection, cancellationToken).ConfigureAwait(false);

    return rows
        .Select(row => DataVaultSatelliteReadPipeline.TryCreateReadRecord(projection, row))
        .Where(row => row is not null)
        .Cast<DataVaultSatelliteReadRecord>()
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ToArray();
  }

  public async Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
      DataVaultProviderReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(context.Request.Satellite);
    var rows = await ReadLatestRowsAsync(context, projection, cancellationToken).ConfigureAwait(false);

    return rows
        .Select(row => DataVaultSatelliteReadPipeline.CreateProjectionRow(projection, row))
        .ToArray();
  }

  public async Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitRowsAsync(
      DataVaultProviderPitReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultPitReadPipeline.CreatePitProjection(context.DbContext, context.Request);
    if (context.Request.ParentHashKeys.Count == 0) {
      return [];
    }

    var matchedPitRows = await ReadMatchedPitRowsAsync(
        context,
        projection,
        cancellationToken).ConfigureAwait(false);
    if (matchedPitRows.Count == 0) {
      return [];
    }

    var satelliteRowsByOrdinal =
        new Dictionary<int, IReadOnlyDictionary<DataVaultPitReadPipeline.SatelliteSnapshotKey, Dictionary<string, object>>>();
    for (var index = 0; index < projection.Satellites.Count; index++) {
      satelliteRowsByOrdinal[index] = await DataVaultPitReadPipeline.ReadSatelliteRowsAsync(
          context.DbContext,
          projection,
          projection.Satellites[index],
          index,
          matchedPitRows.Values,
          cancellationToken).ConfigureAwait(false);
    }

    return matchedPitRows.Values
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .Select(row => DataVaultPitReadPipeline.CreatePitReadRecord(projection, row, satelliteRowsByOrdinal))
        .ToArray();
  }

  public async Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultBridgeReadPipeline.CreateBridgeProjection(context.DbContext, context.Request);
    var rows = await ReadSqliteBridgeRowsAsync(
        context,
        projection,
        cancellationToken).ConfigureAwait(false);

    return DataVaultBridgeReadPipeline.OrderBridgeRows(
        rows.Select(row => DataVaultBridgeReadPipeline.CreateReadRecord(projection, row)),
        row => row.EndpointHashKeys.Select(endpoint => endpoint.HashKey),
        row => row.TraversalDepth);
  }

  public async Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultBridgeReadPipeline.CreateBridgeProjection(context.DbContext, context.Request);
    var rows = await ReadSqliteBridgeRowsAsync(
        context,
        projection,
        cancellationToken).ConfigureAwait(false);
    var projectionRows = rows
        .Select(row => DataVaultBridgeReadPipeline.CreateProjectionReadRow(projection, row));

    return DataVaultBridgeReadPipeline.OrderBridgeRows(
            projectionRows,
            row => row.EndpointHashKeys,
            row => row.TraversalDepth)
        .Select(row => row.ProjectionRow)
        .ToArray();
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> ReadLatestRowsAsync(
      DataVaultProviderReadStrategyContext context,
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection,
      CancellationToken cancellationToken) {
    if (context.Request.ParentHashKeys.Count == 0) {
      return [];
    }

    var connection = context.DbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    try {
      var readRows = new List<Dictionary<string, object>>();
      var selectedColumns = CreateSelectedColumns(projection);
      foreach (var parentHashKeyBatch in context.Request.ParentHashKeys.Chunk(GetParentHashKeyBatchSize(context.Request))) {
        readRows.AddRange(await ExecuteLatestRowsBatchAsync(
            context,
            projection,
            selectedColumns,
            parentHashKeyBatch,
            connection,
            cancellationToken).ConfigureAwait(false));
      }

      return readRows
          .OrderBy(row => row.TryGetValue(projection.ParentHashKeyColumnName, out var value) ? value as string : null, StringComparer.Ordinal)
          .ToArray();
    }
    finally {
      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static async Task<IReadOnlyDictionary<DataVaultPitReadPipeline.PitRowIdentityKey, DataVaultPitReadPipeline.MatchedPitRow>> ReadMatchedPitRowsAsync(
      DataVaultProviderPitReadStrategyContext context,
      DataVaultPitReadPipeline.PitReadProjection projection,
      CancellationToken cancellationToken) {
    var connection = context.DbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    try {
      var matchedRows = new Dictionary<DataVaultPitReadPipeline.PitRowIdentityKey, DataVaultPitReadPipeline.MatchedPitRow>();
      var selectedColumns = CreatePitSelectedColumns(projection);
      foreach (var parentHashKeyBatch in context.Request.ParentHashKeys.Chunk(GetPitParentHashKeyBatchSize())) {
        var persistedRows = await ExecutePitRowsBatchAsync(
            context,
            projection,
            selectedColumns,
            parentHashKeyBatch,
            connection,
            cancellationToken).ConfigureAwait(false);

        foreach (var row in persistedRows) {
          var matchedRow = DataVaultPitReadPipeline.CreateMatchedPitRow(projection, row);
          if (matchedRow.LoadTimestamp > context.Request.AsOf) {
            continue;
          }

          if (!matchedRows.TryGetValue(matchedRow.IdentityKey, out var current) ||
              matchedRow.LoadTimestamp >= current.LoadTimestamp) {
            matchedRows[matchedRow.IdentityKey] = matchedRow;
          }
        }
      }

      return matchedRows;
    }
    finally {
      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> ExecutePitRowsBatchAsync(
      DataVaultProviderPitReadStrategyContext context,
      DataVaultPitReadPipeline.PitReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      IReadOnlyList<string> parentHashKeyBatch,
      DbConnection connection,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    command.CommandText = CreatePitRowsCommandText(
        projection,
        selectedColumns,
        parentHashKeyBatch.Count);

    var parameterIndex = 0;
    foreach (var parentHashKey in parentHashKeyBatch) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreateSqliteParameterName(parameterIndex);
      parameter.Value = DataVaultHashKeyProviderValueConverter.ToProviderParameterValue(
          context.DbContext,
          projection.TableName,
          projection.ParentHashKeyColumnName,
          parentHashKey);
      command.Parameters.Add(parameter);
      parameterIndex++;
    }

    return await ReadCommandRowsAsync(
        command,
        selectedColumns,
        cancellationToken,
        (columnName, value) => DataVaultHashKeyProviderValueConverter.ReadProviderValue(
            context.DbContext,
            projection.TableName,
            columnName,
            value)).ConfigureAwait(false);
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> ReadSqliteBridgeRowsAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      DataVaultBridgeReadPipeline.BridgeReadProjection projection,
      CancellationToken cancellationToken) {
    if (context.Request.EndpointHashKeys.Count == 0) {
      return [];
    }

    var connection = context.DbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;
    if (shouldCloseConnection) {
      await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
    }

    try {
      var readRows = new List<Dictionary<string, object>>();
      var selectedColumns = CreateBridgeSelectedColumns(projection);
      foreach (var endpointHashKeyBatch in context.Request.EndpointHashKeys.Chunk(GetBridgeEndpointHashKeyBatchSize(context.Request))) {
        readRows.AddRange(await ExecuteBridgeRowsBatchAsync(
            context,
            projection,
            selectedColumns,
            endpointHashKeyBatch,
            connection,
            cancellationToken).ConfigureAwait(false));
      }

      return readRows;
    }
    finally {
      if (shouldCloseConnection) {
        await connection.CloseAsync().ConfigureAwait(false);
      }
    }
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> ExecuteBridgeRowsBatchAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      DataVaultBridgeReadPipeline.BridgeReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      IReadOnlyList<string> endpointHashKeyBatch,
      DbConnection connection,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    command.CommandText = CreateBridgeRowsCommandText(
        projection,
        selectedColumns,
        endpointHashKeyBatch.Count,
        context.Request.MaximumDepth.HasValue);

    var parameterIndex = 0;
    foreach (var endpointHashKey in endpointHashKeyBatch) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreateSqliteParameterName(parameterIndex);
      parameter.Value = DataVaultHashKeyProviderValueConverter.ToProviderParameterValue(
          context.DbContext,
          projection.TableName,
          projection.FilterColumnName,
          endpointHashKey);
      command.Parameters.Add(parameter);
      parameterIndex++;
    }

    if (context.Request.MaximumDepth.HasValue) {
      var maximumDepthParameter = command.CreateParameter();
      maximumDepthParameter.ParameterName = CreateSqliteParameterName(parameterIndex);
      maximumDepthParameter.Value = context.Request.MaximumDepth.Value;
      command.Parameters.Add(maximumDepthParameter);
    }

    return await ReadCommandRowsAsync(
        command,
        selectedColumns,
        cancellationToken,
        (columnName, value) => {
          var normalizedValue = string.Equals(columnName, projection.TraversalDepthColumnName, StringComparison.Ordinal) &&
              value is long longValue &&
              longValue >= int.MinValue &&
              longValue <= int.MaxValue
              ? (int)longValue
              : value;

          return DataVaultHashKeyProviderValueConverter.ReadProviderValue(
              context.DbContext,
              projection.TableName,
              columnName,
              normalizedValue);
        }).ConfigureAwait(false);
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> ExecuteLatestRowsBatchAsync(
      DataVaultProviderReadStrategyContext context,
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      IReadOnlyList<string> parentHashKeyBatch,
      DbConnection connection,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    command.CommandText = CreateLatestRowsCommandText(
        projection,
        selectedColumns,
        parentHashKeyBatch.Count,
        context.Request.AsOf is not null);

    var parameterIndex = 0;
    foreach (var parentHashKey in parentHashKeyBatch) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreateSqliteParameterName(parameterIndex);
      parameter.Value = DataVaultHashKeyProviderValueConverter.ToProviderParameterValue(
          context.DbContext,
          projection.TableName,
          projection.ParentHashKeyColumnName,
          parentHashKey);
      command.Parameters.Add(parameter);
      parameterIndex++;
    }

    if (context.Request.AsOf is not null) {
      var asOfParameter = command.CreateParameter();
      asOfParameter.ParameterName = CreateSqliteParameterName(parameterIndex);
      asOfParameter.Value = DataVaultLoadTimestampValueConverter.ToProviderValue(
          context.DbContext,
          projection.TableName,
          projection.LoadTimestampColumnName,
          context.Request.AsOf.Value);
      command.Parameters.Add(asOfParameter);
    }

    var readRows = new List<Dictionary<string, object>>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var row = new Dictionary<string, object>(StringComparer.Ordinal);
      for (var columnIndex = 0; columnIndex < selectedColumns.Count; columnIndex++) {
        var columnName = selectedColumns[columnIndex];
        row[columnName] = await reader.IsDBNullAsync(columnIndex, cancellationToken).ConfigureAwait(false)
            ? null!
            : DataVaultHashKeyProviderValueConverter.ReadProviderValue(
                context.DbContext,
                projection.TableName,
                columnName,
                reader.GetValue(columnIndex));
      }

      readRows.Add(row);
    }

    return readRows;
  }

  private static async Task<IReadOnlyList<Dictionary<string, object>>> ReadCommandRowsAsync(
      DbCommand command,
      IReadOnlyList<string> selectedColumns,
      CancellationToken cancellationToken,
      Func<string, object, object>? normalizeValue = null) {
    var readRows = new List<Dictionary<string, object>>();
    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false)) {
      var row = new Dictionary<string, object>(StringComparer.Ordinal);
      for (var columnIndex = 0; columnIndex < selectedColumns.Count; columnIndex++) {
        var columnName = selectedColumns[columnIndex];
        if (await reader.IsDBNullAsync(columnIndex, cancellationToken).ConfigureAwait(false)) {
          row[columnName] = null!;
          continue;
        }

        var value = reader.GetValue(columnIndex);
        row[columnName] = normalizeValue is null
            ? value
            : normalizeValue(columnName, value);
      }

      readRows.Add(row);
    }

    return readRows;
  }

  internal static string CreateLatestRowsCommandText(
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      int parentHashKeyCount,
      bool hasAsOf) {
    ArgumentNullException.ThrowIfNull(projection);
    ArgumentNullException.ThrowIfNull(selectedColumns);
    if (parentHashKeyCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(parentHashKeyCount));
    }

    var builder = new StringBuilder();
    builder.Append("SELECT ");
    AppendColumnList(builder, selectedColumns);
    builder.Append(" FROM (SELECT ");
    AppendColumnList(builder, selectedColumns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteSqliteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteSqliteIdentifier(projection.LoadTimestampColumnName))
        .Append(" DESC, rowid DESC) AS \"__dvault_row_number\" FROM ")
        .Append(QuoteSqliteIdentifier(projection.TableName))
        .Append(" WHERE ")
        .Append(QuoteSqliteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateSqliteParameterName(index));
    }

    builder.Append(')');
    if (hasAsOf) {
      builder.Append(" AND ")
          .Append(QuoteSqliteIdentifier(projection.LoadTimestampColumnName))
          .Append(" <= ")
          .Append(CreateSqliteParameterName(parentHashKeyCount));
    }

    builder.Append(") WHERE \"__dvault_row_number\" = 1 ORDER BY ")
        .Append(QuoteSqliteIdentifier(projection.ParentHashKeyColumnName));

    return builder.ToString();
  }

  internal static string CreatePitRowsCommandText(
      DataVaultPitReadPipeline.PitReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      int parentHashKeyCount) {
    ArgumentNullException.ThrowIfNull(projection);
    ArgumentNullException.ThrowIfNull(selectedColumns);
    if (parentHashKeyCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(parentHashKeyCount));
    }

    var builder = new StringBuilder();
    builder.Append("SELECT ");
    AppendColumnList(builder, selectedColumns);
    builder.Append(" FROM ")
        .Append(QuoteSqliteIdentifier(projection.TableName))
        .Append(" WHERE ")
        .Append(QuoteSqliteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateSqliteParameterName(index));
    }

    var orderColumns = new[]
    {
        projection.ParentHashKeyColumnName,
    }
        .Concat(projection.DrivingKeyColumnNames)
        .Append(projection.LoadTimestampColumnName)
        .ToArray();
    builder.Append(") ORDER BY ");
    for (var index = 0; index < orderColumns.Length; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteSqliteIdentifier(orderColumns[index]));
    }

    builder.Append(", rowid");

    return builder.ToString();
  }

  internal static string CreateBridgeRowsCommandText(
      DataVaultBridgeReadPipeline.BridgeReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      int endpointHashKeyCount,
      bool hasMaximumDepth) {
    ArgumentNullException.ThrowIfNull(projection);
    ArgumentNullException.ThrowIfNull(selectedColumns);
    if (endpointHashKeyCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(endpointHashKeyCount));
    }

    var builder = new StringBuilder();
    builder.Append("SELECT ");
    AppendColumnList(builder, selectedColumns);
    builder.Append(" FROM ")
        .Append(QuoteSqliteIdentifier(projection.TableName))
        .Append(" WHERE ")
        .Append(QuoteSqliteIdentifier(projection.FilterColumnName))
        .Append(" IN (");

    for (var index = 0; index < endpointHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateSqliteParameterName(index));
    }

    builder.Append(')');
    if (hasMaximumDepth && projection.TraversalDepthColumnName is not null) {
      builder.Append(" AND ")
          .Append(QuoteSqliteIdentifier(projection.TraversalDepthColumnName))
          .Append(" <= ")
          .Append(CreateSqliteParameterName(endpointHashKeyCount));
    }

    builder.Append(" ORDER BY ");
    for (var index = 0; index < projection.Endpoints.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteSqliteIdentifier(projection.Endpoints[index].ColumnName));
    }

    if (projection.TraversalDepthColumnName is not null) {
      builder.Append(", ")
          .Append(QuoteSqliteIdentifier(projection.TraversalDepthColumnName));
    }

    return builder.ToString();
  }

  private static IReadOnlyList<string> CreateSelectedColumns(
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection) {
    return [
        projection.ParentHashKeyColumnName,
        projection.HashDiffColumnName,
        projection.LoadTimestampColumnName,
        projection.RecordSourceColumnName,
        .. projection.PayloadColumnNames,
    ];
  }

  private static IReadOnlyList<string> CreatePitSelectedColumns(
      DataVaultPitReadPipeline.PitReadProjection projection) {
    return [
        projection.ParentHashKeyColumnName,
        .. projection.DrivingKeyColumnNames,
        projection.LoadTimestampColumnName,
        .. projection.Satellites.Select(satellite => satellite.SnapshotReferenceColumnName),
    ];
  }

  private static IReadOnlyList<string> CreateBridgeSelectedColumns(
      DataVaultBridgeReadPipeline.BridgeReadProjection projection) {
    return [
        .. projection.Endpoints.Select(endpoint => endpoint.ColumnName),
        .. (projection.TraversalDepthColumnName is null
            ? Array.Empty<string>()
            : [projection.TraversalDepthColumnName]),
    ];
  }

  private static int GetParentHashKeyBatchSize(DataVaultLatestSatelliteReadRequest request) {
    var asOfParameterCount = request.AsOf is null ? 0 : 1;
    return Math.Min(
        DataVaultSatelliteReadPipeline.ParentHashKeyBatchSize,
        SqliteMaxCommandParameterCount - asOfParameterCount);
  }

  private static int GetPitParentHashKeyBatchSize() {
    return Math.Min(
        DataVaultPitReadPipeline.ParentHashKeyBatchSize,
        SqliteMaxCommandParameterCount);
  }

  private static int GetBridgeEndpointHashKeyBatchSize(DataVaultBridgeReadRequest request) {
    var maximumDepthParameterCount = request.MaximumDepth.HasValue ? 1 : 0;
    return Math.Min(
        500,
        SqliteMaxCommandParameterCount - maximumDepthParameterCount);
  }

  private static void AppendColumnList(StringBuilder builder, IReadOnlyList<string> columns) {
    for (var index = 0; index < columns.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteSqliteIdentifier(columns[index]));
    }
  }

  private static string CreateSqliteParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string QuoteSqliteIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }
}
