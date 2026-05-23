using System.Collections.ObjectModel;
using DCoding.Data.DVault.Modeling;
using Microsoft.EntityFrameworkCore;

namespace DCoding.Data.DVault;

internal static class DataVaultBridgeReadPipeline {
  private const int EndpointHashKeyBatchSize = 500;

  public static Task<IReadOnlyList<DataVaultBridgeReadRecord>> ReadBridgeReadRecordsAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken) {
    return ReadBridgeRowsAsync(
        dbContext,
        request,
        CreateReadRecord,
        record => record.EndpointHashKeys.Select(endpoint => endpoint.HashKey),
        record => record.TraversalDepth,
        cancellationToken);
  }

  public static async Task<IReadOnlyList<DataVaultBridgeProjectionRow>> ReadBridgeProjectionRowsAsync(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      CancellationToken cancellationToken) {
    var rows = await ReadBridgeRowsAsync(
        dbContext,
        request,
        CreateProjectionReadRow,
        row => row.EndpointHashKeys,
        row => row.TraversalDepth,
        cancellationToken).ConfigureAwait(false);

    return rows
        .Select(row => row.ProjectionRow)
        .ToArray();
  }

  private static async Task<IReadOnlyList<TReadRow>> ReadBridgeRowsAsync<TReadRow>(
      DbContext dbContext,
      DataVaultBridgeReadRequest request,
      Func<BridgeReadProjection, Dictionary<string, object>, TReadRow?> createRow,
      Func<TReadRow, IEnumerable<string>> getEndpointHashKeys,
      Func<TReadRow, int?> getTraversalDepth,
      CancellationToken cancellationToken)
      where TReadRow : class {
    var projection = CreateBridgeProjection(dbContext, request);
    if (request.EndpointHashKeys.Count == 0) {
      return [];
    }

    var rows = dbContext.Set<Dictionary<string, object>>(projection.TableName);
    var readRows = new List<TReadRow>();

    foreach (var endpointHashKeyBatch in request.EndpointHashKeys.Chunk(EndpointHashKeyBatchSize)) {
      List<Dictionary<string, object>> persistedRows;
      try {
        var query = rows
            .AsNoTracking()
            .WhereStringPropertyEqualsAny(projection.FilterColumnName, endpointHashKeyBatch);
        if (projection.MaximumDepth.HasValue && projection.TraversalDepthColumnName is not null) {
          query = query.WhereIntPropertyLessThanOrEqual(
              projection.TraversalDepthColumnName,
              projection.MaximumDepth.Value);
        }

        persistedRows = await query
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
      }
      catch (Exception exception) when (exception is not OperationCanceledException) {
        throw BridgeReadFailure(
            projection.MetadataName,
            "could not query generated bridge table/entity '" +
            projection.TableName +
            "'",
            exception);
      }

      foreach (var row in persistedRows) {
        var readRow = createRow(projection, row);
        if (readRow is null) {
          continue;
        }

        var traversalDepth = getTraversalDepth(readRow);
        if (projection.MaximumDepth.HasValue && traversalDepth > projection.MaximumDepth.Value) {
          continue;
        }

        readRows.Add(readRow);
      }
    }

    return OrderBridgeRows(readRows, getEndpointHashKeys, getTraversalDepth);
  }

  internal static BridgeReadProjection CreateBridgeProjection(
      DbContext dbContext,
      DataVaultBridgeReadRequest request) {
    var bridge = request.Bridge;
    if (bridge.ProjectionFeatures != DataVaultBridgeProjectionFeatures.None) {
      throw new NotSupportedException(
          "DVault bridge read failed: bridge metadata '" +
          bridge.Name +
          "' requests unsupported provider-neutral projection feature(s) '" +
          bridge.ProjectionFeatures +
          "'. Bridge reads support only endpoint hash-key columns and hierarchy TraversalDepth.");
    }

    var tableName = GetBridgeTableName(bridge);
    var endpointColumns = bridge.Endpoints
        .Select(endpoint => new BridgeEndpointProjection(
            ToPublicEndpoint(endpoint.Role),
            endpoint.SourceEndpointName,
            GetBridgeEndpointHashKeyColumnName(endpoint)))
        .ToArray();

    ValidateEndpointBindings(bridge, endpointColumns);
    ValidateRequestedEndpoint(bridge, request.Endpoint);
    ValidateDistinctEndpointColumns(bridge, endpointColumns);

    var filterColumn = endpointColumns
        .Single(endpoint => endpoint.Endpoint == request.Endpoint)
        .ColumnName;
    var traversalDepthColumnName = bridge.Kind == DataVaultBridgeKind.Hierarchy
        ? DataVaultBridgeProjectionRow.TraversalDepthName
        : null;

    ValidateGeneratedBridgeEntity(dbContext, bridge, tableName, endpointColumns, traversalDepthColumnName);

    return new BridgeReadProjection(
        bridge.Name,
        bridge.Kind,
        tableName,
        endpointColumns,
        filterColumn,
        traversalDepthColumnName,
        request.MaximumDepth);
  }

  internal static DataVaultBridgeReadRecord CreateReadRecord(
      BridgeReadProjection projection,
      Dictionary<string, object> row) {
    var endpoints = ReadEndpointValues(projection, row);
    var traversalDepth = ReadTraversalDepth(projection, row);

    return new DataVaultBridgeReadRecord(
        projection.MetadataName,
        projection.TableName,
        endpoints,
        traversalDepth);
  }

  internal static BridgeProjectionReadRow CreateProjectionReadRow(
      BridgeReadProjection projection,
      Dictionary<string, object> row) {
    var endpoints = ReadEndpointValues(projection, row);
    var traversalDepth = ReadTraversalDepth(projection, row);
    var values = new Dictionary<string, DataVaultBridgeProjectionValue>(StringComparer.Ordinal);

    foreach (var endpoint in endpoints) {
      values[endpoint.ColumnName] = DataVaultBridgeProjectionValue.Present(endpoint.HashKey);
    }

    if (traversalDepth.HasValue) {
      values[DataVaultBridgeProjectionRow.TraversalDepthName] = DataVaultBridgeProjectionValue.Present(traversalDepth.Value);
    }

    return new BridgeProjectionReadRow(
        endpoints.Select(endpoint => endpoint.HashKey).ToArray(),
        traversalDepth,
        new DataVaultBridgeProjectionRow(
            projection.MetadataName,
            new ReadOnlyDictionary<string, DataVaultBridgeProjectionValue>(values)));
  }

  internal static IReadOnlyList<TReadRow> OrderBridgeRows<TReadRow>(
      IEnumerable<TReadRow> readRows,
      Func<TReadRow, IEnumerable<string>> getEndpointHashKeys,
      Func<TReadRow, int?> getTraversalDepth)
      where TReadRow : class {
    return readRows
        .OrderBy(row => CreateOrdinalSignature(getEndpointHashKeys(row)), StringComparer.Ordinal)
        .ThenBy(row => getTraversalDepth(row) ?? -1)
        .ToArray();
  }

  private static IReadOnlyList<DataVaultBridgeEndpointReadValue> ReadEndpointValues(
      BridgeReadProjection projection,
      Dictionary<string, object> row) {
    var values = new DataVaultBridgeEndpointReadValue[projection.Endpoints.Count];

    for (var index = 0; index < projection.Endpoints.Count; index++) {
      var endpoint = projection.Endpoints[index];
      values[index] = new DataVaultBridgeEndpointReadValue(
          endpoint.Endpoint,
          endpoint.EndpointName,
          endpoint.ColumnName,
          ReadStringProperty(projection, row, endpoint.ColumnName));
    }

    return values;
  }

  private static int? ReadTraversalDepth(
      BridgeReadProjection projection,
      Dictionary<string, object> row) {
    if (projection.TraversalDepthColumnName is null) {
      return null;
    }

    if (!row.TryGetValue(projection.TraversalDepthColumnName, out var value) || value is not int depth) {
      throw BridgeReadFailure(
          projection.MetadataName,
          "expected generated bridge property '" +
          projection.TraversalDepthColumnName +
          "' on table/entity '" +
          projection.TableName +
          "' to contain a non-null integer TraversalDepth value");
    }

    if (depth < 0) {
      throw BridgeReadFailure(
          projection.MetadataName,
          "encountered unsupported negative TraversalDepth value '" +
          depth +
          "' on table/entity '" +
          projection.TableName +
          "'");
    }

    return depth;
  }

  private static string ReadStringProperty(
      BridgeReadProjection projection,
      Dictionary<string, object> row,
      string columnName) {
    if (row.TryGetValue(columnName, out var value) && value is string text) {
      return text;
    }

    throw BridgeReadFailure(
        projection.MetadataName,
        "expected generated bridge property '" +
        columnName +
        "' on table/entity '" +
        projection.TableName +
        "' to contain a non-null string endpoint hash key value");
  }

  private static void ValidateEndpointBindings(
      DataVaultBridgeMetadata bridge,
      IReadOnlyList<BridgeEndpointProjection> endpointColumns) {
    switch (bridge.Kind) {
      case DataVaultBridgeKind.ManyToMany:
        if (endpointColumns.Count != 2 ||
            endpointColumns.Count(endpoint => endpoint.Endpoint == DataVaultBridgeTraversalEndpoint.From) != 1 ||
            endpointColumns.Count(endpoint => endpoint.Endpoint == DataVaultBridgeTraversalEndpoint.To) != 1) {
          throw BridgeReadFailure(
              bridge.Name,
              "has malformed endpoint bindings for bridge kind '" +
              bridge.Kind +
              "'; expected exactly one From endpoint and exactly one To endpoint");
        }

        return;

      case DataVaultBridgeKind.Hierarchy:
        if (endpointColumns.Count != 2 ||
            endpointColumns.Count(endpoint => endpoint.Endpoint == DataVaultBridgeTraversalEndpoint.Ancestor) != 1 ||
            endpointColumns.Count(endpoint => endpoint.Endpoint == DataVaultBridgeTraversalEndpoint.Descendant) != 1) {
          throw BridgeReadFailure(
              bridge.Name,
              "has malformed endpoint bindings for bridge kind '" +
              bridge.Kind +
              "'; expected exactly one Ancestor endpoint and exactly one Descendant endpoint");
        }

        return;

      default:
        throw new NotSupportedException(
            "DVault bridge read failed: bridge metadata '" +
            bridge.Name +
            "' declares unsupported bridge kind '" +
            bridge.Kind +
            "'. Bridge reads support only many-to-many and hierarchy bridge metadata.");
    }
  }

  private static void ValidateRequestedEndpoint(
      DataVaultBridgeMetadata bridge,
      DataVaultBridgeTraversalEndpoint endpoint) {
    var isSupported = bridge.Kind switch {
      DataVaultBridgeKind.ManyToMany => endpoint is DataVaultBridgeTraversalEndpoint.From or DataVaultBridgeTraversalEndpoint.To,
      DataVaultBridgeKind.Hierarchy => endpoint is DataVaultBridgeTraversalEndpoint.Ancestor or DataVaultBridgeTraversalEndpoint.Descendant,
      _ => false,
    };

    if (!isSupported) {
      throw BridgeReadFailure(
          bridge.Name,
          "does not support requested endpoint '" +
          endpoint +
          "' for bridge kind '" +
          bridge.Kind +
          "'");
    }
  }

  private static void ValidateDistinctEndpointColumns(
      DataVaultBridgeMetadata bridge,
      IReadOnlyList<BridgeEndpointProjection> endpointColumns) {
    var duplicateColumnName = endpointColumns
        .GroupBy(endpoint => endpoint.ColumnName, StringComparer.Ordinal)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key)
        .FirstOrDefault();

    if (duplicateColumnName is not null) {
      throw BridgeReadFailure(
          bridge.Name,
          "has malformed endpoint bindings for bridge kind '" +
          bridge.Kind +
          "' because generated endpoint property '" +
          duplicateColumnName +
          "' is not distinct");
    }
  }

  private static void ValidateGeneratedBridgeEntity(
      DbContext dbContext,
      DataVaultBridgeMetadata bridge,
      string tableName,
      IReadOnlyList<BridgeEndpointProjection> endpointColumns,
      string? traversalDepthColumnName) {
    var entityType = dbContext.Model.FindEntityType(tableName);
    if (entityType is null) {
      throw BridgeReadFailure(
          bridge.Name,
          "expected generated bridge table/entity '" +
          tableName +
          "' in the DbContext model");
    }

    var entityKind = entityType.FindAnnotation(DataVaultAnnotationNames.EntityKind)?.Value;
    if (!Equals(entityKind, DataVaultTableKind.Bridge)) {
      throw BridgeReadFailure(
          bridge.Name,
          "expected generated table/entity '" +
          tableName +
          "' to carry bridge entity kind metadata");
    }

    var metadataName = entityType.FindAnnotation(DataVaultAnnotationNames.MetadataName)?.Value as string;
    if (!string.Equals(metadataName, bridge.Name, StringComparison.Ordinal)) {
      throw BridgeReadFailure(
          bridge.Name,
          "expected generated table/entity '" +
          tableName +
          "' to carry metadata name '" +
          bridge.Name +
          "'");
    }

    foreach (var endpoint in endpointColumns) {
      ValidateGeneratedBridgeProperty(
          bridge,
          tableName,
          entityType,
          endpoint.ColumnName,
          typeof(string),
          DataVaultPropertyRole.ParticipantReference);
    }

    if (traversalDepthColumnName is not null) {
      ValidateGeneratedBridgeProperty(
          bridge,
          tableName,
          entityType,
          traversalDepthColumnName,
          typeof(int),
          DataVaultPropertyRole.BridgeDepth);
    }
  }

  private static void ValidateGeneratedBridgeProperty(
      DataVaultBridgeMetadata bridge,
      string tableName,
      Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType,
      string propertyName,
      Type expectedClrType,
      DataVaultPropertyRole expectedRole) {
    var property = entityType.FindProperty(propertyName);
    if (property is null) {
      throw BridgeReadFailure(
          bridge.Name,
          "expected generated bridge property '" +
          propertyName +
          "' on table/entity '" +
          tableName +
          "'");
    }

    if (property.ClrType != expectedClrType) {
      throw BridgeReadFailure(
          bridge.Name,
          "expected generated bridge property '" +
          propertyName +
          "' on table/entity '" +
          tableName +
          "' to use CLR type '" +
          expectedClrType.FullName +
          "' but found '" +
          property.ClrType.FullName +
          "'");
    }

    var propertyRole = property.FindAnnotation(DataVaultAnnotationNames.PropertyRole)?.Value;
    if (!Equals(propertyRole, expectedRole)) {
      throw BridgeReadFailure(
          bridge.Name,
          "expected generated bridge property '" +
          propertyName +
          "' on table/entity '" +
          tableName +
          "' to carry property role '" +
          expectedRole +
          "'");
    }
  }

  private static DataVaultBridgeTraversalEndpoint ToPublicEndpoint(DataVaultBridgeEndpointRole endpointRole) {
    return endpointRole switch {
      DataVaultBridgeEndpointRole.From => DataVaultBridgeTraversalEndpoint.From,
      DataVaultBridgeEndpointRole.To => DataVaultBridgeTraversalEndpoint.To,
      DataVaultBridgeEndpointRole.Ancestor => DataVaultBridgeTraversalEndpoint.Ancestor,
      DataVaultBridgeEndpointRole.Descendant => DataVaultBridgeTraversalEndpoint.Descendant,
      _ => throw new ArgumentOutOfRangeException(nameof(endpointRole), endpointRole, "Unsupported bridge endpoint role."),
    };
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

  private static InvalidOperationException BridgeReadFailure(string bridgeName, string detail) {
    return BridgeReadFailure(bridgeName, detail, innerException: null);
  }

  private static InvalidOperationException BridgeReadFailure(
      string bridgeName,
      string detail,
      Exception? innerException) {
    return new InvalidOperationException(
        "DVault bridge read failed: bridge metadata '" +
        bridgeName +
        "' " +
        detail +
        ".",
        innerException);
  }

  private static string CreateOrdinalSignature(IEnumerable<string> values) {
    return string.Join('\u001f', values);
  }

  internal sealed record BridgeReadProjection(
      string MetadataName,
      DataVaultBridgeKind Kind,
      string TableName,
      IReadOnlyList<BridgeEndpointProjection> Endpoints,
      string FilterColumnName,
      string? TraversalDepthColumnName,
      int? MaximumDepth);

  internal sealed record BridgeEndpointProjection(
      DataVaultBridgeTraversalEndpoint Endpoint,
      string EndpointName,
      string ColumnName);

  internal sealed record BridgeProjectionReadRow(
      IReadOnlyList<string> EndpointHashKeys,
      int? TraversalDepth,
      DataVaultBridgeProjectionRow ProjectionRow);
}
