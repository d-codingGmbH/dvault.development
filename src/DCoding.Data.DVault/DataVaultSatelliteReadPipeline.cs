using System.Collections.ObjectModel;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultSatelliteReadPipeline {
  internal const int ParentHashKeyBatchSize = 500;
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public static Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestReadRecordsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    return ReadLatestReadRecordsCoreAsync(dbContext, request, cancellationToken);
  }

  private static async Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestReadRecordsCoreAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    if (request.ParentHashKeys.Count == 0) {
      return [];
    }

    var projection = CreateSatelliteProjection(request.Satellite);
    var latestRows = new Dictionary<SatelliteReadSeriesKey, SatelliteReadRecordCandidate>();
    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);

    foreach (var parentHashKeyBatch in request.ParentHashKeys.Chunk(ParentHashKeyBatchSize)) {
      var persistedRows = await rows
          .AsNoTracking()
          .WhereStringPropertyEqualsAny(projection.ParentHashKeyColumnName, parentHashKeyBatch)
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);

      foreach (var row in persistedRows) {
        if (!TryCreateReadRecordCandidate(projection, row, out var candidate) ||
            (request.AsOf is not null && candidate.LoadTimestamp > request.AsOf.Value)) {
          continue;
        }

        var key = new SatelliteReadSeriesKey(candidate.ParentHashKey, candidate.DrivingKeyValueSignature);
        if (!latestRows.TryGetValue(key, out var current) ||
            candidate.LoadTimestamp >= current.LoadTimestamp) {
          latestRows[key] = candidate;
        }
      }
    }

    var records = new List<DataVaultSatelliteReadRecord>(latestRows.Count);
    foreach (var candidate in latestRows.Values
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.DrivingKeyValueSignature, StringComparer.Ordinal)) {
      var record = TryCreateReadRecord(projection, candidate.Row);
      if (record is not null) {
        records.Add(record);
      }
    }

    return records;
  }

  public static async Task<IReadOnlyList<DataVaultSatelliteProjectionRow>> ReadLatestProjectionRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken) {
    var rows = await ReadLatestRowsAsync(
        dbContext,
        request,
        CreateProjectionReadRow,
        row => row.ParentHashKey,
        row => row.DrivingKeyValues,
        row => row.LoadTimestamp,
        cancellationToken).ConfigureAwait(false);

    return rows
        .Select(row => row.ProjectionRow)
        .ToArray();
  }

  private static async Task<IReadOnlyList<TReadRow>> ReadLatestRowsAsync<TReadRow>(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      Func<SatelliteReadProjection, Dictionary<string, object>, TReadRow?> createRow,
      Func<TReadRow, string> getParentHashKey,
      Func<TReadRow, IEnumerable<string>> getDrivingKeyValues,
      Func<TReadRow, DateTimeOffset> getLoadTimestamp,
      CancellationToken cancellationToken)
      where TReadRow : class {
    if (request.ParentHashKeys.Count == 0) {
      return [];
    }

    var projection = CreateSatelliteProjection(request.Satellite);
    var latestRows = new Dictionary<SatelliteReadSeriesKey, TReadRow>();
    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);

    foreach (var parentHashKeyBatch in request.ParentHashKeys.Chunk(ParentHashKeyBatchSize)) {
      var persistedRows = await rows
          .AsNoTracking()
          .WhereStringPropertyEqualsAny(projection.ParentHashKeyColumnName, parentHashKeyBatch)
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);

      foreach (var row in persistedRows) {
        var readRow = createRow(projection, row);
        if (readRow is null ||
            (request.AsOf is not null && getLoadTimestamp(readRow) > request.AsOf.Value)) {
          continue;
        }

        var key = new SatelliteReadSeriesKey(
            getParentHashKey(readRow),
            CreateOrdinalSignature(getDrivingKeyValues(readRow)));
        if (!latestRows.TryGetValue(key, out var current) ||
            getLoadTimestamp(readRow) >= getLoadTimestamp(current)) {
          latestRows[key] = readRow;
        }
      }
    }

    return latestRows.Values
        .OrderBy(getParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => CreateOrdinalSignature(getDrivingKeyValues(row)), StringComparer.Ordinal)
        .ToArray();
  }

  internal static SatelliteReadProjection CreateSatelliteProjection(DataVaultSatelliteMetadata satellite) {
    var tableName = NamingPolicy.GetSatelliteTableName(
        new DataVaultSatelliteNameContext(satellite.Parent.Name, satellite.Name));
    var parentHashKeyColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, satellite.Parent.Name, tableName));
    var hashDiffColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashDiff, satellite.Name, tableName));
    var loadTimestampColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, satellite.Name, tableName));
    var recordSourceColumnName = NamingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, satellite.Name, tableName));
    var drivingKeyColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.DrivingKeyNames,
        [parentHashKeyColumnName, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);
    var payloadColumnNames = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.PayloadColumns.Select(column => column.ColumnName),
        [parentHashKeyColumnName, .. drivingKeyColumnNames, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);

    return new SatelliteReadProjection(
        satellite.Name,
        tableName,
        parentHashKeyColumnName,
        hashDiffColumnName,
        loadTimestampColumnName,
        recordSourceColumnName,
        satellite.DrivingKeyNames,
        drivingKeyColumnNames,
        satellite.PayloadColumns.Select(column => column.ColumnName).ToArray(),
        payloadColumnNames);
  }

  internal static DataVaultSatelliteReadRecord? TryCreateReadRecord(
      SatelliteReadProjection projection,
      Dictionary<string, object> row) {
    if (!TryReadString(row, projection.ParentHashKeyColumnName, out var parentHashKey) ||
        !TryReadString(row, projection.HashDiffColumnName, out var hashDiff) ||
        !TryReadString(row, projection.RecordSourceColumnName, out var recordSource) ||
        !row.TryGetValue(projection.LoadTimestampColumnName, out var loadTimestampValue) ||
        !DataVaultLoadTimestampValueConverter.TryReadProviderValue(loadTimestampValue, out var loadTimestamp) ||
        !TryReadNamedValues(row, projection.DrivingKeyNames, projection.DrivingKeyColumnNames, out var drivingKeyValues) ||
        !TryReadNamedValues(row, projection.PayloadNames, projection.PayloadColumnNames, out var payloadValues)) {
      return null;
    }

    return new DataVaultSatelliteReadRecord(
        projection.MetadataName,
        projection.TableName,
        parentHashKey,
        drivingKeyValues,
        hashDiff,
        loadTimestamp,
        recordSource,
        payloadValues);
  }

  internal static DataVaultSatelliteProjectionRow CreateProjectionRow(
      SatelliteReadProjection projection,
      Dictionary<string, object> row) {
    return CreateProjectionReadRow(projection, row).ProjectionRow;
  }

  private static SatelliteProjectionReadRow CreateProjectionReadRow(
      SatelliteReadProjection projection,
      Dictionary<string, object> row) {
    var projectionRow = new DataVaultSatelliteProjectionRow(
        projection.MetadataName,
        new ReadOnlyDictionary<string, DataVaultSatelliteProjectionValue>(CreateProjectionValueMap(projection, row)));
    var parentHashKey = projectionRow.RequiredString(DataVaultSatelliteProjectionRow.ParentHashKeyName);
    var drivingKeyValues = projection.DrivingKeyNames
        .Select(projectionRow.RequiredString)
        .ToArray();
    var loadTimestamp = projectionRow.RequiredDateTimeOffset(DataVaultSatelliteProjectionRow.LoadTimestampName);

    return new SatelliteProjectionReadRow(
        parentHashKey,
        drivingKeyValues,
        loadTimestamp,
        projectionRow);
  }

  private static Dictionary<string, DataVaultSatelliteProjectionValue> CreateProjectionValueMap(
      SatelliteReadProjection projection,
      Dictionary<string, object> row) {
    var values = new Dictionary<string, DataVaultSatelliteProjectionValue>(StringComparer.Ordinal);

    AddProjectionValue(values, DataVaultSatelliteProjectionRow.ParentHashKeyName, projection.ParentHashKeyColumnName, row);
    AddProjectionValue(values, DataVaultSatelliteProjectionRow.HashDiffName, projection.HashDiffColumnName, row);
    AddLoadTimestampProjectionValue(values, projection.LoadTimestampColumnName, row);
    AddProjectionValue(values, DataVaultSatelliteProjectionRow.RecordSourceName, projection.RecordSourceColumnName, row);

    for (var index = 0; index < projection.DrivingKeyNames.Count; index++) {
      AddProjectionValue(values, projection.DrivingKeyNames[index], projection.DrivingKeyColumnNames[index], row);
    }

    for (var index = 0; index < projection.PayloadNames.Count; index++) {
      AddProjectionValue(values, projection.PayloadNames[index], projection.PayloadColumnNames[index], row);
    }

    return values;
  }

  private static void AddProjectionValue(
      Dictionary<string, DataVaultSatelliteProjectionValue> values,
      string mappedName,
      string columnName,
      Dictionary<string, object> row) {
    values[mappedName] = row.TryGetValue(columnName, out var value)
        ? DataVaultSatelliteProjectionValue.Present(value)
        : DataVaultSatelliteProjectionValue.Missing();
  }

  private static void AddLoadTimestampProjectionValue(
      Dictionary<string, DataVaultSatelliteProjectionValue> values,
      string columnName,
      Dictionary<string, object> row) {
    if (!row.TryGetValue(columnName, out var value)) {
      values[DataVaultSatelliteProjectionRow.LoadTimestampName] = DataVaultSatelliteProjectionValue.Missing();
      return;
    }

    if (value is null) {
      values[DataVaultSatelliteProjectionRow.LoadTimestampName] = DataVaultSatelliteProjectionValue.Present(null);
      return;
    }

    values[DataVaultSatelliteProjectionRow.LoadTimestampName] =
        DataVaultLoadTimestampValueConverter.TryReadProviderValue(value, out var loadTimestamp)
            ? DataVaultSatelliteProjectionValue.Present(loadTimestamp)
            : DataVaultSatelliteProjectionValue.Present(value);
  }

  private static bool TryReadNamedValues(
      Dictionary<string, object> row,
      IReadOnlyList<string> metadataNames,
      IReadOnlyList<string> columnNames,
      out IReadOnlyDictionary<string, string> values) {
    var valueMap = new Dictionary<string, string>(StringComparer.Ordinal);
    for (var index = 0; index < columnNames.Count; index++) {
      if (!TryReadString(row, columnNames[index], out var value)) {
        values = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(StringComparer.Ordinal));
        return false;
      }

      valueMap.Add(metadataNames[index], value);
    }

    values = new ReadOnlyDictionary<string, string>(valueMap);
    return true;
  }

  private static bool TryCreateReadRecordCandidate(
      SatelliteReadProjection projection,
      Dictionary<string, object> row,
      out SatelliteReadRecordCandidate candidate) {
    if (!TryReadString(row, projection.ParentHashKeyColumnName, out var parentHashKey) ||
        !TryReadString(row, projection.HashDiffColumnName, out _) ||
        !TryReadString(row, projection.RecordSourceColumnName, out _) ||
        !row.TryGetValue(projection.LoadTimestampColumnName, out var loadTimestampValue) ||
        !DataVaultLoadTimestampValueConverter.TryReadProviderValue(loadTimestampValue, out var loadTimestamp) ||
        !TryReadStringValues(row, projection.DrivingKeyColumnNames, out var drivingKeyValues) ||
        !HasStringValues(row, projection.PayloadColumnNames)) {
      candidate = default;
      return false;
    }

    candidate = new SatelliteReadRecordCandidate(
        row,
        parentHashKey,
        CreateOrdinalSignature(drivingKeyValues),
        loadTimestamp);
    return true;
  }

  private static bool TryReadStringValues(
      Dictionary<string, object> row,
      IReadOnlyList<string> columnNames,
      out IReadOnlyList<string> values) {
    if (columnNames.Count == 0) {
      values = [];
      return true;
    }

    var currentValues = new string[columnNames.Count];
    for (var index = 0; index < columnNames.Count; index++) {
      if (!TryReadString(row, columnNames[index], out var value)) {
        values = [];
        return false;
      }

      currentValues[index] = value;
    }

    values = currentValues;
    return true;
  }

  private static bool HasStringValues(
      Dictionary<string, object> row,
      IReadOnlyList<string> columnNames) {
    foreach (var columnName in columnNames) {
      if (!TryReadString(row, columnName, out _)) {
        return false;
      }
    }

    return true;
  }

  private static bool TryReadString(
      Dictionary<string, object> row,
      string columnName,
      out string value) {
    if (row.TryGetValue(columnName, out var currentValue) &&
        currentValue is string text) {
      value = text;
      return true;
    }

    value = string.Empty;
    return false;
  }

  private static string CreateOrdinalSignature(IEnumerable<string> values) {
    return string.Join('\u001f', values);
  }

  internal sealed record SatelliteReadProjection(
      string MetadataName,
      string TableName,
      string ParentHashKeyColumnName,
      string HashDiffColumnName,
      string LoadTimestampColumnName,
      string RecordSourceColumnName,
      IReadOnlyList<string> DrivingKeyNames,
      IReadOnlyList<string> DrivingKeyColumnNames,
      IReadOnlyList<string> PayloadNames,
      IReadOnlyList<string> PayloadColumnNames);

  private sealed record SatelliteProjectionReadRow(
      string ParentHashKey,
      IReadOnlyList<string> DrivingKeyValues,
      DateTimeOffset LoadTimestamp,
      DataVaultSatelliteProjectionRow ProjectionRow);

  private readonly record struct SatelliteReadRecordCandidate(
      Dictionary<string, object> Row,
      string ParentHashKey,
      string DrivingKeyValueSignature,
      DateTimeOffset LoadTimestamp);

  private readonly record struct SatelliteReadSeriesKey(
      string ParentHashKey,
      string DrivingKeyValueSignature);
}
