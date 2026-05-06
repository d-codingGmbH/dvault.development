namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Provides override points for Data Vault names produced by the modeling flow.
/// </summary>
public interface IDataVaultNamingPolicy {
  /// <summary>
  /// Returns the table name for a hub.
  /// </summary>
  string GetHubTableName(DataVaultHubNameContext context);

  /// <summary>
  /// Returns the table name for a link.
  /// </summary>
  string GetLinkTableName(DataVaultLinkNameContext context);

  /// <summary>
  /// Returns the table name for a satellite.
  /// </summary>
  string GetSatelliteTableName(DataVaultSatelliteNameContext context);

  /// <summary>
  /// Returns the table name for a point-in-time table.
  /// </summary>
  string GetPointInTimeTableName(DataVaultPointInTimeNameContext context);

  /// <summary>
  /// Returns the name for a Data Vault technical column.
  /// </summary>
  string GetTechnicalColumnName(DataVaultTechnicalColumnNameContext context);

  /// <summary>
  /// Returns the name for a point-in-time table column.
  /// </summary>
  string GetPointInTimeColumnName(DataVaultPointInTimeColumnNameContext context);

  /// <summary>
  /// Returns the name for an index produced by the modeling flow.
  /// </summary>
  string GetIndexName(DataVaultIndexNameContext context);

  /// <summary>
  /// Returns the name for a constraint produced by the modeling flow.
  /// </summary>
  string GetConstraintName(DataVaultConstraintNameContext context);
}

/// <summary>
/// Describes a hub table name request.
/// </summary>
public sealed record DataVaultHubNameContext(string EntityName);

/// <summary>
/// Describes a link table name request.
/// </summary>
public sealed record DataVaultLinkNameContext(string? RelationshipName, IReadOnlyList<string> ParticipantNames);

/// <summary>
/// Describes a satellite table name request.
/// </summary>
public sealed record DataVaultSatelliteNameContext(string ParentEntityName, string SatelliteName);

/// <summary>
/// Describes a point-in-time table name request.
/// </summary>
public sealed record DataVaultPointInTimeNameContext(
    string PointInTimeName,
    string HubName,
    IReadOnlyList<string> SatelliteNames);

/// <summary>
/// Describes a technical column name request.
/// </summary>
public sealed record DataVaultTechnicalColumnNameContext(
    DataVaultTechnicalColumnKind Kind,
    string BaseName,
    string OwnerTableName);

/// <summary>
/// Describes a point-in-time table column name request.
/// </summary>
public sealed record DataVaultPointInTimeColumnNameContext(
    DataVaultPointInTimeColumnKind Kind,
    string PointInTimeName,
    string HubName,
    string? SatelliteName,
    string OwnerTableName);

/// <summary>
/// Describes an index name request.
/// </summary>
public sealed record DataVaultIndexNameContext(
    DataVaultIndexKind Kind,
    string TableName,
    IReadOnlyList<string> ColumnNames,
    bool IsUnique);

/// <summary>
/// Describes a constraint name request.
/// </summary>
public sealed record DataVaultConstraintNameContext(
    DataVaultConstraintKind Kind,
    string TableName,
    IReadOnlyList<string> ColumnNames);

/// <summary>
/// Identifies Data Vault technical column families.
/// </summary>
public enum DataVaultTechnicalColumnKind {
  /// <summary>
  /// Hash key column.
  /// </summary>
  HashKey,

  /// <summary>
  /// Hash diff column.
  /// </summary>
  HashDiff,

  /// <summary>
  /// Load timestamp column.
  /// </summary>
  LoadTimestamp,

  /// <summary>
  /// Record source column.
  /// </summary>
  RecordSource,
}

/// <summary>
/// Identifies point-in-time table column families.
/// </summary>
public enum DataVaultPointInTimeColumnKind {
  /// <summary>
  /// Hash-key reference to the PIT table's hub.
  /// </summary>
  HubHashKeyReference,

  /// <summary>
  /// PIT load timestamp used with the hub hash-key reference as the table key.
  /// </summary>
  LoadTimestamp,

  /// <summary>
  /// Snapshot load-timestamp reference for one participating satellite.
  /// </summary>
  SatelliteSnapshotLoadTimestampReference,
}

/// <summary>
/// Identifies index name families produced by the modeling flow.
/// </summary>
public enum DataVaultIndexKind {
  /// <summary>
  /// Index over business-key columns.
  /// </summary>
  BusinessKey,

  /// <summary>
  /// Index over relationship columns.
  /// </summary>
  Relationship,

  /// <summary>
  /// Index over satellite parent columns.
  /// </summary>
  SatelliteParent,

  /// <summary>
  /// Index over bridge traversal columns.
  /// </summary>
  BridgeTraversal,
}

/// <summary>
/// Identifies constraint name families produced by the modeling flow.
/// </summary>
public enum DataVaultConstraintKind {
  /// <summary>
  /// Primary key constraint.
  /// </summary>
  PrimaryKey,

  /// <summary>
  /// Foreign key constraint.
  /// </summary>
  ForeignKey,
}
