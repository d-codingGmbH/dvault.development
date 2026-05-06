namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Represents Data Vault names produced by the modeling flow.
/// </summary>
public sealed class DataVaultModel {
  private DataVaultModel(IReadOnlyList<DataVaultTable> tables) {
    Tables = tables;
  }

  /// <summary>
  /// Gets the tables produced for the model.
  /// </summary>
  public IReadOnlyList<DataVaultTable> Tables { get; }

  /// <summary>
  /// Builds a Data Vault model using optional model convention options.
  /// </summary>
  public static DataVaultModel Create(
      Action<DataVaultModelBuilder> configureModel,
      Action<DataVaultModelOptions>? configureOptions = null) {
    ArgumentNullException.ThrowIfNull(configureModel);

    var options = new DataVaultModelOptions();
    configureOptions?.Invoke(options);

    var builder = new DataVaultModelBuilder(options);
    configureModel(builder);

    return builder.Build();
  }

  internal static DataVaultModel FromTables(IReadOnlyList<DataVaultTable> tables) {
    return new DataVaultModel(tables);
  }
}

/// <summary>
/// Builds Data Vault model declarations and applies naming conventions.
/// </summary>
public sealed partial class DataVaultModelBuilder {
  private readonly List<HubDeclaration> _hubs = [];
  private readonly List<LinkDeclaration> _links = [];
  private readonly List<PointInTimeDeclaration> _pointInTimeTables = [];
  private readonly DataVaultModelOptions _options;

  /// <summary>
  /// Initializes a new model builder.
  /// </summary>
  public DataVaultModelBuilder(DataVaultModelOptions? options = null) {
    _options = options ?? new DataVaultModelOptions();
  }

  /// <summary>
  /// Gets the active Data Vault conventions after UseDataVault has been applied.
  /// </summary>
  public DataVaultConventions? Conventions { get; private set; }

  /// <summary>
  /// Gets a value indicating whether Data Vault conventions are enabled for this model builder.
  /// </summary>
  public bool IsDataVaultEnabled => Conventions is not null;

  /// <summary>
  /// Adds a hub declaration to the model.
  /// </summary>
  public DataVaultHubBuilder Hub(string entityName, Action<DataVaultHubBuilder>? configure = null) {
    ArgumentException.ThrowIfNullOrWhiteSpace(entityName);

    var declaration = new HubDeclaration(entityName);
    _hubs.Add(declaration);

    var builder = new DataVaultHubBuilder(declaration);
    configure?.Invoke(builder);

    return builder;
  }

  /// <summary>
  /// Adds a link declaration whose table name is based on participant names in declaration order.
  /// </summary>
  public DataVaultModelBuilder Link(IEnumerable<string> participantNames) {
    return Link(null, participantNames);
  }

  /// <summary>
  /// Adds a link declaration to the model, using the relationship name when one is supplied.
  /// </summary>
  public DataVaultModelBuilder Link(string? relationshipName, IEnumerable<string> participantNames) {
    ArgumentNullException.ThrowIfNull(participantNames);

    var participants = participantNames.ToArray();
    if (participants.Length < 2) {
      throw new ArgumentException("A link requires at least two participants.", nameof(participantNames));
    }

    foreach (var participantName in participants) {
      ArgumentException.ThrowIfNullOrWhiteSpace(participantName);
    }

    _links.Add(new LinkDeclaration(NormalizeOptionalRelationshipName(relationshipName), participants));
    return this;
  }

  /// <summary>
  /// Adds a point-in-time table declaration for one hub and an ordered set of satellites.
  /// </summary>
  public DataVaultPointInTimeBuilder PointInTime(
      string pointInTimeName,
      string hubName,
      Action<DataVaultPointInTimeBuilder>? configure = null) {
    ArgumentException.ThrowIfNullOrWhiteSpace(pointInTimeName);
    ArgumentException.ThrowIfNullOrWhiteSpace(hubName);

    var declaration = new PointInTimeDeclaration(pointInTimeName, hubName);
    _pointInTimeTables.Add(declaration);

    var builder = new DataVaultPointInTimeBuilder(declaration);
    configure?.Invoke(builder);

    return builder;
  }

  /// <summary>
  /// Adds a point-in-time table declaration for one hub using satellite names in declaration order.
  /// </summary>
  public DataVaultModelBuilder PointInTime(
      string pointInTimeName,
      string hubName,
      IEnumerable<string> satelliteNames) {
    ArgumentNullException.ThrowIfNull(satelliteNames);

    PointInTime(pointInTimeName, hubName, pointInTime => {
      foreach (var satelliteName in satelliteNames) {
        pointInTime.Satellite(satelliteName);
      }
    });

    return this;
  }

  /// <summary>
  /// Applies the configured naming policy and returns the completed model.
  /// </summary>
  public DataVaultModel Build() {
    var namingPolicy = _options.ResolveNamingPolicy();
    ValidatePointInTimeTables();

    var tables = new List<DataVaultTable>();

    foreach (var hub in _hubs) {
      tables.Add(BuildHubTable(hub, namingPolicy));

      foreach (var satellite in hub.Satellites) {
        tables.Add(BuildSatelliteTable(hub, satellite, namingPolicy));
      }
    }

    foreach (var link in _links) {
      tables.Add(BuildLinkTable(link, namingPolicy));
    }

    foreach (var pointInTimeTable in _pointInTimeTables) {
      tables.Add(BuildPointInTimeTable(pointInTimeTable, namingPolicy));
    }

    return DataVaultModel.FromTables(tables);
  }

  internal void UseConventions(DataVaultConventions conventions) {
    ArgumentNullException.ThrowIfNull(conventions);

    Conventions = conventions;
  }

  private static DataVaultTable BuildHubTable(HubDeclaration hub, IDataVaultNamingPolicy namingPolicy) {
    var tableName = namingPolicy.GetHubTableName(new DataVaultHubNameContext(hub.EntityName));
    var hashKeyColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, hub.EntityName, tableName));
    var loadTimestampColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, hub.EntityName, tableName));
    var recordSourceColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, hub.EntityName, tableName));

    var columns = new List<DataVaultColumn>
    {
            new(hashKeyColumnName, DataVaultColumnKind.Technical),
            new(loadTimestampColumnName, DataVaultColumnKind.Technical),
            new(recordSourceColumnName, DataVaultColumnKind.Technical),
        };

    var businessKeyColumns = DefaultDataVaultNamingPolicy
        .GetColumnNames(
            hub.BusinessKeyProperties,
            [hashKeyColumnName, loadTimestampColumnName, recordSourceColumnName])
        .ToArray();

    foreach (var businessKeyColumn in businessKeyColumns) {
      columns.Add(new DataVaultColumn(businessKeyColumn, DataVaultColumnKind.BusinessKey));
    }

    var indexes = businessKeyColumns.Length == 0
        ? Array.Empty<DataVaultIndex>()
        :
        [
            new DataVaultIndex(
                    namingPolicy.GetIndexName(
                        new DataVaultIndexNameContext(DataVaultIndexKind.BusinessKey, tableName, businessKeyColumns, true)),
                    businessKeyColumns,
                    IsUnique: true),
        ];

    var constraints = new[]
    {
            new DataVaultConstraint(
                namingPolicy.GetConstraintName(
                    new DataVaultConstraintNameContext(DataVaultConstraintKind.PrimaryKey, tableName, [hashKeyColumnName])),
                DataVaultConstraintKind.PrimaryKey,
                [hashKeyColumnName]),
        };

    return new DataVaultTable(tableName, DataVaultTableKind.Hub, columns, indexes, constraints);
  }

  private static DataVaultTable BuildSatelliteTable(
      HubDeclaration hub,
      SatelliteDeclaration satellite,
      IDataVaultNamingPolicy namingPolicy) {
    var tableName = namingPolicy.GetSatelliteTableName(
        new DataVaultSatelliteNameContext(hub.EntityName, satellite.SatelliteName));
    var parentHashKeyColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, hub.EntityName, tableName));
    var hashDiffColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashDiff, satellite.SatelliteName, tableName));
    var loadTimestampColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, satellite.SatelliteName, tableName));
    var recordSourceColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, satellite.SatelliteName, tableName));

    var columns = new List<DataVaultColumn>
    {
            new(parentHashKeyColumnName, DataVaultColumnKind.Technical),
            new(hashDiffColumnName, DataVaultColumnKind.Technical),
            new(loadTimestampColumnName, DataVaultColumnKind.Technical),
            new(recordSourceColumnName, DataVaultColumnKind.Technical),
        };

    var payloadColumns = DefaultDataVaultNamingPolicy.GetColumnNames(
        satellite.PayloadProperties,
        [parentHashKeyColumnName, hashDiffColumnName, loadTimestampColumnName, recordSourceColumnName]);

    foreach (var payloadColumn in payloadColumns) {
      columns.Add(new DataVaultColumn(payloadColumn, DataVaultColumnKind.Payload));
    }

    var indexes = new[]
    {
            new DataVaultIndex(
                namingPolicy.GetIndexName(
                    new DataVaultIndexNameContext(
                        DataVaultIndexKind.SatelliteParent,
                        tableName,
                        [parentHashKeyColumnName],
                        IsUnique: false)),
                [parentHashKeyColumnName],
                IsUnique: false),
        };

    var constraints = new[]
    {
            new DataVaultConstraint(
                namingPolicy.GetConstraintName(
                    new DataVaultConstraintNameContext(
                        DataVaultConstraintKind.PrimaryKey,
                        tableName,
                        [parentHashKeyColumnName, loadTimestampColumnName])),
                DataVaultConstraintKind.PrimaryKey,
                [parentHashKeyColumnName, loadTimestampColumnName]),
        };

    return new DataVaultTable(tableName, DataVaultTableKind.Satellite, columns, indexes, constraints);
  }

  private static DataVaultTable BuildLinkTable(LinkDeclaration link, IDataVaultNamingPolicy namingPolicy) {
    var tableName = namingPolicy.GetLinkTableName(
        new DataVaultLinkNameContext(link.RelationshipName, link.ParticipantNames));
    var linkHashKeyBaseName = GetLinkHashKeyBaseName(link);
    var linkHashKeyColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, linkHashKeyBaseName, tableName));
    var loadTimestampColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.LoadTimestamp, linkHashKeyBaseName, tableName));
    var recordSourceColumnName = namingPolicy.GetTechnicalColumnName(
        new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.RecordSource, linkHashKeyBaseName, tableName));

    var participantHashKeyColumnNames = link.ParticipantNames
        .Select(participantName => namingPolicy.GetTechnicalColumnName(
            new DataVaultTechnicalColumnNameContext(DataVaultTechnicalColumnKind.HashKey, participantName, tableName)))
        .ToArray();

    var columns = new List<DataVaultColumn>
    {
            new(linkHashKeyColumnName, DataVaultColumnKind.Technical),
            new(loadTimestampColumnName, DataVaultColumnKind.Technical),
            new(recordSourceColumnName, DataVaultColumnKind.Technical),
        };
    columns.AddRange(participantHashKeyColumnNames.Select(columnName => new DataVaultColumn(columnName, DataVaultColumnKind.Technical)));

    var indexes = new[]
    {
            new DataVaultIndex(
                namingPolicy.GetIndexName(
                    new DataVaultIndexNameContext(
                        DataVaultIndexKind.Relationship,
                        tableName,
                        participantHashKeyColumnNames,
                        IsUnique: false)),
                participantHashKeyColumnNames,
                IsUnique: false),
        };

    var constraints = new[]
    {
            new DataVaultConstraint(
                namingPolicy.GetConstraintName(
                    new DataVaultConstraintNameContext(DataVaultConstraintKind.PrimaryKey, tableName, [linkHashKeyColumnName])),
                DataVaultConstraintKind.PrimaryKey,
                [linkHashKeyColumnName]),
        };

    return new DataVaultTable(tableName, DataVaultTableKind.Link, columns, indexes, constraints);
  }

  private static DataVaultTable BuildPointInTimeTable(
      PointInTimeDeclaration pointInTimeTable,
      IDataVaultNamingPolicy namingPolicy) {
    var tableName = namingPolicy.GetPointInTimeTableName(
        new DataVaultPointInTimeNameContext(
            pointInTimeTable.PointInTimeName,
            pointInTimeTable.HubName,
            pointInTimeTable.SatelliteNames));
    var hubHashKeyColumnName = namingPolicy.GetPointInTimeColumnName(
        new DataVaultPointInTimeColumnNameContext(
            DataVaultPointInTimeColumnKind.HubHashKeyReference,
            pointInTimeTable.PointInTimeName,
            pointInTimeTable.HubName,
            null,
            tableName));
    var loadTimestampColumnName = namingPolicy.GetPointInTimeColumnName(
        new DataVaultPointInTimeColumnNameContext(
            DataVaultPointInTimeColumnKind.LoadTimestamp,
            pointInTimeTable.PointInTimeName,
            pointInTimeTable.HubName,
            null,
            tableName));

    var columns = new List<DataVaultColumn>
    {
            new(hubHashKeyColumnName, DataVaultColumnKind.PointInTime),
            new(loadTimestampColumnName, DataVaultColumnKind.PointInTime),
        };
    var pointInTimeFields = new List<DataVaultPointInTimeField>
    {
            new(
                hubHashKeyColumnName,
                DataVaultPointInTimeColumnKind.HubHashKeyReference,
                null,
                0),
            new(
                loadTimestampColumnName,
                DataVaultPointInTimeColumnKind.LoadTimestamp,
                null,
                1),
        };

    foreach (var satelliteName in pointInTimeTable.SatelliteNames) {
      var satelliteLoadTimestampColumnName = namingPolicy.GetPointInTimeColumnName(
          new DataVaultPointInTimeColumnNameContext(
              DataVaultPointInTimeColumnKind.SatelliteSnapshotLoadTimestampReference,
              pointInTimeTable.PointInTimeName,
              pointInTimeTable.HubName,
              satelliteName,
              tableName));

      columns.Add(new DataVaultColumn(satelliteLoadTimestampColumnName, DataVaultColumnKind.PointInTime));
      pointInTimeFields.Add(new DataVaultPointInTimeField(
          satelliteLoadTimestampColumnName,
          DataVaultPointInTimeColumnKind.SatelliteSnapshotLoadTimestampReference,
          satelliteName,
          null));
    }

    var constraints = new[]
    {
            new DataVaultConstraint(
                namingPolicy.GetConstraintName(
                    new DataVaultConstraintNameContext(
                        DataVaultConstraintKind.PrimaryKey,
                        tableName,
                        [hubHashKeyColumnName, loadTimestampColumnName])),
                DataVaultConstraintKind.PrimaryKey,
                [hubHashKeyColumnName, loadTimestampColumnName]),
        };

    return new DataVaultTable(
        tableName,
        DataVaultTableKind.PointInTime,
        columns,
        Array.Empty<DataVaultIndex>(),
        constraints,
        pointInTimeFields);
  }

  private void ValidatePointInTimeTables() {
    var hubNames = new HashSet<string>(_hubs.Select(hub => hub.EntityName), StringComparer.Ordinal);
    var satellitesByName = _hubs
        .SelectMany(hub => hub.Satellites.Select(satellite => new SatelliteResolution(hub.EntityName, satellite.SatelliteName)))
        .GroupBy(satellite => satellite.SatelliteName, StringComparer.Ordinal)
        .ToDictionary(group => group.Key, group => group.ToArray(), StringComparer.Ordinal);

    foreach (var pointInTimeTable in _pointInTimeTables) {
      if (!hubNames.Contains(pointInTimeTable.HubName)) {
        throw PointInTimeValidationException(
            pointInTimeTable,
            "references missing hub '" + pointInTimeTable.HubName + "'.");
      }

      if (pointInTimeTable.SatelliteNames.Count == 0) {
        throw PointInTimeValidationException(pointInTimeTable, "requires at least one satellite reference.");
      }

      var satelliteNames = new HashSet<string>(StringComparer.Ordinal);
      foreach (var satelliteName in pointInTimeTable.SatelliteNames) {
        if (!satelliteNames.Add(satelliteName)) {
          throw PointInTimeValidationException(
              pointInTimeTable,
              "references satellite '" + satelliteName + "' more than once.");
        }

        if (!satellitesByName.TryGetValue(satelliteName, out var satellites)) {
          throw PointInTimeValidationException(
              pointInTimeTable,
              "references missing satellite '" + satelliteName + "'.");
        }

        if (!satellites.Any(satellite => string.Equals(satellite.HubName, pointInTimeTable.HubName, StringComparison.Ordinal))) {
          throw PointInTimeValidationException(
              pointInTimeTable,
              "references satellite '" +
              satelliteName +
              "' that does not belong to hub '" +
              pointInTimeTable.HubName +
              "'.");
        }
      }
    }
  }

  private static string? NormalizeOptionalRelationshipName(string? relationshipName) {
    return string.IsNullOrWhiteSpace(relationshipName) ? null : relationshipName;
  }

  private static string GetLinkHashKeyBaseName(LinkDeclaration link) {
    return link.RelationshipName ?? string.Join(" ", link.ParticipantNames);
  }

  internal sealed class HubDeclaration(string entityName) {
    public string EntityName { get; } = entityName;

    public List<string> BusinessKeyProperties { get; } = [];

    public List<SatelliteDeclaration> Satellites { get; } = [];
  }

  internal sealed class SatelliteDeclaration(string satelliteName) {
    public string SatelliteName { get; } = satelliteName;

    public List<string> PayloadProperties { get; } = [];
  }

  internal sealed class LinkDeclaration(string? relationshipName, IReadOnlyList<string> participantNames) {
    public string? RelationshipName { get; } = relationshipName;

    public IReadOnlyList<string> ParticipantNames { get; } = participantNames;
  }

  internal sealed class PointInTimeDeclaration(string pointInTimeName, string hubName) {
    public string PointInTimeName { get; } = pointInTimeName;

    public string HubName { get; } = hubName;

    public List<string> SatelliteNames { get; } = [];
  }

  private sealed record SatelliteResolution(string HubName, string SatelliteName);

  private static InvalidOperationException PointInTimeValidationException(
      PointInTimeDeclaration pointInTimeTable,
      string message) {
    return new InvalidOperationException(
        "Point-in-time table '" + pointInTimeTable.PointInTimeName + "' " + message);
  }
}

/// <summary>
/// Builds a hub declaration.
/// </summary>
public sealed class DataVaultHubBuilder {
  private readonly DataVaultModelBuilder.HubDeclaration _declaration;

  internal DataVaultHubBuilder(DataVaultModelBuilder.HubDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds a business-key property to the hub.
  /// </summary>
  public DataVaultHubBuilder BusinessKey(string propertyName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    _declaration.BusinessKeyProperties.Add(propertyName);

    return this;
  }

  /// <summary>
  /// Adds a satellite declaration to the hub.
  /// </summary>
  public DataVaultHubBuilder Satellite(string satelliteName, Action<DataVaultSatelliteBuilder>? configure = null) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);

    var declaration = new DataVaultModelBuilder.SatelliteDeclaration(satelliteName);
    _declaration.Satellites.Add(declaration);

    var builder = new DataVaultSatelliteBuilder(declaration);
    configure?.Invoke(builder);

    return this;
  }
}

/// <summary>
/// Builds a satellite declaration.
/// </summary>
public sealed class DataVaultSatelliteBuilder {
  private readonly DataVaultModelBuilder.SatelliteDeclaration _declaration;

  internal DataVaultSatelliteBuilder(DataVaultModelBuilder.SatelliteDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds a payload property to the satellite.
  /// </summary>
  public DataVaultSatelliteBuilder Payload(string propertyName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
    _declaration.PayloadProperties.Add(propertyName);

    return this;
  }
}

/// <summary>
/// Builds a point-in-time table declaration.
/// </summary>
public sealed class DataVaultPointInTimeBuilder {
  private readonly DataVaultModelBuilder.PointInTimeDeclaration _declaration;

  internal DataVaultPointInTimeBuilder(DataVaultModelBuilder.PointInTimeDeclaration declaration) {
    _declaration = declaration;
  }

  /// <summary>
  /// Adds a satellite reference to the point-in-time table in declaration order.
  /// </summary>
  public DataVaultPointInTimeBuilder Satellite(string satelliteName) {
    ArgumentException.ThrowIfNullOrWhiteSpace(satelliteName);
    _declaration.SatelliteNames.Add(satelliteName);

    return this;
  }
}

/// <summary>
/// Describes one produced Data Vault table.
/// </summary>
public sealed class DataVaultTable {
  /// <summary>
  /// Initializes a new produced table.
  /// </summary>
  public DataVaultTable(
      string name,
      DataVaultTableKind kind,
      IEnumerable<DataVaultColumn> columns,
      IEnumerable<DataVaultIndex> indexes,
      IEnumerable<DataVaultConstraint> constraints)
      : this(name, kind, columns, indexes, constraints, Array.Empty<DataVaultPointInTimeField>()) {
  }

  /// <summary>
  /// Initializes a new produced table with point-in-time field descriptors.
  /// </summary>
  public DataVaultTable(
      string name,
      DataVaultTableKind kind,
      IEnumerable<DataVaultColumn> columns,
      IEnumerable<DataVaultIndex> indexes,
      IEnumerable<DataVaultConstraint> constraints,
      IEnumerable<DataVaultPointInTimeField> pointInTimeFields) {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    ArgumentNullException.ThrowIfNull(columns);
    ArgumentNullException.ThrowIfNull(indexes);
    ArgumentNullException.ThrowIfNull(constraints);
    ArgumentNullException.ThrowIfNull(pointInTimeFields);

    Name = name;
    Kind = kind;
    Columns = columns.ToArray();
    Indexes = indexes.ToArray();
    Constraints = constraints.ToArray();
    PointInTimeFields = pointInTimeFields.ToArray();
  }

  /// <summary>
  /// Gets the produced table name.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the produced table kind.
  /// </summary>
  public DataVaultTableKind Kind { get; }

  /// <summary>
  /// Gets the produced table columns.
  /// </summary>
  public IReadOnlyList<DataVaultColumn> Columns { get; }

  /// <summary>
  /// Gets the produced table indexes.
  /// </summary>
  public IReadOnlyList<DataVaultIndex> Indexes { get; }

  /// <summary>
  /// Gets the produced table constraints.
  /// </summary>
  public IReadOnlyList<DataVaultConstraint> Constraints { get; }

  /// <summary>
  /// Gets provider-neutral point-in-time field descriptors for PIT tables, or an empty list for other table kinds.
  /// </summary>
  public IReadOnlyList<DataVaultPointInTimeField> PointInTimeFields { get; }
}

/// <summary>
/// Describes one produced Data Vault column.
/// </summary>
public sealed record DataVaultColumn(string Name, DataVaultColumnKind Kind);

/// <summary>
/// Describes one produced point-in-time table field and its key participation.
/// </summary>
public sealed record DataVaultPointInTimeField(
    string Name,
    DataVaultPointInTimeColumnKind Kind,
    string? SatelliteName,
    int? KeyOrdinal);

/// <summary>
/// Describes one produced Data Vault index.
/// </summary>
public sealed record DataVaultIndex(string Name, IReadOnlyList<string> ColumnNames, bool IsUnique);

/// <summary>
/// Describes one produced Data Vault constraint.
/// </summary>
public sealed record DataVaultConstraint(
    string Name,
    DataVaultConstraintKind Kind,
    IReadOnlyList<string> ColumnNames);

/// <summary>
/// Identifies produced Data Vault table kinds.
/// </summary>
public enum DataVaultTableKind {
  /// <summary>
  /// Hub table.
  /// </summary>
  Hub,

  /// <summary>
  /// Link table.
  /// </summary>
  Link,

  /// <summary>
  /// Satellite table.
  /// </summary>
  Satellite,

  /// <summary>
  /// Point-in-time table.
  /// </summary>
  PointInTime,

  /// <summary>
  /// PIT table translated from metadata declarations.
  /// </summary>
  Pit,

  /// <summary>
  /// Bridge table.
  /// </summary>
  Bridge,
}

/// <summary>
/// Identifies produced Data Vault column kinds.
/// </summary>
public enum DataVaultColumnKind {
  /// <summary>
  /// Data Vault technical column.
  /// </summary>
  Technical,

  /// <summary>
  /// Business-key column.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Satellite payload column.
  /// </summary>
  Payload,

  /// <summary>
  /// Point-in-time table column.
  /// </summary>
  PointInTime,
}
