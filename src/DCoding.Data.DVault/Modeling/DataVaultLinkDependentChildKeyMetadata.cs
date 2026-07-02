using DCoding.Data.DVault;

namespace DCoding.Data.DVault.Modeling;

/// <summary>
/// Describes one dependent child key column declared by a Data Vault link.
/// </summary>
public sealed class DataVaultLinkDependentChildKeyMetadata {
  /// <summary>
  /// Initializes a new dependent child key metadata declaration.
  /// </summary>
  /// <param name="columnName">The provider-neutral dependent child key column name.</param>
  public DataVaultLinkDependentChildKeyMetadata(string columnName) {
    ColumnName = DataVaultMetadataValidation.RequireName(columnName, nameof(columnName));
  }

  /// <summary>
  /// Gets the provider-neutral dependent child key column name.
  /// </summary>
  public string ColumnName { get; }
}
