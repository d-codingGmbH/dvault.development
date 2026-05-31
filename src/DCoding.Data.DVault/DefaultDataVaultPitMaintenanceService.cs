using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Globalization;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultPitMaintenanceService : IDataVaultPitMaintenanceService {
  private const int ParentHashKeyBatchSize = 500;
  private static readonly IDataVaultNamingPolicy NamingPolicy = DefaultDataVaultNamingPolicy.Instance;

  public DefaultDataVaultPitMaintenanceService() {
  }

  public async Task<DataVaultPitMaintenanceResult> RebuildAsync(
      DbContext dbContext,
      DataVaultPitRebuildRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    using var activity = DataVaultActivityTracing.StartMaintenanceActivity(
        dbContext,
        DataVaultActivityTracing.PitRebuildOperation,
        DataVaultActivityTracing.PitRebuildMaintenanceKind,
        DataVaultActivityTracing.PitReadModelKind,
        DataVaultActivityTracing.FullRebuildScope);

    try {
      var projection = CreatePitProjection(dbContext, request.Pit);
      var satelliteRows = await ReadSatelliteRowsAsync(
          dbContext,
          projection,
          parentHashKeys: null,
          cancellationToken).ConfigureAwait(false);
      var parentHashKeys = satelliteRows
          .SelectMany(rows => rows.Select(row => row.ParentHashKey))
          .Distinct(StringComparer.Ordinal)
          .Order(StringComparer.Ordinal)
          .ToArray();
      var rowsToWrite = CreatePitRows(projection, parentHashKeys, satelliteRows);
      var rowsDeleted = await DeleteAllPitRowsAsync(dbContext, projection, cancellationToken).ConfigureAwait(false);
      DetachTrackedPitRows(dbContext, projection, parentHashKeys: null);
      var rowsWritten = await AddPitRowsAsync(dbContext, projection, rowsToWrite, cancellationToken).ConfigureAwait(false);
      var result = new DataVaultPitMaintenanceResult(
          request.Pit,
          projection.TableName,
          parentHashKeys.Length,
          rowsDeleted,
          rowsWritten);

      activity.RecordSuccess(
          result.RowsDeleted + result.RowsWritten,
          parentKeyCount: null,
          isNoOp: false);

      return result;
    }
    catch (Exception exception) {
      activity.RecordFailure(exception);
      throw;
    }
  }

  public async Task<DataVaultPitMaintenanceResult> MaintainParentsAsync(
      DbContext dbContext,
      DataVaultPitParentMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    using var activity = DataVaultActivityTracing.StartMaintenanceActivity(
        dbContext,
        DataVaultActivityTracing.PitMaintainParentsOperation,
        DataVaultActivityTracing.PitMaintainParentsMaintenanceKind,
        DataVaultActivityTracing.PitReadModelKind,
        DataVaultActivityTracing.ParentsRebuildScope);

    try {
      var tableName = GetPitTableName(request.Pit.Name);
      if (request.ParentHashKeys.Count == 0) {
        var emptyResult = new DataVaultPitMaintenanceResult(
            request.Pit,
            tableName,
            parentHashKeyCount: 0,
            rowsDeleted: 0,
            rowsWritten: 0);
        activity.RecordSuccess(
            affectedRowCount: 0,
            parentKeyCount: 0,
            isNoOp: true);

        return emptyResult;
      }

      var projection = CreatePitProjection(dbContext, request.Pit);
      var satelliteRows = await ReadSatelliteRowsAsync(
          dbContext,
          projection,
          request.ParentHashKeys,
          cancellationToken).ConfigureAwait(false);
      var rowsToWrite = CreatePitRows(projection, request.ParentHashKeys, satelliteRows);
      var rowsDeleted = await DeletePitRowsForParentsAsync(
          dbContext,
          projection,
          request.ParentHashKeys,
          cancellationToken).ConfigureAwait(false);
      DetachTrackedPitRows(dbContext, projection, request.ParentHashKeys.ToHashSet(StringComparer.Ordinal));
      var rowsWritten = await AddPitRowsAsync(dbContext, projection, rowsToWrite, cancellationToken).ConfigureAwait(false);
      var result = new DataVaultPitMaintenanceResult(
          request.Pit,
          projection.TableName,
          request.ParentHashKeys.Count,
          rowsDeleted,
          rowsWritten);

      activity.RecordSuccess(
          result.RowsDeleted + result.RowsWritten,
          result.ParentHashKeyCount,
          result.IsNoOp);

      return result;
    }
    catch (Exception exception) {
      activity.RecordFailure(exception);
      throw;
    }
  }

  private static IReadOnlyList<Dictionary<string, object>> CreatePitRows(
      PitMaintenanceProjection projection,
      IReadOnlyList<string> parentHashKeys,
      IReadOnlyList<IReadOnlyList<SatelliteMaintenanceRow>> satelliteRows) {
    if (projection.DrivingKeyNames.Count > 0) {
      return CreateTupleAwarePitRows(projection, parentHashKeys, satelliteRows);
    }

    var satelliteRowsByParent = satelliteRows
        .Select(rows => rows
            .GroupBy(row => row.ParentHashKey, StringComparer.Ordinal)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<SatelliteMaintenanceRow>)group
                    .OrderBy(row => row.LoadTimestamp)
                    .ThenBy(row => row.ParentHashKey, StringComparer.Ordinal)
                    .ToArray(),
                StringComparer.Ordinal))
        .ToArray();
    var pitRows = new List<Dictionary<string, object>>();

    foreach (var parentHashKey in parentHashKeys.Order(StringComparer.Ordinal)) {
      var timestamps = satelliteRowsByParent
          .SelectMany(rowsByParent => rowsByParent.TryGetValue(parentHashKey, out var rows)
              ? rows
              : Array.Empty<SatelliteMaintenanceRow>())
          .Select(row => row.LoadTimestamp)
          .Distinct()
          .OrderBy(timestamp => timestamp)
          .ToArray();

      foreach (var timestamp in timestamps) {
        var row = new Dictionary<string, object>(StringComparer.Ordinal) {
          [projection.ParentHashKeyColumnName] = parentHashKey,
          [projection.LoadTimestampColumnName] = ToProviderValue(projection.LoadTimestampProperty, timestamp),
        };

        for (var index = 0; index < projection.Satellites.Count; index++) {
          row.Add(
              projection.Satellites[index].SnapshotReferenceColumnName,
              FindSnapshotTimestamp(satelliteRowsByParent[index], parentHashKey, timestamp) is { } snapshotTimestamp
                  ? ToProviderValue(projection.Satellites[index].SnapshotReferenceProperty, snapshotTimestamp)
                  : null!);
        }

        pitRows.Add(row);
      }
    }

    return pitRows;
  }

  private static IReadOnlyList<Dictionary<string, object>> CreateTupleAwarePitRows(
      PitMaintenanceProjection projection,
      IReadOnlyList<string> parentHashKeys,
      IReadOnlyList<IReadOnlyList<SatelliteMaintenanceRow>> satelliteRows) {
    var requestedParents = parentHashKeys.ToHashSet(StringComparer.Ordinal);
    var satelliteRowsByParent = satelliteRows
        .Select(rows => rows
            .GroupBy(row => row.ParentHashKey, StringComparer.Ordinal)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<SatelliteMaintenanceRow>)group
                    .OrderBy(row => row.LoadTimestamp)
                    .ThenBy(row => row.DrivingKeyValueSignature, StringComparer.Ordinal)
                    .ToArray(),
                StringComparer.Ordinal))
        .ToArray();
    var satelliteRowsByIdentity = satelliteRows
        .Select(rows => rows
            .Where(row => row.DrivingKeyValues.Count > 0)
            .GroupBy(row => row.IdentityKey)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<SatelliteMaintenanceRow>)group
                    .OrderBy(row => row.LoadTimestamp)
                    .ThenBy(row => row.DrivingKeyValueSignature, StringComparer.Ordinal)
                    .ToArray()))
        .ToArray();
    var identities = satelliteRowsByIdentity
        .SelectMany(rowsByIdentity => rowsByIdentity.Keys)
        .Where(identity => requestedParents.Contains(identity.ParentHashKey))
        .Distinct()
        .OrderBy(identity => identity.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(identity => identity.DrivingKeyValueSignature, StringComparer.Ordinal)
        .ToArray();
    var pitRows = new List<Dictionary<string, object>>();

    foreach (var identity in identities) {
      var firstTupleTimestamp = satelliteRowsByIdentity
          .Select(rowsByIdentity => rowsByIdentity.TryGetValue(identity, out var rows) && rows.Count > 0
              ? rows[0].LoadTimestamp
              : (DateTimeOffset?)null)
          .Where(timestamp => timestamp.HasValue)
          .Min(timestamp => timestamp!.Value);
      var timestamps = satelliteRowsByIdentity
          .SelectMany(rowsByIdentity => rowsByIdentity.TryGetValue(identity, out var rows)
              ? rows
              : Array.Empty<SatelliteMaintenanceRow>())
          .Concat(satelliteRowsByParent
              .SelectMany(rowsByParent => rowsByParent.TryGetValue(identity.ParentHashKey, out var rows)
                  ? rows.Where(row => row.DrivingKeyValues.Count == 0)
                  : Array.Empty<SatelliteMaintenanceRow>())
              .Where(row => row.LoadTimestamp >= firstTupleTimestamp))
          .Select(row => row.LoadTimestamp)
          .Distinct()
          .OrderBy(timestamp => timestamp)
          .ToArray();

      foreach (var timestamp in timestamps) {
        var row = new Dictionary<string, object>(StringComparer.Ordinal) {
          [projection.ParentHashKeyColumnName] = identity.ParentHashKey,
          [projection.LoadTimestampColumnName] = ToProviderValue(projection.LoadTimestampProperty, timestamp),
        };

        for (var index = 0; index < projection.DrivingKeyColumnNames.Count; index++) {
          row.Add(projection.DrivingKeyColumnNames[index], identity.DrivingKeyValues[index]);
        }

        for (var index = 0; index < projection.Satellites.Count; index++) {
          var snapshotTimestamp = projection.Satellites[index].Satellite.DrivingKeyNames.Count > 0
              ? FindSnapshotTimestamp(satelliteRowsByIdentity[index], identity, timestamp)
              : FindSnapshotTimestamp(satelliteRowsByParent[index], identity.ParentHashKey, timestamp);
          row.Add(
              projection.Satellites[index].SnapshotReferenceColumnName,
              snapshotTimestamp is { } currentSnapshotTimestamp
                  ? ToProviderValue(projection.Satellites[index].SnapshotReferenceProperty, currentSnapshotTimestamp)
                  : null!);
        }

        pitRows.Add(row);
      }
    }

    return pitRows;
  }

  private static DateTimeOffset? FindSnapshotTimestamp(
      IReadOnlyDictionary<string, IReadOnlyList<SatelliteMaintenanceRow>> rowsByParent,
      string parentHashKey,
      DateTimeOffset pitTimestamp) {
    if (!rowsByParent.TryGetValue(parentHashKey, out var rows)) {
      return null;
    }

    DateTimeOffset? snapshotTimestamp = null;
    foreach (var row in rows) {
      if (row.LoadTimestamp > pitTimestamp) {
        break;
      }

      snapshotTimestamp = row.LoadTimestamp;
    }

    return snapshotTimestamp;
  }

  private static DateTimeOffset? FindSnapshotTimestamp(
      IReadOnlyDictionary<PitRowIdentityKey, IReadOnlyList<SatelliteMaintenanceRow>> rowsByIdentity,
      PitRowIdentityKey identity,
      DateTimeOffset pitTimestamp) {
    if (!rowsByIdentity.TryGetValue(identity, out var rows)) {
      return null;
    }

    DateTimeOffset? snapshotTimestamp = null;
    foreach (var row in rows) {
      if (row.LoadTimestamp > pitTimestamp) {
        break;
      }

      snapshotTimestamp = row.LoadTimestamp;
    }

    return snapshotTimestamp;
  }

  private static async Task<IReadOnlyList<IReadOnlyList<SatelliteMaintenanceRow>>> ReadSatelliteRowsAsync(
      DbContext dbContext,
      PitMaintenanceProjection projection,
      IReadOnlyList<string>? parentHashKeys,
      CancellationToken cancellationToken) {
    var satelliteRows = new IReadOnlyList<SatelliteMaintenanceRow>[projection.Satellites.Count];
    for (var index = 0; index < projection.Satellites.Count; index++) {
      satelliteRows[index] = await ReadSatelliteRowsAsync(
          dbContext,
          projection,
          projection.Satellites[index],
          parentHashKeys,
          cancellationToken).ConfigureAwait(false);
    }

    return satelliteRows;
  }

  private static async Task<IReadOnlyList<SatelliteMaintenanceRow>> ReadSatelliteRowsAsync(
      DbContext dbContext,
      PitMaintenanceProjection pitProjection,
      PitSatelliteMaintenanceProjection pitSatellite,
      IReadOnlyList<string>? parentHashKeys,
      CancellationToken cancellationToken) {
    var rows = dbContext.Set<Dictionary<string, object>>(pitSatellite.Satellite.TableName);
    var persistedRows = new List<Dictionary<string, object>>();

    try {
      if (parentHashKeys is null) {
        persistedRows.AddRange(await rows
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false));
      }
      else {
        foreach (var parentHashKeyBatch in parentHashKeys.Chunk(ParentHashKeyBatchSize)) {
          persistedRows.AddRange(await rows
              .AsNoTracking()
              .WhereStringPropertyEqualsAny(pitSatellite.Satellite.ParentHashKeyColumnName, parentHashKeyBatch)
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false));
        }
      }
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      throw PitMaintenanceFailure(
          pitProjection.MetadataName,
          "could not query generated satellite table/entity '" + pitSatellite.Satellite.TableName +
          "' for PIT satellite '" + pitSatellite.MetadataName + "'",
          exception);
    }

    return persistedRows
        .Select(row => {
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
          var drivingKeyValues = ReadRequiredStringValues(
              pitProjection.MetadataName,
              pitSatellite.Satellite.TableName,
              row,
              pitSatellite.Satellite.DrivingKeyColumnNames,
              "satellite driving key");

          return new SatelliteMaintenanceRow(parentHashKey, drivingKeyValues, loadTimestamp);
        })
        .OrderBy(row => row.ParentHashKey, StringComparer.Ordinal)
        .ThenBy(row => row.DrivingKeyValueSignature, StringComparer.Ordinal)
        .ThenBy(row => row.LoadTimestamp)
        .ToArray();
  }

  private static async Task<int> DeleteAllPitRowsAsync(
      DbContext dbContext,
      PitMaintenanceProjection projection,
      CancellationToken cancellationToken) {
    try {
      return await dbContext
          .Set<Dictionary<string, object>>(projection.TableName)
          .ExecuteDeleteAsync(cancellationToken)
          .ConfigureAwait(false);
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      throw PitMaintenanceFailure(
          projection.MetadataName,
          "could not delete generated PIT table/entity '" + projection.TableName + "'",
          exception);
    }
  }

  private static async Task<int> DeletePitRowsForParentsAsync(
      DbContext dbContext,
      PitMaintenanceProjection projection,
      IReadOnlyList<string> parentHashKeys,
      CancellationToken cancellationToken) {
    var rowsDeleted = 0;
    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);

    try {
      foreach (var parentHashKeyBatch in parentHashKeys.Chunk(ParentHashKeyBatchSize)) {
        rowsDeleted += await rows
            .WhereStringPropertyEqualsAny(projection.ParentHashKeyColumnName, parentHashKeyBatch)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
      }
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      throw PitMaintenanceFailure(
          projection.MetadataName,
          "could not delete generated PIT rows from table/entity '" + projection.TableName + "'",
          exception);
    }

    return rowsDeleted;
  }

  private static void DetachTrackedPitRows(
      DbContext dbContext,
      PitMaintenanceProjection projection,
      IReadOnlySet<string>? parentHashKeys) {
    foreach (var entry in dbContext.ChangeTracker.Entries().ToArray()) {
      if (entry.Entity is not Dictionary<string, object> row) {
        continue;
      }

      var producedName = entry.Metadata.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string;
      if (!string.Equals(producedName ?? entry.Metadata.Name, projection.TableName, StringComparison.Ordinal)) {
        continue;
      }

      if (parentHashKeys is not null &&
          (!row.TryGetValue(projection.ParentHashKeyColumnName, out var parentHashKeyValue) ||
          parentHashKeyValue is not string parentHashKey ||
          !parentHashKeys.Contains(parentHashKey))) {
        continue;
      }

      entry.State = EntityState.Detached;
    }
  }

  private static async Task<int> AddPitRowsAsync(
      DbContext dbContext,
      PitMaintenanceProjection projection,
      IReadOnlyList<Dictionary<string, object>> rowsToWrite,
      CancellationToken cancellationToken) {
    if (rowsToWrite.Count == 0) {
      return 0;
    }

    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);
    foreach (var row in rowsToWrite) {
      rows.Add(row);
    }

    try {
      await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    catch (Exception exception) when (exception is not OperationCanceledException) {
      throw PitMaintenanceFailure(
          projection.MetadataName,
          "could not insert regenerated PIT rows into table/entity '" + projection.TableName + "'",
          exception);
    }

    return rowsToWrite.Count;
  }

  private static PitMaintenanceProjection CreatePitProjection(
      DbContext dbContext,
      DataVaultPitMetadata pit) {
    DataVaultPitMaintenanceShapeValidator.ValidateSupportedShape(pit);

    var tableName = GetPitTableName(pit.Name);
    var entityType = dbContext.Model.FindEntityType(tableName);
    if (entityType is null) {
      throw PitMaintenanceFailure(
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

    var satellites = new PitSatelliteMaintenanceProjection[pit.Satellites.Count];
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

      satellites[index] = new PitSatelliteMaintenanceProjection(
          satelliteReference.SatelliteName,
          snapshotReferenceProperty.Name,
          snapshotReferenceProperty,
          CreateSatelliteProjection(dbContext, pit, satelliteReference));
    }

    var drivingKeyNames = GetSharedDrivingKeyNames(pit.Name, satellites);
    var pitDrivingKeyProperties = GetOrderedGeneratedProperties(
        entityType,
        DataVaultPropertyRole.DrivingKey,
        expectedTechnicalRole: null);
    ValidatePitDrivingKeyProperties(
        pit.Name,
        tableName,
        pitDrivingKeyProperties,
        drivingKeyNames);

    return new PitMaintenanceProjection(
        pit.Name,
        tableName,
        parentHashKeyProperty.Name,
        pitDrivingKeyProperties.Select(property => property.Name).ToArray(),
        drivingKeyNames,
        loadTimestampProperty.Name,
        loadTimestampProperty,
        satellites);
  }

  private static SatelliteMaintenanceProjection CreateSatelliteProjection(
      DbContext dbContext,
      DataVaultPitMetadata pit,
      DataVaultPitSatelliteReferenceMetadata satelliteReference) {
    var satelliteName = satelliteReference.SatelliteName;
    var tableName = NamingPolicy.GetSatelliteTableName(
        new DataVaultSatelliteNameContext(pit.Parent.Name, satelliteName));
    var entityType = dbContext.Model.FindEntityType(tableName);
    if (entityType is null) {
      throw PitMaintenanceFailure(
          pit.Name,
          "expected generated satellite table/entity '" + tableName + "' for PIT satellite '" + satelliteName +
          "' in the DbContext model");
    }

    ValidateGeneratedEntity(pit.Name, entityType, tableName, DataVaultTableKind.Satellite, satelliteName, pit.Parent);

    var drivingKeyProperties = GetOrderedGeneratedProperties(
        entityType,
        DataVaultPropertyRole.DrivingKey,
        expectedTechnicalRole: null);
    ValidateSatelliteMultiActiveReference(
        pit.Name,
        satelliteReference,
        drivingKeyProperties);
    foreach (var drivingKeyProperty in drivingKeyProperties) {
      ValidateStringProperty(pit.Name, tableName, drivingKeyProperty, "satellite driving key");
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

    var loadTimestampProperty = GetRequiredGeneratedProperty(
        pit.Name,
        tableName,
        entityType,
        DataVaultPropertyRole.Technical,
        TechnicalMetadataColumnRole.LoadTimestamp,
        metadataName: null,
        "satellite load timestamp");
    ValidateTimestampProperty(pit.Name, tableName, loadTimestampProperty, "satellite load timestamp");

    return new SatelliteMaintenanceProjection(
        tableName,
        parentHashKeyProperty.Name,
        loadTimestampProperty.Name,
        drivingKeyProperties.Select(property => GetRequiredMetadataName(pit.Name, tableName, property, "satellite driving key")).ToArray(),
        drivingKeyProperties.Select(property => property.Name).ToArray());
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
      throw PitMaintenanceFailure(
          pitName,
          "expected generated table/entity '" + tableName + "' to carry " + expectedKind + " entity kind metadata");
    }

    var metadataName = entityType.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string;
    if (!string.Equals(metadataName, expectedMetadataName, StringComparison.Ordinal)) {
      throw PitMaintenanceFailure(
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
      throw PitMaintenanceFailure(
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
      0 => throw PitMaintenanceFailure(
          pitName,
          "expected generated " + description + " property on table/entity '" + tableName + "'" +
          (metadataName is null ? string.Empty : " for metadata name '" + metadataName + "'")),
      _ => throw PitMaintenanceFailure(
          pitName,
          "expected generated " + description + " property on table/entity '" + tableName +
          "' to be unambiguous" +
          (metadataName is null ? string.Empty : " for metadata name '" + metadataName + "'")),
    };
  }

  private static IReadOnlyList<IProperty> GetOrderedGeneratedProperties(
      IEntityType entityType,
      DataVaultPropertyRole expectedRole,
      TechnicalMetadataColumnRole? expectedTechnicalRole) {
    return entityType.GetProperties()
        .Where(property => Equals(property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value, expectedRole))
        .Where(property => expectedTechnicalRole is null ||
            Equals(property.FindAnnotation(DataVaultAnnotationNames.TechnicalColumnRole)?.Value, expectedTechnicalRole))
        .OrderBy(property => property.FindAnnotation(DataVaultAnnotationNames.Ordinal)?.Value is int ordinal ? ordinal : int.MaxValue)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .ToArray();
  }

  private static string GetRequiredMetadataName(
      string pitName,
      string tableName,
      IProperty property,
      string description) {
    if (property.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value is string metadataName &&
        !string.IsNullOrWhiteSpace(metadataName)) {
      return metadataName;
    }

    throw PitMaintenanceFailure(
        pitName,
        "expected generated " + description + " property '" + property.Name +
        "' on table/entity '" + tableName + "' to carry metadata name");
  }

  private static void ValidateSatelliteMultiActiveReference(
      string pitName,
      DataVaultPitSatelliteReferenceMetadata satelliteReference,
      IReadOnlyList<IProperty> drivingKeyProperties) {
    var satelliteIsMultiActive = drivingKeyProperties.Count > 0;
    if (satelliteReference.IsMultiActive == satelliteIsMultiActive) {
      return;
    }

    throw PitMaintenanceFailure(
        pitName,
        "reference metadata for satellite '" +
        satelliteReference.SatelliteName +
        "' declares IsMultiActive=" +
        satelliteReference.IsMultiActive +
        ", but generated satellite metadata declares " +
        (satelliteIsMultiActive ? "multi-active driving keys" : "no driving keys"));
  }

  private static IReadOnlyList<string> GetSharedDrivingKeyNames(
      string pitName,
      IReadOnlyList<PitSatelliteMaintenanceProjection> satellites) {
    IReadOnlyList<string>? drivingKeyNames = null;
    string? drivingKeySatelliteName = null;

    foreach (var satellite in satellites.Where(satellite => satellite.Satellite.DrivingKeyNames.Count > 0)) {
      if (drivingKeyNames is null) {
        drivingKeyNames = satellite.Satellite.DrivingKeyNames;
        drivingKeySatelliteName = satellite.MetadataName;
        continue;
      }

      if (!drivingKeyNames.SequenceEqual(satellite.Satellite.DrivingKeyNames, StringComparer.Ordinal)) {
        throw PitMaintenanceFailure(
            pitName,
            "references multi-active satellite '" +
            satellite.MetadataName +
            "' with driving-key names [" +
            string.Join(", ", satellite.Satellite.DrivingKeyNames) +
            "] that do not match multi-active satellite '" +
            drivingKeySatelliteName +
            "' driving-key names [" +
            string.Join(", ", drivingKeyNames) +
            "] in canonical order");
      }
    }

    if (drivingKeyNames is null) {
      return Array.Empty<string>();
    }

    ValidatePitDrivingKeyNames(pitName, drivingKeyNames);
    return drivingKeyNames;
  }

  private static void ValidatePitDrivingKeyProperties(
      string pitName,
      string tableName,
      IReadOnlyList<IProperty> pitDrivingKeyProperties,
      IReadOnlyList<string> expectedDrivingKeyNames) {
    if (pitDrivingKeyProperties.Count != expectedDrivingKeyNames.Count) {
      throw PitMaintenanceFailure(
          pitName,
          "expected generated PIT table/entity '" + tableName + "' to expose " +
          expectedDrivingKeyNames.Count +
          " driving-key column(s) but found " +
          pitDrivingKeyProperties.Count);
    }

    for (var index = 0; index < pitDrivingKeyProperties.Count; index++) {
      ValidateStringProperty(pitName, tableName, pitDrivingKeyProperties[index], "PIT driving key");
      var metadataName = GetRequiredMetadataName(pitName, tableName, pitDrivingKeyProperties[index], "PIT driving key");
      if (!string.Equals(metadataName, expectedDrivingKeyNames[index], StringComparison.Ordinal)) {
        throw PitMaintenanceFailure(
            pitName,
            "expected generated PIT driving-key property '" +
            pitDrivingKeyProperties[index].Name +
            "' on table/entity '" +
            tableName +
            "' to carry metadata name '" +
            expectedDrivingKeyNames[index] +
            "' but found '" +
            metadataName +
            "'");
      }
    }
  }

  private static void ValidatePitDrivingKeyNames(
      string pitName,
      IReadOnlyList<string> drivingKeyNames) {
    foreach (var drivingKeyName in drivingKeyNames) {
      if (string.Equals(drivingKeyName, DataVaultPitProjectionRow.ParentHashKeyName, StringComparison.Ordinal) ||
          string.Equals(drivingKeyName, DataVaultPitProjectionRow.LoadTimestampName, StringComparison.Ordinal)) {
        throw PitMaintenanceFailure(
            pitName,
            "declares driving-key name '" +
            drivingKeyName +
            "' that collides with a reserved PIT row technical name");
      }
    }
  }

  private static void ValidateStringProperty(
      string pitName,
      string tableName,
      IProperty property,
      string description) {
    if (property.ClrType == typeof(string)) {
      return;
    }

    throw PitMaintenanceFailure(
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

    throw PitMaintenanceFailure(
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

    throw PitMaintenanceFailure(
        pitName,
        "expected generated " + description + " property '" + columnName +
        "' on table/entity '" + tableName + "' to contain a non-null string value");
  }

  private static IReadOnlyList<string> ReadRequiredStringValues(
      string pitName,
      string tableName,
      Dictionary<string, object> row,
      IReadOnlyList<string> columnNames,
      string description) {
    if (columnNames.Count == 0) {
      return Array.Empty<string>();
    }

    var values = new string[columnNames.Count];
    for (var index = 0; index < columnNames.Count; index++) {
      values[index] = ReadRequiredString(
          pitName,
          tableName,
          row,
          columnNames[index],
          description);
    }

    return values;
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

    throw PitMaintenanceFailure(
        pitName,
        "expected generated " + description + " property '" + columnName +
        "' on table/entity '" + tableName + "' to contain a non-null readable load timestamp value");
  }

  private static object ToProviderValue(IProperty property, DateTimeOffset timestamp) {
    return DataVaultLoadTimestampValueConverter.ToProviderValue(property, timestamp);
  }

  private static string GetPitTableName(string pitName) {
    return "Pit" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(pitName);
  }

  private static InvalidOperationException PitMaintenanceFailure(string pitName, string detail) {
    return PitMaintenanceFailure(pitName, detail, innerException: null);
  }

  private static InvalidOperationException PitMaintenanceFailure(
      string pitName,
      string detail,
      Exception? innerException) {
    return new InvalidOperationException(
        "DVault PIT maintenance failed: PIT metadata '" + pitName + "' " + detail + ".",
        innerException);
  }

  private sealed record PitMaintenanceProjection(
      string MetadataName,
      string TableName,
      string ParentHashKeyColumnName,
      IReadOnlyList<string> DrivingKeyColumnNames,
      IReadOnlyList<string> DrivingKeyNames,
      string LoadTimestampColumnName,
      IProperty LoadTimestampProperty,
      IReadOnlyList<PitSatelliteMaintenanceProjection> Satellites);

  private sealed record PitSatelliteMaintenanceProjection(
      string MetadataName,
      string SnapshotReferenceColumnName,
      IProperty SnapshotReferenceProperty,
      SatelliteMaintenanceProjection Satellite);

  private sealed record SatelliteMaintenanceProjection(
      string TableName,
      string ParentHashKeyColumnName,
      string LoadTimestampColumnName,
      IReadOnlyList<string> DrivingKeyNames,
      IReadOnlyList<string> DrivingKeyColumnNames);

  private sealed record SatelliteMaintenanceRow(
      string ParentHashKey,
      IReadOnlyList<string> DrivingKeyValues,
      DateTimeOffset LoadTimestamp) {
    public string DrivingKeyValueSignature { get; } = CreateOrdinalSignature(DrivingKeyValues);

    public PitRowIdentityKey IdentityKey { get; } = new(
        ParentHashKey,
        DrivingKeyValues.ToArray(),
        CreateOrdinalSignature(DrivingKeyValues));
  }

  private sealed class PitRowIdentityKey : IEquatable<PitRowIdentityKey> {
    public PitRowIdentityKey(
        string parentHashKey,
        IReadOnlyList<string> drivingKeyValues,
        string drivingKeyValueSignature) {
      ParentHashKey = parentHashKey;
      DrivingKeyValues = drivingKeyValues;
      DrivingKeyValueSignature = drivingKeyValueSignature;
    }

    public string ParentHashKey { get; }

    public IReadOnlyList<string> DrivingKeyValues { get; }

    public string DrivingKeyValueSignature { get; }

    public bool Equals(PitRowIdentityKey? other) {
      return other is not null &&
          string.Equals(ParentHashKey, other.ParentHashKey, StringComparison.Ordinal) &&
          string.Equals(DrivingKeyValueSignature, other.DrivingKeyValueSignature, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) {
      return Equals(obj as PitRowIdentityKey);
    }

    public override int GetHashCode() {
      return HashCode.Combine(
          StringComparer.Ordinal.GetHashCode(ParentHashKey),
          StringComparer.Ordinal.GetHashCode(DrivingKeyValueSignature));
    }
  }

  private static string CreateOrdinalSignature(IEnumerable<string> values) {
    return string.Concat(values.Select(value => value.Length.ToString(CultureInfo.InvariantCulture) + ":" + value));
  }
}
