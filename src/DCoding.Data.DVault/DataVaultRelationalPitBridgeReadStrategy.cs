using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal abstract class DataVaultRelationalPitBridgeReadStrategy :
    IDataVaultProviderPitReadStrategy,
    IDataVaultProviderBridgeReadStrategy {
  public int Priority => 100;

  protected abstract int MaxCommandParameterCount { get; }

  public abstract bool CanReadPitRows(DbContext dbContext, DataVaultPitAsOfReadRequest request);

  public abstract bool CanReadBridgeRows(DbContext dbContext, DataVaultBridgeReadRequest request);

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
        .ThenBy(row => row.DrivingKeyValueSignature, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .Select(row => DataVaultPitReadPipeline.CreatePitReadRecord(projection, row, satelliteRowsByOrdinal))
        .ToArray();
  }

  public async Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeRowsAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultBridgeReadPipeline.CreateBridgeProjection(context.DbContext, context.Request);
    var rows = await ReadBridgeRowsAsync(
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
    var rows = await ReadBridgeRowsAsync(
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

  protected abstract string CreateParameterName(int index);

  protected virtual string CreateParameterPlaceholder(int index) {
    return CreateParameterName(index);
  }

  protected abstract string QuoteIdentifier(string identifier);

  protected virtual string QuoteTableIdentifier(DbContext dbContext, string tableName) {
    ArgumentNullException.ThrowIfNull(dbContext);

    return QuoteIdentifier(tableName);
  }

  private async Task<IReadOnlyDictionary<DataVaultPitReadPipeline.PitRowIdentityKey, DataVaultPitReadPipeline.MatchedPitRow>> ReadMatchedPitRowsAsync(
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

  private async Task<IReadOnlyList<Dictionary<string, object>>> ExecutePitRowsBatchAsync(
      DataVaultProviderPitReadStrategyContext context,
      DataVaultPitReadPipeline.PitReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      IReadOnlyList<string> parentHashKeyBatch,
      DbConnection connection,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    command.CommandText = CreatePitRowsCommandText(
        context.DbContext,
        projection,
        selectedColumns,
        parentHashKeyBatch.Count);

    for (var index = 0; index < parentHashKeyBatch.Count; index++) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreateParameterName(index);
      parameter.Value = parentHashKeyBatch[index];
      command.Parameters.Add(parameter);
    }

    return await ReadCommandRowsAsync(command, selectedColumns, cancellationToken).ConfigureAwait(false);
  }

  private async Task<IReadOnlyList<Dictionary<string, object>>> ReadBridgeRowsAsync(
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

  private async Task<IReadOnlyList<Dictionary<string, object>>> ExecuteBridgeRowsBatchAsync(
      DataVaultProviderBridgeReadStrategyContext context,
      DataVaultBridgeReadPipeline.BridgeReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      IReadOnlyList<string> endpointHashKeyBatch,
      DbConnection connection,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    command.CommandText = CreateBridgeRowsCommandText(
        context.DbContext,
        projection,
        selectedColumns,
        endpointHashKeyBatch.Count,
        context.Request.MaximumDepth.HasValue);

    var parameterIndex = 0;
    foreach (var endpointHashKey in endpointHashKeyBatch) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreateParameterName(parameterIndex);
      parameter.Value = endpointHashKey;
      command.Parameters.Add(parameter);
      parameterIndex++;
    }

    if (context.Request.MaximumDepth.HasValue) {
      var maximumDepthParameter = command.CreateParameter();
      maximumDepthParameter.ParameterName = CreateParameterName(parameterIndex);
      maximumDepthParameter.Value = context.Request.MaximumDepth.Value;
      command.Parameters.Add(maximumDepthParameter);
    }

    return await ReadCommandRowsAsync(
        command,
        selectedColumns,
        cancellationToken,
        (columnName, value) => string.Equals(columnName, projection.TraversalDepthColumnName, StringComparison.Ordinal)
            ? NormalizeTraversalDepth(value)
            : value).ConfigureAwait(false);
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

  private string CreatePitRowsCommandText(
      DbContext dbContext,
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
        .Append(QuoteTableIdentifier(dbContext, projection.TableName))
        .Append(" WHERE ")
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateParameterPlaceholder(index));
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

      builder.Append(QuoteIdentifier(orderColumns[index]));
    }

    return builder.ToString();
  }

  private string CreateBridgeRowsCommandText(
      DbContext dbContext,
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
        .Append(QuoteTableIdentifier(dbContext, projection.TableName))
        .Append(" WHERE ")
        .Append(QuoteIdentifier(projection.FilterColumnName))
        .Append(" IN (");

    for (var index = 0; index < endpointHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateParameterPlaceholder(index));
    }

    builder.Append(')');
    if (hasMaximumDepth && projection.TraversalDepthColumnName is not null) {
      builder.Append(" AND ")
          .Append(QuoteIdentifier(projection.TraversalDepthColumnName))
          .Append(" <= ")
          .Append(CreateParameterPlaceholder(endpointHashKeyCount));
    }

    builder.Append(" ORDER BY ");
    for (var index = 0; index < projection.Endpoints.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteIdentifier(projection.Endpoints[index].ColumnName));
    }

    if (projection.TraversalDepthColumnName is not null) {
      builder.Append(", ")
          .Append(QuoteIdentifier(projection.TraversalDepthColumnName));
    }

    return builder.ToString();
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

  private int GetPitParentHashKeyBatchSize() {
    return Math.Min(
        DataVaultPitReadPipeline.ParentHashKeyBatchSize,
        MaxCommandParameterCount);
  }

  private int GetBridgeEndpointHashKeyBatchSize(DataVaultBridgeReadRequest request) {
    var maximumDepthParameterCount = request.MaximumDepth.HasValue ? 1 : 0;
    return Math.Min(
        500,
        MaxCommandParameterCount - maximumDepthParameterCount);
  }

  private void AppendColumnList(StringBuilder builder, IReadOnlyList<string> columns) {
    for (var index = 0; index < columns.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteIdentifier(columns[index]));
    }
  }

  protected static string CreateAtParameterName(int index) {
    return "@p" + index.ToString(CultureInfo.InvariantCulture);
  }

  protected static string CreateBareParameterName(int index) {
    return "p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static object NormalizeTraversalDepth(object value) {
    return value switch {
      byte typedValue => (int)typedValue,
      short typedValue => (int)typedValue,
      long typedValue when typedValue >= int.MinValue && typedValue <= int.MaxValue => (int)typedValue,
      decimal typedValue when typedValue >= int.MinValue && typedValue <= int.MaxValue => (int)typedValue,
      _ => value,
    };
  }
}
