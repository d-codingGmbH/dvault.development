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
