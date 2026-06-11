using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one payload column declared by a Data Vault satellite.
/// </summary>
public sealed class DataVaultSatellitePayloadMetadata {
  /// <summary>
  /// Initializes a new satellite payload metadata declaration.
  /// </summary>
  /// <param name="columnName">The provider-neutral satellite payload column name.</param>
  public DataVaultSatellitePayloadMetadata(string columnName) {
    ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
  }

  /// <summary>
  /// Gets the provider-neutral satellite payload column name.
  /// </summary>
  public string ColumnName { get; }
}
