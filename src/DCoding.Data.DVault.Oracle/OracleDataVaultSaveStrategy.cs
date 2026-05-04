using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DCoding.Data.DVault;

internal sealed class OracleDataVaultSaveStrategy : IDataVaultProviderSaveStrategy {
  private const string OracleProviderName = "Oracle.EntityFrameworkCore";
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public int Priority => 100;

  public bool CanSave(DbContext dbContext, IReadOnlyList<DataVaultSaveRequest> requests) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(requests);

    return string.Equals(dbContext.Database.ProviderName, OracleProviderName, StringComparison.Ordinal) &&
        IsCleanContext(dbContext) &&
        IsSupportedRequestBatch(requests);
  }

  public async Task<DataVaultSaveResult> SaveAsync(
      DataVaultProviderSaveStrategyContext context,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(context);

    if (!CanSave(context.DbContext, context.Requests)) {
      throw new InvalidOperationException(
          "Oracle DVault save strategy cannot persist this DbContext and request batch shape.");
    }

    var plans = CreateUniqueRowSavePlans(context);
    var rowsWritten = await ExecuteOracleInsertRowsAsync(
        context.DbContext,
        plans.Select(plan => new OracleInsertRow(
            plan.Table.TableName,
            plan.Table.HashKeyColumnName,
            plan.HashKey,
            plan.Row)),
        cancellationToken).ConfigureAwait(false);
    var savedRecords = plans
        .Select(plan => plan.SavedRecord)
        .ToArray();

    return new DataVaultSaveResult(rowsWritten, savedRecords);
  }

  private static bool IsCleanContext(DbContext dbContext) {
    return !dbContext.ChangeTracker
        .Entries()
        .Any(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
  }

  private static bool IsSupportedRequestBatch(IReadOnlyList<DataVaultSaveRequest> requests) {
    foreach (var request in requests) {
      if (request is null || request.SatelliteOperations.Count != 0) {
        return false;
      }
    }

    return true;
  }

  private static IReadOnlyList<UniqueRowSavePlan> CreateUniqueRowSavePlans(DataVaultProviderSaveStrategyContext context) {
    var plans = new List<UniqueRowSavePlan>();

    foreach (var request in context.Requests) {
      plans.AddRange(request.HubOperations.Select(operation => CreateHubSavePlan(context, request, operation)));
      plans.AddRange(request.LinkOperations.Select(operation => CreateLinkSavePlan(context, request, operation)));
    }

    return plans.ToArray();
  }

  private static UniqueRowSavePlan CreateHubSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultSaveRequest request,
      DataVaultHubSaveOperation operation) {
    var hub = operation.Metadata;
    var tableName = NamingPolicy.GetHubTableName(new DataVaultHubNameContext(hub.Name));
    var hashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, hub.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, hub.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, hub.Name, tableName));
    var businessKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        hub.BusinessKeyColumns.Select(column => column.ColumnName),
        [hashKeyColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var businessKeyFields = hub.BusinessKeyColumns
        .Select(column => new KeyValuePair<string, string>(
            column.ColumnName,
            GetRequiredValue(operation.BusinessKeyValues, column.ColumnName, nameof(operation.BusinessKeyValues))))
        .ToArray();
    var hashKey = ComputeHash(context, businessKeyFields);
    var row = new Dictionary<string, object> {
      [hashKeyColumnName] = hashKey,
      [loadTimestampColumnName] = request.LoadTimestamp,
      [recordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < businessKeyFields.Length; index++) {
      row.Add(businessKeyColumnNames[index], businessKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(tableName, hashKeyColumnName),
        hashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Hub, hub.Name, tableName, hashKey));
  }

  private static UniqueRowSavePlan CreateLinkSavePlan(
      DataVaultProviderSaveStrategyContext context,
      DataVaultSaveRequest request,
      DataVaultLinkSaveOperation operation) {
    var link = operation.Metadata;
    var participantNames = link.Participants
        .Select(participant => participant.HubReference.Name)
        .ToArray();
    var tableName = NamingPolicy.GetLinkTableName(new DataVaultLinkNameContext(link.Name, participantNames));
    var linkHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, link.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, link.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, link.Name, tableName));
    var participantHashKeyColumnNames = participantNames
        .Select(participantName => NamingPolicy.GetTechnicalColumnName(
            new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, participantName, tableName)))
        .ToArray();
    var participantHashKeyFields = participantNames
        .Select(participantName => new KeyValuePair<string, string>(
            participantName,
            GetRequiredValue(operation.ParticipantHashKeyValues, participantName, nameof(operation.ParticipantHashKeyValues))))
        .ToArray();
    var linkHashKey = ComputeHash(context, participantHashKeyFields);
    var row = new Dictionary<string, object> {
      [linkHashKeyColumnName] = linkHashKey,
      [loadTimestampColumnName] = request.LoadTimestamp,
      [recordSourceColumnName] = request.RecordSource,
    };

    for (var index = 0; index < participantHashKeyFields.Length; index++) {
      row.Add(participantHashKeyColumnNames[index], participantHashKeyFields[index].Value);
    }

    return new UniqueRowSavePlan(
        new UniqueTableProjection(tableName, linkHashKeyColumnName),
        linkHashKey,
        row,
        new DataVaultSavedRecord(DataVaultTableKind.Link, link.Name, tableName, linkHashKey));
  }

  private static async Task<int> ExecuteOracleInsertRowsAsync(
      DbContext dbContext,
      IEnumerable<OracleInsertRow> rows,
      CancellationToken cancellationToken) {
    var rowArray = rows.ToArray();
    if (rowArray.Length == 0) {
      return 0;
    }

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
      var rowsWritten = 0;
      foreach (var row in rowArray) {
        rowsWritten += await ExecuteOracleInsertRowAsync(
            connection,
            transaction,
            row,
            cancellationToken).ConfigureAwait(false);
      }

      if (localTransaction is not null) {
        await localTransaction.CommitAsync(cancellationToken).ConfigureAwait(false);
      }

      return rowsWritten;
    }
    catch {
      if (localTransaction is not null) {
        await localTransaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
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

  private static async Task<int> ExecuteOracleInsertRowAsync(
      DbConnection connection,
      DbTransaction transaction,
      OracleInsertRow row,
      CancellationToken cancellationToken) {
    await using var command = connection.CreateCommand();
    command.Transaction = transaction;

    var columns = row.Values.Keys.ToArray();
    command.CommandText = CreateOracleInsertCommandText(
        row.TableName,
        columns,
        row.HashKeyColumnName,
        columns.Length);

    for (var index = 0; index < columns.Length; index++) {
      AddParameter(command, index, row.Values[columns[index]]);
    }

    AddParameter(command, columns.Length, row.HashKey);

    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
  }

  private static string CreateOracleInsertCommandText(
      string tableName,
      IReadOnlyList<string> columns,
      string hashKeyColumnName,
      int hashKeyParameterIndex) {
    var builder = new StringBuilder();
    builder.Append("INSERT INTO ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" (");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(QuoteOracleIdentifier(columns[columnIndex]));
    }

    builder.Append(") SELECT ");

    for (var columnIndex = 0; columnIndex < columns.Count; columnIndex++) {
      if (columnIndex > 0) {
        builder.Append(", ");
      }

      builder.Append(CreateOracleParameterPlaceholder(columnIndex));
    }

    builder.Append(" FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM ")
        .Append(QuoteOracleIdentifier(tableName))
        .Append(" WHERE ")
        .Append(QuoteOracleIdentifier(hashKeyColumnName))
        .Append(" = ")
        .Append(CreateOracleParameterPlaceholder(hashKeyParameterIndex))
        .Append(')');

    return builder.ToString();
  }

  private static void AddParameter(DbCommand command, int index, object value) {
    var parameter = command.CreateParameter();
    parameter.ParameterName = CreateOracleParameterName(index);
    parameter.Value = value;
    command.Parameters.Add(parameter);
  }

  private static string CreateOracleParameterName(int index) {
    return "p" + index.ToString(CultureInfo.InvariantCulture);
  }

  private static string CreateOracleParameterPlaceholder(int index) {
    return ":" + CreateOracleParameterName(index);
  }

  private static string QuoteOracleIdentifier(string identifier) {
    return "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
  }

  private static string ComputeHash(
      DataVaultProviderSaveStrategyContext context,
      IEnumerable<KeyValuePair<string, string>> fields) {
    var normalizedFields = context.StableHashNormalizer.NormalizeFields(
        fields.Select(field => new KeyValuePair<string, object?>(field.Key, field.Value)));

    return context.StableHashService.ComputeHash(normalizedFields).Value;
  }

  private static string GetRequiredValue(
      IReadOnlyDictionary<string, string> values,
      string name,
      string parameterName) {
    if (values.TryGetValue(name, out var value)) {
      return value;
    }

    throw new ArgumentException("The Data Vault save operation is missing required value '" + name + "'.", parameterName);
  }

  private sealed record UniqueTableProjection(string TableName, string HashKeyColumnName);

  private sealed record UniqueRowSavePlan(
      UniqueTableProjection Table,
      string HashKey,
      Dictionary<string, object> Row,
      DataVaultSavedRecord SavedRecord);

  private sealed record OracleInsertRow(
      string TableName,
      string HashKeyColumnName,
      string HashKey,
      Dictionary<string, object> Values);
}
