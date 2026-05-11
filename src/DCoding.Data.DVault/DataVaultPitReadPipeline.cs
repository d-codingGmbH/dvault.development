using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal static class DataVaultPitReadPipeline {
  private const int ParentHashKeyBatchSize = 500;
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public static async Task<IReadOnlyList<DataVaultPitReadRecord>> ReadPitReadRecordsAsync(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken) {
    var projection = CreatePitProjection(dbContext, request);
    if (request.ParentHashKeys.Count == 0) {
      return [];
    }

    var matchedPitRows = await ReadMatchedPitRowsAsync(
        dbContext,
        projection,
        request,
        cancellationToken).ConfigureAwait(false);
    if (matchedPitRows.Count == 0) {
      return [];
    }

    var satelliteRowsByOrdinal = new Dictionary<int, IReadOnlyDictionary<SatelliteSnapshotKey, Dictionary<string, object>>>();
    for (var index = 0; index < projection.Satellites.Count; index++) {
      satelliteRowsByOrdinal[index] = await ReadSatelliteRowsAsync(
          dbContext,
          projection,
          projection.Satellites[index],
          index,
          matchedPitRows.Values,
          cancellationToken).ConfigureAwait(false);
    }

    var records = new List<DataVaultPitReadRecord>();
    foreach (var pitRow in matchedPitRows.Values.OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)) {
      var snapshots = new DataVaultPitSatelliteSnapshot[projection.Satellites.Count];
      for (var index = 0; index < projection.Satellites.Count; index++) {
        snapshots[index] = CreateSatelliteSnapshot(
            projection,
            projection.Satellites[index],
            index,
            pitRow,
            satelliteRowsByOrdinal[index]);
      }

      records.Add(new DataVaultPitReadRecord(
          pitRow.ParentHashKey,
          pitRow.LoadTimestamp,
          snapshots));
    }

    return records;
  }

  private static async Task<IReadOnlyDictionary<string, MatchedPitRow>> ReadMatchedPitRowsAsync(
      DbContext dbContext,
      PitReadProjection projection,
      DataVaultPitAsOfReadRequest request,
      CancellationToken cancellationToken) {
    var matchedRows = new Dictionary<string, MatchedPitRow>(StringComparer.Ordinal);
    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);

    foreach (var parentHashKeyBatch in request.ParentHashKeys.Chunk(ParentHashKeyBatchSize)) {
      List<Dictionary<string, object>> persistedRows;
      try {
        persistedRows = await rows
            .AsNoTracking()
            .WhereStringPropertyEqualsAny(projection.ParentHashKeyColumnName, parentHashKeyBatch)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
      }
      catch (Exception exception) when (exception is not OperationCanceledException) {
        throw PitReadFailure(
            projection.MetadataName,
            "could not query generated PIT table/entity '" + projection.TableName + "'",
            exception);
      }

      foreach (var row in persistedRows) {
        var parentHashKey = ReadRequiredString(
            projection.MetadataName,
            projection.TableName,
            row,
            projection.ParentHashKeyColumnName,
            "PIT parent hash-key");
        var loadTimestamp = ReadRequiredTimestamp(
            projection.MetadataName,
            projection.TableName,
            row,
            projection.LoadTimestampColumnName,
            "PIT load timestamp");
        if (loadTimestamp > request.AsOf) {
          continue;
        }

        var snapshotLoadTimestamps = projection.Satellites
            .Select(satellite => ReadOptionalTimestamp(
                projection.MetadataName,
                projection.TableName,
                row,
                satellite.SnapshotReferenceColumnName,
                "PIT satellite snapshot reference"))
            .ToArray();
        if (!matchedRows.TryGetValue(parentHashKey, out var current) || loadTimestamp >= current.LoadTimestamp) {
          matchedRows[parentHashKey] = new MatchedPitRow(parentHashKey, loadTimestamp, snapshotLoadTimestamps);
        }
      }
    }

    return matchedRows;
  }

  private static async Task<IReadOnlyDictionary<SatelliteSnapshotKey, Dictionary<string, object>>> ReadSatelliteRowsAsync(
      DbContext dbContext,
      PitReadProjection pitProjection,
      PitSatelliteProjection pitSatellite,
      int satelliteOrdinal,
      IEnumerable<MatchedPitRow> matchedPitRows,
      CancellationToken cancellationToken) {
    var requiredKeys = matchedPitRows
        .Where(row => row.SnapshotLoadTimestamps[satelliteOrdinal].HasValue)
        .Select(row => new SatelliteSnapshotKey(row.ParentHashKey, row.SnapshotLoadTimestamps[satelliteOrdinal]!.Value))
        .ToHashSet();
    if (requiredKeys.Count == 0) {
      return new Dictionary<SatelliteSnapshotKey, Dictionary<string, object>>();
    }

    var parentHashKeys = requiredKeys
        .Select(key => key.ParentHashKey)
        .Distinct(StringComparer.Ordinal)
        .ToArray();
    var rows = dbContext.Set<Dictionary<string, object>>(pitSatellite.Satellite.TableName);
    var satelliteRows = new Dictionary<SatelliteSnapshotKey, Dictionary<string, object>>();

    foreach (var parentHashKeyBatch in parentHashKeys.Chunk(ParentHashKeyBatchSize)) {
      List<Dictionary<string, object>> persistedRows;
      try {
        persistedRows = await rows
            .AsNoTracking()
            .WhereStringPropertyEqualsAny(pitSatellite.Satellite.ParentHashKeyColumnName, parentHashKeyBatch)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
      }
      catch (Exception exception) when (exception is not OperationCanceledException) {
        throw PitReadFailure(
            pitProjection.MetadataName,
            "could not query generated satellite table/entity '" + pitSatellite.Satellite.TableName +
            "' for PIT satellite '" + pitSatellite.MetadataName + "'",
            exception);
      }

      foreach (var row in persistedRows) {
        var parentHashKey = ReadRequiredString(
            pitProjection.MetadataName,
            pitSatellite.Satellite.TableName,
            row,
            pitSatellite.Satellite.ParentHashKeyColumnName,
            "satellite parent hash-key");
        var loadTimestamp = ReadRequiredTimestamp(
            pitProjection.MetadataName,
            pitSatellite.Satellite.TableName,
            row,
            pitSatellite.Satellite.LoadTimestampColumnName,
            "satellite load timestamp");
        var key = new SatelliteSnapshotKey(parentHashKey, loadTimestamp);
        if (!requiredKeys.Contains(key)) {
          continue;
        }

        if (satelliteRows.ContainsKey(key)) {
          throw PitReadFailure(
              pitProjection.MetadataName,
              "encountered duplicate satellite row for PIT satellite '" + pitSatellite.MetadataName +
              "' parent hash key '" + parentHashKey + "' and snapshot load timestamp '" + loadTimestamp.ToString("O") + "'");
        }

        satelliteRows.Add(key, row);
      }
    }

    return satelliteRows;
  }

  private static DataVaultPitSatelliteSnapshot CreateSatelliteSnapshot(
      PitReadProjection pitProjection,
      PitSatelliteProjection pitSatellite,
      int satelliteOrdinal,
      MatchedPitRow pitRow,
      IReadOnlyDictionary<SatelliteSnapshotKey, Dictionary<string, object>> satelliteRows) {
    var snapshotLoadTimestamp = pitRow.SnapshotLoadTimestamps[satelliteOrdinal];
    if (!snapshotLoadTimestamp.HasValue) {
      return DataVaultPitSatelliteSnapshot.Missing(pitSatellite.MetadataName, satelliteOrdinal);
    }

    if (!satelliteRows.TryGetValue(
        new SatelliteSnapshotKey(pitRow.ParentHashKey, snapshotLoadTimestamp.Value),
        out var satelliteRow)) {
      return DataVaultPitSatelliteSnapshot.Missing(pitSatellite.MetadataName, satelliteOrdinal);
    }

    var hashDiff = ReadRequiredString(
        pitProjection.MetadataName,
        pitSatellite.Satellite.TableName,
        satelliteRow,
        pitSatellite.Satellite.HashDiffColumnName,
        "satellite hash diff");
    var recordSource = ReadRequiredString(
        pitProjection.MetadataName,
        pitSatellite.Satellite.TableName,
        satelliteRow,
        pitSatellite.Satellite.RecordSourceColumnName,
        "satellite record source");
    var payloadValues = new Dictionary<string, string?>(StringComparer.Ordinal);

    foreach (var payload in pitSatellite.Satellite.Payloads) {
      payloadValues[payload.MetadataName] = ReadOptionalString(
          pitProjection.MetadataName,
          pitSatellite.Satellite.TableName,
          satelliteRow,
          payload.ColumnName,
          "satellite payload");
    }

    return new DataVaultPitSatelliteSnapshot(
        pitSatellite.MetadataName,
        satelliteOrdinal,
        isPresent: true,
        snapshotLoadTimestamp.Value,
        hashDiff,
        recordSource,
        payloadValues);
  }

  private static PitReadProjection CreatePitProjection(
      DbContext dbContext,
      DataVaultPitAsOfReadRequest request) {
    var pit = request.Pit;
    ValidatePitShape(pit);

    var tableName = GetPitTableName(pit.Parent.Name, pit.Satellites.Select(satellite => satellite.SatelliteName));
    var entityType = dbContext.Model.FindEntityType(tableName);
    if (entityType is null) {
      throw PitReadFailure(
          pit.Name,
          "expected generated PIT table/entity '" + tableName + "' in the DbContext model");
    }

    ValidateGeneratedEntity(pit.Name, entityType, tableName, DataVaultTableKind.Pit, pit.Name, pit.Parent);

    var parentHashKeyProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.HashKey,
        pit.Parent.Name,
        "parent hash-key");
    ValidateStringProperty(pit.Name, tableName, parentHashKeyProperty, "parent hash-key");

    var loadTimestampProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.LoadTimestamp,
        metadataName: null,
        "PIT load timestamp");
    ValidateTimestampProperty(pit.Name, tableName, loadTimestampProperty, "PIT load timestamp");

    var satellites = new PitSatelliteProjection[pit.Satellites.Count];
    for (var index = 0; index < pit.Satellites.Count; index++) {
      var satelliteReference = pit.Satellites[index];
      var snapshotReferenceProperty = GetRequiredGeneratedProperty(
          pit.Name,
          tableName,
          entityType,
          DataVaultPropertyRole.SnapshotReference,
          TechnicalMetadataColumnRole.LoadTimestamp,
          satelliteReference.SatelliteName,
          "satellite snapshot reference");
      ValidateTimestampProperty(pit.Name, tableName, snapshotReferenceProperty, "satellite snapshot reference");

      satellites[index] = new PitSatelliteProjection(
          satelliteReference.SatelliteName,
          snapshotReferenceProperty.Name,
          CreateSatelliteProjection(dbContext, pit, satelliteReference.SatelliteName));
    }

    return new PitReadProjection(
        pit.Name,
        tableName,
        parentHashKeyProperty.Name,
        loadTimestampProperty.Name,
        satellites);
  }

  private static void ValidatePitShape(DataVaultPitMetadata pit) {
    if (pit.Parent.Kind != DataVaultMetadataReferenceKind.Hub) {
      throw PitReadFailure(
          pit.Name,
          "declares parent '" + pit.Parent.Name + "' as " + pit.Parent.Kind +
          "; link-based PIT tables and non-hub parents are outside the supported PIT read baseline");
    }

    if (pit.Satellites.Count == 0) {
      throw PitReadFailure(pit.Name, "must declare at least one attached satellite");
    }

    var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
    foreach (var satelliteReference in pit.Satellites) {
      if (!satelliteNames.Add(satelliteReference.SatelliteName)) {
        throw PitReadFailure(
            pit.Name,
            "declares duplicate satellite reference '" + satelliteReference.SatelliteName + "'");
      }

      if (satelliteReference.IsMultiActive) {
        throw PitReadFailure(
            pit.Name,
            "references multi-active satellite '" + satelliteReference.SatelliteName +
            "', which is outside the supported PIT read baseline");
      }
    }
  }

  private static SatelliteReadProjection CreateSatelliteProjection(
      DbContext dbContext,
      DataVaultPitMetadata pit,
      string satelliteName) {
    var tableName = NamingPolicy.GetSatelliteTableName(
        new DataVaultSatelliteNameContext(pit.Parent.Name, satelliteName));
    var entityType = dbContext.Model.FindEntityType(tableName);
    if (entityType is null) {
      throw PitReadFailure(
          pit.Name,
          "expected generated satellite table/entity '" + tableName + "' for PIT satellite '" + satelliteName +
          "' in the DbContext model");
    }

    ValidateGeneratedEntity(pit.Name, entityType, tableName, DataVaultTableKind.Satellite, satelliteName, pit.Parent);

    var drivingKeyProperties = entityType.GetProperties()
        .Where(property => Equals(property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value, DataVaultPropertyRole.DrivingKey))
        .ToArray();
    if (drivingKeyProperties.Length > 0) {
      throw PitReadFailure(
          pit.Name,
          "references multi-active satellite '" + satelliteName +
          "', which is outside the supported PIT read baseline");
    }

    var parentHashKeyProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.HashKey,
        pit.Parent.Name,
        "satellite parent hash-key");
    ValidateStringProperty(pit.Name, tableName, parentHashKeyProperty, "satellite parent hash-key");

    var hashDiffProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.HashDiff,
        metadataName: null,
        "satellite hash diff");
    ValidateStringProperty(pit.Name, tableName, hashDiffProperty, "satellite hash diff");

    var loadTimestampProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.LoadTimestamp,
        metadataName: null,
        "satellite load timestamp");
    ValidateTimestampProperty(pit.Name, tableName, loadTimestampProperty, "satellite load timestamp");

    var recordSourceProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.RecordSource,
        metadataName: null,
        "satellite record source");
    ValidateStringProperty(pit.Name, tableName, recordSourceProperty, "satellite record source");

    var payloads = entityType.GetProperties()
        .Where(property => Equals(property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value, DataVaultPropertyRole.Payload))
        .OrderBy(property => property.FindAnnotation(DataVaultAnnotationNames.Ordinal)?.Value is int ordinal ? ordinal : int.MaxValue)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .Select(property => CreatePayloadProjection(pit.Name, tableName, property))
        .ToArray();
    var duplicatePayloadName = payloads
        .GroupBy(payload => payload.MetadataName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();
    if (duplicatePayloadName is not null) {
      throw PitReadFailure(
          pit.Name,
          "expected generated satellite table/entity '" + tableName +
          "' to expose distinct payload metadata names, but found duplicate '" + duplicatePayloadName + "'");
    }

    return new SatelliteReadProjection(
        tableName,
        parentHashKeyProperty.Name,
        hashDiffProperty.Name,
        loadTimestampProperty.Name,
        recordSourceProperty.Name,
        payloads);
  }

  private static PayloadProjection CreatePayloadProjection(
      string pitName,
      string tableName,
      IProperty property) {
    ValidateStringProperty(pitName, tableName, property, "satellite payload");
    var metadataName = property.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string;
    if (string.IsNullOrWhiteSpace(metadataName)) {
      throw PitReadFailure(
          pitName,
          "expected generated satellite payload property '" + property.Name +
          "' on table/entity '" + tableName + "' to carry payload metadata name");
    }

    return new PayloadProjection(metadataName, property.Name);
  }

  private static void ValidateGeneratedEntity(
      string pitName,
      IEntityType entityType,
      string tableName,
      DataVaultTableKind expectedKind,
      string expectedMetadataName,
      DataVaultMetadataReference? expectedParent) {
    var entityKind = entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value;
    if (!Equals(entityKind, expectedKind)) {
      throw PitReadFailure(
          pitName,
          "expected generated table/entity '" + tableName + "' to carry " + expectedKind + " entity kind metadata");
    }

    var metadataName = entityType.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string;
    if (!string.Equals(metadataName, expectedMetadataName, StringComparison.Ordinal)) {
      throw PitReadFailure(
          pitName,
          "expected generated table/entity '" + tableName + "' to carry metadata name '" +
          expectedMetadataName + "'");
    }

    if (expectedParent is null) {
      return;
    }

    var parentReferenceKind = entityType.FindAnnotation(DataVaultAnnotationNames.ParentReferenceKind)?.Value;
    var parentReferenceName = entityType.FindAnnotation(DataVaultAnnotationNames.ParentReferenceName)?.Value as string;
    if (!Equals(parentReferenceKind, expectedParent.Kind) ||
        !string.Equals(parentReferenceName, expectedParent.Name, StringComparison.Ordinal)) {
      throw PitReadFailure(
          pitName,
          "expected generated table/entity '" + tableName + "' to carry parent " +
          expectedParent.Kind + " reference '" + expectedParent.Name + "'");
    }
  }

  private static IProperty GetRequiredGeneratedProperty(
      string pitName,
      string tableName,
      IEntityType entityType,
      DataVaultPropertyRole expectedRole,
      TechnicalMetadataColumnRole? expectedTechnicalRole,
      string? metadataName,
      string description) {
    var matches = entityType.GetProperties()
        .Where(property => Equals(property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value, expectedRole))
        .Where(property => expectedTechnicalRole is null ||
            Equals(property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole)?.Value, expectedTechnicalRole))
        .Where(property => metadataName is null ||
            string.Equals(property.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string, metadataName, StringComparison.Ordinal))
        .ToArray();

    return matches.Length switch {
      1 => matches[0],
      0 => throw PitReadFailure(
          pitName,
          "expected generated " + description + " property on table/entity '" + tableName + "'" +
          (metadataName is null ? string.Empty : " for metadata name '" + metadataName + "'")),
      _ => throw PitReadFailure(
          pitName,
          "expected generated " + description + " property on table/entity '" + tableName +
          "' to be unambiguous" +
          (metadataName is null ? string.Empty : " for metadata name '" + metadataName + "'")),
    };
  }

  private static void ValidateStringProperty(
      string pitName,
      string tableName,
      IProperty property,
      string description) {
    if (property.ClrType == typeof(string)) {
      return;
    }

    throw PitReadFailure(
        pitName,
        "expected generated " + description + " property '" + property.Name +
        "' on table/entity '" + tableName + "' to use CLR type '" + typeof(string).FullName +
        "' but found '" + property.ClrType.FullName + "'");
  }

  private static void ValidateTimestampProperty(
      string pitName,
      string tableName,
      IProperty property,
      string description) {
    if (CanReadTimestampType(property.ClrType)) {
      return;
    }

    throw PitReadFailure(
        pitName,
        "expected generated " + description + " property '" + property.Name +
        "' on table/entity '" + tableName + "' to use a readable load timestamp CLR type but found '" +
        property.ClrType.FullName + "'");
  }

  private static bool CanReadTimestampType(Type clrType) {
    var type = Nullable.GetUnderlyingType(clrType) ?? clrType;
    return type == typeof(DateTimeOffset) ||
        type == typeof(DateTime) ||
        type == typeof(string) ||
        type == typeof(long) ||
        type == typeof(int) ||
        type == typeof(short) ||
        type == typeof(byte) ||
        type == typeof(decimal);
  }

  private static string ReadRequiredString(
      string pitName,
      string tableName,
      Dictionary<string, object> row,
      string columnName,
      string description) {
    if (row.TryGetValue(columnName, out var value) && value is string text) {
      return text;
    }

    throw PitReadFailure(
        pitName,
        "expected generated " + description + " property '" + columnName +
        "' on table/entity '" + tableName + "' to contain a non-null string value");
  }

  private static string? ReadOptionalString(
      string pitName,
      string tableName,
      Dictionary<string, object> row,
      string columnName,
      string description) {
    if (!row.TryGetValue(columnName, out var value) || value is null) {
      return null;
    }

    if (value is string text) {
      return text;
    }

    throw PitReadFailure(
        pitName,
        "expected generated " + description + " property '" + columnName +
        "' on table/entity '" + tableName + "' to contain a string or null value");
  }

  private static DateTimeOffset ReadRequiredTimestamp(
      string pitName,
      string tableName,
      Dictionary<string, object> row,
      string columnName,
      string description) {
    if (row.TryGetValue(columnName, out var value) &&
        DataVaultLoadTimestampValueConverter.TryReadProviderValue(value, out var timestamp)) {
      return timestamp;
    }

    throw PitReadFailure(
        pitName,
        "expected generated " + description + " property '" + columnName +
        "' on table/entity '" + tableName + "' to contain a non-null readable load timestamp value");
  }

  private static DateTimeOffset? ReadOptionalTimestamp(
      string pitName,
      string tableName,
      Dictionary<string, object> row,
      string columnName,
      string description) {
    if (!row.TryGetValue(columnName, out var value) || value is null) {
      return null;
    }

    if (DataVaultLoadTimestampValueConverter.TryReadProviderValue(value, out var timestamp)) {
      return timestamp;
    }

    throw PitReadFailure(
        pitName,
        "expected generated " + description + " property '" + columnName +
        "' on table/entity '" + tableName + "' to contain a readable load timestamp value or null");
  }

  private static string GetPitTableName(
      string hubName,
      IEnumerable<string> satelliteNames) {
    var namingPolicy = DefaultNamingPolicy.Instance;

    return "Pit" + namingPolicy.NormalizeProducedIdentifier(hubName) +
        string.Concat(satelliteNames.Select(namingPolicy.NormalizeProducedIdentifier));
  }

  private static InvalidOperationException PitReadFailure(string pitName, string detail) {
    return PitReadFailure(pitName, detail, innerException: null);
  }

  private static InvalidOperationException PitReadFailure(
      string pitName,
      string detail,
      Exception? innerException) {
    return new InvalidOperationException(
        "DVault PIT read failed: PIT metadata '" + pitName + "' " + detail + ".",
        innerException);
  }

  private sealed record PitReadProjection(
      string MetadataName,
      string TableName,
      string ParentHashKeyColumnName,
      string LoadTimestampColumnName,
      IReadOnlyList<PitSatelliteProjection> Satellites);

  private sealed record PitSatelliteProjection(
      string MetadataName,
      string SnapshotReferenceColumnName,
      SatelliteReadProjection Satellite);

  private sealed record SatelliteReadProjection(
      string TableName,
      string ParentHashKeyColumnName,
      string HashDiffColumnName,
      string LoadTimestampColumnName,
      string RecordSourceColumnName,
      IReadOnlyList<PayloadProjection> Payloads);

  private sealed record PayloadProjection(string MetadataName, string ColumnName);

  private sealed record MatchedPitRow(
      string ParentHashKey,
      DateTimeOffset LoadTimestamp,
      IReadOnlyList<DateTimeOffset?> SnapshotLoadTimestamps);

  private readonly record struct SatelliteSnapshotKey(string ParentHashKey, DateTimeOffset LoadTimestamp);
}
