using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCoding.Data.DVault;

internal sealed class DefaultDataVaultBridgeMaintenanceService : IDataVaultBridgeMaintenanceService {
  public async Task<DataVaultBridgeMaintenanceResult> RebuildBridgeAsync(
      DbContext dbContext,
      DataVaultBridgeMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    using var activity = DataVaultActivityTracing.StartMaintenanceActivity(
        dbContext,
        DataVaultActivityTracing.BridgeRebuildOperation,
        DataVaultActivityTracing.BridgeRebuildMaintenanceKind,
        DataVaultActivityTracing.BridgeReadModelKind,
        DataVaultActivityTracing.FullRebuildScope);

    try {
      var projection = CreateBridgeMaintenanceProjection(dbContext, request.Bridge);
      var desiredRows = await CreateDesiredRowsAsync(dbContext, projection, cancellationToken).ConfigureAwait(false);
      var bridgeRows = dbContext.Set<Dictionary<string, object>>(projection.BridgeTableName);
      var existingRows = await bridgeRows
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);

      if (existingRows.Count > 0) {
        bridgeRows.RemoveRange(existingRows);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }

      foreach (var desiredRow in desiredRows) {
        bridgeRows.Add(CreateBridgeRow(projection, desiredRow));
      }

      if (desiredRows.Count > 0) {
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }

      var result = new DataVaultBridgeMaintenanceResult(
          projection.MetadataName,
          projection.BridgeTableName,
          desiredRows.Count,
          rowsUpdated: 0,
          existingRows.Count,
          rowsUnchanged: 0);
      activity.RecordSuccess(
          result.RowsInserted + result.RowsUpdated + result.RowsDeleted,
          parentKeyCount: null,
          isNoOp: false);

      return result;
    }
    catch (Exception exception) {
      activity.RecordFailure(exception);
      throw;
    }
  }

  public async Task<DataVaultBridgeMaintenanceResult> MaintainBridgeAsync(
      DbContext dbContext,
      DataVaultBridgeMaintenanceRequest request,
      CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(dbContext);
    ArgumentNullException.ThrowIfNull(request);
    using var activity = DataVaultActivityTracing.StartMaintenanceActivity(
        dbContext,
        DataVaultActivityTracing.BridgeMaintainIncrementalOperation,
        DataVaultActivityTracing.BridgeMaintainIncrementalMaintenanceKind,
        DataVaultActivityTracing.BridgeReadModelKind,
        DataVaultActivityTracing.IncrementalRebuildScope);

    try {
      var projection = CreateBridgeMaintenanceProjection(dbContext, request.Bridge);
      var desiredRows = await CreateDesiredRowsAsync(dbContext, projection, cancellationToken).ConfigureAwait(false);
      var bridgeRows = dbContext.Set<Dictionary<string, object>>(projection.BridgeTableName);
      var existingRows = await bridgeRows
          .ToListAsync(cancellationToken)
          .ConfigureAwait(false);
      var existingRowsByKey = existingRows.ToDictionary(
          row => CreateBridgeRowKey(projection, row),
          StringComparer.Ordinal);
      var rowsInserted = 0;
      var rowsUpdated = 0;
      var rowsUnchanged = 0;

      foreach (var desiredRow in desiredRows) {
        if (!existingRowsByKey.TryGetValue(desiredRow.Key, out var existingRow)) {
          bridgeRows.Add(CreateBridgeRow(projection, desiredRow));
          rowsInserted++;
          continue;
        }

        if (projection.TraversalDepthColumnName is not null &&
            desiredRow.TraversalDepth.HasValue &&
            ReadInt32(projection, existingRow, projection.TraversalDepthColumnName) > desiredRow.TraversalDepth.Value) {
          existingRow[projection.TraversalDepthColumnName] = desiredRow.TraversalDepth.Value;
          rowsUpdated++;
          continue;
        }

        rowsUnchanged++;
      }

      if (rowsInserted > 0 || rowsUpdated > 0) {
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
      }

      var result = new DataVaultBridgeMaintenanceResult(
          projection.MetadataName,
          projection.BridgeTableName,
          rowsInserted,
          rowsUpdated,
          rowsDeleted: 0,
          rowsUnchanged);
      activity.RecordSuccess(
          result.RowsInserted + result.RowsUpdated + result.RowsDeleted,
          parentKeyCount: null,
          isNoOp: result.RowsInserted == 0 && result.RowsUpdated == 0 && result.RowsDeleted == 0);

      return result;
    }
    catch (Exception exception) {
      activity.RecordFailure(exception);
      throw;
    }
  }

  private static BridgeMaintenanceProjection CreateBridgeMaintenanceProjection(
      DbContext dbContext,
      DataVaultBridgeMetadata bridge) {
    if (bridge.ProjectionFeatures != DataVaultBridgeProjectionFeatures.None) {
      throw new NotSupportedException(
          "DVault bridge maintenance failed: bridge metadata '" +
          bridge.Name +
          "' requests unsupported provider-neutral projection feature(s) '" +
          bridge.ProjectionFeatures +
          "'. Bridge maintenance supports only endpoint hash-key columns and hierarchy TraversalDepth.");
    }

    var linkEntity = RequireGeneratedEntity(
        dbContext,
        bridge.Name,
        bridge.LinkReference.Name,
        DataVaultTableKind.Link);
    var bridgeEntity = RequireGeneratedEntity(
        dbContext,
        bridge.Name,
        bridge.Name,
        DataVaultTableKind.Bridge,
        GetBridgeTableName(bridge));
    var endpoints = bridge.Endpoints
        .Select(endpoint => new BridgeEndpointMaintenanceProjection(
            endpoint.Role,
            endpoint.SourceEndpointName,
            GetBridgeEndpointHashKeyColumnName(endpoint),
            ResolveSourceLinkColumnName(bridge, linkEntity, endpoint)))
        .ToArray();

    ValidateEndpointBindings(bridge, endpoints);
    ValidateDistinctColumns(bridge, endpoints.Select(endpoint => endpoint.BridgeColumnName), "bridge");
    ValidateDistinctColumns(bridge, endpoints.Select(endpoint => endpoint.SourceLinkColumnName), "source link");

    foreach (var endpoint in endpoints) {
      ValidateGeneratedProperty(
          bridge.Name,
          bridgeEntity,
          endpoint.BridgeColumnName,
          typeof(string),
          DataVaultPropertyRole.ParticipantReference);
      ValidateGeneratedProperty(
          bridge.Name,
          linkEntity,
          endpoint.SourceLinkColumnName,
          typeof(string),
          DataVaultPropertyRole.ParticipantReference);
    }

    var traversalDepthColumnName = bridge.Kind == DataVaultBridgeKind.Hierarchy
        ? DataVaultBridgeProjectionRow.TraversalDepthName
        : null;
    if (traversalDepthColumnName is not null) {
      ValidateGeneratedProperty(
          bridge.Name,
          bridgeEntity,
          traversalDepthColumnName,
          typeof(int),
          DataVaultPropertyRole.BridgeDepth);
    }

    return new BridgeMaintenanceProjection(
        bridge.Name,
        bridge.Kind,
        GetProducedName(linkEntity),
        GetProducedName(bridgeEntity),
        endpoints,
        traversalDepthColumnName);
  }

  private static IEntityType RequireGeneratedEntity(
      DbContext dbContext,
      string bridgeName,
      string metadataName,
      DataVaultTableKind tableKind,
      string? expectedProducedName = null) {
    var matches = dbContext.Model
        .GetEntityTypes()
        .Where(entity => Equals(entity.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value, tableKind))
        .Where(entity => string.Equals(
            entity.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string,
            metadataName,
            StringComparison.Ordinal))
        .Where(entity => expectedProducedName is null || string.Equals(GetProducedName(entity), expectedProducedName, StringComparison.Ordinal))
        .ToArray();

    return matches.Length switch {
      1 => matches[0],
      0 => throw BridgeMaintenanceFailure(
          bridgeName,
          "expected generated " +
          tableKind.ToString().ToLowerInvariant() +
          " table/entity for metadata '" +
          metadataName +
          "'"),
      _ => throw BridgeMaintenanceFailure(
          bridgeName,
          "found more than one generated " +
          tableKind.ToString().ToLowerInvariant() +
          " table/entity for metadata '" +
          metadataName +
          "'"),
    };
  }

  private static string ResolveSourceLinkColumnName(
      DataVaultBridgeMetadata bridge,
      IEntityType linkEntity,
      DataVaultBridgeEndpointMetadata endpoint) {
    var participantProperties = linkEntity
        .GetProperties()
        .Where(property => Equals(
            property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value,
            DataVaultPropertyRole.ParticipantReference))
        .OrderBy(property => property.FindAnnotation(DataVaultAnnotationNames.Ordinal)?.Value as int? ?? int.MaxValue)
        .ThenBy(property => property.Name, StringComparer.Ordinal)
        .ToArray();

    var participantOrdinal = endpoint.Role switch {
      DataVaultBridgeEndpointRole.From or DataVaultBridgeEndpointRole.Ancestor => bridge.SourceParticipantOrdinal,
      DataVaultBridgeEndpointRole.To or DataVaultBridgeEndpointRole.Descendant => bridge.TargetParticipantOrdinal,
      _ => throw new ArgumentOutOfRangeException(nameof(endpoint), endpoint.Role, "Unsupported bridge endpoint role."),
    };

    if (participantOrdinal.HasValue) {
      if (participantOrdinal.Value >= participantProperties.Length) {
        throw BridgeMaintenanceFailure(
            bridge.Name,
            "selects source-link participant ordinal " +
            participantOrdinal.Value +
            " but generated source link table/entity '" +
            GetProducedName(linkEntity) +
            "' exposes only " +
            participantProperties.Length +
            " participant hash-key column(s)");
      }

      return participantProperties[participantOrdinal.Value].Name;
    }

    var matches = participantProperties
        .Where(property => string.Equals(
            property.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string,
            endpoint.SourceEndpointName,
            StringComparison.Ordinal))
        .ToArray();

    return matches.Length switch {
      1 => matches[0].Name,
      0 => throw BridgeMaintenanceFailure(
          bridge.Name,
          "could not resolve source-link participant '" +
          endpoint.SourceEndpointName +
          "' on generated source link table/entity '" +
          GetProducedName(linkEntity) +
          "'"),
      _ => throw BridgeMaintenanceFailure(
          bridge.Name,
          "found ambiguous source-link participant '" +
          endpoint.SourceEndpointName +
          "' on generated source link table/entity '" +
          GetProducedName(linkEntity) +
          "'"),
    };
  }

  private static void ValidateEndpointBindings(
      DataVaultBridgeMetadata bridge,
      IReadOnlyList<BridgeEndpointMaintenanceProjection> endpoints) {
    switch (bridge.Kind) {
      case DataVaultBridgeKind.ManyToMany:
        if (endpoints.Count != 2 ||
            endpoints.Count(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.From) != 1 ||
            endpoints.Count(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.To) != 1) {
          throw BridgeMaintenanceFailure(
              bridge.Name,
              "has malformed endpoint bindings for bridge kind '" +
              bridge.Kind +
              "'; expected exactly one From endpoint and exactly one To endpoint");
        }

        return;

      case DataVaultBridgeKind.Hierarchy:
        if (endpoints.Count != 2 ||
            endpoints.Count(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Ancestor) != 1 ||
            endpoints.Count(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Descendant) != 1) {
          throw BridgeMaintenanceFailure(
              bridge.Name,
              "has malformed endpoint bindings for bridge kind '" +
              bridge.Kind +
              "'; expected exactly one Ancestor endpoint and exactly one Descendant endpoint");
        }

        return;

      default:
        throw new NotSupportedException(
            "DVault bridge maintenance failed: bridge metadata '" +
            bridge.Name +
            "' declares unsupported bridge kind '" +
            bridge.Kind +
            "'. Bridge maintenance supports only many-to-many and hierarchy bridge metadata.");
    }
  }

  private static void ValidateDistinctColumns(
      DataVaultBridgeMetadata bridge,
      IEnumerable<string> columnNames,
      string tableDescription) {
    var duplicateColumnName = columnNames
        .GroupBy(columnName => columnName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();

    if (duplicateColumnName is not null) {
      throw BridgeMaintenanceFailure(
          bridge.Name,
          "has malformed endpoint bindings for bridge kind '" +
          bridge.Kind +
          "' because generated " +
          tableDescription +
          " property '" +
          duplicateColumnName +
          "' is not distinct");
    }
  }

  private static void ValidateGeneratedProperty(
      string bridgeName,
      IEntityType entityType,
      string propertyName,
      Type expectedClrType,
      DataVaultPropertyRole expectedRole) {
    var property = entityType.FindProperty(propertyName);
    if (property is null) {
      throw BridgeMaintenanceFailure(
          bridgeName,
          "expected generated property '" +
          propertyName +
          "' on table/entity '" +
          GetProducedName(entityType) +
          "'");
    }

    if (property.ClrType != expectedClrType) {
      throw BridgeMaintenanceFailure(
          bridgeName,
          "expected generated property '" +
          propertyName +
          "' on table/entity '" +
          GetProducedName(entityType) +
          "' to use CLR type '" +
          expectedClrType.FullName +
          "' but found '" +
          property.ClrType.FullName +
          "'");
    }

    var propertyRole = property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value;
    if (!Equals(propertyRole, expectedRole)) {
      throw BridgeMaintenanceFailure(
          bridgeName,
          "expected generated property '" +
          propertyName +
          "' on table/entity '" +
          GetProducedName(entityType) +
          "' to carry property role '" +
          expectedRole +
          "'");
    }
  }

  private static async Task<IReadOnlyList<DesiredBridgeRow>> CreateDesiredRowsAsync(
      DbContext dbContext,
      BridgeMaintenanceProjection projection,
      CancellationToken cancellationToken) {
    var sourceRows = await dbContext.Set<Dictionary<string, object>>(projection.SourceLinkTableName)
        .AsNoTracking()
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);

    return projection.Kind switch {
      DataVaultBridgeKind.ManyToMany => CreateManyToManyDesiredRows(projection, sourceRows),
      DataVaultBridgeKind.Hierarchy => CreateHierarchyDesiredRows(projection, sourceRows),
      _ => throw new ArgumentOutOfRangeException(nameof(projection), projection.Kind, "Unsupported bridge kind."),
    };
  }

  private static IReadOnlyList<DesiredBridgeRow> CreateManyToManyDesiredRows(
      BridgeMaintenanceProjection projection,
      IEnumerable<Dictionary<string, object>> sourceRows) {
    var desiredRowsByKey = new Dictionary<string, DesiredBridgeRow>(StringComparer.Ordinal);

    foreach (var sourceRow in sourceRows) {
      var endpointValues = projection.Endpoints
          .Select(endpoint => ReadString(projection, sourceRow, endpoint.SourceLinkColumnName))
          .ToArray();
      var key = CreateOrdinalSignature(endpointValues);
      desiredRowsByKey.TryAdd(key, new DesiredBridgeRow(key, endpointValues, TraversalDepth: null));
    }

    return desiredRowsByKey.Values
        .OrderBy(row => row.Key, StringComparer.Ordinal)
        .ToArray();
  }

  private static IReadOnlyList<DesiredBridgeRow> CreateHierarchyDesiredRows(
      BridgeMaintenanceProjection projection,
      IEnumerable<Dictionary<string, object>> sourceRows) {
    var ancestorEndpoint = projection.Endpoints.Single(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Ancestor);
    var descendantEndpoint = projection.Endpoints.Single(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Descendant);
    var descendantsByAncestor = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);

    foreach (var sourceRow in sourceRows) {
      var ancestor = ReadString(projection, sourceRow, ancestorEndpoint.SourceLinkColumnName);
      var descendant = ReadString(projection, sourceRow, descendantEndpoint.SourceLinkColumnName);

      if (!descendantsByAncestor.TryGetValue(ancestor, out var descendants)) {
        descendants = new HashSet<string>(StringComparer.Ordinal);
        descendantsByAncestor[ancestor] = descendants;
      }

      descendants.Add(descendant);
    }

    var desiredRowsByKey = new Dictionary<string, DesiredBridgeRow>(StringComparer.Ordinal);
    foreach (var ancestor in descendantsByAncestor.Keys.Order(StringComparer.Ordinal)) {
      foreach (var descendantDepth in GetShortestDescendantDepths(ancestor, descendantsByAncestor)) {
        var endpointValues = projection.Endpoints
            .Select(endpoint => endpoint.Role == DataVaultBridgeEndpointRole.Ancestor
                ? ancestor
                : descendantDepth.Descendant)
            .ToArray();
        var key = CreateOrdinalSignature(endpointValues);
        desiredRowsByKey[key] = new DesiredBridgeRow(key, endpointValues, descendantDepth.Depth);
      }
    }

    return desiredRowsByKey.Values
        .OrderBy(row => row.Key, StringComparer.Ordinal)
        .ToArray();
  }

  private static IEnumerable<DescendantDepth> GetShortestDescendantDepths(
      string ancestor,
      IReadOnlyDictionary<string, HashSet<string>> descendantsByAncestor) {
    var depthsByDescendant = new Dictionary<string, int>(StringComparer.Ordinal) {
      [ancestor] = 0,
    };
    var queue = new Queue<DescendantDepth>();

    if (!descendantsByAncestor.TryGetValue(ancestor, out var directDescendants)) {
      return [];
    }

    foreach (var descendant in directDescendants.Order(StringComparer.Ordinal)) {
      queue.Enqueue(new DescendantDepth(descendant, Depth: 1));
    }

    while (queue.Count > 0) {
      var current = queue.Dequeue();
      if (depthsByDescendant.ContainsKey(current.Descendant)) {
        continue;
      }

      depthsByDescendant[current.Descendant] = current.Depth;

      if (!descendantsByAncestor.TryGetValue(current.Descendant, out var nextDescendants)) {
        continue;
      }

      foreach (var nextDescendant in nextDescendants.Order(StringComparer.Ordinal)) {
        if (!depthsByDescendant.ContainsKey(nextDescendant)) {
          queue.Enqueue(new DescendantDepth(nextDescendant, current.Depth + 1));
        }
      }
    }

    return depthsByDescendant
        .Where(pair => pair.Value > 0)
        .OrderBy(pair => pair.Key, StringComparer.Ordinal)
        .Select(pair => new DescendantDepth(pair.Key, pair.Value))
        .ToArray();
  }

  private static Dictionary<string, object> CreateBridgeRow(
      BridgeMaintenanceProjection projection,
      DesiredBridgeRow desiredRow) {
    var row = new Dictionary<string, object>(StringComparer.Ordinal);
    for (var index = 0; index < projection.Endpoints.Count; index++) {
      row[projection.Endpoints[index].BridgeColumnName] = desiredRow.EndpointHashKeys[index];
    }

    if (projection.TraversalDepthColumnName is not null) {
      row[projection.TraversalDepthColumnName] = desiredRow.TraversalDepth ??
          throw BridgeMaintenanceFailure(
              projection.MetadataName,
              "expected hierarchy maintenance row to carry positive TraversalDepth");
    }

    return row;
  }

  private static string CreateBridgeRowKey(
      BridgeMaintenanceProjection projection,
      Dictionary<string, object> row) {
    return CreateOrdinalSignature(projection.Endpoints
        .Select(endpoint => ReadString(projection, row, endpoint.BridgeColumnName)));
  }

  private static string ReadString(
      BridgeMaintenanceProjection projection,
      Dictionary<string, object> row,
      string columnName) {
    if (row.TryGetValue(columnName, out var value) && value is string text) {
      return text;
    }

    throw BridgeMaintenanceFailure(
        projection.MetadataName,
        "expected generated property '" +
        columnName +
        "' on table/entity '" +
        projection.SourceLinkTableName +
        "' or '" +
        projection.BridgeTableName +
        "' to contain a non-null string hash-key value");
  }

  private static int ReadInt32(
      BridgeMaintenanceProjection projection,
      Dictionary<string, object> row,
      string columnName) {
    if (row.TryGetValue(columnName, out var value) && value is int number) {
      return number;
    }

    throw BridgeMaintenanceFailure(
        projection.MetadataName,
        "expected generated property '" +
        columnName +
        "' on table/entity '" +
        projection.BridgeTableName +
        "' to contain a non-null integer TraversalDepth value");
  }

  private static string GetBridgeTableName(DataVaultBridgeMetadata bridge) {
    return "Bridge" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(bridge.Name);
  }

  private static string GetBridgeEndpointHashKeyColumnName(DataVaultBridgeEndpointMetadata endpoint) {
    var baseName = endpoint.Role switch {
      DataVaultBridgeEndpointRole.Ancestor => "Ancestor" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      DataVaultBridgeEndpointRole.Descendant => "Descendant" + DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(endpoint.HubReference.Name),
      _ => endpoint.HubReference.Name,
    };

    return DefaultNamingPolicy.Instance.NormalizeProducedIdentifier(baseName) + "HashKey";
  }

  private static string GetProducedName(IEntityType entityType) {
    return entityType.FindAnnotation(DataVaultAnnotationNames.ProducedName)?.Value as string ??
        entityType.Name;
  }

  private static InvalidOperationException BridgeMaintenanceFailure(string bridgeName, string detail) {
    return new InvalidOperationException(
        "DVault bridge maintenance failed: bridge metadata '" +
        bridgeName +
        "' " +
        detail +
        ".");
  }

  private static string CreateOrdinalSignature(IEnumerable<string> values) {
    return string.Join('\u001f', values);
  }

  private sealed record BridgeMaintenanceProjection(
      string MetadataName,
      DataVaultBridgeKind Kind,
      string SourceLinkTableName,
      string BridgeTableName,
      IReadOnlyList<BridgeEndpointMaintenanceProjection> Endpoints,
      string? TraversalDepthColumnName);

  private sealed record BridgeEndpointMaintenanceProjection(
      DataVaultBridgeEndpointRole Role,
      string EndpointName,
      string BridgeColumnName,
      string SourceLinkColumnName);

  private sealed record DesiredBridgeRow(
      string Key,
      IReadOnlyList<string> EndpointHashKeys,
      int? TraversalDepth);

  private sealed record DescendantDepth(string Descendant, int Depth);
}
