using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one business-key column declared by a Data Vault hub.
/// </summary>
public sealed class DataVaultBusinessKeyMetadata {
  /// <summary>
  /// Initializes a new business-key metadata declaration.
  /// </summary>
  /// <param name="columnName">The provider-neutral business-key column name.</param>
  public DataVaultBusinessKeyMetadata(string columnName) {
    ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
  }

  /// <summary>
  /// Gets the provider-neutral business-key column name.
  /// </summary>
  public string ColumnName { get; }
}
