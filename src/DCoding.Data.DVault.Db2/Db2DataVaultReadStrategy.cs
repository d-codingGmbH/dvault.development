using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class Db2DataVaultReadStrategy :
    DataVaultRelationalPitBridgeReadStrategy,
    IDataVaultProviderReadStrategy {
  private const int Db2MaxCommandParameterCount = 30000;
  private const string RowNumberColumnName = "__dvault_row_number";

  protected override int MaxCommandParameterCount => Db2MaxCommandParameterCount;

  public override bool CanReadLatestSatelliteRows(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(dbContext, request).CanRead;
  }

  public override bool CanReadPitRows(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(dbContext, request).CanRead;
  }

  public override bool CanReadBridgeRows(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    return DataVaultProviderReadStrategyGateEvaluator.EvaluateDb2(dbContext, request).CanRead;
  }

  public override async Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
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

  public override async Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestSatelliteProjectionRowsAsync(
      DataVaultProviderReadStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    var projection = DataVaultSatelliteReadPipeline.CreateSatelliteProjection(context.Request.Satellite);
    var rows = await ReadLatestRowsAsync(context, projection, cancellationToken).ConfigureAwait(false);

    return rows
        .Select(row => DataVaultSatelliteReadPipeline.CreateProjectionRow(projection, row))
        .ToArray();
  }

  protected override string CreateParameterName(int index) {
    return CreateAtParameterName(index);
  }

  protected override string QuoteIdentifier(string identifier) {
    var normalizedIdentifier = identifier.ToUpperInvariant();

    return "\"" + normalizedIdentifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  protected override string QuoteTableIdentifier(DbContext dbContext, string tableName) {
    ArgumentNullException.ThrowIfNull(dbContext);

    var entityType = FindEntityType(dbContext, tableName);
    var physicalTableName = entityType?.GetTableName() ?? tableName;
    var schemaName = entityType?.GetSchema();

    return string.IsNullOrWhiteSpace(schemaName)
        ? QuoteIdentifier(physicalTableName)
        : QuoteIdentifier(schemaName!) + "." + QuoteIdentifier(physicalTableName);
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

  private async Task<IReadOnlyList<Dictionary<string, object>>> ReadLatestRowsAsync(
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

  private async Task<IReadOnlyList<Dictionary<string, object>>> ExecuteLatestRowsBatchAsync(
      DataVaultProviderReadStrategyContext context,
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      IReadOnlyList<string> parentHashKeyBatch,
      DbConnection connection,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = context.DbContext.Database.CurrentTransaction?.GetDbTransaction();
    command.CommandText = CreateLatestRowsCommandText(
        context.DbContext,
        projection,
        selectedColumns,
        parentHashKeyBatch.Count,
        context.Request.AsOf is not null);

    var parameterIndex = 0;
    foreach (var parentHashKey in parentHashKeyBatch) {
      var parameter = command.CreateParameter();
      parameter.ParameterName = CreateParameterName(parameterIndex);
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
      asOfParameter.ParameterName = CreateParameterName(parameterIndex);
      asOfParameter.Value = DataVaultLoadTimestampValueConverter.ToProviderValue(
          context.DbContext,
          projection.TableName,
          projection.LoadTimestampColumnName,
          context.Request.AsOf.Value);
      command.Parameters.Add(asOfParameter);
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

  internal override string CreateLatestRowsCommandText(
      DbContext dbContext,
      DataVaultSatelliteReadPipeline.SatelliteReadProjection projection,
      IReadOnlyList<string> selectedColumns,
      int parentHashKeyCount,
      bool hasAsOf) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(projection);
    ArgumentNullException.ThrowIfNull(selectedColumns);
    if (parentHashKeyCount <= 0) {
      throw new ArgumentOutOfRangeException(nameof(parentHashKeyCount));
    }

    var builder = new StringBuilder();
    builder.Append("SELECT ");
    AppendColumnList(builder, selectedColumns);
    builder.Append(" FROM (SELECT ");
    AppendQualifiedColumnList(builder, "target", selectedColumns);
    builder.Append(", ROW_NUMBER() OVER (PARTITION BY ")
        .Append(QuoteIdentifier("target"))
        .Append('.')
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" ORDER BY ")
        .Append(QuoteIdentifier("target"))
        .Append('.')
        .Append(QuoteIdentifier(projection.LoadTimestampColumnName))
        .Append(" DESC) AS ")
        .Append(QuoteIdentifier(RowNumberColumnName))
        .Append(" FROM ")
        .Append(QuoteTableIdentifier(dbContext, projection.TableName))
        .Append(" AS ")
        .Append(QuoteIdentifier("target"))
        .Append(" WHERE ")
        .Append(QuoteIdentifier("target"))
        .Append('.')
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName))
        .Append(" IN (");

    for (var index = 0; index < parentHashKeyCount; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateParameterName(index));
    }

    builder.Append(')');
    if (hasAsOf) {
      builder.Append(" AND ")
          .Append(QuoteIdentifier("target"))
          .Append('.')
          .Append(QuoteIdentifier(projection.LoadTimestampColumnName))
          .Append(" <= ")
          .Append(CreateParameterName(parentHashKeyCount));
    }

    builder.Append(") AS ")
        .Append(QuoteIdentifier("ranked"))
        .Append(" WHERE ")
        .Append(QuoteIdentifier(RowNumberColumnName))
        .Append(" = 1 ORDER BY ")
        .Append(QuoteIdentifier(projection.ParentHashKeyColumnName));

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

  private int GetParentHashKeyBatchSize(DataVaultLatestSatelliteReadRequest request) {
    var asOfParameterCount = request.AsOf is null ? 0 : 1;
    return Math.Min(
        DataVaultSatelliteReadPipeline.ParentHashKeyBatchSize,
        Db2MaxCommandParameterCount - asOfParameterCount);
  }

  private void AppendColumnList(StringBuilder builder, IReadOnlyList<string> columns) {
    for (var index = 0; index < columns.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteIdentifier(columns[index]));
    }
  }

  private void AppendQualifiedColumnList(
      StringBuilder builder,
      string qualifier,
      IReadOnlyList<string> columns) {
    for (var index = 0; index < columns.Count; index++) {
      if (index > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteIdentifier(qualifier))
          .Append('.')
          .Append(QuoteIdentifier(columns[index]));
    }
  }
}
