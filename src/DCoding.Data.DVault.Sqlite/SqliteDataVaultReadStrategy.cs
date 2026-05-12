using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class SqliteDataVaultReadStrategy : IDataVaultProviderReadStrategy {
  private const int SqliteMaxCommandParameterCount = 900;

  public int Priority => 100;

  public bool CanReadLatestSatelliteRows(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
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
      parameter.Value = parentHashKey;
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
        row[selectedColumns[columnIndex]] = await reader.IsDBNullAsync(columnIndex, cancellationToken).ConfigureAwait(false)
            ? null!
            : reader.GetValue(columnIndex);
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

  private static int GetParentHashKeyBatchSize(DataVaultLatestSatelliteReadRequest request) {
    var asOfParameterCount = request.AsOf is null ? 0 : 1;
    return Math.Min(
        DataVaultSatelliteReadPipeline.ParentHashKeyBatchSize,
        SqliteMaxCommandParameterCount - asOfParameterCount);
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
