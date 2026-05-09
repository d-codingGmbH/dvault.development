using System.Collections.ObjectModel;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultReadService : IDataVaultReadService {
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public async Task<IReadOnlyList<DataVaultSatelliteReadRecord>> ReadLatestSatelliteRowsAsync(
      DbContext dbContext,
      DataVaultLatestSatelliteReadRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);

    if (request.ParentHashKeys.Count == 0) {
      return [];
    }

    var projection = CreateSatelliteProjection(request.Satellite);
    var latestRows = new Dictionary<SatelliteReadSeriesKey, DataVaultSatelliteReadRecord>();
    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);

    foreach (var parentHashKeyBatch in request.ParentHashKeys.Chunk(500)) {
      var persistedRows = await rows
          .AsNoTracking()
          .Where(row => parentHashKeyBatch.Contains(EF.Property<string>(row, projection.ParentHashKeyColumnName)))
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);

      foreach (var row in persistedRows) {
        if (!TryCreateReadRecord(projection, row, out var record) ||
            (request.AsOf is not null && record.LoadTimestamp > request.AsOf.Value)) {
          continue;
        }

        var key = new SatelliteReadSeriesKey(record.ParentHashKey, record.DrivingKeyValues.Values);
        if (!latestRows.TryGetValue(key, out var current) ||
            record.LoadTimestamp >= current.LoadTimestamp) {
          latestRows[key] = record;
        }
      }
    }

    return latestRows.Values
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => CreateOrdinalSignature(row.DrivingKeyValues.Values), StringComparer.Ordinal)
        .ToArray();
  }

  private static SatelliteReadProjection CreateSatelliteProjection(DataVaultSatelliteMetadata satellite) {
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

  private static bool TryCreateReadRecord(
      SatelliteReadProjection projection,
      Dictionary<string, object> row,
      out DataVaultSatelliteReadRecord record) {
    if (!TryReadString(row, projection.ParentHashKeyColumnName, out var parentHashKey) ||
        !TryReadString(row, projection.HashDiffColumnName, out var hashDiff) ||
        !TryReadString(row, projection.RecordSourceColumnName, out var recordSource) ||
        !row.TryGetValue(projection.LoadTimestampColumnName, out var loadTimestampValue) ||
        !DataVaultLoadTimestampValueConverter.TryReadProviderValue(loadTimestampValue, out var loadTimestamp) ||
        !TryReadNamedValues(row, projection.DrivingKeyNames, projection.DrivingKeyColumnNames, out var drivingKeyValues) ||
        !TryReadNamedValues(row, projection.PayloadNames, projection.PayloadColumnNames, out var payloadValues)) {
      record = new DataVaultSatelliteReadRecord(
          string.Empty,
          string.Empty,
          string.Empty,
          new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(StringComparer.Ordinal)),
          string.Empty,
          DateTimeOffset.MinValue,
          string.Empty,
          new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(StringComparer.Ordinal)));
      return false;
    }

    record = new DataVaultSatelliteReadRecord(
        projection.MetadataName,
        projection.TableName,
        parentHashKey,
        drivingKeyValues,
        hashDiff,
        loadTimestamp,
        recordSource,
        payloadValues);
    return true;
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

  private sealed record SatelliteReadProjection(
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

  private sealed class SatelliteReadSeriesKey : IEquatable<SatelliteReadSeriesKey> {
    private readonly string _drivingKeyValueSignature;

    public SatelliteReadSeriesKey(
        string parentHashKey,
        IEnumerable<string> drivingKeyValues) {
      ParentHashKey = parentHashKey;
      DrivingKeyValues = drivingKeyValues.ToArray();
      _drivingKeyValueSignature = CreateOrdinalSignature(DrivingKeyValues);
    }

    public string ParentHashKey { get; }

    public IReadOnlyList<string> DrivingKeyValues { get; }

    public bool Equals(SatelliteReadSeriesKey? other) {
      return other is not null &&
          string.Equals(ParentHashKey, other.ParentHashKey, StringComparison.Ordinal) &&
          string.Equals(_drivingKeyValueSignature, other._drivingKeyValueSignature, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) {
      return Equals(obj as SatelliteReadSeriesKey);
    }

    public override int GetHashCode() {
      return HashCode.Combine(
          StringComparer.Ordinal.GetHashCode(ParentHashKey),
          StringComparer.Ordinal.GetHashCode(_drivingKeyValueSignature));
    }
  }
}
